using AutoFixture.Xunit2;
using DoFramework.Processing;
using FluentAssertions;
using Moq;

namespace DoFrameworkTests.Processing;

public class ContextTests
{
    [Theory]
    [InlineAutoMoqData]
    public void Context_KeyDoesNotExist(
        [Frozen] Mock<ISession> session,
        string key)
    {
        // Arrange
        var sut = new Context(session.Object);
        
        // Act
        var result = sut.KeyExists(key);
        
        // Assert
        result.Should().BeFalse();
    }

    [Theory]
    [InlineAutoMoqData]
    public void Context_KeyDoesExist(
        [Frozen] Mock<ISession> session,
        string key,
        string value)
    {
        // Arrange
        var sut = new Context(session.Object);

        sut.AddOrUpdate(key, value);

        // Act
        var result = sut.KeyExists(key);

        // Assert
        result.Should().BeTrue();
    }

    [Theory]
    [InlineAutoMoqData]
    public void Context_UpdatesExistingValue(
        [Frozen] Mock<ISession> session,
        string key,
        string value,
        string newValue)
    {
        // Arrange
        var sut = new Context(session.Object);

        sut.AddOrUpdate(key, value);

        // Act
        var result = sut.Get(key);

        sut.AddOrUpdate(key, newValue);

        var newResult = sut.Get(key);

        // Assert
        result.Should().Be(value);
        newResult.Should().Be(newValue);
    }

    [Theory]
    [InlineAutoData]
    public void Context_ReturnsNullWhenValueNotFound(
        [Frozen] Mock<ISession> session,
        string key)
    {
        // Arrange
        var sut = new Context(session.Object);

        // Act
        var result = sut.Get(key);

        // Assert
        result.Should().BeNull();
    }

    [Theory]
    [InlineAutoData]
    public void Context_ReturnsObjectWhenTypedValueNotFound(
        [Frozen] Mock<ISession> session,
        string key)
    {
        // Arrange
        var sut = new Context(session.Object);

        sut.AddOrUpdate(key, new SampleClass());

        // Act
        var result = sut.Get<SampleClass>(key);

        // Assert
        result.Should().NotBeNull();
        result!.GetType().Should().Be(typeof(SampleClass));
    }

    [Theory]
    [InlineAutoData]
    public void Context_ReturnsNullWhenTypedValueNotFound(
        [Frozen] Mock<ISession> session,
        string key)
    {
        // Arrange
        var sut = new Context(session.Object);

        // Act
        var result = sut.Get<SampleClass>(key);

        // Assert
        result.Should().BeNull();
    }
}

class SampleClass { }