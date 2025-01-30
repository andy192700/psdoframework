using DoFramework.Domain;

namespace DoFramework.Processing;

/// <summary>
/// Class representing a session with process-related data.
/// </summary>
public class Session : ISession
{
    /// <inheritdoc/>
    public string? CurrentProcessName { get; set; }

    /// <inheritdoc/>
    public int ProcessCount { get; set; }

    /// <inheritdoc/>
    public List<ProcessReport> ProcessReports { get; set; } = [];

    public string? ComposedBy { get; set; }

    public bool Composed => !string.IsNullOrEmpty(ComposedBy);
}
