using namespace DoFramework.CLI;
using namespace DoFramework.Processing;
using namespace DoFramework.Validators;
using namespace DoFramework.Services;
using namespace System.Collections.Generic;

class ExecDoFile : CLIFunction[DoFileTargetExecutorValidator] {
    ExecDoFile() : base("exec") {}

    [void] InvokeInternal([Dictionary[string, object]] $params, [IServiceContainer] $serviceContainer) {
        [ServiceContainerExtensions]::AddParameters($serviceContainer, $params);

        [IDoFileInvoker] $invoker = $serviceContainer.GetService([IDoFileInvoker]);

        $invoker.InvokeTarget($params["target"]);
    }
}