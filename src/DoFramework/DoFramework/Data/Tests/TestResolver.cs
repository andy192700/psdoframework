using DoFramework.Domain;
using DoFramework.Environment;

namespace DoFramework.Data;

/// <summary>
/// Resolves tests using the provided environment and data collection provider.
/// </summary>
/// <param name="environment">The environment interface for retrieving directory information.</param>
/// <param name="provider">The data collection provider for retrieving test descriptors.</param>
public class TestResolver(IEnvironment environment, IDataCollectionProvider<TestDescriptor, string> provider)
    : Resolver<TestDescriptor>(provider, environment.TestsDir)
{
}
