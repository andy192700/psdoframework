using namespace DoFramework.Processing;
using namespace DoFramework.Services;
using module "..\Modules\TestClassModule.psm1";
using module "..\Modules\TestClassModule2.psm1";

class ServiceContainerProcess1 : Process {
    [IProcessDispatcher] $Dispatcher;
    [IServiceContainer] $Services;

    ServiceContainerProcess1([IProcessDispatcher] $dispatcher, [IServiceContainer] $services) {
        $this.Dispatcher = $dispatcher;
        $this.Services = $services;
    }

    [void] Run() {
        $this.Services.RegisterService([IExample], [Example]);

        $this.Services.RegisterService([BasicClass]);

        [ProcessingRequest] $request = [ProcessingRequest]::new(@(
            "ServiceContainerProcess2"
        ));

        $this.Dispatcher.Dispatch($request);
    }
}
