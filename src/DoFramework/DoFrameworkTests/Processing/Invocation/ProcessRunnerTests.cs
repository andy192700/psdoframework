using AutoFixture.Xunit2;
using DoFramework.Data;
using DoFramework.Domain;
using DoFramework.Logging;
using DoFramework.Processing;
using FluentAssertions;
using Moq;

namespace DoFrameworkTests.Processing;

public class ProcessRunnerTests
{
    [Theory]
    [InlineAutoMoqData]
    public void ProcessRunner_RunsProcess(
        [Frozen] Mock<IContext> context,
        [Frozen] Mock<IProcessExecutor> executor,
        string path,
        string name)
    {
        // Arrange
        context.Setup(x => x.Session).Returns(new Session
        {
            ProcessReports = [],
            ProcessCount = 0
        });

        var logger = new Mock<ILogger>();

        var processResolver = new Mock<IResolver<ProcessDescriptor>>();

        var processDescriptor = new ProcessDescriptor
        {
            Name = name
        };

        var resolutionResult = new ResolutionResult<ProcessDescriptor>(true, path, processDescriptor);

        processResolver.Setup(x => x.Resolve(It.IsAny<string>())).Returns(resolutionResult);

        var report = new ProcessReport
        {
            Descriptor = processDescriptor
        };

        executor.Setup(x => x.Execute(It.IsAny<ProcessDescriptor>())).Returns(report);

        var sut = new ProcessRunner(
            context.Object,
            processResolver.Object,
            executor.Object,
            logger.Object);

        // Act
        sut.Run(name);

        // Assert
        executor.Verify(x => x.Execute(It.IsAny<ProcessDescriptor>()), Times.Once);

        logger.Verify(x => x.LogFatal(It.IsAny<string>()), Times.Never);

        report.Name.Should().Be(name);

        context.Object.Session.ProcessReports.Should().Contain(report);
        context.Object.Session.ProcessReports.Should().HaveCount(1);
        context.Object.Session.ProcessCount.Should().Be(1);
        context.Object.Session.CurrentProcessName.Should().Be(name);
    }

    [Theory]
    [InlineAutoMoqData]
    public void ProcessRunner_CannotResolveProcess(
        [Frozen] Mock<IContext> context,
        [Frozen] Mock<IProcessExecutor> executor,
        string path,
        string name)
    {
        // Arrange
        context.Setup(x => x.Session).Returns(new Session
        {
            ProcessReports = [],
            ProcessCount = 0
        });

        var logger = new Mock<ILogger>();

        var processResolver = new Mock<IResolver<ProcessDescriptor>>();

        var resolutionResult = new ResolutionResult<ProcessDescriptor>(true, path, null!);

        processResolver.Setup(x => x.Resolve(It.IsAny<string>())).Returns(resolutionResult);

        var sut = new ProcessRunner(
            context.Object,
            processResolver.Object,
            executor.Object,
            logger.Object);

        // Act
        sut.Run(name);

        // Assert
        var report = context.Object.Session.ProcessReports[0];

        executor.Verify(x => x.Execute(It.IsAny<ProcessDescriptor>()), Times.Never);

        logger.Verify(x => x.LogFatal($"Process not found: {name}"), Times.Once);

        report.Name.Should().Be(name);

        context.Object.Session.ProcessReports.Should().HaveCount(1);
        context.Object.Session.ProcessCount.Should().Be(1);
        context.Object.Session.CurrentProcessName.Should().Be(name);
    }
}