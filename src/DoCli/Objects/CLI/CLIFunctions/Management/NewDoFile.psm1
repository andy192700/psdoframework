using namespace DoFramework.CLI;
using namespace DoFramework.FileSystem;
using namespace DoFramework.Validators;
using namespace DoFramework.Services;
using namespace System.Collections.Generic;

class NewDoFile : CLIFunction[EmptyCLIFunctionDictionaryValidator] {
    NewDoFile() : base("new-dofile") {}

    [void] InvokeInternal([Dictionary[string, object]] $params, [IServiceContainer] $serviceContainer) {
        [IDoFileCreator] $creator = $serviceContainer.GetService([IDoFileCreator]);

        $creator.Create();
    }
}