using namespace DoFramework.Domain;
using namespace DoFramework.Logging;
using namespace DoFramework.Processing;
using namespace DoFramework.Testing;
using namespace System.Collections.Generic;
using module "..\..\..\Modules\ReadPersonsFile.psm1";

Describe 'ReadFileTests' {
    BeforeEach {
        [ProxyResult] $script:mockSession = doing mock -type ([ISession]);

        [ProxyResult] $script:mockContext = doing mock -type ([IContext]);

        $script:mockContext.Proxy.MockProperty("Session", {
            return $script:mockSession.Instance;
        });
        
        [ProxyResult] $script:mockLogger = doing mock -type ([ILogger]);
        
        [ProxyResult] $script:mockReadPersonsFile = doing mock -type ([ReadPersonsFile]) -params @($script:mockContext.Instance);

        [IContext] $script:context = [Context]::new([Session]::new());      
    }

    Context 'ReadFileTests' {
        It 'Is Invalid, no prior reports' {
            # Arrange
            [ReadFile] $sut = [ReadFile]::new(
                $script:mockReadPersonsFile.Instance,
                $script:mockLogger.Instance,
                $script:context);

            # Act
            [bool] $result = $sut.Validate();

            # Assert
            $result | Should -Be $false;
        }

        It 'Is Valid' {
            # Arrange
            [ReadFile] $sut = [ReadFile]::new(
                $script:mockReadPersonsFile.Instance,
                $script:mockLogger.Instance,
                $script:context);
            
            [ProcessReport] $p1 = [ProcessReport]::new();
            $p1.Name = "CreateFile";
            $p1.ProcessResult = [ProcessResult]::Completed;
            $script:context.Session.ProcessReports.Add($p1);

            $script:context.AddOrUpdate("PersonsFilePath", "sample value");

            # Act
            [bool] $result = $sut.Validate();

            # Assert
            $result | Should -Be $true;
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
            $script:mockLogger.Proxy.CountCalls("LogInfo", (doing args -message "Reading persons file...")) | Should -Be 1;
            $script:mockLogger.Proxy.CountCalls("LogInfo", (doing args -message "Found 0 persons.")) | Should -Be 1;
            $script:mockLogger.Proxy.CountCalls("LogInfo", (doing args -message "Here they are:")) | Should -Be 1;

            $script:mockReadPersonsFile.Proxy.CountCalls("Read") | Should -Be 1;
            
            $script:mockContext.Proxy.CountPropertyCalls("Session") | Should -Be 0;
            $script:mockSession.Proxy.CountCalls("KeyExists") | Should -Be 0;
            $script:mockSession.Proxy.CountPropertyCalls("ProcessReports") | Should -Be 0;
        }

        It 'Processes as expected' {
            # Arrange 
            [string] $processName = "ReadFile";

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
