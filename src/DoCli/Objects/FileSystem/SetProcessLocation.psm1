using namespace DoFramework.FileSystem;

<#
.SYNOPSIS
Class for setting the current process location within the DoFramework environment.

.DESCRIPTION
The SetProcessLocation class is designed to change the current directory location 
of the process within the DoFramework environment.
#>
class SetProcessLocation : ISetProcessLocation {
    <#
    .SYNOPSIS
    Sets the current process location to the specified directory.

    .DESCRIPTION
    The Set method changes the current directory location of the process 
    to the specified path.
    #>
    [void] Set([string] $location) {
        Set-Location $location;
    }
}
