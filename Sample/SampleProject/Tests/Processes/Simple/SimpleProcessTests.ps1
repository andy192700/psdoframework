using namespace DoFramework.Domain;
using namespace DoFramework.Processing;

Describe 'SimpleProcessTests' {
    Context 'SimpleProcessTests' {
        It 'Is Valid' {
            # Arrange
            [SimpleProcess] $simple = [SimpleProcess]::new();

            # Act
            [bool] $result = $simple.Validate();

            # Assert
            $result | Should -Be $true;
        }

        It 'Runs as expected' {
            # Arrange
            Mock Write-Host {};

            [SimpleProcess] $simple = [SimpleProcess]::new();

            # Act
            $simple.Run();

            # Assert
            Should -Invoke -CommandName Write-Host -Times 1 -ParameterFilter { $Object -eq "hello world!" };
        }

        It 'Processes as expected' {
            # Arrange 
            [string] $processName = "SimpleProcess";

            # Act
            [IContext] $result = doing run -name $processName -doOutput -silent;

            # Assert
            $result | Should -Not -Be $null;
            $result.Session.ProcessCount | Should -Be 1;
            $result.Session.ProcessReports.Count | Should -Be 1;
            $result.Session.ProcessReports[0].Descriptor.Name | Should -Be $processName;
            $result.Session.ProcessReports[0].ProcessResult | Should -Be ([ProcessResult]::Completed);
        }
    }
}
