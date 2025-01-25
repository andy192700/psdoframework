using DoFramework.Data;
using DoFramework.Environment;
using DoFramework.FileSystem;
using FluentAssertions;
using Moq;

namespace DoFrameworkTests.Data;

public class EnvFileDataProviderTests
{
    [Fact]
    public void EnvFileDataProvider_NoFilesNoData()
    {
        // Arrange
        var environment = new Mock<IEnvironment>();

        var filemanager = new Mock<IFileManager>();

        filemanager.Setup(x => x.GetFiles(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<SearchOption>())).Returns([]);  

        var sut = new EnvFileDataProvider(environment.Object, filemanager.Object);

        // Act
        var result = sut.Provide();

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(0);
    }

    [Theory]
    [InlineAutoMoqData]
    public void EnvFileDataProvider_ReturnsData(
        string path,
        Dictionary<string, string> dict)
    {
        // Arrange
        var environment = new Mock<IEnvironment>();

        var filemanager = new Mock<IFileManager>();

        filemanager.Setup(x => x.GetFiles(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<SearchOption>())).Returns([ new FileInfo(path) ]);

        var strings = new List<string>();

        foreach (var key in dict.Keys)
        {
            strings.Add($"{key}={dict[key]}");
        }

        filemanager.Setup(x => x.ReadAllLines(It.IsAny<string>())).Returns(strings.ToArray());

        var sut = new EnvFileDataProvider(environment.Object, filemanager.Object);

        // Act
        var result = sut.Provide();

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(dict.Count);
    }
}