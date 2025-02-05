using namespace DoFramework.Testing;
using namespace DoFramework.Domain;
using namespace DoFramework.Data;
using namespace DoFramework.Logging;
using namespace DoFramework.Processing;
using namespace DoFramework.Environment;
using namespace DoFramework.Types;
using namespace DoFramework.Validators;
using namespace System.Collections.Generic;
using module ".\PesterConfig.psm1";
using module ".\PesterRunner.psm1";

<#
.SYNOPSIS
Class for running composer tests within the DoFramework environment.

.DESCRIPTION
The ComposerTesterRunner class is designed to execute composer tests within the 
DoFramework environment. It retrieves test descriptors, logs the composer, and 
runs the composer tests using the Pester testing framework.
#>
class ComposerTesterRunner : ITestRunner[ComposerDescriptor] {
    <#
    .SYNOPSIS
    Initializes a new instance of the ComposerTesterRunner class.

    .DESCRIPTION
    Constructor for the ComposerTesterRunner class, which sets up the test provider, 
    composer resolver, environment, lookup type, logger, Pester configuration, 
    and Pester runner.
    #>
    [IResolver[ComposerDescriptor]] $ComposerResolver;
    [IDataCollectionProvider[TestDescriptor, string]] $TestProvider;
    [IEnvironment] $Environment;
    [ILookupType[IComposer]] $LookupType;
    [ILogger] $Logger;
    [PesterConfig] $PesterConfiguration;
    [IPesterRunner] $PesterRunner;

    ComposerTesterRunner(
        [IDataCollectionProvider[TestDescriptor, string]] $testProvider,
        [IResolver[ComposerDescriptor]] $composerResolver, 
        [IEnvironment] $environment, 
        [ILookupType[IComposer]] $lookupType,
        [ILogger] $logger,
        [PesterConfig] $pesterConfiguration,
        [IPesterRunner] $pesterRunner) {
        $this.TestProvider = $testProvider;
        $this.ComposerResolver = $composerResolver;
        $this.Environment = $environment;
        $this.LookupType = $lookupType;
        $this.Logger = $logger;
        $this.PesterConfiguration = $pesterConfiguration;
        $this.PesterRunner = $pesterRunner;
    }

    <#
    .SYNOPSIS
    Executes the composer tests based on the specified filter.

    .DESCRIPTION
    The Test method retrieves composer test descriptors based on the provided filter, 
    logs the number of test files found, collects the test paths, and runs the tests 
    using the Pester framework.
    #>
    [void] Test([string] $filter) {
        [List[TestDescriptor]] $tests = $this.TestProvider.Provide($filter);

        [TestDescriptor[]] $composerTests = $tests | Where-Object { $_.TestType -eq [TestType]::Composer };

        $this.Logger.LogInfo("Found $($composerTests.Length) candidate composer test files.");

        [List[string]] $testPaths = [List[string]]::new();
        
        [List[string]] $composerPaths = [List[string]]::new();

        foreach ($test in $composerTests) {
            [string] $composerName = $test.Name.Substring(0, $test.Name.Length - 5);

            [ResolutionResult[ComposerDescriptor]] $resultComposer = $this.ComposerResolver.Resolve($composerName);

            if (!$resultComposer.Exists) {
                $this.Logger.LogFatal("Composer $composerName does not exist");

                throw;
            }
            else {         
                $this.Logger.LogInfo("Importing Composer '$composerName'.");
            
                [ComposerDescriptor] $descriptor = $resultComposer.Descriptor;
    
                [string] $composerPath = "$($this.Environment.ComposersDir)$([DoFramework.Environment.Environment]::Separator)$($descriptor.Path)";
                
                . $composerPath;

                $composerPaths.Add($composerPath);
    
                $this.LookupType.Lookup($descriptor.Name);
        
                $testPaths.Add("$($this.Environment.TestsDir)$([DoFramework.Environment.Environment]::Separator)$($test.Path)");
            }
        }

        if ($testPaths.Count -gt 0) {
            $this.Logger.LogInfo("Running $($testPaths.Count) composer test files.");
            
            $pesterConfig = $this.PesterConfiguration.GetConfiguration($testPaths.ToArray(), "Composer");
    
            $this.PesterRunner.Run($pesterConfig, $composerPaths.ToArray());
        }
    }
}
