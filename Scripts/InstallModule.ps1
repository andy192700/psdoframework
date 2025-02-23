using module ".\lib\VersionCalculator.psm1";

param (
    [Parameter(Mandatory=$true)] [ValidateNotNullOrEmpty()] [string] $psNuGetSourceName
)

$ErrorActionPreference = "Stop";

[string] $version = [VersionCalculator]::GetLatest($psNuGetSourceName);

Install-Module -Name PSDoFramework -RequiredVersion $version -Repository $psNuGetSourceName;