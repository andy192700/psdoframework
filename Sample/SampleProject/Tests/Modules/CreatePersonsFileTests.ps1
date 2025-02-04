using namespace DoFramework.Processing;
using namespace DoFramework.Testing;
using namespace System.Collections.Generic;
using module "..\..\Modules\CreatePersonsFile.psm1";
using module "..\..\Modules\Models\Person.psm1";

Describe 'CreatePersonsFileTests' {
    BeforeEach {
        [ProxyResult] $script:mockContext = doing mock -type ([IContext]);
    }

    Context 'Tests' {
        It 'Outputs file with expected JSON' {
            # Arrange
            [List[Person]] $persons = [List[Person]]::new();

            [Person] $person1 = [Person]::new();
            $person1.FirstName = "fname1";
            $person1.LastName = "lname1";
            $person1.Age = 10;
            
            [Person] $person2 = [Person]::new();
            $person2.FirstName = "fname2";
            $person2.LastName = "lname2";
            $person2.Age = 20;

            $persons.Add($person1);
            $persons.Add($person2);

            [string] $personJson = $persons | ConvertTo-Json;
            
            [string] $path = "some_path";

            $mockContext.Proxy.MockMethod("Get", {
                param ([string] $key)

                if ($key -eq "persons") {
                    return $persons;
                }

                return $path;
            });
        
            Mock -ModuleName "CreatePersonsFile" ConvertTo-Json {
                return $personJson;
            };

            Mock -ModuleName "CreatePersonsFile" Out-File { };

            [CreatePersonsFile] $sut = [CreatePersonsFile]::new($mockContext.Instance);

            # Act
            $sut.Create();

            # Assert
            $mockContext.Proxy.CountCalls("Get", (doing args -key "persons")) | Should -Be 1;
            $mockContext.Proxy.CountCalls("Get", (doing args -key "PersonsFilePath")) | Should -Be 1;
            $mockContext.Proxy.CountCalls("Get") | Should -Be 2;

            Should -Invoke -CommandName Out-File -ModuleName "CreatePersonsFile" -Times 1 -ParameterFilter { $FilePath -eq $path -and $InputObject -eq $personJson };
            Should -Invoke -CommandName ConvertTo-Json -ModuleName "CreatePersonsFile" -Times 1 -ParameterFilter { $InputObject[0] -eq $persons[0] -and $InputObject[1] -eq $persons[1] };
        }
    }
}