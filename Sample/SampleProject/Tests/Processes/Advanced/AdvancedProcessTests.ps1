using namespace DoFramework.Domain;
using namespace DoFramework.Processing;
using namespace DoFramework.Testing;

Describe 'AdvancedProcessTests' {
    BeforeEach {
        [IContext] $script:context = [Context]::new([Session]::new());
    }

    Context 'AdvancedProcessTests' {
        It 'Is invalid' {
            # Arrange
            [AdvancedProcess] $sut = [AdvancedProcess]::new($script:context);

            # Act
            [bool] $result = $sut.Validate();

            # Assert
            $result | Should -Be $false;
        }

        It 'Is valid' {
            # Arrange
            $script:context.SetComposedBy("AdvancedComposer");
            $script:context.AddOrUpdate("Person1FirstName", "some name1");
            $script:context.AddOrUpdate("Person1LastName", "some name1");
            $script:context.AddOrUpdate("Person1Age", 22);
            $script:context.AddOrUpdate("Person2FirstName", "some name2");
            $script:context.AddOrUpdate("Person2LastName", "some name2");
            $script:context.AddOrUpdate("Person2Age", 3);
            $script:context.AddOrUpdate("Person3FirstName", "some name3");
            $script:context.AddOrUpdate("Person3LastName", "some name3");
            $script:context.AddOrUpdate("Person3Age", 5);

            [AdvancedProcess] $sut = [AdvancedProcess]::new($script:context);

            # Act
            [bool] $result = $sut.Validate();

            # Assert
            $result | Should -Be $true;
        }

        It 'Processes as expected' {
            # Arrange
            [string] $currentDir = "The Directory";
            
            Mock Get-Location {
                return $currentDir;
            }

            [ProxyResult] $mockContext = doing mock -type ([IContext]);
            
            [AdvancedProcess] $sut = [AdvancedProcess]::new($mockContext.Instance);

            # Act
            $sut.Run();

            # Assert
            Should -Invoke -CommandName Get-Location -Times 1;

            $mockContext.Proxy.CountCalls("AddOrUpdate", (doing args -PersonsFilePath "$($currentDir)$([System.IO.Path]::DirectorySeparatorChar)persons.json"));
        }

        It 'Runs as expected' {
            # Arrange 
            [string] $processName = "AdvancedProcess";

            # Act
            [IContext] $result = doing run -name $processName -doOutput -silent;

            # Assert
            $result | Should -Not -Be $null;
            $result.
                Requires().
                ProcessSucceeded($processName).
                Verify() | Should -Be $false;
        }
    }
}
