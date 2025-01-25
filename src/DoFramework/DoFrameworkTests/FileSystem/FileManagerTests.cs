using DoFramework.FileSystem;
using FluentAssertions;
using Microsoft.VisualStudio.TestPlatform.Common.DataCollection;
using System.Text;

namespace DoFrameworkTests.FileSystem;

public class FileManagerTests
{
    private string TestDirectory = $"{System.Environment.CurrentDirectory}{DoFramework.Environment.Environment.Separator}";

    [Theory]
    [InlineAutoMoqData]
    public void FileShouldNotExist(string fileName)
    {
        // Arrange
        var sut = new FileManager();

        // Act
        var result = sut.FileExists($"{TestDirectory}{fileName}");

        // Assert
        result.Should().BeFalse();
    }

    [Theory]
    [InlineAutoMoqData]
    public void WriteAllTextCreatesAFile(string data, string fileName)
    {
        // Arrange
        var sut = new FileManager();

        // Act
        sut.WriteAllText($"{TestDirectory}{fileName}", data);

        var result = sut.FileExists($"{TestDirectory}{fileName}");

        sut.DeleteFile($"{TestDirectory}{fileName}");

        // Assert
        result.Should().BeTrue();
    }

    [Theory]
    [InlineAutoMoqData]
    public void CanDeleteFile(string data, string fileName)
    {
        // Arrange
        var sut = new FileManager();

        // Act
        sut.WriteAllText($"{TestDirectory}{fileName}", data);
        sut.DeleteFile($"{TestDirectory}{fileName}");

        var result = sut.FileExists($"{TestDirectory}{fileName}");

        // Assert
        result.Should().BeFalse();
    }

    [Theory]
    [InlineAutoMoqData]
    public void ReadAllTextReturnsData(string data, string fileName)
    {
        // Arrange
        var sut = new FileManager();

        // Act
        sut.WriteAllText($"{TestDirectory}{fileName}", data);

        var result = sut.ReadAllText($"{TestDirectory}{fileName}");

        sut.DeleteFile($"{TestDirectory}{fileName}");

        // Assert
        result.Should().Be(data);
    }

    [Theory]
    [InlineAutoMoqData]
    public void ReadAllLinesReturnsData(string line1, string line2, string fileName)
    {
        // Arrange
        var sut = new FileManager();

        var sb = new StringBuilder();
        sb.AppendLine(line1);
        sb.AppendLine(line2);

        // Act
        sut.WriteAllText($"{TestDirectory}{fileName}", sb.ToString());

        var result = sut.ReadAllLines($"{TestDirectory}{fileName}");

        sut.DeleteFile($"{TestDirectory}{fileName}");

        // Assert
        result.Should().HaveCount(2);
        result[0].Should().Be(line1);
        result[1].Should().Be(line2);
    }

    [Theory]
    [InlineAutoMoqData]
    public void GetFileReturnsFileExists(string data, string fileName)
    {
        // Arrange
        var sut = new FileManager();

        // Act
        sut.WriteAllText($"{TestDirectory}{fileName}", data);

        var result = sut.GetFileInfo($"{TestDirectory}{fileName}");

        // Assert
        result.Should().NotBeNull();
        result.Exists.Should().BeTrue();

        sut.DeleteFile($"{TestDirectory}{fileName}");
    }

    [Theory]
    [InlineAutoMoqData]
    public void GetFileReturnsFileNotExists(string fileName)
    {
        // Arrange
        var sut = new FileManager();

        // Act
        var result = sut.GetFileInfo($"{TestDirectory}{fileName}");

        // Assert
        result.Should().NotBeNull();
        result.Exists.Should().BeFalse();
    }

    [Theory]
    [InlineAutoMoqData]
    public void GetFilesContainsCreatedFile(string data, string fileName)
    {
        // Arrange
        var sut = new FileManager();

        sut.WriteAllText($"{TestDirectory}{fileName}", data);

        // Act
        var result = sut.GetFiles(System.Environment.CurrentDirectory, "*", SearchOption.AllDirectories);

        // Assert
        result.Should().NotBeNull();
        result.Count().Should().BeGreaterThan(0);
        result.Any(x => x.Name.Equals(fileName)).Should().BeTrue();

        sut.DeleteFile($"{TestDirectory}{fileName}");
    }

    [Theory]
    [InlineAutoMoqData]
    public void GetFilesDoesNotContainFile(string fileName)
    {
        // Arrange
        var sut = new FileManager();

        // Act
        var result = sut.GetFiles(System.Environment.CurrentDirectory, "*", SearchOption.AllDirectories);

        // Assert
        result.Should().NotBeNull();
        result.Count().Should().BeGreaterThan(0);
        result.Any(x => x.Name.Equals(fileName)).Should().BeFalse();
    }

    [Theory]
    [InlineAutoMoqData]
    public void ParentDirectoryExists(string fileName)
    {
        // Arrange
        var sut = new FileManager();

        var path = $"{System.Environment.CurrentDirectory}{DoFramework.Environment.Environment.Separator}{fileName}";

        // Act
        var result = sut.ParentDirectoryExists(path);

        // Assert
        result.Should().BeTrue();
    }

    [Theory]
    [InlineAutoMoqData]
    public void ParentDirectoryDoesNotExist(string fileName)
    {
        // Arrange
        var sut = new FileManager();

        var path = $"{System.Environment.CurrentDirectory}{DoFramework.Environment.Environment.Separator}{fileName}{DoFramework.Environment.Environment.Separator}{fileName}";

        // Act
        var result = sut.ParentDirectoryExists(path);

        // Assert
        result.Should().BeFalse();
    }

    [Theory]
    [InlineAutoMoqData]
    public void CanCreateParentDirectoryNested(string fileName)
    {
        // Arrange
        var sut = new FileManager();

        var sep = DoFramework.Environment.Environment.Separator;

        var path = $"{System.Environment.CurrentDirectory}{sep}{fileName}{sep}{fileName}{sep}{fileName}";

        var before = sut.ParentDirectoryExists(path);

        // Act
        sut.CreateParentDirectory(path);

        var result = sut.ParentDirectoryExists(path);

        // Assert
        result.Should().BeTrue();
        before.Should().BeFalse();

        Directory.Delete($"{System.Environment.CurrentDirectory}{sep}{fileName}{sep}{fileName}");

        Directory.Delete($"{System.Environment.CurrentDirectory}{sep}{fileName}");
    }
}