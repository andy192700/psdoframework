namespace DoFramework.FileSystem;

/// <summary>
/// Contract for replacing any separators in a path for the correct separator as per the current OS.
/// </summary>
public interface IOSSanitise
{
    /// <summary>
    /// Sanitise the path, ensuring correct path separators as per the current OS.
    /// </summary>
    /// <param name="path">The path to sanitise.</param>
    /// <returns>The sanitised path.</returns>
    string Sanitise(string path);
}
