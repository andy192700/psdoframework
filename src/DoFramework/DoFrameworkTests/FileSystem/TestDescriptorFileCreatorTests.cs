using AutoFixture.Xunit2;
using DoFramework.Data;
using DoFramework.Domain;
using DoFramework.Environment;
using DoFramework.FileSystem;
using DoFramework.Testing;
using Moq;
using System.IO;
using System.Text;

namespace DoFrameworkTests.FileSystem;

public class TestDescriptorFileCreatorTests
{
    [Theory]
    [InlineAutoMoqData]
    public void DoesNotCreateFileIfFileExists(
        [Frozen] Mock<IEnvironment> environment,
        [Frozen] Mock<IFileManager> fileManager,
        string name)
    {
        // Arrange
        var simpleDataProvider = new Mock<IDataCollectionProvider<ModuleDescriptor, string>>();

        var testDescriptor = new TestDescriptor
        {
            Name = name,
        };

        fileManager.Setup(x => x.FileExists(It.IsAny<string>())).Returns(true);

        fileManager.Setup(x => x.ParentDirectoryExists(It.IsAny<string>())).Returns(true);

        var sut = new TestDescriptorFileCreator(environment.Object, fileManager.Object, simpleDataProvider.Object);

        // Act
        sut.Create(testDescriptor);

        // Assert
        fileManager.Verify(x => x.WriteAllText(It.IsAny<string>(), It.IsAny<string>()), Times.Never);

        fileManager.Verify(x => x.ParentDirectoryExists(It.IsAny<string>()), Times.Once);

        fileManager.Verify(x => x.CreateParentDirectory(It.IsAny<string>()), Times.Never);
    }

    [Theory]
    [InlineAutoMoqData]
    public void CreatesProcessTestFileIfFileNotExists(
        [Frozen] Mock<IEnvironment> environment,
        [Frozen] Mock<IFileManager> fileManager,
        string name)
    {
        // Arrange

        name = $"{name}Tests";

        var simpleDataProvider = new Mock<IDataCollectionProvider<ModuleDescriptor, string>>();

        simpleDataProvider.Setup(x => x.Provide(It.IsAny<string>())).Returns(new List<ModuleDescriptor>());

        var testDescriptor = new TestDescriptor
        {
            Name = name,
            TestType = TestType.Process
        };

        fileManager.Setup(x => x.FileExists(It.IsAny<string>())).Returns(false);

        fileManager.Setup(x => x.ParentDirectoryExists(It.IsAny<string>())).Returns(true);

        fileManager.Setup(x => x.ParentDirectoryExists(It.IsAny<string>())).Returns(true);

        var sut = new TestDescriptorFileCreator(environment.Object, fileManager.Object, simpleDataProvider.Object);

        // Act
        sut.Create(testDescriptor);

        // Assert
        var definition = DescriptorDefinition(testDescriptor);

        fileManager.Verify(x => x.WriteAllText(It.IsAny<string>(), definition), Times.Once);

        fileManager.Verify(x => x.ParentDirectoryExists(It.IsAny<string>()), Times.Once);

        fileManager.Verify(x => x.CreateParentDirectory(It.IsAny<string>()), Times.Never);
    }

    [Theory]
    [InlineAutoMoqData]
    public void CreatesModuleTestFileIfFileNotExistsAndAddModuleUsing(
        [Frozen] Mock<IEnvironment> environment,
        [Frozen] Mock<IFileManager> fileManager,
        string name,
        string modulePath)
    {
        // Arrange
        var simpleDataProvider = new Mock<IDataCollectionProvider<ModuleDescriptor, string>>();

        simpleDataProvider.Setup(x => x.Provide(It.IsAny<string>())).Returns(new List<ModuleDescriptor>
        {
            new ModuleDescriptor 
            { 
                Name = name,
                Path = modulePath 
            }
        });

        name = $"{name}Tests";

        var testDescriptor = new TestDescriptor
        {
            Name = name,
            TestType = TestType.Module,
            Path = modulePath
        };

        fileManager.Setup(x => x.FileExists(It.IsAny<string>())).Returns(false);

        fileManager.Setup(x => x.ParentDirectoryExists(It.IsAny<string>())).Returns(true);

        var sut = new TestDescriptorFileCreator(environment.Object, fileManager.Object, simpleDataProvider.Object);

        // Act
        sut.Create(testDescriptor);

        // Assert
        var definition = DescriptorDefinition(testDescriptor);

        fileManager.Verify(x => x.WriteAllText(It.IsAny<string>(), definition), Times.Once);

        fileManager.Verify(x => x.ParentDirectoryExists(It.IsAny<string>()), Times.Once);

        fileManager.Verify(x => x.CreateParentDirectory(It.IsAny<string>()), Times.Never);
    }

    [Theory]
    [InlineAutoMoqData]
    public void CreatesModuleTestFileIfFileNotExistsAndDoesNotAddModuleUsing(
        [Frozen] Mock<IEnvironment> environment,
        [Frozen] Mock<IFileManager> fileManager,
        string name,
        string modulePath)
    {
        // Arrange
        var simpleDataProvider = new Mock<IDataCollectionProvider<ModuleDescriptor, string>>();

        simpleDataProvider.Setup(x => x.Provide(It.IsAny<string>())).Returns(new List<ModuleDescriptor>
        {
            new ModuleDescriptor
            {
                Name = name,
                Path = modulePath
            }
        });

        name = $"{name}Tests";

        var testDescriptor = new TestDescriptor
        {
            Name = name,
            TestType = TestType.Module,
            Path = modulePath
        };

        fileManager.Setup(x => x.FileExists(It.IsAny<string>())).Returns(false);

        fileManager.Setup(x => x.ParentDirectoryExists(It.IsAny<string>())).Returns(true);

        var sut = new TestDescriptorFileCreator(environment.Object, fileManager.Object, simpleDataProvider.Object);

        // Act
        sut.Create(testDescriptor);

        // Assert
        var definition = DescriptorDefinition(testDescriptor);

        fileManager.Verify(x => x.WriteAllText(It.IsAny<string>(), definition), Times.Once);

        fileManager.Verify(x => x.ParentDirectoryExists(It.IsAny<string>()), Times.Once);

        fileManager.Verify(x => x.CreateParentDirectory(It.IsAny<string>()), Times.Never);
    }

    [Theory]
    [InlineAutoMoqData]
    public void CreatesParentDirectoryIfNotExists(
        [Frozen] Mock<IEnvironment> environment,
        [Frozen] Mock<IFileManager> fileManager,
        string name)
    {
        // Arrange
        var testDescriptor = new TestDescriptor
        {
            Name = name,
        };

        fileManager.Setup(x => x.ParentDirectoryExists(It.IsAny<string>())).Returns(false);

        var simpleDataProvider = new Mock<IDataCollectionProvider<ModuleDescriptor, string>>();

        simpleDataProvider.Setup(x => x.Provide(It.IsAny<string>())).Returns(new List<ModuleDescriptor>());

        var sut = new TestDescriptorFileCreator(environment.Object, fileManager.Object, simpleDataProvider.Object);

        // Act
        sut.Create(testDescriptor);

        // Assert
        fileManager.Verify(x => x.ParentDirectoryExists(It.IsAny<string>()), Times.Once);

        fileManager.Verify(x => x.CreateParentDirectory(It.IsAny<string>()), Times.Once);
    }

    private static string DescriptorDefinition(TestDescriptor testDescriptor)
    {
        var sb = new StringBuilder();

        if (testDescriptor.TestType == TestType.Module)
        {
            sb.AppendLine($"using module \"..\\..\\Modules\\{testDescriptor.Path}\";");
            sb.AppendLine();
        }

        sb.AppendLine("# TODO: Write tests");
        sb.AppendLine();
        sb.AppendLine($"Describe '{testDescriptor.Name}' {"{"}");
        sb.AppendLine("    BeforeEach {");
        sb.AppendLine("    }");
        sb.AppendLine("    Context 'Example' {");
        sb.AppendLine("        It 'Will Pass' {");
        sb.AppendLine("        }");
        sb.AppendLine("    }");
        sb.AppendLine("}");

        return sb.ToString();
    }
}