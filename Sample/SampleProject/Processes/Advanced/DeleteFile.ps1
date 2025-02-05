using namespace DoFramework.Logging;
using namespace DoFramework.Processing;
using module "..\..\Modules\DeletePersonsFile.psm1";

class DeleteFile : Process {
    [DeletePersonsFile] $DeletePersonsFile;
    [ILogger] $Logger;
    [IContext] $Context;

    DeleteFile(
        [DeletePersonsFile] $deletePersonsFile,
        [ILogger] $logger,
        [IContext] $context) {
        $this.DeletePersonsFile = $deletePersonsFile;
        $this.Logger = $logger;
        $this.Context = $context;
    }

    [bool] Validate() {
        return $this.Context.Requires().
            ComposedBy("AdvancedComposer").
            ConfirmKey("PersonsFilePath").
            ProcessSucceeded("AdvancedProcess").
            Verify();
    }

    [void] Run() {
        $this.Logger.LogInfo("Deleting persons file...");

        $this.DeletePersonsFile.Delete();

        $this.Logger.LogInfo("Deleted persons file...");
    }
}
