using namespace DoFramework.Environment;
using namespace DoFramework.Data;
using namespace DoFramework.Domain;
using namespace DoFramework.Testing;
using namespace DoFramework.Services;
using namespace System;
using namespace System.Text;
using namespace System.Reflection;
using namespace System.Collections.Generic;

<#
.SYNOPSIS
Class for building proxy class type definitions within the DoFramework environment.

.DESCRIPTION
The ProxyClassTypeDefinitionBuilder class is designed to dynamically build proxy 
class types based on the provided type and constructor arguments. It handles 
importing necessary modules, generating constructors, and overriding methods.
#>
class ProxyClassTypeDefinitionBuilder {
    <#
    .SYNOPSIS
    Initializes a new instance of the ProxyClassTypeDefinitionBuilder class.

    .DESCRIPTION
    Constructor for the ProxyClassTypeDefinitionBuilder class, which sets up the 
    environment and module provider for building proxy class type definitions.
    #>
    [IEnvironment] $Environment;
    [IDataCollectionProvider[ModuleDescriptor, string]] $ModuleProvider;

    ProxyClassTypeDefinitionBuilder(
        [IEnvironment] $environment,
        [IDataCollectionProvider[ModuleDescriptor, string]] $moduleProvider
    ) {
        $this.Environment = $environment;
        $this.ModuleProvider = $moduleProvider;
    }

    <#
    .SYNOPSIS
    Builds a proxy class type definition based on the provided type and constructor arguments.

    .DESCRIPTION
    The Build method generates a proxy class type definition, including importing 
    necessary modules, creating constructors, and overriding methods.
    #>
    [Type] Build([Type] $type, [object[]] $constructorArgs) {
        [MethodInfo[]] $methods = $type.GetMethods();

        [StringBuilder] $sb = [StringBuilder]::new();

        [List[ModuleDescriptor]] $content = $this.ModuleProvider.Provide(".*");
        
        foreach ($descriptor in $content) {
            $sb.AppendLine("using module $($this.Environment.ModuleDir)\$($descriptor.Path);");
        }
        
        [string] $className = "$($type.Name)Proxy";

        $sb.AppendLine("class $className : $($this.CleanTypeName($type.FullName)) {");

        $sb.AppendLine("[$($this.CleanTypeName([ClassProxy].FullName))] `$Proxy;");

        [ConstructorInfo[]] $constructors = $type.GetConstructors();

        foreach ($constructor in $constructors) {
            $this.AppendConstructor($className, $constructor, $sb);
        }

        foreach ($method in $methods) {
            if ((!$method.IsStatic `
            -and !$method.IsConstructor `
            -and !$method.IsGenericMethod `
            -and ($method.IsAbstract -or $method.IsVirtual) `
            -and $method.DeclaringType -ne [Object]) `
            -or ($method.IsSpecialName `
                -and ($method.Name.StartsWith("get_") `
                -or $method.Name.StartsWith("set_")))) {
                $this.AppendMethod($method, $sb);
            }
        }

        $sb.AppendLine("}");

        $sb.AppendLine("return [$className]");

        [Type] $type = Invoke-Expression -Command $sb.ToString();

        return $type;
    }

    <#
    .SYNOPSIS
    Appends a constructor definition to the proxy class.

    .DESCRIPTION
    The AppendConstructor method generates a constructor definition for the proxy 
    class, including the proxy parameter and base class initialization.
    #>
    [void] AppendConstructor([string] $className, [ConstructorInfo] $constructor, [StringBuilder] $sb) {
        $sb.AppendLine("$className(");

        $sb.Append("[$($this.CleanTypeName([ClassProxy].FullName))] `$proxy");

        [StringBuilder] $signatureParams = [StringBuilder]::new();
        [StringBuilder] $baseCall = [StringBuilder]::new();

        [ParameterInfo[]] $params = $constructor.GetParameters();

        for ([int] $i = 0; $i -lt $params.Length; $i++) {
            $signatureParams.Append(", [$($this.CleanTypeName($params[$i].ParameterType.FullName))] `$arg$i");
            
            $baseCall.Append("`$arg$i");

            if ($i -lt $params.Length - 1) {
                $baseCall.Append(", ");
            }
        }

        $sb.Append($signatureParams.ToString());

        $sb.Append(") : base($($baseCall.ToString())) ");
        
        $sb.AppendLine("{ `$this.Proxy = `$proxy; }");
    }

    <#
    .SYNOPSIS
    Appends a method definition to the proxy class.

    .DESCRIPTION
    The AppendMethod method generates a method definition for the proxy class, 
    including the method signature, parameters, and proxy invocation.
    #>
    [void] AppendMethod([MethodInfo] $method, [StringBuilder] $sb) {
        $parameters = $method.GetParameters();

        [string] $paramSignatureString = [string]::Empty;
        [string] $paramDefinitionString = [string]::Empty;
        [string] $suppliedParamString = [string]::Empty;

        if ($method.IsSpecialName -and $method.Name.StartsWith("set_")) {
            $paramSignatureString += "[$($this.CleanTypeName($parameters[0].ParameterType.FullName))] `$value";

            $paramDefinitionString += "-value ([$($this.CleanTypeName($parameters[0].ParameterType.FullName))]) ";

            $suppliedParamString += "`$value";
        }
        else {
            for ($i = 0; $i -lt $parameters.Length; $i++) {
                $paramSignatureString += "[$($this.CleanTypeName($parameters[$i].ParameterType.FullName))] `$$($parameters[$i].Name)";

                $paramDefinitionString += "-$($parameters[$i].Name) ([$($this.CleanTypeName($parameters[$i].ParameterType.FullName))]) ";

                $suppliedParamString += "`$$($parameters[$i].Name)";

                if ($i -lt $parameters.Length - 1) {
                    $paramSignatureString += ",";
                    $suppliedParamString += ",";
                }
            }
        }
        
        $sb.AppendLine("[$($this.CleanTypeName($method.ReturnType.FullName))] $($method.Name)($paramSignatureString) {");

        if ($method.ReturnType -eq [Void]) {
            $sb.AppendLine("`$this.Proxy.Invoke((doing get-methodinfo -methodName $($method.Name) -type (`$this.GetType().BaseType) -parameters (doing args $paramDefinitionString)), @( $suppliedParamString ));");
        }
        else {
            $sb.AppendLine("return `$this.Proxy.Invoke((doing get-methodinfo -methodName $($method.Name) -type (`$this.GetType().BaseType) -parameters (doing args $paramDefinitionString)), @( $suppliedParamString ));");
        }

        $sb.AppendLine("}");
    }
    
    <#
    .SYNOPSIS
    Cleans a Type's fullname.

    .DESCRIPTION
    Removes extra information relating to a Type's true full name so it presents as if it were written in code
    E.g. [System.Collections.Generic.Dictionary`2[[System.String, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089],[System.String, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089]]]
    becomes [System.Collections.Generic.Dictionary[[System.String],[System.String]]]
    #>
    [string] CleanTypeName([string] $typeName) {
        return $typeName -replace ', [^]]+', ([string]::Empty) -replace '`\d+', ([string]::Empty)
    }
}
