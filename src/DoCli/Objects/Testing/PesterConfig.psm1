using namespace DoFramework.CLI;
using namespace DoFramework.Environment;
using namespace DoFramework.Testing;
using namespace System.Collections.Generic;

<#
.SYNOPSIS
Class for configuring Pester test settings within the DoFramework environment.

.DESCRIPTION
The PesterConfig class is designed to create and manage configurations for Pester 
tests within the DoFramework environment. It sets up the test paths, code coverage, 
and test result settings based on the provided parameters and environment.
#>
class PesterConfig {
    <#
    .SYNOPSIS
    Initializes a new instance of the PesterConfig class.

    .DESCRIPTION
    Constructor for the PesterConfig class, which sets up the environment and 
    CLI function parameters for Pester configuration.
    #>
    [IEnvironment] $Environment;
    [CLIFunctionParameters] $CLIFunctionParameters;

    PesterConfig([IEnvironment] $environment, [CLIFunctionParameters] $cliFunctionParameters) {
        $this.Environment = $environment;
        $this.CLIFunctionParameters = $cliFunctionParameters;
    }

    <#
    .SYNOPSIS
    Retrieves the Pester configuration settings.

    .DESCRIPTION
    The GetConfiguration method creates and returns the Pester configuration settings 
    based on the provided test paths and test name. It includes settings for code 
    coverage and test result output.
    #>
    [object] GetConfiguration([string[]] $paths, [string] $testName) {
        [Dictionary[string, object]] $dictionary = $this.CLIFunctionParameters.Parameters;

        [PesterOutputType] $outputType = [PesterOutputType]::None;

        [bool] $outputEnabled = $false;

        if ($dictionary.ContainsKey("outputFormat")) {
            $outputType = [PesterOutputType][Enum]::Parse([PesterOutputType], $dictionary["outputFormat"]);

            $outputEnabled = $outputType -ne [PesterOutputType]::None;
        }
        
        $configuration = @{
            Run = @{
                Path = $paths
            }
            CodeCoverage = @{
                Enabled = $outputEnabled
                OutputPath = "$($this.Environment.HomeDir)$([DoFramework.Environment.Environment]::Separator)$($testName)TestCoverage.xml"
            }
            TestResult = @{
                OutputPath = "$($this.Environment.HomeDir)$([DoFramework.Environment.Environment]::Separator)$($testName)TestResults.xml"
                OutputFormat = $outputType.ToString()
                Enabled = $outputEnabled
            }
        };
        
        return $configuration;
    }
}
