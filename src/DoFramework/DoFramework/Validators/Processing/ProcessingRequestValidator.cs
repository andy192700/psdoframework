using DoFramework.Processing;

namespace DoFramework.Validators;

/// <summary>
/// Represents a validator for processing requests, ensuring that there is at least one process.
/// </summary>
public class ProcessingRequestValidator : IValidator<IProcessingRequest>
{
    /// <summary>
    /// Validates the specified processing request.
    /// </summary>
    /// <param name="item">The processing request to validate.</param>
    /// <returns>The result of the validation.</returns>
    public IValidationResult Validate(IProcessingRequest item)
    {
        var errors = new List<string>();

        if (item.Processes.Length == 0)
        {
            errors.Add("Cannot request dispatch with zero processes.");
        }

        return new ValidationResult(errors);
    }
}
