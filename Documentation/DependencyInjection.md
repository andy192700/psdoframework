# Dependency Injection
One of the great features offered by the DoFramework is the ability to inject dependencies into Processes. This can be done in two ways:
- Injection using built in dependencies that the framework offers.
- Injection using classes defined in Modules defined by the developer.

The [sample project](../Sample/SampleProject/) provides in depth examples for both, the sample project also demonstrates how to test the Processes/Modules see [here](./Testing.md) to learn more.

## Built In Injection
This section details the offerings of the framework which a developer can inject into their Processes automatically.

Here is an example injecting the `DoFramework.Processing.Context` instance into a Process:

```PowerShell
using namespace DoFramework.Processing;

class DIExample : Process {
    [IContext] $Context;

    DIExample([IContext] $context) {
        $this.Context = $context;
    }

    [bool] Validate() {
        return $true;
    }

    [void] Run() {
    }
}
```

Above, the `$this.Context` object can be accessed from inside the `Validate` and `Run` methods as it is an instance member. Indeed, regardless of the type of the desired dependency - this is how you inject one.

Further, any dependency defined in the `DoFramework.Services.IServiceContainer` instance which drives the running of Processes, defined [here](../src/DoCli/Objects/Services/ApplicationServiceContainer.psm1) can be injected into a Process.

Continuing on in this section, some incredibly useful dependencies are discussed in more detail.

### IProcessDispatcher
The [DoFramework.Processing.IProcessDispatcher](../src/DoFramework/DoFramework/Processing/Invocation/IProcessDispatcher.cs) is used to invoke other Processes from a Process, this allows developers to split their Processes into individual operations, helping to drive the single responsibility priciple.

It requires a [DoFramework.Processing.IProcessingRequest](../src/DoFramework/DoFramework/Processing/Invocation/IProcessingRequest.cs) instance to be initialised which houses an array of Processes (called out by name). The specified Processes will execute in series from within the current Process.

An example can be found in the sample project [here](../Sample/SampleProject/Processes/Advanced/AdvancedProcess.ps1).

### IContext
The [DoFramework.Processing.IContext](../src/DoFramework/DoFramework/Processing/Context/IContext.cs) instance has two purposes
- Make Processes configurable with external variables (supplied via the CLI or .env* file)
- Observe Processes that have completed already, this allows developers to ensure certain Processes can only execute if other specified Processes have executed already.

The `IContext` feature is described in detail [here](./ProcessContext.md) and the Advanced Process samples within the sample project demonstrate it's advance usage.

### IServiceContainer
The driving `IServiceContainer` can also be injected into a Process, which allows developers to register PowerShell classes defined in Modules, once registered they can be called upon immediately OR they can forfill required dependencies for subsequent Processes.

See how the [advanced example](../Sample/SampleProject/Processes/Advanced/Registrations.ps1) registers classes from Modules which are later dialed in other Processes, for example [see](../Sample/SampleProject/Processes/Advanced/DeleteFile.ps1) where the `DeletePersonsFile` is picked up after being registered in a prior Process.

### ILogger
There is a built in Logger which is useful for writing information out via the CLI, it is governed by the interface [DoFramework.Logging.ILogger](../src/DoFramework/DoFramework/Logging/ILogger.cs).

Using the built in logger is easy and has the following advantages:
- Each message contains a datetime stamp
- Each message indicates it's severity (e.g. INFO, DEBUG, FATAL....)
- Each severity has it's own color which makes items that need attention easy to identify.

Again the advanced example in the sample project contains examples on usage. 

## Developer Driven Injection
It is possible to inject developer defined classes into Processes too, this is done using the aforementioned `IServiceContainer` instance.

It is also worth stating that the `IServiceContainer` will attempt to forfill any required dependencies required to construct an object, registrations are instantiated once and are reused like a singleton.

Note - one current limitation with dependency injection using this feature is that dependencies are required to have one constructor.