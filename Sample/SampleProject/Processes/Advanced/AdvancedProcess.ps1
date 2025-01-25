using namespace DoFramework.Processing;

# Entry point process for this sample project's advanced example, demonstrating Do's capabilities.
# A IContext is injected for accessing shared values, a IProcessDispatcher is injected to allow for the execution of other processes by this current process
# This example checks that all context values are present, and if so it fires off a sequence of processes, executed in series.
class AdvancedProcess : Process {
    [IProcessDispatcher] $Dispatcher;
    [IContext] $Context;

    AdvancedProcess(
        [IProcessDispatcher] $dispatcher,
        [IContext] $context) {
        $this.Dispatcher = $dispatcher;
        $this.Context = $context;
    }

    [bool] Validate() {
        return $this.Context.KeyExists("Person1FirstName") `
            -and $this.Context.KeyExists("Person1LastName") `
            -and $this.Context.KeyExists("Person1Age") `
            -and $this.Context.KeyExists("Person2FirstName") `
            -and $this.Context.KeyExists("Person2LastName") `
            -and $this.Context.KeyExists("Person2Age") `
            -and $this.Context.KeyExists("Person3FirstName") `
            -and $this.Context.KeyExists("Person3LastName") `
            -and $this.Context.KeyExists("Person3Age");
    }

    [void] Run() {
        [ProcessingRequest] $request = [ProcessingRequest]::new(@(
            "Registrations",
            "DeleteFile",
            "CreateData",
            "CreateFile",
            "ReadFile",
            "DeleteFile"
        ));

        $this.Dispatcher.Dispatch($request);
    }
}
