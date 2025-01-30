using DoFramework.Validators;

namespace DoFramework.Types;

/// <summary>
/// Abstract class which performs a type lookup operation against a supplied type, confirming it's type derives from the generic argument.
/// </summary>
/// <typeparam name="TBaseType">The type from which the type to lookup must derive.</typeparam>
public abstract class LookupType<TBaseType>
    : ILookupType<TBaseType> where TBaseType : class
{
    private readonly TypeValidator<TBaseType> _validator;
    private readonly IValidationErrorWriter _validationErrorWriter;

    public LookupType(TypeValidator<TBaseType> validator, IValidationErrorWriter validationErrorWriter)
    {
        _validator = validator;
        _validationErrorWriter = validationErrorWriter;
    }

    /// <summary>
    /// Looks up the type associated with the specified <see cref="TBaseType"/> name.
    /// </summary>
    /// <param name="name">The name of the <see cref="TBaseType"/> to lookup.</param>
    /// <returns>The <see cref="Type"/> associated with the specified <see cref="TBaseType"/> name.</returns>
    /// <exception cref="Exception">Thrown when the <see cref="TBaseType"/> class cannot be found or is invalid.</exception>
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
            throw new Exception($"Could not find the {typeof(TBaseType).Name} class {name}");
        }

        var result = _validator.Validate(type);

        if (!result.IsValid)
        {
            _validationErrorWriter.Write(result);

            throw new Exception($"{typeof(TBaseType).Name} Type is invalid {name}");
        }

        return type;
    }
}