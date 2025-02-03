# Project Structure
Rather than working with isolate scripts, PowerShell development using Do is done within a fixed project structure, it is made up of the following components:
- Processes
- Modules
- Composers
- Tests
- Project file
- Env files

The above are described in more detail in the subsequent sections and a figure illustrating the project structure can be found at the end of this document.

A sample project is provided with this repository to demonstrate the framework's capability [here](../Sample).

## Processes
The replacement for traditional PowerShell scripts, these are runnable entities via the CLI provided by the framework.

Processes are .ps1 files that are intended to house a single PowerShell class which ought to be a derivative of the `DoFramework.Processing.Process` class (provided by the framework).

Though these files must exist within the Processes directory, it is possible to create these within nested directories underneath allowing for scalability.

To add these to your project see the [add-process](./CLIFunctions.md#add-process) function.

For more information regarding Processes see the relevant [documentation](./Processes.md).

## Modules
DoFramework Modules are in fact traditional PowerShell modules, taking the form of .psm1 files.

Modules are optional entities that house functions and classes for reusable/abstraction purposes.

Similar to Processes, these files must exist within the Modules directory, it is possible to create these within nested directories underneath allowing for scalability.

To add these to your project see the [add-module](./CLIFunctions.md#add-module) function.

For more information regarding Modules see the relevant [documentation](./Modules.md).

## Composers
DoFramework Composers are .ps1 files that handle DI registration and configuration, like Processes the intent is that they house a single class implementing the `DoFramework.Processing.IComposer` interface, click [here](./Composers.md) to find out more.

To add these to your project see the [add-composer](./CLIFunctions.md#add-composer) function.

These files must exist within the Composers directory, it is possible to create these within nested directories underneath allowing for scalability.

## Tests
Tests are optional .ps1 files which run off the module Pester (which is deployed with the DoFramework)

Tests test either Modules/Processes, they reside in Tests/Modules or Tests/Processes directory, mirroring the Process/Module directory structure to enfore consistency.

Tests can be added with the optional switches on the `add-process`, `add-module` or `add-composer` functions. If an entity is created without Tests (as mentioned above these are optional) these can be added afterwards using the [add-test](./CLIFunctions.md#add-test) function.

For more information regarding Tests see the relevant [documentation](./Testing.md).

## Project File
DoFramework projects are driven by a project file named `do.json`, this exists at the root of the project's working directory.

The file contains the following information:
- DoFramework version (at the time of project creation)
- PowerShell version (at the time of project creation)
- Processes (paths to the Process files)
- Modules (paths to the Module files)
- Composers (paths to the Composer files)
- Tests (paths to the Test files)

## Env Files
When running a Process via Do, Do will automatically consume all files from the project's root directory who match the pattern `.env*`, these are simple files containing lines of the form `key=value`. These files are consumed at runtime into Do's Processing Context, see the relevant documentation [here](./ProcessContext.md).

## Project Structure Illustration
```
Parent Directory
│   do.json
│   .env*    
│
└───Project Name
    ├───Processes
    │   │   Process1.ps1
    │   │   Process2.ps1
    │   │
    │   └───NestedProcessFolder
    │       │   Process3.ps1
    │       │   Process4.ps1
    │       │   ...
    │
    ├───Modules
    │   │   Module1.psm1
    │   │   Module2.psm1
    │   │
    │   └───NestedProcessFolder
    │       │   Module3.psm1
    │       │   Module4.psm1
    │       │   ...
    │
    ├───Composers
    │   │   Composer1.psm1
    │   │   Composer2.psm1
    │   │
    │   └───NestedProcessFolder
    │       │   Composer3.psm1
    │       │   Composer4.psm1
    │       │   ...
    │
    └───Tests
        ├───Processes
        │   │   Process1Tests.ps1
        │   │   Process2Tests.ps1
        │   │
        │   └───NestedProcessFolder
        │       │   Process3Tests.ps1
        │       │   Process4Tests.ps1
        │       │   ...
        │
        ├───Modules
        │   │   Module1Tests.psm1
        │   │   Module2Tests.psm1
        │   │
        │   └───NestedProcessFolder
        │       │   Module3Tests.psm1
        │       │   Module4Tests.psm1
        │       │   ...
        │
        └───Composers
            │   Composer1Tests.psm1
            │   Composer2Tests.psm1
            │
            └───NestedProcessFolder
                │   Composer3Tests.psm1
                │   Composer4Tests.psm1
                │   ...
```