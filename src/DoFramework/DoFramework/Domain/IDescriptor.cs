using DoFramework.Environment;

namespace DoFramework.Domain;

/// <summary>
/// Represents a descriptor interface providing properties for type name,
/// path, name, and extension, and a method to retrieve the directory.
/// </summary>
public interface IDescriptor
{
    /// <summary>
    /// Gets the type name of the descriptor.
    /// </summary>
    string TypeName { get; }

    /// <summary>
    /// Gets or sets the path associated with the descriptor.
    /// </summary>
    string? Path { get; set; }

    /// <summary>
    /// Gets or sets the name of the descriptor.
    /// </summary>
    string? Name { get; set; }

    /// <summary>
    /// Gets or sets the file extension of the descriptor.
    /// </summary>
    string? Extension { get; set; }

    /// <summary>
    /// Retrieves the directory for the descriptor based on the provided environment.
    /// </summary>
    /// <param name="environment">The environment to determine the directory.</param>
    /// <returns>A string representing the directory.</returns>
    string GetDirectory(IEnvironment environment);
}
