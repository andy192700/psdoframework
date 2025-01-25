using DoFramework.Domain;

namespace DoFramework.Processing;

/// <summary>
/// Defines an interface for a process executor.
/// </summary>
public interface IProcessExecutor
{
    /// <summary>
    /// Executes a process based on the given descriptor and returns a process report.
    /// </summary>
    /// <param name="descriptor">The descriptor that specifies the process to execute.</param>
    /// <returns>A report of the executed process.</returns>
    ProcessReport Execute(ProcessDescriptor descriptor);
}
