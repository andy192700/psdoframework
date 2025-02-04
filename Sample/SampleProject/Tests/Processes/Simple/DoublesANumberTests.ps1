using namespace DoFramework.CLI;
using namespace DoFramework.Domain;
using namespace DoFramework.Processing;
using namespace DoFramework.Testing;
using namespace System.Collections.Generic;
using module "..\..\..\Modules\ModuleWithFunctions.psm1";

Describe 'DoublesANumberTests' {
    BeforeEach {
        [ProxyResult] $script:mockContext = doing mock -type ([IContext]);

        $script:mockContext.Proxy.MockMethod("Get", {
            param (
                [string] $key
            )

            if ($key -eq "InputInteger") {
                return 13;
            }

            return [string]::Empty;
        });
    }
    Context 'DoublesANumberTests' {
        It 'Is Valid' {
            # Arrange
            [DoublesANumber] $simple = [DoublesANumber]::new($script:mockContext.Instance);

            # Act
            [bool] $result = $simple.Validate();

            # Assert
            $result | Should -Be $true;
        }

        It 'Runs as expected' {
            # Arrange
            [int] $mockReturnVal = 222;

            Mock Write-Host {};

            Mock DoubleANumber {
                return $mockReturnVal;
            };

            [DoublesANumber] $simple = [DoublesANumber]::new($script:mockContext.Instance);

            # Act
            $simple.Run();

            # Assert
            $script:mockContext.Proxy.CountCalls("Get") | Should -Be 1;
            $script:mockContext.Proxy.CountCalls("Get", (doing args -key "InputInteger")) | Should -Be 1;

            [int] $inputInteger = $script:mockContext.Instance.Get("InputInteger");

            Should -Invoke -CommandName Write-Host -Times 1 -ParameterFilter { $Object -eq "Input: $inputInteger Output: $mockReturnVal" };
            
            Should -Invoke -CommandName Write-Host -Times 1 -ParameterFilter { $Object -eq "The switch 'MySwitch' was supplied: false" };
        }

        It 'Processes as expected' {
            # Arrange 
            [string] $processName = "DoublesANumber";

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
