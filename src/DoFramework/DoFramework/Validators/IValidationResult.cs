namespace DoFramework.Validators;

/// <summary>
/// Represents the result of a validation operation, indicating whether the validation was successful and providing any validation errors.
/// </summary>
public interface IValidationResult
{
    /// <summary>
    /// Gets a value indicating whether the validation was successful.
    /// </summary>
    bool IsValid { get; }

    /// <summary>
    /// Gets or sets the list of validation errors.
    /// </summary>
    List<string> Errors { get; set; }
}
