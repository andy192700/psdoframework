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

Export-ModuleMember -Function RunTests;