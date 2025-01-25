using namespace System.IO;

Describe 'ReadProcessLocationTests' {
    BeforeEach {
        [string] $sep = [DoFramework.Environment.Environment]::Separator.ToString();

        # Ensure native PowerShell methods are mock-able by using Invoke-Expression rather than a using module statement.
        Invoke-Expression -Command "$(Get-Content "$PSScriptRoot$($sep)..$($sep)..$($sep)..$($sep)..$($sep)Objects$($sep)Environment$($sep)ReadProcessLocation.psm1")";
    }
    
    Context 'Tests' {
        it 'Gets Location' {
            # Arrange            
            [string] $testString = "1234";

            Mock Get-Location {
                return $testString;
            };

            [ReadProcessLocation] $sut = [ReadProcessLocation]::new();

            # Act
            [string] $result = $sut.Read();

            # Assert
            Should -Invoke Get-Location -Times 1 -Exactly;

            $result | Should -Be $testString;
        }
    }
}