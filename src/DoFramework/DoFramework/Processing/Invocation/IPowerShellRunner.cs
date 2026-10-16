namespace DoFramework.Processing;

/// <summary>
/// Provides an abstraction for executing PowerShell scripts in an isolated process.
/// </summary>
public interface IPowerShellRunner
{
    /// <summary>
    /// Executes a PowerShell script using an external <c>pwsh</c> process and streams output and error results.
    /// </summary>
    /// <param name="script">
    /// The PowerShell script content to execute.
    /// This is passed directly to the PowerShell process via the <c>-Command</c> argument.
    /// </param>
    /// <param name="executionPolicy">
    /// The execution policy to apply for the lifetime of the PowerShell process (e.g. <c>Bypass</c>, <c>RemoteSigned</c>).
    /// This is enforced at process scope to avoid relying on machine or user defaults.
    /// </param>
    /// <param name="profile">
    /// Optional path to a PowerShell profile script.
    /// If provided and the file exists, it will be applied to the session before executing the script.
    /// If <c>null</c> or empty, no profile is applied.
    /// </param>
    void Run(string script, string executionPolicy, string? profile);
}
