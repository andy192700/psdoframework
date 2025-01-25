using module ".\lib\VersionCalculator.psm1";

param (
    [Parameter(Mandatory=$true)] [ValidateNotNullOrEmpty()] [string] $psNuGetSourceName,
    [switch] $skipTests
)

$ErrorActionPreference = "Stop";

[string] $version = [VersionCalculator]::Calculate($psNuGetSourceName);

make dotnetbuild version=$version;

if (!$skipTests) {
    make dotnettest;
}

make createmanifest;

if (!$skipTests) {
    make pstestsbuild;
}

make publishmodule psNuGetSourceName=$psNuGetSourceName;

[PSCustomObject] $module = Find-Module -Name "PSDoFramework" -Repository $psNuGetSourceName -ErrorAction SilentlyContinue;

[string] $version = $module.Version;

if (Get-Module -ListAvailable -Name "PSDoFramework") {
    Update-Module -Name "PSDoFramework" -Force -Verbose;
}
else {
    Install-Module -Name "PSDoFramework" -Repository $psNuGetSourceName -Force -Verbose;
}