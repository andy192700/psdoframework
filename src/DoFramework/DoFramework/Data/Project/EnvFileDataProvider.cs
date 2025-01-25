using DoFramework.Environment;
using DoFramework.FileSystem;

namespace DoFramework.Data;

/// <summary>
/// Provides environment file data by reading key-value pairs from .env files in the home directory.
/// </summary>
/// <param name="environment">The environment interface for retrieving directory information.</param>
/// <param name="fileManager">The file manager for handling file operations.</param>
public class EnvFileDataProvider(
    IEnvironment environment,
    IFileManager fileManager) : ISimpleDataProvider<Dictionary<string, object>>
{
    private readonly IEnvironment _environment = environment;
    private readonly IFileManager _fileManager = fileManager;

    /// <summary>
    /// Provides a dictionary containing key-value pairs from .env files in the home directory.
    /// </summary>
    /// <returns>A dictionary containing key-value pairs from .env files.</returns>
    public Dictionary<string, object> Provide()
    {
        var envFileData = new Dictionary<string, object>();

        var filter = ".env*";

        var matchingFiles = _fileManager.GetFiles(_environment.HomeDir, filter, SearchOption.TopDirectoryOnly);

        foreach (var file in matchingFiles)
        {
            var fileContent = _fileManager.ReadAllLines(file.FullName);

            foreach (var line in fileContent)
            {
                var keyValue = line.Split('=');

                if (keyValue.Length == 2)
                {
                    var key = keyValue[0].Trim();
                    var value = keyValue[1].Trim();

                    envFileData[key] = value;
                }
            }
        }

        return envFileData;
    }
}
