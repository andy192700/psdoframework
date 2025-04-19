using module "..\Common\TestContext.psm1";

Describe 'ManageComposerTests' {
    BeforeEach {        
        [TestContext] $script:context = [TestContext]::new();
        
        [string] $script:composerName = "ManageComposerTestsComposer";

        [string] $script:testName = "$($script:composerName)Tests";

        [string] $script:composerPath = $script:context.ComputeComposerPath($script:composerName);

        [string] $script:testsPath = $script:context.ComputeComposerTestPath($script:testName);
        
        $script:context.SetCurrentPathToTestProject();
    }

    AfterEach {
        $script:context.ResetCurrentPath();
    }

    it 'Creates then deletes Composer' {
        $script:context.VerifyFiles($script:testsPath, $script:composerPath, $false, $false);

        doing new-composer -name $script:composerName -silent;

        $script:context.VerifyFiles($script:testsPath, $script:composerPath, $false, $true);

        [object] $projectContent = Get-Content -Path $script:context.ProjectPath | ConvertFrom-Json;

        [string[]] $composers = $projectContent.Composers;

        $composers -contains "$($script:composerName).ps1" | Should -Be $true;

        [string[]] $tests = $projectContent.Tests.ComposerTests;

        $tests.Length | Should -Be 1;
        
        doing delete-composer -name $script:composerName -silent;

        $script:context.VerifyFiles($script:testsPath, $script:composerPath, $false, $false);
    }

    it 'Creates then deletes composer With Tests' {
        $script:context.VerifyFiles($script:testsPath, $script:composerPath, $false, $false);

        doing new-composer -name $script:composerName -addTests -silent;

        $script:context.VerifyFiles($script:testsPath, $script:composerPath, $true, $true);

        [object] $projectContent = Get-Content -Path $script:context.ProjectPath | ConvertFrom-Json;

        [string[]] $composers = $projectContent.Composers;

        $composers -contains "$($script:composerName).ps1" | Should -Be $true;

        [string[]] $tests = $projectContent.Tests.ComposerTests;

        $tests -contains $script:context.ComputeLocalComposerTestPath($script:testName) | Should -Be $true;
        
        doing delete-composer -name $script:composerName -silent;

        $script:context.VerifyFiles($script:testsPath, $script:composerPath, $false, $false);
    }
}