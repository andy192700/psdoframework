namespace DoFramework.FileSystem;

/// <summary>
/// Interface for setting the process location.
/// </summary>
public interface ISetProcessLocation
{
    /// <summary>
    /// Sets the process location.
    /// </summary>
    /// <param name="location">The location to set.</param>
    void Set(string location);
}
