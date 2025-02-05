using namespace DoFramework.Processing;
using namespace System.Collections.Generic;
using module ".\Models\Person.psm1";

class CreatePersons {
    [IContext] $Context;

    CreatePersons([IContext] $context) {
        $this.Context = $context;
    }

    [void] Create() {
        [List[Person]] $persons = [List[Person]]::new();

        [Person] $person1 = [Person]::new();
        $person1.FirstName = $this.Context.Get("Person1FirstName");
        $person1.LastName = $this.Context.Get("Person1LastName");
        $person1.Age = $this.Context.Get("Person1Age");
        
        [Person] $person2 = [Person]::new();
        $person2.FirstName = $this.Context.Get("Person2FirstName");
        $person2.LastName = $this.Context.Get("Person2LastName");
        $person2.Age = $this.Context.Get("Person2Age");
        
        [Person] $person3 = [Person]::new();
        $person3.FirstName = $this.Context.Get("Person3FirstName");
        $person3.LastName = $this.Context.Get("Person3LastName");
        $person3.Age = $this.Context.Get("Person3Age");

        $persons.Add($person1);
        $persons.Add($person2);
        $persons.Add($person3);

        $this.Context.AddOrUpdate("persons", $persons);
    }
}
