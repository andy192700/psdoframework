Describe 'SetProcessLocationTests' {
    BeforeEach {
        [string] $sep = [DoFramework.Environment.Environment]::Separator.ToString();

        Invoke-Expression -Command "$(Get-Content "$PSScriptRoot$($sep)..$($sep)..$($sep)..$($sep)..$($sep)Objects$($sep)FileSystem$($sep)SetProcessLocation.psm1")";
    }
    
    Context 'Tests' {
        it 'Sets Location' {
            # Arrange            
            Mock Set-Location -Verifiable;

            [SetProcessLocation] $sut = [SetProcessLocation]::new();

            [string] $testString = "1234";

            # Act
            $sut.Set($testString);

            # Assert
            Should -Invoke Set-Location -Times 1 -Exactly -ParameterFilter { $Path -eq $testString }
        }
    }
}