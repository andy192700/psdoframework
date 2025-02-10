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
Class for adding Composers within the DoFramework environment.

.DESCRIPTION
The NewComposer class is designed to add new Composers within the DoFramework 
environment. It handles the setup of parameters, environment checks, and 
creation of Composer descriptors.
#>
class NewComposer : CLIFunction[DescriptorManagementDictionaryValidator] {
    <#
    .SYNOPSIS
    Initializes a new instance of the AddComposer class.

    .DESCRIPTION
    Constructor for the NewComposer class, which sets up the base name 
    for the command as "New-Composer".
    #>
    NewComposer() : base("New-Composer") {}

    [void] InvokeInternal([Dictionary[string, object]] $params, [IServiceContainer] $serviceContainer) {
        [ServiceContainerExtensions]::AddParameters($serviceContainer, $params);
        [ServiceContainerExtensions]::CheckEnvironment($serviceContainer);

        [IMapper[string, ComposerDescriptor]] $mapper = $serviceContainer.GetService[IMapper[string, ComposerDescriptor]]();
    
        [ComposerDescriptor] $descriptor = $mapper.Map("$($params["name"]).ps1");

        [IValidator[IDescriptor]] $validator = $serviceContainer.GetService[IValidator[IDescriptor]]();
        
        [IValidationResult] $result = $validator.Validate($descriptor);

        if ($result.IsValid) {        
            $serviceContainer.GetService[IDescriptorFileCreator[ComposerDescriptor]]().Create($descriptor);

            $serviceContainer.GetService[IDataCreator[ComposerDescriptor]]().Create($descriptor);

            [CLIFunctionParameters] $cliParams = $serviceContainer.GetService([CLIFunctionParameters]);
    
            [bool] $isSilent = $cliParams.ParseSwitch("silent");
    
            if ($cliParams.ParseSwitch("addTests")) { 
                doing Add-Test -name "$($params["name"])Tests" -forComposer -silent $isSilent;
            }
        }
        else {
            [IValidationErrorWriter] $errorWriter = $serviceContainer.GetService[IValidationErrorWriter]();

            $errorWriter.Write($result);
        }
    }
}
