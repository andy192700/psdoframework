using namespace System.Collections.Generic;
using namespace System.Management.Automation;
using module "..\..\..\..\Objects\Processing\DoFileTarget.psm1";

Describe 'DoFileTargetValidatorTests' {
    BeforeEach {
        $Global:targets = [Dictionary[string, DoFileTarget]]::new();

        [ScriptBlock] $Script:sb1 = {
            return "Abc";
        };

        [ScriptBlock] $Script:sb2 = {
            return "123";
        };

        [string] $Script:target = "target1";

        [DoFileTarget] $script:dft1 = [DoFileTarget]::new($Script:sb1, $null);

        [DoFileTarget] $script:dft2 = [DoFileTarget]::new($Script:sb2, $Script:target);

        $Global:targets[$Script:target] = $script:dft1;
    }

    AfterEach {
        $Global:targets = $null;
    }

    Context 'Tests' {
        it 'ToScriptBlock Not Inherited' {
            # Arrange / Act
            [ScriptBlock] $result = $script:dft1.ToScriptBlock($Global:targets);

            # Assert
            $result | Should -Be $Script:sb1;
        }
        
        it 'ToScriptBlock Inherited' {
            # Arrange / Act
            [ScriptBlock] $result = $script:dft2.ToScriptBlock($Global:targets);

            # Assert
            $result.ToString() | Should -Be ([ScriptBlock]::Create($Script:sb1.ToString() + $Script:sb2.ToString()).ToString());
        }
    }
}