using module "..\CLI\CLIFunctions\Management\Processes\AddProcess.psm1";
using module "..\CLI\CLIFunctions\Management\Processes\RemoveProcess.psm1";
using module "..\CLI\CLIFunctions\Management\Modules\AddModule.psm1";
using module "..\CLI\CLIFunctions\Management\Modules\RemoveModule.psm1";
using module "..\CLI\CLIFunctions\Management\Tests\AddTest.psm1";
using module "..\CLI\CLIFunctions\Management\Tests\RemoveTest.psm1";
using module "..\CLI\CLIFunctions\Management\Composers\AddComposer.psm1";
using module "..\CLI\CLIFunctions\Management\Composers\RemoveComposer.psm1";
using module "..\CLI\CLIFunctions\Management\CreateProject.psm1";
using module "..\CLI\CLIFunctions\Invocation\Compose.psm1";
using module "..\CLI\CLIFunctions\Invocation\RunProcess.psm1";
using module "..\CLI\CLIFunctions\Invocation\RunTests.psm1";
using module "..\CLI\CLIFunctions\Util\ReadArgs.psm1";
using module "..\CLI\CLIFunctions\Util\GetMethodInfo.psm1";
using module "..\CLI\CLIFunctions\Util\CreateMock.psm1";
using module "..\Environment\ReadProcessLocation.psm1";

using namespace DoFramework.CLI;
using namespace DoFramework.Data;
using namespace DoFramework.Domain;
using namespace DoFramework.Mappers;
using namespace DoFramework.Logging;
using namespace DoFramework.Services;
using namespace DoFramework.Validators;
using namespace DoFramework.Types;
using namespace DoFramework.Modules;
using namespace DoFramework.Environment;
using namespace System.Collections.Generic;

<#
.SYNOPSIS
Class for creating the CLI function service container within the DoFramework environment.

.DESCRIPTION
The CLIFunctionServiceContainer class is designed to register and configure all 
necessary CLI function services within the DoFramework environment. It sets up various 
service dependencies and returns an initialized service container.
#>
class CLIFunctionServiceContainer {
    <#
    .SYNOPSIS
    Creates and configures the CLI function service container.

    .DESCRIPTION
    The Create method registers all necessary CLI function services, including 
    validators, loggers, mappers, and various CLI functions. It returns an 
    initialized service container with all dependencies.
    #>
    static [IServiceContainer] Create() {
        [IServiceContainer] $container = [ServiceContainer]::new();

        $container.RegisterService[IConsoleWrapper, ConsoleWrapper]();
        $container.RegisterService[ILogger, Logger]();
        $container.RegisterService[IValidationErrorWriter, ValidationErrorWriter]();
        $container.RegisterService[CLIArgValidator]();
        $container.RegisterService[Compose]();
        $container.RegisterService[RunProcess]();
        $container.RegisterService[ArgMapper]();
        $container.RegisterService[AddProcess]();
        $container.RegisterService[AddModule]();
        $container.RegisterService[AddTest]();
        $container.RegisterService[AddComposer]();
        $container.RegisterService[CreateProject]();
        $container.RegisterService[RemoveModule]();
        $container.RegisterService[RemoveTest]();
        $container.RegisterService[RemoveProcess]();
        $container.RegisterService[RemoveComposer]();
        $container.RegisterService[RunTests]();
        $container.RegisterService[ReadArgs]();
        $container.RegisterService[GetMethodInfo]();
        $container.RegisterService[CreateMock]();

        return $container;
    }
}
