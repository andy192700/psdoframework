using namespace DoFramework.Domain;
using namespace DoFramework.Logging;
using namespace DoFramework.Processing;
using namespace DoFramework.Testing;
using namespace System.Collections.Generic;
using module "..\..\..\Modules\CreatePersons.psm1";

Describe 'CreateDataTests' {
    BeforeEach {
        [ProxyResult] $script:mockSession = doing create-proxy -type ([ISession]);

        [ProxyResult] $script:mockContext = doing create-proxy -type ([IContext]);

        $script:mockContext.Proxy.MockProperty("Session", {
            return $script:mockSession.Instance;
        });
        
        [ProxyResult] $script:mockLogger = doing create-proxy -type ([ILogger]);
        
        [ProxyResult] $script:mockCreatePersons = doing create-proxy -type ([CreatePersons]) -params @($script:mockContext.Instance);
    }

    Context 'CreateDataTests' {
        It 'Is Invalid, no prior reports' {
            # Arrange
            [CreateData] $sut = [CreateData]::new(
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
            $script:mockSession.Proxy.CountPropertyCalls("ProcessReports") | Should -Be 1;
        }
    }

    Context 'CreateDataTests' {
        It 'Is Invalid, first report name does not match' {
            # Arrange
            [CreateData] $sut = [CreateData]::new(
                [CreatePersons]($script:mockCreatePersons.Instance),
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
                
                $reports.Add($p1);
                $reports.Add($p2);

                return $reports;
            });

            # Act
            [bool] $result = $sut.Validate();

            # Assert
            $result | Should -Be $false;

            $script:mockContext.Proxy.CountPropertyCalls("Session") | Should -Be 2;
            $script:mockSession.Proxy.CountPropertyCalls("ProcessReports") | Should -Be 2;
        }

        It 'Is Invalid, second report name does not match' {
            # Arrange
            [CreateData] $sut = [CreateData]::new(
                [CreatePersons]($script:mockCreatePersons.Instance),
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
                
                $reports.Add($p1);
                $reports.Add($p2);               

                return $reports;
            });

            # Act
            [bool] $result = $sut.Validate();

            # Assert
            $result | Should -Be $false;

            $script:mockContext.Proxy.CountPropertyCalls("Session") | Should -Be 3;
            $script:mockSession.Proxy.CountPropertyCalls("ProcessReports") | Should -Be 3;
        }
        
        It 'Is valid' {
            # Arrange
            [CreateData] $sut = [CreateData]::new(
                [CreatePersons]($script:mockCreatePersons.Instance),
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
                
                $reports.Add($p1);
                $reports.Add($p2);

                return $reports;
            });

            # Act
            [bool] $result = $sut.Validate();

            # Assert
            $result | Should -Be $true;

            $script:mockContext.Proxy.CountPropertyCalls("Session") | Should -Be 3;
            $script:mockSession.Proxy.CountPropertyCalls("ProcessReports") | Should -Be 3;
        }
        
        It 'Runs as Expected' {
            # Arrange
            [CreateData] $sut = [CreateData]::new(
                [CreatePersons]($script:mockCreatePersons.Instance),
                $script:mockLogger.Instance,
                $script:mockContext.Instance);

            # Act
            $sut.Run();

            # Assert
            $script:mockLogger.Proxy.CountCalls("LogInfo") | Should -Be 2;
            $script:mockLogger.Proxy.CountCalls("LogInfo", (doing read-args -message "Creating persons data...")) | Should -Be 1;
            $script:mockLogger.Proxy.CountCalls("LogInfo", (doing read-args -message "Created persons data...")) | Should -Be 1;

            $script:mockCreatePersons.Proxy.CountCalls("Create") | Should -Be 1;

            $script:mockContext.Proxy.CountPropertyCalls("Session") | Should -Be 0;
            $script:mockSession.Proxy.CountPropertyCalls("ProcessReports") | Should -Be 0;
        }

        It 'Processes as expected' {
            # Arrange 
            [string] $processName = "CreateData";

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
