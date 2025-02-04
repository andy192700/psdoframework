using DoFramework.Processing;
using DoFramework.Validators;

namespace DoFramework.Types;

/// <summary>
/// Implements a <see cref="IComposer"/> type lookup, providing validation and error reporting capabilities.
/// </summary>
public class LookupComposerType : LookupType<IComposer>
{
    public LookupComposerType(
        TypeValidator<IComposer> validator,
        IValidationErrorWriter validationErrorWriter) : base(validator, validationErrorWriter) { }
}