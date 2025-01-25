using DoFramework.Testing;

namespace DoFramework.Validators;

/// <summary>
/// Represents a validator for test runner dictionaries, enforcing the validation of required and optional parameters.
/// </summary>
public class TestRunnerDictionaryValidator : CLIFunctionDictionaryValidator
{
    /// <summary>
    /// Initializes a new instance of the <see cref="TestRunnerDictionaryValidator"/> class.
    /// </summary>
    public TestRunnerDictionaryValidator()
    {
        RequiredParameters.Add(("filter", typeof(string)));

        OptionalParameters.Add(("outputFormat", ValidatePesterOutputType));
    }

    /// <summary>
    /// Validates if the given value is a valid <see cref="PesterOutputType"/>.
    /// </summary>
    /// <param name="value">The value to validate.</param>
    /// <returns><c>true</c> if the value is a valid <see cref="PesterOutputType"/>; otherwise, <c>false</c>.</returns>
    private bool ValidatePesterOutputType(object value)
    {
        return Enum.GetNames(typeof(PesterOutputType)).Any(x => x.Equals(value));
    }
}
