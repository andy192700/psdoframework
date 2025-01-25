namespace DoFramework.Validators;

/// <summary>
/// Represents the result of a validation operation, containing validation errors and indicating whether the validation was successful.
/// </summary>
public class ValidationResult : IValidationResult
{
    /// <summary>
    /// Gets a value indicating whether the validation was successful.
    /// </summary>
    public bool IsValid
    {
        get
        {
            return Errors.Count == 0;
        }
    }

    /// <summary>
    /// Gets or sets the list of validation errors.
    /// </summary>
    public List<string> Errors { get; set; }

    public ValidationResult(List<string> errors)
    {
        Errors = errors;
    }
}
