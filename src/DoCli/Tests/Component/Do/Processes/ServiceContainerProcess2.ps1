using namespace DoFramework.Processing;
using module "..\Modules\TestClassModule.psm1";
using module "..\Modules\TestClassModule2.psm1";

class ServiceContainerProcess2 : Process {
    [IExample] $Interface;
    [BasicClass] $Instance;
    [IContext] $Context;

    ServiceContainerProcess2(
        [IExample] $interface, 
        [BasicClass] $instance, 
        [IContext] $context) {
        $this.Interface = $interface;
        $this.Instance = $instance;
        $this.Context = $context;
    }

    [void] Run() {
        $this.Context.AddOrUpdate("Interface", $this.Interface);
        $this.Context.AddOrUpdate("Instance", $this.Instance);
    }
}
