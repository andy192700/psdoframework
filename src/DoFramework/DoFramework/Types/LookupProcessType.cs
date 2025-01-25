using DoFramework.Validators;

namespace DoFramework.Types;

/// <summary>
/// Implements a process type lookup, providing validation and error reporting capabilities.
/// </summary>
public class LookupProcessType(IValidator<Type> validator, IValidationErrorWriter validationErrorWriter) : ILookupProcessType
{
    private readonly IValidator<Type> _validator = validator;
    private readonly IValidationErrorWriter _validationErrorWriter = validationErrorWriter;

    /// <summary>
    /// Looks up the type associated with the specified process name.
    /// </summary>
    /// <param name="name">The name of the process to lookup.</param>
    /// <returns>The <see cref="Type"/> associated with the specified process name.</returns>
    /// <exception cref="Exception">Thrown when the process class cannot be found or is invalid.</exception>
    public Type Lookup(string name)
    {
        Type type = null!;

        foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
        {
            var potentialType = assembly.GetType(name, false);

            if (potentialType is not null)
            {
                type = potentialType;

                break;
            }
        }

        if (type is null)
        {
            throw new Exception($"Could not find the process class {name}");
        }

        var result = _validator.Validate(type);

        if (!result.IsValid)
        {
            _validationErrorWriter.Write(result);

            throw new Exception($"Process Type is invalid {name}");
        }

        return type;
    }
}
