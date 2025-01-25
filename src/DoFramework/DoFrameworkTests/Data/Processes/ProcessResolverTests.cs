using AutoFixture.Xunit2;
using DoFramework.Data;
using DoFramework.Domain;
using DoFramework.Environment;
using FluentAssertions;
using Moq;

namespace DoFrameworkTests.Data;

public class ProcessResolverTests
{
    [Theory]
    [InlineAutoMoqData]
    public void ProcessResolve_ResolvesProcess(
        [Frozen] Mock<IEnvironment> environment,
        string name,
        string path)
    {
        // Arrange
        var provider = new Mock<IDataCollectionProvider<ProcessDescriptor, string>>();

        var descriptor = new ProcessDescriptor
        {
            Name = name,
            Path = path
        };

        var descriptors = new List<ProcessDescriptor>
        {
            descriptor
        };

        provider.Setup(x => x.Provide(name)).Returns(descriptors);

        var sut = new ProcessResolver(environment.Object, provider.Object);

        // Act
        var result = sut.Resolve(name);

        // Assert
        result.Should().NotBeNull();
        result.Exists.Should().BeTrue();
        result.Path.Should().Be($"{environment.Object.ProcessesDir}{DoFramework.Environment.Environment.Separator}{path}");
        result.Descriptor.Should().Be(descriptor);
    }

    [Theory]
    [InlineAutoMoqData]
    public void ProcessResolve_DoesNotResolveProcess(
        [Frozen] Mock<IEnvironment> environment,
        string name)
    {
        // Arrange
        var provider = new Mock<IDataCollectionProvider<ProcessDescriptor, string>>();

        provider.Setup(x => x.Provide(name)).Returns([]);

        var sut = new ProcessResolver(environment.Object, provider.Object);

        // Act
        var result = sut.Resolve(name);

        // Assert
        result.Should().NotBeNull();
        result.Exists.Should().BeFalse();
        result.Path.Should().BeNull();
        result.Descriptor.Should().BeNull();
    }
}