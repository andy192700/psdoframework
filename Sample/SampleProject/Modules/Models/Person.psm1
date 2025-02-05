class Person {
    [string] $FirstName;
    [string] $LastName;
    [int] $Age;

    [string] ToString() {
        return "FullName: $($this.FirstName) $($this.LastName) Age: $($this.Age)";
    }
}