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
    make pstests;
}

make publishmodule psNuGetSourceName=$psNuGetSourceName;

make dotnetpublish nuGetGallerySourceName=$psNuGetSourceName;

make installmodule;

if (!$skipTests) {
    make pstestspostinstall;
    
    make installtool psNuGetSourceName=$psNuGetSourceName;
}
