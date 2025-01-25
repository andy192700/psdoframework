using DoFramework.Mappers;
using FluentAssertions;

namespace DoFrameworkTests.Mappers;

public class ArgMapperTests
{

    [Fact]
    public void ArgMapper_CanMapEmpty()
    {
        // Arrange
        var sut = new ArgMapper();

        // Act
        var result = sut.Map([]);

        // Assert
        result.Should().NotBeNull();

        result.GetType().Should().Be(typeof(Dictionary<string, object>));

        result.Count.Should().Be(0);
    }

    [Theory]
    [InlineAutoMoqData]
    public void ArgMapper_CanMapOneParam(
        string param1,
        string val1)
    {
        // Arrange
        var args = new object[]
        {
            $"-{param1}",
            val1
        };

        var sut = new ArgMapper();

        // Act
        var result = sut.Map(args);

        // Assert
        result.Should().NotBeNull();
        
        result.GetType().Should().Be(typeof(Dictionary<string, object>));

        result.Count.Should().Be(1);

        result[param1].Should().Be(val1);
    }

    [Theory]
    [InlineAutoMoqData]
    public void ArgMapper_CanMapOneSwitches(
        string switch1)
    {
        // Arrange
        var args = new object[]
        {
            $"-{switch1}"
        };

        var sut = new ArgMapper();

        // Act
        var result = sut.Map(args);

        // Assert
        result.Should().NotBeNull();

        result.GetType().Should().Be(typeof(Dictionary<string, object>));

        result.Count.Should().Be(1);

        result[switch1].Should().Be(true);
    }

    [Theory]
    [InlineAutoMoqData]
    public void ArgMapper_CanMapTwoParamsTwoSwitches(
        string param1,
        string val1,
        string switch1,
        string param2,
        string val2,
        string switch2)
    {
        // Arrange
        var args = new object[]
        {
            $"-{param1}",
            val1,
            $"-{switch1}",
            $"-{param2}",
            val2,
            $"-{switch2}"
        };

        var sut = new ArgMapper();

        // Act
        var result = sut.Map(args);

        // Assert
        result.Should().NotBeNull();

        result.GetType().Should().Be(typeof(Dictionary<string, object>));

        result.Count.Should().Be(4);

        result[param1].Should().Be(val1);

        result[param2].Should().Be(val2);

        result[switch1].Should().Be(true);

        result[switch2].Should().Be(true);
    }
}