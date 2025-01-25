using System.Reflection;

namespace DoFramework.Testing;

/// <summary>
/// Defines a wrapper for script blocks, providing methods to invoke, read parameters, and check for return types.
/// </summary>
public interface IScriptBlockWrapper
{
    /// <summary>
    /// Invokes the script block with the specified method and arguments.
    /// </summary>
    /// <param name="targetMethod">The method information to use for invocation.</param>
    /// <param name="args">The arguments to pass to the script block.</param>
    /// <returns>The result of the invocation.</returns>
    object? Invoke(MethodInfo targetMethod, params object[] args);

    /// <summary>
    /// Reads the parameters of the script block.
    /// </summary>
    /// <returns>A dictionary containing the parameter names and their types.</returns>
    Dictionary<string, Type> ReadParameters();

    /// <summary>
    /// Determines whether the script block has a return type.
    /// </summary>
    /// <returns><c>true</c> if the script block has a return type; otherwise, <c>false</c>.</returns>
    bool HasReturnType();
}
