# Processes
Processes are the bedrock of the DoFramework, replacing standard scripting with .ps1 files containing a single class which must derive from the [DoFramework.Processing.Process](../src/DoFramework/DoFramework/Processing/Process.cs) class.

See [here](./ProjectStructure.md) for more detail on where Processes fit into a project.

## Process features
Proceses have the following features:
- Dependency Injection via the class constructor.
- Built in error handling/current directory management.
- Overridable pre-flight validation checks.
- Reference Modules in the project using "developer familiar" PowerShell using statements.

See the [Dependency Injection](./DependencyInjection.md) documentation for more information relating to injecting services spun up with the DoFramework by default OR those registered by previous Processes. This feature has many built in uses, such as accessing of [Context](./ProcessContext.md) values, dispatching of new Processes and more!

Processes can either: run successfully, be invalid or fail. If a Process is invalid or fails, all subsequent Processes will not execute and be in a "not run" state, see all possible states [here](../src/DoFramework/DoFramework/Domain/ProcessResult.cs).

If an exception is thrown whilst a Process is executing, Do will automatically catch this, log the Exception and flag the Process as failed. In addition, Do will always ensure the executing shell's directory is always reset regardless of the result of a Process.

More information on creating/working with DoFramework Modules can be found [here](./Modules.md).

There are many example Processes in the [Sample Project](../Sample), consult the specific documentation on how to [add processes](./CLIFunctions.md#add-process) to a project, to learn about how to run a process, see this [section](./CLIFunctions.md#run-process).

End to end/unit tests can be written for Processes, does it work in isolation, does the framework execute the process successfully? In depth examples can be found in the sample project. For more information on working with tests see [here](./Testing.md).

## Process Walk through
When a Process is added to a project, it's code will take the form:

```PowerShell
using namespace DoFramework.Processing;

class DocumentationSample : Process {
    [void] Run() {
        # TODO: implement process
    }
}
```

Above we see a code snippet with a namespace reference and a single class. In particular we see no reference to a constructor (as it uses the default constructor of the Process class) and no override of the Validate method.

Below is a code snippet of a Process with more features, which includes:
- A reference of a DoFramework Module.
- Class properties
- Class constructor
- Override Validate method

Using these tools, an implementer can achieve exactly what they wish to using the tools described throughout this document.

Note - unlike dotnet/java where the reference variable `this` is optional, in PowerShell this is mandatory, otherwise the code will not be interpereted correctly.

```PowerShell
using namespace DoFramework.Processing;
using module "..\..\Modules\MyModule.psm1"; # Reference to a Module, accessing any functions/classes that it offers

class DocumentationSample : Process {
    # Class Properties
    [string] $MyStringProp1;
    [int] $MyIntProp1;
    [MyDependency] $MyDependency;

    DocumentationSample([MyDependency] $myDependency) {
        # Inject dependencies or some other constructor activity
        $this.MyDependency = $myDependency;
        $this.MyIntProp1 = 0;
        $this.MyStringProp1 = "123";
    }

    [bool] Validate() {
        # Some activity to check whether it is okay to run the Process.
        return $true;
    }

    [void] Run() {
        # TODO: implement process
    }
}
```