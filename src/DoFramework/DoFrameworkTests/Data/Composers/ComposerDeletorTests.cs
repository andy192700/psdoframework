using DoFramework.Data;
using DoFramework.Domain;
using DoFramework.Environment;
using DoFramework.FileSystem;
using DoFramework.Logging;
using Moq;

namespace DoFrameworkTests.Data;

public class ComposerDeletorTests
{
    [Theory]
    [InlineAutoMoqData]
    public void ComposerDeletor_CannotDeleteTest(
        string name,
        string path)
    {
        // Arrange
        var saveProjectContents = new Mock<IDataCreator<ProjectContents>>();

        var readProjectContents = new Mock<ISimpleDataProvider<ProjectContents>>();

        readProjectContents.Setup(x => x.Provide()).Returns(new ProjectContents());

        var environment = new Mock<IEnvironment>();

        var fileManager = new Mock<IFileManager>();

        var logger = new Mock<ILogger>();

        var sut = new ComposerDeletor(
            saveProjectContents.Object,
            readProjectContents.Object,
            environment.Object,
            fileManager.Object,
            logger.Object);

        var descriptor = new ComposerDescriptor
        {
            Name = name,
            Path = path
        };

        // Act
        sut.Delete(descriptor);

        // Assert
        readProjectContents.Verify(x => x.Provide(), Times.Once);

        saveProjectContents.Verify(x => x.Create(It.IsAny<ProjectContents>()), Times.Never);

        fileManager.Verify(x => x.DeleteFile(It.IsAny<string>()), Times.Never);

        logger.Verify(x => x.LogInfo(It.IsAny<string>()), Times.Never);

        logger.Verify(x => x.LogWarning($"Composer: {descriptor.Name} could not be located for removal."), Times.Once);
    }

    [Theory]
    [InlineAutoMoqData]
    public void ComposerDeletor_DeletesTest(
        string name,
        string path)
    {
        // Arrange
        var saveProjectContents = new Mock<IDataCreator<ProjectContents>>();

        var readProjectContents = new Mock<ISimpleDataProvider<ProjectContents>>();

        var projectContents = new ProjectContents();

        var descriptor = new ComposerDescriptor
        {
            Name = name,
            Path = path
        };

        projectContents.Composers.Add(descriptor);

        readProjectContents.Setup(x => x.Provide()).Returns(projectContents);

        var environment = new Mock<IEnvironment>();

        var fileManager = new Mock<IFileManager>();

        var logger = new Mock<ILogger>();

        var sut = new ComposerDeletor(
            saveProjectContents.Object,
            readProjectContents.Object,
            environment.Object,
            fileManager.Object,
            logger.Object);

        // Act
        sut.Delete(descriptor);

        // Assert
        readProjectContents.Verify(x => x.Provide(), Times.Once);

        saveProjectContents.Verify(x => x.Create(It.IsAny<ProjectContents>()), Times.Once);

        fileManager.Verify(x => x.DeleteFile(It.IsAny<string>()), Times.Once);

        logger.Verify(x => x.LogInfo($"Composer: {descriptor.Name} removed."), Times.Once);

        logger.Verify(x => x.LogWarning(It.IsAny<string>()), Times.Never);
    }
}