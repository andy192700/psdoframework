using namespace DoFramework.Processing;

# Class that uses data from the IContext to delete a file of persons
class DeletePersonsFile {
    [IContext] $Context;

    DeletePersonsFile([IContext] $context) {
        $this.Context = $context;
    }
    
    [void] Delete() {
        [string] $filePath = $this.Context.Get("PersonsFilePath");

        if ((Test-Path -Path $filePath)) {
            Remove-Item -Path $filePath -Force | Out-Null;
        }
    }
}
