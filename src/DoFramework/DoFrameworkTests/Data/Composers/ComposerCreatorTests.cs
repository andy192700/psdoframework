using DoFramework.Data;
using DoFramework.Domain;
using DoFramework.Logging;
using FluentAssertions;
using Moq;

namespace DoFrameworkTests.Data;

public class ComposerCreatorTests
{
    [Theory]
    [InlineAutoMoqData]
    public void ComposerCreator_CreatesModule(
        string name,
        string path)
    {
        // Arrange
        var saveProjectContents = new Mock<IDataCreator<ProjectContents>>();

        var readProjectContents = new Mock<ISimpleDataProvider<ProjectContents>>();

        var projectContents = new ProjectContents();

        readProjectContents.Setup(x => x.Provide()).Returns(projectContents);

        var logger = new Mock<ILogger>();

        var sut = new ComposerCreator(saveProjectContents.Object, readProjectContents.Object, logger.Object);

        var descriptor = new ComposerDescriptor
        {
            Name = name,
            Path = path
        };

        // Act
        sut.Create(descriptor);

        // Assert
        projectContents.Processes.Should().HaveCount(0);
        projectContents.Modules.Should().HaveCount(0);
        projectContents.Composers.Should().HaveCount(1);
        projectContents.Tests.Should().HaveCount(0);

        readProjectContents.Verify(x => x.Provide(), Times.Once);

        saveProjectContents.Verify(x => x.Create(It.IsAny<ProjectContents>()), Times.Once);

        logger.Verify(x => x.LogInfo($"Composer registered at {descriptor.Name} with path {descriptor.Path}"), Times.Once);
    }
}