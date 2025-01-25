using DoFramework.Services;
using FluentAssertions;

namespace DoFrameworkTests.Services;

public class ObjectBuilderTests
{
    [Fact]
    public void ObjectBuilder_BuildsObject()
    {
        // Arrange
        var sut = new ObjectBuilder();

        // Act
        var result = sut.BuildObject(typeof(SampleClass1), []);

        // Assert
        result.Should().NotBeNull();

        result.GetType().Should().Be(typeof(SampleClass1));
    }

    [Theory]
    [InlineAutoMoqData]
    public void ObjectBuilder_BuildsObjectWithParams(
        SampleClass1 sampleClass1, 
        string thestring, 
        int theint)
    {
        // Arrange
        var sut = new ObjectBuilder();

        // Act
        var result = sut.BuildObject(typeof(SampleClass2), [sampleClass1, thestring, theint]);

        // Assert
        result.Should().NotBeNull();

        result.GetType().Should().Be(typeof(SampleClass2));
    }
}

public class SampleClass1 { }

public class SampleClass2 
{
#pragma warning disable
    public SampleClass2(SampleClass1 sampleClass1, string thestring, int  theint) { }
#pragma warning enable
}
