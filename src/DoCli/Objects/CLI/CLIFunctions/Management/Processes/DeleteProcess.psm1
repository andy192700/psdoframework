using namespace DoFramework.CLI;
using namespace DoFramework.Services;
using namespace DoFramework.Domain;
using namespace DoFramework.Data;
using namespace DoFramework.Validators;
using namespace DoFramework.Testing;
using namespace System.Collections.Generic;

<#
.SYNOPSIS
Class for removing processes within the DoFramework environment.

.DESCRIPTION
The DeleteProcess class is designed to remove existing processes within the DoFramework 
environment. It handles the setup of parameters, environment checks, and deletion 
of process descriptors.
#>
class DeleteProcess : CLIFunction[DescriptorManagementDictionaryValidator] {
    <#
    .SYNOPSIS
    Initializes a new instance of the DeleteProcess class.

    .DESCRIPTION
    Constructor for the DeleteProcess class, which sets up the base name 
    for the command as "Delete-Process".
    #>
    DeleteProcess() : base("Delete-Process") {}

    [void] InvokeInternal([Dictionary[string, object]] $params, [IServiceContainer] $serviceContainer) {       
        [ServiceContainerExtensions]::AddParameters($serviceContainer, $params);
        [ServiceContainerExtensions]::CheckEnvironment($serviceContainer);

        [ProcessDescriptor] $descriptor = [ProcessDescriptor]::new();
    
        $descriptor.Name = $params["name"];
    
        $serviceContainer.GetService[IDataDeletor[ProcessDescriptor]]().Delete($descriptor);
        
        [string] $testName = "$($descriptor.Name)Tests";

        [IDataCollectionProvider[TestDescriptor, string]] $testProvider = $serviceContainer.GetService([IDataCollectionProvider[TestDescriptor, string]]);
        
        [List[TestDescriptor]] $tests = $testProvider.Provide($testName);

        [TestDescriptor[]] $moduleTests = $tests | Where-Object { $_.TestType -eq [TestType]::Process -and $_.name -eq $testName };

        [CLIFunctionParameters] $parameters = $serviceContainer.GetService[CLIFunctionParameters]();

        [bool] $isSilent = $parameters.ParseSwitch("silent");

        foreach ($moduleTest in $moduleTests) {
            doing delete-test -name $moduleTest.Name -silent $isSilent;
        }
    }
}
