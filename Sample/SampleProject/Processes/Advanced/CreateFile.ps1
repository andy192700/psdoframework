using namespace DoFramework.Logging;
using namespace DoFramework.Processing;
using module "..\..\Modules\CreatePersonsFile.psm1";

class CreateFile : Process {
    [CreatePersonsFile] $CreatePersonsFile;
    [ILogger] $Logger;
    [IContext] $Context;

    CreateFile(
        [CreatePersonsFile] $createPersonsFile,
        [ILogger] $logger,
        [IContext] $context) {
        $this.CreatePersonsFile = $createPersonsFile;
        $this.Logger = $logger;
        $this.Context = $context;
    }

    [bool] Validate() {
        return $this.Context.Requires().
            ConfirmKey("PersonsFilePath").
            ProcessSucceeded("CreateData").
            Verify();
    }

    [void] Run() {
        $this.Logger.LogInfo("Creating persons file...");

        $this.CreatePersonsFile.Create();

        $this.Logger.LogInfo("Created persons file...");
    }
}
