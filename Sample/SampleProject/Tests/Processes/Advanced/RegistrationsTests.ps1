using namespace DoFramework.Domain;
using namespace DoFramework.Processing;
using namespace DoFramework.Services;
using namespace DoFramework.Testing;
using module "..\..\..\Modules\CreatePersons.psm1";
using module "..\..\..\Modules\CreatePersonsFile.psm1";
using module "..\..\..\Modules\DeletePersonsFile.psm1";
using module "..\..\..\Modules\ReadPersonsFile.psm1";

Describe 'RegistrationsTests' {
    BeforeEach {
        [ProxyResult] $script:mockSession = doing create-proxy -type ([ISession]);

        [ProxyResult] $script:mockContext = doing create-proxy -type ([IContext]);

        $script:mockContext.Proxy.MockProperty("Session", {
            return $script:mockSession.Instance;
        });

        [ProxyResult] $script:mockContainer = doing create-proxy -type ([IServiceContainer]);
    }
    Context 'RegistrationsTests' {
        It 'Is Invalid, no previous processes ran' {
            # Arrange
            [Registrations] $sut = [Registrations]::new(
                $script:mockContainer.Instance,
                $script:mockContext.Instance);

            $script:mockSession.Proxy.MockProperty("ProcessCount", {
                return 0;
            });

            # Act
            [bool] $result = $sut.Validate();

            # Assert
            $result | Should -Be $false;

            $script:mockSession.Proxy.CountPropertyCalls("ProcessCount") | Should -Be 1;
        }

        It 'Is Valid' {
            # Arrange
            [Registrations] $sut = [Registrations]::new(
                $script:mockContainer.Instance,
                $script:mockContext.Instance);

            $script:mockSession.Proxy.MockProperty("ProcessCount", {
                return 2;
            });

            # Act
            [bool] $result = $sut.Validate();

            # Assert
            $result | Should -Be $true;

            $script:mockSession.Proxy.CountPropertyCalls("ProcessCount") | Should -Be 1;
        }

        It 'Processes as expected' {
            # Arrange
            [Registrations] $sut = [Registrations]::new(
                $script:mockContainer.Instance,
                $script:mockContext.Instance);

            [string] $currentDir = "someValue";

            Mock Get-Location {};

            # Act
            $sut.Run();

            # Assert
            $script:mockContainer.Proxy.CountCalls("RegisterService") | Should -Be 4;
            $script:mockContainer.Proxy.CountCalls("RegisterService", (doing read-args -type ([CreatePersons]))) | Should -Be 1;
            $script:mockContainer.Proxy.CountCalls("RegisterService", (doing read-args -type ([CreatePersonsFile]))) | Should -Be 1;
            $script:mockContainer.Proxy.CountCalls("RegisterService", (doing read-args -type ([DeletePersonsFile]))) | Should -Be 1;
            $script:mockContainer.Proxy.CountCalls("RegisterService", (doing read-args -type ([ReadPersonsFile]))) | Should -Be 1;

            $script:mockSession.Proxy.CountPropertyCalls("ProcessCount") | Should -Be 0;
            
            Should -Invoke -CommandName Get-Location -Times 1;

            $script:mockContext.Proxy.CountCalls("AddOrUpdate") | Should -Be 1;
            $script:mockContext.Proxy.CountCalls("AddOrUpdate", (doing read-args -key "PersonsFilePath" -value "$currentDir$([System.IO.Path]::DirectorySeparatorChar)persons.json"))
        }

        It 'Processes as expected' {
            # Arrange 
            [string] $processName = "Registrations";

            # Act
            [IContext] $result = doing run-process -name $processName -doOutput -silent;

            # Assert
            $result | Should -Not -Be $null;
            $result.Session.ProcessCount | Should -Be 1;
            $result.Session.ProcessReports.Count | Should -Be 1;
            $result.Session.ProcessReports[0].Descriptor.Name | Should -Be $processName;
            $result.Session.ProcessReports[0].ProcessResult | Should -Be ([ProcessResult]::Invalidated);
        }
    }
}
