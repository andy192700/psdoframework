# CLI Functions
All of the DoFramework's features are accessed via the PowerShell function `doing` which is delivered with the module.

The syntax, available functions and parameter options are detailed on this page.

# Usage
The current directory for the current PowerShell instance is where the framework expects to be operating from, most functions require to be in a directory where it can locate the project file.

To allow more flexibility the `projectPath` may be supplied such that calling functions to operate on a project can be ran from anywhere. The `projectPath` parameter is a universally available to all functions, see the [Universal Parameters](#Universal-Parameters) for more details.

One nuance to be aware of is when making code changes to existing Processes, Modules and Tests that the framework may not always pick up changes to method signatures etc. It is considered best practice to reload the PowerShell terminal before re-running, one way to work around this is to use make, see the make target `sampleproject` in the base of this repository.

# Syntax
All doing function expects a minimum of one argument - the target function name, example below.

```PowerShell
doing FUNCTIONNAME
```

Note - function names are not case sensitive, but parameters names are case sensitive.

However, there are functions with optional or required parameters, the following example supplies a value of `1` for the argument `arg1`.

```PowerShell
doing FUNCTIONNAME -arg1 1
```

Multiple parameters can be supplied, which can also take the form of PowerShell switches, the last example (below) shows 2 standard parameters followed by two switches.

```PowerShell
doing FUNCTIONNAME -arg1 1 -arg2 2 -switch1 -switch2
```

The order in which parameters is supplied does not matter with exception to the desired function name which must always be first.

# Available Functions
This section contains all of the available functions, their purpose, parameter information and examples.

## Project Management Functions
Documented within the section are the functions responsible for creating and managing projects.

### Create-Project
Creates a new project if it does not exist already, see the [Project Structure](./ProjectStructure.md) documentation to understand more about a project's layout and components.

Parameters:
| Parameter Name  | Required | Desription | Type | Default Value |
|----------|----------|----------|----------|----------|
| name | No | The project's name, this is the name of the directory housing the project code. | string | Do |

Example calls via PowerShell:
```PowerShell
doing create-project

doing create-project -name MyProject
```

### Add-Process
Adds a new Process to a project, see the [Processes](./Processes.md) documentation to learn more.

If desired, this function can create the associated test file for the Process, in this case the framework will also call the [add-test](#add-test) function.

Parameters:
| Parameter Name  | Required | Desription | Type | Default Value |
|----------|----------|----------|----------|----------|
| name | Yes | The Process's name, this should include the full path to the Process file from the project's Process directory. Note that the name of the Process is the final part of the supplied string. If a Process with the same name already exists, the framework will alert the caller and take no action. | string | N/A |
| addTests | No | Specifies if the framework should create the associated test file for the Process. | switch/boolean | false |

Example calls via PowerShell:
```PowerShell
doing add-process -name MyProcess

doing add-process -name "My/Nested/ProcessFile"

doing add-process -name "My/Nested/ProcessFile" -addTests
```

### Remove-Process
Removes a Process from a project, deleting the file, if it exists.

If there is a test file associated with the specified Process, the framework will call the [remove-test](#remove-test) function.

Parameters:
| Parameter Name  | Required | Desription | Type | Default Value |
|----------|----------|----------|----------|----------|
| name | Yes | The Process's name, this should not include the full path like the associated `add-process` function. | string | N/A |

Example calls via PowerShell:
```PowerShell
doing remove-process -name MyProcess
```

### Add-Module
Adds a new Module to a project, see the [Modules](./Modules.md) documentation to learn more.

If desired, this function can create the associated test file for the Module, in this case the framework will also call the [add-test](#add-test) function.

Parameters:
| Parameter Name  | Required | Desription | Type | Default Value |
|----------|----------|----------|----------|----------|
| name | Yes | The Module's name, this should include the full path to the Module file from the project's Module directory. Note that the name of the Module is the final part of the supplied string. If a Module with the same name already exists, the framework will alert the caller and take no action. | string | N/A |
| addTests | No | Specifies if the framework should create the associated test file for the Module. | switch/boolean | false |

Example calls via PowerShell:
```PowerShell
doing add-module -name MyModule

doing add-module -name "My/Nested/ModuleFile"

doing add-module -name "My/Nested/ModuleFile" -addTests
```

### Remove-Module
Removes a Module from a project, deleting the file, if it exists.

If there is a test file associated with the specified Module, the framework will call the [remove-test](#remove-test) function.

Parameters:
| Parameter Name  | Required | Desription | Type | Default Value |
|----------|----------|----------|----------|----------|
| name | Yes | The Module's name, this should not include the full path like the associated `add-module` function. | string | N/A |

Example calls via PowerShell:
```PowerShell
doing remove-module -name MyModule
```

### Add-Test
Adds a new Test to a project, see the [Testing](./Testing.md) documentation to learn more.

Called by the [add-process](#add-process) or [add-module](#add-module) functions if the `addTests` parameter is supplied.

This can also be called retrospectively to supplement an existing Module/Process if the Test file was not created at their time of creation.

A test MUST be designated for either a Module or Process, despite the below parameters `forProcess` and `forModule` indicated below being flagged as optional.

Parameters:
| Parameter Name  | Required | Desription | Type | Default Value |
|----------|----------|----------|----------|----------|
| name | Yes | The Test's name, this should include the full path to the Test file from the project's associated Process or Module directory. Note that the name of the Test is the final part of the supplied string. If a Test with the same name already exists, the framework will alert the caller and take no action. The Test's name should be suffixed "Tests". | string | N/A |
| forProcess | No | Informs the framework that the test is intended for a Process. | switch/boolean | false |
| forModule | No | Informs the framework that the test is intended for a Module. | switch/boolean | false |

Example calls via PowerShell:
```PowerShell
doing add-test -name MyProcessTests -forProcess

doing add-test -name MyModuleTests -forModule

doing add-test -name "My/Nested/ProcessFileTests" -forProcess

doing add-test -name "My/Nested/ModuleFileTests" -forModule
```

### Remove-Test
Removes a Test from a project, deleting the file, if it exists.

Parameters:
| Parameter Name  | Required | Desription | Type | Default Value |
|----------|----------|----------|----------|----------|
| name | Yes | The Test's name, this should not include the full path like the associated `add-module` function. | string | N/A |

Example calls via PowerShell:
```PowerShell
doing remove-test -name MyProcessTests

doing remove-test -name MyModuleTests
```

## Invocation Functions

This section details how to utilise a projects low hanging fruit: running processes and tests.

### Run-Process
Runs a specified Process, see the [Process](./Processes.md) for more detail.

Parameters:
| Parameter Name  | Required | Desription | Type | Default Value |
|----------|----------|----------|----------|----------|
| name | Yes | The Process's name, this should not include the full path like the associated `add-process` function. | string | N/A |
| doOutput | No | Returns the [DoFramework.Processing.Session](../src/DoFramework/DoFramework/Processing/Context/ISession.cs) object associated with the run, this object provides insight into Processes that have ran, the outcome as well as the associated [DoFramework.Processing.IContext](../src/DoFramework/DoFramework/Processing/Context/IContext.cs). | switch/boolean | false |
| showReports | No | Presents a view of the Processes executed, the output ([DoFramework.Domain.ProcessResult](../src/DoFramework/DoFramework/Domain/ProcessResult.cs)) and information relating to execution time, all in tabular form. | switch/boolean | false |
| extra parameters | No | Optional collection of additional values or switches, these must also follow the syntax called out in the [Syntax](#syntax) section. The intent of these parameters is to load extra data for Process consumption at runtime, see the section in the [Process Context](./ProcessContext.md#cli-based-context-population) documentation to learn more. | Any | N/A |

Example calls via PowerShell:
```PowerShell
doing run-process -name "MyProcess"

doing run-process -name "MyProcess" -doOutput

doing run-process -name "MyProcess" -doOutput -showReports

doing run-process -name "MyProcess" -doOutput -showReports -key1 Val1 -AdditionalSwitch
```

### Run-Tests
Runs Tests specified by a filter, see the [Testing](./Testing.md) documentation to discover more.

Parameters:
| Parameter Name  | Required | Desription | Type | Default Value |
|----------|----------|----------|----------|----------|
| filter | Yes | Filters the Tests on their name, use `.*` to run all tests or a specific substring to target individuals | string | N/A |
| forProcesses | No | Switch to instruct the framework to only execute Process Tests. | switch/boolean | false |
| forModules | No | Switch to instruct the framework to only execute Module Tests. | switch/boolean | false |
| outputFormat | No | Indicates to the framework that it should output the Pester test results to the base directory of the current project. Values must parse exactly to one of the [DoFramework.Testing.PesterOutputType](../src/DoFramework/DoFramework/Testing/PesterOutputType.cs) values which dictates the test output type. | string | [PesterOutputType]::None |

Example calls via PowerShell:
```PowerShell
doing run-tests -filter .*

doing run-tests -filter MyProcess

doing run-tests -filter MyProcessTests

doing run-tests -filter MyModule

doing run-tests -filter MyModuleTests

doing run-tests -filter .* -forProcesses

doing run-tests -filter .* -forModules

doing run-tests -filter .* -outputFormat None

doing run-tests -filter .* -outputFormat NUnitXml

doing run-tests -filter .* -outputFormat JUnitXml
```

## Utility Functions
Utility functions are called from within code files within a project structure, the framework provides these to enrich developer experience where necessary.

### Create-Proxy
Creates a proxy for a given type, allowing mocking of PowerShell classes and Dotnet interfaces. To learn more see how to mock [PowerShell classes](./Testing.md#mocking-powershell-classes) and [Dotnet Interfaces](./Testing.md#mocking-dotnet-interfaces).

Example calls via PowerShell:
Parameters:
| Parameter Name  | Required | Desription | Type | Default Value |
|----------|----------|----------|----------|----------|
| type | Yes | The type of the class/interface to be mocked. | System.Type | N/A |
| params | No | Required if the requested constructor of the type to be mocked has arguments. | System.object[] | @() |

```PowerShell
# Consider the following PowerShell classes which we wish to mock.
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

# Now we Mock the classes
[DoFramework.Testing.ProxyResult] $otherClassProxy = doing create-proxy -type ([OtherClass]);
[DoFramework.Testing.ProxyResult] $otherClass2Proxy = doing create-proxy -type ([OtherClass2]);
[DoFramework.Testing.ProxyResult] $myClassProxy = doing create-proxy -type ([MyClass]) -params @($otherClassProxy.Instance, $otherClass2Proxy.Instance);
```

### Get-MethodInfo
**Used by the create-proxy function, not intended for developer usage.**

Invoked by a Proxy object's method when mock behaviour is introduced, returning a `System.Reflection.MemberInfo` instance detailing the mocked method.

Parameters:
| Parameter Name  | Required | Desription | Type | Default Value |
|----------|----------|----------|----------|----------|
| type | Yes | The type of the parent class/interface. | System.Type | N/A |
| methodName | Yes | The name of the mocked method. | string | N/A |
| parameters | No | The constructor argumens, supplied as an object array. | System.object[] | @() |

Example calls via PowerShell:
```PowerShell
[System.reflection.MethodInfo] $methodInfo = doing get-methodinfo -methodName SomeMethod -type ([MyClass]) -parameters @("some argument", "some other argument")
```

### Read-Args
The read-args method is provided as a convenience method returning a `Dictionary[string, object]`, it is generally used when verifying mock method calls.

Parameters:
| Parameter Name  | Required | Desription | Type | Default Value |
|----------|----------|----------|----------|----------|
| extra parameters | No | Optional collection of additional values or switches, these must also follow the syntax called out in the [Syntax](#syntax) section. Each key and value supplied in the form of standard parameters becomes an entry in the dictionary. | Any | N/A |

Example calls via PowerShell:
```PowerShell
# An empty Dictionary
[System.Collections.Generic.Dictionary[string, object]] $dictionary = doing read-args;

# A Dictionary with 1 entry
[System.Collections.Generic.Dictionary[string, object]] $dictionary = doing read-args -key1 val1;

# A Dictionary with 2 entries.
[System.Collections.Generic.Dictionary[string, object]] $dictionary = doing read-args -key1 val1 -key2 val2;
```

# Universal Parameters

The DoFramework offers optional parameters for universal use across **all** functions, these are documented here.

Universal Parameters:
| Parameter Name  | Desription | Type | Default Value |
|----------|----------|----------|----------|
| silent | A switch which suppresses logging by the framework | switch/boolean | false |
| projectPath | This framework requires the executing shell to be in the directory of a project by default, by supplying this parameter, this does not have to be the case. By supplying the full path of the parent directory of a project, a developer can use the functions specified in this document from anywhere. The make targets `sampleproject` and `sampleprojecttests` in the root of this repository demonstrate its usage. | string | N/A | 

Example calls via PowerShell, demonstrated using the `create-project` function:

```PowerShell
doing create-project -silent

doing create-project -projectPath "C:\\my\path\to\my\project"

doing create-project -projectPath "my/path/to/my/project"
```