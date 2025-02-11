using module "..\CLI\CLIFunctions\Management\Processes\NewProcess.psm1";
using module "..\CLI\CLIFunctions\Management\Processes\DeleteProcess.psm1";
using module "..\CLI\CLIFunctions\Management\Modules\NewModule.psm1";
using module "..\CLI\CLIFunctions\Management\Modules\DeleteModule.psm1";
using module "..\CLI\CLIFunctions\Management\Tests\NewTest.psm1";
using module "..\CLI\CLIFunctions\Management\Tests\DeleteTest.psm1";
using module "..\CLI\CLIFunctions\Management\Composers\NewComposer.psm1";
using module "..\CLI\CLIFunctions\Management\Composers\DeleteComposer.psm1";
using module "..\CLI\CLIFunctions\Management\NewProject.psm1";
using module "..\CLI\CLIFunctions\Management\NewDoFile.psm1";
using module "..\CLI\CLIFunctions\Invocation\Compose.psm1";
using module "..\CLI\CLIFunctions\Invocation\RunProcess.psm1";
using module "..\CLI\CLIFunctions\Invocation\RunTests.psm1";
using module "..\CLI\CLIFunctions\Invocation\ExecDoFile.psm1";
using module "..\CLI\CLIFunctions\Util\ReadArgs.psm1";
using module "..\CLI\CLIFunctions\Util\GetMethodInfo.psm1";
using module "..\CLI\CLIFunctions\Util\CreateMock.psm1";

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
        $container.RegisterService[NewProcess]();
        $container.RegisterService[NewModule]();
        $container.RegisterService[NewTest]();
        $container.RegisterService[NewComposer]();
        $container.RegisterService[NewProject]();
        $container.RegisterService[NewDoFile]();
        $container.RegisterService[DeleteModule]();
        $container.RegisterService[DeleteTest]();
        $container.RegisterService[DeleteProcess]();
        $container.RegisterService[DeleteComposer]();
        $container.RegisterService[RunTests]();
        $container.RegisterService[ExecDoFile]();
        $container.RegisterService[ReadArgs]();
        $container.RegisterService[GetMethodInfo]();
        $container.RegisterService[CreateMock]();

        return $container;
    }
}
