# Modules
DoFramework Modules are standard PowerShell modules which developers/contributors can use to house/abstract PowerShell functions and classes away from Process class files. They are optional in the sense that they are not required to use the framework, but are indeed a useful feature. 

Modules have the file format .psm1, see [here](./ProjectStructure.md) for more detail on where Modules fit into a project.

It is possible to develop traditional PowerShell modules for usage outside of the DoFramework with the features present here, with some advantages from a testing perspective:
- Assistance writinng and running PowerShell tests using the CLI provided by this framework
- The ability to mock PowerShell classes

## Module Features
Modules provide the following features:
- The creation of reusable PowerShell Functions.
- The creation of reusable PowerShell classes.
- The creation of dotnet interfaces.
- THe possibility of referencing other dotnet packages without polluting class files with noise.

Unit tests can be written for Modules, do the functions and classes function as intended? In depth examples can be found in the sample project.

There are many example Modules in the [Sample Project](../Sample), consult the specific documentation on how to [add modules](./CLIFunctions.md#add-module) to a project. For more information on working with tests see [here](./Testing.md).

Additionally, the PowerShell unit/component tests which test this framework also provide good examples to show how the framework can be used, for example see [here](../src/DoCli/Tests/Component/Do/Modules/TestClassModule.psm1) where a dotnet interface consumed by a using statement (similar to a Process) and some classes are specified.

To see how Modules can be used by Processes see [here](./Processes.md).

To explore the possibiliies with Do's Depedency Injection functionality see [here](./DependencyInjection.md).

## Modules Walk Through
When a Module is added to a Project, it's code will take the form:

```PowerShell
# TODO: Create classes and functions.

function ExampleFunction {
}

Export-ModuleMember -Function ExampleFunction;

class ExampleClass {
}
```

By default an example function and class are included to provide some boiler plate support.

Note - the `Export-ModuleMember` is required such that the functions will be accessible from outside of the module.