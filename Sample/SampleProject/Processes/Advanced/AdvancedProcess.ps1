using namespace DoFramework.Processing;

# Entry point process for this sample project's advanced example, demonstrating Do's capabilities.
# A IContext is injected for accessing shared values, a IProcessDispatcher is injected to allow for the execution of other processes by this current process
# This example checks that all context values are present, and if so it fires off a sequence of processes, executed in series.
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
