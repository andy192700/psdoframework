using namespace DoFramework.CLI;
using namespace DoFramework.Data;
using namespace DoFramework.Domain;
using namespace DoFramework.Services;
using namespace DoFramework.Testing;
using namespace DoFramework.Validators;
using namespace System.Collections.Generic;

<#
.SYNOPSIS
Class for removing modules within the DoFramework environment.

.DESCRIPTION
The RemoveModule class is designed to remove existing modules within the DoFramework 
environment. It handles the setup of parameters, environment checks, and deletion 
of module descriptors.
#>
class RemoveModule : CLIFunction[DescriptorManagementDictionaryValidator] {
    <#
    .SYNOPSIS
    Initializes a new instance of the RemoveModule class.

    .DESCRIPTION
    Constructor for the RemoveModule class, which sets up the base name 
    for the command as "Remove-Module".
    #>
    RemoveModule() : base("Remove-Module") {}

    [void] InvokeInternal([Dictionary[string, object]] $params, [IServiceContainer] $serviceContainer) {        
        [ServiceContainerExtensions]::AddParameters($serviceContainer, $params);
        [ServiceContainerExtensions]::CheckEnvironment($serviceContainer);

        [ModuleDescriptor] $descriptor = [ModuleDescriptor]::new();
    
        $descriptor.Name = $params["name"];
    
        $serviceContainer.GetService[IDataDeletor[ModuleDescriptor]]().Delete($descriptor);

        [string] $testName = "$($descriptor.Name)Tests";

        [IDataCollectionProvider[TestDescriptor, string]] $testProvider = $serviceContainer.GetService([IDataCollectionProvider[TestDescriptor, string]]);
        
        [List[TestDescriptor]] $tests = $testProvider.Provide($testName);

        [TestDescriptor[]] $processTests = $tests | Where-Object { $_.TestType -eq [TestType]::Module -and $_.name -eq $testName};

        [CLIFunctionParameters] $parameters = $serviceContainer.GetService[CLIFunctionParameters]();

        [bool] $isSilent = $parameters.ParseSwitch("silent");

        foreach ($processTest in $processTests) {
            doing remove-test -name $processTest.Name -silent $isSilent;
        }
    }
}
