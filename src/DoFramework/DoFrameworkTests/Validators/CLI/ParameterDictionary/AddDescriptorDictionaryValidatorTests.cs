using DoFramework.Validators;
using FluentAssertions;

namespace DoFrameworkTests.Validators;

public class AddDescriptorDictionaryValidatorTests
{

    [Theory]
    [InlineAutoMoqData]
    public void AddDescriptorDictionaryValidator_IsValid(string value)
    {
        // Arrange
        var parameters = new Dictionary<string, object>
        {
            { "name", value }
        };

        var sut = new DescriptorManagementDictionaryValidator();

        // Act
        var result = sut.Validate(parameters);

        // Assert
        result.IsValid.Should().BeTrue();

        result.Errors.Should().BeEmpty();
    }

    [Theory]
    [InlineAutoMoqData]
    public void AddDescriptorDictionaryValidator_InValidMissingParam(string name, string value)
    {
        // Arrange
        var parameters = new Dictionary<string, object>
        {
            { name, value }
        };

        var sut = new DescriptorManagementDictionaryValidator();

        // Act
        var result = sut.Validate(parameters);

        // Assert
        result.IsValid.Should().BeFalse();
        
        result.Errors[0].Should().Be($"Required CLI Function parameter is invalid or missing: 'name'");
    }

    [Fact]
    public void AddDescriptorDictionaryValidator_InValidNoParam()
    {
        // Arrange
        var parameters = new Dictionary<string, object>();

        var sut = new DescriptorManagementDictionaryValidator();

        // Act
        var result = sut.Validate(parameters);

        // Assert
        result.IsValid.Should().BeFalse();

        result.Errors[0].Should().Be($"Required CLI Function parameter is invalid or missing: 'name'");
    }

    [Theory]
    [InlineAutoMoqData]
    public void AddDescriptorDictionaryValidator_InValidInvalidParam(double value)
    {
        // Arrange
        var parameters = new Dictionary<string, object>
        {
            { "name", value }
        };

        var sut = new DescriptorManagementDictionaryValidator();

        // Act
        var result = sut.Validate(parameters);

        // Assert
        result.IsValid.Should().BeFalse();

        result.Errors[0].Should().Be($"Required CLI Function parameter is invalid or missing: 'name'");
    }
}