using namespace System.Collections.Generic;
using namespace System.Management.Automation;

class DoFileTarget {
    [ScriptBlock] $Block;
    [string] $Inherits;

    DoFileTarget([ScriptBlock] $block, [string] $inherits) {
        $this.Block = $block;
        $this.Inherits = $inherits;
    }

    [ScriptBlock] ToScriptBlock([IDictionary[string, DoFileTarget]] $targets) {
        if (![string]::IsNullOrEmpty($this.Inherits)) {
            return [ScriptBlock]::Create($targets[$this.Inherits].ToScriptBlock($targets).ToString() + $this.Block.ToString());
        }

        return $this.Block;
    }
}