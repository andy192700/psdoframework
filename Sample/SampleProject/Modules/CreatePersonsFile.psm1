using namespace DoFramework.Processing;
using namespace System.Collections.Generic;
using module ".\Models\Person.psm1";

# Class that uses data from the IContext to write a collection of Persons to a file
class CreatePersonsFile {
    [IContext] $Context;

    CreatePersonsFile([IContext] $context) {
        $this.Context = $context;
    }
    
    [void] Create() {
        [List[Person]] $persons = $this.Context.Get[List[Person]]("persons");

        [string] $json = ConvertTo-Json -InputObject $persons;

        Out-File -FilePath $this.Context.Get("PersonsFilePath") -InputObject $json;
    }
}