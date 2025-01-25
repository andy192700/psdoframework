namespace DoFramework.Domain;

/// <summary>
/// Represents the storage for project contents, including its name, processes, tests, and modules.
/// </summary>
public class ProjectContentsStorage
{
    /// <summary>
    /// Gets or sets the name of the project contents storage.
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// Gets or sets the Do-Framework Version of the project for compatability purposes
    /// </summary>
    public string? Version { get; set; }

    /// <summary>
    /// Gets or sets the PowerShell Version of the project for compatability purposes
    /// </summary>
    public string? PSVersion { get; set; }

    /// <summary>
    /// Gets or sets the list of processes associated with the project.
    /// </summary>
    public List<string> Processes { get; set; } = [];

    /// <summary>
    /// Gets or sets the test storage associated with the project.
    /// </summary>
    public TestStorage Tests { get; set; } = new();

    /// <summary>
    /// Gets or sets the list of modules associated with the project.
    /// </summary>
    public List<string> Modules { get; set; } = [];
}
