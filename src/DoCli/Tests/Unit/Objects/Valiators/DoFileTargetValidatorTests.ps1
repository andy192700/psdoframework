using namespace DoFramework.Validators;
using namespace System.Collections.Generic;
using module "..\..\..\..\Objects\Processing\DoFileTarget.psm1";
using module "..\..\..\..\Objects\Validators\DoFileTargetValidator.psm1";

Describe 'DoFileTargetValidatorTests' {
    BeforeEach {
        $Global:targets = [Dictionary[string, DoFileTarget]]::new();

        [string] $Script:target = "myTarget";
    }

    AfterEach {
        $Global:targets = $null;
    }

    Context 'Tests' {
        it 'Is valid' {
            # Arrange
            $Global:targets[$Script:target] = [DoFileTarget]::new($null, $null);

            [DoFileTargetValidator] $sut = [DoFileTargetValidator]::new();

            # Act
            [IValidationResult] $result = $sut.Validate($Script:target);

            # Assert
            $result.IsValid | Should -Be $true;
        }
        
        it 'Is not valid' {
            # Arrange
            [DoFileTargetValidator] $sut = [DoFileTargetValidator]::new();

            # Act
            [IValidationResult] $result = $sut.Validate($Script:target);

            # Assert
            $result.IsValid | Should -Be $false;
            $result.Errors.Count | Should -Be 1;
            $result.Errors[0] | Should -Be "Could not locate target '$Script:target'";
        }
    }
}