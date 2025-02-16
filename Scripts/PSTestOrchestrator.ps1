using module ".\lib\VersionCalculator.psm1";

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

function RunTests {
    param (
        [string] $testRoot
    )

    [string[]] $paths = Get-ChildItem -Path $testRoot -Recurse -File -Filter "*Tests.ps1" | 
        Where-Object { 
            $_.FullName -notmatch "E2E[\\/]Do" -and $_.FullName -notmatch "E2ETool[\\/]Do"
        } |
        ForEach-Object { 
            $_.FullName 
        };

    Write-Host "Running $($paths.Length) test files.";

    if (!(Test-Path -Path $script:testResultsPath)) {
        New-Item -ItemType Directory -Path $script:testResultsPath;
    }

    $pesterConfig = @{
        Run = @{
            Path = $paths
            PassThru = $true
            Exit = $true
        }
        CodeCoverage = @{
            Enabled = $false
        }
        TestResult = @{
            Enabled = $false
        }
    };

    $result = Invoke-Pester -Configuration $pesterConfig;   
    
    if ($result.FailedCount -gt 0) { 
        throw "Pester tests failed." 
    }
}

if (!$useLatest) {
    # run unit tests
    RunTests -testRoot "$($testRoot)$($sep)Unit";
}

# run component tests
RunTests -testRoot "$($testRoot)$($sep)E2E";

if ($useLatest) {
    # run some E2E tests on the dotnet tool
    RunTests -testRoot "$($testRoot)$($sep)E2ETool";
}

# run sample project tests
doing test -filter .* -silent -home "$(Get-Location)$($sep)Sample$($sep)";