using DoFramework.Processing;
using DoFramework.Validators;
using FluentAssertions;

namespace DoFrameworkTests.Validators;

public class ProcessingRequestValidatorTests
{
    [Theory]
    [InlineAutoMoqData]
    public void ProcessingRequestValidator_ZeroProcessesInvalid(ProcessingRequest processingRequest)
    {
        // Arrange
        processingRequest.Processes = [];

        var sut = new ProcessingRequestValidator();

        // Act
        var result = sut.Validate(processingRequest);

        // Assert
        result.Should().NotBeNull();

        result.IsValid.Should().BeFalse();

        result.Errors.Should().HaveCount(1);

        result.Errors[0].Should().Be("Cannot request dispatch with zero processes.");
    }


    [Theory]
    [InlineAutoMoqData]
    public void ProcessingRequestValidator_Valid(ProcessingRequest processingRequest)
    {
        // Arrange
        var sut = new ProcessingRequestValidator();

        // Act
        var result = sut.Validate(processingRequest);

        // Assert
        result.Should().NotBeNull();

        result.IsValid.Should().BeTrue();

        result.Errors.Should().HaveCount(0);
    }
}