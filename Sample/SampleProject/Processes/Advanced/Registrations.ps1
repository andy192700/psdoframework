using namespace DoFramework.Processing;
using namespace DoFramework.Services;
using module "..\..\Modules\CreatePersons.psm1";
using module "..\..\Modules\CreatePersonsFile.psm1";
using module "..\..\Modules\DeletePersonsFile.psm1";
using module "..\..\Modules\ReadPersonsFile.psm1";

# A process that registers some dependencies to Do's built in IServiceContainer object, which is injected like any other dependency.
# These types are registered, either for use later in the current process OR in a subsequent process where they are used via constructor injection.
class Registrations : Process {
    [IServiceContainer] $ServiceContainer;
    [IContext] $Context;

    Registrations(
        [IServiceContainer] $serviceContainer,
        [IContext] $context) {
        $this.ServiceContainer = $serviceContainer;
        $this.Context = $context;
    }

    [bool] Validate() {
        return $this.Context.Session.ProcessCount -gt 1;
    }

    [void] Run() {
        $this.ServiceContainer.RegisterService([CreatePersons]);
        $this.ServiceContainer.RegisterService([CreatePersonsFile]);
        $this.ServiceContainer.RegisterService([DeletePersonsFile]);
        $this.ServiceContainer.RegisterService([ReadPersonsFile]);

        [string] $currentDir = Get-Location;

        [string] $filePath = "$($currentDir)$([System.IO.Path]::DirectorySeparatorChar)persons.json";

        $this.Context.AddOrUpdate("PersonsFilePath", $filePath);
    }
}
