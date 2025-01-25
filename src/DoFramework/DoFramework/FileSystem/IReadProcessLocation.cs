namespace DoFramework.FileSystem;

/// <summary>
/// Interface for reading the process location.
/// </summary>
public interface IReadProcessLocation
{
    /// <summary>
    /// Reads the process location.
    /// </summary>
    /// <returns>The process location as a string.</returns>
    string Read();
}
