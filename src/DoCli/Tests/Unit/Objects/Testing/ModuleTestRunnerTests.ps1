using namespace DoFramework.CLI;
using namespace DoFramework.Domain;
using namespace DoFramework.Data;
using namespace DoFramework.Environment;
using namespace DoFramework.Logging;
using namespace DoFramework.Testing;
using namespace System.Collections.Generic;
using module "..\..\..\..\Objects\Testing\PesterConfig.psm1";
using module "..\..\..\..\Objects\Testing\ModuleTestRunner.psm1";

Describe 'ModuleTestRunnerTests' {

    BeforeEach {
        [ProxyResult] $mockEnv = doing create-proxy -type ([IEnvironment]);

        [ProxyResult] $script:mockLogger = doing create-proxy -type ([ILogger]);

        [ProxyResult] $script:mockProvider = doing create-proxy -type ([IDataCollectionProvider[TestDescriptor, string]]);
        
        [ProxyResult] $script:pesterRunner = doing create-proxy -type ([IPesterRunner]);

        [CLIFunctionParameters] $params = [CLIFunctionParameters]::new();

        $params.Parameters = [Dictionary[string, object]]::new();

        [PesterConfig] $script:config = [PesterConfig]::new($mockEnv.Instance, $params);
    }

    Context 'Tests' {
        it 'No tests found, pester will not invoke and logging is correct' {
            # Arrange            
            $mockProvider.Proxy.MockMethod("Provide", {
                param ([string] $parameter)

                return [List[TestDescriptor]]::new();
            });

            [ModuleTestRunner] $sut = [ModuleTestRunner]::new(
                $mockProvider.Instance, 
                $mockLogger.Instance, 
                $mockEnv.Instance, 
                $config,
                $pesterRunner.Instance);

            # Act 
            $sut.Test("abc");

            # Assert
            $mockProvider.Proxy.CountCalls("Provide", (doing read-args -parameter "abc")) | Should -Be 1;

            $mockLogger.Proxy.CountCalls("LogInfo") | Should -Be 1;   

            $mockLogger.Proxy.CountCalls("LogInfo", (doing read-args -message "Found 0 candidate module test files.")) | Should -Be 1;

            $pesterRunner.Proxy.CountCalls("Run") | Should -Be 0;  
        }
        
        it 'Tests found, pester invoked and logging is correct' {
            # Arrange
            $mockProvider.Proxy.MockMethod("Provide", {
                param ([string] $parameter)
                [List[TestDescriptor]] $descriptors = [List[TestDescriptor]]::new();

                [TestDescriptor] $descriptor1 = [TestDescriptor]::new();
                $descriptor1.TestType = [TestType]::Module;
                $descriptors.Add($descriptor1);

                [TestDescriptor] $descriptor2 = [TestDescriptor]::new();
                $descriptor2.TestType = [TestType]::Module;
                $descriptors.Add($descriptor2);

                return $descriptors;
            });

            [ModuleTestRunner] $sut = [ModuleTestRunner]::new(
                $mockProvider.Instance, 
                $mockLogger.Instance, 
                $mockEnv.Instance, 
                $config,
                $pesterRunner.Instance);

            # Act 
            $sut.Test("abc");

            # Assert
            $mockProvider.Proxy.CountCalls("Provide", (doing read-args -parameter "abc")) | Should -Be 1;

            $mockLogger.Proxy.CountCalls("LogInfo") | Should -Be 2;    

            $mockLogger.Proxy.CountCalls("LogInfo", (doing read-args -message "Found 2 candidate module test files.")) | Should -Be 1;

            $mockLogger.Proxy.CountCalls("LogInfo", (doing read-args -message "Running 2 module test files.")) | Should -Be 1;

            $pesterRunner.Proxy.CountCalls("Run") | Should -Be 1;   
        }
    }
}