using namespace DoFramework.Domain;
using namespace DoFramework.Environment;
using namespace DoFramework.Processing;
using namespace DoFramework.Services;
using namespace DoFramework.Types;
using namespace System.Reflection;

<#
.SYNOPSIS
Class for building process instances within the DoFramework environment.

.DESCRIPTION
The ProcessBuilder class is designed to create and configure process instances 
within the DoFramework environment. It sets up service containers, looks up 
process types, and constructs process instances with the necessary parameters.
#>
class ProcessBuilder : IProcessBuilder {
    <#
    .SYNOPSIS
    Initializes a new instance of the ProcessBuilder class.

    .DESCRIPTION
    Constructor for the ProcessBuilder class, which sets up the service container, 
    environment, lookup type, type validator, and logger for the process building.
    #>
    [IEnvironment] $Environment;
    [IReadOnlyServiceContainer] $ServiceContainer;
    [ILookupType[IProcess]] $LookupType;

    ProcessBuilder(
        [IEnvironment] $environment,
        [IReadOnlyServiceContainer] $serviceContainer,
        [ILookupType[IProcess]] $lookupType) {
        $this.Environment = $environment;
        $this.ServiceContainer = $serviceContainer;
        $this.LookupType = $lookupType;
    }

    <#
    .SYNOPSIS
    Builds a process instance based on the provided descriptor.

    .DESCRIPTION
    The Build method retrieves the process type, gathers the necessary constructor 
    parameters, and creates a new instance of the process within the DoFramework environment.
    #>
    [IProcess] Build([ProcessDescriptor] $descriptor) {
        . "$($this.Environment.ProcessesDir)$([DoFramework.Environment.Environment]::Separator)$($descriptor.Path)";

        [Type] $type = $this.LookupType.Lookup($descriptor.Name);

        [ParameterInfo[]] $parameters = $type.GetConstructors()[0].GetParameters();
        
        [object[]] $constructorParams = @();

        try {
            foreach($param in $parameters) {
                [object] $service = $this.ServiceContainer.GetService($param.ParameterType);
    
                $constructorParams += $service;
            }
        }
        catch {
            return $null;
        }
        
        return New-Object -TypeName $type.Name -ArgumentList $constructorParams;
    }
}
