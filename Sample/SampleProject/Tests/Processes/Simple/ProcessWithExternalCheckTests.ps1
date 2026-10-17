using namespace DoFramework.Domain;
using namespace DoFramework.Processing;
using namespace DoFramework.Testing;

Describe 'ProcessWithExternalCheckTests' {
    BeforeEach {
        [ProxyResult] $script:mockContext = doing mock -type ([IContext]);

        $script:mockContext.Proxy.MockMethod("Requires", {
            return [ContextVerifier]::new($script:mockContext.Instance);
        });

        $script:mockContext.Proxy.MockMethod("Get", {
            param (
                [string] $key
            )

            if ($key -eq "sample") {
                return 123;
            }

            return [string]::Empty;
        });
    }
    Context 'ProcessWithExternalCheckTests' {
        It 'Is Invalid' {
            # Arrange
            $script:mockContext.Proxy.Reset();

            $script:mockContext.Proxy.MockMethod("Requires", {
                return [ContextVerifier]::new($script:mockContext.Instance);
            });

            [ProcessWithExternalCheck] $sut = [ProcessWithExternalCheck]::new($script:mockContext.Instance);

            # Act
            [bool] $result = $sut.Validate();

            # Assert
            $result | Should -Be $false;
        }

        It 'Is Valid' {
            # Arrange
            [ProcessWithExternalCheck] $sut = [ProcessWithExternalCheck]::new($script:mockContext.Instance);

            # Act
            [bool] $result = $sut.Validate();

            # Assert
            $result | Should -Be $true;
        }

        It 'Processes as expected' {
            # Arrange 
            [string] $processName = "ProcessWithExternalCheck";

            # Act
            [IContext] $result = doing run -name $processName -doOutput -silent -sample "123";

            # Assert
            $result | Should -Not -Be $null;
            $result.Session.ProcessCount | Should -Be 1;
            $result.Session.ProcessReports.Count | Should -Be 1;
            $result.Session.ProcessReports[0].Descriptor.Name | Should -Be $processName;
            $result.Session.ProcessReports[0].ProcessResult | Should -Be ([ProcessResult]::Completed);
        }
    }
}
