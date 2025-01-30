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
Class for running process tests within the DoFramework environment.

.DESCRIPTION
The ProcessTesterRunner class is designed to execute process tests within the 
DoFramework environment. It retrieves test descriptors, logs the process, and 
runs the process tests using the Pester testing framework.
#>
class ProcessTesterRunner : ITestRunner[ProcessDescriptor] {
    <#
    .SYNOPSIS
    Initializes a new instance of the ProcessTesterRunner class.

    .DESCRIPTION
    Constructor for the ProcessTesterRunner class, which sets up the test provider, 
    process resolver, environment, lookup type, logger, Pester configuration, 
    and Pester runner.
    #>
    [IResolver[ProcessDescriptor]] $ProcessResolver;
    [IDataCollectionProvider[TestDescriptor, string]] $TestProvider;
    [IEnvironment] $Environment;
    [ILookupType[IProcess]] $LookupType;
    [ILogger] $Logger;
    [PesterConfig] $PesterConfiguration;
    [IPesterRunner] $PesterRunner;

    ProcessTesterRunner(
        [IDataCollectionProvider[TestDescriptor, string]] $testProvider,
        [IResolver[ProcessDescriptor]] $processResolver, 
        [IEnvironment] $environment, 
        [ILookupType[IProcess]] $lookupType,
        [ILogger] $logger,
        [PesterConfig] $pesterConfiguration,
        [IPesterRunner] $pesterRunner) {
        $this.TestProvider = $testProvider;
        $this.ProcessResolver = $processResolver;
        $this.Environment = $environment;
        $this.LookupType = $lookupType;
        $this.Logger = $logger;
        $this.PesterConfiguration = $pesterConfiguration;
        $this.PesterRunner = $pesterRunner;
    }

    <#
    .SYNOPSIS
    Executes the process tests based on the specified filter.

    .DESCRIPTION
    The Test method retrieves process test descriptors based on the provided filter, 
    logs the number of test files found, collects the test paths, and runs the tests 
    using the Pester framework.
    #>
    [void] Test([string] $filter) {
        [List[TestDescriptor]] $tests = $this.TestProvider.Provide($filter);

        [TestDescriptor[]] $processTests = $tests | Where-Object { $_.TestType -eq [TestType]::Process };

        $this.Logger.LogInfo("Found $($processTests.Length) candidate process test files.");

        [List[string]] $testPaths = [List[string]]::new();
        
        [List[string]] $processPaths = [List[string]]::new();

        foreach ($test in $processTests) {
            [string] $processName = $test.Name.Substring(0, $test.Name.Length - 5);

            [ResolutionResult[ProcessDescriptor]] $resultProcess = $this.ProcessResolver.Resolve($processName);

            if (!$resultProcess.Exists) {
                $this.Logger.LogFatal("Process $processName does not exist");

                throw;
            }
            else {         
                $this.Logger.LogInfo("Importing Process '$processName'.");
            
                [ProcessDescriptor] $descriptor = $resultProcess.Descriptor;
    
                [string] $processPath = "$($this.Environment.ProcessesDir)$([DoFramework.Environment.Environment]::Separator)$($descriptor.Path)";
                
                . $processPath;

                $processPaths.Add($processPath);
    
                $this.LookupType.Lookup($descriptor.Name);
        
                $testPaths.Add("$($this.Environment.TestsDir)$([DoFramework.Environment.Environment]::Separator)$($test.Path)");
            }
        }

        if ($testPaths.Count -gt 0) {
            $this.Logger.LogInfo("Running $($testPaths.Count) process test files.");
            
            $pesterConfig = $this.PesterConfiguration.GetConfiguration($testPaths.ToArray(), "Process");
    
            $this.PesterRunner.Run($pesterConfig, $processPaths.ToArray());
        }
    }
}
