using DoFramework.Validators;
using FluentAssertions;

namespace DoFrameworkTests.Validators;

public class EmptyCLIFunctionDictionaryValidatorTests
{
    [Theory]
    [InlineAutoMoqData]
    public void EmptyCLIFunctionDictionaryValidatorTests_IsValid(Dictionary<string, object> dictionary)
    {
        // Arrange
        var sut = new EmptyCLIFunctionDictionaryValidator();

        // Act
        var result = sut.Validate(dictionary);

        // Assert
        result.IsValid.Should().BeTrue();
        result.Errors.Should().BeEmpty();
    }
}