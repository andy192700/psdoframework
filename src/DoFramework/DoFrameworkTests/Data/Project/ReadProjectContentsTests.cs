using DoFramework.Data;
using DoFramework.Domain;
using DoFramework.FileSystem;
using DoFramework.Mappers;
using FluentAssertions;
using Moq;

namespace DoFrameworkTests.Data;

public class ReadProjectContentsTests
{
    [Theory]
    [InlineAutoMoqData]
    public void ReadProjectContents_ProvidesProject(
        string serialisedData,
        ProjectContentsStorage projectContentsStorage)
    {
        // Arrange
        var projectContents = new ProjectContents();

        var filemanager = new Mock<IFileManager>();

        filemanager.Setup(x => x.ReadAllText(Path.Combine(System.Environment.CurrentDirectory, "do.json")))
            .Returns(serialisedData);

        var converter = new Mock<IJsonConverter>();

        converter.Setup(x => x.Deserialize<ProjectContentsStorage>(serialisedData))
            .Returns(projectContentsStorage);

        var mapper = new Mock<IMapper<ProjectContentsStorage, ProjectContents>>();

        mapper.Setup(x => x.Map(projectContentsStorage)).Returns(projectContents);

        var readLocation = new Mock<IReadProcessLocation>();

        readLocation.Setup(x => x.Read()).Returns(System.Environment.CurrentDirectory);

        var sut = new ReadProjectContents(mapper.Object,
            readLocation.Object,
            filemanager.Object,
            converter.Object);

        // Act
        var result = sut.Provide();

        // Assert
        result.Should().Be(projectContents);
    }
}