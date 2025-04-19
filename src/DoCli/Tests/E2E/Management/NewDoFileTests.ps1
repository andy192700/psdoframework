using module "..\Common\TestContext.psm1";

Describe 'NewDoFileTests' {
    BeforeEach {
        [TestContext] $script:context = [TestContext]::new();

        [string] $script:sep = [DoFramework.Environment.Environment]::Separator.ToString();

        $script:context.SetCurrentPathToTestProject();

        [string] $script:testDirectory = "$($script:context.ComponentTestsPath)$($script:sep)TestProject";

        [string] $script:testDoFile = "$($script:testDirectory)$($script:sep)dofile.ps1";

        New-Item -ItemType Directory -Path $script:testDirectory | Out-Null;

        Set-Location $script:testDirectory;
    }

    AfterEach {
        Remove-Item -Path $script:testDoFile -Recurse -Force | Out-Null;

        $script:context.SetCurrentPathToTestProject();

        Remove-Item -Path $script:testDirectory -Recurse -Force | Out-Null;

        $script:context.ResetCurrentPath();
    }

    it 'Creates Do File' {
        # Arrange
        [bool] $doFileExistsBefore = (Test-Path -Path $script:testDoFile);

        # Act
        doing new-dofile -silent;

        # Assert
        $doFileExistsBefore | Should -Be $false;

        (Test-Path -Path $script:testDoFile) | Should -Be $true;
    }
}