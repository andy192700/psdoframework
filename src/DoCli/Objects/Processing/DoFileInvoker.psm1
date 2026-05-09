using namespace DoFramework.CLI;
using namespace DoFramework.FileSystem;
using namespace DoFramework.Logging;
using namespace DoFramework.Processing;
using namespace DoFramework.Validators;
using namespace DoFramework.Services;
using namespace System.Collections.Generic;
using namespace System.Management.Automation;
using namespace System.Management.Automation.Runspace;
using namespace System.Text;

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
    [IPowerShellRunner] $PSRunner;

    DoFileInvoker (
        [CLIFunctionParameters] $parameters,
        [IValidator[string]] $validator,
        [IValidationErrorWriter] $validationErrorWriter,
        [IFileManager] $fileManager,
        [ILogger] $logger,
        [IReadProcessLocation] $readProcessLocation,
        [ISetProcessLocation] $setProcessLocation,
        [IPowerShellRunner] $psRunner
    ) {
        $this.Parameters = $parameters;
        $this.Validator = $validator
        $this.ValidationErrorWriter = $validationErrorWriter
        $this.FileManager = $fileManager
        $this.Logger = $logger
        $this.ReadProcessLocation = $readProcessLocation
        $this.SetProcessLocation = $setProcessLocation
        $this.PSRunner = $psRunner;
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
                    [bool] $disableReload = $this.Parameters.ParseSwitch("disableReload");

                    if ($disableReload) {
                        foreach ($key in $this.Parameters.Parameters.Keys) {
                            if ($key -ne "target") {
                                New-Variable -Name $key -Value $this.Parameters.Parameters[$key] -Force;
                            }
                        }      
  
                        $Global:targets[$target].ToScriptBlock($Global:targets).Invoke();
                    }
                    else {
                        [string] $psProfile = $null;

                        $profileVar = Get-Variable PROFILE -Scope Global -ErrorAction SilentlyContinue;

                        if ($profileVar -and $profileVar.Value) {
                            $psProfile = $profileVar.Value.CurrentUserCurrentHost;
                        }

                        [string] $psScript = $this.GenerateDoFilePSScript($dofilePath, $target);
                        [string] $psExecPolicy = (Get-ExecutionPolicy)

                        $this.PSRunner.Run($psScript, $psExecPolicy, $psProfile);
                    }
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

    [string] GenerateDoFilePSScript([string] $doFilePath, [string] $target) {
        [StringBuilder] $sb = [StringBuilder]::new();

        $sb.AppendLine("[System.Collections.Generic.Dictionary[string, object]] `$Global:targets = [System.Collections.Generic.Dictionary[string, object]]::new();");

        $sb.AppendLine(". $doFilePath");

        foreach ($key in $this.Parameters.Parameters.Keys) {
            if ($key -ne "target") {
                $sb.AppendLine("New-Variable -Name `"$key`" -Value $($this.Parameters.Parameters[$key]) -Force")
            }
        }

        $sb.AppendLine($Global:targets[$target].ToScriptBlock($Global:targets).ToString());

        return $sb.ToString();
    } 
}
