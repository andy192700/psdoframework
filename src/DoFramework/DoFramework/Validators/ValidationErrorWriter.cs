using DoFramework.Logging;

namespace DoFramework.Validators;

/// <summary>
/// Represents a writer for validation errors, logging errors using the provided logger.
/// </summary>
public class ValidationErrorWriter : IValidationErrorWriter
{
    private readonly ILogger _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="ValidationErrorWriter"/> class with the specified logger.
    /// </summary>
    /// <param name="logger">The logger to use for logging validation errors.</param>
    public ValidationErrorWriter(ILogger logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// Writes the validation errors to the log.
    /// </summary>
    /// <param name="validationResult">The validation result containing the errors to log.</param>
    public void Write(IValidationResult validationResult)
    {
        foreach (var error in validationResult.Errors)
        {
            _logger.LogError(error);
        }
    }
}
