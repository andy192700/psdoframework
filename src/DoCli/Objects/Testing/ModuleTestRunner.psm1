using namespace DoFramework.Testing;
using namespace DoFramework.Domain;
using namespace DoFramework.Data;
using namespace DoFramework.Logging;
using namespace DoFramework.Environment;
using namespace System.Collections.Generic;
using module ".\PesterConfig.psm1";
using module ".\PesterRunner.psm1";

<#
.SYNOPSIS
Class for running module tests within the DoFramework environment.

.DESCRIPTION
The ModuleTestRunner class is designed to execute module tests within the 
DoFramework environment. It retrieves test descriptors, logs the process, and 
runs the module tests using the Pester testing framework.
#>
class ModuleTestRunner : ITestRunner[ModuleDescriptor] {
    <#
    .SYNOPSIS
    Initializes a new instance of the ModuleTestRunner class.

    .DESCRIPTION
    Constructor for the ModuleTestRunner class, which sets up the test provider, 
    logger, environment, Pester configuration, and Pester runner.
    #>
    [IDataCollectionProvider[TestDescriptor, string]] $TestProvider;
    [ILogger] $Logger;
    [IEnvironment] $Environment;
    [PesterConfig] $PesterConfiguration;
    [IPesterRunner] $PesterRunner;

    ModuleTestRunner(
        [IDataCollectionProvider[TestDescriptor, string]] $testProvider,
        [ILogger] $logger,
        [IEnvironment] $environment,
        [PesterConfig] $pesterConfiguration,
        [IPesterRunner] $pesterRunner) {
        $this.TestProvider = $testProvider;
        $this.Logger = $logger;
        $this.Environment = $environment;
        $this.PesterConfiguration = $pesterConfiguration;
        $this.PesterRunner = $pesterRunner;
    }

    <#
    .SYNOPSIS
    Executes the module tests based on the specified filter.

    .DESCRIPTION
    The Test method retrieves module test descriptors based on the provided filter, 
    logs the number of test files found, collects the test paths, and runs the tests 
    using the Pester framework.
    #>
    [void] Test([string] $filter) {
        [List[TestDescriptor]] $tests = $this.TestProvider.Provide($filter);

        [TestDescriptor[]] $moduleTests = $tests | Where-Object { $_.TestType -eq [TestType]::Module };

        $this.Logger.LogInfo("Found $($moduleTests.Length) candidate module test files.");

        [List[string]] $testPaths = [List[string]]::new();

        foreach ($test in $moduleTests) {
            $testPaths.Add("$($this.Environment.TestsDir)$([DoFramework.Environment.Environment]::Separator)$($test.Path)");
        }

        if ($testPaths.Count -gt 0) {
            $this.Logger.LogInfo("Running $($testPaths.Count) module test files.");

            $pesterConfig = $this.PesterConfiguration.GetConfiguration($testPaths.ToArray(), "Module");

            $this.PesterRunner.Run($pesterConfig);
        }
    }
}
