namespace DoFramework.Processing;

/// <summary>
/// Defines an interface for checking if there are any failed reports.
/// </summary>
public interface IFailedReportChecker
{
    /// <summary>
    /// Checks if there are any failed reports.
    /// </summary>
    /// <returns><c>true</c> if there are any failed reports; otherwise, <c>false</c>.</returns>
    bool Check();
}
