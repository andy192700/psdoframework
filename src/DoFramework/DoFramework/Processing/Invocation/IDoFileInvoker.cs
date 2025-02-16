namespace DoFramework.Processing;

/// <summary>
/// Defines an interface for invoking a specified target, typically related to file operations.
/// </summary>
public interface IDoFileInvoker
{
    /// <summary>
    /// Invokes a target action associated with a file operation.
    /// </summary>
    /// <param name="target">
    /// The identifier of the target to invoke. This could represent a file, operation name, or other relevant entity
    /// depending on the implementation.
    /// </param>
    void InvokeTarget(string target);
}

