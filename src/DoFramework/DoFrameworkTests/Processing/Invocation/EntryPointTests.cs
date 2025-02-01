using AutoFixture.Xunit2;
using DoFramework.CLI;
using DoFramework.Logging;
using DoFramework.Processing;
using DoFramework.Validators;
using FluentAssertions;
using Moq;

namespace DoFrameworkTests.Processing;

public class EntryPointTests
{
    [Theory]
    [InlineAutoMoqData]
    public void EntryPoint_DispatchesProcessAndReturnsContext(
        [Frozen] Mock<IProcessRunner> runner,
        [Frozen] Mock<IDisplayReports> displayReports,
        CLIFunctionParameters parameters,
        string name)
    {
        // Arrange
        var logger = new Logger(new ConsoleWrapper());
        var validator = new ProcessingRequestValidator();
        var context = new TestContext();
        context.Session.CurrentProcessName = name;
        parameters.Parameters = [];

        parameters.Parameters!.Add("name", name);

        parameters.Parameters!.Add("doOutput", true);

        var sut = new EntryPoint(
            context,
            runner.Object,
            displayReports.Object,
            validator,
            logger,
            parameters);

        // Act
        var result = sut.Enter();

        // Assert
        result.Should().NotBeNull();

        context.Session.CurrentProcessName.Should().BeEmpty();
    }

    [Theory]
    [InlineAutoMoqData]
    public void EntryPoint_DispatchesProcessAndReturnsNull(
        [Frozen] Mock<IProcessRunner> runner,
        [Frozen] Mock<IDisplayReports> displayReports,
        CLIFunctionParameters parameters,
        string name)
    {
        // Arrange
        var logger = new Logger(new ConsoleWrapper());
        var validator = new ProcessingRequestValidator();
        var context = new TestContext();
        context.Session.CurrentProcessName = name;
        parameters.Parameters = [];

        parameters.Parameters!.Add("name", name);

        var sut = new EntryPoint(
            context,
            runner.Object,
            displayReports.Object,
            validator,
            logger,
            parameters);

        // Act
        var result = sut.Enter();

        // Assert
        result.Should().BeNull();

        context.Session.CurrentProcessName.Should().BeEmpty();
    }
    [Theory]
    [InlineAutoMoqData]
    public void EntryPoint_DispatchesProcessAndReturnsContextProcessingRequest(
        [Frozen] Mock<IProcessRunner> runner,
        [Frozen] Mock<IDisplayReports> displayReports,
        CLIFunctionParameters parameters,
        string name)
    {
        // Arrange
        var processingRequest = new ProcessingRequest([]);
        var logger = new Logger(new ConsoleWrapper());
        var validator = new ProcessingRequestValidator();
        var context = new TestContext();
        context.Session.CurrentProcessName = name;
        parameters.Parameters = [];

        parameters.Parameters!.Add("name", name);

        parameters.Parameters!.Add("doOutput", true);

        var sut = new EntryPoint(
            context,
            runner.Object,
            displayReports.Object,
            validator,
            logger,
            parameters);

        // Act
        var result = sut.Enter(processingRequest);

        // Assert
        result.Should().NotBeNull();

        context.Session.CurrentProcessName.Should().BeEmpty();
    }

    [Theory]
    [InlineAutoMoqData]
    public void EntryPoint_DispatchesProcessAndReturnsNullProcessingRequest(
        [Frozen] Mock<IProcessRunner> runner,
        [Frozen] Mock<IDisplayReports> displayReports,
        CLIFunctionParameters parameters,
        string name)
    {
        // Arrange
        var logger = new Logger(new ConsoleWrapper());
        var processingRequest = new ProcessingRequest([]);
        var validator = new ProcessingRequestValidator();
        var context = new TestContext();
        context.Session.CurrentProcessName = name;
        parameters.Parameters = [];

        parameters.Parameters!.Add("name", name);

        var sut = new EntryPoint(
            context,
            runner.Object,
            displayReports.Object,
            validator,
            logger,
            parameters);

        // Act
        var result = sut.Enter(processingRequest);

        // Assert
        result.Should().BeNull();

        context.Session.CurrentProcessName.Should().BeEmpty();
    }
}

public class TestContext : IContext
{
    public ISession Session { get; set; } = new Session();

    public void AddOrUpdate(string key, object value)
    {
        throw new NotImplementedException();
    }

    public object? Get(string key)
    {
        throw new NotImplementedException();
    }

    public TReturn? Get<TReturn>(string key) where TReturn : class
    {
        throw new NotImplementedException();
    }

    public bool KeyExists(string key)
    {
        throw new NotImplementedException();
    }

    public bool ParseSwitch(string key)
    {
        throw new NotImplementedException();
    }

    public IContextVerifier Requires()
    {
        throw new NotImplementedException();
    }
}
