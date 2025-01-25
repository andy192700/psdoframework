using namespace DoFramework.CLI;
using namespace DoFramework.Validators;
using namespace DoFramework.Services;
using namespace DoFramework.Testing;
using namespace DoFramework.Domain;
using namespace DoFramework.Logging;
using namespace System.Collections.Generic;

<#
.SYNOPSIS
Class for running all tests within the DoFramework environment.

.DESCRIPTION
The RunTests class is designed to execute all tests (module and process) within the 
DoFramework environment. It handles the setup of parameters, environment checks, 
refreshes module states, and runs the tests using the specified filter.
#>
class RunTests : CLIFunction[TestRunnerDictionaryValidator] {
    <#
    .SYNOPSIS
    Initializes a new instance of the RunTests class.

    .DESCRIPTION
    Constructor for the RunTests class, which sets up the base name 
    for the command as "Run-Tests".
    #>
    RunTests() : base("Run-Tests") {}

    [void] InvokeInternal([Dictionary[string, object]] $params, [IServiceContainer] $serviceContainer) {        
        [ServiceContainerExtensions]::AddParameters($serviceContainer, $params);
        [ServiceContainerExtensions]::CheckEnvironment($serviceContainer);
    
        [ILogger] $logger = $serviceContainer.GetService[ILogger]();
        
        $logger.LogInfo("Executing test runners with filter = $($params["filter"])");

        [CLIFunctionParameters] $cliParams = $serviceContainer.GetService([CLIFunctionParameters]);

        [bool] $runProcessTests = $cliParams.ParseSwitch("forProcesses");
        [bool] $runModulesTests = $cliParams.ParseSwitch("forModules");
        [bool] $runAllTests = -not $runProcessTests -and -not $runModulesTests;

        if ($runProcessTests -or $runAllTests) {
            [ITestRunner[ProcessDescriptor]] $processTestRunner = $serviceContainer.GetService[ITestRunner[ProcessDescriptor]]();

            $processTestRunner.Test($params["filter"]);
        }
        
        if ($runModulesTests -or $runAllTests) {
            [ITestRunner[ModuleDescriptor]] $moduleTestRunner = $serviceContainer.GetService[ITestRunner[ModuleDescriptor]]();

            $moduleTestRunner.Test($params["filter"]);
        }
    }
}
