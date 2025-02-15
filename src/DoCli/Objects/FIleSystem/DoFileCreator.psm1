using namespace DoFramework.FileSystem;
using namespace DoFramework.Logging;

class DoFileCreator : IDoFileCreator {
    [IFileManager] $FileManager;
    [ILogger] $Logger;
    [IReadProcessLocation] $ReadProcessLocation;

    DoFileCreator(
        [IFileManager] $fileManager,
        [ILogger] $logger,
        [IReadProcessLocation] $readProcessLocation) {
        $this.FileManager = $fileManager;
        $this.ReadProcessLocation = $readProcessLocation;
        $this.Logger = $logger;
    }

    [void] Create() {        
        [char] $sep = [DoFramework.Environment.Environment]::Separator;

        [string] $currentDir = $this.ReadProcessLocation.Read();

        [string] $dofilePath = "$($currentDir)$($sep)dofile.ps1";

        if ($this.FileManager.FileExists($dofilePath)) {
            $this.Logger.LogFatal("'dofile.ps1' already exists in the current directory.");
        }
        else {
            [string] $content = @'
$myVar = "hello world!!!";
$theBool = $false;

Target A {
    if ($theBool) {
        Write-Host "boolset";
    }

    Write-Host $myVar $theBool;
}

Target B -inherits A {
    Write-Host "the end";
}

Target C {}
'@;

            Set-Content -Path $dofilePath -Value $content;

            $this.Logger.LogInfo("Successfully created 'dofile.ps1.' in the current directory.");
        }
    }
}