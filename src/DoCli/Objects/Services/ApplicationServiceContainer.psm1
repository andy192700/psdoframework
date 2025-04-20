using module "..\FileSystem\DoFileCreator.psm1";
using module "..\FileSystem\ReadProcessLocation.psm1";
using module "..\FileSystem\SetProcessLocation.psm1";
using module "..\Processing\ProcessBuilder.psm1";
using module "..\Processing\DisplayReports.psm1";
using module "..\Processing\DoFileInvoker.psm1";
using module "..\Testing\ModuleTestRunner.psm1";
using module "..\Testing\ProcessTesterRunner.psm1";
using module "..\Testing\ComposerTesterRunner.psm1";
using module "..\Testing\PesterConfig.psm1";
using module "..\Testing\PesterRunner.psm1";
using module "..\Testing\ProxyClassTypeDefinitionBuilder.psm1";
using module "..\Mappers\RunMethodInfoMapper.psm1";
using module "..\Validators\ProxyTypeValidator.psm1";
using module "..\Validators\DoFileTargetValidator.psm1";

using namespace DoFramework.Data;
using namespace DoFramework.Domain;
using namespace DoFramework.Environment;
using namespace DoFramework.FileSystem;
using namespace DoFramework.Logging;
using namespace DoFramework.Processing;
using namespace DoFramework.Services;
using namespace DoFramework.Validators;
using namespace DoFramework.Mappers;
using namespace DoFramework.Modules;
using namespace DoFramework.Types;
using namespace DoFramework.Testing;
using namespace System.Reflection;
using namespace System.Collections.Generic;
using namespace System.Management.Automation;

<#
.SYNOPSIS
Class for creating the application service container within the DoFramework environment.

.DESCRIPTION
The ApplicationServiceContainer class is designed to register and configure all 
necessary services within the DoFramework environment. It sets up various 
service dependencies and returns an initialized service container.
#>
class ApplicationServiceContainer {
    <#
    .SYNOPSIS
    Creates and configures the service container.

    .DESCRIPTION
    The Create method registers all necessary services, including file management, 
    JSON conversion, environment settings, data providers, mappers, validators, 
    and more. It returns an initialized service container with all dependencies.
    #>
    static [IServiceContainer] Create() {
        [IServiceContainer] $container = [ServiceContainer]::new();

        # Essentials
        $container.RegisterService[IFileManager, FileManager]();
        $container.RegisterService[IJsonConverter, JsonConverter]();
        $container.RegisterService[IEnvironment, DoFramework.Environment.Environment]();
        $container.RegisterService[ISetProcessLocation, SetProcessLocation]();
        $container.RegisterService[IReadProcessLocation, ReadProcessLocation]();
        $container.RegisterService[ILogger, Logger]();
        $container.RegisterService[IConsoleWrapper, ConsoleWrapper]();
        $container.RegisterService[IMapper[string, TestDescriptor], TestDescriptorMapper]();
        $container.RegisterService[IMapper[string, ProcessDescriptor], ProcessDescriptorMapper]();
        $container.RegisterService[IMapper[string, ModuleDescriptor], ModuleDescriptorMapper]();
        $container.RegisterService[IMapper[string, ComposerDescriptor], ComposerDescriptorMapper]();
        $container.RegisterService[IResolver[ProcessDescriptor], ProcessResolver]();
        $container.RegisterService[IResolver[ModuleDescriptor], ModuleResolver]();
        $container.RegisterService[IResolver[ComposerDescriptor], ComposerResolver]();
        $container.RegisterService[IResolver[TestDescriptor], TestResolver]();
        $container.RegisterService[IOSSanitise, OSSanitise]();
        $container.RegisterService[ISimpleDataProvider[ProjectContents], ReadProjectContents]();
        $container.RegisterService[IDataCollectionProvider[TestDescriptor, string], TestProvider]();
        $container.RegisterService[IDataCollectionProvider[ProcessDescriptor, string], ProcessProvider]();
        $container.RegisterService[IDataCollectionProvider[ModuleDescriptor, string], ModuleProvider]();
        $container.RegisterService[IDataCollectionProvider[ComposerDescriptor, string], ComposerProvider]();

        # Processing
        $container.RegisterService[ISimpleDataProvider[Dictionary[string, object]], EnvFileDataProvider]();
        $container.RegisterService[IValidationErrorWriter, ValidationErrorWriter]();
        $container.RegisterService[IValidator[IDescriptor], DescriptorCreatorValidator]();
        $container.RegisterService[IContext, Context]();
        $container.RegisterService[ISession, Session]();
        $container.RegisterService[IDisplayReports, DisplayReports]();
        $container.RegisterService[IContextWriter, ContextWriter]();
        $container.RegisterService[IConsumeEnvFiles, ConsumeEnvFiles]();
        $container.RegisterService[IValidator[string], DoFileTargetValidator]();
        $container.RegisterService[IDoFileInvoker, DoFileInvoker]();
        $container.RegisterService[ILookupType[IComposer], LookupComposerType]()
        $container.RegisterService[TypeValidator[IComposer], ComposerTypeValidator]()
        $container.RegisterService[IProcessRegistry, ProcessRegistry]()
        $container.RegisterService[IComposerOrchestrator, ComposerOrchestrator]()
        $container.RegisterService[IProcessInstanceRunner, ProcessInstanceRunner]()
        $container.RegisterService[IProcessExecutor, ProcessExecutor]()
        $container.RegisterService[IProcessRunner, ProcessRunner]()
        $container.RegisterService[IEntryPoint, EntryPoint]()
        $container.RegisterService[IFailedReportChecker, FailedReportChecker]()
        $container.RegisterService[ILookupType[IProcess], LookupProcessType]()
        $container.RegisterService[IValidator[IProcessingRequest], ProcessingRequestValidator]()
        $container.RegisterService[TypeValidator[IProcess], ProcessTypeValidator]()

        # Testing
        $container.RegisterService[PesterConfig]();
        $container.RegisterService[ProxyTypeValidator]();
        $container.RegisterService[ProxyClassTypeDefinitionBuilder]();
        $container.RegisterService[IPesterRunner, PesterRunner]();
        $container.RegisterService[ITestRunner[ModuleDescriptor], ModuleTestRunner]();
        $container.RegisterService[ITestRunner[ProcessDescriptor], ProcessTesterRunner]();
        $container.RegisterService[ITestRunner[ComposerDescriptor], ComposerTesterRunner]();
        $container.RegisterService[IMapper[object, MethodInfo], RunMethodInfoMapper]();

        ## writing operations
        $container.RegisterService[IDoFileCreator, DoFileCreator]();
        $container.RegisterService[IMapper[ProjectContentsStorage, ProjectContents], ReadProjectContentsMapper]();
        $container.RegisterService[IMapper[ProjectContents, ProjectContentsStorage], SaveProjectContentsMapper]();
        $container.RegisterService[IDataCreator[TestDescriptor], TestCreator]();
        $container.RegisterService[IDataDeletor[TestDescriptor], TestDeletor]();
        $container.RegisterService[IDataCreator[ProcessDescriptor], ProcessCreator]();
        $container.RegisterService[IDataDeletor[ProcessDescriptor], ProcessDeletor]();
        $container.RegisterService[IDataCreator[ModuleDescriptor], ModuleCreator]();
        $container.RegisterService[IDataDeletor[ModuleDescriptor], ModuleDeletor]();
        $container.RegisterService[IDataCreator[ComposerDescriptor], ComposerCreator]();
        $container.RegisterService[IDataDeletor[ComposerDescriptor], ComposerDeletor]();
        $container.RegisterService[IDataCreator[ProjectContents], SaveProjectContents]();
        $container.RegisterService[IDescriptorFileCreator[ProcessDescriptor], ProcessDescriptorFileCreator]();
        $container.RegisterService[IDescriptorFileCreator[ModuleDescriptor], ModuleDescriptorFileCreator]();
        $container.RegisterService[IDescriptorFileCreator[ComposerDescriptor], ComposerDescriptorFileCreator]();
        $container.RegisterService[IDescriptorFileCreator[TestDescriptor], TestDescriptorFileCreator]();
        $container.RegisterService[TestDescriptorCreatorValidator]();

        return $container;
    }
}
