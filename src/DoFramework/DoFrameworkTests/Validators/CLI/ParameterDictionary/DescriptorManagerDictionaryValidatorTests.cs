using DoFramework.Validators;
using FluentAssertions;

namespace DoFrameworkTests.Validators;

public class DescriptorManagerDictionaryValidatorTests
{
    [Theory]
    [InlineAutoMoqData(typeof(DescriptorManagementDictionaryValidator), "name")]
    public void DescriptorManagerDictionaryValidator_IsValid(Type typeValidator, string expectedParamName)
    {
        // Arrange
        var sut = (CLIFunctionDictionaryValidator)Activator.CreateInstance(typeValidator)!;

        var dictionary = new Dictionary<string, object>
        {
            { expectedParamName, expectedParamName }
        };

        // Act
        var result = sut.Validate(dictionary);

        // Assert
        result.IsValid.Should().BeTrue();
        result.Errors.Should().BeEmpty();
    }

    [Theory]
    [InlineAutoMoqData(typeof(DescriptorManagementDictionaryValidator), "name")]
    public void DescriptorManagerDictionaryValidator_InValidMissingParam(Type typeValidator, string expectedParamName)
    {
        // Arrange
        var sut = (CLIFunctionDictionaryValidator)Activator.CreateInstance(typeValidator)!;

        var dictionary = new Dictionary<string, object>();

        // Act
        var result = sut.Validate(dictionary);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().HaveCount(1);

        result.Errors[0].Should().Be($"Required CLI Function parameter is invalid or missing: '{expectedParamName}'");
    }


    [Theory]
    [InlineAutoMoqData(typeof(DescriptorManagementDictionaryValidator), "name")]
    public void DescriptorManagerDictionaryValidator_InValidIncorrectType(Type typeValidator, string expectedParamName, int value)
    {
        // Arrange
        var sut = (CLIFunctionDictionaryValidator)Activator.CreateInstance(typeValidator)!;

        var dictionary = new Dictionary<string, object>
        {
            { expectedParamName, value }
        };

        // Act
        var result = sut.Validate(dictionary);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().HaveCount(1);

        result.Errors[0].Should().Be($"Required CLI Function parameter is invalid or missing: '{expectedParamName}'");
    }
}