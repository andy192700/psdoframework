using DoFramework.Processing;

namespace DoFramework.Validators;

/// <summary>
/// Validates <see cref="IProcess"/> types, ensuring they derive from the <see cref="IProcess"/> class and have exactly one constructor.
/// </summary>
public class ProcessTypeValidator : TypeValidator<IProcess> 
{
    protected override string TypeError { get; set; } = $"{nameof(Process)} classes must derive from the {typeof(IProcess).FullName} class.";

    protected override string MultipleConstructorError { get; set; } = $"{nameof(Process)} classes must not have more than one constructor, only one constructor is allowed.";
}