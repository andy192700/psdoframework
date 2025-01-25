namespace DoFramework.Processing;

/// <summary>
/// Represents an abstract base class for a process that can be validated and run.
/// </summary>
public abstract class Process : IProcess
{
    /// <summary>
    /// Validates the process.
    /// </summary>
    /// <returns><c>true</c> if the process is valid; otherwise, <c>false</c>.</returns>
    public virtual bool Validate()
    {
        return true;
    }

    /// <summary>
    /// Runs the process.
    /// </summary>
    public abstract void Run();
}
