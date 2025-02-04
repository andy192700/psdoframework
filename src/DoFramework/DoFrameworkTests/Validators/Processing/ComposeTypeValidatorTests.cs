using DoFramework.Processing;
using DoFramework.Validators;
using FluentAssertions;

namespace DoFrameworkTests.Validators;

public class ComposerTypeValidatorTests
{
    private string TypeError { get; set; } = $"{nameof(IComposer)} classes must derive from the {typeof(IComposer).FullName} class.";

    private string MultipleConstructorError { get; set; } = $"{nameof(IComposer)} classes must not have more than one constructor, only one constructor is allowed.";

    [Theory]
    [InlineAutoMoqData]
    public void ComposerTypeValidator_NotAComposerTypeInvalid(SampleType sampleType)
    {
        // Arrange
        var sut = new ComposerTypeValidator();

        // Act
        var result = sut.Validate(sampleType.GetType());

        // Assert
        result.Should().NotBeNull();

        result.IsValid.Should().BeFalse();

        result.Errors.Should().HaveCount(1);

        result.Errors[0].Should().Be(TypeError);
    }

    [Theory]
    [InlineAutoMoqData]
    public void ComposerTypeValidator_NotAComposerTypeMultipleConstructorsInvalid(SampleTypeMultipleConstructors sampleType)
    {
        // Arrange
        var sut = new ComposerTypeValidator();

        // Act
        var result = sut.Validate(sampleType.GetType());

        // Assert
        result.Should().NotBeNull();

        result.IsValid.Should().BeFalse();

        result.Errors.Should().HaveCount(2);

        result.Errors[0].Should().Be(TypeError);
        result.Errors[1].Should().Be(MultipleConstructorError);
    }

    [Theory]
    [InlineAutoMoqData]
    public void ComposerTypeValidator_ComposerTypeMultipleConstructorsInvalid(ComposerMultipleConstructors sampleType)
    {
        // Arrange
        var sut = new ComposerTypeValidator();

        // Act
        var result = sut.Validate(sampleType.GetType());

        // Assert
        result.Should().NotBeNull();

        result.IsValid.Should().BeFalse();

        result.Errors.Should().HaveCount(1);

        result.Errors[0].Should().Be(MultipleConstructorError);
    }

    [Theory]
    [InlineAutoMoqData]
    public void ComposerTypeValidator_ComposerTypeValid(SampleComposer sampleType)
    {
        // Arrange
        var sut = new ComposerTypeValidator();

        // Act
        var result = sut.Validate(sampleType.GetType());

        // Assert
        result.Should().NotBeNull();

        result.IsValid.Should().BeTrue();

        result.Errors.Should().HaveCount(0);
    }
}