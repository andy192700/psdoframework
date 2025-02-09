using namespace DoFramework.Processing;
using namespace DoFramework.Testing;
using module "..\..\Modules\ConfigurableObject.psm1";

Describe 'ConfigurationComposerTests' {
    BeforeEach {
        [ProxyResult] $script:serviceRepeater = doing mock -type ([IRepeater[Type]]);

        $script:serviceRepeater.Proxy.MockMethod("And", {
            param (
                [Type] $input
            )

            return $script:serviceRepeater.Instance;
        });

        [ProxyResult] $script:configRepeater = doing mock -type ([IRepeater[Type]]);

        $script:configRepeater.Proxy.MockMethod("And", {
            param (
                [Type] $input
            )

            return $script:configRepeater.Instance;
        });
        
        [ProxyResult] $script:processRepeater = doing mock -type ([IRepeater[string]]);
        
        $script:processRepeater.Proxy.MockMethod("And", {
            param (
                [string] $input
            )

            return $script:processRepeater.Instance;
        });

        [ProxyResult] $script:workBench = doing mock -type ([IComposerWorkBench]);

        $script:workBench.Proxy.MockMethod("RegisterService", {
            param (
                [Type] $serviceType
            )

            return $script:serviceRepeater.Instance;
        });

        $script:workBench.Proxy.MockMethod("Configure", {
            param (
                [Type] $configType
            )

            return $script:configRepeater.Instance;
        });

        $script:workBench.Proxy.MockMethod("RegisterProcess", {
            param (
                [string] $processName
            )

            return $script:processRepeater.Instance;
        });
    }

    Context 'ConfigurationComposerTests' {
        It 'Builds Successfully' {
            # Arrange
            [ConfigurationComposer] $sut = [ConfigurationComposer]::new()

            # Act
            $sut.Compose($script:workBench.instance);

            # Assert
            $script:workBench.Proxy.CountCalls("RegisterService", (doing args -serviceType ([MyConfigurationService]))) | Should -Be 1;
            
            $script:workBench.Proxy.CountCalls("Configure", (doing args -configType ([MyConfig]))) | Should -Be 1;
            $script:configRepeater.Proxy.CountCalls("And", (doing args -input ([MySecondConfig]))) | Should -Be 1;
            $script:configRepeater.Proxy.CountCalls("And", (doing args -input ([MyThirdConfig]))) | Should -Be 1;

            $script:workBench.Proxy.CountCalls("RegisterProcess", (doing args -processName ("ConfigurationSample"))) | Should -Be 1;
        }

        It 'Composes Successfully' {
            # Arrange 
            [string] $composerName = "ConfigurationComposer";
            
            # Act
            [IContext] $result = doing compose -name $composerName -silent -doOutput;

            # Assert
            $result.Requires().
                ComposedBy($composerName).
                ProcessSucceeded("ConfigurationSample").
                Verify() | Should -Be $true;
        }
    }
}
