using DoFramework.CLI;
using DoFramework.Logging;
using DoFramework.Environment;
using DoFramework.Processing;
using DoFramework.Services;
using FluentAssertions;
using DoFramework.FileSystem;
using DoFramework.Domain;
using DoFramework.Types;
using DoFramework.Validators;
using System.ComponentModel;
using DoFramework.Data;
using DoFramework.Mappers;

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

    [Fact]
    public void ServiceContainerExtensions_AddsProcessingServices()
    {
        // Arrange
        var sut = new ServiceContainer();

        sut.RegisterService<IEnvironment, DoFramework.Environment.Environment>();
        sut.RegisterService<ISession, Session>();
        sut.RegisterService<IContext, Context>();
        sut.RegisterService<IConsoleWrapper, ConsoleWrapper>();
        sut.RegisterService<ILogger, Logger>();
        sut.RegisterService<IReadProcessLocation, TestReadLocation>();
        sut.RegisterService<ISetProcessLocation, TestSetLocation>();
        sut.RegisterService<ISimpleDataProvider<ProjectContents>, TestContentsProvider>();
        sut.RegisterService<IMapper<ProjectContentsStorage, ProjectContents>, ReadProjectContentsMapper>();
        sut.RegisterService<IMapper<string, ProcessDescriptor>, ProcessDescriptorMapper>();
        sut.RegisterService<IMapper<string, ModuleDescriptor>, ModuleDescriptorMapper>();
        sut.RegisterService<IMapper<string, ComposerDescriptor>, ComposerDescriptorMapper>();
        sut.RegisterService<IMapper<string, TestDescriptor>, TestDescriptorMapper>();
        sut.RegisterService<IOSSanitise, OSSanitise>();
        sut.RegisterService<IFileManager, FileManager>();
        sut.RegisterService<IJsonConverter, JsonConverter>();
        sut.RegisterService<IResolver<ProcessDescriptor>, ProcessResolver>();
        sut.RegisterService<IDataCollectionProvider<ProcessDescriptor, string>, ProcessProvider>();
        sut.RegisterService<IDisplayReports, TestReportShower>();
        sut.RegisterService<IContextWriter, ContextWriter>();
        sut.RegisterService<IValidationErrorWriter, ValidationErrorWriter>();

        sut.AddParameters([]);

        // Act
        sut.AddProcessingServices(typeof(TestProcessBuilder));

        // Assert
        sut.GetService<IProcessBuilder>().Should().NotBeNull();
        sut.GetService<IProcessBuilder>().Should().BeOfType<TestProcessBuilder>();
    }

    [Fact]
    public void ServiceContainerExtensions_AddsComposerServices()
    {
        // Arrange
        var sut = new ServiceContainer();
        sut.RegisterService<IEnvironment, DoFramework.Environment.Environment>();
        sut.RegisterService<ISession, Session>();
        sut.RegisterService<IContext, Context>();
        sut.RegisterService<IConsoleWrapper, ConsoleWrapper>();
        sut.RegisterService<ILogger, Logger>();
        sut.RegisterService<IReadProcessLocation, TestReadLocation>();
        sut.RegisterService<ISetProcessLocation, TestSetLocation>();
        sut.RegisterService<ISimpleDataProvider<ProjectContents>, TestContentsProvider>();
        sut.RegisterService<IMapper<ProjectContentsStorage, ProjectContents>, ReadProjectContentsMapper>();
        sut.RegisterService<IMapper<string, ProcessDescriptor>, ProcessDescriptorMapper>();
        sut.RegisterService<IMapper<string, ModuleDescriptor>, ModuleDescriptorMapper>();
        sut.RegisterService<IMapper<string, ComposerDescriptor>, ComposerDescriptorMapper>();
        sut.RegisterService<IMapper<string, TestDescriptor>, TestDescriptorMapper>();
        sut.RegisterService<IOSSanitise, OSSanitise>();
        sut.RegisterService<IFileManager, FileManager>();
        sut.RegisterService<IJsonConverter, JsonConverter>();
        sut.RegisterService<IResolver<ComposerDescriptor>, ComposerResolver>();
        sut.RegisterService<IDataCollectionProvider<ComposerDescriptor, string>, ComposerProvider>();
        sut.RegisterService<IDisplayReports, TestReportShower>();
        sut.RegisterService<IContextWriter, ContextWriter>();
        sut.RegisterService<IValidationErrorWriter, ValidationErrorWriter>();

        // Act
        sut.AddComposerServices(typeof(TestComposerBuilder));

        // Assert
        sut.GetService<IComposerBuilder>().Should().NotBeNull();
        sut.GetService<IComposerBuilder>().Should().BeOfType<TestComposerBuilder>();
    }
}

public class TestReportShower : IDisplayReports
{
    public void Display(List<ProcessReport> processReports)
    {
    }
}

public class TestContentsProvider : ISimpleDataProvider<ProjectContents>
{
    public ProjectContents Provide()
    {
        return new ProjectContents();
    }
}

public class TestSetLocation : ISetProcessLocation
{
    public void Set(string location)
    {
    }
}

public class TestReadLocation : IReadProcessLocation
{
    public string Read()
    {
        return string.Empty;
    }
}

public class TestProcessBuilder : IProcessBuilder
{
    public IProcess Build(ProcessDescriptor descriptor)
    {
        throw new NotImplementedException();
    }
}

public class TestComposerBuilder : IComposerBuilder
{
    public IComposer Build(ComposerDescriptor composerDescriptor)
    {
        throw new NotImplementedException();
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