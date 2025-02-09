using namespace DoFramework.Processing;
using namespace DoFramework.Testing;
using module "..\..\Modules\CreatePersons.psm1";
using module "..\..\Modules\CreatePersonsFile.psm1";
using module "..\..\Modules\DeletePersonsFile.psm1";
using module "..\..\Modules\ReadPersonsFile.psm1";

Describe 'AdvancedComposerTests' {
    BeforeEach {
        [ProxyResult] $script:serviceRepeater = doing mock -type ([IRepeater[Type]]);

        $script:serviceRepeater.Proxy.MockMethod("And", {
            param (
                [Type] $input
            )

            return $script:serviceRepeater.Instance;
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

        $script:workBench.Proxy.MockMethod("RegisterProcess", {
            param (
                [string] $processName
            )

            return $script:processRepeater.Instance;
        });
    }

    Context 'AdvancedComposerTests' {
        It 'Builds Successfully' {
            # Arrange
            [AdvancedComposer] $sut = [AdvancedComposer]::new();
            
            # Act
            $sut.Compose($script:workBench.instance);
            
            # Assert
            $script:workBench.Proxy.CountCalls("RegisterService", (doing args -serviceType ([CreatePersons]))) | Should -Be 1;            
            $script:serviceRepeater.Proxy.CountCalls("And", (doing args -input ([CreatePersonsFile]))) | Should -Be 1;
            $script:serviceRepeater.Proxy.CountCalls("And", (doing args -input ([DeletePersonsFile]))) | Should -Be 1;
            $script:serviceRepeater.Proxy.CountCalls("And", (doing args -input ([ReadPersonsFile]))) | Should -Be 1;

            $script:workBench.Proxy.CountCalls("RegisterProcess", (doing args -processName ("AdvancedProcess"))) | Should -Be 1;
            $script:processRepeater.Proxy.CountCalls("And", (doing args -input ("DeleteFile"))) | Should -Be 2;
            $script:processRepeater.Proxy.CountCalls("And", (doing args -input ("CreateData"))) | Should -Be 1;
            $script:processRepeater.Proxy.CountCalls("And", (doing args -input ("CreateFile"))) | Should -Be 1;
            $script:processRepeater.Proxy.CountCalls("And", (doing args -input ("ReadFile"))) | Should -Be 1;
        }

        It 'Composes Successfully' {
            # Arrange 
            [string] $composerName = "AdvancedComposer";
            
            # Act
            [IContext] $result = doing compose -name $composerName -silent -doOutput;

            # Assert
            $result.Requires().
                ComposedBy($composerName).
                ProcessSucceeded("AdvancedProcess").
                ProcessSucceeded("DeleteFile").
                ProcessSucceeded("CreateData").
                ProcessSucceeded("CreateFile").
                ProcessSucceeded("ReadFile").
                Verify() | Should -Be $true;
        }
    }
}
