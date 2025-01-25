using DoFramework.Domain;

namespace DoFramework.Processing;

/// <summary>
/// Class representing a session with process-related data.
/// </summary>
public class Session : ISession
{
    /// <summary>
    /// Gets or sets the name of the current process.
    /// </summary>
    public string? CurrentProcessName { get; set; }

    /// <summary>
    /// Gets or sets the count of processes.
    /// </summary>
    public int ProcessCount { get; set; }

    /// <summary>
    /// Gets or sets the list of process reports.
    /// </summary>
    public List<ProcessReport> ProcessReports { get; set; } = [];
}
