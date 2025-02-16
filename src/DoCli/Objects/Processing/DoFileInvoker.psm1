using namespace DoFramework.CLI;
using namespace DoFramework.FileSystem;
using namespace DoFramework.Logging;
using namespace DoFramework.Processing;
using namespace DoFramework.Validators;
using namespace DoFramework.Services;
using namespace System.Collections.Generic;

# Class responsible for invoking a specified target within a file.
# Implements the IDoFileInvoker interface, including functionality for validating, setting process locations, and executing a target.
class DoFileInvoker : IDoFileInvoker {
    [CLIFunctionParameters] $Parameters;
    [IValidator[string]] $Validator;
    [IValidationErrorWriter] $ValidationErrorWriter;
    [IFileManager] $FileManager;
    [ILogger] $Logger;
    [IReadProcessLocation] $ReadProcessLocation;
    [ISetProcessLocation] $SetProcessLocation;

    DoFileInvoker (
        [CLIFunctionParameters] $parameters,
        [IValidator[string]] $validator,
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

    # Method to invoke a specific target from a file, validating and setting required parameters before execution.
    [void] InvokeTarget([string] $target) {
        [Dictionary[string, object]] $Global:targets = [Dictionary[string, object]]::new();

        [char] $sep = [DoFramework.Environment.Environment]::Separator;

        [string] $currentDir = $this.ReadProcessLocation.Read();

        [string] $dofilePath = "$($currentDir)$($sep)dofile.ps1";

        if (!$this.FileManager.FileExists($dofilePath)) {
            $this.Logger.LogFatal("Could not locate 'dofile.ps1' in the current directory.");
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
                            [bool] $varExists = $false;

                            try {
                                Get-Variable -Name $key;

                                $varExists = $true;
                            }
                            catch {}

                            if ($varExists) {
                                Remove-Variable -Name $key;
                            }

                            New-Variable -Name $key -Value $this.Parameters.Parameters[$key];
                        }
                    }        
            
                    $Global:targets[$target].ToScriptBlock($Global:targets).Invoke();
                }
            }
            catch {
                $this.Logger.LogError("Error whilst attempting to execute target '$target'.");
                $this.Logger.LogError($_.Exception.Message);
            }

            $this.SetProcessLocation.Set($currentDir);
        }

        $Global:targets = $null;
    }
}
