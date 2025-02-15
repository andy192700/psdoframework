$myVar = "hello world!!!";
$theBool = $false;
$homeDir = Get-Location;
$processName = "SimpleProcess";
$composerName = "AdvancedComposer";
$testFilter = ".*";

Target A {
    if ($theBool) {
        Write-Host boolset;
    }

    Write-Host $myVar;
}

Target B -inherits A {
    Write-Host "the end";
}

Target C {}

Target SampleProcess {
    doing run -name $processName -showReports -home "$homeDir/Sample";
}

Target SampleComposer {
    doing compose -name $composerName -showReports -home "$homeDir/Sample";
}

Target SampleTests {
    doing test -filter $testFilter -home "$homeDir/Sample";
}