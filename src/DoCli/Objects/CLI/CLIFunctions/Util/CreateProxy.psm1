using module "..\..\..\Validators\ProxyTypeValidator.psm1";
using module "..\..\..\Testing\ProxyClassTypeDefinitionBuilder.psm1";

using namespace DoFramework.CLI;
using namespace DoFramework.Validators;
using namespace DoFramework.Testing;
using namespace System;
using namespace System.Collections.Generic;
using namespace DoFramework.Services;

<#
.SYNOPSIS
Class for creating proxies within the DoFramework environment.

.DESCRIPTION
The CreateProxy class is designed to create proxies for specified types within 
the DoFramework environment. It handles the setup of parameters, validation of 
types, and creation of proxy instances.
#>
class CreateProxy : CLIFunction[EmptyCLIFunctionDictionaryValidator, ProxyResult] {
    <#
    .SYNOPSIS
    Initializes a new instance of the CreateProxy class.

    .DESCRIPTION
    Constructor for the CreateProxy class, which sets up the base name 
    for the command as "Create-Proxy".
    #>
    CreateProxy() : base("Create-Proxy") {}

    <#
    .SYNOPSIS
    Invokes the process of creating a proxy.

    .DESCRIPTION
    The Invoke method sets up parameters, validates the base type, and creates 
    a proxy instance if the validation is successful. If the base type is an 
    interface, it creates an interface proxy; otherwise, it builds a class proxy.
    #>
    [ProxyResult] Invoke([Dictionary[string, object]] $params, [IServiceContainer] $serviceContainer) {
        [ServiceContainerExtensions]::AddParameters($serviceContainer, $params);

        [Type] $baseType = $params["type"];

        [object[]] $constructorArgs = @();

        if ($params.ContainsKey("params")) {
            $constructorArgs = $params["params"];
        }

        [ProxyTypeValidator] $validator = $serviceContainer.GetService[ProxyTypeValidator]();

        [IValidationResult] $result = $validator.Validate($baseType);

        if (!$result.IsValid) {
            [IValidationErrorWriter] $validationErrorWriter = $serviceContainer.GetService[IValidationErrorWriter]();

            $validationErrorWriter.Write($result);

            throw;
        }
        else {
            if ($baseType.IsInterface) {
                return [ProxyFactory]::CreateProxy($baseType);
            }
            
            [ProxyClassTypeDefinitionBuilder] $proxyTypeBuilder = $serviceContainer.GetService[ProxyClassTypeDefinitionBuilder]();
            
            [Type] $type = $proxyTypeBuilder.Build($baseType, $constructorArgs);

            [ClassProxy] $proxy = [ClassProxy]::new();

            [IObjectBuilder] $objectBuilder = [ObjectBuilder]::new();

            [List[object]] $buildArgs = [List[object]]::new();

            $buildArgs.Add($proxy);
            $buildArgs.AddRange($constructorArgs);

            [object] $obj = $objectBuilder.BuildObject($type, $buildArgs);

            return [ProxyFactory]::CreateClassProxy($proxy, $obj);
        }
    }
}
