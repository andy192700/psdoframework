using namespace DoFramework.Processing;

class TestOrchestrator : Process {
    [IProcessDispatcher] $Dispatcher;

    TestOrchestrator([IProcessDispatcher] $dispatcher) {
        $this.Dispatcher = $dispatcher;
    }

    [void] Run() {

        [ProcessingRequest] $request = [ProcessingRequest]::new(@(
            "TestProcess1", 
            "TestProcess2",
            "TestProcess3"
        ));

        $this.Dispatcher.Dispatch($request);
    }
}
