namespace DoFramework.Processing;

/// <summary>
/// Represents an interface for a process registry.
/// </summary>
public interface IProcessRegistry
{
    /// <summary>
    /// Registers a process with the specified process name.
    /// </summary>
    /// <param name="processName">The name of the process to register.</param>
    void RegisterProcess(string processName);

    /// <summary>
    /// Converts the registered processes to a processing request.
    /// </summary>
    /// <returns>The processing request for the registered processes.</returns>
    IProcessingRequest ToProcessRequest();
}
