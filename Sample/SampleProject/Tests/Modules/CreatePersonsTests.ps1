using namespace DoFramework.Processing;
using namespace DoFramework.Testing;
using namespace System.Collections.Generic;
using module "..\..\Modules\CreatePersons.psm1";
using module "..\..\Modules\Models\Person.psm1";

Describe 'CreatePersonsTests' {
    BeforeEach {
        [ProxyResult] $script:mockContext = doing mock -type ([IContext]);
    }

    Context 'CreatePersonsTests' {
        It 'Creates data and mocks observe correct inputs' {
            # Arrange
            [CreatePersons] $sut = [CreatePersons]::new($script:mockContext.Instance);

            $script:mockContext.Proxy.MockMethod("Get", {
                param ([string] $key)

                if ($key.Contains("Age")) {
                    return [int]$key.Replace("Person", [string]::Empty).Replace("Age", [string]::Empty);
                }

                return $key;
            });

            [List[Person]] $script:persons = $null;

            $script:mockContext.Proxy.MockMethod("AddOrUpdate", {
                param (
                    [string] $key,
                    [object] $value
                )
                $script:persons = $value;
            });

            # Act
            $sut.Create();

            # Assert
            $script:mockContext.Proxy.CountCalls("AddOrUpdate") | Should -Be 1;       
            $script:mockContext.Proxy.CountCalls("AddOrUpdate", (doing args -key "persons" -value $script:persons)) | Should -Be 1;    

            $script:mockContext.Proxy.CountCalls("Get") | Should -Be 9; 
            $script:mockContext.Proxy.CountCalls("Get", (doing args -key "Person1FirstName")) | Should -Be 1;
            $script:mockContext.Proxy.CountCalls("Get", (doing args -key "Person1LastName")) | Should -Be 1;
            $script:mockContext.Proxy.CountCalls("Get", (doing args -key "Person1Age")) | Should -Be 1;
            $script:mockContext.Proxy.CountCalls("Get", (doing args -key "Person2FirstName")) | Should -Be 1;
            $script:mockContext.Proxy.CountCalls("Get", (doing args -key "Person2LastName")) | Should -Be 1;
            $script:mockContext.Proxy.CountCalls("Get", (doing args -key "Person2Age")) | Should -Be 1;
            $script:mockContext.Proxy.CountCalls("Get", (doing args -key "Person3FirstName")) | Should -Be 1;
            $script:mockContext.Proxy.CountCalls("Get", (doing args -key "Person3LastName")) | Should -Be 1;
            $script:mockContext.Proxy.CountCalls("Get", (doing args -key "Person3Age")) | Should -Be 1;

            $script:persons.Count | Should -Be 3;
            $script:persons[0].FirstName | Should -Be "Person1FirstName";
            $script:persons[0].LastName | Should -Be "Person1LastName";
            $script:persons[0].Age | Should -Be 1;
            $script:persons[1].FirstName | Should -Be "Person2FirstName";
            $script:persons[1].LastName | Should -Be "Person2LastName";
            $script:persons[1].Age | Should -Be 2;
            $script:persons[2].FirstName | Should -Be "Person3FirstName";
            $script:persons[2].LastName | Should -Be "Person3LastName";
            $script:persons[2].Age | Should -Be 3;
        }
    }
}
