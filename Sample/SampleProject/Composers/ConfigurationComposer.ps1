using namespace DoFramework.Processing;
using module "..\Modules\ConfigurableObject.psm1";

class ConfigurationComposer : IComposer {
    [void] Compose([IComposerWorkBench] $workBench) {

        $workBench.RegisterService([MyConfigurationService])

        $workBench.Configure([MyConfig]).
            And([MySecondConfig]).
            And([MyThirdConfig]);

        $workBench.RegisterProcess("ConfigurationSample");
    }
}