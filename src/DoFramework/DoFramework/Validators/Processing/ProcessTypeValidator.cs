using DoFramework.Processing;

namespace DoFramework.Validators;

/// <summary>
/// Validates process types, ensuring they derive from the <see cref="Process"/> class and have exactly one constructor.
/// </summary>
public class ProcessTypeValidator : IValidator<Type>
{
    private string TypeError = $"Process classes must derive from the {typeof(Process).FullName} class.";

    private const string MultipleConstructorError = "Process classes must not have more than one constructor, only one constructor is allowed.";

    /// <summary>
    /// Validates the specified process type.
    /// </summary>
    /// <param name="item">The process type to validate.</param>
    /// <returns>The result of the validation.</returns>
    public IValidationResult Validate(Type item)
    {
        var errors = new List<string>();

        if (!item.IsSubclassOf(typeof(Process)))
        {
            errors.Add(TypeError);
        }

        var constructors = item.GetConstructors();

        if (constructors.Length != 1)
        {
            errors.Add(MultipleConstructorError);
        }

        return new ValidationResult(errors);
    }
}
