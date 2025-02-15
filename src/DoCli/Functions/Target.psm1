using namespace System.Collections.Generic;
using namespace System.Management.Automation;
using module "..\Objects\Processing\DoFileTarget.psm1";

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