using namespace DoFramework.Logging;
using namespace DoFramework.Processing;
using namespace System.Collections.Generic;
using module "..\..\Modules\Models\Person.psm1";
using module "..\..\Modules\ReadPersonsFile.psm1";

# A process that reads a file, adding it to an IContext, it requires injection from a previous process 
#    - specifically the ReadPersonsFile object and other dependencies supplied by the DoFramework.
# If instantiation is possible, the process will execute if prior successfull processes have been completed, in the correct order.
# Whilst executing it calls the ReadPersonsFile.Read method which requests data from the IContext and reads the JSON file storing the data, returning a List.
# Logging is added to provide observability, use of "-silent" via the command line will suppress this
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
        return $this.Context.Session.ProcessReports.Count -eq 4 `
            -and $this.Context.Session.ProcessReports[0].Descriptor.Name -eq "Registrations" `
            -and $this.Context.Session.ProcessReports[1].Descriptor.Name -eq "DeleteFile" `
            -and $this.Context.Session.ProcessReports[2].Descriptor.Name -eq "CreateData" `
            -and $this.Context.Session.ProcessReports[3].Descriptor.Name -eq "CreateFile" `
            -and $this.Context.KeyExists("PersonsFilePath");
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
