using namespace DoFramework.CLI;
using namespace DoFramework.FileSystem;
using namespace DoFramework.Validators;
using namespace DoFramework.Services;
using namespace System.Collections.Generic;
<#
.SYNOPSIS
Class for creating a new DoFile in the DoFramework environment.
.DESCRIPTION
The NewDoFile class handles the creation of a new DoFile by invoking the necessary 
services for DoFile creation.
#>
class NewDoFile : CLIFunction[EmptyCLIFunctionDictionaryValidator] {

    <#
    .SYNOPSIS
    Initializes the NewDoFile class.
    .DESCRIPTION
    Constructor for the NewDoFile class, setting the base command name as "new-dofile".
    #>
    NewDoFile() : base("new-dofile") {}

    <#
    .SYNOPSIS
    Invokes the process of creating a new DoFile.
    .DESCRIPTION
    The InvokeInternal method adds parameters to the service container and calls the 
    IDoFileCreator service to create the new DoFile.
    #>
    [void] InvokeInternal([Dictionary[string, object]] $params, [IServiceContainer] $serviceContainer) {
        [ServiceContainerExtensions]::AddParameters($serviceContainer, $params);

        [IDoFileCreator] $creator = $serviceContainer.GetService([IDoFileCreator]);

        $creator.Create();
    }
}
