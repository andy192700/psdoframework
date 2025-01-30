using DoFramework.Domain;
using DoFramework.Environment;

namespace DoFramework.Data;

public class ComposerResolver(IEnvironment environment, IDataCollectionProvider<ComposerDescriptor, string> provider)
    : Resolver<ComposerDescriptor>(provider, environment.ModuleDir)
{
}
