using namespace System.IO;
using module ".\lib\VersionCalculator.psm1";

param (
    [Parameter(Mandatory=$true)] [ValidateNotNullOrEmpty()] [string] $psNuGetSourceName,
    [Parameter(Mandatory=$true)] [ValidateNotNullOrEmpty()] [string] $solutionFile,
    [Parameter(Mandatory=$true)] [ValidateNotNullOrEmpty()] [string] $solutionConfig,
    [string] $psNuGetApiKey
)

$ErrorActionPreference = "Stop";

[string] $version = [VersionCalculator]::GetLatest($psNuGetSourceName);

[char] $sep = [Path]::DirectorySeparatorChar;

dotnet pack $solutionFile --configuration $solutionConfig --no-build "/p:Version=$version";

if (![string]::IsNullOrEmpty($psNuGetApiKey)) {
    dotnet nuget push "$(Get-Location)$($sep)src$($sep)DoFramework$($sep)PSDoFramework.Tool$($sep)bin$($sep)$solutionConfig$($sep)PSDoFramework.Tool.$($version).nupkg" --source $psNuGetSourceName --api-key $psNuGetApiKey;
}
else {
    dotnet nuget push "$(Get-Location)$($sep)src$($sep)DoFramework$($sep)PSDoFramework.Tool$($sep)bin$($sep)$solutionConfig$($sep)PSDoFramework.Tool.$($version).nupkg" --source $psNuGetSourceName;
}