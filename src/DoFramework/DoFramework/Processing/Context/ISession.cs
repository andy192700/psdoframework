using DoFramework.Domain;

namespace DoFramework.Processing;

/// <summary>
/// Interface for managing session-related operations and data.
/// </summary>
public interface ISession
{
    /// <summary>
    /// Gets or sets the name of the current process.
    /// </summary>
    string? CurrentProcessName { get; set; }

    /// <summary>
    /// Gets or sets the count of processes.
    /// </summary>
    int ProcessCount { get; set; }

    /// <summary>
    /// Gets or sets the list of process reports.
    /// </summary>
    List<ProcessReport> ProcessReports { get; set; }
}
