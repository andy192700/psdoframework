using DoFramework.Environment;
using DoFramework.Testing;

namespace DoFramework.Domain;

/// <summary>
/// Represents a test descriptor that implements the IDescriptor interface.
/// Provides properties for type name, path, name, extension, and test type,
/// and includes a method to retrieve the directory.
/// </summary>
public class TestDescriptor : IDescriptor
{
    /// <summary>
    /// Gets the type name of the test by removing "Descriptor" from the class name.
    /// </summary>
    public string TypeName => GetType().Name.Replace("Descriptor", string.Empty);

    /// <summary>
    /// Gets or sets the path associated with the test.
    /// </summary>
    public string? Path { get; set; }

    /// <summary>
    /// Gets or sets the name of the test.
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// Gets or sets the file extension of the test. Default value is ".ps1".
    /// </summary>
    public string? Extension { get; set; } = ".ps1";

    /// <summary>
    /// Retrieves the directory for the test based on the provided environment.
    /// </summary>
    /// <param name="environment">The environment to determine the directory.</param>
    /// <returns>A string representing the directory.</returns>
    public string GetDirectory(IEnvironment environment) => environment.TestsDir;

    /// <summary>
    /// Gets or sets the type of the test.
    /// </summary>
    public TestType? TestType { get; set; }
}
