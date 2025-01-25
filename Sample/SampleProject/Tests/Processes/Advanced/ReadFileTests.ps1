using namespace DoFramework.Domain;
using namespace DoFramework.Logging;
using namespace DoFramework.Processing;
using namespace DoFramework.Testing;
using namespace System.Collections.Generic;
using module "..\..\..\Modules\ReadPersonsFile.psm1";

Describe 'ReadFileTests' {
    BeforeEach {
        [ProxyResult] $script:mockSession = doing create-proxy -type ([ISession]);

        [ProxyResult] $script:mockContext = doing create-proxy -type ([IContext]);

        $script:mockContext.Proxy.MockProperty("Session", {
            return $script:mockSession.Instance;
        });
        
        [ProxyResult] $script:mockLogger = doing create-proxy -type ([ILogger]);
        
        [ProxyResult] $script:mockReadPersonsFile = doing create-proxy -type ([ReadPersonsFile]) -params @($script:mockContext.Instance);
    }

    Context 'ReadFileTests' {
        It 'Is Invalid, no prior reports' {
            # Arrange
            [ReadFile] $sut = [ReadFile]::new(
                $script:mockReadPersonsFile.Instance,
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
            $script:mockSession.Proxy.CountCalls("KeyExists") | Should -Be 0;
            $script:mockSession.Proxy.CountPropertyCalls("ProcessReports") | Should -Be 1;
        }

        It 'Is Invalid, first report name does not match' {
            # Arrange
            [ReadFile] $sut = [ReadFile]::new(
                $script:mockReadPersonsFile.Instance,
                $script:mockLogger.Instance,
                $script:mockContext.Instance);
                
            $script:mockSession.Proxy.MockProperty("ProcessReports", {
                [List[ProcessReport]] $reports = [List[ProcessReport]]::new();

                [ProcessReport] $p1 = [ProcessReport]::new();
                [ProcessDescriptor] $ps1 = [ProcessDescriptor]::new();
                $ps1.Name = "wrong";
                $p1.Descriptor = $ps1;
                
                [ProcessReport] $p2 = [ProcessReport]::new();
                [ProcessDescriptor] $ps2 = [ProcessDescriptor]::new();
                $ps2.Name = "DeleteFile";
                $p2.Descriptor = $ps2;
                
                [ProcessReport] $p3 = [ProcessReport]::new();
                [ProcessDescriptor] $ps3 = [ProcessDescriptor]::new();
                $ps3.Name = "CreateData";
                $p3.Descriptor = $ps3;
                
                [ProcessReport] $p4 = [ProcessReport]::new();
                [ProcessDescriptor] $ps4 = [ProcessDescriptor]::new();
                $ps4.Name = "CreateFile";
                $p4.Descriptor = $ps4;
                
                $reports.Add($p1);
                $reports.Add($p2);
                $reports.Add($p3);
                $reports.Add($p4);

                return $reports;
            });

            # Act
            [bool] $result = $sut.Validate();

            # Assert
            $result | Should -Be $false;

            $script:mockContext.Proxy.CountPropertyCalls("Session") | Should -Be 2;
            $script:mockSession.Proxy.CountCalls("KeyExists") | Should -Be 0;
            $script:mockSession.Proxy.CountPropertyCalls("ProcessReports") | Should -Be 2;
        }

        It 'Is Invalid, second report name does not match' {
            # Arrange
            [ReadFile] $sut = [ReadFile]::new(
                $script:mockReadPersonsFile.Instance,
                $script:mockLogger.Instance,
                $script:mockContext.Instance);
                
            $script:mockSession.Proxy.MockProperty("ProcessReports", {
                [List[ProcessReport]] $reports = [List[ProcessReport]]::new();

                [ProcessReport] $p1 = [ProcessReport]::new();
                [ProcessDescriptor] $ps1 = [ProcessDescriptor]::new();
                $ps1.Name = "Registrations";
                $p1.Descriptor = $ps1;
                
                [ProcessReport] $p2 = [ProcessReport]::new();
                [ProcessDescriptor] $ps2 = [ProcessDescriptor]::new();
                $ps2.Name = "wrong";
                $p2.Descriptor = $ps2;
                
                [ProcessReport] $p3 = [ProcessReport]::new();
                [ProcessDescriptor] $ps3 = [ProcessDescriptor]::new();
                $ps3.Name = "CreateData";
                $p3.Descriptor = $ps3;
                
                [ProcessReport] $p4 = [ProcessReport]::new();
                [ProcessDescriptor] $ps4 = [ProcessDescriptor]::new();
                $ps4.Name = "CreateFile";
                $p4.Descriptor = $ps4;
                
                $reports.Add($p1);
                $reports.Add($p2);
                $reports.Add($p3);
                $reports.Add($p4);

                return $reports;
            });

            # Act
            [bool] $result = $sut.Validate();

            # Assert
            $result | Should -Be $false;

            $script:mockContext.Proxy.CountPropertyCalls("Session") | Should -Be 3;
            $script:mockSession.Proxy.CountCalls("KeyExists") | Should -Be 0;
            $script:mockSession.Proxy.CountPropertyCalls("ProcessReports") | Should -Be 3;
        }

        It 'Is Invalid, third report name does not match' {
            # Arrange
            [ReadFile] $sut = [ReadFile]::new(
                $script:mockReadPersonsFile.Instance,
                $script:mockLogger.Instance,
                $script:mockContext.Instance);
                
            $script:mockSession.Proxy.MockProperty("ProcessReports", {
                [List[ProcessReport]] $reports = [List[ProcessReport]]::new();

                [ProcessReport] $p1 = [ProcessReport]::new();
                [ProcessDescriptor] $ps1 = [ProcessDescriptor]::new();
                $ps1.Name = "Registrations";
                $p1.Descriptor = $ps1;
                
                [ProcessReport] $p2 = [ProcessReport]::new();
                [ProcessDescriptor] $ps2 = [ProcessDescriptor]::new();
                $ps2.Name = "DeleteFile";
                $p2.Descriptor = $ps2;
                
                [ProcessReport] $p3 = [ProcessReport]::new();
                [ProcessDescriptor] $ps3 = [ProcessDescriptor]::new();
                $ps3.Name = "wrong";
                $p3.Descriptor = $ps3;
                
                [ProcessReport] $p4 = [ProcessReport]::new();
                [ProcessDescriptor] $ps4 = [ProcessDescriptor]::new();
                $ps4.Name = "CreateFile";
                $p4.Descriptor = $ps4;
                
                $reports.Add($p1);
                $reports.Add($p2);
                $reports.Add($p3);
                $reports.Add($p4);

                return $reports;
            });

            # Act
            [bool] $result = $sut.Validate();

            # Assert
            $result | Should -Be $false;

            $script:mockContext.Proxy.CountPropertyCalls("Session") | Should -Be 4;
            $script:mockSession.Proxy.CountCalls("KeyExists") | Should -Be 0;
            $script:mockSession.Proxy.CountPropertyCalls("ProcessReports") | Should -Be 4;
        }

        It 'Is Invalid, fourth report name does not match' {
            # Arrange
            [ReadFile] $sut = [ReadFile]::new(
                $script:mockReadPersonsFile.Instance,
                $script:mockLogger.Instance,
                $script:mockContext.Instance);
                
            $script:mockSession.Proxy.MockProperty("ProcessReports", {
                [List[ProcessReport]] $reports = [List[ProcessReport]]::new();

                [ProcessReport] $p1 = [ProcessReport]::new();
                [ProcessDescriptor] $ps1 = [ProcessDescriptor]::new();
                $ps1.Name = "Registrations";
                $p1.Descriptor = $ps1;
                
                [ProcessReport] $p2 = [ProcessReport]::new();
                [ProcessDescriptor] $ps2 = [ProcessDescriptor]::new();
                $ps2.Name = "DeleteFile";
                $p2.Descriptor = $ps2;
                
                [ProcessReport] $p3 = [ProcessReport]::new();
                [ProcessDescriptor] $ps3 = [ProcessDescriptor]::new();
                $ps3.Name = "CreateData";
                $p3.Descriptor = $ps3;
                
                [ProcessReport] $p4 = [ProcessReport]::new();
                [ProcessDescriptor] $ps4 = [ProcessDescriptor]::new();
                $ps4.Name = "wrong";
                $p4.Descriptor = $ps4;
                
                $reports.Add($p1);
                $reports.Add($p2);
                $reports.Add($p3);
                $reports.Add($p4);

                return $reports;
            });

            # Act
            [bool] $result = $sut.Validate();

            # Assert
            $result | Should -Be $false;

            $script:mockContext.Proxy.CountPropertyCalls("Session") | Should -Be 5;
            $script:mockSession.Proxy.CountCalls("KeyExists") | Should -Be 0;
            $script:mockSession.Proxy.CountPropertyCalls("ProcessReports") | Should -Be 5;
        }

        It 'Is Invalid, missing context entry' {
            # Arrange
            [ReadFile] $sut = [ReadFile]::new(
                $script:mockReadPersonsFile.Instance,
                $script:mockLogger.Instance,
                $script:mockContext.Instance);

            $script:mockContext.Proxy.MockMethod("KeyExists", {
                param (
                    [string] $key
                )

                return $false;
            });
                
            $script:mockSession.Proxy.MockProperty("ProcessReports", {
                [List[ProcessReport]] $reports = [List[ProcessReport]]::new();

                [ProcessReport] $p1 = [ProcessReport]::new();
                [ProcessDescriptor] $ps1 = [ProcessDescriptor]::new();
                $ps1.Name = "Registrations";
                $p1.Descriptor = $ps1;
                
                [ProcessReport] $p2 = [ProcessReport]::new();
                [ProcessDescriptor] $ps2 = [ProcessDescriptor]::new();
                $ps2.Name = "DeleteFile";
                $p2.Descriptor = $ps2;
                
                [ProcessReport] $p3 = [ProcessReport]::new();
                [ProcessDescriptor] $ps3 = [ProcessDescriptor]::new();
                $ps3.Name = "CreateData";
                $p3.Descriptor = $ps3;
                
                [ProcessReport] $p4 = [ProcessReport]::new();
                [ProcessDescriptor] $ps4 = [ProcessDescriptor]::new();
                $ps4.Name = "CreateFile";
                $p4.Descriptor = $ps4;
                
                $reports.Add($p1);
                $reports.Add($p2);
                $reports.Add($p3);
                $reports.Add($p4);

                return $reports;
            });

            # Act
            [bool] $result = $sut.Validate();

            # Assert
            $result | Should -Be $false;

            $script:mockContext.Proxy.CountPropertyCalls("Session") | Should -Be 5;
            $script:mockContext.Proxy.CountCalls("KeyExists") | Should -Be 1;
            $script:mockContext.Proxy.CountCalls("KeyExists", (doing read-args -key "PersonsFilePath")) | Should -Be 1;
            $script:mockSession.Proxy.CountPropertyCalls("ProcessReports") | Should -Be 5;
        }

        It 'Is Valid' {
            # Arrange
            [ReadFile] $sut = [ReadFile]::new(
                $script:mockReadPersonsFile.Instance,
                $script:mockLogger.Instance,
                $script:mockContext.Instance);

            $script:mockContext.Proxy.MockMethod("KeyExists", {
                param (
                    [string] $key
                )

                return $true;
            });
                
            $script:mockSession.Proxy.MockProperty("ProcessReports", {
                [List[ProcessReport]] $reports = [List[ProcessReport]]::new();

                [ProcessReport] $p1 = [ProcessReport]::new();
                [ProcessDescriptor] $ps1 = [ProcessDescriptor]::new();
                $ps1.Name = "Registrations";
                $p1.Descriptor = $ps1;
                
                [ProcessReport] $p2 = [ProcessReport]::new();
                [ProcessDescriptor] $ps2 = [ProcessDescriptor]::new();
                $ps2.Name = "DeleteFile";
                $p2.Descriptor = $ps2;
                
                [ProcessReport] $p3 = [ProcessReport]::new();
                [ProcessDescriptor] $ps3 = [ProcessDescriptor]::new();
                $ps3.Name = "CreateData";
                $p3.Descriptor = $ps3;
                
                [ProcessReport] $p4 = [ProcessReport]::new();
                [ProcessDescriptor] $ps4 = [ProcessDescriptor]::new();
                $ps4.Name = "CreateFile";
                $p4.Descriptor = $ps4;
                
                $reports.Add($p1);
                $reports.Add($p2);
                $reports.Add($p3);
                $reports.Add($p4);

                return $reports;
            });

            # Act
            [bool] $result = $sut.Validate();

            # Assert
            $result | Should -Be $true;

            $script:mockContext.Proxy.CountPropertyCalls("Session") | Should -Be 5;
            $script:mockContext.Proxy.CountCalls("KeyExists") | Should -Be 1;
            $script:mockContext.Proxy.CountCalls("KeyExists", (doing read-args -key "PersonsFilePath")) | Should -Be 1;
            $script:mockSession.Proxy.CountPropertyCalls("ProcessReports") | Should -Be 5;
        }
        
        It 'Runs as Expected' {
            # Arrange
            [ReadFile] $sut = [ReadFile]::new(
                $script:mockReadPersonsFile.Instance,
                $script:mockLogger.Instance,
                $script:mockContext.Instance);

            # Act
            $sut.Run();

            # Assert
            $script:mockLogger.Proxy.CountCalls("LogInfo") | Should -Be 3;
            $script:mockLogger.Proxy.CountCalls("LogInfo", (doing read-args -message "Reading persons file...")) | Should -Be 1;
            $script:mockLogger.Proxy.CountCalls("LogInfo", (doing read-args -message "Found 0 persons.")) | Should -Be 1;
            $script:mockLogger.Proxy.CountCalls("LogInfo", (doing read-args -message "Here they are:")) | Should -Be 1;

            $script:mockReadPersonsFile.Proxy.CountCalls("Read") | Should -Be 1;
            
            $script:mockContext.Proxy.CountPropertyCalls("Session") | Should -Be 0;
            $script:mockSession.Proxy.CountCalls("KeyExists") | Should -Be 0;
            $script:mockSession.Proxy.CountPropertyCalls("ProcessReports") | Should -Be 0;
        }

        It 'Processes as expected' {
            # Arrange 
            [string] $processName = "ReadFile";

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
