using namespace DoFramework.Domain;
using namespace DoFramework.Logging;
using namespace DoFramework.Processing;
using namespace System.Collections.Generic;
using module "..\..\Modules\CreatePersons.psm1";

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
