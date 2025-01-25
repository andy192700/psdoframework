using module "..\Objects\CLI\CLIFunctionSelector.psm1";

<#
.SYNOPSIS
    Function to call the DoFramework's PowerShell command line interface.

.DESCRIPTION
    This function is the gateway to the all functions offered by the framework.

.PARAMETERS        
    As a minimum all calls require a singular argument specifying the function's name.

    If parameters are required they should be supplied in the form "-Name Value".

.EXAMPLES
    doing FUNCTIONNAME
    doing FUNCTIONNAME -arg1 1 -arg2 2
#>
function Doing {
    $Global:ErrorActionPreference = "Stop";

    [CLIFunctionSelector] $selector = [CLIFunctionSelector]::new();

    $selector.Execute($args);
}

Export-ModuleMember -Function Doing;
