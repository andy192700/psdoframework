using module "..\Common\TestContext.psm1";

Describe 'ManageModuleTests' {
    BeforeEach {        
        [TestContext] $script:context = [TestContext]::new();
        
        [string] $script:moduleName = "ManageModuleTestsModule";

        [string] $script:testName = "$($script:moduleName)Tests";

        [string] $script:modulePath = $script:context.ComputeModulePath($script:moduleName);

        [string] $script:testsPath = $script:context.ComputeModuleTestPath($script:testName);
        
        $script:context.SetCurrentPathToTestProject();
    }

    AfterEach {
        $script:context.ResetCurrentPath();
    }

    it 'Creates then deletes Module' {
        $script:context.VerifyFiles($script:testsPath, $script:modulePath, $false, $false);

        doing new-module -name $script:moduleName -silent;

        $script:context.VerifyFiles($script:testsPath, $script:modulePath, $false, $true);

        [object] $projectContent = Get-Content -Path $script:context.ProjectPath | ConvertFrom-Json;

        [string[]] $modules = $projectContent.Modules;

        $modules -contains "$($script:moduleName).psm1" | Should -Be $true;

        [string[]] $tests = $projectContent.Tests.ModuleTests;

        $tests.Length | Should -Be 1;
        
        doing remove-module -name $script:moduleName -silent;

        $script:context.VerifyFiles($script:testsPath, $script:modulePath, $false, $false);
    }

    it 'Creates then deletes Module With Tests' {
        $script:context.VerifyFiles($script:testsPath, $script:modulePath, $false, $false);

        doing new-module -name $script:moduleName -addTests -silent;

        $script:context.VerifyFiles($script:testsPath, $script:modulePath, $true, $true);

        [object] $projectContent = Get-Content -Path $script:context.ProjectPath | ConvertFrom-Json;

        [string[]] $modules = $projectContent.Modules;

        $modules -contains "$($script:moduleName).psm1" | Should -Be $true;

        [string[]] $tests = $projectContent.Tests.ModuleTests;

        $tests -contains $script:context.ComputeLocalModuleTestPath($script:testName) | Should -Be $true;
        
        doing remove-module -name $script:moduleName -silent;

        $script:context.VerifyFiles($script:testsPath, $script:modulePath, $false, $false);
    }
}