# PSDoFramework
We are always doing something!

The PSDoFramework (referred to as `DoFramework` internally) is an open source PowerShell module that enables developers to create fully object-orientated PowerShell projects rather than isolated and unstructured scripts. The objective is to bring practices from modern application development into the realm of PowerShell.

This framework offers developers some excellent features:
- Fully object-orientated code and project structure
- A useful CLI for building, managing and running projects
- Dependency Injection
- Environmental variable consumption
- Unit and end to end testing
- Simple mocking of PowerShell classes and dotnet interfaces

To install the module and start creating projects see the [getting started](./Documentation/GettingStarted.md) documentation.

A [sample project](./Sample/) is provided to demonstrate the framework's capabilities.

All available package versions can be found via Microsoft's [PowerShell Gallery](https://www.powershellgallery.com/packages/PSDoFramework).

Release notes are stored [here](./Documentation/ReleaseNotes/).

## Documentation
This sections contains links and descriptions to all available documentation.

| Name  | Description | Link |
|----------|----------|----------|
| Project Structure | Details a project's structure and its components. | [Link](./Documentation/ProjectStructure.md) |
| Processes | Processes replace traditional PowerShell scripts, these are to be thought of as "entry points". | [Link](./Documentation/Processes.md) |
| Modules | Modules are PowerShell Modules that can be utilised using developer-friendly `using` statements, they enrich Processes with reusable PowerShell functions and classes. | [Link](./Documentation/Modules.md) |
| Composers | Composers perform DI registration, configure objects and dictate which Processes should run. | [Link](./Documentation/Composers.md) |
| Testing | Writing tests is possible for Processes and Modules, the DoFramework incorporates Pester into its installation and supplements testing capability with the ability to mock PowerShell classes and dotnet interfaces with ease. | [Link](./Documentation/Testing.md) |
| CLI Functions | Learn more about this framework's PowerShell command line interface. | [Link](./Documentation/CLIFunctions.md) |
| Dependency Injection | Discover how to inject dependencies into Processes, improving testability. | [Link](./Documentation/DependencyInjection.md) |
| Process Context | Developers can inject a context into Processes, this allows the setting of variables from outside of code using the CLI or .env files. | [Link](./Documentation/ProcessContext.md) |
| Third Party Licenses | Documents all third party components, their version numbers, why they are used and provides links to license agreements. | [Link](./Documentation/ThirdPartyLicenses.md) |