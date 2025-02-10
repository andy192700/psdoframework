using module "..\Common\TestContext.psm1";

Describe 'ManageProcessTests' {
    BeforeEach {         
        [TestContext] $script:context = [TestContext]::new();
        
        [string] $script:processName = "ManageProcessTestsProcess";

        [string] $script:testName = "$($script:processName)Tests";

        [string] $script:processPath = $script:context.ComputeProcessPath($script:processName);

        [string] $script:testsPath = $script:context.ComputeProcessTestPath($script:testName);
        
        $script:context.SetCurrentPathToTestProject();

        [string] $script:originalProjectContent = Get-Content -Path $script:context.ProjectPath;
    }

    AfterEach {
        $script:context.ResetCurrentPath();
    }

    it 'Creates then deletes Process' {
        $script:context.VerifyFiles($script:testsPath, $script:processPath, $false, $false);

        doing new-process -name $script:processName -silent;

        $script:context.VerifyFiles($script:testsPath, $script:processPath, $false, $true);

        [object] $projectContent = Get-Content -Path $script:context.ProjectPath | ConvertFrom-Json;

        [string[]] $processes = $projectContent.Processes;

        $processes -contains "$($script:processName).ps1" | Should -Be $true;

        [string[]] $tests = $projectContent.Tests.Processtests;

        $tests.Length | Should -Be 1;
        
        doing remove-process -name $script:processName -silent;

        $script:context.VerifyFiles($script:testsPath, $script:processPath, $false, $false);

        [string] $projectContent = Get-Content -Path $script:context.ProjectPath;

        $projectContent | Should -Be $script:originalProjectContent;
    }

    it 'Creates then deletes Process With Tests' {
        $script:context.VerifyFiles($script:testsPath, $script:processPath, $false, $false);

        doing new-process -name $script:processName -addTests -silent;

        $script:context.VerifyFiles($script:testsPath, $script:processPath, $true, $true);

        [object] $projectContent = Get-Content -Path $script:context.ProjectPath | ConvertFrom-Json;

        [string[]] $processes = $projectContent.Processes;

        $processes -contains "$($script:processName).ps1" | Should -Be $true;

        [string[]] $tests = $projectContent.Tests.Processtests;

        $tests -contains $script:context.ComputeLocalProcessTestPath($script:testName) | Should -Be $true;
        
        doing remove-process -name $script:processName -silent;

        $script:context.VerifyFiles($script:testsPath, $script:processPath, $false, $false);

        [string] $projectContent = Get-Content -Path $script:context.ProjectPath;

        $projectContent | Should -Be $script:originalProjectContent;
    }
}