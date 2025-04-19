using namespace DoFramework.CLI;
using namespace DoFramework.Data;
using namespace DoFramework.Domain;
using namespace DoFramework.Services;
using namespace DoFramework.Testing;
using namespace DoFramework.Validators;
using namespace System.Collections.Generic;

<#
.SYNOPSIS
Class for removing Composers within the DoFramework environment.

.DESCRIPTION
The DeleteComposer class is designed to remove existing Composers within the DoFramework 
environment. It handles the setup of parameters, environment checks, and deletion 
of Composer descriptors.
#>
class DeleteComposer : CLIFunction[DescriptorManagementDictionaryValidator] {
    <#
    .SYNOPSIS
    Initializes a new instance of the DeleteComposer class.

    .DESCRIPTION
    Constructor for the DeleteComposer class, which sets up the base name 
    for the command as "delete-composer".
    #>
    DeleteComposer() : base("delete-composer") {}

    [void] InvokeInternal([Dictionary[string, object]] $params, [IServiceContainer] $serviceContainer) {        
        [ServiceContainerExtensions]::AddParameters($serviceContainer, $params);
        [ServiceContainerExtensions]::CheckEnvironment($serviceContainer);

        [ComposerDescriptor] $descriptor = [ComposerDescriptor]::new();
    
        $descriptor.Name = $params["name"];
    
        $serviceContainer.GetService[IDataDeletor[ComposerDescriptor]]().Delete($descriptor);

        [string] $testName = "$($descriptor.Name)Tests";

        [IDataCollectionProvider[TestDescriptor, string]] $testProvider = $serviceContainer.GetService([IDataCollectionProvider[TestDescriptor, string]]);
        
        [List[TestDescriptor]] $tests = $testProvider.Provide($testName);

        [TestDescriptor[]] $processTests = $tests | Where-Object { $_.TestType -eq [TestType]::Composer -and $_.name -eq $testName};

        [CLIFunctionParameters] $parameters = $serviceContainer.GetService[CLIFunctionParameters]();

        [bool] $isSilent = $parameters.ParseSwitch("silent");

        foreach ($processTest in $processTests) {
            doing delete-test -name $processTest.Name -silent $isSilent;
        }
    }
}
