using module "..\..\..\Mappers\RunMethodInfoMapper.psm1";

using namespace DoFramework.CLI;
using namespace DoFramework.Mappers;
using namespace DoFramework.Services;
using namespace DoFramework.Validators;
using namespace System.Reflection;
using namespace System.Collections.Generic;

<#
.SYNOPSIS
Class for retrieving method information within the DoFramework environment.

.DESCRIPTION
The GetMethodInfo class is designed to retrieve detailed information about methods 
within a specified type in the DoFramework environment. It handles the setup of 
parameters, validation of the method name, and extraction of method details.
#>
class GetMethodInfo : CLIFunction[GetRunMethodInfoDictionaryValidator, MethodInfo] {
    <#
    .SYNOPSIS
    Initializes a new instance of the GetMethodInfo class.

    .DESCRIPTION
    Constructor for the GetMethodInfo class, which sets up the base name 
    for the command as "Get-MethodInfo".
    #>
    GetMethodInfo() : base("Get-MethodInfo") {}

    <#
    .SYNOPSIS
    Invokes the process of retrieving method information.

    .DESCRIPTION
    The Invoke method sets up parameters, validates the specified type and method name, 
    and retrieves the detailed information about the method if the validation is successful.
    #>
    [MethodInfo] Invoke([Dictionary[string, object]] $params, [IServiceContainer] $serviceContainer) {
        [ServiceContainerExtensions]::AddParameters($serviceContainer, $params);

        [Type] $type = $params["type"];

        [string] $methodName = $params["methodName"];
        
        [Dictionary[string, object]] $parameters = $params.ContainsKey("parameters") ? $params["parameters"] : [Dictionary[string, object]]::new();

        [object[]] $methods = $type.GetMethods();

        [IMapper[object, MethodInfo]] $mapper = $serviceContainer.GetService[IMapper[object, MethodInfo]]();

        [MethodInfo] $methodInfo = $null;

        foreach ($method in $methods) {
            if ($method.Name -eq $methodName) {
                [MethodInfo] $result =  $mapper.Map($method);

                [ParameterInfo[]] $methodParameters = $method.GetParameters();

                if ($parameters.Count -eq $methodParameters.Length) {
                    [int] $matchingCount = 0;

                    [int] $i = 0;

                    foreach ($key in $parameters.keys) {

                        [ParameterInfo] $p = $methodParameters[$i];

                        if ($key -eq $p.Name -and $parameters[$key] -eq $p.ParameterType) {
                            $matchingCount++;
                        }

                        $i++;
                    }

                    if ($matchingCount -eq $methodParameters.Length) {
                        $methodInfo = $result;

                        break;
                    }
                }
            }
        }

        if ($null -eq $methodInfo) {
            throw "$($type.FullName) does not have a method named $methodName";
        }

        return $methodInfo;
    }
}
