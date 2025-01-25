namespace DoFramework.Validators;

/// <summary>
/// Defines a validator for items of type <typeparamref name="TItem"/>, providing a method to validate the item.
/// </summary>
/// <typeparam name="TItem">The type of the item to validate.</typeparam>
public interface IValidator<TItem>
{
    /// <summary>
    /// Validates the specified item.
    /// </summary>
    /// <param name="item">The item to validate.</param>
    /// <returns>The result of the validation.</returns>
    IValidationResult Validate(TItem item);
}
