using AutoFixture.Xunit2;
using DoFramework.Processing;
using FluentAssertions;
using Moq;
using System.Text;

namespace DoFrameworkTests.Processing;

public class ComputeHierarchyPrefixTests
{
    private const string Prefix = "--";

    [Theory]
    [InlineAutoMoqData]
    public void ComputeHierarchyPrefix_CorrectDepth(
        [Frozen] Mock<IContext> contextMock,
        int processCount)
    {
        // Arrange
        var session = new Session
        {
            ProcessCount = processCount
        };

        contextMock.Setup(x => x.Session).Returns(session);

        var sut = new ComputeHierarchyPrefix(contextMock.Object);

        // Act
        var result = sut.Compute();

        // Assert
        result.Should().Be(ComputePrefix(processCount - 1));
    }

    private static string ComputePrefix(int depth)
    {
        var sb = new StringBuilder();

        for (int i = 0; i < depth; i++) 
        {
            sb.Append(Prefix);
        }

        return sb.ToString();
    }
}