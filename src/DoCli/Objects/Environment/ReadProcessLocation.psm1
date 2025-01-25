using namespace DoFramework.FileSystem;
using namespace System.Management.Automation;

<#
.SYNOPSIS
Class for reading the current process location within the DoFramework environment.

.DESCRIPTION
The ReadProcessLocation class is designed to return the current directory location 
of the process within the DoFramework environment.
#>
class ReadProcessLocation : IReadProcessLocation {
    <#
    .SYNOPSIS
    Reads and returns the current process location.

    .DESCRIPTION
    The Read method retrieves and returns the current directory location 
    of the process.
    #>
    [string] Read() {
        return Get-Location;
    }
}
