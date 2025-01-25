using DoFramework.Domain;

namespace DoFramework.Data;

/// <summary>
/// Abstract base class for resolving descriptors using a data collection provider and directory path.
/// </summary>
/// <typeparam name="TDescriptor">The type of descriptor to resolve, which must implement the IDescriptor interface.</typeparam>
/// <param name="provider">The data collection provider for retrieving descriptors.</param>
/// <param name="directoryPath">The directory path where the descriptors are located.</param>
public abstract class Resolver<TDescriptor>(IDataCollectionProvider<TDescriptor, string> provider, string directoryPath) : IResolver<TDescriptor> where TDescriptor : IDescriptor
{
    /// <summary>
    /// Gets or sets the directory path where the descriptors are located.
    /// </summary>
    private string DirectoryPath { get; set; } = directoryPath;

    private readonly IDataCollectionProvider<TDescriptor, string> _provider = provider;

    /// <summary>
    /// Resolves a descriptor based on the specified module name.
    /// </summary>
    /// <param name="module">The name of the module to resolve.</param>
    /// <returns>A resolution result containing the resolved descriptor.</returns>
    public ResolutionResult<TDescriptor> Resolve(string module)
    {
        var items = _provider.Provide(module);

        var descriptor = items.FirstOrDefault(m => m.Name == module);

        if (descriptor != null)
        {
            return new(true, $"{DirectoryPath}{Environment.Environment.Separator}{descriptor.Path}", descriptor);
        }

        return new(false, default, default!);
    }
}
