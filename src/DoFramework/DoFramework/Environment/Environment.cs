using DoFramework.Data;
using DoFramework.Domain;
using DoFramework.FileSystem;

namespace DoFramework.Environment;

/// <summary>
/// Represents an environment with various directory settings and validation capabilities.
/// </summary>
public class Environment : IEnvironment
{
    /// <inheritdoc/>
    public string HomeDir { get; set; }

    /// <inheritdoc/>
    public string ProcessesDir { get; set; }

    /// <inheritdoc/>
    public string TestsDir { get; set; }

    /// <inheritdoc/>
    public string ModuleDir { get; set; }

    /// <inheritdoc/>
    public string ComposersDir { get; set; }

    /// <inheritdoc/>
    public static char Separator { get; } = Path.DirectorySeparatorChar;

    private readonly IReadProcessLocation _readProcessLocation;
    private readonly ISimpleDataProvider<ProjectContents> _projectContentsProvider;

    /// <summary>
    /// Initializes a new instance of the <see cref="Environment"/> class.
    /// </summary>
    /// <param name="readProcessLocation">An instance of <see cref="IReadProcessLocation"/> to read the process location.</param>
    /// <param name="fileManager">An instance of <see cref="IFileManager"/> to manage file operations.</param>
    /// <param name="projectContentsProvider">An instance of <see cref="ISimpleDataProvider{ProjectContents}"/> to provide project contents.</param>
    public Environment(
        IReadProcessLocation readProcessLocation,
        ISimpleDataProvider<ProjectContents> projectContentsProvider)
    {
        _readProcessLocation = readProcessLocation;
        _projectContentsProvider = projectContentsProvider;

        HomeDir = _readProcessLocation.Read();

        var contents = _projectContentsProvider.Provide();
        ProcessesDir = $"{HomeDir}{Separator}{contents.Name}{Separator}Processes";
        TestsDir = $"{HomeDir}{Separator}{contents.Name}{Separator}Tests";
        ModuleDir = $"{HomeDir}{Separator}{contents.Name}{Separator}Modules";
        ComposersDir = $"{HomeDir}{Separator}{contents.Name}{Separator}Composers";
    }
}
