using namespace DoFramework.CLI;
using namespace DoFramework.Services;
using namespace DoFramework.Processing;
using namespace DoFramework.Validators;
using namespace System.Collections.Generic;
using module "..\..\..\Processing\ComposerBuilder.psm1";
using module "..\..\..\Processing\ProcessBuilder.psm1";

class Compose : CLIFunction[DescriptorManagementDictionaryValidator, [IContext]] {
    Compose() : base("Compose") {}

    [IContext] Invoke([Dictionary[string, object]] $params, [IServiceContainer] $serviceContainer) {        
        [ServiceContainerExtensions]::AddParameters($serviceContainer, $params);
        [ServiceContainerExtensions]::CheckEnvironment($serviceContainer);
        [ServiceContainerExtensions]::ConsumeEnvFiles($serviceContainer);
        [ServiceContainerExtensions]::AddComposerServices($serviceContainer, [ComposerBuilder]);

        [IComposerOrchestrator] $orchestrator = $serviceContainer.GetService([IComposerOrchestrator]);

        [bool] $success = $orchestrator.Orchestrate($params["name"], $serviceContainer);

        if (!$success) {
            return $null;
        }

        [IProcessRegistry] $registry = $serviceContainer.GetService([IProcessRegistry]);
        
        [ServiceContainerExtensions]::AddProcessingServices($serviceContainer, [ProcessBuilder]);

        [IEntryPoint] $entryPoint = $serviceContainer.GetService[IEntryPoint]();

        return $entryPoint.Enter($registry.ToProcessRequest());
    }
}
