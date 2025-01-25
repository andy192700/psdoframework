using namespace DoFramework.Processing;
using module "..\..\Modules\ModuleWithFunctions.psm1";

# A process that will always fail as it calls the PS function "ThrowsAnException" from the ModuleWithFunctions.psm1 module, which throws an Exception when called
class FailingProcess : Process {
    [void] Run() {
        ThrowsAnException;
    }
}
