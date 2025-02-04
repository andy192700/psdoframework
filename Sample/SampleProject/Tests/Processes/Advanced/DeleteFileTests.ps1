using namespace DoFramework.Domain;
using namespace DoFramework.Logging;
using namespace DoFramework.Processing;
using namespace DoFramework.Testing;
using namespace System.Collections.Generic;
using module "..\..\..\Modules\DeletePersonsFile.psm1";

Describe 'DeleteFileTests' {
    BeforeEach {
        [ProxyResult] $script:mockSession = doing mock -type ([ISession]);

        [ProxyResult] $script:mockContext = doing mock -type ([IContext]);

        $script:mockContext.Proxy.MockProperty("Session", {
            return $script:mockSession.Instance;
        });
        
        [ProxyResult] $script:mockLogger = doing mock -type ([ILogger]);
        
        [ProxyResult] $script:mockDeletePersonsFile = doing mock -type ([DeletePersonsFile]) -params @($script:mockContext.Instance);
        
        [IContext] $script:context = [Context]::new([Session]::new());
    }
    Context 'DeleteFileTests' {
        It 'Is Invalid, no prior reports' {
            # Arrange
            [DeleteFile] $sut = [DeleteFile]::new(
                $script:mockCreatePersons.Instance,
                $script:mockLogger.Instance,
                $script:context);

            # Act
            [bool] $result = $sut.Validate();

            # Assert
            $result | Should -Be $false;
        }

        It 'Is Valid' {
            # Arrange
            [DeleteFile] $sut = [DeleteFile]::new(
                $script:mockCreatePersons.Instance,
                $script:mockLogger.Instance,
                $script:context);
                
            [ProcessReport] $p1 = [ProcessReport]::new();
            $p1.Name = "AdvancedProcess";
            $p1.ProcessResult = [ProcessResult]::Completed;
            $script:context.Session.ProcessReports.Add($p1);

            $script:context.SetComposedBy("AdvancedComposer");

            $script:context.AddOrUpdate("PersonsFilePath", "sample value");

            # Act
            [bool] $result = $sut.Validate();

            # Assert
            $result | Should -Be $true;
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
            $script:mockLogger.Proxy.CountCalls("LogInfo", (doing args -message "Deleting persons file...")) | Should -Be 1;
            $script:mockLogger.Proxy.CountCalls("LogInfo", (doing args -message "Deleted persons file...")) | Should -Be 1;

            $script:mockDeletePersonsFile.Proxy.CountCalls("Delete") | Should -Be 1;

            $script:mockContext.Proxy.CountPropertyCalls("Session") | Should -Be 0;
            $script:mockContext.Proxy.CountCalls("KeyExists") | Should -Be 0;
            $script:mockSession.Proxy.CountPropertyCalls("ProcessReports") | Should -Be 0;
        }

        It 'Processes as expected' {
            # Arrange 
            [string] $processName = "DeleteFile";

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
