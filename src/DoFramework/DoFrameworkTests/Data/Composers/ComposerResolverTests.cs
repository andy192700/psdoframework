using AutoFixture.Xunit2;
using DoFramework.Data;
using DoFramework.Domain;
using DoFramework.Environment;
using FluentAssertions;
using Moq;

namespace DoFrameworkTests.Data;

public class ComposerResolverTests
{
    [Theory]
    [InlineAutoMoqData]
    public void ComposerResolve_ResolvesComposer(
        [Frozen] Mock<IEnvironment> environment,
        string name,
        string path)
    {
        // Arrange
        var provider = new Mock<IDataCollectionProvider<ComposerDescriptor, string>>();

        var descriptor = new ComposerDescriptor
        {
            Name = name,
            Path = path
        };

        var descriptors = new List<ComposerDescriptor>
        {
            descriptor
        };

        provider.Setup(x => x.Provide(name)).Returns(descriptors);

        var sut = new ComposerResolver(environment.Object, provider.Object);

        // Act
        var result = sut.Resolve(name);

        // Assert
        result.Should().NotBeNull();
        result.Exists.Should().BeTrue();
        result.Path.Should().Be($"{environment.Object.ComposersDir}{DoFramework.Environment.Environment.Separator}{path}");
        result.Descriptor.Should().Be(descriptor);
    }

    [Theory]
    [InlineAutoMoqData]
    public void ComposerResolve_DoesNotResolveComposer(
        [Frozen] Mock<IEnvironment> environment,
        string name)
    {
        // Arrange
        var provider = new Mock<IDataCollectionProvider<ComposerDescriptor, string>>();

        provider.Setup(x => x.Provide(name)).Returns([]);

        var sut = new ComposerResolver(environment.Object, provider.Object);

        // Act
        var result = sut.Resolve(name);

        // Assert
        result.Should().NotBeNull();
        result.Exists.Should().BeFalse();
        result.Path.Should().BeNull();
        result.Descriptor.Should().BeNull();
    }
}