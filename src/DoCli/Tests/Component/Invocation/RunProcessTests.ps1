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
        [IContext] $context = doing run-process -name $processName -doOutput -silent;

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
        [IContext] $context = doing run-process -name $processName -silent;

        # Assert
        $context | Should -Be $null;
    }

    it 'Runs a Process with no output, output var is invalid' {
        # Arrange 
        [string] $processName = "TestProcess1";

        # Act
        [IContext] $context = doing run-process -name $processName -doOutput "invalid" -silent;

        # Assert
        $context | Should -Be $null;
    }
    
    it 'Runs a Process That Runs Other Processes' {
        # Arrange 
        [string] $processName = "TestOrchestrator";

        # Act
        [IContext] $context = doing run-process -name $processName -doOutput -silent;

        # Assert
        $context.Get("ContextEntry") | Should -Be "some_value";

        $context.Session.CurrentProcessName | Should -Be ([string]::Empty);

        $context.Session.ProcessCount | Should -Be 4;

        $context.Session.ProcessReports.Count | Should -Be 4;

        $context.Session.ProcessReports[0].ProcessResult | Should -Be ([ProcessResult]::Completed);

        $context.Session.ProcessReports[0].Name | Should -Be "--TestProcess1";

        $context.Session.ProcessReports[0].Descriptor.Name | Should -Be "TestProcess1";

        $context.Session.ProcessReports[1].ProcessResult | Should -Be ([ProcessResult]::Completed);

        $context.Session.ProcessReports[1].Name | Should -Be "--TestProcess2";

        $context.Session.ProcessReports[1].Descriptor.Name | Should -Be "TestProcess2";

        $context.Session.ProcessReports[2].ProcessResult | Should -Be ([ProcessResult]::Completed);

        $context.Session.ProcessReports[2].Name | Should -Be "--TestProcess3";

        $context.Session.ProcessReports[2].Descriptor.Name | Should -Be "TestProcess3";

        $context.Session.ProcessReports[3].ProcessResult | Should -Be ([ProcessResult]::Completed);

        $context.Session.ProcessReports[3].Name | Should -Be $processName;

        $context.Session.ProcessReports[3].Descriptor.Name | Should -Be $processName;
    }    

    it 'Runs a Process That Runs Other Processes and Fails' {
        # Arrange 
        [string] $processName = "TestOrchestrator";

        # Act
        [IContext] $context = doing run-process -name $processName -Mode "Fail" -doOutput -silent;

        # Assert
        $context.Get("ContextEntry") | Should -BeNullOrEmpty;

        $context.Session.CurrentProcessName | Should -Be ([string]::Empty);

        $context.Session.ProcessCount | Should -Be 4;

        $context.Session.ProcessReports.Count | Should -Be 4;

        $context.Session.ProcessReports[0].ProcessResult | Should -Be ([ProcessResult]::Completed);

        $context.Session.ProcessReports[0].Name | Should -Be "--TestProcess1";

        $context.Session.ProcessReports[0].Descriptor.Name | Should -Be "TestProcess1";

        $context.Session.ProcessReports[1].ProcessResult | Should -Be ([ProcessResult]::Failed);

        $context.Session.ProcessReports[1].Name | Should -Be "--TestProcess2";

        $context.Session.ProcessReports[1].Descriptor.Name | Should -Be "TestProcess2";

        $context.Session.ProcessReports[2].ProcessResult | Should -Be ([ProcessResult]::NotRun);

        $context.Session.ProcessReports[2].Name | Should -Be "--TestProcess3";

        $context.Session.ProcessReports[2].Descriptor.Name | Should -Be "TestProcess3";

        $context.Session.ProcessReports[3].ProcessResult | Should -Be ([ProcessResult]::Failed);

        $context.Session.ProcessReports[3].Name | Should -Be $processName;

        $context.Session.ProcessReports[3].Descriptor.Name | Should -Be $processName;
    }    

    it 'Runs a Process That Runs Other Processes and Fails due to being Invalid' {
        # Arrange 
        [string] $processName = "TestOrchestrator";

        # Act
        [IContext] $context = doing run-process -name $processName -Mode "Invalid" -doOutput -silent;

        # Assert
        $context.Get("ContextEntry") | Should  -BeNullOrEmpty;

        $context.Session.CurrentProcessName | Should -Be ([string]::Empty);

        $context.Session.ProcessCount | Should -Be 4;

        $context.Session.ProcessReports.Count | Should -Be 4;

        $context.Session.ProcessReports[0].ProcessResult | Should -Be ([ProcessResult]::Completed);

        $context.Session.ProcessReports[0].Name | Should -Be "--TestProcess1";

        $context.Session.ProcessReports[0].Descriptor.Name | Should -Be "TestProcess1";

        $context.Session.ProcessReports[1].ProcessResult | Should -Be ([ProcessResult]::Invalidated);

        $context.Session.ProcessReports[1].Name | Should -Be "--TestProcess2";

        $context.Session.ProcessReports[1].Descriptor.Name | Should -Be "TestProcess2";

        $context.Session.ProcessReports[2].ProcessResult | Should -Be ([ProcessResult]::NotRun);

        $context.Session.ProcessReports[2].Name | Should -Be "--TestProcess3";

        $context.Session.ProcessReports[2].Descriptor.Name | Should -Be "TestProcess3";

        $context.Session.ProcessReports[3].ProcessResult | Should -Be ([ProcessResult]::Failed);

        $context.Session.ProcessReports[3].Name | Should -Be $processName;

        $context.Session.ProcessReports[3].Descriptor.Name | Should -Be $processName;
    }

    it 'Processes Can execute, register their own services which are injected' {
        # Arrange
        [string] $processName = "ServiceContainerProcess1";

        # Act
        [IContext] $context = doing run-process -name $processName -doOutput -silent;

        # Assert
        $interface = $context.Get("Interface");

        $instance = $context.Get("Instance");

        $interface | Should -Not -BeNullOrEmpty;

        $instance | Should -Not -BeNullOrEmpty;

        $interface.GetType().FullName | Should -Be ([Example].FullName);

        $instance.GetType().FullName | Should -Be ([BasicClass].FullName);

        $context.Session.CurrentProcessName | Should -Be ([string]::Empty);

        $context.Session.ProcessCount | Should -Be 2;

        $context.Session.ProcessReports.Count | Should -Be 2;

        $context.Session.ProcessReports[0].ProcessResult | Should -Be ([ProcessResult]::Completed);

        $context.Session.ProcessReports[0].Name | Should -Be "--ServiceContainerProcess2";

        $context.Session.ProcessReports[0].Descriptor.Name | Should -Be "ServiceContainerProcess2";

        $context.Session.ProcessReports[1].ProcessResult | Should -Be ([ProcessResult]::Completed);

        $context.Session.ProcessReports[1].Name | Should -Be $processName;

        $context.Session.ProcessReports[1].Descriptor.Name | Should -Be $processName;

    }
}