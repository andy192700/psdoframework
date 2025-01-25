using namespace System.Reflection;
using namespace DoFramework.Mappers;
using namespace DoFramework.Testing;

<#
.SYNOPSIS
Class for mapping run method information within the DoFramework environment.

.DESCRIPTION
The RunMethodInfoMapper class is designed to map method information from a source 
object to a PowershellMethodInfo instance within the DoFramework environment.
#>
class RunMethodInfoMapper : IMapper[object, MethodInfo] {
    <#
    .SYNOPSIS
    Maps method information from a source object to a MethodInfo instance.

    .DESCRIPTION
    The Map method takes a source object and maps its method information, including 
    name, parameters, and return type, to a PowershellMethodInfo instance.
    #>
    [MethodInfo] Map([object] $source) {
        return [PowershellMethodInfo]::new($source.Name, $source.GetParameters(), $source.ReturnType);
    }
}
