using namespace DoFramework.Logging;
using namespace DoFramework.Processing;
using module "..\..\Modules\CreatePersonsFile.psm1";

# A process that creates a file, adding it to an IContext, it requires injection from a previous process 
#    - specifically the CreatePersonsFile object and other dependencies supplied by the DoFramework.
# If instantiation is possible, the process will execute if prior successfull processes have been completed, in the correct order.
# Whilst executing it calls the CreatePersonsFile.Create method which requests data from the IContext and writes it to a JSON file.
# Logging is added to provide observability, use of "-silent" via the command line will suppress this
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
            ProcessSucceeded("DeleteFile").
            ProcessSucceeded("CreateData").
            Verify();
    }

    [void] Run() {
        $this.Logger.LogInfo("Creating persons file...");

        $this.CreatePersonsFile.Create();

        $this.Logger.LogInfo("Created persons file...");
    }
}
