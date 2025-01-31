using namespace DoFramework.Processing;
using module "..\Modules\CreatePersons.psm1";
using module "..\Modules\CreatePersonsFile.psm1";
using module "..\Modules\DeletePersonsFile.psm1";
using module "..\Modules\ReadPersonsFile.psm1";

class AdvancedComposer : IComposer {
    [void] Compose([IComposerWorkBench] $workBench) {
        
        $workBench.RegisterService([CreatePersons]).
            And([CreatePersonsFile]).
            And([DeletePersonsFile]).
            And([ReadPersonsFile]);

        $workBench.RegisterProcess("AdvancedProcess").
            And("DeleteFile").
            And("CreateData").
            And("CreateFile").
            And("ReadFile").
            And("DeleteFile");
    }
}