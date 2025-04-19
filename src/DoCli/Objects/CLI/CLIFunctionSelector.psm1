using module "..\Services\CLIFunctionServiceContainer.psm1";
using module "..\Services\ApplicationServiceContainer.psm1";

using namespace System.Collections.Generic;
using namespace DoFramework.CLI;
using namespace DoFramework.Processing;
using namespace DoFramework.Mappers;
using namespace DoFramework.Validators;
using namespace DoFramework.Logging;
using namespace DoFramework.Services;

<#
.SYNOPSIS
Class for selecting and executing CLI functions within the DoFramework environment.

.DESCRIPTION
The CLIFunctionSelector class is designed to validate arguments, map them to the 
appropriate CLI function, and execute the function within the DoFramework environment.
It handles the setup of services, validation of arguments, and execution of selected functions.
#>
class CLIFunctionSelector {
    [CLIArgValidator] $CLIArgValidator;
    [ArgMapper] $ArgMapper;
    [List[object]] $CLIFunctions;
    [IValidationErrorWriter] $ValidationErrorWriter;
    [ILogger] $Logger;

    <#
    .SYNOPSIS
    Initializes a new instance of the CLIFunctionSelector class.

    .DESCRIPTION
    Constructor for the CLIFunctionSelector class, which sets up the necessary services 
    and validators for function selection and execution.
    #>
    CLIFunctionSelector() {
        [IServiceContainer] $Services = [CLIFunctionServiceContainer]::Create();

        $this.CLIArgValidator = $Services.GetService[CLIArgValidator]();  
        $this.ArgMapper = $Services.GetService[ArgMapper]();        
        $this.CLIFunctions = $Services.GetServicesByType[ICLIFunction]();        
        $this.ValidationErrorWriter = $Services.GetService[IValidationErrorWriter]();
        $this.Logger = $Services.GetService[ILogger]();
    }

    <#
    .SYNOPSIS
    Executes the specified CLI function with the provided arguments.

    .DESCRIPTION
    The Execute method validates the provided arguments, maps them to the appropriate 
    CLI function, and executes the function if the validation is successful.
    #>
    [object] Execute([object[]] $functionArgs) {
        [IValidationResult] $cliArgValidatorResult = $this.CLIArgValidator.Validate($functionArgs);

        $this.ValidationErrorWriter.Write($cliArgValidatorResult);

        [object] $outputValue = $null;

        if ($cliArgValidatorResult.IsValid) {
            [object[]] $remainingArgs = $null;

            if ($functionArgs.Length -gt 1) {
                $remainingArgs = $functionArgs[1..($functionArgs.Length - 1)];
            }
            else {
                $remainingArgs = @();
            }
            
            [Dictionary[string, object]] $dictionary = $this.ArgMapper.Map($remainingArgs);

            [string] $originalPath = (Get-Location);

            [string] $path = $originalPath;

            if ($dictionary.ContainsKey("home")) {
                $path = $dictionary["home"];

                if (!(Test-Path -Path $path)) {
                    throw "Requested project path '$($path)' does not exist.";
                }
            }

            try {
                Set-Location -Path $path;

                [string] $functionName = $functionArgs[0];

                [ICLIFunction] $func = $this.SelectFunction($functionName);

                if ($null -eq $func) {
                    $this.Logger.LogFatal("Doing does not have a function named '$functionName'");
                }
                else {
                    [IValidationResult] $result = $func.Validate($dictionary);

                    if ($result.IsValid) {
                        [IServiceContainer] $serviceContainer = [ApplicationServiceContainer]::Create();
                        
                        $outputValue = $func.Invoke($dictionary, $serviceContainer);
                    }
                    else {
                        $this.ValidationErrorWriter.Write($result);
                    }                    
                }
            }
            catch {
                Write-Host "Fatal Error whilst attempting to call doing - $($_)" -ForegroundColor Red;
            }
            finally {                
                Set-Location -Path $originalPath;
            }
        }

        return $outputValue;
    }

    <#
    .SYNOPSIS
    Selects the appropriate CLI function based on the function name.

    .DESCRIPTION
    The SelectFunction method iterates through the available CLI functions and 
    returns the function that matches the provided function name.
    #>
    [ICLIFunction] SelectFunction([string] $functionName) {
        foreach ($function in $this.CLIFunctions) {
            if ($function.Name -eq $functionName) {
                return $function;
            }
        }

        return $null;
    }
}
