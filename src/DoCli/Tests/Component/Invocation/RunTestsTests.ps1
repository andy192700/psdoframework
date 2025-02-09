using namespace System.Xml;
using module "..\Common\TestContext.psm1";

Describe 'RunTestsTests' {
    BeforeEach {
        [TestContext] $script:context = [TestContext]::new();

        $script:context.SetCurrentPathToTestProject();

        $script:context.DeleteTestOutputIfExists();
    }

    AfterEach {
        $script:context.ResetCurrentPath();

        $script:context.DeleteTestOutputIfExists();
    }

    it 'Runs Tests and no results are written out' {
        # Arrange / Act
        doing test -filter .* -silent;

        # Assert
        [bool] $moduleOutputsExist = $script:context.ModuleTestOutputsExist();
        
        [bool] $processOutputsExist = $script:context.ProcessTestOutputsExist();

        $moduleOutputsExist | Should -Be $false;

        $processOutputsExist | Should -Be $false;
    } 

    it 'Runs Tests and Results are Written Out NUnitXml' {
        # Arrange / Act
        doing test -filter .* -outputFormat NUnitXml -silent;

        # Assert
        [bool] $moduleOutputsExist = $script:context.ModuleTestOutputsExist();
        
        [bool] $processOutputsExist = $script:context.ProcessTestOutputsExist();

        $moduleOutputsExist -and $processOutputsExist | Should -Be $true;

        $script:context.CountNodes("ModuleTestCoverage", "/report/package/class") | Should -Be 1;

        $script:context.CountNodes("ModuleTestResults", "/test-results/test-suite/results/test-suite") | Should -Be 1;

        $script:context.ReadNodeAttribute("ModuleTestResults", "/test-results/environment", "nunit-version") | Should -Not -BeNullOrEmpty;

        $script:context.CountNodes("ProcessTestCoverage", "/report/package/class") | Should -Be 1;

        $script:context.CountNodes("ProcessTestResults", "/test-results/test-suite/results/test-suite") | Should -Be 1;

        $script:context.ReadNodeAttribute("ProcessTestResults", "/test-results/environment", "nunit-version") | Should -Not -BeNullOrEmpty;
    } 

    it 'Runs Tests and Results are Written Out NUnitXml Filtered' {
        # Arrange / Act
        doing test -filter Tests -outputFormat NUnitXml -silent;

        # Assert
        [bool] $moduleOutputsExist = $script:context.ModuleTestOutputsExist();
        
        [bool] $processOutputsExist = $script:context.ProcessTestOutputsExist();

        $moduleOutputsExist -and $processOutputsExist | Should -Be $true;

        $script:context.CountNodes("ModuleTestCoverage", "/report/package/class") | Should -Be 1;

        $script:context.CountNodes("ModuleTestResults", "/test-results/test-suite/results/test-suite") | Should -Be 1;

        $script:context.ReadNodeAttribute("ModuleTestResults", "/test-results/environment", "nunit-version") | Should -Not -BeNullOrEmpty;

        $script:context.CountNodes("ProcessTestCoverage", "/report/package/class") | Should -Be 1;

        $script:context.CountNodes("ProcessTestResults", "/test-results/test-suite/results/test-suite") | Should -Be 1;

        $script:context.ReadNodeAttribute("ProcessTestResults", "/test-results/environment", "nunit-version") | Should -Not -BeNullOrEmpty;
    } 

    it 'Runs Tests and Results are Written Out NUnitXml Filtered No Tests' {
        # Arrange / Act
        doing test -filter WillNotFindAny -outputFormat NUnitXml -silent;

        # Assert
        [bool] $moduleOutputsExist = $script:context.ModuleTestOutputsExist();
        
        [bool] $processOutputsExist = $script:context.ProcessTestOutputsExist();

        $moduleOutputsExist | Should -Be $false;

        $processOutputsExist | Should -Be $false;
    } 

    it 'Runs Tests and Results are Written Out JUnitXml' {
        # Arrange / Act
        doing test -filter .* -outputFormat JUnitXml -silent;

        # Assert
        [bool] $moduleOutputsExist = $script:context.ModuleTestOutputsExist();
        
        [bool] $processOutputsExist = $script:context.ProcessTestOutputsExist();

        $moduleOutputsExist -and $processOutputsExist | Should -Be $true;

        $script:context.CountNodes("ModuleTestCoverage", "/report/package/class") | Should -Be 1;

        $script:context.CountNodes("ModuleTestResults", "/testsuites/testsuite") | Should -Be 1;

        $script:context.ReadNodeAttribute("ModuleTestResults", "/testsuites", "xsi:noNamespaceSchemaLocation") | Should -Not -BeNullOrEmpty;

        $script:context.CountNodes("ProcessTestCoverage", "/report/package/class") | Should -Be 1;

        $script:context.CountNodes("ProcessTestResults", "/testsuites/testsuite") | Should -Be 1;

        $script:context.ReadNodeAttribute("ProcessTestResults", "/testsuites", "xsi:noNamespaceSchemaLocation") | Should -Not -BeNullOrEmpty;
    } 

    it 'Runs Process Tests and Results are Written Out NUnitXml' {
        doing test -filter .* -outputFormat NUnitXml -forProcesses -silent;

        # Assert
        [bool] $moduleOutputsExist = $script:context.ModuleTestOutputsExist();
        
        [bool] $processOutputsExist = $script:context.ProcessTestOutputsExist();

        $moduleOutputsExist | Should -Be $false;

        $processOutputsExist | Should -Be $true;

        $script:context.CountNodes("ProcessTestCoverage", "/report/package/class") | Should -Be 1;

        $script:context.CountNodes("ProcessTestResults", "/test-results/test-suite/results/test-suite") | Should -Be 1;

        $script:context.ReadNodeAttribute("ProcessTestResults", "/test-results/environment", "nunit-version") | Should -Not -BeNullOrEmpty;
    } 

    it 'Runs Process Tests and Results are Written Out NUnitXml Filtered' {
        doing test -filter Tests -outputFormat NUnitXml -forProcesses -silent;

        # Assert
        [bool] $moduleOutputsExist = $script:context.ModuleTestOutputsExist();
        
        [bool] $processOutputsExist = $script:context.ProcessTestOutputsExist();

        $moduleOutputsExist | Should -Be $false;

        $processOutputsExist | Should -Be $true;

        $script:context.CountNodes("ProcessTestCoverage", "/report/package/class") | Should -Be 1;

        $script:context.CountNodes("ProcessTestResults", "/test-results/test-suite/results/test-suite") | Should -Be 1;

        $script:context.ReadNodeAttribute("ProcessTestResults", "/test-results/environment", "nunit-version") | Should -Not -BeNullOrEmpty;
    } 

    it 'Runs Process Tests and Results are Written Out NUnitXml Filtered No Tests' {
        # Arrange / Act
        doing test -filter WillNotFindAny -outputFormat NUnitXml -forProcesses -silent;

        # Assert
        [bool] $moduleOutputsExist = $script:context.ModuleTestOutputsExist();
        
        [bool] $processOutputsExist = $script:context.ProcessTestOutputsExist();

        $moduleOutputsExist | Should -Be $false;

        $processOutputsExist | Should -Be $false;
    } 

    it 'Runs Process Tests and Results are Written Out JUnitXml' {
        # Arrange / Act
        doing test -filter Tests -outputFormat JUnitXml -forProcesses -silent;

        # Assert
        [bool] $moduleOutputsExist = $script:context.ModuleTestOutputsExist();
        
        [bool] $processOutputsExist = $script:context.ProcessTestOutputsExist();

        $moduleOutputsExist | Should -Be $false;

        $processOutputsExist | Should -Be $true;

        $script:context.CountNodes("ProcessTestCoverage", "/report/package/class") | Should -Be 1;

        $script:context.CountNodes("ProcessTestResults", "/testsuites/testsuite") | Should -Be 1;

        $script:context.ReadNodeAttribute("ProcessTestResults", "/testsuites", "xsi:noNamespaceSchemaLocation") | Should -Not -BeNullOrEmpty;
    } 

    it 'Runs Module Tests and Results are Written Out NUnitXml' {
        doing test -filter .* -outputFormat NUnitXml -forModules -silent;

        # Assert
        [bool] $moduleOutputsExist = $script:context.ModuleTestOutputsExist();
        
        [bool] $processOutputsExist = $script:context.ProcessTestOutputsExist();

        $moduleOutputsExist | Should -Be $true;

        $processOutputsExist | Should -Be $false;

        $script:context.CountNodes("ModuleTestCoverage", "/report/package/class") | Should -Be 1;

        $script:context.CountNodes("ModuleTestResults", "/test-results/test-suite/results/test-suite") | Should -Be 1;

        $script:context.ReadNodeAttribute("ModuleTestResults", "/test-results/environment", "nunit-version") | Should -Not -BeNullOrEmpty;
    } 

    it 'Runs Module Tests and Results are Written Out NUnitXml Filtered' {
        doing test -filter Tests -outputFormat NUnitXml -forModules -silent;

        # Assert
        [bool] $moduleOutputsExist = $script:context.ModuleTestOutputsExist();
        
        [bool] $processOutputsExist = $script:context.ProcessTestOutputsExist();

        $moduleOutputsExist | Should -Be $true;

        $processOutputsExist | Should -Be $false;

        $script:context.CountNodes("ModuleTestCoverage", "/report/package/class") | Should -Be 1;

        $script:context.CountNodes("ModuleTestResults", "/test-results/test-suite/results/test-suite") | Should -Be 1;

        $script:context.ReadNodeAttribute("ModuleTestResults", "/test-results/environment", "nunit-version") | Should -Not -BeNullOrEmpty;
    } 

    it 'Runs Module Tests and Results are Written Out NUnitXml Filtered No Tests' {
        # Arrange / Act
        doing test -filter WillNotFindAny -outputFormat NUnitXml -forModules -silent;

        # Assert
        [bool] $moduleOutputsExist = $script:context.ModuleTestOutputsExist();
        
        [bool] $processOutputsExist = $script:context.ProcessTestOutputsExist();

        $moduleOutputsExist | Should -Be $false;

        $processOutputsExist | Should -Be $false;
    } 

    it 'Runs Module Tests and Results are Written Out JUnitXml' {
        # Arrange / Act
        doing test -filter Tests -outputFormat JUnitXml -forModules -silent;

        # Assert
        [bool] $moduleOutputsExist = $script:context.ModuleTestOutputsExist();
        
        [bool] $processOutputsExist = $script:context.ProcessTestOutputsExist();

        $moduleOutputsExist | Should -Be $true;

        $processOutputsExist | Should -Be $false;

        $script:context.CountNodes("ModuleTestCoverage", "/report/package/class") | Should -Be 1;

        $script:context.CountNodes("ModuleTestResults", "/testsuites/testsuite") | Should -Be 1;

        $script:context.ReadNodeAttribute("ModuleTestResults", "/testsuites", "xsi:noNamespaceSchemaLocation") | Should -Not -BeNullOrEmpty;
    } 




    

    it 'Runs Composer Tests and Results are Written Out NUnitXml' {
        doing test -filter .* -outputFormat NUnitXml -forComposers -silent;

        # Assert
        [bool] $moduleOutputsExist = $script:context.ModuleTestOutputsExist();
        
        [bool] $processOutputsExist = $script:context.ProcessTestOutputsExist();

        [bool] $composerOutputsExist = $script:context.ComposerTestOutputsExist();

        $composerOutputsExist | Should -Be $true;

        $moduleOutputsExist | Should -Be $false;

        $processOutputsExist | Should -Be $false;

        $script:context.CountNodes("ComposerTestCoverage", "/report/package/class") | Should -Be 1;

        $script:context.CountNodes("ComposerTestResults", "/test-results/test-suite/results/test-suite") | Should -Be 1;

        $script:context.ReadNodeAttribute("ComposerTestResults", "/test-results/environment", "nunit-version") | Should -Not -BeNullOrEmpty;
    }

    it 'Runs Composer Tests and Results are Written Out NUnitXml Filtered' {
        doing test -filter Tests -outputFormat NUnitXml -forComposers -silent;

        # Assert
        [bool] $moduleOutputsExist = $script:context.ModuleTestOutputsExist();
        
        [bool] $processOutputsExist = $script:context.ProcessTestOutputsExist();

        [bool] $composerOutputsExist = $script:context.ComposerTestOutputsExist();

        $composerOutputsExist | Should -Be $true;

        $moduleOutputsExist | Should -Be $false;

        $processOutputsExist | Should -Be $false;

        $script:context.CountNodes("ComposerTestCoverage", "/report/package/class") | Should -Be 1;

        $script:context.CountNodes("ComposerTestResults", "/test-results/test-suite/results/test-suite") | Should -Be 1;

        $script:context.ReadNodeAttribute("ComposerTestResults", "/test-results/environment", "nunit-version") | Should -Not -BeNullOrEmpty;
    } 

    it 'Runs Composer Tests and Results are Written Out NUnitXml Filtered No Tests' {
        # Arrange / Act
        doing test -filter WillNotFindAny -outputFormat NUnitXml -forComposers -silent;

        # Assert
        [bool] $moduleOutputsExist = $script:context.ModuleTestOutputsExist();
        
        [bool] $processOutputsExist = $script:context.ProcessTestOutputsExist();

        [bool] $composerOutputsExist = $script:context.ComposerTestOutputsExist();

        $composerOutputsExist | Should -Be $false;

        $moduleOutputsExist | Should -Be $false;

        $processOutputsExist | Should -Be $false;
    } 

    it 'Runs Composer Tests and Results are Written Out JUnitXml' {
        # Arrange / Act
        doing test -filter Tests -outputFormat JUnitXml -forComposers -silent;

        # Assert
        [bool] $moduleOutputsExist = $script:context.ModuleTestOutputsExist();
        
        [bool] $processOutputsExist = $script:context.ProcessTestOutputsExist();

        [bool] $composerOutputsExist = $script:context.ComposerTestOutputsExist();

        $composerOutputsExist | Should -Be $true;

        $moduleOutputsExist | Should -Be $false;

        $processOutputsExist | Should -Be $false;

        $script:context.CountNodes("ComposerTestCoverage", "/report/package/class") | Should -Be 1;

        $script:context.CountNodes("ComposerTestResults", "/testsuites/testsuite") | Should -Be 1;

        $script:context.ReadNodeAttribute("ComposerTestResults", "/testsuites", "xsi:noNamespaceSchemaLocation") | Should -Not -BeNullOrEmpty;
    }
}
