class VersionCalculator {
    static [string] Calculate([string] $psNuGetSourceName) {
        [string] $highestReadMeVersion = [VersionCalculator]::GetLatestReadMe().ToString();

        [string] $latestModuleVersion = "1.1.0";

        [PSCustomObject] $module = Find-Module -Name "PSDoFramework" -Repository $psNuGetSourceName -ErrorAction SilentlyContinue;

        if ($null -ne $module) {
            [string] $latestModuleVersion = $module.Version;

            [string[]] $parts = $latestModuleVersion.Split(".");

            $latestModuleVersion = [System.Version]::Parse("$($parts[0]).$($parts[1]).$(([int]::Parse($parts[2])) + 1)")
        }

        [string] $newVersion = $latestModuleVersion;

        $latestParts = $latestModuleVersion.Split('.');
        $highestParts = $highestReadMeVersion.Split('.');

        for ($i = 0; $i -lt 3; $i++) {
            if ([int]$latestParts[$i] -gt [int]$highestParts[$i]) {
                $newVersion = $latestModuleVersion;
                break;
            } 
            elseif ([int]$latestParts[$i] -lt [int]$highestParts[$i]) {
                $newVersion = $highestReadMeVersion;
                break;
            }
        }

        Write-Host $latestModuleVersion $highestReadMeVersion $newVersion; 

        return $newVersion.ToString();
    }

    static [string] GetLatest([string] $psNuGetSourceName) {
        [PSCustomObject] $module = Find-Module -Name "PSDoFramework" -Repository $psNuGetSourceName -ErrorAction SilentlyContinue;

        [string] $version = "1.1.0";

        if ($null -ne $module) {
            return $module.Version;
        }

        return $version;
    }

    static [string] GetCurrent() {
        Import-Module .\Build\PSDoFramework\PSDoFramework.psd1 -Force;

        [PSCustomObject] $module = Get-Module -Name "PSDoFramework";

        return $module.Version;
    }

    static [System.Version] GetLatestReadMe() {
        [char] $sep = [System.IO.Path]::DirectorySeparatorChar;
        [string] $releaseNotesDir = "$(Get-Location)$($sep)Documentation$($sep)ReleaseNotes";

        [System.Version] $highestReadMeVersion = Get-ChildItem -Path $releaseNotesDir | ForEach-Object {
            [System.Version]::Parse($_.Name.Replace($_.Extension, [string]::Empty).Substring(1))
        } | Sort-Object -Descending | Select-Object -First 1;

        return $highestReadMeVersion;
    }
}