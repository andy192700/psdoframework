namespace DoFramework.Environment;

/// <summary>
/// Provides an interface for environment settings and validation.
/// </summary>
public interface IEnvironment
{
    /// <summary>
    /// Gets or sets the home directory of the environment.
    /// </summary>
    string HomeDir { get; set; }

    /// <summary>
    /// Gets or sets the processes directory within the environment.
    /// </summary>
    string ProcessesDir { get; set; }

    /// <summary>
    /// Gets or sets the tests directory within the environment.
    /// </summary>
    string TestsDir { get; set; }

    /// <summary>
    /// Gets or sets the modules directory within the environment.
    /// </summary>
    string ModuleDir { get; set; }

    /// <summary>
    /// Checks if the environment is valid by verifying the existence of a specific file.
    /// </summary>
    /// <returns>True if the environment is valid; otherwise, false.</returns>
    bool CheckEnvironment();

    /// <summary>
    /// Validates the environment by checking for the existence of a required project file.
    /// </summary>
    void ValidateEnvironment();
}
