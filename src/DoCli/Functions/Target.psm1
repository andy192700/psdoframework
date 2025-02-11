using namespace System.Management.Automation;
using module "..\Objects\Processing\DoFileTarget.psm1";

function Target {
    param (
        [Parameter(Mandatory=$true)] [ValidateNotNullOrEmpty()] [string] $name,
        [ScriptBlock] $scriptBlock,
        [string] $inherits
    )

    $Global:targets[$name] = [DoFileTarget]::new($scriptBlock, $inherits);
}

Export-ModuleMember -Function Target;