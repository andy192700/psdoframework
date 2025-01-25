using AutoFixture.Xunit2;
using DoFramework.Domain;
using DoFramework.Environment;
using DoFramework.FileSystem;
using Moq;
using System.Text;

namespace DoFrameworkTests.FileSystem;

public class ProcessDescriptorFileCreatorTests
{
    [Theory]
    [InlineAutoMoqData]
    public void DoesNotCreateFileIfFileExists(
        [Frozen] Mock<IEnvironment> environment,
        [Frozen] Mock<IFileManager> fileManager,
        string name)
    {
        // Arrange
        var processDescriptor = new ProcessDescriptor
        {
            Name = name,
        };

        fileManager.Setup(x => x.ParentDirectoryExists(It.IsAny<string>())).Returns(true);

        fileManager.Setup(x => x.FileExists(It.IsAny<string>())).Returns(true);

        var sut = new ProcessDescriptorFileCreator(environment.Object, fileManager.Object);

        // Act
        sut.Create(processDescriptor);

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
        string name)
    {
        // Arrange
        var processDescriptor = new ProcessDescriptor
        {
            Name = name,
        };

        fileManager.Setup(x => x.ParentDirectoryExists(It.IsAny<string>())).Returns(true);

        fileManager.Setup(x => x.FileExists(It.IsAny<string>())).Returns(false);

        var sut = new ProcessDescriptorFileCreator(environment.Object, fileManager.Object);

        // Act
        sut.Create(processDescriptor);

        // Assert
        var definition = DescriptorDefinition(processDescriptor);

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
        var processDescriptor = new ProcessDescriptor
        {
            Name = name,
        };

        fileManager.Setup(x => x.ParentDirectoryExists(It.IsAny<string>())).Returns(false);

        var sut = new ProcessDescriptorFileCreator(environment.Object, fileManager.Object);

        // Act
        sut.Create(processDescriptor);

        // Assert
        fileManager.Verify(x => x.ParentDirectoryExists(It.IsAny<string>()), Times.Once);

        fileManager.Verify(x => x.CreateParentDirectory(It.IsAny<string>()), Times.Once);
    }

    private static string DescriptorDefinition(ProcessDescriptor processDescriptor)
    {
        var sb = new StringBuilder();

        sb.AppendLine("using namespace DoFramework.Processing;");
        sb.AppendLine();
        sb.AppendLine($"class {processDescriptor.Name} : Process {"{"}");
        sb.AppendLine("    [void] Run() {");
        sb.AppendLine("        # TODO: implement process");
        sb.AppendLine("    }");
        sb.AppendLine("}");

        return sb.ToString();
    }
}