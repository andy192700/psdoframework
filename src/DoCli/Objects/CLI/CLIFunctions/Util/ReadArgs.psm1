using namespace DoFramework.CLI;
using namespace DoFramework.Validators;
using namespace DoFramework.Services;
using namespace System.Collections.Generic;

<#
.SYNOPSIS
Class for reading arguments within the DoFramework environment.

.DESCRIPTION
The ReadArgs class is designed to read and return arguments within the 
DoFramework environment. It handles the setup of parameters and returns 
the provided arguments.
#>
class ReadArgs : CLIFunction[EmptyCLIFunctionDictionaryValidator, [Dictionary[string, object]]] {
    <#
    .SYNOPSIS
    Initializes a new instance of the ReadArgs class.

    .DESCRIPTION
    Constructor for the ReadArgs class, which sets up the base name 
    for the command as "Read-Args".
    #>
    ReadArgs() : base("Read-Args") {}

    <#
    .SYNOPSIS
    Invokes the process of reading and returning arguments.

    .DESCRIPTION
    The Invoke method sets up parameters and returns the provided arguments.
    #>
    [Dictionary[string, object]] Invoke([Dictionary[string, object]] $params, [IServiceContainer] $serviceContainer) {
        return $params;
    }
}
