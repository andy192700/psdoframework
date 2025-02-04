using DoFramework.Data;
using DoFramework.Domain;
using FluentAssertions;
using Moq;

namespace DoFrameworkTests.Data;

public class ComposerProviderTests
{
    [Theory]
    [InlineAutoMoqData]
    public void ComposerProvider_DoesProvide(
        string filter,
        int descriptorCount)
    {
        // Arrange
        var projectContents = new ProjectContents();

        for (var i = 0; i < descriptorCount; i++)
        {
            projectContents.Composers.Add(new ComposerDescriptor
            {
                Name = filter
            });
        }

        var readProjectContents = new Mock<ISimpleDataProvider<ProjectContents>>();

        readProjectContents.Setup(x => x.Provide()).Returns(projectContents);

        var sut = new ComposerProvider(readProjectContents.Object);

        // Act
        var result = sut.Provide(filter);

        // Assert
        result.Should().HaveCount(descriptorCount);
    }
}