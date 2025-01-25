using AutoFixture.Xunit2;
using DoFramework.Domain;
using DoFramework.Environment;
using DoFramework.FileSystem;
using DoFramework.Logging;
using DoFramework.Processing;
using FluentAssertions;
using Moq;

namespace DoFrameworkTests.Processing;

public class ProcessInstanceRunnerTests
{
    [Theory]
    [InlineAutoMoqData]
    public void ProcessInstanceRunner_RunsProcess(
        [Frozen] Mock<IEnvironment> environment,
        [Frozen] Mock<ILogger> logger,
        string proessName)
    {
        // Arrange
        var process = new Mock<IProcess>();
        var setLocation = new Mock<ISetProcessLocation>();

        var sut = new ProcessInstanceRunner(environment.Object, setLocation.Object, logger.Object);

        var report = new ProcessReport
        {
            Descriptor = new ProcessDescriptor()
            {
                Name = proessName,
                Instance = process.Object
            }
        };

        // Act
        sut.RunInstance(report);

        // Assert
        report.ProcessResult.Should().Be(ProcessResult.Completed);
        report.StartTime.Should().NotBeNull();
        report.EndTime.Should().NotBeNull();
        report.EndTime.Should().BeAfter(report.StartTime!.Value);

        logger.Verify(x => x.LogFatal(It.IsAny<string>()), Times.Never);
        process.Verify(x => x.Run(), Times.Once);
        setLocation.Verify(x => x.Set(It.IsAny<string>()), Times.Once);
    }

    [Theory]
    [InlineAutoMoqData]
    public void ProcessInstanceRunner_HandlesProcessException(
        [Frozen] Mock<IEnvironment> environment,
        [Frozen] Mock<ILogger> logger,
        string proessName,
        Exception exception)
    {
        // Arrange
        var process = new Mock<IProcess>();
        var setLocation = new Mock<ISetProcessLocation>();

        process.Setup(x => x.Run()).Throws(exception);

        var sut = new ProcessInstanceRunner(environment.Object, setLocation.Object, logger.Object);

        var report = new ProcessReport
        {
            Descriptor = new ProcessDescriptor()
            {
                Name = proessName,
                Instance = process.Object
            }
        };

        // Act
        sut.RunInstance(report);

        // Assert
        report.ProcessResult.Should().Be(ProcessResult.Failed);
        report.StartTime.Should().NotBeNull();
        report.EndTime.Should().NotBeNull();
        report.EndTime.Should().BeAfter(report.StartTime!.Value);

        process.Verify(x => x.Run(), Times.Once);
        logger.Verify(x => x.LogFatal($"Whilst executing {report.Descriptor!.Name}, an error occurred: {exception.Message}"), Times.Once);
        logger.Verify(x => x.LogFatal($"Process failed: {report.Descriptor.Name}"), Times.Once);
        setLocation.Verify(x => x.Set(It.IsAny<string>()), Times.Once);
    }
}