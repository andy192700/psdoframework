# Dependency Injection
One of the great features offered by the DoFramework is the ability to inject dependencies into Processes. This can be done in two ways:
- Injection using built in dependencies that the framework offers.
- Injection using classes defined in Modules defined by the developer, this MUST be done by a [composer](./Composers.md).

The [sample project](../Sample/SampleProject/) provides in depth examples for both, the sample project also demonstrates how to test components, see [here](./Testing.md) to learn more.

## Built In Injection
This section details the offerings of the framework which a developer can inject into their Processes automatically.

Here is an example injecting the `DoFramework.Processing.IContext` instance into a Process:

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

An example can be found in the sample project [here](../Sample/SampleProject/Processes/Advanced/AdvancedProcess.ps1).

### IContext
The [DoFramework.Processing.IContext](../src/DoFramework/DoFramework/Processing/Context/IContext.cs) instance has two purposes
- Make Processes configurable with external variables (supplied via the CLI or .env* file)
- Observe Processes that have completed already, this allows developers to ensure certain Processes can only execute if other specified Processes have executed already.

The `IContext` feature is described in detail [here](./ProcessContext.md) and the Advanced Process samples within the sample project demonstrate it's advance usage.

### ILogger
There is a built in Logger which is useful for writing information out via the CLI, it is governed by the interface [DoFramework.Logging.ILogger](../src/DoFramework/DoFramework/Logging/ILogger.cs).

Using the built in logger is easy and has the following advantages:
- Each message contains a datetime stamp
- Each message indicates it's severity (e.g. INFO, DEBUG, FATAL....)
- Each severity has it's own color which makes items that need attention easy to identify.

Again the advanced example in the sample project contains examples on usage. 

## Developer Driven Injection
It is possible to inject developer defined classes into Processes too, as previously stated this is done by using a Composer to initiate processing.

The framework will attempt to fulfill any required dependencies of a type registered and will only be instantiated once, much like a singleton.

Note - one current limitation with dependency injection using this feature is that dependencies are required to have one constructor.