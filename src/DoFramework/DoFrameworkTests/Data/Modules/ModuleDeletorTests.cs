using DoFramework.Data;
using DoFramework.Domain;
using DoFramework.Environment;
using DoFramework.FileSystem;
using DoFramework.Logging;
using Moq;

namespace DoFrameworkTests.Data;

public class ModuleDeletorTests
{
    [Theory]
    [InlineAutoMoqData]
    public void ModuleDeletor_CannotDeleteTest(
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

        var sut = new ModuleDeletor(
            saveProjectContents.Object,
            readProjectContents.Object,
            environment.Object,
            fileManager.Object,
            logger.Object);

        var descriptor = new ModuleDescriptor
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

        logger.Verify(x => x.LogWarning($"Module: {descriptor.Name} could not be located for removal."), Times.Once);
    }

    [Theory]
    [InlineAutoMoqData]
    public void ModuleDeletor_DeletesTest(
        string name,
        string path)
    {
        // Arrange
        var saveProjectContents = new Mock<IDataCreator<ProjectContents>>();

        var readProjectContents = new Mock<ISimpleDataProvider<ProjectContents>>();

        var projectContents = new ProjectContents();

        var descriptor = new ModuleDescriptor
        {
            Name = name,
            Path = path
        };

        projectContents.Modules.Add(descriptor);

        readProjectContents.Setup(x => x.Provide()).Returns(projectContents);

        var environment = new Mock<IEnvironment>();

        var fileManager = new Mock<IFileManager>();

        var logger = new Mock<ILogger>();

        var sut = new ModuleDeletor(
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

        logger.Verify(x => x.LogInfo($"Module: {descriptor.Name} removed."), Times.Once);

        logger.Verify(x => x.LogWarning(It.IsAny<string>()), Times.Never);
    }
}