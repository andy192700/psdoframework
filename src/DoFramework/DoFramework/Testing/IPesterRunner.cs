namespace DoFramework.Testing;

/// <summary>
/// Defines a runner for executing Pester tests with given configurations and paths.
/// </summary>
public interface IPesterRunner
{
    /// <summary>
    /// Runs the Pester tests with the specified configuration and paths.
    /// </summary>
    /// <param name="config">The configuration object for the Pester run.</param>
    /// <param name="paths">The array of paths where the tests are located.</param>
    void Run(object config, string[] paths);

    /// <summary>
    /// Runs the Pester tests with the specified configuration.
    /// </summary>
    /// <param name="config">The configuration object for the Pester run.</param>
    void Run(object config);
}
