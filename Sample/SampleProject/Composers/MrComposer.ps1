using namespace DoFramework.Processing;
using namespace DoFramework.Services;
using module "..\Modules\ConfigurableObject.psm1";

class MrComposer : IComposer {
    [void] Compose([IServiceContainer] $container) {
        Write-Host "hello world!!!";

        [ServiceContainerExtensions]::Configure($container, [MyConfig]);


        Write-Host "$($container.GetService([MyConfig]).MyString) $($container.GetService([MyConfig]).MyInt)"

        [ServiceContainerExtensions]::RegisterProcess($container, "DoublesANumber");
        [ServiceContainerExtensions]::RegisterProcess($container, "SimpleProcess");
    }
}