using DoFramework.Data;
using DoFramework.Domain;
using DoFramework.Environment;
using DoFramework.FileSystem;
using DoFramework.Logging;
using DoFramework.Mappers;
using Moq;

namespace DoFrameworkTests.Data;

public class SaveProjectContentsTests
{
    [Theory]
    [InlineAutoMoqData]
    public void SaveProjectContents_SavesProjectContents(
        ProjectContentsStorage projectContentsStorage,
        string serialisedString)
    {
        // Arrange
        var projectContents = new ProjectContents();

        var environment = new Mock<IEnvironment>();

        environment.Setup(x => x.HomeDir).Returns(System.Environment.CurrentDirectory);

        var filemanager = new Mock<IFileManager>();

        var logger = new Mock<ILogger>();

        var mapper = new Mock<IMapper<ProjectContents, ProjectContentsStorage>>();

        mapper.Setup(x => x.Map(It.IsAny<ProjectContents>())).Returns(projectContentsStorage);

        var converter = new Mock<IJsonConverter>();

        converter.Setup(x => x.Serialize(It.IsAny<object>())).Returns(serialisedString);

        var sut = new SaveProjectContents(
            environment.Object, 
            logger.Object,
            mapper.Object,
            filemanager.Object,
            converter.Object);

        // Act
        sut.Create(projectContents);

        // Assert
        filemanager.Verify(x => x.WriteAllText(Path.Combine(System.Environment.CurrentDirectory, "do.json"), serialisedString), Times.Once);

        logger.Verify(x => x.LogInfo("Project file updated."), Times.Once);
    }
}