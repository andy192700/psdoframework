namespace DoFramework.Validators;

/// <summary>
/// Represents a validator for descriptor management dictionaries, enforcing the validation of required parameters.
/// </summary>
public class DescriptorManagementDictionaryValidator : CLIFunctionDictionaryValidator
{
    /// <summary>
    /// Initializes a new instance of the <see cref="DescriptorManagementDictionaryValidator"/> class.
    /// </summary>
    public DescriptorManagementDictionaryValidator()
    {
        RequiredParameters.Add(("name", typeof(string)));
    }
}
