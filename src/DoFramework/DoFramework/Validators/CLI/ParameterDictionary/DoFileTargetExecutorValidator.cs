namespace DoFramework.Validators;

public class DoFileTargetExecutorValidator : CLIFunctionDictionaryValidator
{
    public DoFileTargetExecutorValidator()
    {
        RequiredParameters.Add(("target", typeof(string)));
    }
}
