using namespace DoFramework.Processing;
using namespace System.Collections.Generic;
using module ".\Sample\SampleProject\Modules\ReadPersonsFile.psm1";
using module ".\Sample\SampleProject\Modules\Models\Person.psm1";
#Sample do file, includes calls to existing sample project located in \Sample
#See the documentation here: Documentation\Dofiles.md

$myVar = "hello world!!!";
$theBool = $false;
$homeDir = Get-Location;
$processName = "SimpleProcess";
$composerName = "AdvancedComposer";
$testFilter = ".*";

Target A {
    if ($theBool) {
        Write-Host boolset;
    }

    Write-Host $myVar;
}

Target B -inherits A {
    Write-Host "the end";
}

Target SampleProcess {
    doing run -name $processName -showReports -home "$homeDir/Sample";
}

Target SampleComposer {
    doing compose -name $composerName -showReports -home "$homeDir/Sample";
}

Target SampleTests {
    doing test -filter $testFilter -home "$homeDir/Sample";
}

Target SampleMock {
    class SomeOtherClass {
        [string] Echo() {
            return [string]::Empty;
        }
    }
    
    [DoFramework.Testing.ProxyResult] $mock = doing mock -type ([SomeOtherClass]);

    $mock.Proxy.MockMethod("Echo", {
        return "hello world 12345";
    });

    Write-Host "Mocked instance echos: $($mock.Instance.Echo())";
}

Target SampleMockViaDotSource {
    . .\dofilesampleclass.ps1;
    
    [DoFramework.Testing.ProxyResult] $mock = doing mock -type ([SampleClass]);

    $mock.Proxy.MockMethod("SayHello", {
        return "hello world";
    });

    Write-Host "Mocked instance says hello: $($mock.Instance.SayHello())";
}

Target SampleMockViaModuleReference {
    [DoFramework.Testing.ProxyResult] $mockContext = doing mock -type ([IContext]);

    [DoFramework.Testing.ProxyResult] $mock = doing mock -type ([ReadPersonsFile]) -params @($mockContext.Instance);

    $mock.Proxy.MockMethod("Read", {
        [List[Person]] $people = [List[Person]]::new();

        [Person] $person = [Person]::new();
        $person.FirstName = "Ronald";
        $person.LastName = "Schmitt";
        $person.Age = 57;

        $people.Add($person);

        return $people;
    });

    [List[Person]] $mockedPeople = $mock.Instance.Read();

    Write-Host "Mocked people count: $($mockedPeople.Count)";

    foreach ($person in $mockedPeople) {
        Write-Host "String representation of the mocked person: $($person.ToString())";
    }

    Write-Host "Persons mocked $($mock.Proxy.CountCalls("Read")) times.";
}