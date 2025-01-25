using DoFramework.Data;
using DoFramework.Domain;
using DoFramework.Logging;
using FluentAssertions;
using Moq;

namespace DoFrameworkTests.Data;

public class ProcessCreatorTests
{
    [Theory]
    [InlineAutoMoqData]
    public void ProcessCreator_CreatesProcess(
        string name,
        string path)
    {
        // Arrange
        var saveProjectContents = new Mock<IDataCreator<ProjectContents>>();

        var readProjectContents = new Mock<ISimpleDataProvider<ProjectContents>>();

        var projectContents = new ProjectContents();

        readProjectContents.Setup(x => x.Provide()).Returns(projectContents);

        var logger = new Mock<ILogger>();

        var sut = new ProcessCreator(saveProjectContents.Object, readProjectContents.Object, logger.Object);

        var descriptor = new ProcessDescriptor
        {
            Name = name,
            Path = path
        };

        // Act
        sut.Create(descriptor);

        // Assert
        projectContents.Processes.Should().HaveCount(1);
        projectContents.Modules.Should().HaveCount(0);
        projectContents.Tests.Should().HaveCount(0);

        readProjectContents.Verify(x => x.Provide(), Times.Once);

        saveProjectContents.Verify(x => x.Create(It.IsAny<ProjectContents>()), Times.Once);

        logger.Verify(x => x.LogInfo($"Process registered at {descriptor.Name} with path {descriptor.Path}"), Times.Once);
    }
}