namespace DoFramework.Validators;

/// <summary>
/// Represents a validator for CLI arguments, providing error messages and examples for invalid calls.
/// </summary>
public class CLIArgValidator : IValidator<object[]>
{
    private const string Examples = "Example calls: 'doing FUNCTIONNAME' 'doing FUNCTIONNAME -arg1 1 -arg2 2 -switch1 -switch2'";
    private const string EmptyParamsMsg = "Invalid doing function call - no parameters were supplied.";
    private const string InvalidFormatMsg = "Invalid doing function call - invalid parameter format: '{0}'.";

    /// <summary>
    /// Validates the specified array of CLI arguments.
    /// </summary>
    /// <param name="item">The array of arguments to validate.</param>
    /// <returns>The result of the validation.</returns>
    public IValidationResult Validate(object[] item)
    {
        var errors = new List<string>();

        if (item.Length == 0)
        {
            errors.Add(EmptyParamsMsg + System.Environment.NewLine + Examples);
        }
        else if (item.Length > 1)
        {
            var remainingArgs = item.Skip(1).ToArray();

            var i = 0;

            while (i < remainingArgs.Length)
            {
                if (!remainingArgs[i].ToString()!.StartsWith("-"))
                {
                    errors.Add(string.Format(InvalidFormatMsg, remainingArgs[i]) + System.Environment.NewLine + Examples);
                }

                if (i < remainingArgs.Length - 1)
                {
                    var next = remainingArgs[i + 1];

                    if (next.ToString()!.StartsWith("-"))
                    {
                        i += 1;
                    }
                    else
                    {
                        i += 2;
                    }
                }
                else
                {
                    break;
                }
            }
        }

        return new ValidationResult(errors);
    }
}
