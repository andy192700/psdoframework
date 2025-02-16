using namespace System.Collections.Generic;
using namespace System.Management.Automation;
using module "..\Objects\Processing\DoFileTarget.psm1";

<#
.SYNOPSIS
    Function called by the DoFramework's dofile.ps1 offering, which specified Targets for CLI execution.

.DESCRIPTION
    Developers specify their own Targets with their own associated code blocks for easy CLI usage using this function.

.PARAMETERS        
    name - the name of the target (case sensitive!)
    scriptBlock - the code block to be executed upon calling of the Target
    inherits - Specify an inherited Target. if specified, the framework will execute the inherited Target first.
               This allows developers to chain Targets together to allow for some scaling.

.EXAMPLES
    Target A {
        # my code here
    }

    Target B -inherits A {
        # my code here
    }
#>
function Target {
    param (
        [Parameter(Mandatory=$true)] [ValidateNotNullOrEmpty()] [string] $name,
        [ScriptBlock] $scriptBlock,
        [string] $inherits
    )

    if (!$Global:targets.TryAdd($name, [DoFileTarget]::new($scriptBlock, $inherits))) {
        throw "Multiple targets with the name '$name' specified.";
    }
}

Export-ModuleMember -Function Target;