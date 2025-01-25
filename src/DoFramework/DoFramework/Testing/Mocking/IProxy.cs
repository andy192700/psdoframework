using System.Management.Automation;

namespace DoFramework.Testing;

/// <summary>
/// Represents a proxy interface with methods for mocking, counting calls, querying callbacks, and resetting.
/// </summary>
public interface IProxy
{
    /// <summary>
    /// Mocks the specified property with the given <see cref="ScriptBlock"/>.
    /// </summary>
    /// <param name="propertyName">The name of the property to mock.</param>
    /// <param name="scriptBlock">The script block to use for mocking.</param>
    void MockProperty(string propertyName, ScriptBlock scriptBlock);

    /// <summary>
    /// Mocks a method with the specified name using the given <see cref="ScriptBlock"/>.
    /// </summary>
    /// <param name="methodName">The name of the method to mock.</param>
    /// <param name="scriptBlock">The script block to use for mocking.</param>
    void MockMethod(string methodName, ScriptBlock scriptBlock);

    /// <summary>
    /// Mocks a method with the specified name using the given <see cref="IScriptBlockWrapper"/>.
    /// </summary>
    /// <param name="methodName">The name of the method to mock.</param>
    /// <param name="scriptBlockWrapper">The script block wrapper to use for mocking.</param>
    void MockMethod(string methodName, IScriptBlockWrapper scriptBlockWrapper);

    /// <summary>
    /// Counts the number of calls to the specified method with optional arguments and an expected return value.
    /// </summary>
    /// <param name="methodName">The name of the method to count calls for.</param>
    /// <param name="args">Optional arguments to match for the call.</param>
    /// <returns>The number of calls matching the specified criteria.</returns>
    int CountCalls(string methodName, Dictionary<string, object> args = default!);

    /// <summary>
    /// Counts the total number of calls to the specified property.
    /// </summary>
    /// <param name="propertyName">The name of the property to count calls for.</param>
    /// <returns>The total number of calls to the property.</returns>
    int CountPropertyCalls(string propertyName);

    /// <summary>
    /// Resets the proxy state.
    /// </summary>
    void Reset();
}
