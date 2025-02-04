using namespace DoFramework.Domain;
using namespace DoFramework.Logging;
using namespace DoFramework.Processing;
using namespace System.Collections.Generic;
using module "..\..\Modules\CreatePersons.psm1";

# A process that creates data, adding it to an IContext, it requires injection from a previous process 
#    - specifically the CreatePersons object and other dependencies supplied by the DoFramework.
# If instantiation is possible, the process will execute if prior successfull processes have been completed, in the correct order.
# Whilst executing it calls the CreatePersons.Create method which appends data to the IContext.
# Logging is added to provide observability, use of "-silent" via the command line will suppress this
class CreateData : Process {
    [CreatePersons] $CreatePersons;
    [ILogger] $Logger;
    [IContext] $Context;

    CreateData(
        [CreatePersons] $createPersons, 
        [ILogger] $logger,
        [IContext] $context) {
        $this.CreatePersons = $createPersons;
        $this.Logger = $logger;
        $this.Context = $context;
    }

    [bool] Validate() {
        return $this.Context.Requires().
            ProcessSucceeded("DeleteFile").
            Verify();
    }

    [void] Run() {
        $this.Logger.LogInfo("Creating persons data...");

        $this.CreatePersons.Create();
        
        $this.Logger.LogInfo("Created persons data...");
    }
}
