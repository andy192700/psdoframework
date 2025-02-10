solutionFile='.\src\DoFramework\DoFramework.sln'
solutionConfig=Release
psNuGetSourceName=LocalNuGetRepository
psNuGetSourceLocation=.\LocalNuGetRepository
psNuGetApiKey=''
version=1.0.0
composerName=AdvancedComposer
processName=SimpleProcess
testFilter=.*

# Validation
validate: 
	@echo "GNU Make version: $(MAKE_VERSION)"

# Dotnet CLI
dotnetbuild:
	dotnet build $(solutionFile) --configuration $(solutionConfig) '/p:Version=$(version)'

dotnettest:
	dotnet test $(solutionFile) --configuration $(solutionConfig) --no-build --logger "trx;LogFileName=test-results.trx" --results-directory ./test-results

dotnetpublish:
	pwsh -ExecutionPolicy Bypass -Command "& '${CURDIR}/Scripts/dotnettoolpublish.ps1' -psNuGetSourceName $(psNuGetSourceName) -solutionFile $(solutionFile) -solutionConfig $(solutionConfig) -psNuGetApiKey $(psNuGetApiKey);"

# DoFramework PowerShell Testing
pstests:
	pwsh -ExecutionPolicy Bypass -Command "& '${CURDIR}/Scripts/PSTestOrchestrator.ps1';"
	
pstestspostinstall:
	pwsh -ExecutionPolicy Bypass -Command "& '${CURDIR}/Scripts/PSTestOrchestrator.ps1' -useLatest -psNuGetSourceName $(psNuGetSourceName);"

# Module deployment
createmanifest:
	pwsh -ExecutionPolicy Bypass -Command "& '${CURDIR}/Scripts/CreateManifest.ps1' -psNuGetSourceName $(psNuGetSourceName)"

publishmodule:
	pwsh -ExecutionPolicy Bypass -Command "& '${CURDIR}/Scripts/PublishPowerShellModule.ps1' -psNuGetSourceName $(psNuGetSourceName) -psNuGetApiKey $(psNuGetApiKey)"

installmodule:
	pwsh -ExecutionPolicy Bypass -Command "& '${CURDIR}/Scripts/InstallModule.ps1' -psNuGetSourceName $(psNuGetSourceName)"

# Local Development support features
createlocalnugetsource:
	pwsh -ExecutionPolicy Bypass -Command "& '${CURDIR}/Scripts/CreateLocalNuGetSource.ps1' -psNuGetSourceName $(psNuGetSourceName) -psNuGetSourceLocation $(psNuGetSourceLocation)"

removelocalnugetsource:
	pwsh -ExecutionPolicy Bypass -Command "& '${CURDIR}/Scripts/RemoveLocalNuGetSource.ps1' -psNuGetSourceName $(psNuGetSourceName) -psNuGetSourceLocation $(psNuGetSourceLocation)"

localbuild:
	pwsh -ExecutionPolicy Bypass -Command "& '${CURDIR}/Scripts/LocalBuild.ps1' -psNuGetSourceName $(psNuGetSourceName)"

localbuildSkipTests:
	pwsh -ExecutionPolicy Bypass -Command "& '${CURDIR}/Scripts/LocalBuild.ps1' -psNuGetSourceName $(psNuGetSourceName) -skipTests"

echopsversion:
	pwsh -ExecutionPolicy Bypass -Command '& Write-Host "PSVersion: $$($$PSVersionTable.PSVersion.ToString())"'

# Run Samples
runsampleprocess:
	pwsh -ExecutionPolicy Bypass -Command "doing run -name $(processName) -showReports -projectPath \"${CURDIR}/Sample\""
	
runsamplecomposer:
	pwsh -ExecutionPolicy Bypass -Command "doing compose -name $(composerName) -showReports -projectPath \"${CURDIR}/Sample\""

runsampletests:
	pwsh -ExecutionPolicy Bypass -Command "doing test -filter $(testFilter) -projectPath \"${CURDIR}/Sample\""
	
runsampleprocesswithtool:
	psdoing run -name $(processName) -showReports -projectPath "${CURDIR}/Sample"
	
runsamplecomposerwithtool:
	psdoing compose -name $(composerName) -showReports -projectPath "${CURDIR}/Sample"

runsampletestswithtool:
	psdoing test -filter $(testFilter) -projectPath "${CURDIR}/Sample"

# Pipeline
gitrelease:
	pwsh -ExecutionPolicy Bypass -Command "& '${CURDIR}/Scripts/GitRelease.ps1'"