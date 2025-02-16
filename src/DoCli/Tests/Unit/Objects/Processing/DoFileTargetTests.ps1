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

        [string] $Script:targetNotExist = "target2";

        [DoFileTarget] $script:dft1 = [DoFileTarget]::new($Script:sb1, $null);

        [DoFileTarget] $script:dft2 = [DoFileTarget]::new($Script:sb2, $Script:target);

        [DoFileTarget] $script:dft3 = [DoFileTarget]::new($Script:sb2, $Script:targetNotExist);

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
        
        it 'ToScriptBlock Throws' {
            # Arrange / Act
            $func = {
                $script:dft3.ToScriptBlock($Global:targets);
            };

            # Assert
            $func | Should -Throw -ExceptionType ([System.ArgumentException]) -ExpectedMessage "The inherited target '$($Script:targetNotExist)' does not exist.";
        }
    }
}