using namespace DoFramework.CLI;
using namespace DoFramework.Mappers;
using namespace DoFramework.Domain;
using namespace DoFramework.Environment;
using namespace DoFramework.FileSystem;
using namespace DoFramework.Services;
using namespace DoFramework.Data;
using namespace DoFramework.Validators;
using namespace DoFramework.Testing;
using namespace System.Collections.Generic;

<#
.SYNOPSIS
Class for adding tests within the DoFramework environment.

.DESCRIPTION
The NewTest class and its derived classes are designed to add new tests within 
the DoFramework environment. They handle the setup of parameters, environment 
checks, and creation of test descriptors.
#>
class NewTest : CLIFunction[DescriptorManagementDictionaryValidator] {
    <#
    .SYNOPSIS
    Initializes a new instance of the NewTest class.

    .DESCRIPTION
    Constructor for the NewTest class, which sets up the base name 
    for the command based on the provided function name.
    #>
    NewTest() : base("new-test") {}

    [void] AppendValues([Dictionary[string, object]] $params) {}

    <#
    .SYNOPSIS
    Invokes the process of adding a test.

    .DESCRIPTION
    The InvokeInternal method sets up parameters, checks the environment, maps the test 
    descriptor, validates it, and creates the test descriptor if valid.
    #>
    [void] InvokeInternal([Dictionary[string, object]] $params, [IServiceContainer] $serviceContainer) {
        $this.AppendValues($params);
        
        [ServiceContainerExtensions]::AddParameters($serviceContainer, $params);
        [ServiceContainerExtensions]::CheckEnvironment($serviceContainer);

        [IMapper[string, TestDescriptor]] $mapper = $serviceContainer.GetService[IMapper[string, TestDescriptor]]();

        [CLIFunctionParameters] $cliParams = $serviceContainer.GetService([CLIFunctionParameters]);
    
        if ($cliParams.ParseSwitch("forProcess")) { 
            $params["testType"] = [TestType]::Process;
            $params["PathPrefix"] = "Processes";
        }

        if ($cliParams.ParseSwitch("forModule")) { 
            $params["testType"] = [TestType]::Module;
            $params["PathPrefix"] = "Modules";
        }

        if ($cliParams.ParseSwitch("forComposer")) { 
            $params["testType"] = [TestType]::Composer;
            $params["PathPrefix"] = "Composers";
        }

        [TestDescriptor] $descriptor = $mapper.Map("$($params["PathPrefix"])$([DoFramework.Environment.Environment]::Separator)$($params["name"]).ps1");
    
        $descriptor.TestType = $params["testType"];

        [TestDescriptorCreatorValidator] $validator = $serviceContainer.GetService[TestDescriptorCreatorValidator]();
        
        [IValidationResult] $result = $validator.Validate($descriptor);

        if ($result.IsValid) {
            $serviceContainer.GetService[IDescriptorFileCreator[TestDescriptor]]().Create($descriptor);

            $serviceContainer.GetService[IDataCreator[TestDescriptor]]().Create($descriptor);
        }
        else {
            [IValidationErrorWriter] $errorWriter = $serviceContainer.GetService[IValidationErrorWriter]();

            $errorWriter.Write($result);
        }
    }
}