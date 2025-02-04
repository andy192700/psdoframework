using namespace DoFramework.Processing;
using namespace DoFramework.Testing;
using module "..\..\Modules\DeletePersonsFile.psm1";
using module "..\..\Modules\Models\Person.psm1";

Describe 'DeletePersonsFileTests' {
    BeforeEach {
        [ProxyResult] $script:mockContext = doing mock -type ([IContext]);
    }

    Context 'DeletePersonsFileTests' {
        It 'File does not exist, does not attempt to remove' {
            # Arrange
            [string] $filePath = "The File Path";

            [DeletePersonsFile] $sut = [DeletePersonsFile]::new($script:mockContext.Instance);

            $mockContext.Proxy.MockMethod("Get", {
                param ([string] $key)

                return $filePath;
            });

            Mock -ModuleName "DeletePersonsFile" Test-Path {
                param (
                    $Path
                )

                return $false;
            }

            Mock -ModuleName "DeletePersonsFile" Remove-Item {
                param (
                    $Path
                )
            }

            # Act
            $sut.Delete();

            # Assert
            $mockContext.Proxy.CountCalls("Get") | Should -Be 1;
            $mockContext.Proxy.CountCalls("Get", (doing args -key "PersonsFilePath")) | Should -Be 1;
            
            Should -Invoke -CommandName Test-Path -ModuleName "DeletePersonsFile" -Times 1 -ParameterFilter { $Path -eq $filePath };
            Should -Invoke -CommandName Remove-Item -ModuleName "DeletePersonsFile" -Times 0;
        }

        It 'File exists, and is deleted' {
            # Arrange
            [string] $filePath = "The File Path";

            [DeletePersonsFile] $sut = [DeletePersonsFile]::new($script:mockContext.Instance);

            $mockContext.Proxy.MockMethod("Get", {
                param ([string] $key)

                return $filePath;
            });

            Mock -ModuleName "DeletePersonsFile" Test-Path {
                param (
                    $Path
                )

                return $true;
            }

            Mock -ModuleName "DeletePersonsFile" Remove-Item {
                param (
                    $Path
                )
            }

            # Act
            $sut.Delete();

            # Assert
            $mockContext.Proxy.CountCalls("Get") | Should -Be 1;
            $mockContext.Proxy.CountCalls("Get", (doing args -key "PersonsFilePath")) | Should -Be 1;
            
            Should -Invoke -CommandName Test-Path -ModuleName "DeletePersonsFile" -Times 1 -ParameterFilter { $Path -eq $filePath };
            Should -Invoke -CommandName Remove-Item -ModuleName "DeletePersonsFile" -Times 1 -ParameterFilter { $Path -eq $filePath };
        }
    }
}
