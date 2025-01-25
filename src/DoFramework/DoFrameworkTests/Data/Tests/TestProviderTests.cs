using DoFramework.Data;
using DoFramework.Domain;
using FluentAssertions;
using Moq;

namespace DoFrameworkTests.Data;

public class TestProviderTests
{
    [Theory]
    [InlineAutoMoqData]
    public void TestProvider_DoesProvide(
        string filter,
        int descriptorCount)
    {
        // Arrange
        var projectContents = new ProjectContents();

        for (var i = 0; i < descriptorCount; i++)
        {
            projectContents.Tests.Add(new TestDescriptor
            {
                Name = filter
            });
        }

        var readProjectContents = new Mock<ISimpleDataProvider<ProjectContents>>();

        readProjectContents.Setup(x => x.Provide()).Returns(projectContents);

        var sut = new TestProvider(readProjectContents.Object);

        // Act
        var result = sut.Provide(filter);

        // Assert
        result.Should().HaveCount(descriptorCount);
    }
}