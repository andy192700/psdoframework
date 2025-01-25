namespace DoFramework.Processing;

/// <summary>
/// Interface for writing key-value pairs to a context.
/// </summary>
public interface IContextWriter
{
    /// <summary>
    /// Writes the key-value pairs from the dictionary to the context.
    /// </summary>
    /// <param name="dictionary">The dictionary containing key-value pairs.</param>
    void Write(Dictionary<string, object> dictionary);
}
