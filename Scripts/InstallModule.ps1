using module ".\lib\VersionCalculator.psm1";

param (
    [Parameter(Mandatory=$true)] [ValidateNotNullOrEmpty()] [string] $psNuGetSourceName
)

$ErrorActionPreference = "Stop";

[string] $version = [VersionCalculator]::GetLatest($psNuGetSourceName);

Write-Host "Latest PSDoFramework version on the repository $($psNuGetSourceName): $version";

Install-Module -Name "PSDoFramework" -RequiredVersion $version -Repository $psNuGetSourceName -Force;

Write-Host "Installed PSDoFramework version $version from the repository: $($psNuGetSourceName)";

dotnet tool uninstall PSDoFramework.Tool --global;

dotnet tool install PSDoFramework.Tool --global --source $psNuGetSourceName;
