using namespace System.Collections.Generic

# DoFramework module dependency install script, this exists so users do not need to ensure they have required dependencies installed.

# Required modules
[Dictionary[string, string]] $requiredModules = [Dictionary[string, string]]::new();
$requiredModules["Pester"] = "5.7.1";

# Install required modules
foreach ($moduleName in $requiredModules.Keys) {
    [string] $moduleVersion = $requiredModules[$moduleName];

    [object[]] $moduleJson = Get-Module -ListAvailable -Name $moduleName | select-object Version;

    [PSCustomObject] $capturedVersion = ($moduleJson | Where-Object {
        return $_.Version -eq $moduleVersion;
    });

    if ($null -eq $capturedVersion) {
        Install-Module -Name $moduleName -RequiredVersion $moduleVersion -Force -Scope CurrentUser -SkipPublisherCheck;
    }
}