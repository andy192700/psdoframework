using AutoFixture.Xunit2;
using DoFramework.Data;
using DoFramework.Domain;
using DoFramework.Environment;
using FluentAssertions;
using Moq;

namespace DoFrameworkTests.Data;

public class ModuleResolverTests
{
    [Theory]
    [InlineAutoMoqData]
    public void ModuleResolve_ResolvesModule(
        [Frozen] Mock<IEnvironment> environment,
        string name,
        string path)
    {
        // Arrange
        var provider = new Mock<IDataCollectionProvider<ModuleDescriptor, string>>();

        var descriptor = new ModuleDescriptor
        { 
            Name = name, 
            Path = path 
        };

        var descriptors = new List<ModuleDescriptor>
        {
            descriptor
        };

        provider.Setup(x => x.Provide(name)).Returns(descriptors);

        var sut = new ModuleResolver(environment.Object, provider.Object);

        // Act
        var result = sut.Resolve(name);

        // Assert
        result.Should().NotBeNull();
        result.Exists.Should().BeTrue();
        result.Path.Should().Be($"{environment.Object.ModuleDir}{DoFramework.Environment.Environment.Separator}{path}");
        result.Descriptor.Should().Be(descriptor);
    }

    [Theory]
    [InlineAutoMoqData]
    public void ModuleResolve_DoesNotResolveModule(
        [Frozen] Mock<IEnvironment> environment,
        string name)
    {
        // Arrange
        var provider = new Mock<IDataCollectionProvider<ModuleDescriptor, string>>();

        provider.Setup(x => x.Provide(name)).Returns([]);

        var sut = new ModuleResolver(environment.Object, provider.Object);

        // Act
        var result = sut.Resolve(name);

        // Assert
        result.Should().NotBeNull();
        result.Exists.Should().BeFalse();
        result.Path.Should().BeNull();
        result.Descriptor.Should().BeNull();
    }
}