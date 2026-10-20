using namespace DoFramework.Testing;
using namespace System;
using namespace System.Collections.Generic;
using namespace System.Management.Automation;
using namespace System.Reflection;
using namespace System.Text;

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
    Builds a proxy class type definition based on the provided type and constructor arguments.

    .DESCRIPTION
    The Build method generates a proxy class type definition, including importing 
    necessary modules, creating constructors, and overriding methods.
    #>
    [Type] Build([Type] $type, [object[]] $constructorArgs) {
        [StringBuilder] $sb = [StringBuilder]::new();

        [List[string]] $modules = $this.DeriveModules($type);
        
        foreach ($module in $modules) {
            if ($module.Trim().Length -gt 0) {
                $sb.AppendLine("using module '$($module)';");
            }
        }
        
        [string] $className = "$($type.Name)Proxy";

        $sb.AppendLine("class $className : $($this.CleanTypeName($type.FullName)) {");

        $sb.AppendLine("[$($this.CleanTypeName([ClassProxy].FullName))] `$Proxy;");

        [ConstructorInfo[]] $constructors = $type.GetConstructors();

        foreach ($constructor in $constructors) {
            $this.AppendConstructor($className, $constructor, $sb);
        }
        
        [MethodInfo[]] $methods = $type.GetMethods();

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

        $accelerators = [PowerShell].Assembly.GetType("System.Management.Automation.TypeAccelerators");

        $accelerators::Add($type.FullName, $type);

        return [Type](Invoke-Expression -Command $sb.ToString());
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
        return $typeName -replace ', [^]]+', ([string]::Empty) -replace '`\d+', ([string]::Empty) -replace '^<[^>]+>\.', ([string]::Empty);
    }

    [List[string]] DeriveModules([Type] $type) {
        [List[string]] $modulePaths = [List[string]]::new();

        # lead type
        $modulePaths.AddRange($this.DeriveModuleFromType($type));

        # constructor param types
        [ConstructorInfo[]] $constructors = $type.GetConstructors();

        foreach ($constructor in $constructors) {
            [ParameterInfo[]] $params = $constructor.GetParameters();

            for ([int] $i = 0; $i -lt $params.Length; $i++) {     
                $modulePaths.AddRange($this.DeriveModuleFromType($params[$i].ParameterType));
            }
        }

        # method param types
        
        [MethodInfo[]] $methods = $type.GetMethods();

        foreach ($method in $methods) {
            $parameters = $method.GetParameters();

            for ($i = 0; $i -lt $parameters.Length; $i++) {
                $modulePaths.AddRange($this.DeriveModuleFromType($parameters[$i].ParameterType));
            }

            $modulePaths.AddRange($this.DeriveModuleFromType($method.ReturnType));
        }

        return [List[string]]($modulePaths | Select-Object -Unique)
    }

    [List[string]] DeriveModuleFromType([Type] $type) {
        [List[string]] $modules = [List[string]]::new();

        [DynamicClassImplementationAssemblyAttribute] $attr = $type.Module.Assembly.GetCustomAttributes($true) | Where-Object {
            $_ -is [DynamicClassImplementationAssemblyAttribute]
        } | Select-Object -First 1;

        if ($null -ne $attr) {
            if ($null -ne $attr.ScriptFile) {
                [string] $moduleString = $attr.ScriptFile.ToString();

                if (
                    $moduleString.Trim().Length -gt 0 -and
                    [System.IO.File]::Exists($moduleString) -and
                    [System.IO.Path]::GetExtension($moduleString) -ieq ".psm1"
                ) {
                    $modules.Add($moduleString);
                }
            }
        }

        # arrays
        if ($type.IsArray) {
            $modules.AddRange($this.DeriveModuleFromType($type.GetElementType()));
        }

        # generics
        if ($type.IsGenericType) {
            [Type[]] $genericArgs = $type.GetGenericArguments();

            foreach ($genericArg in $genericArgs) {
                $modules.AddRange($this.DeriveModuleFromType($genericArg));
            }
        }

        return $modules;
    } 
}
