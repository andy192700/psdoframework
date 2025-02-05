using namespace DoFramework.Logging;
using namespace DoFramework.Processing;
using namespace System.Collections.Generic;
using module "..\..\Modules\Models\Person.psm1";
using module "..\..\Modules\ReadPersonsFile.psm1";

class ReadFile : Process {
    [ReadPersonsFile] $ReadPersonsFile;
    [ILogger] $Logger;
    [IContext] $Context;

    ReadFile(
        [ReadPersonsFile] $readPersonsFile, 
        [ILogger] $logger,
        [IContext] $context) {
        $this.ReadPersonsFile = $readPersonsFile;
        $this.Logger = $logger;
        $this.Context = $context;
    }

    [bool] Validate() {
        return $this.Context.Requires().
            ConfirmKey("PersonsFilePath").
            ProcessSucceeded("CreateFile").
            Verify();
    }

    [void] Run() {
        $this.Logger.LogInfo("Reading persons file...");

        [List[Person]] $persons = $this.ReadPersonsFile.Read();

        $this.Logger.LogInfo("Found $($persons.Count) persons.");

        $this.Logger.LogInfo("Here they are:");

        foreach ($person in $persons) {
            $this.Logger.LogInfo($person.ToString());
        }
    }
}
