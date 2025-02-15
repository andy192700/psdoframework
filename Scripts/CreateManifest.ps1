using namespace System.IO;
using namespace System.Collections.Generic;
using module ".\lib\VersionCalculator.psm1";

param (
    [Parameter(Mandatory=$true)] [ValidateNotNullOrEmpty()] [string] $psNuGetSourceName
)

$ErrorActionPreference = "Stop";

[string] $version = [VersionCalculator]::Calculate($psNuGetSourceName);

[char] $sep = [Path]::DirectorySeparatorChar;

[string] $rootDir = Resolve-Path (Join-Path -Path $PSScriptRoot -ChildPath "..");

[string] $buildDir = "$rootDir$($sep)Build$($sep)PSDoFramework";

if ((Test-Path -Path $buildDir)) {
    Remove-Item -Path $buildDir -Recurse -Force;
}

New-Item -ItemType Directory -Path $buildDir | Out-Null;

New-Item -ItemType Directory -Path "$buildDir$($sep)DoCli" | Out-Null;

Copy-Item -Path "$rootDir$($sep)src$($sep)DoCli$($sep)Functions" -Destination "$buildDir$($sep)DoCli$($sep)Functions" -Recurse -Force;

Copy-Item -Path "$rootDir$($sep)src$($sep)DoCli$($sep)Objects" -Destination "$buildDir$($sep)DoCli$($sep)Objects" -Recurse -Force;

Copy-Item -Path "$rootDir$($sep)Scripts$($sep)ModuleDependenciesInstall.ps1" -Destination "$buildDir$($sep)ModuleDependenciesInstall.ps1" -Force;

Copy-Item -Path "$rootDir$($sep)src$($sep)DoFramework$($sep)DoFramework$($sep)bin$($sep)Release$($sep)net8$($sep)DoFramework.dll" -Destination "$buildDir";

[string] $baseDir = "$buildDir$($sep)DoCli";

[FileSystemInfo[]] $items = @();

$items += Get-ChildItem "$baseDir$($sep)Objects$($sep)FileSystem" -Recurse;
$items += Get-ChildItem "$baseDir$($sep)Objects$($sep)Mappers" -Recurse;
$items += Get-ChildItem "$baseDir$($sep)Objects$($sep)Modules" -Recurse;
$items += Get-ChildItem "$baseDir$($sep)Objects$($sep)Processing" -Recurse;
$items += Get-ChildItem "$baseDir$($sep)Objects$($sep)Validators" -Recurse;
$items += Get-ChildItem "$baseDir$($sep)Objects$($sep)Services$($sep)ApplicationServiceContainer.psm1";
$items += Get-ChildItem "$baseDir$($sep)Objects$($sep)CLI" -Recurse;
$items += Get-ChildItem "$baseDir$($sep)Objects$($sep)Services$($sep)CLIFunctionServiceContainer.psm1";
$items += Get-ChildItem "$baseDir$($sep)Functions";

[string] $manifestPath = "$buildDir$($sep)PSDoFramework.psd1";

[List[string]] $nestedModules = [List[string]]::new();

foreach ($item in $items) {
    if ($item.Extension -eq ".psm1") {
        $nestedModules.Add($item.FullName.Replace($baseDir, ".$($sep)DoCli"));
    }
}

[Hashtable] $manifestProperties = @{
    Path            = $manifestPath
    Description     = "The PSDoFramework, an object orientated Powershell development framework."
    Author          = "Andy192700"
    CompanyName     = "Andy192700"
    ModuleVersion   = $version
    PowerShellVersion = "7.4"
    FunctionsToExport = @("Doing", "Target")
    NestedModules = $nestedModules.ToArray()
    RequiredAssemblies = @(".$($sep)DoFramework.dll")
    ScriptsToProcess = @(".$($sep)ModuleDependenciesInstall.ps1")
};

New-ModuleManifest @manifestProperties `
     -Tags @("Development", "Framework") `
     -LicenseUri "https://github.com/andy192700/psdoframework/blob/main/LICENSE";