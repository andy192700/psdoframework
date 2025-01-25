namespace DoFramework.Types;

/// <summary>
/// Defines a lookup for process types by name.
/// </summary>
public interface ILookupProcessType
{
    /// <summary>
    /// Looks up the type associated with the specified process name.
    /// </summary>
    /// <param name="name">The name of the process to lookup.</param>
    /// <returns>The <see cref="Type"/> associated with the specified process name.</returns>
    Type Lookup(string name);
}

