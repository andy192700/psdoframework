using namespace DoFramework.CLI;
using namespace DoFramework.Validators;
using namespace DoFramework.Services;
using namespace DoFramework.Testing;
using namespace DoFramework.Domain;
using namespace DoFramework.Logging;
using namespace System.Collections.Generic;
using module "..\..\..\Processing\ComposerBuilder.psm1";
using module "..\..\..\Processing\ProcessBuilder.psm1";

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
    for the command as "Test".
    #>
    RunTests() : base("Test") {}

    [void] InvokeInternal([Dictionary[string, object]] $params, [IServiceContainer] $serviceContainer) {        
        [ServiceContainerExtensions]::AddParameters($serviceContainer, $params);
        [ServiceContainerExtensions]::CheckEnvironment($serviceContainer);
        [ServiceContainerExtensions]::AddComposerServices($serviceContainer, [ComposerBuilder]);
        [ServiceContainerExtensions]::AddProcessingServices($serviceContainer, [ProcessBuilder]);
    
        [ILogger] $logger = $serviceContainer.GetService[ILogger]();
        
        $logger.LogInfo("Executing test runners with filter = $($params["filter"])");

        [CLIFunctionParameters] $cliParams = $serviceContainer.GetService([CLIFunctionParameters]);

        [bool] $runProcessTests = $cliParams.ParseSwitch("forProcesses");
        [bool] $runModulesTests = $cliParams.ParseSwitch("forModules");
        [bool] $runComposerTests = $cliParams.ParseSwitch("forComposers");
        [bool] $runAllTests = -not $runProcessTests -and -not $runModulesTests -and -not $runComposerTests;

        if ($runProcessTests -or $runAllTests) {
            [ITestRunner[ProcessDescriptor]] $processTestRunner = $serviceContainer.GetService[ITestRunner[ProcessDescriptor]]();

            $processTestRunner.Test($params["filter"]);
        }
        
        if ($runModulesTests -or $runAllTests) {
            [ITestRunner[ModuleDescriptor]] $moduleTestRunner = $serviceContainer.GetService[ITestRunner[ModuleDescriptor]]();

            $moduleTestRunner.Test($params["filter"]);
        }
        
        if ($runComposerTests -or $runAllTests) {
            [ITestRunner[ComposerDescriptor]] $composerTestRunner = $serviceContainer.GetService[ITestRunner[ComposerDescriptor]]();

            $composerTestRunner.Test($params["filter"]);
        }
    }
}
