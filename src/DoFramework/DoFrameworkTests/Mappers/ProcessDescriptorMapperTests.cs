using AutoFixture.Xunit2;
using DoFramework.Domain;
using DoFramework.FileSystem;
using DoFramework.Mappers;
using FluentAssertions;
using Moq;

namespace DoFrameworkTests.Mappers;

public class ProcessDescriptorMapperTests
{
    private string Prefix = $"path{DoFramework.Environment.Environment.Separator}to{DoFramework.Environment.Environment.Separator}descriptor{DoFramework.Environment.Environment.Separator}";
    private string Suffix = ".psm1";

    [Theory]
    [InlineAutoMoqData]
    public void ProcessDescriptorMapper_CanMapDescendingPath(
        [Frozen] Mock<IOSSanitise> osSanitise, 
        string name)
    {
        // Arrange
        var path = $"{Prefix}{name}{Suffix}";

        osSanitise.Setup(x => x.Sanitise(It.IsAny<string>())).Returns(path);

        var sut = new ProcessDescriptorMapper(osSanitise.Object);

        // Act
        var result = sut.Map(path);

        // Assert
        result.Should().NotBeNull();

        result.GetType().Should().Be(typeof(ProcessDescriptor));

        result.Name.Should().Be(name);

        result.Path.Should().Be(path);
    }

    [Theory]
    [InlineAutoMoqData]
    public void ProcessDescriptorMapper_CanMapLocalPath(
        [Frozen] Mock<IOSSanitise> osSanitise, 
        string name)
    {
        // Arrange
        var path = $"{name}{Suffix}";

        osSanitise.Setup(x => x.Sanitise(It.IsAny<string>())).Returns(path);

        var sut = new ProcessDescriptorMapper(osSanitise.Object);

        // Act
        var result = sut.Map(path);

        // Assert
        result.Should().NotBeNull();

        result.GetType().Should().Be(typeof(ProcessDescriptor));

        result.Name.Should().Be(name);

        result.Path.Should().Be(path);
    }
}