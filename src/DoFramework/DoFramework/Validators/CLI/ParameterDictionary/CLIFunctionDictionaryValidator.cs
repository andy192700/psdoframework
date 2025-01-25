namespace DoFramework.Validators;

/// <summary>
/// Represents a validator for CLI function dictionaries, providing validation for required and optional parameters.
/// </summary>
public abstract class CLIFunctionDictionaryValidator : IValidator<Dictionary<string, object>>
{
    /// <summary>
    /// Gets or sets the list of required parameters with their types.
    /// </summary>
    protected List<(string, Type)> RequiredParameters { get; set; } = [];

    /// <summary>
    /// Gets or sets the list of optional parameters with their validation functions.
    /// </summary>
    protected List<(string, Func<object, bool>)> OptionalParameters { get; set; } = [];

    /// <summary>
    /// Validates the specified dictionary of CLI function parameters.
    /// </summary>
    /// <param name="item">The dictionary of parameters to validate.</param>
    /// <returns>The result of the validation.</returns>
    public IValidationResult Validate(Dictionary<string, object> item)
    {
        var errors = new List<string>();

        var missingRequiredParameters = new List<(string, Type)>();

        foreach (var parameter in RequiredParameters)
        {
            if (item.TryGetValue(parameter.Item1, out object? value))
            {
                var typeMatch = parameter.Item2.IsAssignableFrom(value.GetType())
                    || parameter.Item2.IsSubclassOf(value.GetType());

                if (!typeMatch)
                {
                    missingRequiredParameters.Add(parameter);
                }
            }
            else
            {
                missingRequiredParameters.Add(parameter);
            }
        }

        foreach (var missingParameter in missingRequiredParameters)
        {
            errors.Add($"Required CLI Function parameter is invalid or missing: '{missingParameter.Item1}'");
        }

        foreach (var optionalParameter in OptionalParameters)
        {
            if (item.TryGetValue(optionalParameter.Item1, out object? value))
            {
                if (!optionalParameter.Item2(value))
                {
                    errors.Add($"Optional CLI Function parameter is invalid: '{optionalParameter.Item1}'");
                }
            }
        }

        return new ValidationResult(errors);
    }
}
