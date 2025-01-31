using namespace DoFramework.Processing;
using module "..\Modules\ConfigurableObject.psm1";

class ConfigurationComposer : IComposer {
    [void] Compose([IComposerWorkBench] $workBench) {

        $workBench.Configure([MyConfig]).
            And([MySecondConfig]).
            And([MyThirdConfig]);

        $workBench.RegisterProcess("ConfigurationSample");
    }
}