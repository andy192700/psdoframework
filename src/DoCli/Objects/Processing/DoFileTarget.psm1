using namespace System.Collections.Generic;
using namespace System.Management.Automation;

# Class representing a file target with an associated ScriptBlock and optional inheritance from another target.
# Contains functionality to combine inherited targets with its own block of code.
class DoFileTarget {
    [ScriptBlock] $Block;
    [string] $Inherits;

    DoFileTarget([ScriptBlock] $block, [string] $inherits) {
        $this.Block = $block;
        $this.Inherits = $inherits;
    }

    # Method to combine the inherited target's ScriptBlock (if any) with the current target's ScriptBlock.
    # Returns the resulting ScriptBlock.
    [ScriptBlock] ToScriptBlock([IDictionary[string, object]] $targets) {
        if (![string]::IsNullOrEmpty($this.Inherits)) {
            return [ScriptBlock]::Create($targets[$this.Inherits].ToScriptBlock($targets).ToString() + $this.Block.ToString());
        }

        return $this.Block;
    }
}