using AutoFixture.Xunit2;
using DoFramework.CLI;
using DoFramework.Logging;
using DoFramework.Environment;
using DoFramework.Processing;
using DoFramework.Services;
using FluentAssertions;
using Moq;

namespace DoFrameworkTests.Services;

public class ServiceContainerExtensionsTests
{
    [Fact]
    public void ServiceContainerExtensions_CheckEnvironment()
    {
        // Arrange
        var sut = new ServiceContainer();

        sut.RegisterService<IEnvironment, TestEnvironment>();


        // Act
        var func = ()=> sut.CheckEnvironment();

        // Assert
        func.Should().NotThrow();
    }

    [Fact]
    public void ServiceContainerExtensions_CheckEnvironmentShouldThrow()
    {
        // Arrange
        var sut = new ServiceContainer();

        sut.RegisterService<IEnvironment, TestThrowingEnvironment>();

        // Act
        var func = () => sut.CheckEnvironment();

        // Assert
        func.Should().Throw<Exception>().WithMessage("Test Exception.");
    }

    [Fact]
    public void ServiceContainerExtensions_EnvironmentNotRegisteredResolutionFailure()
    {
        // Arrange
        var sut = new ServiceContainer();

        // Act
        var func = () => sut.CheckEnvironment();

        // Assert
        func.Should().Throw<Exception>().WithMessage($"Service of Type '{typeof(IEnvironment)}' could not be resolved.");
    }

    [Theory]
    [InlineAutoMoqData]
    public void ServiceContainerExtensions_RegistersAppParams(
        Dictionary<string, object> appParams)
    {
        // Arrange
        var sut = new ServiceContainer();

        sut.RegisterService<IConsoleWrapper, ConsoleWrapper>();
        sut.RegisterService<ILogger, Logger>();

        // Act
        sut.AddParameters(appParams);

        var result = sut.Instances.Count;

        // Assert
        sut.GetService<CLIFunctionParameters>().Should().NotBeNull();

        result.Should().Be(4);

        sut.GetService<ILogger>().Parameters!.Parameters.Should().BeEquivalentTo(appParams);
    }

    [Fact]
    public void ServiceContainerExtensions_ConsumeEnvFilesWritesDictionary()
    {
        // Arrange
        var sut = new ServiceContainer();

        sut.RegisterService<IConsumeEnvFiles, TestConsumeEnvFiles>();

        var consumeEnv = sut.GetService<IConsumeEnvFiles>() as TestConsumeEnvFiles;

        // Act
        sut.ConsumeEnvFiles();

        // Assert
        consumeEnv!.Called.Should().BeTrue();
    }
}

public class TestEnvironment : IEnvironment
{
    public string HomeDir { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    public string ProcessesDir { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    public string TestsDir { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    public string ModuleDir { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

    public bool CheckEnvironment() 
    { 
        return true; 
    }

    public void ValidateEnvironment() 
    { 

    }
}

public class TestThrowingEnvironment : IEnvironment
{
    public string HomeDir { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    public string ProcessesDir { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    public string TestsDir { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    public string ModuleDir { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

    public bool CheckEnvironment()
    {
        return true;
    }

    public void ValidateEnvironment()
    {
        throw new Exception("Test Exception.");
    }
}

public class TestConsumeEnvFiles : IConsumeEnvFiles
{
    public bool Called { get; private set; }

    public void Consume() 
    {
        Called = true;
    }
}