using namespace DoFramework.Domain;
using namespace DoFramework.Processing;
using namespace System.Collections.Generic;

<#
.SYNOPSIS
Class for displaying process reports within the DoFramework environment.

.DESCRIPTION
The DisplayReports class is designed to display detailed process reports 
within the DoFramework environment. It sorts the reports and formats 
them for clear output.
#>
class DisplayReports : IDisplayReports {
    <#
    .SYNOPSIS
    Displays the process reports.

    .DESCRIPTION
    The Display method sorts the process reports, formats them into a table, 
    and outputs the table to the host.
    #>
    [void] Display([List[ProcessReport]] $processReports) {
        $processReports 
            | Sort-Object { [string]::IsNullOrEmpty($_.StartTime) }, StartTime 
            | Format-Table Name, ProcessResult, StartTime, EndTime, @{Label="Duration (s)"; Expression={$_.Duration}}
            | Out-Host;
    }
}
