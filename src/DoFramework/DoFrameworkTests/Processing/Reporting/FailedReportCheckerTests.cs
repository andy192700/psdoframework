using AutoFixture.Xunit2;
using DoFramework.Domain;
using DoFramework.Processing;
using FluentAssertions;
using Moq;

namespace DoFrameworkTests.Processing;

public class FailedReportCheckerTests
{
    [Theory]
    [InlineAutoMoqData]
    public void FailedReportChecker_NoReportsPasses([Frozen] Mock<IContext> contextMock)
    {
        // Arrange
        var session = new Session();

        contextMock.Setup(x => x.Session).Returns(session);

        var sut = new FailedReportChecker(contextMock.Object);

        // Act
        var result = sut.Check();

        // Assert
        result.Should().BeFalse();
    }

    [Theory]
    [InlineAutoMoqData(ProcessResult.Completed, false)]
    [InlineAutoMoqData(ProcessResult.NotRun, false)]
    [InlineAutoMoqData(ProcessResult.Invalidated, true)]
    [InlineAutoMoqData(ProcessResult.NotFound, true)]
    [InlineAutoMoqData(ProcessResult.Failed, true)]
    public void FailedReportChecker_CorrectResultIsObserved(
        ProcessResult processResult,
        bool isFail,
        [Frozen] Mock<IContext> contextMock)
    {
        // Arrange
        var report = new ProcessReport
        {
            ProcessResult = processResult
        };

        var session = new Session();

        session.ProcessReports.Add(report);

        contextMock.Setup(x => x.Session).Returns(session);

        var sut = new FailedReportChecker(contextMock.Object);

        // Act
        var result = sut.Check();

        // Assert
        result.Should().Be(isFail);
    }
}