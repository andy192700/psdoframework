using DoFramework.FileSystem;
using DoFramework.Logging;
using System.Diagnostics;

namespace DoFramework.Processing;

/// <summary>
/// Concrete implementation for <see cref="IPowerShellRunner"/> contract.
/// </summary>
public class PowerShellRunner : IPowerShellRunner
{
    private readonly IConsoleWrapper _consoleWrapper;

    private readonly IFileManager _fileManager;

    public PowerShellRunner(IConsoleWrapper consoleWrapper, IFileManager fileManager)
    {
        _consoleWrapper = consoleWrapper;
        _fileManager = fileManager;
    }

    /// <inheritdoc />
    public void Run(string script, string executionPolicy, string? profile)
    {
        var psArgs = profile is not null && _fileManager.FileExists(profile)
        ? $"-NoProfile -ExecutionPolicy {executionPolicy} -Command \". '{profile}'; {script}\""
        : $"-NoProfile -ExecutionPolicy {executionPolicy} -Command \"{script}\"";

        var psi = new ProcessStartInfo
        {
            FileName = "pwsh",
            Arguments = psArgs,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        using var process = new System.Diagnostics.Process();
        process.StartInfo = psi;

        process.OutputDataReceived += WriteOutput;
        process.ErrorDataReceived += WriteError;

        process.Start();

        process.BeginOutputReadLine();
        process.BeginErrorReadLine();

        process.WaitForExit();
    }

    private void WriteOutput(object o, DataReceivedEventArgs e)
    {
        if (e.Data is null)
        {
            return;
        }

        _consoleWrapper.WriteLine(e.Data);
    }

    private void WriteError(object o, DataReceivedEventArgs e)
    {
        if (e.Data is null)
        {
            return;
        }

        _consoleWrapper.SetForegroundColor(ConsoleColor.Red);

        _consoleWrapper.WriteLine(e.Data);

        _consoleWrapper.ResetColor();
    }
}