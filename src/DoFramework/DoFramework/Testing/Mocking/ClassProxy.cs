using System.Reflection;

namespace DoFramework.Testing;

/// <summary>
/// Represents a proxy class that extends the <see cref="Proxy"/> class.
/// </summary>
public class ClassProxy : Proxy
{
    /// <summary>
    /// Invokes the specified method with no arguments.
    /// </summary>
    /// <param name="targetMethod">The target method to invoke.</param>
    /// <returns>The result of the method invocation.</returns>
    public object? Invoke(MethodInfo targetMethod)
    {
        return Invoke(targetMethod, []);
    }

    /// <summary>
    /// Invokes the specified method with the given arguments.
    /// </summary>
    /// <param name="targetMethod">The target method to invoke.</param>
    /// <param name="arguments">The arguments to pass to the method.</param>
    /// <returns>The result of the method invocation.</returns>
    public new object? Invoke(MethodInfo targetMethod, params object[] arguments)
    {
        return base.Invoke(targetMethod, arguments);
    }
}
