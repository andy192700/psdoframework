using module "..\..\Modules\ModuleWithFunctions.psm1";

Describe 'ModuleWithFunctionsTests' {
    Context 'ModuleWithFunctionsTests' {
        It 'Double a number, doubles a number' {
            # Arrange / Act
            [int] $number = 3333;

            [int] $result = DoubleANumber -number $number;

            # Assert
            $result | Should -Be 6666;
        }

        it 'Function that throws, does indeed throw' {
            # Arrange

            # Act / Assert
            { ThrowsAnException } | Should -Throw -ExpectedMessage "Exception thrown!!!";
        }
    }
}
