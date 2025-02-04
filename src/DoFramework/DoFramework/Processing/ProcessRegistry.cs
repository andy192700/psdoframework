namespace DoFramework.Processing;

/// <summary>
/// Represents a process registry that holds and manages a list of processes.
/// </summary>
public class ProcessRegistry : IProcessRegistry
{
    /// <summary>
    /// Gets or sets the list of registered processes.
    /// </summary>
    private IList<string> Processes { get; set; } = new List<string>();

    /// <inheritdoc/>
    public void RegisterProcess(string processName)
    {
        Processes.Add(processName);
    }

    /// <inheritdoc/>
    public IProcessingRequest ToProcessRequest()
    {
        return new ProcessingRequest([.. Processes]);
    }
}
