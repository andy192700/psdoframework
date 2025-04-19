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
Class for adding modules within the DoFramework environment.

.DESCRIPTION
The NewModule class is designed to add new modules within the DoFramework 
environment. It handles the setup of parameters, environment checks, and 
creation of module descriptors.
#>
class NewModule : CLIFunction[DescriptorManagementDictionaryValidator] {
    <#
    .SYNOPSIS
    Initializes a new instance of the NewModule class.

    .DESCRIPTION
    Constructor for the NewModule class, which sets up the base name 
    for the command as "New-Module".
    #>
    NewModule() : base("New-Module") {}

    [void] InvokeInternal([Dictionary[string, object]] $params, [IServiceContainer] $serviceContainer) {
        [ServiceContainerExtensions]::AddParameters($serviceContainer, $params);
        [ServiceContainerExtensions]::CheckEnvironment($serviceContainer);

        [IMapper[string, ModuleDescriptor]] $mapper = $serviceContainer.GetService[IMapper[string, ModuleDescriptor]]();
    
        [ModuleDescriptor] $descriptor = $mapper.Map("$($params["name"]).psm1");

        [IValidator[IDescriptor]] $validator = $serviceContainer.GetService[IValidator[IDescriptor]]();
        
        [IValidationResult] $result = $validator.Validate($descriptor);

        if ($result.IsValid) {        
            $serviceContainer.GetService[IDescriptorFileCreator[ModuleDescriptor]]().Create($descriptor);

            $serviceContainer.GetService[IDataCreator[ModuleDescriptor]]().Create($descriptor);

            [CLIFunctionParameters] $cliParams = $serviceContainer.GetService([CLIFunctionParameters]);
    
            [bool] $isSilent = $cliParams.ParseSwitch("silent");
    
            if ($cliParams.ParseSwitch("addTests")) { 
                doing new-test -name "$($params["name"])Tests" -forModule -silent $isSilent;
            }
        }
        else {
            [IValidationErrorWriter] $errorWriter = $serviceContainer.GetService[IValidationErrorWriter]();

            $errorWriter.Write($result);
        }
    }
}
