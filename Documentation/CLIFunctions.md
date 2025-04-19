# CLI Functions
All of the DoFramework's features are accessed via the PowerShell function `doing` which is delivered with the module.

The syntax, available functions and parameter options are detailed on this page.

# Usage
The current directory for the current PowerShell instance is where the framework expects to be operating from, most functions require to be in a directory where it can locate the project file.

To allow more flexibility the `home` may be supplied such that calling functions to operate on a project can be ran from anywhere. The `home` parameter is a universally available to all functions, see the [Universal Parameters](#Universal-Parameters) for more details.

One nuance to be aware of is when making code changes to existing Composers, Processes, Modules and Tests that the framework may not always pick up changes to method signatures etc. It is considered best practice to reload the PowerShell terminal before re-running, one way to work around this is to use make, see the make targets `runsample*` in the base of this repository.

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

# Universal Parameters
The DoFramework offers optional parameters for universal use across **all** functions, these are documented here.

Universal Parameters:
| Parameter Name  | Desription | Type | Default Value |
|----------|----------|----------|----------|
| silent | A switch which suppresses logging by the framework | switch/boolean | false |
| home | This framework requires the executing shell to be in the directory of a project by default, by supplying this parameter, this does not have to be the case. By supplying the full path of the parent directory of a project, a developer can use the functions specified in this document from anywhere. The make targets `runsample*` in the root of this repository demonstrate its usage. | string | N/A | 

Example calls via PowerShell, demonstrated using the `new-project` function:

```PowerShell
doing new-project -silent

doing new-project -home "C:\\my\path\to\my\project"

doing new-project -home "my/path/to/my/project"

doing new-dofile -silent

doing new-dofile -home "C:\\my\path\to\my\project"

doing new-dofile -home "my/path/to/my/project"
```

# Available Functions
This section contains all of the available functions, their purpose, parameter information and examples.

## Project Management Functions
Documented within the section are the functions responsible for creating and managing projects.

### New-Project
Creates a new project if it does not exist already, or at the location specified by the [home parameter](#Universal-Parameters), see the [Project Structure](./ProjectStructure.md) documentation to understand more about a project's layout and components.

Parameters:
| Parameter Name  | Required | Desription | Type | Default Value |
|----------|----------|----------|----------|----------|
| name | No | The project's name, this is the name of the directory housing the project code. | string | Do |

Example calls via PowerShell:
```PowerShell
doing new-project

doing new-project -name MyProject
```

### New-DoFile
Creates a new dofile.ps1 if it does not exist in the current directory, or at the location specified by the [home parameter](#Universal-Parameters).

Example calls via PowerShell:
```PowerShell
doing new-dofile
```

### new-process
Adds a new Process to a project, see the [Processes](./Processes.md) documentation to learn more.

If desired, this function can create the associated test file for the Process, in this case the framework will also call the [new-test](#new-test) function.

Parameters:
| Parameter Name  | Required | Desription | Type | Default Value |
|----------|----------|----------|----------|----------|
| name | Yes | The Process's name, this should include the full path to the Process file from the project's Process directory. Note that the name of the Process is the final part of the supplied string. If a Process with the same name already exists, the framework will alert the caller and take no action. | string | N/A |
| addTests | No | Specifies if the framework should create the associated test file for the Process. | switch/boolean | false |

Example calls via PowerShell:
```PowerShell
doing new-process -name MyProcess

doing new-process -name "My/Nested/ProcessFile"

doing new-process -name "My/Nested/ProcessFile" -addTests
```

### delete-process
Removes a Process from a project, deleting the file, if it exists.

If there is a test file associated with the specified Process, the framework will call the [delete-test](#delete-test) function.

Parameters:
| Parameter Name  | Required | Desription | Type | Default Value |
|----------|----------|----------|----------|----------|
| name | Yes | The Process's name, this should not include the full path like the associated `new-process` function. | string | N/A |

Example calls via PowerShell:
```PowerShell
doing delete-process -name MyProcess
```

### new-module
Adds a new Module to a project, see the [Modules](./Modules.md) documentation to learn more.

If desired, this function can create the associated test file for the Module, in this case the framework will also call the [new-test](#new-test) function.

Parameters:
| Parameter Name  | Required | Desription | Type | Default Value |
|----------|----------|----------|----------|----------|
| name | Yes | The Module's name, this should include the full path to the Module file from the project's Module directory. Note that the name of the Module is the final part of the supplied string. If a Module with the same name already exists, the framework will alert the caller and take no action. | string | N/A |
| addTests | No | Specifies if the framework should create the associated test file for the Module. | switch/boolean | false |

Example calls via PowerShell:
```PowerShell
doing new-module -name MyModule

doing new-module -name "My/Nested/ModuleFile"

doing new-module -name "My/Nested/ModuleFile" -addTests
```

### delete-module
Removes a Module from a project, deleting the file, if it exists.

If there is a test file associated with the specified Module, the framework will call the [delete-test](#delete-test) function.

Parameters:
| Parameter Name  | Required | Desription | Type | Default Value |
|----------|----------|----------|----------|----------|
| name | Yes | The Module's name, this should not include the full path like the associated `new-module` function. | string | N/A |

Example calls via PowerShell:
```PowerShell
doing delete-module -name MyModule
```
### new-composer
Adds a new Composer to a project, see the [Composer](./Composers.md) documentation to learn more.

If desired, this function can create the associated test file for the Composer, in this case the framework will also call the [new-test](#new-test) function.

Parameters:
| Parameter Name  | Required | Desription | Type | Default Value |
|----------|----------|----------|----------|----------|
| name | Yes | The Composer's name, this should include the full path to the Composer file from the project's Composer directory. Note that the name of the Composer is the final part of the supplied string. If a Composer with the same name already exists, the framework will alert the caller and take no action. | string | N/A |
| addTests | No | Specifies if the framework should create the associated test file for the Composer. | switch/boolean | false |

Example calls via PowerShell:
```PowerShell
doing new-composer -name MyComposer

doing new-composer -name "My/Nested/MyComposer"

doing new-composer -name "My/Nested/MyComposer" -addTests
```

### delete-composer
Removes a Composer from a project, deleting the file, if it exists.

If there is a test file associated with the specified Composer, the framework will call the [delete-test](#delete-test) function.

Parameters:
| Parameter Name  | Required | Desription | Type | Default Value |
|----------|----------|----------|----------|----------|
| name | Yes | The Composer's name, this should not include the full path like the associated `new-composer` function. | string | N/A |

Example calls via PowerShell:
```PowerShell
doing delete-composer -name MyComposer
```

### new-test
Adds a new Test to a project, see the [Testing](./Testing.md) documentation to learn more.

Called by the [new-process](#new-process), [new-module](#new-module) and [new-composer](#new-composer) functions if the `addTests` parameter is supplied.

This can also be called retrospectively to supplement an existing Module/Process if the Test file was not created at their time of creation.

A test MUST be designated for either a Module or Process, despite the below parameters `forProcess` and `forModule` indicated below being flagged as optional.

Parameters:
| Parameter Name  | Required | Desription | Type | Default Value |
|----------|----------|----------|----------|----------|
| name | Yes | The Test's name, this should include the full path to the Test file from the project's associated Process or Module directory. Note that the name of the Test is the final part of the supplied string. If a Test with the same name already exists, the framework will alert the caller and take no action. The Test's name should be suffixed "Tests". | string | N/A |
| forProcess | No | Informs the framework that the test is intended for a Process. | switch/boolean | false |
| forModule | No | Informs the framework that the test is intended for a Module. | switch/boolean | false |
| forComposer | No | Informs the framework that the test is intended for a Composer. | switch/boolean | false |

Example calls via PowerShell:
```PowerShell
doing new-test -name MyProcessTests -forProcess

doing new-test -name MyModuleTests -forModule

doing new-test -name MyComposerTests -forComposer

doing new-test -name "My/Nested/ProcessFileTests" -forProcess

doing new-test -name "My/Nested/ModuleFileTests" -forModule

doing new-test -name "My/Nested/MyComposerTests" -forComposer
```

### delete-test
Removes a Test from a project, deleting the file, if it exists.

Parameters:
| Parameter Name  | Required | Desription | Type | Default Value |
|----------|----------|----------|----------|----------|
| name | Yes | The Test's name, this should not include the full path like the associated `new-module` function. | string | N/A |

Example calls via PowerShell:
```PowerShell
doing delete-test -name MyProcessTests

doing delete-test -name MyModuleTests
```

## Invocation Functions

This section details how to utilise a projects low hanging fruit: running processes and tests.

### Run
Runs a specified Process, see the [documentation](./Processes.md) for more detail.

Parameters:
| Parameter Name  | Required | Desription | Type | Default Value |
|----------|----------|----------|----------|----------|
| name | Yes | The Process's name, this should not include the full path like the associated `new-process` function. | string | N/A |
| doOutput | No | Returns the [DoFramework.Processing.IContext](../src/DoFramework/DoFramework/Processing/Context/IContext.cs) object associated with the run. | switch/boolean | false |
| showReports | No | Presents a view of the Processes executed, the output ([DoFramework.Domain.ProcessResult](../src/DoFramework/DoFramework/Domain/ProcessResult.cs)) and information relating to execution time, all in tabular form. | switch/boolean | false |
| extra parameters | No | Optional collection of additional values or switches, these must also follow the syntax called out in the [Syntax](#syntax) section. The intent of these parameters is to load extra data for Process consumption at runtime, see the section in the [Process Context](./ProcessContext.md#cli-based-context-population) documentation to learn more. | Any | N/A |

Example calls via PowerShell:
```PowerShell
doing run -name "MyProcess"

doing run -name "MyProcess" -doOutput

doing run -name "MyProcess" -doOutput -showReports

doing run -name "MyProcess" -doOutput -showReports -key1 Val1 -AdditionalSwitch
```

### Compose
Runs a Composer, see the [documentation](./Composers.md) for more detail.

Parameters:
| Parameter Name  | Required | Desription | Type | Default Value |
|----------|----------|----------|----------|----------|
| name | Yes | The Composer's name, this should not include the full path like the associated `new-composer` function. | string | N/A |
| doOutput | No | Returns the [DoFramework.Processing.IContext](../src/DoFramework/DoFramework/Processing/Context/IContext.cs) object associated with the run. | switch/boolean | false |
| showReports | No | Presents a view of the Processes executed, the output ([DoFramework.Domain.ProcessResult](../src/DoFramework/DoFramework/Domain/ProcessResult.cs)) and information relating to execution time, all in tabular form. | switch/boolean | false |
| extra parameters | No | Optional collection of additional values or switches, these must also follow the syntax called out in the [Syntax](#syntax) section. The intent of these parameters is to load extra data for Process consumption at runtime, see the section in the [Process Context](./ProcessContext.md#cli-based-context-population) documentation to learn more. | Any | N/A |

Example calls via PowerShell:
```PowerShell
doing compose -name "MyComposer"

doing compose -name "MyComposer" -doOutput

doing compose -name "MyComposer" -doOutput -showReports

doing compose -name "MyComposer" -doOutput -showReports -key1 Val1 -AdditionalSwitch
```

### Test
Runs Tests specified by a filter, see the [documentation](./Testing.md) to discover more.

Parameters:
| Parameter Name  | Required | Desription | Type | Default Value |
|----------|----------|----------|----------|----------|
| filter | Yes | Filters the Tests on their name, use `.*` to run all tests or a specific substring to target individuals | string | N/A |
| forProcesses | No | Switch to instruct the framework to only execute Process Tests. | switch/boolean | false |
| forModules | No | Switch to instruct the framework to only execute Module Tests. | switch/boolean | false |
| forComposers | No | Switch to instruct the framework to only execute Composer Tests. | switch/boolean | false |
| outputFormat | No | Indicates to the framework that it should output the Pester test results to the base directory of the current project. Values must parse exactly to one of the [DoFramework.Testing.PesterOutputType](../src/DoFramework/DoFramework/Testing/PesterOutputType.cs) values which dictates the test output type. | string | [PesterOutputType]::None |

Example calls via PowerShell:
```PowerShell
doing test -filter .*

doing test -filter MyProcess

doing test -filter MyProcessTests

doing test -filter MyModule

doing test -filter MyModuleTests

doing test -filter MyComposer

doing test -filter MyComposerTests

doing test -filter .* -forProcesses

doing test -filter .* -forModules

doing test -filter .* -forComposers

doing test -filter .* -outputFormat None

doing test -filter .* -outputFormat NUnitXml

doing test -filter .* -outputFormat JUnitXml
```

### Exec
Executes a target with a dofile.

Parameters:
| Parameter Name  | Required | Desription | Type | Default Value |
|----------|----------|----------|----------|----------|
| target | Yes | The name of the dofile target to execute. | string | N/A |
| extra parameters | No | Optional collection of additional values or switches, these must also follow the syntax called out in the [Syntax](#syntax) section. These values override default values if specified in the dofile itself (outside of Target functions) or create new ones if they do not exist. | Any | N/A |

Example calls via PowerShell:
```PowerShell
doing exec -target MyTarget

doing exec -target MyTarget -myBoolOrSwitch

doing exec -target MyTarget -myString "123"

doing exec -target MyTarget -myString "123" -myBoolOrSwitch
```

## Utility Functions
Utility functions are called from within code files within a project structure, the framework provides these to enrich developer experience where necessary.

### Mock
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
[DoFramework.Testing.ProxyResult] $otherClassProxy = doing mock -type ([OtherClass]);
[DoFramework.Testing.ProxyResult] $otherClass2Proxy = doing mock -type ([OtherClass2]);
[DoFramework.Testing.ProxyResult] $myClassProxy = doing mock -type ([MyClass]) -params @($otherClassProxy.Instance, $otherClass2Proxy.Instance);
```

### Get-MethodInfo
**Used by the mock function, not intended for developer usage.**

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

### Args
The args method is provided as a convenience method returning a `Dictionary[string, object]`, it is generally used when verifying mock method calls.

Parameters:
| Parameter Name  | Required | Desription | Type | Default Value |
|----------|----------|----------|----------|----------|
| extra parameters | No | Optional collection of additional values or switches, these must also follow the syntax called out in the [Syntax](#syntax) section. Each key and value supplied in the form of standard parameters becomes an entry in the dictionary. | Any | N/A |

Example calls via PowerShell:
```PowerShell
# An empty Dictionary
[System.Collections.Generic.Dictionary[string, object]] $dictionary = doing args;

# A Dictionary with 1 entry
[System.Collections.Generic.Dictionary[string, object]] $dictionary = doing args -key1 val1;

# A Dictionary with 2 entries.
[System.Collections.Generic.Dictionary[string, object]] $dictionary = doing args -key1 val1 -key2 val2;
```