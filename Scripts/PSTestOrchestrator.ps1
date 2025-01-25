using namespace DoFramework.Logging;
using namespace DoFramework.Testing;

param (
    [switch] $isBuildRun
)

$ErrorActionPreference = "Stop";

[ILogger] $logger = [Logger]::new([ConsoleWrapper]::new());

[char] $sep = [DoFramework.Environment.Environment]::Separator;

[string] $testRoot = [string]::Empty;

if ($isBuildRun) {
    $testRoot = Join-Path -Path $PSScriptRoot -ChildPath "..$($sep)Build$($sep)PSDoFramework$($sep)DoCli$($sep)Tests";
}
else {
    $testRoot = Join-Path -Path $PSScriptRoot -ChildPath "..$($sep)src$($sep)DoCli$($sep)Tests";
}

[string] $script:testResultsPath = "$(Get-Location)$($sep)test-results";

function RunTests {
    param (
        [string] $testRoot,
        [string] $name,
        [ILogger] $logger
    )

    [string[]] $paths = Get-ChildItem -Path $testRoot -Recurse -File -Filter "*Tests.ps1" | ForEach-Object { $_.FullName };

    $logger.LogInfo("Running $($paths.Length) test files.");


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
            OutputPath = "$($script:testResultsPath)$($sep)$($name)TestCoverage.xml"
        }
        TestResult = @{
            OutputPath = "$($script:testResultsPath)$($sep)$($name)TestResults.xml"
            OutputFormat = "NUnitXml"
            Enabled = $true
        }
    };

    $result = Invoke-Pester -Configuration $pesterConfig;   
    
    if ($result.FailedCount -gt 0) { 
        throw "Pester tests failed." 
    }
}

# run unit tests
RunTests -testRoot "$($testRoot)$($sep)Unit" -name "Unit" -logger $logger;

# run component tests
RunTests -testRoot "$($testRoot)$($sep)Component" -name "Component" -logger $logger;

# run sample project tests
[string] $currentLocation = (Get-Location);

try {
    Set-Location "$($currentLocation)$($sep)Sample$($sep)";

    doing run-tests -filter .* -outputFormat NUnitXml -silent;
}
catch {
    Set-Location $currentLocation;
    
    throw $_;
}

Set-Location $currentLocation;

if ((Test-Path -Path "$($script:testResultsPath)$($sep)ModuleTestResults.xml")) {
    Remove-Item -Path "$($script:testResultsPath)$($sep)ModuleTestResults.xml"
}

if ((Test-Path -Path "$($script:testResultsPath)$($sep)ProcessTestResults.xml")) {
    Remove-Item -Path "$($script:testResultsPath)$($sep)ProcessTestResults.xml"
}

Move-Item -Path "$($currentLocation)$($sep)Sample$($sep)ModuleTestResults.xml" -Destination "$($script:testResultsPath)$($sep)ModuleTestResults.xml";
Move-Item -Path "$($currentLocation)$($sep)Sample$($sep)ProcessTestResults.xml" -Destination "$($script:testResultsPath)$($sep)ProcessTestResults.xml";