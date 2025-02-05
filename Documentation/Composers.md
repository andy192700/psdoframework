# Composers
Composers represent advanced functionality in the DoFramework, they are .ps1 files containing a single class which must implement the [DoFramework.Processing.IComposer](../src/DoFramework/DoFramework/Processing/IComposer.cs) interface.

See [here](./ProjectStructure.md) for more detail on where Composers fit into a project.

See the [add-Composer](./CLIFunctions.md#add-composer) function to learn about adding Composers to your project.

To learn more about the `compose` function, which runs `Composers`, see [here](./CLIFunctions.md#compose).

## Composer Features
Composers are runnable entities that are designed to behave like a program.cs file does in dotnet, offering the following features:
- Dependency Injection
- Configuring Objects
- Registering Processes to execute after the Composer runs

A Composer must have an empty constructor and have a singular method `Compose` expecting a [DoFramework.Processing.IWorkBench](../src/DoFramework/DoFramework/Processing/ComposerWorkBench/IComposerWorkBench.cs) which allows a developer to utilies the features mentioned here.

The `IComposerWorkBench` provides some syntactic advantages for developers using an [IRepeater](../src/DoFramework/DoFramework/Processing/ComposerWorkBench/IRepeater.cs) pattern to mimic dotnet extension methods, follow the exanples listed within the following subsections to understand more.

### Dependency Injection
Registering classes for injection into Processes can only be done using a Composer. Consider the below example where `MyClass`, `MyClass2` and `MyClass3` are injected via a Composer.

```PowerShell
class MyComposer : IComposer {
    [void] Compose([IComposerWorkBench] $workBench) {
        
        $workBench.RegisterService([MyClass]).
            And([MyClass2]).
            And([MyClass3]);

        $workBench.RegisterProcess("MyProcess");
    }
}
```

See the [Dependency Injection](./DependencyInjection.md) documentation for more information relating to injecting services.

### Configuring Objects
It is possible to register a class for injection and populate primative properties on that class ready for usage in a Process later on. Consider the following code where the following data existing in a .env file

```PowerShell
class MyClass1 {
    [int] $MyInt;
    [string] $MyString;
}

class MyClass2 {
    [int] $MyInt2;
    [string] $MyString2;
}
```

```
MyClass1.MyInt=24
MyClass1.MyString=abc
MyClass2.MyInt2=23
MyClass2.MyString2=abcd
```

Or is supplied via the CLI like:

```
doing compose -name MyComposer -"MyClass1.MyInt" 24 -"MyClass1.MyString" 24 -"MyClass2.MyInt2" 24 -"MyClass2.MyString2" 24
```

Either method will ensure that the `IContext` has the key value pairs specified and the following example will ensure these services are registered and their properties set before injection.

```PowerShell
class MyComposer : IComposer {
    [void] Compose([IComposerWorkBench] $workBench) {

        $workBench.Configure([MyClass1]).
            And([MyClass2]);

        $workBench.RegisterProcess("MyProcess");
    }
}
```

### Registering Processes
A Composer is required to register a minimum of one Process, which is done using the `RegisterProcess` method on the `IComposerWorkBench` object, like configuring/registering classes for DI this method returns an `IRepeater` which can be used to easily register Processes for invocation. Consider the following example which runs the Processes `MyProcess1`, `MyProcess2` and `MyProcess3`.

```PowerShell
class MyComposer : IComposer {
    [void] Compose([IComposerWorkBench] $workBench) {

        $workBench.RegisterProcess("MyProcess1").
            And("MyProcess2").
            And("MyProcess3").
    }
}
```

Please note that this is the only way to run multiple Processes - it must be done via a Composer, running isolated Processes on their own is possible however, see the [run](./CLIFunctions.md#run) function to learn more.