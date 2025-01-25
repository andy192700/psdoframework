using AutoFixture.Xunit2;
using DoFramework.Data;
using DoFramework.Domain;
using DoFramework.Environment;
using FluentAssertions;
using Moq;

namespace DoFrameworkTests.Data;

public class TestResolvertests
{
    [Theory]
    [InlineAutoMoqData]
    public void TestResolve_ResolvesTest(
        [Frozen] Mock<IEnvironment> environment,
        string name,
        string path)
    {
        // Arrange
        var provider = new Mock<IDataCollectionProvider<TestDescriptor, string>>();

        var descriptor = new TestDescriptor
        {
            Name = name,
            Path = path
        };

        var descriptors = new List<TestDescriptor>
        {
            descriptor
        };

        provider.Setup(x => x.Provide(name)).Returns(descriptors);

        var sut = new TestResolver(environment.Object, provider.Object);

        // Act
        var result = sut.Resolve(name);

        // Assert
        result.Should().NotBeNull();
        result.Exists.Should().BeTrue();
        result.Path.Should().Be($"{environment.Object.TestsDir}{DoFramework.Environment.Environment.Separator}{path}");
        result.Descriptor.Should().Be(descriptor);
    }

    [Theory]
    [InlineAutoMoqData]
    public void TestResolve_DoesNotResolveTest(
        [Frozen] Mock<IEnvironment> environment,
        string name)
    {
        // Arrange
        var provider = new Mock<IDataCollectionProvider<TestDescriptor, string>>();

        provider.Setup(x => x.Provide(name)).Returns([]);

        var sut = new TestResolver(environment.Object, provider.Object);

        // Act
        var result = sut.Resolve(name);

        // Assert
        result.Should().NotBeNull();
        result.Exists.Should().BeFalse();
        result.Path.Should().BeNull();
        result.Descriptor.Should().BeNull();
    }
}