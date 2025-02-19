using module ".\lib\VersionCalculator.psm1";

param (
    [Parameter(Mandatory=$true)] [ValidateNotNullOrEmpty()] [string] $psNuGetSourceName
)

$ErrorActionPreference = "Stop";

[string] $version = [VersionCalculator]::GetLatest($psNuGetSourceName);

dotnet tool uninstall PSDoFramework.Tool --global;

dotnet tool install PSDoFramework.Tool --global --version $version --source $psNuGetSourceName;

Write-Host "Executing dummy call to dotnet tool to install powershell module behind the scenes....."; 
# call the dotnet tool to install the module the first time...
psdoing;