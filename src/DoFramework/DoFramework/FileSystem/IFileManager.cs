namespace DoFramework.FileSystem;

/// <summary>
/// Interface for managing file operations.
/// </summary>
public interface IFileManager
{
    /// <summary>
    /// Determines whether the specified file exists.
    /// </summary>
    /// <param name="path">The file path.</param>
    /// <returns>True if the file exists; otherwise, false.</returns>
    bool FileExists(string path);

    /// <summary>
    /// Determines whether the parent directory of the specified path exists.
    /// </summary>
    /// <param name="path">The file path.</param>
    /// <returns>True if the parent directory exists; otherwise, false.</returns>
    bool ParentDirectoryExists(string path);

    /// <summary>
    /// Reads all lines from the specified file.
    /// </summary>
    /// <param name="path">The file path.</param>
    /// <returns>An array of lines from the file.</returns>
    string[] ReadAllLines(string path);

    /// <summary>
    /// Reads all text from the specified file.
    /// </summary>
    /// <param name="path">The file path.</param>
    /// <returns>The contents of the file as a string.</returns>
    string ReadAllText(string path);

    /// <summary>
    /// Writes the specified string to a file.
    /// </summary>
    /// <param name="path">The file path.</param>
    /// <param name="data">The data to write.</param>
    void WriteAllText(string path, string data);

    /// <summary>
    /// Gets information about the specified file.
    /// </summary>
    /// <param name="path">The file path.</param>
    /// <returns>A FileInfo object representing the file.</returns>
    FileInfo GetFileInfo(string path);

    /// <summary>
    /// Gets an array of files from the specified directory.
    /// </summary>
    /// <param name="path">The directory path.</param>
    /// <param name="searchPattern">The search pattern.</param>
    /// <param name="searchOption">Specifies whether to search all subdirectories or only the current directory.</param>
    /// <returns>An array of FileInfo objects representing the files.</returns>
    FileInfo[] GetFiles(string path, string searchPattern, SearchOption searchOption);

    /// <summary>
    /// Deletes the specified file.
    /// </summary>
    /// <param name="path">The file path.</param>
    void DeleteFile(string path);

    /// <summary>
    /// Creates the parent directory for the specified file path.
    /// </summary>
    /// <param name="path">The file path.</param>
    void CreateParentDirectory(string path);
}
