namespace DoFramework.FileSystem;

/// <summary>
/// Class providing file management functionalities.
/// </summary>
public class FileManager : IFileManager
{
    /// <summary>
    /// Determines whether the specified file exists.
    /// </summary>
    /// <param name="path">The file path.</param>
    /// <returns>True if the file exists; otherwise, false.</returns>
    public bool FileExists(string path)
    {
        return File.Exists(path);
    }

    /// <summary>
    /// Determines whether the parent directory of the specified path exists.
    /// </summary>
    /// <param name="path">The file path.</param>
    /// <returns>True if the parent directory exists; otherwise, false.</returns>
    public bool ParentDirectoryExists(string path)
    {
        var descriptorFileInfo = new FileInfo(path);

        var parentPath = descriptorFileInfo.Directory!.FullName;

        return Directory.Exists(parentPath);
    }

    /// <summary>
    /// Reads all lines from the specified file.
    /// </summary>
    /// <param name="path">The file path.</param>
    /// <returns>An array of lines from the file.</returns>
    public string[] ReadAllLines(string path)
    {
        return File.ReadAllLines(path);
    }

    /// <summary>
    /// Reads all text from the specified file.
    /// </summary>
    /// <param name="path">The file path.</param>
    /// <returns>The contents of the file as a string.</returns>
    public string ReadAllText(string path)
    {
        return File.ReadAllText(path);
    }

    /// <summary>
    /// Writes the specified string to a file.
    /// </summary>
    /// <param name="path">The file path.</param>
    /// <param name="data">The data to write.</param>
    public void WriteAllText(string path, string data)
    {
        File.WriteAllText(path, data);
    }

    /// <summary>
    /// Gets information about the specified file.
    /// </summary>
    /// <param name="path">The file path.</param>
    /// <returns>A FileInfo object representing the file.</returns>
    public FileInfo GetFileInfo(string path)
    {
        return new(path);
    }

    /// <summary>
    /// Gets an array of files from the specified directory.
    /// </summary>
    /// <param name="path">The directory path.</param>
    /// <param name="searchPattern">The search pattern.</param>
    /// <param name="searchOption">Specifies whether to search all subdirectories or only the current directory.</param>
    /// <returns>An array of FileInfo objects representing the files.</returns>
    public FileInfo[] GetFiles(string path, string searchPattern, SearchOption searchOption)
    {
        var directoryInfo = new DirectoryInfo(path);

        return directoryInfo.GetFiles(searchPattern, SearchOption.TopDirectoryOnly);
    }

    /// <summary>
    /// Deletes the specified file.
    /// </summary>
    /// <param name="path">The file path.</param>
    public void DeleteFile(string path)
    {
        File.Delete(path);
    }

    /// <summary>
    /// Creates the parent directory for the specified file path.
    /// </summary>
    /// <param name="path">The file path.</param>
    public void CreateParentDirectory(string path)
    {
        var descriptorFileInfo = new FileInfo(path);

        var parentPath = descriptorFileInfo.Directory!.FullName;

        Directory.CreateDirectory(parentPath);
    }
}
