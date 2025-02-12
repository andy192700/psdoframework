using namespace DoFramework.CLI;
using namespace DoFramework.FileSystem;
using namespace DoFramework.Logging;
using namespace DoFramework.Processing;
using namespace DoFramework.Validators;
using namespace DoFramework.Services;
using namespace System.Collections.Generic;
using module ".\DoFileTarget.psm1";
using module "..\Validators\DoFileTargetValidator.psm1";

class DoFileInvoker : IDoFileInvoker {
    [CLIFunctionParameters] $Parameters;
    [DoFileTargetValidator] $Validator;
    [IValidationErrorWriter] $ValidationErrorWriter;
    [IFileManager] $FileManager;
    [ILogger] $Logger;
    [IReadProcessLocation] $ReadProcessLocation;
    [ISetProcessLocation] $SetProcessLocation;

    DoFileInvoker (
        [CLIFunctionParameters] $parameters,
        [DoFileTargetValidator] $validator,
        [IValidationErrorWriter] $validationErrorWriter,
        [IFileManager] $fileManager,
        [ILogger] $logger,
        [IReadProcessLocation] $readProcessLocation,
        [ISetProcessLocation] $setProcessLocation
    ) {
        $this.Parameters = $parameters;
        $this.Validator = $validator
        $this.ValidationErrorWriter = $validationErrorWriter
        $this.FileManager = $fileManager
        $this.Logger = $logger
        $this.ReadProcessLocation = $readProcessLocation
        $this.SetProcessLocation = $setProcessLocation
    }

    [void] InvokeTarget([string] $target) {
        [Dictionary[string, DoFileTarget]] $Global:targets = [Dictionary[string, DoFileTarget]]::new();

        [char] $sep = [DoFramework.Environment.Environment]::Separator;

        [string] $currentDir = $this.ReadProcessLocation.Read();

        [string] $dofilePath = "$($currentDir)$($sep)dofile.ps1";

        if (!$this.FileManager.FileExists($dofilePath)) {
            $this.Logger.LogFatal("Could not locate 'dofile.ps1' in the current directory");
        }
        else {
            try {
                . $dofilePath;

                [IValidationResult] $result = $this.Validator.Validate($target);

                if (!$result.IsValid) {
                    $this.ValidationErrorWriter.Write($result);
                }
                else {
                    foreach ($key in $this.Parameters.Parameters.Keys) {
                        if ($key -ne "target") {
                            Set-Variable -Name $key -Value $this.Parameters.Parameters[$key];
                        }
                    }        
            
                    $Global:targets[$target].ToScriptBlock($Global:targets).Invoke();
                }
            }
            catch {
                $this.Logger.LogError("Error whilst attempting to execute target '$target'");
                $this.Logger.LogError($_.Exception.Message);
            }

            $this.SetProcessLocation.Set($currentDir);
        }

        $Global:targets = $null;
    }
}
