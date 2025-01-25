namespace DoFramework.Processing;

/// <summary>
/// Implements the IComputeHierarchyPrefix interface to compute the hierarchy prefix based on the process count and report count.
/// </summary>
public class ComputeHierarchyPrefix : IComputeHierarchyPrefix
{
    private readonly IContext _context;

    private const string PrefixConstant = "--";

    /// <summary>
    /// Initializes a new instance of the <see cref="ComputeHierarchyPrefix"/> class.
    /// </summary>
    /// <param name="context">The context for computing the hierarchy prefix.</param>
    public ComputeHierarchyPrefix(IContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Computes the hierarchy prefix based on the process count and report count.
    /// </summary>
    /// <returns>The computed hierarchy prefix.</returns>
    public string Compute()
    {
        var processCount = _context.Session.ProcessCount;

        var reportCount = _context.Session.ProcessReports.Count;

        var depth = processCount - reportCount - 1;

        var prefix = string.Empty;

        for (var i = 0; i < depth; i++)
        {
            prefix += PrefixConstant;
        }

        return prefix;
    }
}
