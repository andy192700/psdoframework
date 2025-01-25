using AutoFixture.Xunit2;
using DoFramework.Domain;
using DoFramework.Logging;
using DoFramework.Processing;
using FluentAssertions;
using Moq;

namespace DoFrameworkTests.Processing;

public class ProcessExecutorTests
{
    [Theory]
    [InlineAutoMoqData]
    public void ProcessExecutor_FailedChecksNotRun(
        [Frozen] Mock<IContext> context,
        [Frozen] Mock<IProcessInstanceRunner> processInstanceRunner,
        string name
    )
    {
        // Arrange
        context.Setup(x => x.Session).Returns(new Session
        {
            ProcessReports = [],
            ProcessCount = 0,
            CurrentProcessName = name
        });

        var processBuilder = new Mock<IProcessBuilder>();

        var logger = new Mock<ILogger>();

        var failedReportChecker = new Mock<IFailedReportChecker>();

        failedReportChecker.Setup(x => x.Check()).Returns(true);

        var sut = new ProcessExecutor(
            context.Object,
            processInstanceRunner.Object,
            processBuilder.Object,
            logger.Object,
            failedReportChecker.Object);

        var processDescriptor = new ProcessDescriptor
        {
            Name = name,
            
        };

        // Act
        var result = sut.Execute(processDescriptor);

        // Assert
        result.Should().NotBeNull();
        result.ProcessResult.Should().Be(ProcessResult.NotRun);

        processBuilder.Verify(x => x.Build(It.IsAny<ProcessDescriptor>()), Times.Once);

        failedReportChecker.Verify(x => x.Check(), Times.Once);

        logger.Verify(x => x.LogWarning($"Process not run: {name}"), Times.Once);
    }

    [Theory]
    [InlineAutoMoqData]
    public void ProcessExecutor_FailedChecksFailed(
        [Frozen] Mock<IContext> context,
        [Frozen] Mock<IProcessInstanceRunner> processInstanceRunner,
        string name
    )
    {
        // Arrange
        context.Setup(x => x.Session).Returns(new Session
        {
            ProcessReports = [],
            ProcessCount = 0
        });

        var processBuilder = new Mock<IProcessBuilder>();

        var logger = new Mock<ILogger>();

        var failedReportChecker = new Mock<IFailedReportChecker>();

        failedReportChecker.Setup(x => x.Check()).Returns(true);

        var sut = new ProcessExecutor(
            context.Object,
            processInstanceRunner.Object,
            processBuilder.Object,
            logger.Object,
            failedReportChecker.Object);

        var processDescriptor = new ProcessDescriptor
        {
            Name = name,

        };

        // Act
        var result = sut.Execute(processDescriptor);

        // Assert
        result.Should().NotBeNull();
        result.ProcessResult.Should().Be(ProcessResult.Failed);

        processBuilder.Verify(x => x.Build(It.IsAny<ProcessDescriptor>()), Times.Once);

        failedReportChecker.Verify(x => x.Check(), Times.Exactly(2));

        logger.Verify(x => x.LogWarning($"Process not run: {name}"), Times.Once);
    }


    [Theory]
    [InlineAutoMoqData]
    public void ProcessExecutor_ValidationFailsAndThrows(
        [Frozen] Mock<IContext> context,
        [Frozen] Mock<IProcessInstanceRunner> processInstanceRunner,
        string name,
        string errorMessage
    )
    {
        // Arrange
        context.Setup(x => x.Session).Returns(new Session
        {
            ProcessReports = [],
            ProcessCount = 0,
            CurrentProcessName = name
        });

        var processBuilder = new Mock<IProcessBuilder>();

        processBuilder.Setup(x => x.Build(It.IsAny<ProcessDescriptor>())).Returns(new TestThrowingValidationProcess(errorMessage));

        var logger = new Mock<ILogger>();

        var failedReportChecker = new Mock<IFailedReportChecker>();

        failedReportChecker.Setup(x => x.Check()).Returns(false);

        var sut = new ProcessExecutor(
            context.Object,
            processInstanceRunner.Object,
            processBuilder.Object,
            logger.Object,
            failedReportChecker.Object);

        var processDescriptor = new ProcessDescriptor
        {
            Name = name,

        };

        // Act
        var result = sut.Execute(processDescriptor);

        // Assert
        result.Should().NotBeNull();
        result.ProcessResult.Should().Be(ProcessResult.Invalidated);

        processBuilder.Verify(x => x.Build(It.IsAny<ProcessDescriptor>()), Times.Once);

        failedReportChecker.Verify(x => x.Check(), Times.Once);

        logger.Verify(x => x.LogWarning(It.IsAny<string>()), Times.Never);

        logger.Verify(x => x.LogFatal($"Whilst validating {name}, an error occurred: {errorMessage}"), Times.Once);

        logger.Verify(x => x.LogFatal($"Process failed: {name}"), Times.Once);

        logger.Verify(x => x.LogFatal($"Process invalidated: {name}"), Times.Once);
    }

    [Theory]
    [InlineAutoMoqData]
    public void ProcessExecutor_PassedReportCheck(
        [Frozen] Mock<IContext> context,
        string name
    )
    {
        // Arrange
        context.Setup(x => x.Session).Returns(new Session
        {
            ProcessReports = [],
            ProcessCount = 0,
            CurrentProcessName = name
        });

        var processBuilder = new Mock<IProcessBuilder>();

        processBuilder.Setup(x => x.Build(It.IsAny<ProcessDescriptor>())).Returns(new TestProcess());

        var logger = new Mock<ILogger>();

        var failedReportChecker = new Mock<IFailedReportChecker>();

        failedReportChecker.Setup(x => x.Check()).Returns(false);

        var sut = new ProcessExecutor(
            context.Object,
            new CompletingProcessInstanceRunner(),
            processBuilder.Object,
            logger.Object,
            failedReportChecker.Object);

        var processDescriptor = new ProcessDescriptor
        {
            Name = name
        };

        // Act
        var result = sut.Execute(processDescriptor);

        // Assert
        result.Should().NotBeNull();
        result.ProcessResult.Should().Be(ProcessResult.Completed);

        processBuilder.Verify(x => x.Build(It.IsAny<ProcessDescriptor>()), Times.Once);

        failedReportChecker.Verify(x => x.Check(), Times.Once);

        logger.Verify(x => x.LogWarning(It.IsAny<string>()), Times.Never);
    }
}

public class TestProcess : IProcess
{
    public void Run() { }

    public bool Validate()
    {
        return true;
    }
}

public class TestThrowingValidationProcess(string errorMessage) : IProcess
{
    private string ErrorMessage { get; set; } = errorMessage;

    public void Run() { }

    public bool Validate()
    {
        throw new Exception(ErrorMessage);
    }
}

public class CompletingProcessInstanceRunner : IProcessInstanceRunner
{
    public void RunInstance(ProcessReport report)
    {
        report.ProcessResult = ProcessResult.Completed;
    }
}