using namespace DoFramework.CLI;
using namespace DoFramework.Services;
using namespace DoFramework.Processing;
using namespace DoFramework.Validators;
using namespace System.Collections.Generic;

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
    for the command as "Run-Process".
    #>
    RunProcess() : base("Run-Process") {}

    [IContext] Invoke([Dictionary[string, object]] $params, [IServiceContainer] $serviceContainer) {        
        [ServiceContainerExtensions]::AddParameters($serviceContainer, $params);
        [ServiceContainerExtensions]::CheckEnvironment($serviceContainer);
        [ServiceContainerExtensions]::ConsumeEnvFiles($serviceContainer);
        
        [IProcessingRequest] $request = [ProcessingRequest]::new($params["name"], $params);
    
        $serviceContainer.GetService[IProcessDispatcher]().Dispatch($request);

        [IContext] $context = $serviceContainer.GetService[IContext]();

        $context.Session.CurrentProcessName = [string]::Empty;

        [CLIFunctionParameters] $cliParams = $serviceContainer.GetService([CLIFunctionParameters]);

        [IContext] $output = if ($cliParams.ParseSwitch("doOutput")) { $context } else { $null };

        return $output;
    }
}
