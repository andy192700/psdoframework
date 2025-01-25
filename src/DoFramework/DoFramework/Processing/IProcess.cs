namespace DoFramework.Processing;

/// <summary>
/// Defines an interface for a process that can be validated and run.
/// </summary>
public interface IProcess
{
    /// <summary>
    /// Validates the process.
    /// </summary>
    /// <returns><c>true</c> if the process is valid; otherwise, <c>false</c>.</returns>
    bool Validate();

    /// <summary>
    /// Runs the process.
    /// </summary>
    void Run();
}
