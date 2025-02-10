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
using module "..\..\..\..\Objects\Testing\ComposerTesterRunner.psm1";

Describe 'ComposerTestRunnerTests' {
    BeforeEach {
        [ProxyResult] $mockEnv = doing mock -type ([IEnvironment]);

        [ProxyResult] $script:mockLogger = doing mock -type ([ILogger]);

        [ProxyResult] $script:mockProvider = doing mock -type ([IDataCollectionProvider[TestDescriptor, string]]);
        
        [ProxyResult] $script:pesterRunner = doing mock -type ([IPesterRunner]);

        [ProxyResult] $script:mockResolver = doing mock -type ([IResolver[ComposerDescriptor]]);

        [ProxyResult] $script:mockTypeLookup = doing mock -type ([ILookupType[IComposer]]);

        [CLIFunctionParameters] $params = [CLIFunctionParameters]::new();

        $params.Parameters = [Dictionary[string, object]]::new();

        [PesterConfig] $script:config = [PesterConfig]::new($mockEnv.Instance, $params);
    }

    it 'No tests found, pester will not invoke and logging is correct' {
        # Arrange           
        [string] $composerName = "abc";

        $mockProvider.Proxy.MockMethod("Provide", {
            param ([string] $parameter)

            return [List[TestDescriptor]]::new();
        });

        [ComposerTesterRunner] $sut = [ComposerTesterRunner]::new(
            $mockProvider.Instance, 
            $mockResolver.Instance,
            $mockEnv.Instance, 
            $mockTypeLookup.Instance,
            $mockLogger.Instance, 
            $config,
            $pesterRunner.Instance);

        # Act 
        $sut.Test($composerName);

        # Assert
        $mockProvider.Proxy.CountCalls("Provide", (doing args -parameter $composerName)) | Should -Be 1;

        $mockLogger.Proxy.CountCalls("LogInfo") | Should -Be 1;   

        $mockLogger.Proxy.CountCalls("LogInfo", (doing args -message "Found 0 candidate composer test files.")) | Should -Be 1;

        $pesterRunner.Proxy.CountCalls("Run") | Should -Be 0;  
    }

    it "Cannot resolve composer" {
        # Arrange           
        [string] $composerName = "abc";

        $mockProvider.Proxy.MockMethod("Provide", {
            param ([string] $parameter)
            [List[TestDescriptor]] $descriptors = [List[TestDescriptor]]::new();

            [TestDescriptor] $descriptor1 = [TestDescriptor]::new();
            $descriptor1.TestType = [TestType]::Composer;
            $descriptor1.Name = "$($composerName)Tests";
            $descriptors.Add($descriptor1);

            [TestDescriptor] $descriptor2 = [TestDescriptor]::new();
            $descriptor2.TestType = [TestType]::Composer;
            $descriptor2.Name = "$($composerName)Tests";
            $descriptors.Add($descriptor2);

            return $descriptors;
        });

        $mockResolver.Proxy.MockMethod("Resolve", {
            param ([string] $module)

            return [ResolutionResult[ComposerDescriptor]]::new($false, [string]::Empty, [ComposerDescriptor]::new());
        });

        [ComposerTesterRunner] $sut = [ComposerTesterRunner]::new(
            $mockProvider.Instance, 
            $mockResolver.Instance,
            $mockEnv.Instance, 
            $mockTypeLookup.Instance,
            $mockLogger.Instance, 
            $config,
            $pesterRunner.Instance);

        # Act 
        $func = {
            $sut.Test($composerName);
        };

        $func | Should -Throw;

        # Assert
        $mockProvider.Proxy.CountCalls("Provide", (doing args -parameter $composerName)) | Should -Be 1;

        $mockLogger.Proxy.CountCalls("LogFatal", (doing args -message "Composer $($composerName) does not exist")) | Should -Be 1;  

        $mockLogger.Proxy.CountCalls("LogInfo") | Should -Be 1;   

        $mockLogger.Proxy.CountCalls("LogInfo", (doing args -message "Found 2 candidate composer test files.")) | Should -Be 1;

        $pesterRunner.Proxy.CountCalls("Run") | Should -Be 0; 
    }

    it "Resolves Composer" {
        # Arrange           
        [string] $composerName = "abc";

        $mockProvider.Proxy.MockMethod("Provide", {
            param ([string] $parameter)
            [List[TestDescriptor]] $descriptors = [List[TestDescriptor]]::new();

            [TestDescriptor] $descriptor1 = [TestDescriptor]::new();
            $descriptor1.TestType = [TestType]::Composer;
            $descriptor1.Name = "TestComposerTests";
            $descriptor1.Path = "TestComposerTests.ps1";
            $descriptors.Add($descriptor1);

            [TestDescriptor] $descriptor2 = [TestDescriptor]::new();
            $descriptor2.TestType = [TestType]::Composer;
            $descriptor2.Name = "TestComposerTests";
            $descriptor2.Path = "TestComposerTests.ps1";
            $descriptors.Add($descriptor2);

            return $descriptors;
        });

        $mockResolver.Proxy.MockMethod("Resolve", {
            param ([string] $module)

            [ComposerDescriptor] $descriptor = [ComposerDescriptor]::new();

            $descriptor.Path = "TestComposer.ps1";

            return [ResolutionResult[ComposerDescriptor]]::new($true, "TestComposer", $descriptor);
        });

        [ProxyResult] $mockReadProcessLocation = doing mock -type ([IReadProcessLocation]);

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

        [char] $sep = [DoFramework.Environment.Environment]::Separator;

        [ComposerTesterRunner] $sut = [ComposerTesterRunner]::new(
            $mockProvider.Instance, 
            $mockResolver.Instance,
            $env, 
            $mockTypeLookup.Instance,
            $mockLogger.Instance, 
            $config,
            $pesterRunner.Instance);

        # Act 
        $sut.Test($composerName);

        # Assert
        $mockProvider.Proxy.CountCalls("Provide", (doing args -parameter $composerName)) | Should -Be 1;

        $mockLogger.Proxy.CountCalls("LogFatal") | Should -Be 0;

        $mockLogger.Proxy.CountCalls("LogInfo") | Should -Be 4;   

        $mockLogger.Proxy.CountCalls("LogInfo", (doing args -message "Importing Composer 'TestComposer'.")) | Should -Be 2;

        $mockLogger.Proxy.CountCalls("LogInfo", (doing args -message "Found 2 candidate composer test files.")) | Should -Be 1;

        $mockLogger.Proxy.CountCalls("LogInfo", (doing args -message "Running 2 composer test files.")) | Should -Be 1;

        $pesterRunner.Proxy.CountCalls("Run") | Should -Be 1;
        
        $mockTypeLookup.Proxy.CountCalls("Lookup") | Should -Be 2;
    }
}