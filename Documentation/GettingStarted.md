# Getting Started
This document details how to get moving with this framework:
- OS Availability & Prerequisites
- Installation
- Creating a Project

## OS Availability & Prerequisites
The DoFramework is proven out in it's build pipeline, fully built and tested with the following OS':
- Windows
- MacOS
- Linux

The DoFramework will **always** be supported by the current LTS version of PowerShell, this will only change in the months running up to the change over of LTS versions.

Currently this is **PowerShell 7.4**, developers must ensure this is installed before attempting to install the module.

Information on available PowerShell versions can be discovered [here](https://learn.microsoft.com/en-us/powershell/scripting/install/powershell-support-lifecycle?view=powershell-7.4#powershell-end-of-support-dates).

**Ensure PowerShell is enabled.**.

If you are developing VSCode take a look at the VSCode PowerShell [extension](https://marketplace.visualstudio.com/items?itemName=ms-vscode.PowerShell) (recommended, not required).

## Installation
On an appropriate OS and with the aforementioned PowerShell version installed, a developer can proceed with installing the module.

In a PowerShell Core terminal, run the following:

```PowerShell
install-module -Name PSDoFramework
```

Alternatively, to be explicit, install the Module using the official PowerShell NuGet repository and a specific DoFramework version:

```PowerShell
install-module -Name PSDoFramework -RequiredVersion x.x.x -Repository PSGallery
```

Above, `x.x.x` indicates the framework version, to find all available versions of this module run the following:

```PowerShell
find-module -Name PSDoFramework -allversions
```

Installing this Module delivers all required dependencies.

## Creating a Project
The intention of this section is to provide a walk through of a simplest example of how to setup a project, add a Process and run it.

Firstly, open a PowerShell Core Terminal, then in a directory of your choosing run the following:

```PowerShell
doing new-project
```

The above command will create a project, including:
- a `do.json` file
- a directory structure for housing your code (which will be empty at this point)
- subdirectories with `.gitkeep` files within them, this is so the structure can be instantly committed to source control, feel free to remove them as they are completely benign otherwise

For more information regarding project structure see the relevant [documentation](./ProjectStructure.md), for more detail regarding the `new-project` function see [here](./CLIFunctions.md#new-project).

Secondly, add a Process to the project:

```PowerShell
doing add-process -name MyProcess
```

A file named `MyProcess.ps1` will now be present in this Process subdirectory within the project directory structure, it will also be added to the do.json file.

For more information regarding Processes, explore the [documentation](./Processes.md), the `add-process` function is also documented [here](./CLIFunctions.md#add-process).

Now that we have a project and a Process added, we can run it!

Execute the following command to run the Process:

```PowerShell
doing run -name MyProcess -showReports
```

The Process will execute and the framework will print some headline results out on the terminal.

To learn more about the `run` function, see the [documentation](./CLIFunctions.md#run).

The fun doesn't stop there - this framework is packed full of useful features which can assist developers in achieving their goals in a clean and scalable manner:
- Utilise [Composers](./Composers.md) to orchestrate Processing, delve into advanced configuration and register depedencies
- Supplement your Processes with "off the shelf" features provided by the framework using [dependency injection](./DependencyInjection.md)
- Further supplement your Processes with reusable PowerShell functions and classes using [Modules](./Modules.md)
- Learn about [Testing](./Testing.md) Modules and Processes to grow confidence in your pipeline/local development scripting
- Explore how you can configure your Processes with environmental variables using the built in [Process Context](./ProcessContext.md)
- Take a look at the [sample project](../Sample/) which has been designed to demonstrate this framework's capability

All functions offered by this framework are documented [here](./CLIFunctions.md).

# Contributing
Contributions to the framework of any form - refactoring/improvements or new features are all welcome.

This section covers what is required to setup for local development and what is expected in pull requests to main.

## Local Development Setup
In addition to the prerequisites to use the DoFramework, some extra dependencies are required to aid local setup:
- GNU Make
- Dotnet 8

To build the framework locally we use the make target `localbuild`, this builds the framework's PowerShell from the code, publishes it to a local PowerShell NuGet repository and installs it.

Firstly, open a PowerShell terminal in the base of this repository and create the local NuGet repository by running:

```PowerShell
make createlocalnugetsource
```

Finally, run the `localbuild` make target from the same location:

```PowerShell
make localbuild
```
The above command runs a localised version of the pipeline, where it:
- Builds the framework's dotnet project
- Runs the dotnet project's unit tests
- Creates a PowerShell Module manifest
- Imports the PowerShell Module
- Runs the PowerShell automated tests:
    - PowerShell unit tests
    - PowerShell component tests
    - Sample Project tests
- Publishes the PowerShell module to the local repository
- Installs/updates the PowerShell module on the local system

Running this process and passing all of the tests provides good confidence that the new framework is in good health.

## Pull Requests
Pull requests are required in order to commit to main (committing directly has been disabled), they should include:
- A full description of the change, and what it achieves
- Updates to documentation, where required
- Screenshots of the tests passing in the local build
