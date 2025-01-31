namespace DoFramework.Processing;

public class ContextVerifier : IContextVerifier
{
    private readonly IContext _context;
    private readonly IList<Func<IContext, bool>> checks = [];

    public ContextVerifier(IContext context)
    {
        _context = context;
    }

    public IContextVerifier ComposedBy(string composerName)
    {
        checks.Add(c => c.Session.Composed && c.Session.ComposedBy == composerName);

        return this;
    }

    public IContextVerifier ConfirmKey(string key)
    {
        checks.Add(c => c.KeyExists(key));

        return this;
    }

    public IContextVerifier ProcessSucceeded(string processName)
    {
        checks.Add(c => c.Session.ProcessReports.Any(x => x.ProcessResult == Domain.ProcessResult.Completed && x.Name == processName));

        return this;
    }

    public bool Verify()
    {
        return checks.All(x => x(_context));
    }
}