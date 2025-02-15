using namespace DoFramework.CLI;
using namespace DoFramework.FileSystem;
using namespace DoFramework.Logging;
using namespace DoFramework.Processing;
using namespace DoFramework.Testing;
using namespace DoFramework.Validators;
using namespace System.Collections.Generic;
using module "..\..\..\..\Objects\Processing\DoFileTarget.psm1";
using module "..\..\..\..\Objects\Processing\DoFileInvoker.psm1";

Describe 'DoFileInvokerValidatorTests' {
    BeforeEach {
        [CLIFunctionParameters] $script:parameters = [CLIFunctionParameters]::new();
        $script:parameters.Parameters = [Dictionary[string, object]]::new();

        [ProxyResult] $script:mockValidator = doing mock -type ([IValidator[string]]);
        [ProxyResult] $script:mockValidationErrorWriter = doing mock -type ([IValidationErrorWriter]);
        [ProxyResult] $script:mockFileManager = doing mock -type ([IFileManager]);
        [ProxyResult] $script:mockLogger = doing mock -type ([ILogger]);
        [ProxyResult] $script:mockReadProcessLocation = doing mock -type ([IReadProcessLocation]);
        [ProxyResult] $script:mockSetProcessLocation = doing mock -type ([ISetProcessLocation]);

        [char] $sep = [DoFramework.Environment.Environment]::Separator;
        New-Item -ItemType Directory -Path "$(Get-Location)$($sep)testdir";
        New-Item -ItemType File -Path "$(Get-Location)$($sep)testdir$($sep)dofile.ps1";
    }

    AfterEach {
        Remove-Item -Path "$(Get-Location)$($sep)testdir" -Recurse -Force;
    }

    Context 'Tests' {
        it 'Do File Not Found' {
            # Arrange
            [string] $target = "MyTarget";

            $script:mockReadProcessLocation.Proxy.MockMethod("Read", {
                return [string]::Empty;
            });

            $script:mockFileManager.Proxy.MockMethod("FileExists", {
                param (
                    [string] $path
                )

                return $false;
            });

            [DoFileInvoker] $sut = [DoFileInvoker]::new(
                $script:parameters,
                $script:mockValidator.Instance,
                $script:mockValidationErrorWriter.Instance,
                $script:mockFileManager.Instance,
                $script:mockLogger.Instance,
                $script:mockReadProcessLocation.Instance,
                $script:mockSetProcessLocation.Instance
            );

            # Act
            $sut.InvokeTarget($target);

            # Assert
            $script:mockReadProcessLocation.Proxy.CountCalls("Read") | Should -Be 1;
            $script:mockLogger.Proxy.CountCalls("LogFatal", (doing args -message "Could not locate 'dofile.ps1' in the current directory.")) | Should -Be 1;
        }
        
        it 'Exception is Caught - multiple Targets with same name' {
            # Arrange
            [string] $target = "MyTarget";

            $script:mockReadProcessLocation.Proxy.MockMethod("Read", {
                return ("$(Get-Location)$($sep)testdir").ToString();
            });

            $script:mockFileManager.Proxy.MockMethod("FileExists", {
                param (
                    [string] $path
                )

                return $true;
            });
            
            Set-Content -Path "$(Get-Location)$($sep)testdir$($sep)dofile.ps1" -Value @"
Target $target {
}

Target $target {
}
"@;

            [DoFileInvoker] $sut = [DoFileInvoker]::new(
                $script:parameters,
                $script:mockValidator.Instance,
                $script:mockValidationErrorWriter.Instance,
                $script:mockFileManager.Instance,
                $script:mockLogger.Instance,
                $script:mockReadProcessLocation.Instance,
                $script:mockSetProcessLocation.Instance
            );

            # Act
            $sut.InvokeTarget($target);

            # Assert
            $script:mockReadProcessLocation.Proxy.CountCalls("Read") | Should -Be 1;
            $script:mockSetProcessLocation.Proxy.CountCalls("Set") | Should -Be 1;
            $script:mockLogger.Proxy.CountCalls("LogError", (doing args -message "Error whilst attempting to execute target '$target'.")) | Should -Be 1;
            $script:mockLogger.Proxy.CountCalls("LogError", (doing args -message "Multiple targets with the name 'MyTarget' specified.")) | Should -Be 1;
        }

        it 'Target fails validation' {
            # Arrange
            [string] $target = "MyTarget";
            [string] $validationError1 = "Error_1";
            [string] $validationError2 = "Error_2";

            $script:mockReadProcessLocation.Proxy.MockMethod("Read", {
                return ("$(Get-Location)$($sep)testdir").ToString();
            });

            $script:mockFileManager.Proxy.MockMethod("FileExists", {
                param (
                    [string] $path
                )

                return $true;
            });

            [List[string]] $errors = [List[string]]::new();
            $errors.Add($validationError1);
            $errors.Add($validationError2);

            [IValidationResult] $validationResult = [ValidationResult]::new($errors);

            $script:mockValidator.Proxy.MockMethod("Validate", {
                param (
                    [string] $item
                )

                return $validationResult;
            });

            [DoFileInvoker] $sut = [DoFileInvoker]::new(
                $script:parameters,
                $script:mockValidator.Instance,
                $script:mockValidationErrorWriter.Instance,
                $script:mockFileManager.Instance,
                $script:mockLogger.Instance,
                $script:mockReadProcessLocation.Instance,
                $script:mockSetProcessLocation.Instance
            );

            # Act
            $sut.InvokeTarget($target);

            # Assert
            $script:mockReadProcessLocation.Proxy.CountCalls("Read") | Should -Be 1;
            $script:mockSetProcessLocation.Proxy.CountCalls("Set") | Should -Be 1;
            $script:mockValidationErrorWriter.Proxy.CountCalls("Write", (doing args -validationResult $validationResult)) | Should -Be 1;
        }

        it 'Sets variables and invokes target, implicit types' {
            # Arrange
            [string] $target = "MyTarget";
            [string] $global:outputString = $null;

            Set-Content -Path "$(Get-Location)$($sep)testdir$($sep)dofile.ps1" -Value @"
`$wrench = `$null;
`$something = `$false;

Target $target {
    `$global:outputString = "`$wrench `$something";
}
"@;

            $script:parameters.Parameters["something"] = $true;
            $script:parameters.Parameters["wrench"] = "spanner";

            $script:mockReadProcessLocation.Proxy.MockMethod("Read", {
                return ("$(Get-Location)$($sep)testdir").ToString();
            });

            $script:mockFileManager.Proxy.MockMethod("FileExists", {
                param (
                    [string] $path
                )

                return $true;
            });

            [IValidationResult] $validationResult = [ValidationResult]::new([List[string]]::new());

            $script:mockValidator.Proxy.MockMethod("Validate", {
                param (
                    [string] $item
                )

                return $validationResult;
            });

            [DoFileInvoker] $sut = [DoFileInvoker]::new(
                $script:parameters,
                $script:mockValidator.Instance,
                $script:mockValidationErrorWriter.Instance,
                $script:mockFileManager.Instance,
                $script:mockLogger.Instance,
                $script:mockReadProcessLocation.Instance,
                $script:mockSetProcessLocation.Instance
            );

            # Act
            $sut.InvokeTarget($target);

            # Assert
            $script:mockReadProcessLocation.Proxy.CountCalls("Read") | Should -Be 1;
            $script:mockSetProcessLocation.Proxy.CountCalls("Set") | Should -Be 1;

            $global:outputString | Should -Be "spanner True"
        }

        it 'Sets variables and invokes target, explicit types' {
            # Arrange
            [string] $target = "MyTarget";
            [string] $global:outputString = $null;

            Set-Content -Path "$(Get-Location)$($sep)testdir$($sep)dofile.ps1" -Value @"
[string] `$wrench = `$null;
[bool] `$something = `$false;

Target $target {
    `$global:outputString = "`$wrench `$something";
}
"@;

            $script:parameters.Parameters["something"] = $true;
            $script:parameters.Parameters["wrench"] = "spanner";

            $script:mockReadProcessLocation.Proxy.MockMethod("Read", {
                return ("$(Get-Location)$($sep)testdir").ToString();
            });

            $script:mockFileManager.Proxy.MockMethod("FileExists", {
                param (
                    [string] $path
                )

                return $true;
            });

            [IValidationResult] $validationResult = [ValidationResult]::new([List[string]]::new());

            $script:mockValidator.Proxy.MockMethod("Validate", {
                param (
                    [string] $item
                )

                return $validationResult;
            });

            [DoFileInvoker] $sut = [DoFileInvoker]::new(
                $script:parameters,
                $script:mockValidator.Instance,
                $script:mockValidationErrorWriter.Instance,
                $script:mockFileManager.Instance,
                $script:mockLogger.Instance,
                $script:mockReadProcessLocation.Instance,
                $script:mockSetProcessLocation.Instance
            );

            # Act
            $sut.InvokeTarget($target);

            # Assert
            $script:mockReadProcessLocation.Proxy.CountCalls("Read") | Should -Be 1;
            $script:mockSetProcessLocation.Proxy.CountCalls("Set") | Should -Be 1;

            $global:outputString | Should -Be "spanner True"
        }

        it 'Sets variables and invokes target, default values' {
            # Arrange
            [string] $target = "MyTarget";
            [string] $global:outputString = $null;

            Set-Content -Path "$(Get-Location)$($sep)testdir$($sep)dofile.ps1" -Value @"
[string] `$wrench = 'some str';
[bool] `$something = `$false;

Target $target {
    `$global:outputString = "`$wrench `$something";
}
"@;

            $script:mockReadProcessLocation.Proxy.MockMethod("Read", {
                return ("$(Get-Location)$($sep)testdir").ToString();
            });

            $script:mockFileManager.Proxy.MockMethod("FileExists", {
                param (
                    [string] $path
                )

                return $true;
            });

            [IValidationResult] $validationResult = [ValidationResult]::new([List[string]]::new());

            $script:mockValidator.Proxy.MockMethod("Validate", {
                param (
                    [string] $item
                )

                return $validationResult;
            });

            [DoFileInvoker] $sut = [DoFileInvoker]::new(
                $script:parameters,
                $script:mockValidator.Instance,
                $script:mockValidationErrorWriter.Instance,
                $script:mockFileManager.Instance,
                $script:mockLogger.Instance,
                $script:mockReadProcessLocation.Instance,
                $script:mockSetProcessLocation.Instance
            );

            # Act
            $sut.InvokeTarget($target);

            # Assert
            $script:mockReadProcessLocation.Proxy.CountCalls("Read") | Should -Be 1;
            $script:mockSetProcessLocation.Proxy.CountCalls("Set") | Should -Be 1;

            $global:outputString | Should -Be "some str False"
        }

        it 'Sets variables and invokes target, no defaults specified' {
            # Arrange
            [string] $target = "MyTarget";
            [string] $global:outputString = $null;

            Set-Content -Path "$(Get-Location)$($sep)testdir$($sep)dofile.ps1" -Value @"
Target $target {
    `$global:outputString = "`$wrench `$something";
}
"@;

            $script:parameters.Parameters["something"] = $true;
            $script:parameters.Parameters["wrench"] = "spanner";

            $script:mockReadProcessLocation.Proxy.MockMethod("Read", {
                return ("$(Get-Location)$($sep)testdir").ToString();
            });

            $script:mockFileManager.Proxy.MockMethod("FileExists", {
                param (
                    [string] $path
                )

                return $true;
            });

            [IValidationResult] $validationResult = [ValidationResult]::new([List[string]]::new());

            $script:mockValidator.Proxy.MockMethod("Validate", {
                param (
                    [string] $item
                )

                return $validationResult;
            });

            [DoFileInvoker] $sut = [DoFileInvoker]::new(
                $script:parameters,
                $script:mockValidator.Instance,
                $script:mockValidationErrorWriter.Instance,
                $script:mockFileManager.Instance,
                $script:mockLogger.Instance,
                $script:mockReadProcessLocation.Instance,
                $script:mockSetProcessLocation.Instance
            );

            # Act
            $sut.InvokeTarget($target);

            # Assert
            $script:mockReadProcessLocation.Proxy.CountCalls("Read") | Should -Be 1;
            $script:mockSetProcessLocation.Proxy.CountCalls("Set") | Should -Be 1;

            $global:outputString | Should -Be "spanner True"
        }
    }
}