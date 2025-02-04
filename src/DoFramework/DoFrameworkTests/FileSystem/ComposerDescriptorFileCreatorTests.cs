using AutoFixture.Xunit2;
using DoFramework.Domain;
using DoFramework.Environment;
using DoFramework.FileSystem;
using Moq;
using System.Text;

namespace DoFrameworkTests.FileSystem;

public class ComposerDescriptorFileCreatorTests
{
    [Theory]
    [InlineAutoMoqData]
    public void DoesNotCreateFileIfFileExists(
        [Frozen] Mock<IEnvironment> environment,
        [Frozen] Mock<IFileManager> fileManager,
        ComposerDescriptor ComposerDescriptor
    )
    {
        // Arrange
        fileManager.Setup(x => x.FileExists(It.IsAny<string>())).Returns(true);

        fileManager.Setup(x => x.ParentDirectoryExists(It.IsAny<string>())).Returns(true);

        var sut = new ComposerDescriptorFileCreator(environment.Object, fileManager.Object);

        // Act
        sut.Create(ComposerDescriptor);

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
        ComposerDescriptor ComposerDescriptor
    )
    {
        // Arrange
        fileManager.Setup(x => x.FileExists(It.IsAny<string>())).Returns(false);

        fileManager.Setup(x => x.ParentDirectoryExists(It.IsAny<string>())).Returns(true);

        var sut = new ComposerDescriptorFileCreator(environment.Object, fileManager.Object);

        // Act
        sut.Create(ComposerDescriptor);

        // Assert
        var definition = DescriptorDefinition(ComposerDescriptor);

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
        var ComposerDescriptor = new ComposerDescriptor
        {
            Name = name,
        };

        fileManager.Setup(x => x.ParentDirectoryExists(It.IsAny<string>())).Returns(false);

        var sut = new ComposerDescriptorFileCreator(environment.Object, fileManager.Object);

        // Act
        sut.Create(ComposerDescriptor);

        // Assert
        fileManager.Verify(x => x.ParentDirectoryExists(It.IsAny<string>()), Times.Once);

        fileManager.Verify(x => x.CreateParentDirectory(It.IsAny<string>()), Times.Once);
    }

    private static string DescriptorDefinition(ComposerDescriptor composerDescriptor)
    {
        var sb = new StringBuilder();

        sb.AppendLine("using namespace DoFramework.Processing;");
        sb.AppendLine();
        sb.AppendLine($"class {composerDescriptor.Name} : IComposer {"{"}");
        sb.AppendLine("    [void] Compose([IComposerWorkBench] $workBench) {");
        sb.AppendLine("        # TODO: implement composer");
        sb.AppendLine("    }");
        sb.AppendLine("}");

        return sb.ToString();
    }
}