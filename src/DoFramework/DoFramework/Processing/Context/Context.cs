namespace DoFramework.Processing;

/// <summary>
/// Class representing a context that stores session and key-value pairs.
/// </summary>
/// <param name="session">The session instance.</param>
public class Context(ISession session) : IContext
{
    /// <summary>
    /// Gets or sets the session.
    /// </summary>
    public ISession Session { get; set; } = session;

    private readonly Dictionary<string, object> _context = [];

    /// <inheritdoc/>
    public object? Get(string key)
    {
        _ = _context.TryGetValue(key, out var value);
        return value;
    }

    /// <inheritdoc/>
    public TReturn? Get<TReturn>(string key) where TReturn : class
    {
        return Get(key) as TReturn;
    }

    /// <inheritdoc/>
    public void AddOrUpdate(string key, object value)
    {
        if (_context.ContainsKey(key))
        {
            _context[key] = value;
        }
        else
        {
            _context.Add(key, value);
        }
    }

    /// <inheritdoc/>
    public bool KeyExists(string key)
    {
        return _context.ContainsKey(key);
    }

    /// <inheritdoc />
    public bool ParseSwitch(string key)
    {
        if (_context is null)
        {
            return false;
        }

        if (_context!.ContainsKey(key))
        {
            if (bool.TryParse(_context![key].ToString(), out var switchValue) && switchValue)
            {
                return true;
            }

            return false;
        }

        return false;
    }

    /// <inheritdoc/>
    public IContextVerifier Requires()
    {
        return new ContextVerifier(this);
    }

    /// <inheritdoc/>
    public void SetComposedBy(string composedBy)
    {
        Session.ComposedBy = composedBy;
    }
}
