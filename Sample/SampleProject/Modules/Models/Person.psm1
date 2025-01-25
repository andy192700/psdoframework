# A model/domain class housing crucial information about a person, and the ability to present that person as a string.
class Person {
    [string] $FirstName;
    [string] $LastName;
    [int] $Age;

    [string] ToString() {
        return "FullName: $($this.FirstName) $($this.LastName) Age: $($this.Age)";
    }
}