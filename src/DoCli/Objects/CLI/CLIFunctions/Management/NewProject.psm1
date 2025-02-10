using namespace DoFramework.CLI;
using namespace DoFramework.Data;
using namespace DoFramework.Domain;
using namespace DoFramework.Logging;
using namespace DoFramework.Environment;
using namespace DoFramework.FileSystem;
using namespace DoFramework.Validators;
using namespace DoFramework.Services;
using namespace System.Collections.Generic;

<#
.SYNOPSIS
Class for creating projects within the DoFramework environment.

.DESCRIPTION
The NewProject class is designed to create new projects within the DoFramework 
environment. It handles the setup of parameters, environment checks, directory 
creation, and initialization of project files.
#>
class NewProject : CLIFunction[EmptyCLIFunctionDictionaryValidator] {
    <#
    .SYNOPSIS
    Initializes a new instance of the CreateProject class.

    .DESCRIPTION
    Constructor for the CreateProject class, which sets up the base name 
    for the command as "New-Project".
    #>
    NewProject() : base("New-Project") {}
    
    <#
    .SYNOPSIS
    Invokes the process of creating a project.

    .DESCRIPTION
    The InvokeInternal method sets up parameters, checks the environment, 
    creates necessary directories, and initializes project files.
    #>
    [void] InvokeInternal([Dictionary[string, object]] $params, [IServiceContainer] $serviceContainer) {            
        [string] $doProjPath = Join-Path -Path (Get-Location) -ChildPath "do.json";

        if (!(Test-Path -Path $doProjPath)) {    

            [string] $projectName = if (!$params.ContainsKey("name")) { "Do" } else { $params["name"] };

            [ProjectContentsStorage] $contents = [ProjectContentsStorage]::new();

            $contents.Name = $projectName;

            $contents.Version = (get-module -Name PSDoFramework).Version.ToString();

            $contents.PSVersion = $global:psversiontable.PSVersion.ToString();

            [IFileManager] $fileManager = [FileManager]::new();

            [IJsonConverter] $jsonConverter = [JsonConverter]::new();

            $fileManager.WriteAllText($doProjPath, $jsonConverter.Serialize($contents));

            [ServiceContainerExtensions]::AddParameters($serviceContainer, $params);
    
            [ILogger] $logger = $serviceContainer.GetService[ILogger]();

            [IEnvironment] $environment = $serviceContainer.GetService[IEnvironment]();

            $logger.LogInfo("New project '$($projectName)' @ '$($environment.HomeDir)'");
            
            $this.CreateDirectory($environment.ProcessesDir);
            $this.CreateDirectory($environment.TestsDir);
            $this.CreateDirectory("$($environment.TestsDir)$([DoFramework.Environment.Environment]::Separator)Processes");
            $this.CreateDirectory("$($environment.TestsDir)$([DoFramework.Environment.Environment]::Separator)Modules");
            $this.CreateDirectory("$($environment.TestsDir)$([DoFramework.Environment.Environment]::Separator)Composers");
            $this.CreateDirectory($environment.ModuleDir);
            $this.CreateDirectory($environment.ComposersDir);

            New-Item -ItemType File "$($environment.HomeDir)$([DoFramework.Environment.Environment]::Separator).env";
        }
        else {
            [ILogger] $logger = [Logger]::new([ConsoleWrapper]::new());

            $logger.LogError("Do project already exists at this location.");
        }
    }

    <#
    .SYNOPSIS
    Creates a directory and a .gitkeep file within it.

    .DESCRIPTION
    The CreateDirectory method creates a new directory at the specified path 
    and adds a .gitkeep file to it.
    #>
    [void] CreateDirectory([string] $path) {            
        New-Item -ItemType Directory $path;

        New-Item -ItemType File "$path$([DoFramework.Environment.Environment]::Separator).gitkeep";
    }
}
