namespace DoFramework.Processing;

/// <summary>
/// Defines an interface for running a process.
/// </summary>
public interface IProcessRunner
{
    /// <summary>
    /// Runs the specified task.
    /// </summary>
    /// <param name="task">The task to be executed.</param>
    void Run(string task);
}
