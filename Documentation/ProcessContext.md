# Process Context
When running Processes using this framework, a `Context` object is available for storing information which is available globally. There are three mechanisms for adding data to the context:
- Variables stored in .env* files.
- Variables supplied via the command line (these override variables stored in the .env* files, if they exist).
- Manually appending values to the `Context` object, achieved via Dependency Injection

There is a wealth of examples in the [sample project](../Sample/SampleProject).

# Injecting an IContext instance into a Process
To read/write variables using a `Context`, the first thing that must be done is to inject a `DoFramework.Processing.IContext` instance into a Process, which can be done like so:

```PowerShell
using namespace DoFramework.Processing;

class MyExampleClass : Process {
    [IContext] $Context;

    MyExampleClass([IContext] $context) {
        $this.Context = $context;
    }    
```

Once injected the instance field `$this.Context` will be set and can be used during the `Validate` and `Run` methods, for example:

```PowerShell
using namespace DoFramework.Processing;

class MyExampleClass : Process {
    [IContext] $Context;

    MyExampleClass([IContext] $context) {
        $this.Context = $context;
    }    

    [bool] Validate() {
        return $this.Context.KeyExists("SomeKey");
    }

    [void] Run() {
        Write-Host "SomeVar = $($this.Context.Get("SomeVar"))" 
    }
}
```

# Context methods
The contract (interface) for the `Context` is defined [here](../src/DoFramework/DoFramework/Processing/Context/IContext.cs), this section details how each call "looks" in PowerShell with a series of snippets.

Each example uses the `Context` like it is a local variable so it appears in the form `$context`.

## Get
Here is the basic `Get` method which returns a variable keyed by a specific string.

```PowerShell
[string] $someVar = $context.Get("SomeVar");
```

If there is an entry in the `Context` with the name `SomeVar` then the variable `$someVar` will be set, otherwise it will be `$null`.

## Generic Get
It is also possible to request a variable from the `Context` using an explicit cast, this is useful for more complex objects. This function works in the same way as it's none-generic sibling - returning null if it does not exist.

Note - this method is intended for none primative objects (not integers, floats and so on).

```PowerShell
[MyComplexType] $someVar = $context.Get[MyComplexType]("whateveritiscalled");

[MyComplexGenericType[int]] $someVar = $context.Get[MyComplexGenericType[int]]("whateveritiscalled");
```

## AddOrUpdate
The `AddOrUpdate` method does exactly what it says on the tin - adds a variable if no other variable with that name exists OR updates a variable with the supplied name that already exists irrespective of it's former type.

This is how a developer/contributor would write variables to the `Context`, as mentioned in the introduction.

```PowerShell
$context.AddOrUpdate("myKey", "myValue");
```

## KeyExists
It is useful to check if a key exists in the context - this may be useful in Process `Validate` method definitions e.g. "don't run if this following variable does not exist.". Similar to the previous function, it does what you would expect - checks the `Context` for a particular variable, `$true` if it exists and `$false` if it does not exist.

```PowerShell
 [bool] $keyExists = $this.Context.KeyExists("SomeKey");
 ```

 ## ParseSwitch
 It is possible to parse boolean values held by the `Context`. If `-theSwitch` is passed whilst running the `run-process` command for example the following could be used to interperet it:

 ```PowerShell
 #Example CLI call
 doing run-process -name SomeProcess -theSwitch;

 #Example showing how to parse a switch from an IContext object
 [bool] $theSwitch = $this.Context.KeyExists("theSwitch");
 ```

 `$theSwitch` will be:
 - false if there is a boolean value in the context with name `theSwitch` and set to false.
 - false if there is NO boolean value in the context with name `theSwitch`.
 - false if there is a none boolean value in the context with name `theSwitch`.
 - true if there is a boolean value in the context with name `theSwitch` and set to true.

 # .env* file based Context population
 Do will automatically digest all files following the `.env*` pattern when `run-process` is called via the CLI, it expects them to have the form `key=value` like so:

 ```
 this=that
 myVar=thatVar
 hello=world
 foo=bar
 ```

 In a Process or Module these values can be accessed with the previously discussed `Get` methods.

 # CLI based Context population
 Variables can be fed into a process via the CLI as well as by .env files.

 The following `run-process` call runs a process called `myProcess` but then appends a variable for consumption by a Process:

 ```PowerShell
 doing run-process -name myProcess -hello world
 ```

 Which could then be accessed like so:

```PowerShell
[string] $helloVar = $context.Get("hello");
```

The string `$helloVar` will have the value `world`.

Additionally, it is worth noting that variable supplied via the CLI will override those stored in .env files, this gives developers opportunity to override default values.

## Parsing Switches
As previously mentioned, it is possible to supply switches as extra parameters when running a Process, this is also done by injecting a `Context` instance into a Process class, the `Context` has a method `ParseSwitch` which will return:
- false if the switch does not exist, is not a valid bool or if a bool is supplied that is false
- true if the switch is supplied or a bool is supplied that is true

A full worked example is below:

```PowerShell
using namespace DoFramework.Processing;

class ExampleUsingSwitch : Process {
    [IContext] $Context;

    ExampleUsingSwitch([IContext] $context) {
        $this.Context = $context;
    }

    [void] Run() {
        Write-Host "Switch = $($this.Context.ParseSwitch("MySwitch"))";
    }
}
```

Below is an example of calling `run-process`, passing a switch:

```PowerShell
doing run-process -name "nameOfProcess" -MySwitch
```

A runnable example exists in the sample project [here](../Sample/SampleProject/Processes/Simple/DoublesANumber.ps1).