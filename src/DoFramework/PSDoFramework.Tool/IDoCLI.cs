namespace PSDoFramework.Tool;

/// <summary>
/// Contract for calling the Do Framework's CLI
/// </summary>
public interface IDoCLI
{
    /// <summary>
    /// Call the CLI.
    /// </summary>
    /// <param name="args">The command line args, passed through this app to the pwsh call.</param>
    void Exec(string[] args);
}
