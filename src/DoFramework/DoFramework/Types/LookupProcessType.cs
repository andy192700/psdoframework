using DoFramework.Processing;
using DoFramework.Validators;

namespace DoFramework.Types;

/// <summary>
/// Implements a <see cref="IProcess"/> type lookup, providing validation and error reporting capabilities.
/// </summary>
public class LookupProcessType : LookupType<IProcess>
{
    public LookupProcessType(
        TypeValidator<IProcess> validator, 
        IValidationErrorWriter validationErrorWriter) : base(validator, validationErrorWriter) { }
}
