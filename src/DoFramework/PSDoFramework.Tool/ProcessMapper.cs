using DoFramework.Mappers;
using Microsoft.Extensions.Options;
using System.Diagnostics;
using System.Text;

namespace PSDoFramework.Tool;

/// <summary>
/// Concrete impl that maps application args to a <see cref="Process"/>.
/// </summary>
public class ProcessMapper : IMapper<string[], Process>
{
    private readonly IOptions<PowerShellSettings> _settings;

    public ProcessMapper(IOptions<PowerShellSettings> settings)
    {
        _settings = settings;
    }

    /// <inheritdoc/>
    public Process Map(string[] args)
    {
        var doingArgs = new StringBuilder();

        for (int i = 0; i < args.Length; i++)
        {
            doingArgs.Append($" {args[i]}");
        }

        var version = _settings.Value.FrameworkVersion;
        var repository = _settings.Value.Repository;

        var arguments = @$"$ErrorActionPreferene = 'Stop';
        $module = Get-InstalledModule -Name PSDoFramework;
        if ($null -eq $module -or $module.Version -ne ""{version}"") {{
            Write-Host ""Installing the PowerShell Module 'PSdoFramework' Version: {version} Repository: {repository}"" -ForegroundColor Yellow;
            Install-Module -Name PSDoFramework -Repository {repository} -RequiredVersion {version} -Force;
        }}

        doing {doingArgs.ToString()};";

        var process = new Process();

        process.StartInfo.FileName = "pwsh";

        process.StartInfo.Arguments = $"-ExecutionPolicy Bypass -Command \"{arguments}\"";
        
        process.StartInfo.RedirectStandardError = true;

        process.StartInfo.UseShellExecute = false;

        return process;
    }
}
