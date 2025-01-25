using namespace DoFramework.Processing;
using namespace System.Collections.Generic;
using module ".\Models\Person.psm1";

# Class that uses data from the IContext to read a specific JSON file and create a collection of Persons as a result.
class ReadPersonsFile {
    [IContext] $Context;

    ReadPersonsFile([IContext] $context) {
        $this.Context = $context;
    }
    
    [List[Person]] Read() {
        [string] $filePath = $this.Context.Get("PersonsFilePath");

        [string] $data = Get-Content -Path $filePath;

        [PSCustomObject] $jsonContents = ConvertFrom-Json -InputObject $data;

        [List[Person]] $persons = [List[Person]]::new();

        foreach ($item in $jsonContents) { 
            $person = [Person]::new(); 
            $person.FirstName = $item.FirstName; 
            $person.LastName = $item.LastName;
            $person.Age = $item.Age;
            $persons.Add($person);
        }

        return $persons;
    }
}