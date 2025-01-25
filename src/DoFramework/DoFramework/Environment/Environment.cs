using DoFramework.Data;
using DoFramework.Domain;
using DoFramework.FileSystem;

namespace DoFramework.Environment;

/// <summary>
/// Represents an environment with various directory settings and validation capabilities.
/// </summary>
public class Environment : IEnvironment
{
    /// <summary>
    /// Gets or sets the home directory of the environment.
    /// </summary>
    public string HomeDir { get; set; }

    /// <summary>
    /// Gets or sets the processes directory within the environment.
    /// </summary>
    public string ProcessesDir { get; set; }

    /// <summary>
    /// Gets or sets the tests directory within the environment.
    /// </summary>
    public string TestsDir { get; set; }

    /// <summary>
    /// Gets or sets the modules directory within the environment.
    /// </summary>
    public string ModuleDir { get; set; }

    /// <summary>
    /// Gets the directory separator character.
    /// </summary>
    public static char Separator { get; } = Path.DirectorySeparatorChar;

    private readonly IReadProcessLocation _readProcessLocation;
    private readonly IFileManager _FileManager;
    private readonly ISimpleDataProvider<ProjectContents> _projectContentsProvider;

    /// <summary>
    /// Initializes a new instance of the <see cref="Environment"/> class.
    /// </summary>
    /// <param name="readProcessLocation">An instance of <see cref="IReadProcessLocation"/> to read the process location.</param>
    /// <param name="fileManager">An instance of <see cref="IFileManager"/> to manage file operations.</param>
    /// <param name="projectContentsProvider">An instance of <see cref="ISimpleDataProvider{ProjectContents}"/> to provide project contents.</param>
    public Environment(
        IReadProcessLocation readProcessLocation,
        IFileManager fileManager,
        ISimpleDataProvider<ProjectContents> projectContentsProvider)
    {
        _readProcessLocation = readProcessLocation;
        _FileManager = fileManager;
        _projectContentsProvider = projectContentsProvider;

        HomeDir = _readProcessLocation.Read();

        ValidateEnvironment();

        var contents = _projectContentsProvider.Provide();
        ProcessesDir = $"{HomeDir}{Separator}{contents.Name}{Separator}Processes";
        TestsDir = $"{HomeDir}{Separator}{contents.Name}{Separator}Tests";
        ModuleDir = $"{HomeDir}{Separator}{contents.Name}{Separator}Modules";
    }

    /// <summary>
    /// Checks if the environment is valid by verifying the existence of a specific file.
    /// </summary>
    /// <returns>True if the environment is valid; otherwise, false.</returns>
    public bool CheckEnvironment()
    {
        return _FileManager.FileExists($"{HomeDir}{Separator}do.json");
    }

    /// <summary>
    /// Validates the environment by checking for the existence of a required project file.
    /// </summary>
    /// <exception cref="Exception">Thrown when the required project file is not found.</exception>
    public void ValidateEnvironment()
    {
        if (!CheckEnvironment())
        {
            throw new Exception("Error - Do project not found.");
        }
    }
}
