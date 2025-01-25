using DoFramework.Data;
using FluentAssertions;

namespace DoFrameworkTests.Data;

public class JsonConverterTests
{
    [Theory]
    [InlineAutoMoqData]
    public void CanConvertToStringAndBack(Example example)
    {
        // Arrange
        var sut = new JsonConverter();

        // Act
        var asString = sut.Serialize(example);
        var result = sut.Deserialize<Example>(asString);

        // Assert
        result.Should().NotBeNull();
        result.IntValue.Should().Be(example.IntValue);
        result.StringValue.Should().Be(example.StringValue);
    }

    [Theory]
    [InlineAutoMoqData]
    public void CanConvertToObjectAndBack(Example example)
    {
        // Arrange
        var sut = new JsonConverter();
        var input = sut.Serialize(example);

        // Act
        var asObj = sut.Deserialize<Example>(input);
        var result = sut.Serialize(example);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(input);
    }
}

public class Example
{
    public int IntValue { get; set; }

    public string? StringValue { get; set; }
}