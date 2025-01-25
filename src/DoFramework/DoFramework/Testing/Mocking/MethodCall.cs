using System.Reflection;

namespace DoFramework.Testing;

/// <summary>
/// Represents a method call with its corresponding method information, arguments, and result.
/// </summary>
public class MethodCall
{
    /// <summary>
    /// Initializes a new instance of the <see cref="MethodCall"/> class with the specified method and arguments.
    /// </summary>
    /// <param name="method">The method information for the method call.</param>
    /// <param name="args">The arguments for the method call.</param>
    public MethodCall(MethodInfo method, Dictionary<string, object> args)
    {
        Method = method;
        Args = args;
    }

    /// <summary>
    /// Gets the method information for the method call.
    /// </summary>
    public MethodInfo Method { get; }

    /// <summary>
    /// Gets the arguments for the method call.
    /// </summary>
    public Dictionary<string, object> Args { get; }

    /// <summary>
    /// Gets or sets the result of the method call.
    /// </summary>
    public object? Result { get; set; }
}
