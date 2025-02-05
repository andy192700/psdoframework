using AutoFixture.Xunit2;
using DoFramework.Data;
using DoFramework.Domain;
using DoFramework.Environment;
using DoFramework.FileSystem;
using DoFramework.Mappers;
using DoFramework.Validators;
using DoFramework.Testing;
using FluentAssertions;
using Moq;
using Newtonsoft.Json;

namespace DoFrameworkTests.Validators;

public class DescriptorCreatorValidatorTests
{
    [Theory]
    [InlineAutoMoqData]
    public void ProcesssDescriptorCreatorValidator_InvalidExtension(
        [Frozen] Mock<IEnvironment> environment,
        [Frozen] Mock<IReadProcessLocation> processReader,
        ProjectContentsStorage projectContentsStorage,
        string name,
        string path,
        string extension)
    {
        InValidExtensionsTest<ProcessDescriptor>(environment, processReader, projectContentsStorage, name, path, extension);
    }

    [Theory]
    [InlineAutoMoqData]
    public void ModuleDescriptorCreatorValidator_InvalidExtension(
        [Frozen] Mock<IEnvironment> environment,
        [Frozen] Mock<IReadProcessLocation> processReader,
        ProjectContentsStorage projectContentsStorage,
        string name,
        string path,
        string extension)
    {
        InValidExtensionsTest<ModuleDescriptor>(environment, processReader, projectContentsStorage, name, path, extension);
    }

    [Theory]
    [InlineAutoMoqData]
    public void ComposerDescriptorCreatorValidator_InvalidExtension(
        [Frozen] Mock<IEnvironment> environment,
        [Frozen] Mock<IReadProcessLocation> processReader,
        ProjectContentsStorage projectContentsStorage,
        string name,
        string path,
        string extension)
    {
        InValidExtensionsTest<ComposerDescriptor>(environment, processReader, projectContentsStorage, name, path, extension);
    }

    [Theory]
    [InlineAutoMoqData]
    public void TestModuleDescriptorCreatorValidator_InvalidExtension(
        [Frozen] Mock<IEnvironment> environment,
        [Frozen] Mock<IReadProcessLocation> processReader,
        ProjectContentsStorage projectContentsStorage,
        string name,
        string path,
        string extension)
    {
        // Arrange
        var osSanitise = new OSSanitise();

        var processDescriptorMapper = new ProcessDescriptorMapper(osSanitise);
        var moduleDescriptorMapper = new ModuleDescriptorMapper(osSanitise);
        var testDescriptorMapper = new TestDescriptorMapper(osSanitise);
        var composerDescriptorMapper = new ComposerDescriptorMapper(osSanitise);

        var descriptor = new TestDescriptor
        {
            Name = $"{name}Tests",
            TestType = TestType.Module
        };

        var jsonConverter = new Mock<IJsonConverter>();

        projectContentsStorage.Modules.Add(name);

        jsonConverter.Setup(x => x.Deserialize<ProjectContentsStorage>(It.IsAny<string>())).Returns(projectContentsStorage);

        var fileManager = new Mock<IFileManager>();

        descriptor.Path = $"{path}{DoFramework.Environment.Environment.Separator}{descriptor.Name}.{extension}";

        var contentsMapper = new ReadProjectContentsMapper(processDescriptorMapper, moduleDescriptorMapper, testDescriptorMapper, composerDescriptorMapper, osSanitise);

        var contentsProvider = new ReadProjectContents(contentsMapper, processReader.Object, fileManager.Object, jsonConverter.Object);

        fileManager.Setup(x => x.GetFileInfo(It.IsAny<string>())).Returns(new FileInfo(descriptor.Path));

        string jsonString = JsonConvert.SerializeObject(projectContentsStorage, Formatting.Indented);

        fileManager.Setup(x => x.ReadAllText(It.IsAny<string>())).Returns(jsonString);

        environment.Setup(x => x.HomeDir).Returns(System.Environment.CurrentDirectory);

        processReader.Setup(x => x.Read()).Returns(System.Environment.CurrentDirectory);

        var sut = new DescriptorCreatorValidator(environment.Object, fileManager.Object, contentsProvider);

        // Act
        var result = sut.Validate(descriptor);

        // Assert
        result.Should().NotBeNull();

        result.IsValid.Should().BeFalse();

        result.Errors.Count.Should().Be(1);

        result.Errors[0].Should().Be($"A {descriptor.TypeName} file must have the extension '{descriptor.Extension}'");
    }

    [Theory]
    [InlineAutoMoqData]
    public void ProcessDescriptorCreatorValidator_InvalidAlreadyExists(
        [Frozen] Mock<IEnvironment> environment,
        [Frozen] Mock<IReadProcessLocation> processReader,
        string name,
        string path)
    {
        var projectContents = new ProjectContents();

        var descriptor = new ProcessDescriptor
        {
            Name = name
        };

        descriptor.Path = $"{path}{DoFramework.Environment.Environment.Separator}{descriptor.Name}{descriptor.Extension}";

        projectContents.Processes.Add(descriptor);

        InvalidAlreadyExistsTest(environment, processReader, projectContents, descriptor);
    }

    [Theory]
    [InlineAutoMoqData]
    public void ModuleDescriptorCreatorValidator_InvalidAlreadyExists(
        [Frozen] Mock<IEnvironment> environment,
        Mock<IReadProcessLocation> processReader,
        string name,
        string path)
    {
        var projectContents = new ProjectContents();

        var descriptor = new ModuleDescriptor
        {
            Name = name
        };

        descriptor.Path = $"{path}{DoFramework.Environment.Environment.Separator}{descriptor.Name}{descriptor.Extension}";

        projectContents.Modules.Add(descriptor);

        InvalidAlreadyExistsTest(environment, processReader, projectContents, descriptor);
    }

    [Theory]
    [InlineAutoMoqData]
    public void ComposerDescriptorCreatorValidator_InvalidAlreadyExists(
        [Frozen] Mock<IEnvironment> environment,
        Mock<IReadProcessLocation> processReader,
        string name,
        string path)
    {
        var projectContents = new ProjectContents();

        var descriptor = new ComposerDescriptor
        {
            Name = name
        };

        descriptor.Path = $"{path}{DoFramework.Environment.Environment.Separator}{descriptor.Name}{descriptor.Extension}";

        projectContents.Composers.Add(descriptor);

        InvalidAlreadyExistsTest(environment, processReader, projectContents, descriptor);
    }

    [Theory]
    [InlineAutoMoqData]
    public void TestDescriptorCreatorValidator_InvalidAlreadyExists(
        [Frozen] Mock<IEnvironment> environment,
        [Frozen] Mock<IReadProcessLocation> processReader,
        string name,
        string path)
    {
        var projectContents = new ProjectContents();

        var descriptor = new TestDescriptor
        {
            Name = $"{name}Tests",
            TestType = TestType.Process
        };

        descriptor.Path = $"{path}{DoFramework.Environment.Environment.Separator}{descriptor.Name}{descriptor.Extension}";

        projectContents.Processes.Add(new ProcessDescriptor
        {
            Name = name,
            Path = name
        });

        projectContents.Tests.Add(descriptor);

        InvalidAlreadyExistsTest(environment, processReader, projectContents, descriptor);
    }

    [Theory]
    [InlineAutoMoqData]
    public void ProcessDescriptorCreatorValidator_Valid(
        [Frozen] Mock<IEnvironment> environment,
        [Frozen] Mock<IReadProcessLocation> processReader,
        ProjectContentsStorage projectContentsStorage,
        string name,
        string path)
    {
        ValidTest<ProcessDescriptor>(environment, processReader, projectContentsStorage, name, path);
    }

    [Theory]
    [InlineAutoMoqData]
    public void ComposerDescriptorCreatorValidator_Valid(
        [Frozen] Mock<IEnvironment> environment,
        [Frozen] Mock<IReadProcessLocation> processReader,
        ProjectContentsStorage projectContentsStorage,
        string name,
        string path)
    {
        ValidTest<ComposerDescriptor>(environment, processReader, projectContentsStorage, name, path);
    }

    [Theory]
    [InlineAutoMoqData]
    public void ModuleDescriptorCreatorValidator_Valid(
        [Frozen] Mock<IEnvironment> environment,
        [Frozen] Mock<IReadProcessLocation> processReader,
        ProjectContentsStorage projectContentsStorage,
        string name,
        string path)
    {
        ValidTest<ModuleDescriptor>(environment, processReader, projectContentsStorage, name, path);
    }

    [Theory]
    [InlineAutoMoqData]
    public void TestDescriptorCreatorValidator_Valid(
        [Frozen] Mock<IEnvironment> environment,
        [Frozen] Mock<IReadProcessLocation> processReader,
        ProjectContentsStorage projectContentsStorage,
        string name,
        string path)
    {
        // Arrange
        var osSanitise = new OSSanitise();

        var processDescriptorMapper = new ProcessDescriptorMapper(osSanitise);
        var moduleDescriptorMapper = new ModuleDescriptorMapper(osSanitise);
        var testDescriptorMapper = new TestDescriptorMapper(osSanitise);
        var composerDescriptorMapper = new ComposerDescriptorMapper(osSanitise);

        var fileManager = new Mock<IFileManager>();

        var jsonConverter = new Mock<IJsonConverter>();

        projectContentsStorage.Processes.Add(name);

        jsonConverter.Setup(x => x.Deserialize<ProjectContentsStorage>(It.IsAny<string>())).Returns(projectContentsStorage);

        var descriptor = new TestDescriptor
        {
            Name = $"{name}Tests",
            TestType = TestType.Process
        };

        descriptor.Path = $"{path}{DoFramework.Environment.Environment.Separator}{descriptor.Name}{descriptor.Extension}";

        var contentsMapper = new ReadProjectContentsMapper(processDescriptorMapper, moduleDescriptorMapper, testDescriptorMapper, composerDescriptorMapper, osSanitise);

        var contentsProvider = new ReadProjectContents(contentsMapper, processReader.Object, fileManager.Object, jsonConverter.Object);

        fileManager.Setup(x => x.GetFileInfo(It.IsAny<string>())).Returns(new FileInfo(descriptor.Path));

        string jsonString = JsonConvert.SerializeObject(projectContentsStorage, Formatting.Indented);

        fileManager.Setup(x => x.ReadAllText(It.IsAny<string>())).Returns(jsonString);

        environment.Setup(x => x.HomeDir).Returns(System.Environment.CurrentDirectory);

        processReader.Setup(x => x.Read()).Returns(System.Environment.CurrentDirectory);

        var sut = new DescriptorCreatorValidator(environment.Object, fileManager.Object, contentsProvider);

        // Act
        var result = sut.Validate(descriptor);

        // Assert
        result.Should().NotBeNull();

        result.IsValid.Should().BeTrue();

        result.Errors.Count.Should().Be(0);
    }

    [Theory]
    [InlineAutoMoqData]
    public void TestDescriptorCreatorValidator_InValidProcessNotExist(
        [Frozen] Mock<IEnvironment> environment,
        [Frozen] Mock<IReadProcessLocation> processReader,
        ProjectContentsStorage projectContentsStorage,
        string name,
        string path)
    {
        // Arrange
        var osSanitise = new Mock<IOSSanitise>();

        osSanitise.Setup(o => o.Sanitise(It.IsAny<string>())).Returns(path);

        var processDescriptorMapper = new ProcessDescriptorMapper(osSanitise.Object);
        var moduleDescriptorMapper = new ModuleDescriptorMapper(osSanitise.Object);
        var testDescriptorMapper = new TestDescriptorMapper(osSanitise.Object);
        var composerDescriptorMapper = new ComposerDescriptorMapper(osSanitise.Object);

        var fileManager = new Mock<IFileManager>();

        var jsonConverter = new Mock<IJsonConverter>();

        jsonConverter.Setup(x => x.Deserialize<ProjectContentsStorage>(It.IsAny<string>())).Returns(projectContentsStorage);

        projectContentsStorage.Processes.Add(name);

        var descriptor = new TestDescriptor
        {
            Name = $"{name}Tests",
            TestType = TestType.Process
        };

        descriptor.Path = $"{path}{DoFramework.Environment.Environment.Separator}{descriptor.Name}{descriptor.Extension}";

        var contentsMapper = new ReadProjectContentsMapper(processDescriptorMapper, moduleDescriptorMapper, testDescriptorMapper, composerDescriptorMapper, osSanitise.Object);

        var contentsProvider = new ReadProjectContents(contentsMapper, processReader.Object, fileManager.Object, jsonConverter.Object);

        fileManager.Setup(x => x.GetFileInfo(It.IsAny<string>())).Returns(new FileInfo(descriptor.Path));

        string jsonString = JsonConvert.SerializeObject(projectContentsStorage, Formatting.Indented);

        fileManager.Setup(x => x.ReadAllText(It.IsAny<string>())).Returns(jsonString);

        environment.Setup(x => x.HomeDir).Returns(System.Environment.CurrentDirectory);

        processReader.Setup(x => x.Read()).Returns(System.Environment.CurrentDirectory);

        var sut = new DescriptorCreatorValidator(environment.Object, fileManager.Object, contentsProvider);

        // Act
        var result = sut.Validate(descriptor);

        // Assert
        result.Should().NotBeNull();

        result.IsValid.Should().BeFalse();

        result.Errors.Count.Should().Be(1);

        result.Errors[0].Should().Be($"Cannot create tests for the Process {name} because it does not exist");
    }

    [Theory]
    [InlineAutoMoqData]
    public void TestDescriptorCreatorValidator_InValidModulesNotExist(
        [Frozen] Mock<IEnvironment> environment,
        [Frozen] Mock<IReadProcessLocation> processReader,
        ProjectContentsStorage projectContentsStorage,
        string name,
        string path)
    {
        // Arrange
        var osSanitise = new Mock<IOSSanitise>();

        osSanitise.Setup(o => o.Sanitise(It.IsAny<string>())).Returns(path);

        var processDescriptorMapper = new ProcessDescriptorMapper(osSanitise.Object);
        var moduleDescriptorMapper = new ModuleDescriptorMapper(osSanitise.Object);
        var testDescriptorMapper = new TestDescriptorMapper(osSanitise.Object);
        var composerDescriptorMapper = new ComposerDescriptorMapper(osSanitise.Object);

        var fileManager = new Mock<IFileManager>();

        var jsonConverter = new Mock<IJsonConverter>();

        jsonConverter.Setup(x => x.Deserialize<ProjectContentsStorage>(It.IsAny<string>())).Returns(projectContentsStorage);

        projectContentsStorage.Modules.Add(name);

        var descriptor = new TestDescriptor
        {
            Name = $"{name}Tests",
            TestType = TestType.Module
        };

        descriptor.Path = $"{path}{DoFramework.Environment.Environment.Separator}{descriptor.Name}{descriptor.Extension}";

        var contentsMapper = new ReadProjectContentsMapper(processDescriptorMapper, moduleDescriptorMapper, testDescriptorMapper, composerDescriptorMapper, osSanitise.Object);

        var contentsProvider = new ReadProjectContents(contentsMapper, processReader.Object, fileManager.Object, jsonConverter.Object);

        fileManager.Setup(x => x.GetFileInfo(It.IsAny<string>())).Returns(new FileInfo(descriptor.Path));

        string jsonString = JsonConvert.SerializeObject(projectContentsStorage, Formatting.Indented);

        fileManager.Setup(x => x.ReadAllText(It.IsAny<string>())).Returns(jsonString);

        environment.Setup(x => x.HomeDir).Returns(System.Environment.CurrentDirectory);

        processReader.Setup(x => x.Read()).Returns(System.Environment.CurrentDirectory);

        var sut = new DescriptorCreatorValidator(environment.Object, fileManager.Object, contentsProvider);

        // Act
        var result = sut.Validate(descriptor);

        // Assert
        result.Should().NotBeNull();

        result.IsValid.Should().BeFalse();

        result.Errors.Count.Should().Be(1);

        result.Errors[0].Should().Be($"Cannot create tests for the Module {name} because it does not exist");
    }

    private void InValidExtensionsTest<TDescriptor>(
        Mock<IEnvironment> environment,
        Mock<IReadProcessLocation> processReader,
        ProjectContentsStorage projectContentsStorage,
        string name,
        string path,
        string extension) where TDescriptor : IDescriptor, new()
    {
        // Arrange
        var osSanitise = new Mock<IOSSanitise>();

        osSanitise.Setup(o => o.Sanitise(It.IsAny<string>())).Returns(path);

        var processDescriptorMapper = new ProcessDescriptorMapper(osSanitise.Object);
        var moduleDescriptorMapper = new ModuleDescriptorMapper(osSanitise.Object);
        var testDescriptorMapper = new TestDescriptorMapper(osSanitise.Object);
        var composerDescriptorMapper = new ComposerDescriptorMapper(osSanitise.Object);

        var descriptor = new TDescriptor
        {
            Name = name
        };

        var jsonConverter = new Mock<IJsonConverter>();

        jsonConverter.Setup(x => x.Deserialize<ProjectContentsStorage>(It.IsAny<string>())).Returns(projectContentsStorage);

        var fileManager = new Mock<IFileManager>();

        descriptor.Path = $"{path}{DoFramework.Environment.Environment.Separator}{descriptor.Name}.{extension}";

        var contentsMapper = new ReadProjectContentsMapper(processDescriptorMapper, moduleDescriptorMapper, testDescriptorMapper, composerDescriptorMapper, osSanitise.Object);

        var contentsProvider = new ReadProjectContents(contentsMapper, processReader.Object, fileManager.Object, jsonConverter.Object);

        fileManager.Setup(x => x.GetFileInfo(It.IsAny<string>())).Returns(new FileInfo(descriptor.Path));

        string jsonString = JsonConvert.SerializeObject(projectContentsStorage, Formatting.Indented);

        fileManager.Setup(x => x.ReadAllText(It.IsAny<string>())).Returns(jsonString);

        environment.Setup(x => x.HomeDir).Returns(System.Environment.CurrentDirectory);

        processReader.Setup(x => x.Read()).Returns(System.Environment.CurrentDirectory);

        var sut = new DescriptorCreatorValidator(environment.Object, fileManager.Object, contentsProvider);

        // Act
        var result = sut.Validate(descriptor);

        // Assert
        result.Should().NotBeNull();

        result.IsValid.Should().BeFalse();

        result.Errors.Count.Should().Be(1);

        result.Errors[0].Should().Be($"A {descriptor.TypeName} file must have the extension '{descriptor.Extension}'");
    }

    private void InvalidAlreadyExistsTest(
        Mock<IEnvironment> environment,
        Mock<IReadProcessLocation> processReader,
        ProjectContents projectContents,
        IDescriptor descriptor)
    {
        // Arrange
        var osSanitise = new OSSanitise();

        var processDescriptorMapper = new ProcessDescriptorMapper(osSanitise);
        var moduleDescriptorMapper = new ModuleDescriptorMapper(osSanitise);
        var testDescriptorMapper = new TestDescriptorMapper(osSanitise);
        var composerDescriptorMapper = new ComposerDescriptorMapper(osSanitise);

        var contentsMapper = new ReadProjectContentsMapper(processDescriptorMapper, moduleDescriptorMapper, testDescriptorMapper, composerDescriptorMapper, osSanitise);
        var saveContentsMapper = new SaveProjectContentsMapper();

        var jsonConverter = new Mock<IJsonConverter>();

        var fileManager = new Mock<IFileManager>();

        var contentsProvider = new ReadProjectContents(contentsMapper, processReader.Object, fileManager.Object, jsonConverter.Object);

        fileManager.Setup(x => x.GetFileInfo(It.IsAny<string>())).Returns(new FileInfo(descriptor.Path!));

        var projectContentsStorage = saveContentsMapper.Map(projectContents);

        jsonConverter.Setup(x => x.Deserialize<ProjectContentsStorage>(It.IsAny<string>())).Returns(projectContentsStorage);

        string jsonString = JsonConvert.SerializeObject(projectContentsStorage, Formatting.Indented);

        fileManager.Setup(x => x.ReadAllText(It.IsAny<string>())).Returns(jsonString);

        environment.Setup(x => x.HomeDir).Returns(System.Environment.CurrentDirectory);

        processReader.Setup(x => x.Read()).Returns(System.Environment.CurrentDirectory);

        var sut = new DescriptorCreatorValidator(environment.Object, fileManager.Object, contentsProvider);

        // Act
        var result = sut.Validate(descriptor);

        // Assert
        result.Should().NotBeNull();

        result.IsValid.Should().BeFalse();

        result.Errors.Count.Should().Be(1);

        result.Errors[0].Should().Be($"A {descriptor.TypeName} named {descriptor.Name} already exists");
    }

    private void ValidTest<TDescriptor>(
        Mock<IEnvironment> environment,
        Mock<IReadProcessLocation> processReader,
        ProjectContentsStorage projectContentsStorage,
        string name,
        string path) where TDescriptor : IDescriptor, new()
    {
        // Arrange
        var osSanitise = new Mock<IOSSanitise>();

        osSanitise.Setup(o => o.Sanitise(It.IsAny<string>())).Returns(path);

        var processDescriptorMapper = new ProcessDescriptorMapper(osSanitise.Object);
        var moduleDescriptorMapper = new ModuleDescriptorMapper(osSanitise.Object);
        var testDescriptorMapper = new TestDescriptorMapper(osSanitise.Object);
        var composerDescriptorMapper = new ComposerDescriptorMapper(osSanitise.Object);

        var fileManager = new Mock<IFileManager>();

        var jsonConverter = new Mock<IJsonConverter>();

        jsonConverter.Setup(x => x.Deserialize<ProjectContentsStorage>(It.IsAny<string>())).Returns(projectContentsStorage);

        var descriptor = new TDescriptor
        {
            Name = name
        };

        descriptor.Path = $"{path}{DoFramework.Environment.Environment.Separator}{descriptor.Name}{descriptor.Extension}";

        var contentsMapper = new ReadProjectContentsMapper(processDescriptorMapper, moduleDescriptorMapper, testDescriptorMapper, composerDescriptorMapper, osSanitise.Object);

        var contentsProvider = new ReadProjectContents(contentsMapper, processReader.Object, fileManager.Object, jsonConverter.Object);

        fileManager.Setup(x => x.GetFileInfo(It.IsAny<string>())).Returns(new FileInfo(descriptor.Path));

        string jsonString = JsonConvert.SerializeObject(projectContentsStorage, Formatting.Indented);

        fileManager.Setup(x => x.ReadAllText(It.IsAny<string>())).Returns(jsonString);

        environment.Setup(x => x.HomeDir).Returns(System.Environment.CurrentDirectory);

        processReader.Setup(x => x.Read()).Returns(System.Environment.CurrentDirectory);

        var sut = new DescriptorCreatorValidator(environment.Object, fileManager.Object, contentsProvider);

        // Act
        var result = sut.Validate(descriptor);

        // Assert
        result.Should().NotBeNull();

        result.IsValid.Should().BeTrue();

        result.Errors.Count.Should().Be(0);
    }
}
