# Testing
The DoFramework provides the facility to write tests for [Modules](./Modules.md), [Processes](./Processes.md) and [Composers](./Composers.md) created within a project.

Test files are .ps1 files that live with the "Tests" directory of a project, see the overview of Do's [project structure](./ProjectStructure.md).

DoFramework tests are built off the back of the [Pester](https://pester.dev/docs/quick-start) module (included with the DoFramework) which allows the mocking of PowerShell functions, in addition this framework offers PowerShell class mocking/dotnet interface mocking.

It is also worth pointing out that the execution of a Process using Do is performed via the CLI (a PowerShell function) and as a further consequence full end to end tests can be written (if desired) for Processes.

Full examples are included in the [sample project](../Sample/).

Adding tests can be done 3 ways - see the new-process, new-module, new-composer and add-test functions [here](./CLIFunctions.md).

To run tests see the [test](./CLIFunctions.md#test) function.

## Writing Process Tests
Once an associated test file is created for a Process, e.g. if a process named "DoSomething.ps1" exists in the root of the Processes directory and a Test named "DoSomethingTests.ps1" exists in the root of the "Tests/Processes" directory, then the test file "DoSomethingTests" will be able to use the type "DoSomething" without any intervention from a developer. This is because Processes are dot-sourced just before their associated tests run.

To reference modules/namespaces required by tests, `using` statements are required so the test file has access to the required types/functions.

## Writing Composer Tests
Composer tests work in exactly the same way as Process tests do - where the `IComposer` implementation class is dot-sourced before the tests are ran thus giving the tests access to the type.

## Writing Modules Tests
Differing slightly from Process/Composer Tests, a developer is required to import their module using a `using` statement at the top of the test file.

Then just like Process tests, enrich the test file with `using` statements as required.

## Mocking Functions
Mocking a PowerShell function using Pester is done by calling the function `Mock` and writing a script block to create the desired behaviour, like so:

```PowerShell
Mock MyFunction {
   param (
        <Params>
    )

    # Implement Mock Behaviour
}
```

If the function being mocked is created or called from inside a DoFramework module, then the module name is required otherwise Pester will miss it. For example:

```PowerShell
Mock -ModuleName "MyModule" MyFunction {
    param (
        $param1,
        $param2
    )

    # Implement Mock Behaviour
}
```

To verify the functon has been called a certain number of times use the following:

```PowerShell
Should -Invoke -CommandName MyFunction -Times 1;
```

To verify the function was called with specific parameters add a `ParameterFilter` like so:

```PowerShell
Should -Invoke -CommandName MyFunction -Times 1 -ParameterFilter { $param1 -eq "Value1" -and $param2 -eq "Value2" };
```

Again, if the function is created or called from inside a Module, supplement the above with the `ParameterFilter`:

```PowerShell
Should -Invoke -CommandName MyFunction -ModuleName "MyModule" -Times 1 -ParameterFilter { $param1 -eq "Value1" -and $param2 -eq "Value2" };
```

## Mocking PowerShell classes
As previously mentioned, Do provides additional functionality to create a mock instances of PowerShell classes, these are derivatives of the type being mocked.

This section outlines how to use this functionality, feel free to read through the sample project or [automated tests](../src/DoCli/Tests/Component/Util/CreateProxyTests.ps1) for many working examples.

Consider the below classes:

```PowerShell
class OtherClass {}

class OtherClass2 {}

class MyClass {
    [OtherClass] $OtherClass;
    [OtherClass2] $OtherClass2;

    MyClass([OtherClass] $otherClass, [OtherClass2] $otherClass2) {
        $this.OtherClass = $otherClass;
        $this.OtherClass2 = $otherClass2;
    }

    [void] MyMethodToMock1([string] $someInput) {

    }

    [int] MyMethodToMock2() {
        return 3;
    }
}
```

Use the [mock](./CLIFunctions.md#mock) function to create [ProxyResult](../src/DoFramework/DoFramework/Testing/Mocking/ProxyResult.cs) objects which house:
- An `Instance` property - this is the mocked object which derives from the input type
- A `Proxy` property - this allows methods to be mocked and can verify calls.

The `params` argument is only required when there are constructor parameters.

```PowerShell
[DoFramework.Testing.ProxyResult] $otherClassProxy = doing mock -type ([OtherClass]);
[DoFramework.Testing.ProxyResult] $otherClass2Proxy = doing mock -type ([OtherClass2]);
[DoFramework.Testing.ProxyResult] $myClassProxy = doing mock -type ([MyClass]) -params @($otherClassProxy.Instance, $otherClass2Proxy.Instance);
```

Create mock behaviour using the `Proxy` property, Mocking is done by passing a script block.

Note that the type and names of parameters MUST match those defined on the actual method.

Note - mocking methods is optional, defaults for method return types will return if the method has not been mocked.

```PowerShell
$myClassProxy.Proxy.MockMethod("MyMethodToMock1", {
    param ([string] $someInput)
    # Implement Mock Behaviour
});

$myClassProxy.Proxy.MockMethod("MyMethodToMock2", {
    # Implement Mock Behaviour
    return 5;
});
```

The methods are called at some point during the test via the `Instance` property:

```PowerShell
$myClassProxy.Instance.MyMethodToMock1("some string");
$myClassProxy.Instance.MyMethodToMock2();
```

Finally, count the calls to the methods, this can be done for all calls for a particular function or to verify particular calls to the function be passing in a parameter dictionary.

See the [source code](../src/DoFramework/DoFramework/Testing/Mocking/IProxy.cs) for the `Proxy` property for more details.

This is done below using the [args](./CLIFunctions.md#args) function, which creates a `[Dictionary[string, object]]` for convenience.

```PowerShell
$result.Proxy.CountCalls("MyMethodToMock1") | Should -Be 1;
$result.Proxy.CountCalls("MyMethodToMock1", (doing args -someInput "some string")) | Should -Be 1;
$result.Proxy.CountCalls("MyMethodToMock2") | Should -Be 1;
```

If required, call the `Reset` method on the `Proxy` property to clear method calls and mock behaviour.

```PowerShell
$result.Proxy.Reset();
```

## Mocking Dotnet interfaces
It is also possible to mock Dotnet interfaces using the same method as is used for PowerShell classes. With the one drawback that generic methods cannot be mocked (this is not supported) as PowerShell itself does not support this. Generic classes however are supported.

Consder the following Dotnet interface.

```c#
interface MyInterface {
    string MyString { get; set; }
    void MyMethod();
}
```

Similar to a class use the `mock` function to create a mock instance and proxy:
```PowerShell
[DoFramework.Testing.ProxyResult] $myInterfaceProxy = doing mock -type ([MyInterface]);
```

Mocking methods works analagously to PowerShell classes.

In order to mock interface/class properties the `IProxy` interface provides a `MockProperty` method, called like so:

```PowerShell
$myInterfaceProxy.Proxy.MockProperty("MyString", {
    # Implement Mock Behaviour

    return "sample value";
});

$myInterfaceProxy.Proxy.MockMethod("MyMethod", {
    # Implement Mock Behaviour
});
```

To count property calls use the method `CountPropertyCalls`:

```PowerShell
$myInterfaceProxy.Proxy.CountPropertyCalls("MyString") | Should -Be 1;
$myInterfaceProxy.Proxy.CountCalls("MyMethod") | Should -Be 1;
```