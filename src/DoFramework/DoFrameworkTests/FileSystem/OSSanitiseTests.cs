using DoFramework.FileSystem;
using FluentAssertions;

namespace DoFrameworkTests.FileSystem;

public class OSSanitiseTests
{
    [Theory]
    [InlineAutoMoqData]
    public void Sanitiser_SanitisesPath(
        string someString,
        OSSanitise sut)
    {
        // Arrange
        var sep = DoFramework.Environment.Environment.Separator;

        var inputPath = $"{someString}\\{someString}/{someString}\\{someString}/{someString}/{someString}";

        // Act
        var result = sut.Sanitise(inputPath);

        // Assert
        var expectedPath = $"{someString}{sep}{someString}{sep}{someString}{sep}{someString}{sep}{someString}{sep}{someString}";

        result.Should().Be(expectedPath);
    }
}
