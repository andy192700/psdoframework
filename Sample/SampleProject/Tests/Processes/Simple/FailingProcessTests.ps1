using namespace DoFramework.Domain;
using namespace DoFramework.Processing;

Describe 'FailingProcessTests' {
    BeforeEach {
    }
    Context 'FailingProcessTests' {
        It 'Is Valid' {
            # Arrange
            [FailingProcess] $simple = [FailingProcess]::new();

            # Act
            [bool] $result = $simple.Validate();

            # Assert
            $result | Should -Be $true;
        }

        It 'Fails' {
            # Arrange
            [FailingProcess] $simple = [FailingProcess]::new();

            # Act / Assert
            { $simple.Run() } | Should -Throw -ExpectedMessage "Exception thrown!!!";
        }

        It 'Fails with mocked function' {
            # Arrange
            [string] $someErrorMessage = "the error message";

            Mock ThrowsAnException {
                throw $someErrorMessage;
            };

            [FailingProcess] $simple = [FailingProcess]::new();

            # Act / Assert
            { $simple.Run() } | Should -Throw -ExpectedMessage $someErrorMessage;
        }

        It 'Processes as expected' {
            # Arrange 
            [string] $processName = "FailingProcess";

            # Act
            [IContext] $result = doing run-process -name $processName -doOutput -silent;

            # Assert
            $result | Should -Not -Be $null;
            $result.Session.ProcessCount | Should -Be 1;
            $result.Session.ProcessReports.Count | Should -Be 1;
            $result.Session.ProcessReports[0].Descriptor.Name | Should -Be $processName;
            $result.Session.ProcessReports[0].ProcessResult | Should -Be ([ProcessResult]::Failed);
        }
    }
}
