using namespace DoFramework.Domain;
using namespace DoFramework.Logging;
using namespace DoFramework.Processing;
using namespace DoFramework.Testing;
using namespace System.Collections.Generic;
using module "..\..\..\Modules\DeletePersonsFile.psm1";

Describe 'DeleteFileTests' {
    BeforeEach {
        [ProxyResult] $script:mockSession = doing create-proxy -type ([ISession]);

        [ProxyResult] $script:mockContext = doing create-proxy -type ([IContext]);

        $script:mockContext.Proxy.MockProperty("Session", {
            return $script:mockSession.Instance;
        });
        
        [ProxyResult] $script:mockLogger = doing create-proxy -type ([ILogger]);
        
        [ProxyResult] $script:mockDeletePersonsFile = doing create-proxy -type ([DeletePersonsFile]) -params @($script:mockContext.Instance);
    }
    Context 'DeleteFileTests' {
        It 'Is Invalid, no prior reports' {
            # Arrange
            [DeleteFile] $sut = [DeleteFile]::new(
                $script:mockCreatePersons.Instance,
                $script:mockLogger.Instance,
                $script:mockContext.Instance);

            $script:mockSession.Proxy.MockProperty("ProcessReports", {
                [List[ProcessReport]] $reports = [List[ProcessReport]]::new();

                return $reports;
            });

            # Act
            [bool] $result = $sut.Validate();

            # Assert
            $result | Should -Be $false;

            $script:mockContext.Proxy.CountPropertyCalls("Session") | Should -Be 1;
            $script:mockContext.Proxy.CountCalls("KeyExists") | Should -Be 0;
            $script:mockSession.Proxy.CountPropertyCalls("ProcessReports") | Should -Be 1;
        }

        It 'Is Invalid, first report name does not match' {
            # Arrange
            [DeleteFile] $sut = [DeleteFile]::new(
                $script:mockCreatePersons.Instance,
                $script:mockLogger.Instance,
                $script:mockContext.Instance);

            $script:mockSession.Proxy.MockProperty("ProcessReports", {
                [List[ProcessReport]] $reports = [List[ProcessReport]]::new();
            
                [ProcessReport] $p1 = [ProcessReport]::new();
                [ProcessDescriptor] $ps1 = [ProcessDescriptor]::new();
                $ps1.Name = "wrong";
                $p1.Descriptor = $ps1;

                $reports.Add($p1);

                return $reports;
            });

            # Act
            [bool] $result = $sut.Validate();

            # Assert
            $result | Should -Be $false;

            $script:mockContext.Proxy.CountPropertyCalls("Session") | Should -Be 2;
            $script:mockContext.Proxy.CountCalls("KeyExists") | Should -Be 0;
            $script:mockSession.Proxy.CountPropertyCalls("ProcessReports") | Should -Be 2;
        }

        It 'Is Invalid, missing context value' {
            # Arrange
            [DeleteFile] $sut = [DeleteFile]::new(
                $script:mockCreatePersons.Instance,
                $script:mockLogger.Instance,
                $script:mockContext.Instance);
                
            $script:mockSession.Proxy.MockProperty("ProcessReports", {
                [List[ProcessReport]] $reports = [List[ProcessReport]]::new();
            
                [ProcessReport] $p1 = [ProcessReport]::new();
                [ProcessDescriptor] $ps1 = [ProcessDescriptor]::new();
                $ps1.Name = "Registrations";
                $p1.Descriptor = $ps1;
                
                $reports.Add($p1);

                return $reports;
            });
        
            $script:mockContext.Proxy.MockMethod("KeyExists", {
                param (
                    [string] $key
                )

                return $false;
            });

            # Act
            [bool] $result = $sut.Validate();

            # Assert
            $result | Should -Be $false;

            $script:mockContext.Proxy.CountPropertyCalls("Session") | Should -Be 2;
            $script:mockContext.Proxy.CountCalls("KeyExists") | Should -Be 1;
            $script:mockContext.Proxy.CountCalls("KeyExists", (doing read-args -key "PersonsFilePath")) | Should -Be 1;
            $script:mockSession.Proxy.CountPropertyCalls("ProcessReports") | Should -Be 2;
        }

        It 'Is Valid' {
            # Arrange
            [DeleteFile] $sut = [DeleteFile]::new(
                $script:mockCreatePersons.Instance,
                $script:mockLogger.Instance,
                $script:mockContext.Instance);
                
            $script:mockSession.Proxy.MockProperty("ProcessReports", {
                [List[ProcessReport]] $reports = [List[ProcessReport]]::new();
            
                [ProcessReport] $p1 = [ProcessReport]::new();
                [ProcessDescriptor] $ps1 = [ProcessDescriptor]::new();
                $ps1.Name = "Registrations";
                $p1.Descriptor = $ps1;

                $reports.Add($p1);

                return $reports;
            });
        
            $script:mockContext.Proxy.MockMethod("KeyExists", {
                param (
                    [string] $key
                )

                return $true;
            });

            # Act
            [bool] $result = $sut.Validate();

            # Assert
            $result | Should -Be $true;

            $script:mockContext.Proxy.CountPropertyCalls("Session") | Should -Be 2;
            $script:mockContext.Proxy.CountCalls("KeyExists") | Should -Be 1;
            $script:mockContext.Proxy.CountCalls("KeyExists", (doing read-args -key "PersonsFilePath")) | Should -Be 1;
            $script:mockSession.Proxy.CountPropertyCalls("ProcessReports") | Should -Be 2;
        }
        
        It 'Runs as Expected' {
            # Arrange
            [DeleteFile] $sut = [DeleteFile]::new(
                [DeletePersonsFile]($script:mockDeletePersonsFile.Instance),
                $script:mockLogger.Instance,
                $script:mockContext.Instance);

            # Act
            $sut.Run();

            # Assert
            $script:mockLogger.Proxy.CountCalls("LogInfo") | Should -Be 2;
            $script:mockLogger.Proxy.CountCalls("LogInfo", (doing read-args -message "Deleting persons file...")) | Should -Be 1;
            $script:mockLogger.Proxy.CountCalls("LogInfo", (doing read-args -message "Deleted persons file...")) | Should -Be 1;

            $script:mockDeletePersonsFile.Proxy.CountCalls("Delete") | Should -Be 1;

            $script:mockContext.Proxy.CountPropertyCalls("Session") | Should -Be 0;
            $script:mockContext.Proxy.CountCalls("KeyExists") | Should -Be 0;
            $script:mockSession.Proxy.CountPropertyCalls("ProcessReports") | Should -Be 0;
        }

        It 'Processes as expected' {
            # Arrange 
            [string] $processName = "DeleteFile";

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
