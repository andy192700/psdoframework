using DoFramework.Testing;
using DoFramework.Validators;
using FluentAssertions;

namespace DoFrameworkTests.Validators;

public class TestRunnerDictionaryValidatorTests
{
    [Theory]
    [InlineAutoMoqData]
    public void TestRunnerDictionaryValidator_IsValidNoOptional(
        Dictionary<string, object> dictionary, 
        string filter)
    {
        // Arrange
        var sut = new TestRunnerDictionaryValidator();

        dictionary.Add("filter", filter);

        // Act
        var result = sut.Validate(dictionary);

        // Assert
        result.IsValid.Should().BeTrue();
        result.Errors.Should().HaveCount(0);
    }

    [Theory]
    [InlineAutoMoqData]
    public void TestRunnerDictionaryValidator_IsValidWithOptional(
        Dictionary<string, object> dictionary,
        string filter,
        PesterOutputType pesterOutputType)
    {
        // Arrange
        var sut = new TestRunnerDictionaryValidator();

        dictionary.Add("filter", filter);
        dictionary.Add("outputFormat", pesterOutputType.ToString());

        // Act
        var result = sut.Validate(dictionary);

        // Assert
        result.IsValid.Should().BeTrue();
        result.Errors.Should().HaveCount(0);
    }

    [Theory]
    [InlineAutoMoqData]
    public void TestRunnerDictionaryValidator_IsValidMissingParams(
        Dictionary<string, object> dictionary)
    {
        // Arrange
        var sut = new TestRunnerDictionaryValidator();

        // Act
        var result = sut.Validate(dictionary);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().HaveCount(1);

        result.Errors[0].Should().Be($"Required CLI Function parameter is invalid or missing: 'filter'");
    }

    [Theory]
    [InlineAutoMoqData]
    public void TestRunnerDictionaryValidator_InValidIncorrectValues(
        Dictionary<string, object> dictionary,
        int value)
    {
        // Arrange
        var sut = new TestRunnerDictionaryValidator();

        dictionary.Add("filter", value);
        dictionary.Add("outputFormat", value);

        // Act
        var result = sut.Validate(dictionary);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().HaveCount(2);

        result.Errors.Should().Contain($"Required CLI Function parameter is invalid or missing: 'filter'");
        result.Errors.Should().Contain($"Optional CLI Function parameter is invalid: 'outputFormat'");
    }
}