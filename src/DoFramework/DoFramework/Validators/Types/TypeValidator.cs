namespace DoFramework.Validators;

/// <summary>
/// Abstract type validator, a blueprint for confirming if a type derives from a given type.
/// </summary>
/// <typeparam name="TBaseType">The type from which the supplied type should derive.</typeparam>
public abstract class TypeValidator<TBaseType> : IValidator<Type> where TBaseType : class
{
    protected abstract string TypeError { get; set; }

    protected abstract string MultipleConstructorError {  get; set; }

    /// <summary>
    /// Validates the specified process type.
    /// </summary>
    /// <param name="item">The process type to validate.</param>
    /// <returns>The result of the validation.</returns>
    public virtual IValidationResult Validate(Type item)
    {
        var errors = new List<string>();

        if (!item.IsAssignableTo(typeof(TBaseType)))
        {
            errors.Add(TypeError);
        }

        var constructors = item.GetConstructors();

        if (constructors.Length > 1)
        {
            errors.Add(MultipleConstructorError);
        }

        return new ValidationResult(errors);
    }
}