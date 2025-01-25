using DoFramework.Domain;

namespace DoFramework.Processing;

/// <summary>
/// Defines an interface for displaying process reports.
/// </summary>
public interface IDisplayReports
{
    /// <summary>
    /// Displays the specified list of process reports.
    /// </summary>
    /// <param name="processReports">The list of process reports to display.</param>
    void Display(List<ProcessReport> processReports);
}
