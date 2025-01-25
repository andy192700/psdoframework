using DoFramework.Environment;
using DoFramework.Processing;

namespace DoFramework.Domain;

/// <summary>
/// Represents a process descriptor that implements the IDescriptor interface.
/// Provides properties for type name, path, name, extension, and an instance of the process.
/// Also includes a method to retrieve the directory.
/// </summary>
public class ProcessDescriptor : IDescriptor
{
    /// <summary>
    /// Gets the type name of the process by removing "Descriptor" from the class name.
    /// </summary>
    public string TypeName => GetType().Name.Replace("Descriptor", string.Empty);

    /// <summary>
    /// Gets or sets the path associated with the process.
    /// </summary>
    public string? Path { get; set; }

    /// <summary>
    /// Gets or sets the name of the process.
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// Gets or sets the file extension of the process. Default value is ".ps1".
    /// </summary>
    public string? Extension { get; set; } = ".ps1";

    /// <summary>
    /// Gets or sets an instance of the process.
    /// </summary>
    public IProcess? Instance { get; set; }

    /// <summary>
    /// Retrieves the directory for the process based on the provided environment.
    /// </summary>
    /// <param name="environment">The environment to determine the directory.</param>
    /// <returns>A string representing the directory.</returns>
    public string GetDirectory(IEnvironment environment) => environment.ProcessesDir;
}
