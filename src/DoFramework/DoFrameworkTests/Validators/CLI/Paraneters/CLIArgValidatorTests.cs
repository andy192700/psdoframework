using DoFramework.Validators;
using FluentAssertions;

namespace DoFrameworkTests.Validators;

public class CLIArgValidatorTests
{
    private const string Examples = "Example calls: 'doing FUNCTIONNAME' 'doing FUNCTIONNAME -arg1 1 -arg2 2 -switch1 -switch2'";
    private const string EmptyParamsMsg = "Invalid doing function call - no parameters were supplied.";
    private const string InvalidFormatMsg = "Invalid doing function call - invalid parameter format: '{0}'.";

    [Theory]
    [InlineAutoMoqData]
    public void CLIArgValidator_IsValid(string functionName)
    {
        // Arrange
        var parameters = new object[] { functionName };

        var sut = new CLIArgValidator();

        // Act
        var result = sut.Validate(parameters);

        // Assert
        result.IsValid.Should().BeTrue();

        result.Errors.Should().BeEmpty();
    }

    [Theory]
    [InlineAutoMoqData]
    public void CLIArgValidator_IsValidWithParams(
        string functionName, 
        string param1,
        string param2,
        string param3,
        string param4)
    {
        // Arrange
        var parameters = new object[] 
        { 
            functionName,
            $"-{param1}",
            param2,
            $"-{param3}",
            param4
        };

        var sut = new CLIArgValidator();

        // Act
        var result = sut.Validate(parameters);

        // Assert
        result.IsValid.Should().BeTrue();

        result.Errors.Should().BeEmpty();
    }

    [Theory]
    [InlineAutoMoqData]
    public void CLIArgValidator_IsValidSwitch(string functionName, string switch1)
    {
        // Arrange
        var parameters = new object[] { functionName, $"-{switch1}" };

        var sut = new CLIArgValidator();

        // Act
        var result = sut.Validate(parameters);

        // Assert
        result.IsValid.Should().BeTrue();

        result.Errors.Should().BeEmpty();
    }

    [Theory]
    [InlineAutoMoqData]
    public void CLIArgValidator_IsValidWithParamsAndSwitches(
        string functionName,
        string param1,
        string param2,
        string switch1,
        string param3,
        string param4,
        string switch2)
    {
        // Arrange
        var parameters = new object[]
        {
            functionName,
            $"-{param1}",
            param2,
            $"-{switch1}",
            $"-{param3}",
            param4,
            $"-{switch2}"
        };

        var sut = new CLIArgValidator();

        // Act
        var result = sut.Validate(parameters);

        // Assert
        result.IsValid.Should().BeTrue();

        result.Errors.Should().BeEmpty();
    }

    [Theory]
    [InlineAutoMoqData]
    public void CLIArgValidator_InValidOddParamCount(string functionName, string param1)
    {
        // Arrange
        var parameters = new object[] { functionName, param1 };

        var sut = new CLIArgValidator();

        // Act
        var result = sut.Validate(parameters);

        // Assert
        result.IsValid.Should().BeFalse();

        result.Errors.Should().HaveCount(1);

        result.Errors[0].Should().Be(string.Format(InvalidFormatMsg, param1) + System.Environment.NewLine + Examples);
    }

    [Theory]
    [InlineAutoMoqData]
    public void CLIArgValidator_InValidParamFormat(string functionName, string param1, string param2)
    {
        // Arrange
        var parameters = new object[] { functionName, param1, param2 };

        var sut = new CLIArgValidator();

        // Act
        var result = sut.Validate(parameters);

        // Assert
        result.IsValid.Should().BeFalse();

        result.Errors.Should().HaveCount(1);

        result.Errors[0].Should().Be(string.Format(InvalidFormatMsg, param1) + System.Environment.NewLine + Examples);
    }

    [Fact]
    public void CLIArgValidator_InValidNoFunctionSpecified()
    {
        // Arrange
        var parameters = Array.Empty<object>();

        var sut = new CLIArgValidator();

        // Act
        var result = sut.Validate(parameters);

        // Assert
        result.IsValid.Should().BeFalse();

        result.Errors.Should().HaveCount(1);

        result.Errors[0].Should().Be(EmptyParamsMsg + System.Environment.NewLine + Examples);
    }
}