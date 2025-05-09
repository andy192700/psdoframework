# PSDoFramework
We are always doing something!

The PSDoFramework (referred to as `DoFramework` internally) is an open source PowerShell module that enables developers to create fully object-orientated PowerShell projects rather than isolated and unstructured scripts. The objective is to bring practices from modern application development into the realm of PowerShell. 

This module is cross platform and only requires installation of the module to build/run PowerShell projects built with the framework.

This framework offers developers some excellent features:
- Fully object-orientated code and project structure
- A useful CLI for building, managing and running projects
- A built in makefile alternative (dofiles) that leverages PowerShell's superior syntax and command line freedoms
- Dependency Injection
- Environmental variable consumption
- Unit and end to end testing
- Simple mocking of PowerShell classes and dotnet interfaces

To install the module and start creating projects see the [getting started](./Documentation/GettingStarted.md) documentation.

To install the dotnet tool wrapper for the PowerShell module see the [dotnet tool](./Documentation/DotnetTool.md) documentation.

A [sample project](./Sample/) is provided to demonstrate the framework's capabilities.

A [sample dofile](./dofile.ps1) is also provided to show the makefile alternative's simplicity and usage.

All available PowerShell module versions can be found via Microsoft's [PowerShell Gallery](https://www.powershellgallery.com/packages/PSDoFramework), the versions of the dotnet tool can be found on the [NuGet Gallery](https://www.nuget.org/packages/PSDoFramework.Tool).

Release notes are stored [here](./Documentation/ReleaseNotes/).

## Documentation
This sections contains links and descriptions to all available documentation.

| Name  | Description | Link |
|----------|----------|----------|
| Dofiles | Explains the purpose and usage of dofiles. | [Link](./Documentation/Dofiles.md) |
| Project Structure | Details a project's structure and its components. | [Link](./Documentation/ProjectStructure.md) |
| Processes | Processes replace traditional PowerShell scripts, these are to be thought of as "entry points". | [Link](./Documentation/Processes.md) |
| Modules | Modules are PowerShell Modules that can be utilised using developer-friendly `using` statements, they enrich Processes with reusable PowerShell functions and classes. | [Link](./Documentation/Modules.md) |
| Composers | Composers perform DI registration, configure objects and dictate which Processes should run. | [Link](./Documentation/Composers.md) |
| Testing | Writing tests is possible for Processes and Modules, the DoFramework incorporates Pester into its installation and supplements testing capability with the ability to mock PowerShell classes and dotnet interfaces with ease. | [Link](./Documentation/Testing.md) |
| CLI Functions | Learn more about this framework's PowerShell command line interface. | [Link](./Documentation/CLIFunctions.md) |
| Dependency Injection | Discover how to inject dependencies into Processes, improving testability. | [Link](./Documentation/DependencyInjection.md) |
| Process Context | Developers can inject a context into Processes, this allows the setting of variables from outside of code using the CLI or .env files. | [Link](./Documentation/ProcessContext.md) |
| DotnetTool | A dotnet tool exists for CLI-agnostic convenience. | [Link](./Documentation/DotnetTool.md) |
| Third Party Licenses | Documents all third party components, their version numbers, why they are used and provides links to license agreements. | [Link](./Documentation/ThirdPartyLicenses.md) |