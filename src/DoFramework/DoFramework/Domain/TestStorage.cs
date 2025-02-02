namespace DoFramework.Domain;

/// <summary>
/// Represents storage for test-related data, including process and module tests.
/// </summary>
public class TestStorage
{
    /// <summary>
    /// Gets or sets the list of process tests associated with the storage.
    /// </summary>
    public List<string> ProcessTests { get; set; } = [];

    /// <summary>
    /// Gets or sets the list of module tests associated with the storage.
    /// </summary>
    public List<string> ModuleTests { get; set; } = [];

    /// <summary>
    /// Gets or sets the list of composer tests associated with the storage.
    /// </summary>
    public List<string> ComposerTests { get; set; } = [];
}
