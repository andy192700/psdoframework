using namespace DoFramework.Domain;
using namespace DoFramework.Processing;
using namespace DoFramework.Testing;

Describe 'AdvancedProcessTests' {
    BeforeEach {
        [ProxyResult] $script:mockContext = doing mock -type ([IContext]);
        
        [ProxyResult] $script:mockDispatcher = doing mock -type ([IProcessDispatcher]);
    }

    Context 'AdvancedProcessTests' {
        It 'Is invalid' {
            # Arrange
            [AdvancedProcess] $sut = [AdvancedProcess]::new($script:mockDispatcher.Instance, $script:mockContext.Instance);

            $mockContext.Proxy.MockMethod("KeyExists", {
                param ([string] $key)

                return $false;
            });

            # Act
            [bool] $result = $sut.Validate();

            # Assert
            $result | Should -Be $false;
            $script:mockContext.Proxy.CountCalls("KeyExists") | Should -Be 1; 
            $script:mockContext.Proxy.CountCalls("KeyExists", (doing args -key "Person1FirstName")) | Should -Be 1;
            $script:mockContext.Proxy.CountCalls("KeyExists", (doing args -key "Person1LastName")) | Should -Be 0;
            $script:mockContext.Proxy.CountCalls("KeyExists", (doing args -key "Person1Age")) | Should -Be 0;
            $script:mockContext.Proxy.CountCalls("KeyExists", (doing args -key "Person2FirstName")) | Should -Be 0;
            $script:mockContext.Proxy.CountCalls("KeyExists", (doing args -key "Person2LastName")) | Should -Be 0;
            $script:mockContext.Proxy.CountCalls("KeyExists", (doing args -key "Person2Age")) | Should -Be 0;
            $script:mockContext.Proxy.CountCalls("KeyExists", (doing args -key "Person3FirstName")) | Should -Be 0;
            $script:mockContext.Proxy.CountCalls("KeyExists", (doing args -key "Person3LastName")) | Should -Be 0;
            $script:mockContext.Proxy.CountCalls("KeyExists", (doing args -key "Person3Age")) | Should -Be 0;
        }
        It 'Is valid' {
            # Arrange
            [AdvancedProcess] $sut = [AdvancedProcess]::new($script:mockDispatcher.Instance, $script:mockContext.Instance);

            $mockContext.Proxy.MockMethod("KeyExists", {
                param ([string] $key)

                return $true;
            });

            # Act
            [bool] $result = $sut.Validate();

            # Assert
            $result | Should -Be $true;
            $script:mockContext.Proxy.CountCalls("KeyExists") | Should -Be 9; 
            $script:mockContext.Proxy.CountCalls("KeyExists", (doing args -key "Person1FirstName")) | Should -Be 1;
            $script:mockContext.Proxy.CountCalls("KeyExists", (doing args -key "Person1LastName")) | Should -Be 1;
            $script:mockContext.Proxy.CountCalls("KeyExists", (doing args -key "Person1Age")) | Should -Be 1;
            $script:mockContext.Proxy.CountCalls("KeyExists", (doing args -key "Person2FirstName")) | Should -Be 1;
            $script:mockContext.Proxy.CountCalls("KeyExists", (doing args -key "Person2LastName")) | Should -Be 1;
            $script:mockContext.Proxy.CountCalls("KeyExists", (doing args -key "Person2Age")) | Should -Be 1;
            $script:mockContext.Proxy.CountCalls("KeyExists", (doing args -key "Person3FirstName")) | Should -Be 1;
            $script:mockContext.Proxy.CountCalls("KeyExists", (doing args -key "Person3LastName")) | Should -Be 1;
            $script:mockContext.Proxy.CountCalls("KeyExists", (doing args -key "Person3Age")) | Should -Be 1;
        }

        It 'Dispatches processes' {
            # Arrange
            [AdvancedProcess] $sut = [AdvancedProcess]::new($script:mockDispatcher.Instance, $script:mockContext.Instance);

            [ProcessingRequest] $script:request;

            $mockDispatcher.Proxy.MockMethod("Dispatch", {
                param ([ProcessingRequest] $processingRequest)

                $script:request = $processingRequest;
            });

            # Act
            $sut.Run();

            # Assert
            $script:mockContext.Proxy.CountCalls("KeyExists") | Should -Be 0;
            $script:mockDispatcher.Proxy.CountCalls("Dispatch") | Should -Be 1;

            $script:request.Processes.Length | Should -Be 6;
            $script:request.Processes -contains "Registrations" | Should -Be $true;
            $script:request.Processes -contains "DeleteFile" | Should -Be $true;
            $script:request.Processes -contains "CreateData" | Should -Be $true;
            $script:request.Processes -contains "CreateFile" | Should -Be $true;
            $script:request.Processes -contains "ReadFile" | Should -Be $true;
            $script:request.Processes -contains "DeleteFile" | Should -Be $true;
        }

        It 'Processes as expected' {
            # Arrange 
            [string] $processName = "AdvancedProcess";

            # Act
            [IContext] $result = doing run -name $processName -doOutput -silent;

            # Assert
            $result | Should -Not -Be $null;
            $result.Session.ProcessCount | Should -Be 7;
            $result.Session.ProcessReports.Count | Should -Be 7;

            [string[]] $processes = @(
                "Registrations",
                "DeleteFile",
                "CreateData",
                "CreateFile",
                "ReadFile",
                "DeleteFile"
            );

            for ($i = 0; $i -lt $processes.Length; $i++) {
                $result.Session.ProcessReports[$i].Descriptor.Name | Should -Be $processes[$i];
                $result.Session.ProcessReports[$i].ProcessResult | Should -Be ([ProcessResult]::Completed);
            }

            $result.Session.ProcessReports[6].Descriptor.Name | Should -Be $processName;
            $result.Session.ProcessReports[6].ProcessResult | Should -Be ([ProcessResult]::Completed);
        }
    }
}
