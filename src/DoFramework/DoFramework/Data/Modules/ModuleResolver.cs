using DoFramework.Domain;
using DoFramework.Environment;

namespace DoFramework.Data;

/// <summary>
/// Resolves modules using the provided environment and data collection provider.
/// </summary>
/// <param name="environment">The environment interface for retrieving directory information.</param>
/// <param name="provider">The data collection provider for retrieving module descriptors.</param>
public class ModuleResolver(IEnvironment environment, IDataCollectionProvider<ModuleDescriptor, string> provider)
    : Resolver<ModuleDescriptor>(provider, environment.ModuleDir)
{
}
