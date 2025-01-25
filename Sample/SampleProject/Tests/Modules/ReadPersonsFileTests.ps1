using namespace DoFramework.Processing;
using namespace DoFramework.Testing;
using namespace System.Collections.Generic;
using module "..\..\Modules\ReadPersonsFile.psm1";
using module "..\..\Modules\Models\Person.psm1";

Describe 'ReadPersonsFileTests' {
    BeforeEach {
        [ProxyResult] $script:mockContext = doing create-proxy -type ([IContext]);
    }

    Context 'ReadPersonsFileTests' {
        It 'No data acquired, observations are correct' {
            # Arrange
            [string] $filePath = "The File Path";

            [ReadPersonsFile] $sut = [ReadPersonsFile]::new($script:mockContext.Instance);

            $mockContext.Proxy.MockMethod("Get", {
                param ([string] $key)

                return $filePath;
            });

            Mock -ModuleName "ReadPersonsFile" ConvertFrom-Json {
                return $null;
            };

            Mock -ModuleName "ReadPersonsFile" Get-Content {
                param (
                    $Path
                )

                return "[]";
            }

            # Act
            [List[Person]] $result = $sut.Read();

            # Assert
            $result.Count | Should -Be 0;

            $mockContext.Proxy.CountCalls("Get") | Should -Be 1;
            $mockContext.Proxy.CountCalls("Get", (doing Read-Args -key "PersonsFilePath")) | Should -Be 1;
            
            Should -Invoke -CommandName Get-Content -ModuleName "ReadPersonsFile" -Times 1 -ParameterFilter { $Path -eq $filePath };
            
            Should -Invoke -CommandName ConvertFrom-Json -ModuleName "ReadPersonsFile" -Times 1 -ParameterFilter { $InputObject -eq "[]" };
        }

        It 'Data acquired, observations are correct' {
            # Arrange
            [string] $filePath = "The File Path";

            [ReadPersonsFile] $sut = [ReadPersonsFile]::new($script:mockContext.Instance);

            $mockContext.Proxy.MockMethod("Get", {
                param ([string] $key)

                return $filePath;
            });

            Mock -ModuleName "ReadPersonsFile" ConvertFrom-Json {
                return @(
                    [PSCustomObject]@{
                        FirstName = "Ralph"
                        LastName  = "Wiggum"
                        Age       = 23
                    },
                    [PSCustomObject]@{
                        FirstName = "Moe"
                        LastName  = "Szyslak"
                        Age       = 35
                    },
                    [PSCustomObject]@{
                        FirstName = "Sideshow"
                        LastName  = "Bob"
                        Age       = 58
                    }
                );
            };

            [string] $fileContent = @"
[
  {
    "FirstName": "Ralph",
    "LastName": "Wiggum",
    "Age": 23
  },
  {
    "FirstName": "Moe",
    "LastName": "Szyslak",
    "Age": 35
  },
  {
    "FirstName": "Sideshow",
    "LastName": "Bob",
    "Age": 58
  }
]
"@;

            Mock -ModuleName "ReadPersonsFile" Get-Content {
                param (
                    $Path
                )

                return $fileContent; 
            }

            # Act
            [List[Person]] $result = $sut.Read();

            # Assert
            $result.Count | Should -Be 3;

            $result[0].FirstName | Should -Be "Ralph";
            $result[0].LastName | Should -Be "Wiggum";
            $result[0].Age | Should -Be 23;
            $result[1].FirstName | Should -Be "Moe";
            $result[1].LastName | Should -Be "Szyslak";
            $result[1].Age | Should -Be 35;
            $result[2].FirstName | Should -Be "Sideshow";
            $result[2].LastName | Should -Be "Bob";
            $result[2].Age | Should -Be 58;

            $mockContext.Proxy.CountCalls("Get") | Should -Be 1;
            $mockContext.Proxy.CountCalls("Get", (doing Read-Args -key "PersonsFilePath")) | Should -Be 1;
            
            Should -Invoke -CommandName Get-Content -ModuleName "ReadPersonsFile" -Times 1 -ParameterFilter { $Path -eq $filePath };

            Should -Invoke -CommandName ConvertFrom-Json -ModuleName "ReadPersonsFile" -Times 1 -ParameterFilter { $InputObject -eq $fileContent };
        }
    }
}
