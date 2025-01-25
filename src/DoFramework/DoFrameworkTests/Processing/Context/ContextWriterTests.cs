using AutoFixture.Xunit2;
using DoFramework.Processing;
using Moq;

namespace DoFrameworkTests.Processing;

public class ContextWriterTests
{
    [Theory]
    [InlineAutoMoqData]
    public void ContextWriter_DoesWriteToContext(
        [Frozen] Mock<IContext> context,
        Dictionary<string, object> dictionary)
    {
        // Arrange
        var sut = new ContextWriter(context.Object);

        // Act
        sut.Write(dictionary);

        // Assert
        context.Verify(x => x.AddOrUpdate(It.IsAny<string>(), It.IsAny<object>()), Times.Exactly(dictionary.Count));
    }
}