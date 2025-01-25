namespace DoFramework.Domain;

/// <summary>
/// Represents the contents of a project, including its name, processes, tests, and modules.
/// </summary>
public class ProjectContents
{
    /// <summary>
    /// Gets or sets the name of the project.
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
    /// Gets or sets the list of process descriptors associated with the project.
    /// </summary>
    public List<ProcessDescriptor> Processes { get; set; } = [];

    /// <summary>
    /// Gets or sets the list of test descriptors associated with the project.
    /// </summary>
    public List<TestDescriptor> Tests { get; set; } = [];

    /// <summary>
    /// Gets or sets the list of module descriptors associated with the project.
    /// </summary>
    public List<ModuleDescriptor> Modules { get; set; } = [];
}
