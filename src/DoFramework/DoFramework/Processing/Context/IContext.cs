namespace DoFramework.Processing;

/// <summary>
/// Interface for managing context operations.
/// </summary>
public interface IContext
{
    /// <summary>
    /// Gets or sets the session associated with the context.
    /// </summary>
    ISession Session { get; set; }

    /// <summary>
    /// Retrieves the value associated with the specified key.
    /// </summary>
    /// <param name="key">The key whose value to get.</param>
    /// <returns>The value associated with the specified key, or null if the key does not exist.</returns>
    object? Get(string key);

    /// <summary>
    /// Retrieves the value associated with the specified key.
    /// </summary>
    /// <typeparam name="TReturn">The Type of the stored key, if it exists</typeparam>
    /// <param name="key">The key whose value to get.</param>
    /// <returns>The value associated with the specified key, or null if the key does not exist.</returns>
    TReturn? Get<TReturn>(string key) where TReturn : class;

    /// <summary>
    /// Adds a new key-value pair or updates an existing one.
    /// </summary>
    /// <param name="key">The key to add or update.</param>
    /// <param name="value">The value associated with the key.</param>
    void AddOrUpdate(string key, object value);

    /// <summary>
    /// Checks if the context contains the specified key.
    /// </summary>
    /// <param name="key">The key to check.</param>
    /// <returns>True if the context contains the key; otherwise, false.</returns>
    bool KeyExists(string key);

    /// <summary>
    /// Parses a switch, returning a bool matching the key, if it exists. False always if it does not
    /// </summary>
    /// <param name="key">The name of the key to parse.</param>
    /// <returns>THe parsed switch.</returns>
    public bool ParseSwitch(string key);
}
