using namespace DoFramework.Domain;
using namespace DoFramework.Environment;
using namespace DoFramework.Logging;
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
    [ILogger] $Logger;

    ProcessBuilder(
        [IEnvironment] $environment,
        [IReadOnlyServiceContainer] $serviceContainer,
        [ILookupType[IProcess]] $lookupType,
        [ILogger] $logger) {
        $this.Environment = $environment;
        $this.ServiceContainer = $serviceContainer;
        $this.LookupType = $lookupType;
        $this.Logger = $logger;
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

        [ParameterInfo[]] $parameters = $null;

        [ConstructorInfo[]] $constructors = $type.GetConstructors() | Sort-Object { $_.GetParameters().Count } -Descending;

        foreach($constructor in $constructors) {
            [ParameterInfo[]] $constructorParameters = $constructor.GetParameters();

            [int] $serviceCount = 0;

            foreach ($constructorParameter in $constructorParameters) {
                if ($this.ServiceContainer.HasService($constructorParameter.ParameterType)) {
                    $serviceCount++;
                }
            }
            
            if ($serviceCount -eq $constructorParameters.Count) {
                $parameters = $constructorParameters;
                break;
            }
        }

        if ($null -eq $parameters) {
            $this.Logger.LogError("Unable to locate a suitable constructor for Process '$($type.FullName)'.");

            return $null;
        }
        
        [object[]] $constructorParams = @();

        try {
            foreach($param in $parameters) {
                [object] $service = $this.ServiceContainer.GetService($param.ParameterType);
    
                $constructorParams += $service;
            }
        }
        catch {
            $this.Logger.LogError("Error whilst attempting to satisfy dependencies for Process '$($type.FullName)'.");
            $this.Logger.LogError($_.Exception.Message);

            return $null;
        }
        
        return New-Object -TypeName $type.Name -ArgumentList $constructorParams;
    }
}
