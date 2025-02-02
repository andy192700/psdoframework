using namespace DoFramework.Domain;
using namespace DoFramework.Logging;
using namespace DoFramework.Processing;
using namespace DoFramework.Testing;
using namespace System.Collections.Generic;
using module "..\..\..\Modules\CreatePersonsFile.psm1";

Describe 'CreateFileTests' {
    BeforeEach {
        [ProxyResult] $script:mockSession = doing mock -type ([ISession]);

        [ProxyResult] $script:mockContext = doing mock -type ([IContext]);

        $script:mockContext.Proxy.MockProperty("Session", {
            return $script:mockSession.Instance;
        });
        
        [ProxyResult] $script:mockLogger = doing mock -type ([ILogger]);
        
        [ProxyResult] $script:mockCreatePersonsFile = doing mock -type ([CreatePersonsFile]) -params @($script:mockContext.Instance);
    }

    Context 'CreateFileTests' {
        It 'Is Invalid, no prior reports' {
            # Arrange
            [CreateFile] $sut = [CreateFile]::new(
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
            $script:mockSession.Proxy.CountCalls("KeyExists") | Should -Be 0;
            $script:mockSession.Proxy.CountPropertyCalls("ProcessReports") | Should -Be 1;
        }

        It 'Is Invalid, first report name does not match' {
            # Arrange
            [CreateFile] $sut = [CreateFile]::new(
                $script:mockCreatePersonsFile.Instance,
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
                
                $reports.Add($p1);
                $reports.Add($p2);
                $reports.Add($p3);

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
            [CreateFile] $sut = [CreateFile]::new(
                $script:mockCreatePersonsFile.Instance,
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
                
                $reports.Add($p1);
                $reports.Add($p2);
                $reports.Add($p3);

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
            [CreateFile] $sut = [CreateFile]::new(
                $script:mockCreatePersonsFile.Instance,
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
                
                $reports.Add($p1);
                $reports.Add($p2);
                $reports.Add($p3);

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

        It 'Is Invalid, context entry missing' {
            # Arrange
            [CreateFile] $sut = [CreateFile]::new(
                $script:mockCreatePersonsFile.Instance,
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
                
                $reports.Add($p1);
                $reports.Add($p2);
                $reports.Add($p3);

                return $reports;
            });

            # Act
            [bool] $result = $sut.Validate();

            # Assert
            $result | Should -Be $false;

            $script:mockContext.Proxy.CountPropertyCalls("Session") | Should -Be 4;
            $script:mockContext.Proxy.CountCalls("KeyExists") | Should -Be 1;
            $script:mockContext.Proxy.CountCalls("KeyExists", (doing args -key "PersonsFilePath")) | Should -Be 1;
            $script:mockSession.Proxy.CountPropertyCalls("ProcessReports") | Should -Be 4;
        }

        It 'Is Valid' {
            # Arrange
            [CreateFile] $sut = [CreateFile]::new(
                $script:mockCreatePersonsFile.Instance,
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
                
                $reports.Add($p1);
                $reports.Add($p2);
                $reports.Add($p3);

                return $reports;
            });

            # Act
            [bool] $result = $sut.Validate();

            # Assert
            $result | Should -Be $true;

            $script:mockContext.Proxy.CountPropertyCalls("Session") | Should -Be 4;
            $script:mockContext.Proxy.CountCalls("KeyExists") | Should -Be 1;
            $script:mockContext.Proxy.CountCalls("KeyExists", (doing args -key "PersonsFilePath")) | Should -Be 1;
            $script:mockSession.Proxy.CountPropertyCalls("ProcessReports") | Should -Be 4;
        }
        
        It 'Runs as Expected' {
            # Arrange
            [CreateFile] $sut = [CreateFile]::new(
                $script:mockCreatePersonsFile.Instance,
                $script:mockLogger.Instance,
                $script:mockContext.Instance);

            # Act
            $sut.Run();

            # Assert
            $script:mockLogger.Proxy.CountCalls("LogInfo") | Should -Be 2;
            $script:mockLogger.Proxy.CountCalls("LogInfo", (doing args -message "Creating persons file...")) | Should -Be 1;
            $script:mockLogger.Proxy.CountCalls("LogInfo", (doing args -message "Created persons file...")) | Should -Be 1;

            $script:mockCreatePersonsFile.Proxy.CountCalls("Create") | Should -Be 1;

            $script:mockContext.Proxy.CountPropertyCalls("Session") | Should -Be 0;
            $script:mockSession.Proxy.CountCalls("KeyExists") | Should -Be 0;
            $script:mockSession.Proxy.CountPropertyCalls("ProcessReports") | Should -Be 0;
        }

        It 'Processes as expected' {
            # Arrange 
            [string] $processName = "CreateFile";

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
