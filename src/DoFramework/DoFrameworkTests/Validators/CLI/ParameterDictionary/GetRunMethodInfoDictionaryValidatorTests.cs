using DoFramework.Validators;
using FluentAssertions;
using System.Reflection;

namespace DoFrameworkTests.Validators;

public class GetRunMethodInfoDictionaryValidatorTests
{
    [Theory]
    [InlineAutoMoqData]
    public void GetRunMethodInfoDictionaryValidator_IsValid(
        Dictionary<string, object> dictionary,
        string methodName,
        Type type)
    {
        // Arrange
        dictionary.Add("methodName", methodName);
        dictionary.Add("type", type);

        var sut = new GetRunMethodInfoDictionaryValidator();

        // Act
        var result = sut.Validate(dictionary);

        // Assert
        result.IsValid.Should().BeTrue();
        result.Errors.Should().BeEmpty();
    }

    [Theory]
    [InlineAutoMoqData]
    public void GetRunMethodInfoDictionaryValidator_InValidMissingParam(Dictionary<string, object> dictionary)
    {
        // Arrange
        var sut = new GetRunMethodInfoDictionaryValidator();

        // Act
        var result = sut.Validate(dictionary);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().HaveCount(2);
        
        result.Errors.Should().Contain($"Required CLI Function parameter is invalid or missing: 'methodName'");
        result.Errors.Should().Contain($"Required CLI Function parameter is invalid or missing: 'type'");
    }

    [Theory]
    [InlineAutoMoqData]
    public void GetRunMethodInfoDictionaryValidator_InValidIncorrectTypes(
        Dictionary<string, object> dictionary,
        int value)
    {
        // Arrange
        var sut = new GetRunMethodInfoDictionaryValidator();
        dictionary.Add("methodName", value);
        dictionary.Add("type", value);

        // Act
        var result = sut.Validate(dictionary);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().HaveCount(2);

        result.Errors.Should().Contain($"Required CLI Function parameter is invalid or missing: 'methodName'");
        result.Errors.Should().Contain($"Required CLI Function parameter is invalid or missing: 'type'");
    }
}