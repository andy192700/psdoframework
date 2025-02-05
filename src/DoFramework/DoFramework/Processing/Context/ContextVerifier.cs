namespace DoFramework.Processing;

/// <summary>
/// Represents a context verifier that verifies various conditions within a context.
/// </summary>
public class ContextVerifier : IContextVerifier
{
    private readonly IContext _context;
    private readonly IList<Func<IContext, bool>> checks = [];

    /// <summary>
    /// Initializes a new instance of the <see cref="ContextVerifier"/> class.
    /// </summary>
    /// <param name="context">The context to verify.</param>
    public ContextVerifier(IContext context)
    {
        _context = context;
    }

    /// <inheritdoc/>
    public IContextVerifier ComposedBy(string composerName)
    {
        checks.Add(c => c.Session.Composed && c.Session.ComposedBy == composerName);

        return this;
    }

    /// <inheritdoc/>
    public IContextVerifier ConfirmKey(string key)
    {
        checks.Add(c => c.KeyExists(key));

        return this;
    }

    /// <inheritdoc/>
    public IContextVerifier ProcessSucceeded(string processName)
    {
        checks.Add(c => c.Session.ProcessReports.Any(x => x.ProcessResult == Domain.ProcessResult.Completed && x.Name == processName));

        return this;
    }

    /// <inheritdoc/>
    public bool Verify()
    {
        return checks.All(x => x(_context));
    }
}
