using namespace DoFramework.Domain;
using namespace DoFramework.Processing;
using module "..\Common\TestContext.psm1";
using module "..\Do\Modules\TestClassModule.psm1";
using module "..\Do\Modules\TestClassModule2.psm1";

Describe 'RunProcessTests' {
    BeforeEach {
        [TestContext] $script:context = [TestContext]::new();

        $script:context.SetCurrentPathToTestProject();
    }

    AfterEach {
        $script:context.ResetCurrentPath();
    }

    it 'Runs a Process' {
        # Arrange 
        [string] $processName = "TestProcess1";

        # Act
        [IContext] $context = doing run -name $processName -doOutput -silent;

        # Assert
        $context.Session.CurrentProcessName | Should -Be ([string]::Empty);

        $context.Session.ProcessCount | Should -Be 1;

        $context.Session.ProcessReports.Count | Should -Be 1;

        $context.Session.ProcessReports[0].ProcessResult | Should -Be ([ProcessResult]::Completed);

        $context.Session.ProcessReports[0].Name | Should -Be $processName;

        $context.Session.ProcessReports[0].Descriptor.Name | Should -Be $processName;
    }

    it 'Runs a Process with no output' {
        # Arrange 
        [string] $processName = "TestProcess1";

        # Act
        [IContext] $context = doing run -name $processName -silent;

        # Assert
        $context | Should -Be $null;
    }

    it 'Runs a Process with no output, output var is invalid' {
        # Arrange 
        [string] $processName = "TestProcess1";

        # Act
        [IContext] $context = doing run -name $processName -doOutput "invalid" -silent;

        # Assert
        $context | Should -Be $null;
    }
}