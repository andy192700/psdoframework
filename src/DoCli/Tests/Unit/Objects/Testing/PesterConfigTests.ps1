using namespace DoFramework.CLI;
using namespace DoFramework.Environment;
using namespace DoFramework.Testing;
using namespace System.Collections.Generic;
using module "..\..\..\..\Objects\Testing\PesterConfig.psm1";

Describe 'PesterConfigTests' {
    BeforeEach {
        [ProxyResult] $script:mockEnv = doing mock -type ([IEnvironment]);

        $mockEnv.Instance.HomeDir = "HomeDirectory";

        [string] $script:testName = "testName";

        [string[]] $script:pathArray = @( "123", "456", "abc" );
    }

    Context 'Tests' {
        it 'Invalid Enum Throws' {
            # Arrange
            [CLIFunctionParameters] $params = [CLIFunctionParameters]::new();

            $params.Parameters = [Dictionary[string, object]]::new();

            $params.Parameters["outputFormat"] = "Astring";

            [PesterConfig] $sut = [PesterConfig]::new($mockEnv.Instance, $params);

            # Act 
            [scriptblock] $func = {
                $sut.GetConfiguration($pathArray, $testName);
            };

            # Asset
            $func | Should -Throw "Exception calling `"Parse`" with `"2`" argument(s): `"Requested value '$($params.Parameters["outputFormat"])' was not found.`"";            
        }
        
        it 'No output specified yields no output' {
            # Arrange
            [CLIFunctionParameters] $params = [CLIFunctionParameters]::new();

            $params.Parameters = [Dictionary[string, object]]::new();

            [PesterConfig] $sut = [PesterConfig]::new($mockEnv.Instance, $params);

            # Act 
            [object] $result = $sut.GetConfiguration($pathArray, $testName);

            # Asset
            $result.Run.Path.Length | should -Be 3;
            $result.CodeCoverage.Enabled | should -Be $false;
            $result.CodeCoverage.OutputPath 
                | should -Be "$($mockEnv.Instance.HomeDir)$([DoFramework.Environment.Environment]::Separator)$($testName)TestCoverage.xml";     
            $result.TestResult.Enabled | should -Be $false;
            $result.TestResult.OutputFormat | should -Be ([PesterOutputType]::None.ToString());
            $result.TestResult.OutputPath 
                | should -Be "$($mockEnv.Instance.HomeDir)$([DoFramework.Environment.Environment]::Separator)$($testName)TestResults.xml";
        }
        
        it 'Output nunit is as expected' {
            # Arrange
            [CLIFunctionParameters] $params = [CLIFunctionParameters]::new();

            $params.Parameters = [Dictionary[string, object]]::new();

            $params.Parameters["outputFormat"] = [PesterOutputType]::NUnitXml;

            [PesterConfig] $sut = [PesterConfig]::new($mockEnv.Instance, $params);

            # Act 
            [object] $result = $sut.GetConfiguration($pathArray, $testName);

            # Asset
            $result.Run.Path.Length | should -Be 3;
            $result.CodeCoverage.Enabled | should -Be $true;
            $result.CodeCoverage.OutputPath 
                | should -Be "$($mockEnv.Instance.HomeDir)$([DoFramework.Environment.Environment]::Separator)$($testName)TestCoverage.xml";     
            $result.TestResult.Enabled | should -Be $true;
            $result.TestResult.OutputFormat | should -Be ([PesterOutputType]::NUnitXml.ToString());
            $result.TestResult.OutputPath 
                | should -Be "$($mockEnv.Instance.HomeDir)$([DoFramework.Environment.Environment]::Separator)$($testName)TestResults.xml";
        }
        
        it 'Output junit is as expected' {
            # Arrange
            [CLIFunctionParameters] $params = [CLIFunctionParameters]::new();

            $params.Parameters = [Dictionary[string, object]]::new();

            $params.Parameters["outputFormat"] = [PesterOutputType]::JUnitXml;

            [PesterConfig] $sut = [PesterConfig]::new($mockEnv.Instance, $params);

            # Act 
            [object] $result = $sut.GetConfiguration($pathArray, $testName);

            # Asset
            $result.Run.Path.Length | should -Be 3;
            $result.CodeCoverage.Enabled | should -Be $true;
            $result.CodeCoverage.OutputPath 
                | should -Be "$($mockEnv.Instance.HomeDir)$([DoFramework.Environment.Environment]::Separator)$($testName)TestCoverage.xml";     
            $result.TestResult.Enabled | should -Be $true;
            $result.TestResult.OutputFormat | should -Be ([PesterOutputType]::JUnitXml.ToString());
            $result.TestResult.OutputPath 
                | should -Be "$($mockEnv.Instance.HomeDir)$([DoFramework.Environment.Environment]::Separator)$($testName)TestResults.xml";
        }
    }
}