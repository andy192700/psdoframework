using AutoFixture.Xunit2;
using DoFramework.Data;
using DoFramework.Processing;
using Moq;

namespace DoFrameworkTests.Processing;

public class ConsumeEnvFilesTests
{
    [Theory]
    [InlineAutoMoqData]
    public void ConsumeEnvFiles_ConsumesProvidedEnvFileData(
        [Frozen] Mock<IContextWriter> contextWriter, 
        [Frozen] Mock<ISimpleDataProvider<Dictionary<string, object>>> envFileDataProvider,
        Dictionary<string, object> data)
    {
        // Arrange
        envFileDataProvider.Setup(x => x.Provide()).Returns(data);

        var sut = new ConsumeEnvFiles(contextWriter.Object, envFileDataProvider.Object);

        // Act
        sut.Consume();

        // Assert
        envFileDataProvider.Verify(x => x.Provide(), Times.Once);

        contextWriter.Verify(x => x.Write(data), Times.Once);
    }
}