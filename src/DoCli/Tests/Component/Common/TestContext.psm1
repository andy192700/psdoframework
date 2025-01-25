using namespace DoFramework;
using namespace System.Collections.Generic;
using namespace System.Xml;

class TestContext {
    [string] $OriginalDirectory;
    [string] $ComponentTestsPath;
    [string] $ProjectPath;
    [string] $SecondProjectDirectory;
    [string] $Sep;
    [List[string]] $TestOutputFiles;

    TestContext() {
        $this.Sep = [Environment.Environment]::Separator.ToString();
        $this.OriginalDirectory = (Get-Location);
        $this.ComponentTestsPath = "$($this.OriginalDirectory)$($this.Sep)src$($this.Sep)DoCli$($this.Sep)Tests$($this.Sep)Component";
        $this.ProjectPath = "$($this.ComponentTestsPath)$($this.Sep)do.json";
        $this.SecondProjectDirectory = "$($this.ComponentTestsPath)$($this.Sep)SecondProjectPath";
        
        $this.TestOutputFiles = [List[string]]::new();

        $this.TestOutputFiles.Add("$($this.ComponentTestsPath)$($this.Sep)ModuleTestCoverage.xml");
        $this.TestOutputFiles.Add("$($this.ComponentTestsPath)$($this.Sep)ModuleTestResults.xml");
        $this.TestOutputFiles.Add("$($this.ComponentTestsPath)$($this.Sep)ProcessTestCoverage.xml");
        $this.TestOutputFiles.Add("$($this.ComponentTestsPath)$($this.Sep)ProcessTestResults.xml");
    }

    [void] SetCurrentPathToTestProject() {
        Set-Location $this.ComponentTestsPath;
    }

    [void] ResetCurrentPath() {
        Set-Location $this.OriginalDirectory;
    }

    [void] VerifyFiles([string] $testPath, [string] $path, [bool] $testExists, [bool] $moduleExists) {
        [bool] $testExistsCheck = Test-Path -Path $testPath;
    
        [bool] $moduleExistsCheck = Test-Path -Path $path;
    
        $testExistsCheck | Should -Be $testExists;
    
        $moduleExistsCheck | Should -Be $moduleExists;
    }

    [string] ComputeModulePath([string] $moduleName) {
        return "$($this.ComponentTestsPath)$($this.Sep)Do$($this.Sep)Modules$($this.Sep)$($moduleName).psm1";
    }

    [string] ComputeLocalModuleTestPath([string] $testName) {
        return "Modules$($this.Sep)$testName.ps1";
    }

    [string] ComputeModuleTestPath([string] $testName) {
        return "$($this.ComponentTestsPath)$($this.Sep)Do$($this.Sep)Tests$($this.Sep)Modules$($this.Sep)$($testName).ps1";
    }

    [string] ComputeProcessPath([string] $processName) {
        return "$($this.ComponentTestsPath)$($this.Sep)Do$($this.Sep)Processes$($this.Sep)$($processName).ps1";
    }

    [string] ComputeLocalProcessTestPath([string] $testName) {
        return "Processes$($this.Sep)$testName.ps1";
    }

    [string] ComputeProcessTestPath([string] $testName) {
        return "$($this.ComponentTestsPath)$($this.Sep)Do$($this.Sep)Tests$($this.Sep)Processes$($this.Sep)$($testName).ps1";
    }

    [void] DeleteTestOutputIfExists() {
        foreach ($file in $this.TestOutputFiles) {
            if ((Test-Path $file)) {
                Remove-Item -Path $file -Force | Out-Null;
            }
        }
    }

    [bool] ModuleTestOutputsExist() {
        foreach ($file in $this.TestOutputFiles) {
            if ($file -like "*Module*") {
                [bool] $exists = (Test-Path $file);
                
                if (!$exists) {
                    return $false;
                }
            }
        }

        return $true;
    }

    [bool] ProcessTestOutputsExist() {
        foreach ($file in $this.TestOutputFiles) {
            if ($file -like "*Process*") {
                [bool] $exists = (Test-Path $file);
    
                if (!$exists) {
                    return $false;
                }
            }
        }

        return $true;
    }

    [XmlDocument] ReadTestOutput([string] $fileFilter) {
        [string] $file = $this.TestOutputFiles | Where-Object {
            return $_ -like "*$fileFilter*";
        };

        [XmlDocument] $xdoc = [XmlDocument]::new();

        $xdoc.Load($file);

        return $xdoc;
    }

    [string] ReadNodeAttribute([string] $fileFilter, [string] $xPath, [string] $attributeName) {
        [XmlDocument] $xdoc = $this.ReadTestOutput($fileFilter);

        [XmlNode] $node = $xdoc.SelectSingleNode($xPath);

        return $node.GetAttribute($attributeName);
    }

    [int] CountNodes([string] $fileFilter, [string] $xPath) {
        [XmlDocument] $xdoc = $this.ReadTestOutput($fileFilter);

        [XmlNode[]] $nodes = $xdoc.SelectNodes($xPath);

        return $nodes.Length;
    }
}