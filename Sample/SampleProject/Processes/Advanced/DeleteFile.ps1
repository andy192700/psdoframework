using namespace DoFramework.Logging;
using namespace DoFramework.Processing;
using module "..\..\Modules\DeletePersonsFile.psm1";

# A process that deletes a file, adding it to an IContext, it requires injection from a previous process 
#    - specifically the DeletePersonsFile object and other dependencies supplied by the DoFramework.
# If instantiation is possible, the process will execute if prior successfull processes have been completed, in the correct order.
# Whilst executing it calls the DeletePersonsFile.Delete method which requests data from the IContext and removes the JSON file, if it exists.
# Logging is added to provide observability, use of "-silent" via the command line will suppress this
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
