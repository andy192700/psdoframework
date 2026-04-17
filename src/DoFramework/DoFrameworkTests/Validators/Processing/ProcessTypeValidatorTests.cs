using DoFramework.Processing;
using DoFramework.Validators;
using FluentAssertions;

namespace DoFrameworkTests.Validators;

public class ProcessTypeValidatorTests
{
    private string TypeError { get; set; } = $"{nameof(Process)} classes must derive from the {typeof(IProcess).FullName} class.";

    [Theory]
    [InlineAutoMoqData]
    public void ProcessTypeValidator_NotAProcessTypeInvalid(SampleType sampleType)
    {
        // Arrange
        var sut = new ProcessTypeValidator();

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
    public void ProcessTypeValidator_ProcessTypeValid(SampleProces sampleType)
    {
        // Arrange
        var sut = new ProcessTypeValidator();

        // Act
        var result = sut.Validate(sampleType.GetType());

        // Assert
        result.Should().NotBeNull();

        result.IsValid.Should().BeTrue();

        result.Errors.Should().HaveCount(0);
    }
}