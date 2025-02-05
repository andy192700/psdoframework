using namespace DoFramework.Processing;

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
