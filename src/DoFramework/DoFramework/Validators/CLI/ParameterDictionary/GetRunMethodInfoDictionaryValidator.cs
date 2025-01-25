namespace DoFramework.Validators;

/// <summary>
/// Represents a validator for dictionaries used to get run method information, enforcing the validation of required parameters.
/// </summary>
public class GetRunMethodInfoDictionaryValidator : CLIFunctionDictionaryValidator
{
    /// <summary>
    /// Initializes a new instance of the <see cref="GetRunMethodInfoDictionaryValidator"/> class.
    /// </summary>
    public GetRunMethodInfoDictionaryValidator()
    {
        RequiredParameters.Add(("methodName", typeof(string)));
        RequiredParameters.Add(("type", typeof(Type)));
    }
}
