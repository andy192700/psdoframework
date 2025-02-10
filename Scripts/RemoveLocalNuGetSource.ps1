param (
    [Parameter(Mandatory=$true)] [ValidateNotNullOrEmpty()] [string] $psNuGetSourceName,
    [Parameter(Mandatory=$true)] [ValidateNotNullOrEmpty()] [string] $psNuGetSourceLocation
)

$ErrorActionPreference = "Stop";

if ((Get-PSRepository -Name $psNuGetSourceName -ErrorAction SilentlyContinue)) {
    Unregister-PSRepository -Name $psNuGetSourceName;
    
    Write-Host "Local NuGet repository '$psNuGetSourceName' has been removed.";
} else {
    Write-Host "Local NuGet source '$psNuGetSourceName' does not exist.";
}

if ((Test-Path -Path $psNuGetSourceLocation)) {
    Remove-Item -Path $psNuGetSourceLocation -Recurse | Out-Null;
}

Remove-Item ".\nuget.config" -Force;