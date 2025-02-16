using module "..\Common\TestContext.psm1";

Describe 'ExecDoFileTests' {
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

    it 'Execs Do File, explicit types' {
        # Arrange
        [string] $target = "MyTarget";
        [string] $global:outputString = $null;

        Set-Content -Path $script:testDoFile -Value @"
[string] `$wrench = `$null;
[bool] `$something = `$false;

Target $target {
    `$global:outputString = "`$wrench `$something";
}
"@;

        # Act
        doing exec -target $target -wrench breakerbar -something -silent;

        # Assert
        $global:outputString | Should -Be "breakerbar True"
    }

    it 'Execs Do File, implicit types' {
        # Arrange
        [string] $target = "MyTarget";
        [string] $global:outputString = $null;

        Set-Content -Path $script:testDoFile -Value @"
`$wrench = `$null;
`$something = `$false;

Target $target {
    `$global:outputString = "`$wrench `$something";
}
"@;

        # Act
        doing exec -target $target -wrench wheelbrace -something -silent;

        # Assert
        $global:outputString | Should -Be "wheelbrace True"
    }

    it 'Execs Do File, no defaults' {
        # Arrange
        [string] $target = "MyTarget";
        [string] $global:outputString = $null;

        Set-Content -Path $script:testDoFile -Value @"
Target $target {
    `$global:outputString = "`$wrench `$something";
}
"@;

        # Act
        doing exec -target $target -wrench socket -something -silent;

        # Assert
        $global:outputString | Should -Be "socket True"
    }

    it 'Execs Do File, defaults' {
        # Arrange
        [string] $target = "MyTarget";
        [string] $global:outputString = $null;

        Set-Content -Path $script:testDoFile -Value @"
[string] `$wrench = 'mr str';
[bool] `$something = `$false;

Target $target {
    `$global:outputString = "`$wrench `$something";
}
"@;

        # Act
        doing exec -target $target -silent;

        # Assert
        $global:outputString | Should -Be "mr str False"
    }
}