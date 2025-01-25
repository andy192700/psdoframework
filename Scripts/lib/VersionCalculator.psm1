class VersionCalculator {
    static [string] Calculate([string] $psNuGetSourceName) {
        [PSCustomObject] $module = Find-Module -Name "PSDoFramework" -Repository $psNuGetSourceName -ErrorAction SilentlyContinue;

        [string] $version = "1.1.0";

        if ($null -ne $module) {
            [string] $latestVersion = $module.Version;

            [string[]] $parts = $latestVersion.Split(".");

            $version = "$($parts[0]).$($parts[1]).$(([int]::Parse($parts[2])) + 1)"
        }

        return $version;
    }
}