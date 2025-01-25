using DoFramework.CLI;
using FluentAssertions;

namespace DoFrameworkTests.CLI;

public class CLIFunctionParametersTests
{

    [Theory]
    [InlineAutoMoqData]
    public void Parameters_ParseSwitchIsFalseByDefault(
        string switchKey)
    {
        // Arrange
        var sut = new CLIFunctionParameters
        {
            Parameters = []
        };

        // Act
        var result = sut.ParseSwitch(switchKey);

        // Assert
        result.Should().BeFalse();
    }

    [Theory]
    [InlineAutoMoqData]
    public void Parameters_7ParseSwitchIsTrueWhenSet(
        string switchKey)
    {
        // Arrange
        var sut = new CLIFunctionParameters();

        sut.Parameters = new Dictionary<string, object>
        {
            { switchKey, true }
        };

        // Act
        var result = sut.ParseSwitch(switchKey);

        // Assert
        result.Should().BeTrue();
    }

    [Theory]
    [InlineAutoMoqData]
    public void Parameters_7ParseSwitchIsfalseWhenSet(
        string switchKey)
    {
        // Arrange
        var sut = new CLIFunctionParameters();

        sut.Parameters = new Dictionary<string, object>
        {
            { switchKey, false }
        };

        // Act
        var result = sut.ParseSwitch(switchKey);

        // Assert
        result.Should().BeFalse();
    }

    [Theory]
    [InlineAutoMoqData]
    public void Parameters_7ParseSwitchIsfalseWhenSetAndInvalid(
        string switchKey)
    {
        // Arrange
        var sut = new CLIFunctionParameters();

        sut.Parameters = new Dictionary<string, object>
        {
            { switchKey, switchKey }
        };

        // Act
        var result = sut.ParseSwitch(switchKey);

        // Assert
        result.Should().BeFalse();
    }
}
