using module "..\..\..\Modules\Models\Person.psm1";

Describe 'PersonTests' {
    Context 'Tests' {
        It 'ToString method functions correctly' {
            # Arrange
            [string] $firstName = "First_Name";
            [string] $lastName = "Last_Name";
            [int] $age = 333;
            
            [Person] $sut = [Person]::new();
            $sut.FirstName = $firstName;
            $sut.LastName = $lastName;
            $sut.Age = $age;

            # Act
            [string] $result = $sut.ToString();

            # Assert
            $result | Should -Be "FullName: $firstName $lastName Age: $age";
        }
    }
}
