using AutoFixture.Xunit2;
using DoFramework.CLI;
using DoFramework.Logging;
using DoFramework.Environment;
using DoFramework.Processing;
using DoFramework.Services;
using FluentAssertions;
using Moq;
using DoFramework.FileSystem;
using System.Net.Http.Headers;

namespace DoFrameworkTests.Services;

public class ServiceContainerExtensionsTests
{
    [Fact]
    public void ServiceContainerExtensions_CheckEnvironment()
    {
        // Arrange
        var sut = new ServiceContainer();

        sut.RegisterService<IFileManager, PassingFileManager>();
        sut.RegisterService<IReadProcessLocation, TestLocationReader>();

        // Act
        var func = () => sut.CheckEnvironment();

        // Assert
        func.Should().NotThrow();
    }

    [Fact]
    public void ServiceContainerExtensions_CheckEnvironmentShouldThrow()
    {
        // Arrange
        var sut = new ServiceContainer();

        sut.RegisterService<IFileManager, FailingFileManager>();
        sut.RegisterService<IReadProcessLocation, TestLocationReader>();

        // Act
        var func = () => sut.CheckEnvironment();

        // Assert
        func.Should().Throw<Exception>().WithMessage("Could not find do.json.");
    }

    [Theory]
    [InlineAutoMoqData]
    public void ServiceContainerExtensions_RegistersAppParams(
        Dictionary<string, object> appParams)
    {
        // Arrange
        var sut = new ServiceContainer();

        sut.RegisterService<ISession, Session>();
        sut.RegisterService<IContext, Context>();
        sut.RegisterService<IContextWriter, ContextWriter>();
        sut.RegisterService<IConsoleWrapper, ConsoleWrapper>();
        sut.RegisterService<ILogger, Logger>();

        // Act
        sut.AddParameters(appParams);

        // Assert
        sut.GetService<CLIFunctionParameters>().Should().NotBeNull();
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
    public string ComposersDir { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

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
    public string ComposersDir { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

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

public class TestLocationReader : IReadProcessLocation
{
    public string Read()
    {
        return string.Empty;
    }
}

public class PassingFileManager : IFileManager
{
    public void CreateParentDirectory(string path)
    {
        throw new NotImplementedException();
    }

    public void DeleteFile(string path)
    {
        throw new NotImplementedException();
    }

    public bool FileExists(string path)
    {
        return true;
    }

    public FileInfo GetFileInfo(string path)
    {
        throw new NotImplementedException();
    }

    public FileInfo[] GetFiles(string path, string searchPattern, SearchOption searchOption)
    {
        throw new NotImplementedException();
    }

    public bool ParentDirectoryExists(string path)
    {
        throw new NotImplementedException();
    }

    public string[] ReadAllLines(string path)
    {
        throw new NotImplementedException();
    }

    public string ReadAllText(string path)
    {
        throw new NotImplementedException();
    }

    public void WriteAllText(string path, string data)
    {
        throw new NotImplementedException();
    }
}

public class FailingFileManager : IFileManager
{
    public void CreateParentDirectory(string path)
    {
        throw new NotImplementedException();
    }

    public void DeleteFile(string path)
    {
        throw new NotImplementedException();
    }

    public bool FileExists(string path)
    {
        return false;
    }

    public FileInfo GetFileInfo(string path)
    {
        throw new NotImplementedException();
    }

    public FileInfo[] GetFiles(string path, string searchPattern, SearchOption searchOption)
    {
        throw new NotImplementedException();
    }

    public bool ParentDirectoryExists(string path)
    {
        throw new NotImplementedException();
    }

    public string[] ReadAllLines(string path)
    {
        throw new NotImplementedException();
    }

    public string ReadAllText(string path)
    {
        throw new NotImplementedException();
    }

    public void WriteAllText(string path, string data)
    {
        throw new NotImplementedException();
    }
}