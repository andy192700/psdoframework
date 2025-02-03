using namespace DoFramework.Domain;
using namespace DoFramework.Processing;
using namespace DoFramework.Testing;
using module "..\..\..\Modules\ConfigurableObject.psm1";

Describe 'ConfigurationSampleTests' {
    BeforeEach {
        [ProxyResult] $script:context = doing mock -type ([IContext]);

        [ProxyResult] $script:verifier = doing mock -type ([IContextVerifier]);

        $script:context.Proxy.MockMethod("Requires", {
            return $script:verifier.Instance;
        });

        $script:verifier.Proxy.MockMethod("ComposedBy", {
            param (
                [string] $composerName
            )

            return $script:verifier.Instance;
        });

        [MyConfig] $config1 = [MyConfig]::new();
        $config1.MyInt = 3;
        $config1.MyString = "abc";

        [MySecondConfig] $config2 = [MySecondConfig]::new();
        $config2.MyDouble = 2.1;
        $config2.MyBool = $true;

        [MyThirdConfig] $config3 = [MyThirdConfig]::new();
        $config3.MyShort = 7;
        $config3.MyFloat = 1.123;

        [MyConfigurationService] $script:config = [MyConfigurationService]::new($config1, $config2, $config3);

    }

    Context 'ConfigurationSampleTests' {
        It 'Is Invalid' {
            # Arrange
            $script:verifier.Proxy.MockMethod("Verify", {
                return $false;
            });

            [ConfigurationSample] $sut = [ConfigurationSample]::new($script:context.instance, $script:config);

            # Act
            [bool] $result = $sut.Validate();

            # Assert
            $result | Should -Be $false;
            $script:context.Proxy.CountCalls("Requires") | Should -Be 1;
            $script:verifier.Proxy.CountCalls("ComposedBy", (doing args -composerName "ConfigurationComposer")) | Should -Be 1;
            $script:verifier.Proxy.CountCalls("Verify") | Should -Be 1;
        }

        It 'Is Valid' {
            # Arrange
            $script:verifier.Proxy.MockMethod("Verify", {
                return $true;
            });

            [ConfigurationSample] $sut = [ConfigurationSample]::new($script:context.instance, $script:config);

            # Act
            [bool] $result = $sut.Validate();

            # Assert
            $result | Should -Be $true;
            $script:context.Proxy.CountCalls("Requires") | Should -Be 1;
            $script:verifier.Proxy.CountCalls("ComposedBy", (doing args -composerName "ConfigurationComposer")) | Should -Be 1;
            $script:verifier.Proxy.CountCalls("Verify") | Should -Be 1;
        }

        It 'Runs as expected' {
            # Arrange
            Mock Write-Host {}

            [ConfigurationSample] $sut = [ConfigurationSample]::new($script:context.instance, $script:config);

            # Act
            $sut.Run();

            # Assert
            Should -Invoke -CommandName Write-Host -Times 1 -ParameterFilter { $Object -eq "1: $($script:config.Config1.MyInt) $($script:config.Config1.MyString)"};
            Should -Invoke -CommandName Write-Host -Times 1 -ParameterFilter { $Object -eq "2: $($script:config.Config2.MyDouble) $($script:config.Config2.MyBool)"};
            Should -Invoke -CommandName Write-Host -Times 1 -ParameterFilter { $Object -eq "3: $($script:config.Config3.MyShort) $($script:config.Config3.MyFloat)"};
        }

        It 'Processes as expected' {
            # Arrange 
            [string] $processName = "ConfigurationSample";

            # Act
            [IContext] $result = doing run -name $processName -doOutput -silent;

            # Assert
            $result | Should -Not -Be $null;
            $result.Session.ProcessCount | Should -Be 1;
            $result.Session.ProcessReports.Count | Should -Be 1;
            $result.Session.ProcessReports[0].Descriptor.Name | Should -Be $processName;
            $result.Session.ProcessReports[0].ProcessResult | Should -Be ([ProcessResult]::Invalidated);
        }
    }
}
