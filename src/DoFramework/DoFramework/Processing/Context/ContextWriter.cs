namespace DoFramework.Processing;

/// <summary>
/// Class for writing key-value pairs to the context.
/// </summary>
/// <param name="context">The context instance.</param>
public class ContextWriter(IContext context) : IContextWriter
{
    private readonly IContext _context = context;

    /// <summary>
    /// Writes the key-value pairs from the dictionary to the context.
    /// </summary>
    /// <param name="dictionary">The dictionary containing key-value pairs.</param>
    public void Write(Dictionary<string, object> dictionary)
    {
        foreach (var key in dictionary.Keys)
        {
            _context!.AddOrUpdate(key, dictionary[key]);
        }
    }
}
