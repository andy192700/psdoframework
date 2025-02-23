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

$moduleName = 'PSDoFramework';
$desiredVersion = '{version}';
$repository = '{repository}';

$installedModule = Get-Module -ListAvailable -Name $moduleName | Where-Object {{ $_.Version -eq $desiredVersion }};

if ($null -eq $installedModule) {{
    Write-Host PowerShell Module $moduleName version $desiredVersion is not installed. Installing from the repository $repository...;

    Install-Module -Name $moduleName -Repository $repository -RequiredVersion $desiredVersion -Force -Scope CurrentUser;

    $installedModule = Get-Module -ListAvailable -Name $moduleName | Where-Object {{ $_.Version -eq $desiredVersion }};
    
    if ($null -ne $installedModule) {{
        Write-Host PowerShell Module $moduleName version $desiredVersion has been successfully installed.;
    }} 
    else {{
        Write-Host Failed to install PowerShell Module $moduleName version $desiredVersion`.;
    }}
}}

Import-Module -Name $moduleName $desiredVersion;
doing {doingArgs.ToString()};";

        var process = new Process();

        process.StartInfo.FileName = "pwsh";

        process.StartInfo.Arguments = $"-ExecutionPolicy Bypass -Command \"{arguments}\"";
        
        process.StartInfo.RedirectStandardError = true;

        process.StartInfo.UseShellExecute = false;

        return process;
    }
}
