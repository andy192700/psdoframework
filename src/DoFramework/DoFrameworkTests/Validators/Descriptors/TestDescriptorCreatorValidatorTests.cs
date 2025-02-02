using DoFramework.Data;
using DoFramework.Domain;
using DoFramework.Environment;
using DoFramework.Mappers;
using DoFramework.Validators;
using DoFramework.Testing;
using Moq;
using Newtonsoft.Json;
using FluentAssertions;
using AutoFixture.Xunit2;
using DoFramework.FileSystem;
using DoFramework.CLI;
using System.Text;

namespace DoFrameworkTests.Validators;

public class TestDescriptorCreatorValidatorTests
{
    [Theory]
    [InlineAutoMoqData]
    public void TestDescriptorCreatorValidator_IsValid(
        Mock<IEnvironment> environment,
        Mock<IReadProcessLocation> processReader,
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

        var descriptor = new TestDescriptor
        {
            Name = $"{name}Tests",
            TestType = TestType.Process
        };

        projectContentsStorage.Processes.Add(name);

        descriptor.Path = $"{path}{DoFramework.Environment.Environment.Separator}{descriptor.Name}{descriptor.Extension}";

        var fileManager = new Mock<IFileManager>();

        var jsonConverter = new Mock<IJsonConverter>();

        jsonConverter.Setup(x => x.Deserialize<ProjectContentsStorage>(It.IsAny<string>())).Returns(projectContentsStorage);

        var contentsMapper = new ReadProjectContentsMapper(processDescriptorMapper, moduleDescriptorMapper, testDescriptorMapper, composerDescriptorMapper, osSanitise);

        var contentsProvider = new ReadProjectContents(contentsMapper, processReader.Object, fileManager.Object, jsonConverter.Object);

        fileManager.Setup(x => x.GetFileInfo(It.IsAny<string>())).Returns(new FileInfo(descriptor.Path));

        string jsonString = JsonConvert.SerializeObject(projectContentsStorage, Formatting.Indented);

        fileManager.Setup(x => x.ReadAllText(It.IsAny<string>())).Returns(jsonString);

        processReader.Setup(x => x.Read()).Returns(System.Environment.CurrentDirectory);

        environment.Setup(x => x.HomeDir).Returns(System.Environment.CurrentDirectory);

        var parameters = new CLIFunctionParameters();

        parameters.Parameters = new Dictionary<string, object>
        {
            { "forProcess", true }
        };

        var sut = new TestDescriptorCreatorValidator(environment.Object, fileManager.Object, contentsProvider, parameters);

        // Act
        var result = sut.Validate(descriptor);

        // Assert
        result.Should().NotBeNull();

        result.IsValid.Should().BeTrue();

        result.Errors.Count.Should().Be(0);
    }

    [Theory]
    [InlineAutoMoqData]
    public void TestDescriptorCreatorValidator_InValidBadName(
        [Frozen] Mock<IReadProcessLocation> processReader,
        [Frozen] Mock<IEnvironment> environment,
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

        projectContentsStorage.Processes.Add($"{name}Test");

        var descriptor = new TestDescriptor
        {
            Name = $"{name}Test.....",
            TestType = TestType.Process
        };

        descriptor.Path = $"{path}{DoFramework.Environment.Environment.Separator}{descriptor.Name}{descriptor.Extension}";

        var fileManager = new Mock<IFileManager>();

        var jsonConverter = new Mock<IJsonConverter>();

        jsonConverter.Setup(x => x.Deserialize<ProjectContentsStorage>(It.IsAny<string>())).Returns(projectContentsStorage);

        var contentsMapper = new ReadProjectContentsMapper(processDescriptorMapper, moduleDescriptorMapper, testDescriptorMapper, composerDescriptorMapper, osSanitise);

        var contentsProvider = new ReadProjectContents(contentsMapper, processReader.Object, fileManager.Object, jsonConverter.Object);

        fileManager.Setup(x => x.GetFileInfo(It.IsAny<string>())).Returns(new FileInfo(descriptor.Path));

        string jsonString = JsonConvert.SerializeObject(projectContentsStorage, Formatting.Indented);

        fileManager.Setup(x => x.ReadAllText(It.IsAny<string>())).Returns(jsonString);

        processReader.Setup(x => x.Read()).Returns(System.Environment.CurrentDirectory);

        environment.Setup(x => x.HomeDir).Returns(System.Environment.CurrentDirectory);

        var parameters = new CLIFunctionParameters();

        parameters.Parameters = new Dictionary<string, object>
        {
            { "forProcess", true }
        };

        var sut = new TestDescriptorCreatorValidator(environment.Object, fileManager.Object, contentsProvider, parameters);

        // Act
        var result = sut.Validate(descriptor);

        // Assert
        result.Should().NotBeNull();

        result.IsValid.Should().BeFalse();

        result.Errors.Count.Should().Be(1);

        result.Errors[0].Should().Be("Tests names must be suffixed with the string 'Tests'.");
    }

    [Theory]
    [InlineAutoMoqData]
    public void TestDescriptorCreatorValidator_InValidNoTestTypeRequested(
        [Frozen] Mock<IReadProcessLocation> processReader,
        [Frozen] Mock<IEnvironment> environment,
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

        var descriptor = new TestDescriptor
        {
            Name = $"{name}Tests",
            TestType = TestType.Process
        };

        projectContentsStorage.Processes.Add(name);

        descriptor.Path = $"{path}{DoFramework.Environment.Environment.Separator}{descriptor.Name}{descriptor.Extension}";

        var fileManager = new Mock<IFileManager>();

        var jsonConverter = new Mock<IJsonConverter>();

        jsonConverter.Setup(x => x.Deserialize<ProjectContentsStorage>(It.IsAny<string>())).Returns(projectContentsStorage);

        var contentsMapper = new ReadProjectContentsMapper(processDescriptorMapper, moduleDescriptorMapper, testDescriptorMapper, composerDescriptorMapper, osSanitise);

        var contentsProvider = new ReadProjectContents(contentsMapper, processReader.Object, fileManager.Object, jsonConverter.Object);

        fileManager.Setup(x => x.GetFileInfo(It.IsAny<string>())).Returns(new FileInfo(descriptor.Path));

        string jsonString = JsonConvert.SerializeObject(projectContentsStorage, Formatting.Indented);

        fileManager.Setup(x => x.ReadAllText(It.IsAny<string>())).Returns(jsonString);

        processReader.Setup(x => x.Read()).Returns(System.Environment.CurrentDirectory);

        environment.Setup(x => x.HomeDir).Returns(System.Environment.CurrentDirectory);

        var parameters = new CLIFunctionParameters();

        parameters.Parameters = [];

        var sut = new TestDescriptorCreatorValidator(environment.Object, fileManager.Object, contentsProvider, parameters);

        // Act
        var result = sut.Validate(descriptor);

        // Assert
        result.Should().NotBeNull();

        result.IsValid.Should().BeFalse();

        result.Errors.Count.Should().Be(1);

        var sb = new StringBuilder();
        sb.AppendLine("Please specify a type of test to define.");
        sb.AppendLine("-forProcess for Process tests.");
        sb.AppendLine("-forModule for Module tests.");
        sb.AppendLine("-forComposer for Composer tests.");

        result.Errors[0].Should().Be(sb.ToString());
    }

    [Theory]
    [InlineAutoMoqData]
    public void TestDescriptorCreatorValidator_InValidMultipleTestTypeRequested(
        [Frozen] Mock<IReadProcessLocation> processReader,
        [Frozen] Mock<IEnvironment> environment,
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

        projectContentsStorage.Processes.Add(name);

        var descriptor = new TestDescriptor
        {
            Name = $"{name}Tests",
            TestType = TestType.Composer
        };

        descriptor.Path = $"{path}{DoFramework.Environment.Environment.Separator}{descriptor.Name}{descriptor.Extension}";

        var fileManager = new Mock<IFileManager>();

        var jsonConverter = new Mock<IJsonConverter>();

        jsonConverter.Setup(x => x.Deserialize<ProjectContentsStorage>(It.IsAny<string>())).Returns(projectContentsStorage);

        var contentsMapper = new ReadProjectContentsMapper(processDescriptorMapper, moduleDescriptorMapper, testDescriptorMapper, composerDescriptorMapper, osSanitise);

        var contentsProvider = new ReadProjectContents(contentsMapper, processReader.Object, fileManager.Object, jsonConverter.Object);

        fileManager.Setup(x => x.GetFileInfo(It.IsAny<string>())).Returns(new FileInfo(descriptor.Path));

        string jsonString = JsonConvert.SerializeObject(projectContentsStorage, Formatting.Indented);

        fileManager.Setup(x => x.ReadAllText(It.IsAny<string>())).Returns(jsonString);

        processReader.Setup(x => x.Read()).Returns(System.Environment.CurrentDirectory);

        environment.Setup(x => x.HomeDir).Returns(System.Environment.CurrentDirectory);

        var parameters = new CLIFunctionParameters
        {
            Parameters = new Dictionary<string, object>
            {
                { "forProcess", true },
                { "forModule", true },
                { "forComposer", true }
            }
        };

        var sut = new TestDescriptorCreatorValidator(environment.Object, fileManager.Object, contentsProvider, parameters);

        // Act
        var result = sut.Validate(descriptor);

        // Assert
        result.Should().NotBeNull();

        result.IsValid.Should().BeFalse();

        result.Errors.Count.Should().Be(1);

        var sb = new StringBuilder();
        sb.AppendLine("Multiple test types requested, only ONE can be selected.");
        sb.AppendLine("-forProcess for Process tests.");
        sb.AppendLine("-forModule for Module tests.");
        sb.AppendLine("-forComposer for Composer tests.");

        result.Errors[0].Should().Be(sb.ToString());
    }
}