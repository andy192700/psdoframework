Using namespace DoFramework.CLI;
Using namespace System.Collections.Generic;
Using namespace DoFramework.Services;
Using namespace DoFramework.Domain;
using namespace DoFramework.Data;
using namespace DoFramework.Validators;

<#
.SYNOPSIS
Class for removing tests within the DoFramework environment.

.DESCRIPTION
The DeleteTest class is designed to remove existing tests within the DoFramework 
environment. It handles the setup of parameters, environment checks, and deletion 
of test descriptors.
#>
class DeleteTest : CLIFunction[DescriptorManagementDictionaryValidator] {
    <#
    .SYNOPSIS
    Initializes a new instance of the RemoveTest class.

    .DESCRIPTION
    Constructor for the DeleteTest class, which sets up the base name 
    for the command as "Delete-Test".
    #>
    DeleteTest() : base("Delete-Test") {}

    [void] InvokeInternal([Dictionary[string, object]] $params, [IServiceContainer] $serviceContainer) {        
        [ServiceContainerExtensions]::AddParameters($serviceContainer, $params);
        [ServiceContainerExtensions]::CheckEnvironment($serviceContainer);

        [TestDescriptor] $descriptor = [TestDescriptor]::new();
    
        $descriptor.Name = $params["name"];
    
        $serviceContainer.GetService[IDataDeletor[TestDescriptor]]().Delete($descriptor);
    }
}
