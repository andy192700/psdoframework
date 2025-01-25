param (
    [Parameter(Mandatory=$true)] [ValidateNotNullOrEmpty()] [string] $psNuGetSourceName,
    [Parameter(Mandatory=$true)] [ValidateNotNullOrEmpty()] [string] $psNuGetSourceLocation
)

$ErrorActionPreference = "Stop";

if (!(Test-Path -Path $psNuGetSourceLocation)) {
    New-Item -ItemType Directory -Path $psNuGetSourceLocation | Out-Null;
}

if (!(Get-PSRepository -Name $psNuGetSourceName -ErrorAction SilentlyContinue)) {
    Register-PSRepository -Name $psNuGetSourceName `
                          -SourceLocation $psNuGetSourceLocation `
                          -InstallationPolicy Trusted;
    Write-Host;
    Write-Host "Local NuGet source '$psNuGetSourceName' created successfully.";
    Write-Host;
    Write-Host "Restart your PowerShell session to refresh your PowerShell NuGet repositories.`r`rUse Get-PSRepository to confirm it exists." -ForegroundColor Green;
} else {
    Write-Host "Local NuGet source '$psNuGetSourceName' already exists.";
}
