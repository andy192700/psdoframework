using namespace DoFramework.Processing;
using namespace System.Collections.Generic;
using module ".\Models\Person.psm1";

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