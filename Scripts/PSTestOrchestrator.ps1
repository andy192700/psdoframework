using module ".\lib\VersionCalculator.psm1";
using module ".\lib\TestHelper.psm1";

param (
    [switch] $useLatest,
    [string] $psNuGetSourceName
)

$ErrorActionPreference = "Stop";

if ($useLatest) {
    [string] $version = [VersionCalculator]::GetLatest($psNuGetSourceName);

    Import-Module -name PSDoFramework -RequiredVersion $version -Force;
}
else {
    Import-Module .\Build\PSDoFramework\PSDoFramework.psd1 -Force;
}

[char] $sep = [System.IO.Path]::DirectorySeparatorChar;

[string] $testRoot = Join-Path -Path $PSScriptRoot -ChildPath "..$($sep)src$($sep)DoCli$($sep)Tests";

[string] $script:testResultsPath = "$(Get-Location)$($sep)test-results";

if (!$useLatest) {
    # run unit tests
    RunTests -testRoot "$($testRoot)$($sep)Unit";
}

# run component tests
RunTests -testRoot "$($testRoot)$($sep)E2E";

# run sample project tests
doing test -filter .* -silent -home "$(Get-Location)$($sep)Sample$($sep)";