using DoFramework.Domain;
using DoFramework.Environment;

namespace DoFramework.Data;

/// <summary>
/// Resolves processes using the provided environment and data collection provider.
/// </summary>
/// <param name="environment">The environment interface for retrieving directory information.</param>
/// <param name="provider">The data collection provider for retrieving process descriptors.</param>
public class ProcessResolver(IEnvironment environment, IDataCollectionProvider<ProcessDescriptor, string> provider)
    : Resolver<ProcessDescriptor>(provider, environment.ProcessesDir)
{
}
