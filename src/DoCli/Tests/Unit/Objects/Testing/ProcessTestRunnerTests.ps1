using namespace DoFramework.CLI;
using namespace DoFramework.Domain;
using namespace DoFramework.Data;
using namespace DoFramework.Environment;
using namespace DoFramework.FileSystem;
using namespace DoFramework.Logging;
using namespace DoFramework.Processing;
using namespace DoFramework.Testing;
using namespace DoFramework.Types;
using namespace System.Collections.Generic;
using module "..\..\..\..\Objects\Testing\PesterConfig.psm1";
using module "..\..\..\..\Objects\Testing\ProcessTesterRunner.psm1";

Describe 'ProcessTestRunnerTests' {
    BeforeEach {
        [ProxyResult] $mockEnv = doing mock -type ([IEnvironment]);

        [ProxyResult] $script:mockLogger = doing mock -type ([ILogger]);

        [ProxyResult] $script:mockProvider = doing mock -type ([IDataCollectionProvider[TestDescriptor, string]]);
        
        [ProxyResult] $script:pesterRunner = doing mock -type ([IPesterRunner]);

        [ProxyResult] $script:mockResolver = doing mock -type ([IResolver[ProcessDescriptor]]);

        [ProxyResult] $script:mockTypeLookup = doing mock -type ([ILookupType[IProcess]]);

        [CLIFunctionParameters] $params = [CLIFunctionParameters]::new();

        $params.Parameters = [Dictionary[string, object]]::new();

        [PesterConfig] $script:config = [PesterConfig]::new($mockEnv.Instance, $params);
    }

    it 'No tests found, pester will not invoke and logging is correct' {
        # Arrange           
        [string] $processName = "abc";

        $mockProvider.Proxy.MockMethod("Provide", {
            param ([string] $parameter)

            return [List[TestDescriptor]]::new();
        });

        [ProcessTesterRunner] $sut = [ProcessTesterRunner]::new(
            $mockProvider.Instance, 
            $mockResolver.Instance,
            $mockEnv.Instance, 
            $mockTypeLookup.Instance,
            $mockLogger.Instance, 
            $config,
            $pesterRunner.Instance);

        # Act 
        $sut.Test($processName);

        # Assert
        $mockProvider.Proxy.CountCalls("Provide", (doing args -parameter $processName)) | Should -Be 1;

        $mockLogger.Proxy.CountCalls("LogInfo") | Should -Be 1;   

        $mockLogger.Proxy.CountCalls("LogInfo", (doing args -message "Found 0 candidate process test files.")) | Should -Be 1;

        $pesterRunner.Proxy.CountCalls("Run") | Should -Be 0;  
    }

    it "Cannot resolve process" {
        # Arrange           
        [string] $processName = "abc";

        $mockProvider.Proxy.MockMethod("Provide", {
            param ([string] $parameter)
            [List[TestDescriptor]] $descriptors = [List[TestDescriptor]]::new();

            [TestDescriptor] $descriptor1 = [TestDescriptor]::new();
            $descriptor1.TestType = [TestType]::Process;
            $descriptor1.Name = "$($processName)Tests";
            $descriptors.Add($descriptor1);

            [TestDescriptor] $descriptor2 = [TestDescriptor]::new();
            $descriptor2.TestType = [TestType]::Process;
            $descriptor2.Name = "$($processName)Tests";
            $descriptors.Add($descriptor2);

            return $descriptors;
        });

        $mockResolver.Proxy.MockMethod("Resolve", {
            param ([string] $module)

            return [ResolutionResult[ProcessDescriptor]]::new($false, [string]::Empty, [ProcessDescriptor]::new());
        });

        [ProcessTesterRunner] $sut = [ProcessTesterRunner]::new(
            $mockProvider.Instance, 
            $mockResolver.Instance,
            $mockEnv.Instance, 
            $mockTypeLookup.Instance,
            $mockLogger.Instance, 
            $config,
            $pesterRunner.Instance);

        # Act 
        $func = {
            $sut.Test($processName);
        };

        $func | Should -Throw;

        # Assert
        $mockProvider.Proxy.CountCalls("Provide", (doing args -parameter $processName)) | Should -Be 1;

        $mockLogger.Proxy.CountCalls("LogFatal", (doing args -message "Process $($processName) does not exist")) | Should -Be 1;  

        $mockLogger.Proxy.CountCalls("LogInfo") | Should -Be 1;   

        $mockLogger.Proxy.CountCalls("LogInfo", (doing args -message "Found 2 candidate process test files.")) | Should -Be 1;

        $pesterRunner.Proxy.CountCalls("Run") | Should -Be 0; 
    }

    it "Resolves Processes" {
        # Arrange           
        [string] $processName = "abc";

        $mockProvider.Proxy.MockMethod("Provide", {
            param ([string] $parameter)
            [List[TestDescriptor]] $descriptors = [List[TestDescriptor]]::new();

            [TestDescriptor] $descriptor1 = [TestDescriptor]::new();
            $descriptor1.TestType = [TestType]::Process;
            $descriptor1.Name = "ProcessNameTests";
            $descriptor1.Path = "ProcessNameTests.ps1";
            $descriptors.Add($descriptor1);

            [TestDescriptor] $descriptor2 = [TestDescriptor]::new();
            $descriptor2.TestType = [TestType]::Process;
            $descriptor2.Name = "ProcessNameTests";
            $descriptor2.Path = "ProcessNameTests.ps1";
            $descriptors.Add($descriptor2);

            return $descriptors;
        });

        $mockResolver.Proxy.MockMethod("Resolve", {
            param ([string] $module)

            [ProcessDescriptor] $descriptor = [ProcessDescriptor]::new();

            $descriptor.Path = "ProcessName.ps1";

            return [ResolutionResult[ProcessDescriptor]]::new($true, "ProcessName", $descriptor);
        });

        [ProxyResult] $mockReadProcessLocation = doing mock -type ([IReadProcessLocation]);

        [char] $sep = [DoFramework.Environment.Environment]::Separator;

        $mockReadProcessLocation.Proxy.MockMethod("Read", {
            [string] $currentDir = (Get-Location);

            return (Join-Path -ChildPath "$($sep)src$($sep)DoCli$($sep)Tests$($sep)E2E" -Path $currentDir);
        });

        [ProxyResult] $mockFileSystem = doing mock -type ([IFileManager]);

        $mockFileSystem.Proxy.MockMethod("FileExists", {
            param ([string] $path)

            return $true;
        });
        
        [ProxyResult] $mockProjectProvider = doing mock -type ([ISimpleDataProvider[ProjectContents]]);

        $mockProjectProvider.Proxy.MockMethod("Provide", {
            [ProjectContents] $contents = [ProjectContents]::new();

            $contents.Name = "Do";

            return $contents;
        });

        $mockTypeLookup.Proxy.MockMethod("Lookup", {
            param ([string] $name)

            return [object];
        });

        [DoFramework.Environment.Environment] $env = [DoFramework.Environment.Environment]::new(
            $mockReadProcessLocation.Instance,
            $mockProjectProvider.Instance
        );

        [ProcessTesterRunner] $sut = [ProcessTesterRunner]::new(
            $mockProvider.Instance, 
            $mockResolver.Instance,
            $env, 
            $mockTypeLookup.Instance,
            $mockLogger.Instance, 
            $config,
            $pesterRunner.Instance);

        # Act 
        $sut.Test($processName);

        # Assert
        $mockProvider.Proxy.CountCalls("Provide", (doing args -parameter $processName)) | Should -Be 1;

        $mockLogger.Proxy.CountCalls("LogFatal") | Should -Be 0;

        $mockLogger.Proxy.CountCalls("LogInfo") | Should -Be 4;   

        $mockLogger.Proxy.CountCalls("LogInfo", (doing args -message "Importing Process 'ProcessName'.")) | Should -Be 2;

        $mockLogger.Proxy.CountCalls("LogInfo", (doing args -message "Found 2 candidate process test files.")) | Should -Be 1;

        $mockLogger.Proxy.CountCalls("LogInfo", (doing args -message "Running 2 process test files.")) | Should -Be 1;

        $pesterRunner.Proxy.CountCalls("Run") | Should -Be 1;
        
        $mockTypeLookup.Proxy.CountCalls("Lookup") | Should -Be 2;
    }
}