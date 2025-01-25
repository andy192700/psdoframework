using DoFramework.Domain;

namespace DoFramework.Processing;

/// <summary>
/// Implements the IFailedReportChecker interface to check for failed process reports.
/// </summary>
public class FailedReportChecker : IFailedReportChecker
{
    private readonly IContext _context;

    /// <summary>
    /// Initializes a new instance of the <see cref="FailedReportChecker"/> class.
    /// </summary>
    /// <param name="context">The context used for checking the process reports.</param>
    public FailedReportChecker(IContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Checks if there are any failed, invalidated, or not found process reports.
    /// </summary>
    /// <returns><c>true</c> if there are any failed, invalidated, or not found process reports; otherwise, <c>false</c>.</returns>
    public bool Check()
    {
        return _context.Session.ProcessReports
            .Count(r => r.ProcessResult == ProcessResult.Invalidated
                     || r.ProcessResult == ProcessResult.Failed
                     || r.ProcessResult == ProcessResult.NotFound) > 0;
    }
}
