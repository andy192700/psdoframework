using namespace DoFramework.Processing;
using module "..\..\Modules\ModuleWithFunctions.psm1";

class FailingProcess : Process {
    [void] Run() {
        ThrowsAnException;
    }
}
