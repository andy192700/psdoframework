using DoFramework.Services;
using DoFramework.Validators;

namespace DoFramework.CLI;

/// <summary>
/// Represents an abstract CLI function with a specified validator and return type.
/// </summary>
/// <typeparam name="TValidator">The type of the validator. Must inherit from <see cref="CLIFunctionDictionaryValidator"/>.</typeparam>
/// <typeparam name="TReturn">The return type of the function.</typeparam>
/// <param name="name">The name of the CLI function.</param>
public abstract class CLIFunction<TValidator, TReturn>(string name)
    : ICLIFunction<TReturn> where TValidator : CLIFunctionDictionaryValidator, new()
{
    /// <summary>
    /// Gets the name of the CLI function.
    /// </summary>
    public string Name { get; private set; } = name;

    private readonly TValidator _validator = new();

    /// <summary>
    /// Invokes the CLI function with the specified arguments and service container.
    /// </summary>
    /// <param name="args">The arguments for the function.</param>
    /// <param name="serviceContainer">The service container providing necessary services.</param>
    /// <returns>The result of the function invocation.</returns>
    public abstract TReturn Invoke(Dictionary<string, object> args, IServiceContainer serviceContainer);

    /// <summary>
    /// Validates the provided arguments using the specified validator.
    /// </summary>
    /// <param name="args">The arguments to validate.</param>
    /// <returns>The validation result.</returns>
    public IValidationResult Validate(Dictionary<string, object> args)
    {
        return _validator.Validate(args);
    }
}


/// <summary>
/// Represents an abstract CLI function with a specified validator and object return type.
/// Inherits from <see cref="CLIFunction{TValidator, object}"/>.
/// </summary>
/// <typeparam name="TValidator">The type of the validator. Must inherit from <see cref="CLIFunctionDictionaryValidator"/>.</typeparam>
/// <param name="name">The name of the CLI function.</param>
public abstract class CLIFunction<TValidator>(string name)
    : CLIFunction<TValidator, object>(name) where TValidator : CLIFunctionDictionaryValidator, new()
{
    /// <summary>
    /// Invokes the CLI function with the specified arguments and service container.
    /// </summary>
    /// <param name="args">The arguments for the function.</param>
    /// <param name="serviceContainer">The service container providing necessary services.</param>
    /// <returns>The result of the function invocation as an object.</returns>
    public override object Invoke(Dictionary<string, object> args, IServiceContainer serviceContainer)
    {
        InvokeInternal(args, serviceContainer);

        return null!;
    }

    /// <summary>
    /// Invokes the internal logic of the CLI function with the specified arguments and service container.
    /// </summary>
    /// <param name="args">The arguments for the function.</param>
    /// <param name="serviceContainer">The service container providing necessary services.</param>
    protected abstract void InvokeInternal(Dictionary<string, object> args, IServiceContainer serviceContainer);
}
