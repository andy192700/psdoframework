using DoFramework.Domain;

namespace DoFramework.Processing;

/// <summary>
/// Defines an interface for running a process instance.
/// </summary>
public interface IProcessInstanceRunner
{
    /// <summary>
    /// Runs the process instance based on the given report.
    /// </summary>
    /// <param name="report">The report containing details about the process instance to run.</param>
    void RunInstance(ProcessReport report);
}
