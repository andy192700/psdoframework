using namespace DoFramework.CLI;
using namespace DoFramework.Services;
using namespace DoFramework.Processing;
using namespace DoFramework.Validators;
using namespace System.Collections.Generic;
using module "..\..\..\Processing\ProcessBuilder.psm1";

<#
.SYNOPSIS
Class for running processes within the DoFramework environment.

.DESCRIPTION
The RunProcess class is designed to execute processes within the DoFramework 
environment. It handles the setup of parameters, environment checks, and the 
dispatch of processing requests.
#>
class RunProcess : CLIFunction[DescriptorManagementDictionaryValidator, [IContext]] {
    <#
    .SYNOPSIS
    Initializes a new instance of the RunProcess class.

    .DESCRIPTION
    Constructor for the RunProcess class, which sets up the base name 
    for the command as "Run".
    #>
    RunProcess() : base("Run") {}

    [IContext] Invoke([Dictionary[string, object]] $params, [IServiceContainer] $serviceContainer) {      
        [ServiceContainerExtensions]::CheckEnvironment($serviceContainer); 
        [ServiceContainerExtensions]::ConsumeEnvFiles($serviceContainer);    
        [ServiceContainerExtensions]::AddParameters($serviceContainer, $params);
        [ServiceContainerExtensions]::AddProcessingServices($serviceContainer, [ProcessBuilder]);
        
        [IEntryPoint] $entryPoint = $serviceContainer.GetService[IEntryPoint]();

        return $entryPoint.Enter();
    }
}