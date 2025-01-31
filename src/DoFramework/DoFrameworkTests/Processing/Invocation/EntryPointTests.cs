using AutoFixture.Xunit2;
using DoFramework.CLI;
using DoFramework.Processing;
using FluentAssertions;
using Moq;

namespace DoFrameworkTests.Processing;

public class EntryPointTests
{
//    [Theory]
//    [InlineAutoMoqData]
//    public void EntryPoint_DispatchesProcessAndReturnsContext(
//        [Frozen] Mock<IProcessDispatcher> dispatcher,
//        CLIFunctionParameters parameters,
//        string name)
//    {
//        // Arrange
//        var context = new TestContext();
//        context.Session.CurrentProcessName = name;
//        parameters.Parameters = [];

//        parameters.Parameters!.Add("name", name);

//        parameters.Parameters!.Add("doOutput", true);

//        var sut = new EntryPoint(dispatcher.Object, context, parameters);

//        // Act
//        var result = sut.Enter();

//        // Assert
//        result.Should().NotBeNull();

//        context.Session.CurrentProcessName.Should().BeEmpty();

//        dispatcher.Verify(x => x.Dispatch(It.IsAny<IProcessingRequest>())); 
//    }

//    [Theory]
//    [InlineAutoMoqData]
//    public void EntryPoint_DispatchesProcessAndReturnsNull(
//        [Frozen] Mock<IProcessDispatcher> dispatcher,
//        CLIFunctionParameters parameters,
//        string name)
//    {
//        // Arrange
//        var context = new TestContext();
//        context.Session.CurrentProcessName = name;
//        parameters.Parameters = [];

//        parameters.Parameters!.Add("name", name);

//        var sut = new EntryPoint(dispatcher.Object, context, parameters);

//        // Act
//        var result = sut.Enter();

//        // Assert
//        result.Should().BeNull();

//        context.Session.CurrentProcessName.Should().BeEmpty();

//        dispatcher.Verify(x => x.Dispatch(It.IsAny<IProcessingRequest>()));
//    }
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
