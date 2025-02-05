using DoFramework.Processing;

namespace DoFramework.Validators;

/// <summary>
/// Validates <see cref="IComposer"/> types, ensuring they derive from the <see cref="IComposer"/> class and have exactly one constructor.
/// </summary>
public class ComposerTypeValidator : TypeValidator<IComposer> 
{
    protected override string TypeError { get; set; } = $"{nameof(IComposer)} classes must derive from the {typeof(IComposer).FullName} class.";

    protected override string MultipleConstructorError { get; set; } = $"{nameof(IComposer)} classes must not have more than one constructor, only one constructor is allowed.";

    private string NoneEmptyConstructorError = $"{nameof(IComposer)} classes must have an empty constructor.";

    public override IValidationResult Validate(Type item)
    {
        var result = base.Validate(item);

        var constructors = item.GetConstructors();

        if (constructors.Length == 1)
        {
            var parameters = constructors[0].GetParameters();

            if (parameters.Length > 0)
            {
                result.Errors.Add(NoneEmptyConstructorError);
            }
        }

        return result;
    }
}