using namespace DoFramework.Processing;

class AdvancedProcess : Process {
    [IContext] $Context;

    AdvancedProcess(
        [IContext] $context) {
        $this.Context = $context;
    }

    [bool] Validate() {
        return $this.Context.Requires().
            ComposedBy("AdvancedComposer").
            ConfirmKey("Person1FirstName").
            ConfirmKey("Person1LastName").
            ConfirmKey("Person1Age").
            ConfirmKey("Person2FirstName").
            ConfirmKey("Person2LastName").
            ConfirmKey("Person2Age").
            ConfirmKey("Person3FirstName").
            ConfirmKey("Person3LastName").
            ConfirmKey("Person3Age").
            Verify();
    }

    [void] Run() {
        [string] $currentDir = Get-Location;

        [string] $filePath = "$($currentDir)$([System.IO.Path]::DirectorySeparatorChar)persons.json";

        $this.Context.AddOrUpdate("PersonsFilePath", $filePath);
    }
}
