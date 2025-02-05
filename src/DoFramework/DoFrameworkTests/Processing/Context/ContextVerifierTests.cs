using DoFramework.Domain;
using DoFramework.Processing;
using FluentAssertions;

namespace DoFrameworkTests.Processing;

public class ContextVerifierTests
{
    [Fact]
    public void ContextVerifier_IsValidByDefault()
    {
        // Arrange
        var context = new Context(new Session());
        var sut = new ContextVerifier(context);

        // Act
        var result = sut.Verify();

        // Assert
        result.Should().BeTrue();
    }

    [Theory]
    [InlineAutoMoqData("")]
    [InlineAutoMoqData("abc")]
    public void ContextVerifier_IsInValidComposedBy(string composedBy, string actuallyComposedBy)
    {
        // Arrange
        var context = new Context(new Session());

        context.Session.ComposedBy = actuallyComposedBy;

        var sut = new ContextVerifier(context);

        // Act
        var result = sut.
            ComposedBy(composedBy).
            Verify();

        // Assert
        result.Should().BeFalse();
    }

    [Theory]
    [InlineAutoMoqData]
    public void ContextVerifier_IsValidComposedBy(string composedBy)
    {
        // Arrange
        var context = new Context(new Session());

        context.Session.ComposedBy = composedBy;

        var sut = new ContextVerifier(context);

        // Act
        var result = sut.
            ComposedBy(composedBy).
            Verify();

        // Assert
        result.Should().BeTrue();
    }

    [Theory]
    [InlineAutoMoqData]
    public void ContextVerifier_IsInValidConfirmKey(string key)
    {
        // Arrange
        var context = new Context(new Session());

        var sut = new ContextVerifier(context);

        // Act
        var result = sut.
            ConfirmKey(key).
            Verify();

        // Assert
        result.Should().BeFalse();
    }

    [Theory]
    [InlineAutoMoqData]
    public void ContextVerifier_IsValidConfirmKey(string key, object value)
    {
        // Arrange
        var context = new Context(new Session());

        context.AddOrUpdate(key, value);

        var sut = new ContextVerifier(context);

        // Act
        var result = sut.
            ConfirmKey(key).
            Verify();

        // Assert
        result.Should().BeTrue();
    }

    [Theory]
    [InlineAutoMoqData]
    public void ContextVerifier_IsInValidProcessSucceeded(string processName)
    {
        // Arrange
        var context = new Context(new Session());

        var sut = new ContextVerifier(context);

        // Act
        var result = sut.
            ProcessSucceeded(processName).
            Verify();

        // Assert
        result.Should().BeFalse();
    }

    [Theory]
    [InlineAutoMoqData]
    public void ContextVerifier_IsValidProcessSucceeded(string processName)
    {
        // Arrange
        var context = new Context(new Session());

        context.Session.ProcessReports.Add(new ProcessReport
        {
            ProcessResult = ProcessResult.Completed,
            Name = processName
        });

        var sut = new ContextVerifier(context);

        // Act
        var result = sut.
            ProcessSucceeded(processName).
            Verify();

        // Assert
        result.Should().BeTrue();
    }

    [Theory]
    [InlineAutoMoqData(ProcessResult.Failed)]
    [InlineAutoMoqData(ProcessResult.NotRun)]
    [InlineAutoMoqData(ProcessResult.Invalidated)]
    [InlineAutoMoqData(ProcessResult.NotFound)]
    public void ContextVerifier_IsValidProcessNotSucceeded(ProcessResult processResult, string processName)
    {
        // Arrange
        var context = new Context(new Session());

        context.Session.ProcessReports.Add(new ProcessReport
        {
            ProcessResult = processResult,
            Name = processName
        });

        var sut = new ContextVerifier(context);

        // Act
        var result = sut.
            ProcessSucceeded(processName).
            Verify();

        // Assert
        result.Should().BeFalse();
    }

    [Theory]
    [InlineAutoMoqData]
    public void ContextVerifier_IsValidMultipleRequirements(
        string composedBy,
        string processName1,
        string processName2,
        string key1,
        string key2,
        object value1,
        object value2)
    {
        // Arrange
        var context = new Context(new Session());

        context.Session.ComposedBy = composedBy;

        context.AddOrUpdate(key1, value1);
        context.AddOrUpdate(key2, value2);

        context.Session.ProcessReports.Add(new ProcessReport
        {
            ProcessResult = ProcessResult.Completed,
            Name = processName1
        });

        context.Session.ProcessReports.Add(new ProcessReport
        {
            ProcessResult = ProcessResult.Completed,
            Name = processName2
        });

        var sut = new ContextVerifier(context);

        // Act
        var result = sut.
            ComposedBy(composedBy).
            ConfirmKey(key1).
            ConfirmKey(key2).
            ProcessSucceeded(processName1).
            ProcessSucceeded(processName2).
            Verify();

        // Assert
        result.Should().BeTrue();
    }
}
