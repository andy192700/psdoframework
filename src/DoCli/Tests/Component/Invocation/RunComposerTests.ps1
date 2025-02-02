using namespace DoFramework.Domain;
using namespace DoFramework.Processing;
using module "..\Common\TestContext.psm1";
using module "..\Do\Modules\TestClassModule.psm1";
using module "..\Do\Modules\TestClassModule2.psm1";

Describe 'RunComposerTests' {
    BeforeEach {
        [TestContext] $script:context = [TestContext]::new();

        $script:context.SetCurrentPathToTestProject();
    }

    AfterEach {
        $script:context.ResetCurrentPath();
    }

    it 'Runs a Composer' {
        # Arrange 
        [string] $composerName = "TestComposer";
        [string] $processName = "TestProcess1";

        # Act
        [IContext] $context = doing compose -name $composerName -doOutput -silent -theProcessToRun $processName;

        # Assert
        $context.Session.CurrentProcessName | Should -Be ([string]::Empty);

        $context.Session.ProcessCount | Should -Be 1;

        $context.Session.ProcessReports.Count | Should -Be 1;

        $context.Session.ProcessReports[0].ProcessResult | Should -Be ([ProcessResult]::Completed);

        $context.Session.ProcessReports[0].Name | Should -Be $processName;

        $context.Session.ProcessReports[0].Descriptor.Name | Should -Be $processName;

        $context.Session.Composed | Should -Be $true;

        $context.Session.ComposedBy | Should -Be $composerName;
    }

    it 'Runs a Composer with no output' {
        # Arrange 
        [string] $composerName = "TestComposer";
        [string] $processName = "TestProcess1";

        # Act
        [IContext] $context = doing compose -name $composerName -silent -theProcessToRun $processName;

        # Assert
        $context | Should -Be $null;
    }

    it 'Runs a Composer with no output, output var is invalid' {
        # Arrange 
        [string] $composerName = "TestComposer";
        [string] $processName = "TestProcess1";

        # Act
        [IContext] $context = doing compose -name $composerName -doOutput "invalid" -silent -theProcessToRun $processName;

        # Assert
        $context | Should -Be $null;
    }
}