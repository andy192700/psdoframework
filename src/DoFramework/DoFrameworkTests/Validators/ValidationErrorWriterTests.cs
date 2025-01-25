using AutoFixture.Xunit2;
using DoFramework.Logging;
using DoFramework.Validators;
using Moq;

namespace DoFrameworkTests.Validators;

public class ValidationErrorWriterTests
{
    [Theory]
    [InlineAutoMoqData]
    public void ValidationErrorWriter_DoesLogErrors(
        ValidationResult validationResult,
        [Frozen] Mock<ILogger> mockLogger)
    { 
        // Arrange
        var sut = new ValidationErrorWriter(mockLogger.Object);

        // Act
        sut.Write(validationResult);

        // Assert
        mockLogger.Verify(x => x.LogError(It.IsAny<string>()), Times.Exactly(validationResult.Errors.Count));
    }
}