using namespace DoFramework.Domain;
using namespace DoFramework.Logging;
using namespace DoFramework.Processing;
using namespace DoFramework.Testing;
using namespace System.Collections.Generic;
using module "..\..\..\Modules\CreatePersons.psm1";

Describe 'CreateDataTests' {
    BeforeEach {
        [ProxyResult] $script:mockSession = doing mock -type ([ISession]);

        [ProxyResult] $script:mockContext = doing mock -type ([IContext]);

        $script:mockContext.Proxy.MockProperty("Session", {
            return $script:mockSession.Instance;
        });
        
        [ProxyResult] $script:mockLogger = doing mock -type ([ILogger]);
        
        [ProxyResult] $script:mockCreatePersons = doing mock -type ([CreatePersons]) -params @($script:mockContext.Instance);
        
        [IContext] $script:context = [Context]::new([Session]::new());
    }

    Context 'CreateDataTests' {
        It 'Is Invalid, no prior reports' {
            # Arrange
            [CreateData] $sut = [CreateData]::new(
                [CreatePersons]($script:mockCreatePersons.Instance),
                $script:mockLogger.Instance,
                $script:context);
    
            # Act
            [bool] $result = $sut.Validate();

            # Assert
            $result | Should -Be $false;
        }
    
        It 'Is valid' {
            # Arrange
            [CreateData] $sut = [CreateData]::new(
                [CreatePersons]($script:mockCreatePersons.Instance),
                $script:mockLogger.Instance,
                $script:context);
                
            [ProcessReport] $p1 = [ProcessReport]::new();
            $p1.Name = "DeleteFile";
            $p1.ProcessResult = [ProcessResult]::Completed;
            $script:context.Session.ProcessReports.Add($p1);

            # Act
            [bool] $result = $sut.Validate();

            # Assert
            $result | Should -Be $true;
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
            $script:mockLogger.Proxy.CountCalls("LogInfo", (doing args -message "Creating persons data...")) | Should -Be 1;
            $script:mockLogger.Proxy.CountCalls("LogInfo", (doing args -message "Created persons data...")) | Should -Be 1;

            $script:mockCreatePersons.Proxy.CountCalls("Create") | Should -Be 1;

            $script:mockContext.Proxy.CountPropertyCalls("Session") | Should -Be 0;
            $script:mockSession.Proxy.CountPropertyCalls("ProcessReports") | Should -Be 0;
        }

        It 'Processes as expected' {
            # Arrange 
            [string] $processName = "CreateData";

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
