using namespace DoFramework.FileSystem;
using namespace DoFramework.Logging;
using namespace DoFramework.Testing;

Describe 'DoFileCreatorTests' {
    BeforeEach {
        [string] $script:sep = [DoFramework.Environment.Environment]::Separator.ToString();

        Invoke-Expression -Command "$(Get-Content -Raw "$PSScriptRoot$($sep)..$($sep)..$($sep)..$($sep)..$($sep)Objects$($sep)FileSystem$($sep)DoFileCreator.psm1")";

        [ProxyResult] $script:fsManager = doing mock -type ([IFileManager]);

        [ProxyResult] $script:logger = doing mock -type ([ILogger]);

        [ProxyResult] $script:readLocation = doing mock -type ([IReadProcessLocation]);

        $script:readLocation.Proxy.MockMethod("Read", {
            return [string]::Empty;
        });
        
        Mock Set-Content { };
    }
    
    Context 'Tests' {
        it 'Creates file' {
            # Arrange
            $script:fsManager.Proxy.MockMethod("FileExists", {
                param (
                    [string] $path
                )

                return $false;
            });

            [DoFileCreator] $sut = [DoFileCreator]::new(
                $script:fsManager.Instance, 
                $script:logger.Instance, 
                $script:readLocation.Instance);

            # Act
            $sut.Create();

            # Assert
            [string] $content = @'
$myVar = "hello world!!!";
$theBool = $false;

Target A {
    if ($theBool) {
        Write-Host "boolset";
    }

    Write-Host $myVar $theBool;
}

Target B -inherits A {
    Write-Host "the end";
}

Target C {}
'@;

            $script:readLocation.Proxy.CountCalls("Read") | Should -Be 1;

            Should -Invoke Set-Content -Times 1 -Exactly -ParameterFilter { $Path -eq "$($script:readLocation.Instance.Read())$($script:sep)dofile.ps1" -and $Value -eq $content};
            
            $script:logger.Proxy.CountCalls("LogInfo", (doing args -message "Successfully created 'dofile.ps1.' in the current directory.")) | Should -Be 1;
        }
        
        it 'Does not create file as already exists' {
            # Arrange
            $script:fsManager.Proxy.MockMethod("FileExists", {
                param (
                    [string] $path
                )

                return $true;
            });

            [DoFileCreator] $sut = [DoFileCreator]::new(
                $script:fsManager.Instance, 
                $script:logger.Instance, 
                $script:readLocation.Instance);

            # Act
            $sut.Create();

            # Assert
            $script:readLocation.Proxy.CountCalls("Read") | Should -Be 1;

            Should -Invoke Set-Content -Times 0;
            
            $script:logger.Proxy.CountCalls("LogInfo") | Should -Be 0;
            
            $script:logger.Proxy.CountCalls("LogFatal", (doing args -message "'dofile.ps1' already exists in the current directory.")) | Should -Be 1;            
        }
    }
}