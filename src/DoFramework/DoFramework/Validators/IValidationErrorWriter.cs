namespace DoFramework.Validators;

/// <summary>
/// Defines a writer for validation errors, providing a method to output validation results.
/// </summary>
public interface IValidationErrorWriter
{
    /// <summary>
    /// Writes the specified validation result.
    /// </summary>
    /// <param name="validationResult">The validation result to write.</param>
    void Write(IValidationResult validationResult);
}
