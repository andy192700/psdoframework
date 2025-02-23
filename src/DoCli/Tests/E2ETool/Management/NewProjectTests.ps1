using module "..\Common\TestContext.psm1";

Describe 'CreateProjectTests' {
    BeforeEach {
        [TestContext] $script:context = [TestContext]::new();

        [string] $script:sep = [DoFramework.Environment.Environment]::Separator.ToString();

        $script:context.SetCurrentPathToTestProject();

        [string] $script:testDirectory = "$($script:context.ComponentTestsPath)$($script:sep)TestProject";

        New-Item -ItemType Directory -Path $script:testDirectory | Out-Null;

        Set-Location $script:testDirectory;
    }

    AfterEach {
        $script:context.SetCurrentPathToTestProject();

        Remove-Item -Path $script:testDirectory -Recurse -Force | Out-Null;

        $script:context.ResetCurrentPath();
    }

    it 'Creates Project With Specific Name' {
        # Arrange
        [string] $projectName = "theproject";

        [string] $projectDirectory = "$($script:testDirectory)$($script:sep)$($projectName)";

        [string] $projectFile = "$($script:testDirectory)$($script:sep)do.json";

        [string] $envFile = "$($script:testDirectory)$($script:sep).env";

        # Act
        psdoing new-project -name $projectName -silent;

        # Assert
        (Test-Path -Path $projectDirectory) | Should -Be $true;

        (Test-Path -Path "$($projectDirectory)$($script:sep)Processes$($script:sep).gitkeep") | Should -Be $true;

        (Test-Path -Path "$($projectDirectory)$($script:sep)Modules$($script:sep).gitkeep") | Should -Be $true;

        (Test-Path -Path "$($projectDirectory)$($script:sep)Composers$($script:sep).gitkeep") | Should -Be $true;

        (Test-Path -Path "$($projectDirectory)$($script:sep)Tests$($script:sep).gitkeep") | Should -Be $true;

        (Test-Path -Path "$($projectDirectory)$($script:sep)Tests$($script:sep)Processes$($script:sep).gitkeep") | Should -Be $true;

        (Test-Path -Path "$($projectDirectory)$($script:sep)Tests$($script:sep)Modules$($script:sep).gitkeep") | Should -Be $true;

        (Test-Path -Path "$($projectDirectory)$($script:sep)Tests$($script:sep)Composers$($script:sep).gitkeep") | Should -Be $true;

        (Test-Path -Path $projectFile) | Should -Be $true;

        (Test-Path -Path $envFile) | Should -Be $true;

        [object] $projectContent = Get-Content -Path $projectFile | ConvertFrom-Json;

        [string[]] $processes = $projectContent.Processes;

        [string[]] $modules = $projectContent.Modules;

        [string[]] $processTests = $projectContent.Tests.Processes;

        [string[]] $moduleTests = $projectContent.Tests.Modules;

        [string] $name = $projectContent.Name;

        [string] $version = $projectContent.Version;

        [string] $psVersion = $projectContent.PSVersion;
        
        $processes.Length | Should -Be 0;
        
        $modules.Length | Should -Be 0;
        
        $processTests.Length | Should -Be 0;
        
        $moduleTests.Length | Should -Be 0;

        $name | Should -Be $projectName;

        $version | Should -Be (get-module -Name PSDoFramework).Version.ToString();

        $psVersion | Should -Be $global:psversiontable.PSVersion.ToString();
    }

    it 'Creates Project With Default Name' {
        # Arrange
        [string] $projectName = "Do";

        [string] $projectDirectory = "$($script:testDirectory)$($script:sep)$($projectName)";

        [string] $projectFile = "$($script:testDirectory)$($script:sep)do.json";

        [string] $envFile = "$($script:testDirectory)$($script:sep).env";

        # Act
        psdoing new-project -silent;

        # Assert
        (Test-Path -Path $projectDirectory) | Should -Be $true;

        (Test-Path -Path "$($projectDirectory)$($script:sep)Processes$($script:sep).gitkeep") | Should -Be $true;

        (Test-Path -Path "$($projectDirectory)$($script:sep)Modules$($script:sep).gitkeep") | Should -Be $true;

        (Test-Path -Path "$($projectDirectory)$($script:sep)Composers$($script:sep).gitkeep") | Should -Be $true;

        (Test-Path -Path "$($projectDirectory)$($script:sep)Tests$($script:sep).gitkeep") | Should -Be $true;

        (Test-Path -Path "$($projectDirectory)$($script:sep)Tests$($script:sep)Processes$($script:sep).gitkeep") | Should -Be $true;

        (Test-Path -Path "$($projectDirectory)$($script:sep)Tests$($script:sep)Modules$($script:sep).gitkeep") | Should -Be $true;

        (Test-Path -Path "$($projectDirectory)$($script:sep)Tests$($script:sep)Composers$($script:sep).gitkeep") | Should -Be $true;

        (Test-Path -Path $projectFile) | Should -Be $true;

        (Test-Path -Path $envFile) | Should -Be $true;

        [object] $projectContent = Get-Content -Path $projectFile | ConvertFrom-Json;

        [string[]] $processes = $projectContent.Processes;

        [string[]] $modules = $projectContent.Modules;

        [string[]] $composers = $projectContent.Composers;

        [string[]] $processTests = $projectContent.Tests.Processes;

        [string[]] $moduleTests = $projectContent.Tests.Modules;

        [string[]] $composerTests = $projectContent.Tests.Composers;

        [string] $name = $projectContent.Name;

        [string] $version = $projectContent.Version;

        [string] $psVersion = $projectContent.PSVersion;
        
        $processes.Length | Should -Be 0;
        
        $modules.Length | Should -Be 0;
        
        $composers.Length | Should -Be 0;
        
        $processTests.Length | Should -Be 0;
        
        $moduleTests.Length | Should -Be 0;
        
        $composerTests.Length | Should -Be 0;

        $name | Should -Be $projectName;

        $version | Should -Be (get-module -Name PSDoFramework).Version.ToString();

        $psVersion | Should -Be $global:psversiontable.PSVersion.ToString();
    }

    it 'Creates Project With Default Name In A SubDirectory' {
        # Arrange
        [string] $projectName = "Do";

        [string] $projectDirectory = $script:context.SecondProjectDirectory;

        [string] $projectFile = "$($script:context.SecondProjectDirectory)$($script:sep)do.json";

        [string] $envFile = "$($script:context.SecondProjectDirectory)$($script:sep).env";

        New-Item -ItemType Directory -Path $projectDirectory | Out-Null;

        # Act
        psdoing new-project -home $projectDirectory -silent;

        # Assert
        (Test-Path -Path $projectDirectory) | Should -Be $true;

        (Test-Path -Path "$($projectDirectory)$($script:sep)$($projectName)$($script:sep)Processes$($script:sep).gitkeep") | Should -Be $true;

        (Test-Path -Path "$($projectDirectory)$($script:sep)$($projectName)$($script:sep)Modules$($script:sep).gitkeep") | Should -Be $true;

        (Test-Path -Path "$($projectDirectory)$($script:sep)$($projectName)$($script:sep)Tests$($script:sep).gitkeep") | Should -Be $true;

        (Test-Path -Path "$($projectDirectory)$($script:sep)$($projectName)$($script:sep)Tests$($script:sep)Processes$($script:sep).gitkeep") | Should -Be $true;

        (Test-Path -Path "$($projectDirectory)$($script:sep)$($projectName)$($script:sep)Tests$($script:sep)Modules$($script:sep).gitkeep") | Should -Be $true;

        (Test-Path -Path $projectFile) | Should -Be $true;

        (Test-Path -Path $envFile) | Should -Be $true;

        [object] $projectContent = Get-Content -Path $projectFile | ConvertFrom-Json;

        [string[]] $processes = $projectContent.Processes;

        [string[]] $modules = $projectContent.Modules;

        [string[]] $composers = $projectContent.Composers;

        [string[]] $processTests = $projectContent.Tests.Processes;

        [string[]] $moduleTests = $projectContent.Tests.Modules;

        [string[]] $composerTests = $projectContent.Tests.Composers;

        [string] $name = $projectContent.Name;

        [string] $version = $projectContent.Version;

        [string] $psVersion = $projectContent.PSVersion;
        
        $processes.Length | Should -Be 0;
        
        $modules.Length | Should -Be 0;
        
        $composers.Length | Should -Be 0;
        
        $processTests.Length | Should -Be 0;
        
        $moduleTests.Length | Should -Be 0;
        
        $composerTests.Length | Should -Be 0;

        $name | Should -Be $projectName;

        $version | Should -Be (get-module -Name PSDoFramework).Version.ToString();

        $psVersion | Should -Be $global:psversiontable.PSVersion.ToString();

        Remove-Item -Path $projectDirectory -Force -Recurse | Out-Null;
    }

    it 'Cannot Create Project With Default Name In A SubDirectory, as it does not exist' {
        # Arrange
        [string] $projectDirectory = $script:context.SecondProjectDirectory;

        # Act
        [string] $result = psdoing new-project -home $projectDirectory -silent;

        # Assert
        $result | Should -BeLike "*Requested project path*does not exist.*";
    }
}