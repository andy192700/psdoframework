using module ".\lib\VersionCalculator.psm1";

param (
    [Parameter(Mandatory=$true)] [ValidateNotNullOrEmpty()] [string] $psNuGetSourceName,
    [Parameter(Mandatory=$true)] [ValidateNotNullOrEmpty()] [string] $solutionFile,
    [Parameter(Mandatory=$true)] [ValidateNotNullOrEmpty()] [string] $solutionConfig
)

$ErrorActionPreference = "Stop";

[char] $sep = [System.IO.Path]::DirectorySeparatorChar;

[string] $version = [VersionCalculator]::Calculate($psNuGetSourceName);

[string] $jsonFilePath = "$(Get-Location)$($sep)src$($sep)DoFramework$($sep)PSDoFramework.Tool$($sep)appsettings.json";

[pscustomobject] $jsonContent = Get-Content -Path $jsonFilePath -Raw | ConvertFrom-Json;

$jsonContent.PowerShellSettings.Repository = $psNuGetSourceName;

[string] $updatedJsonContent = $jsonContent | ConvertTo-Json -Depth 10;

Set-Content -Path $jsonFilePath -Value $updatedJsonContent;

dotnet build $solutionFile --configuration $solutionConfig "/p:Version=$version;PSNugetRepository=$psNuGetSourceName";