using AutoFixture.Xunit2;
using DoFramework.Domain;
using DoFramework.Environment;
using DoFramework.FileSystem;
using Moq;
using System.Text;

namespace DoFrameworkTests.FileSystem;

public class ModuleDescriptorFileCreatorTests
{
    [Theory]
    [InlineAutoMoqData]
    public void DoesNotCreateFileIfFileExists(
        [Frozen] Mock<IEnvironment> environment,
        [Frozen] Mock<IFileManager> fileManager,
        ModuleDescriptor moduleDescriptor
    )
    {
        // Arrange
        fileManager.Setup(x => x.FileExists(It.IsAny<string>())).Returns(true);

        fileManager.Setup(x => x.ParentDirectoryExists(It.IsAny<string>())).Returns(true);

        var sut = new ModuleDescriptorFileCreator(environment.Object, fileManager.Object);

        // Act
        sut.Create(moduleDescriptor);

        // Assert
        fileManager.Verify(x => x.WriteAllText(It.IsAny<string>(), It.IsAny<string>()), Times.Never);

        fileManager.Verify(x => x.ParentDirectoryExists(It.IsAny<string>()), Times.Once);

        fileManager.Verify(x => x.CreateParentDirectory(It.IsAny<string>()), Times.Never);
    }

    [Theory]
    [InlineAutoMoqData]
    public void CreatesFileIfFileNotExists(
        [Frozen] Mock<IEnvironment> environment,
        [Frozen] Mock<IFileManager> fileManager,
        ModuleDescriptor moduleDescriptor
    )
    {
        // Arrange
        fileManager.Setup(x => x.FileExists(It.IsAny<string>())).Returns(false);

        fileManager.Setup(x => x.ParentDirectoryExists(It.IsAny<string>())).Returns(true);

        var sut = new ModuleDescriptorFileCreator(environment.Object, fileManager.Object);

        // Act
        sut.Create(moduleDescriptor);

        // Assert
        var definition = DescriptorDefinition();

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
        var moduleDescriptor = new ModuleDescriptor
        {
            Name = name,
        };

        fileManager.Setup(x => x.ParentDirectoryExists(It.IsAny<string>())).Returns(false);

        var sut = new ModuleDescriptorFileCreator(environment.Object, fileManager.Object);

        // Act
        sut.Create(moduleDescriptor);

        // Assert
        fileManager.Verify(x => x.ParentDirectoryExists(It.IsAny<string>()), Times.Once);

        fileManager.Verify(x => x.CreateParentDirectory(It.IsAny<string>()), Times.Once);
    }

    private static string DescriptorDefinition()
    {
        var sb = new StringBuilder();

        sb.AppendLine("# TODO: Create classes and functions.");
        sb.AppendLine();
        sb.AppendLine("function ExampleFunction {");
        sb.AppendLine("}");
        sb.AppendLine();
        sb.AppendLine("Export-ModuleMember -Function ExampleFunction;");
        sb.AppendLine();
        sb.AppendLine("class ExampleClass {");
        sb.AppendLine("}");

        return sb.ToString();
    }
}