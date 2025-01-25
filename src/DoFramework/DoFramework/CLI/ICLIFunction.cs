using DoFramework.Services;
using DoFramework.Validators;

namespace DoFramework.CLI;

/// <summary>
/// Defines a CLI function with a name and validation capability.
/// </summary>
public interface ICLIFunction
{
    /// <summary>
    /// Gets the name of the CLI function.
    /// </summary>
    string Name { get; }

    /// <summary>
    /// Validates the provided arguments.
    /// </summary>
    /// <param name="args">The arguments to validate.</param>
    /// <returns>The result of the validation.</returns>
    IValidationResult Validate(Dictionary<string, object> args);
}

/// <summary>
/// Defines a CLI function with a specific return type.
/// </summary>
/// <typeparam name="TReturn">The return type of the function.</typeparam>
public interface ICLIFunction<TReturn> : ICLIFunction
{
    /// <summary>
    /// Invokes the CLI function with the specified arguments and service container.
    /// </summary>
    /// <param name="args">The arguments for the function.</param>
    /// <param name="serviceContainer">The service container providing necessary services.</param>
    /// <returns>The result of the function invocation.</returns>
    TReturn Invoke(Dictionary<string, object> args, IServiceContainer serviceContainer);
}
