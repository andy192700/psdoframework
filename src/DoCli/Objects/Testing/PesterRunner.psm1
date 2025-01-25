using namespace DoFramework.Testing;
using namespace DoFramework.Types;

<#
.SYNOPSIS
Class for running Pester tests within the DoFramework environment.

.DESCRIPTION
The PesterRunner class is designed to execute Pester tests using specified 
configurations and paths within the DoFramework environment. It provides methods 
for running tests with and without specified paths.
#>
class PesterRunner : IPesterRunner {

    PesterRunner() {
        <# TODO - this is debt and needs moving to an appropriate and general "global import" area. #>
        Import-Module -Name Pester -RequiredVersion 5.7.1;
    }

    <#
    .SYNOPSIS
    Runs Pester tests with the specified configuration and paths.

    .DESCRIPTION
    The Run method iterates through the provided paths, loads each script, 
    and then invokes Pester with the given configuration.
    #>
    [void] Run([object] $config, [string[]] $paths) {
        foreach ($path in $paths) {
            . $path;
        }

        Invoke-Pester -Configuration $config;
    }

    <#
    .SYNOPSIS
    Runs Pester tests with the specified configuration.

    .DESCRIPTION
    The Run method invokes Pester with the given configuration without 
    loading any additional scripts.
    #>
    [void] Run([object] $config) {
        Invoke-Pester -Configuration $config;
    }
}
