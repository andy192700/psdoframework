using namespace DoFramework.Processing;

class TestComposer : IComposer {
    [void] Compose([IComposerWorkBench] $workBench) {
        [IContext] $context = $workBench.GetService([IContext]);

        $workBench.RegisterProcess($context.Get("theProcessToRun"));
    }
}
