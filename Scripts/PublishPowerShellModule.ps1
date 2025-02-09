param (
    [Parameter(Mandatory=$true)] [ValidateNotNullOrEmpty()] [string] $psNuGetSourceName,    
    [string] $psNuGetApiKey
)

$ErrorActionPreference = "Stop";

[string] $modulePath = (Join-Path -Path (Get-Location) -ChildPath "Build\PSDoFramework");

if (![string]::IsNullOrEmpty($psNuGetApiKey)) {
    Publish-Module -Path $modulePath -Repository $psNuGetSourceName -NuGetApiKey $psNuGetApiKey;
}
else {
    make dotnetpublish psNuGetSourceName=$psNuGetSourceName;

    Publish-Module -Path $modulePath -Repository $psNuGetSourceName;
}