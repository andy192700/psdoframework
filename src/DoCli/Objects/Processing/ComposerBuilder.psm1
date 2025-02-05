using namespace DoFramework.Domain;
using namespace DoFramework.Environment;
using namespace DoFramework.Processing;
using namespace DoFramework.Services;
using namespace DoFramework.Types;

class ComposerBuilder : IComposerBuilder {
    <#
    .SYNOPSIS
    Initializes a new instance of the ProcessBuilder class.

    .DESCRIPTION
    Constructor for the ProcessBuilder class, which sets up the service container, 
    environment, lookup type, type validator, and logger for the process building.
    #>
    [IEnvironment] $Environment;
    [ILookupType[IComposer]] $LookupType;

    ComposerBuilder(
        [IEnvironment] $environment,
        [ILookupType[IComposer]] $lookupType) {
        $this.Environment = $environment;
        $this.LookupType = $lookupType;
    }

    <#
    .SYNOPSIS
    Builds a process instance based on the provided descriptor.

    .DESCRIPTION
    The Build method retrieves the process type, gathers the necessary constructor 
    parameters, and creates a new instance of the process within the DoFramework environment.
    #>
    [IComposer] Build([ComposerDescriptor] $descriptor) {
        . "$($this.Environment.ComposersDir)$([DoFramework.Environment.Environment]::Separator)$($descriptor.Path)";

        [Type] $type = $this.LookupType.Lookup($descriptor.Name);
        
        return New-Object -TypeName $type.Name;
    }
}
