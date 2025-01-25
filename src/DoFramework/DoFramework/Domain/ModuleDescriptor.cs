using DoFramework.Environment;

namespace DoFramework.Domain;

/// <summary>
/// Represents a module descriptor that implements the IDescriptor interface.
/// Provides properties for type name, path, name, and extension, and a method to retrieve the directory.
/// </summary>
public class ModuleDescriptor : IDescriptor
{
    /// <summary>
    /// Gets the type name of the module by removing "Descriptor" from the class name.
    /// </summary>
    public string TypeName => GetType().Name.Replace("Descriptor", string.Empty);

    /// <summary>
    /// Gets or sets the path associated with the module.
    /// </summary>
    public string? Path { get; set; }

    /// <summary>
    /// Gets or sets the name of the module.
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// Gets or sets the file extension of the module. Default value is ".psm1".
    /// </summary>
    public string? Extension { get; set; } = ".psm1";

    /// <summary>
    /// Retrieves the directory for the module based on the provided environment.
    /// </summary>
    /// <param name="environment">The environment to determine the directory.</param>
    /// <returns>A string representing the directory.</returns>
    public string GetDirectory(IEnvironment environment) => environment.ModuleDir;
}
