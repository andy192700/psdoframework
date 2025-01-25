using namespace DoFramework.CLI;
using namespace DoFramework.Mappers;
using namespace DoFramework.Domain;
using namespace DoFramework.Environment;
using namespace DoFramework.FileSystem;
using namespace DoFramework.Services;
using namespace DoFramework.Data;
using namespace DoFramework.Validators;
using namespace System.Collections.Generic;

<#
.SYNOPSIS
Class for adding processes within the DoFramework environment.

.DESCRIPTION
The AddProcess class is designed to add new processes within the DoFramework 
environment. It handles the setup of parameters, environment checks, and 
creation of process descriptors.
#>
class AddProcess : CLIFunction[DescriptorManagementDictionaryValidator] {
    <#
    .SYNOPSIS
    Initializes a new instance of the AddProcess class.

    .DESCRIPTION
    Constructor for the AddProcess class, which sets up the base name 
    for the command as "Add-Process".
    #>
    AddProcess() : base("Add-Process") {}

    [void] InvokeInternal([Dictionary[string, object]] $params, [IServiceContainer] $serviceContainer) {        
        [ServiceContainerExtensions]::AddParameters($serviceContainer, $params);
        [ServiceContainerExtensions]::CheckEnvironment($serviceContainer);

        [IMapper[string, ProcessDescriptor]] $mapper = $serviceContainer.GetService[IMapper[string, ProcessDescriptor]]();
    
        [ProcessDescriptor] $descriptor = $mapper.Map("$($params["name"]).ps1");

        [IValidator[IDescriptor]] $validator = $serviceContainer.GetService[IValidator[IDescriptor]]();
        
        [IValidationResult] $result = $validator.Validate($descriptor);

        if ($result.IsValid) {
            $serviceContainer.GetService[IDescriptorFileCreator[ProcessDescriptor]]().Create($descriptor);

            $serviceContainer.GetService[IDataCreator[ProcessDescriptor]]().Create($descriptor);

            [CLIFunctionParameters] $cliParams = $serviceContainer.GetService([CLIFunctionParameters]);
    
            [bool] $isSilent = $cliParams.ParseSwitch("silent");
    
            if ($cliParams.ParseSwitch("addTests")) { 
                doing Add-Test -name "$($params["name"])Tests" -forProcess -silent $isSilent;
            }
        }
        else {
            [IValidationErrorWriter] $errorWriter = $serviceContainer.GetService[IValidationErrorWriter]();

            $errorWriter.Write($result);
        }
    }
}
