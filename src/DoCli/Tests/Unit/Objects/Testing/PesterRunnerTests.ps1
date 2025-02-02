using namespace DoFramework.CLI;
using namespace DoFramework.Environment;
using namespace System.Collections.Generic;
using module "..\..\..\..\Objects\Testing\PesterConfig.psm1";

Describe 'PesterRunnerTests' {
    BeforeEach {
        [string] $sep = [DoFramework.Environment.Environment]::Separator.ToString();

        # Ensure native PowerShell methods are mock-able by using Invoke-Expression rather than a using module statement.
        Invoke-Expression -Command "$(Get-Content "$PSScriptRoot$($sep)..$($sep)..$($sep)..$($sep)..$($sep)Objects$($sep)Testing$($sep)PesterRunner.psm1")";

        [ProxyResult] $mockEnv = doing mock -type ([IEnvironment]);

        [CLIFunctionParameters] $params = [CLIFunctionParameters]::new();

        $params.Parameters = [Dictionary[string, object]]::new();

        [PesterConfig] $script:config = [PesterConfig]::new($mockEnv.Instance, $params);

        Mock Invoke-Pester -Verifiable;
    }
    
    Context 'Tests' {
        it 'Should Invoke' {
            # Arrange
            [PesterRunner] $sut = [PesterRunner]::new();

            [object] $testConfig = $config.GetConfiguration([string[]]::new(0), "Module");

            # Act
            $sut.Run($testConfig);

            # Assert
            Should -Invoke Invoke-Pester -Times 1 -Exactly;
        }
        
        it 'Should Invoke With Paths' {
            # Arrange
            [PesterRunner] $sut = [PesterRunner]::new();

            [object] $testConfig = $config.GetConfiguration([string[]]::new(0), "Process");

            # Act
            $sut.Run($testConfig, [string[]]::new(0));

            # Assert
            Should -Invoke Invoke-Pester -Times 1 -Exactly;
        }
    }
}