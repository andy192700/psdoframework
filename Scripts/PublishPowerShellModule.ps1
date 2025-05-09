using namespace System.IO;

param (
    [Parameter(Mandatory=$true)] [ValidateNotNullOrEmpty()] [string] $psNuGetSourceName,    
    [string] $psNuGetApiKey
)

$ErrorActionPreference = "Stop";

[char] $sep = [Path]::DirectorySeparatorChar;

[string] $modulePath = (Join-Path -Path (Get-Location) -ChildPath "Build$($sep)PSDoFramework");

if (![string]::IsNullOrEmpty($psNuGetApiKey)) {
    Publish-Module -Path $modulePath -Repository $psNuGetSourceName -NuGetApiKey $psNuGetApiKey;
}
else {
    Publish-Module -Path $modulePath -Repository $psNuGetSourceName;
}
