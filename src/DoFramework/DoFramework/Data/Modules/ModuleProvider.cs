using DoFramework.Domain;
using System.Text.RegularExpressions;

namespace DoFramework.Data;

/// <summary>
/// Provides a collection of module descriptors based on a specified filter.
/// </summary>
/// <param name="readProjectContents">The data provider for reading project contents.</param>
public class ModuleProvider(ISimpleDataProvider<ProjectContents> readProjectContents) : IDataCollectionProvider<ModuleDescriptor, string>
{
    private readonly ISimpleDataProvider<ProjectContents> _readProjectContents = readProjectContents;

    /// <summary>
    /// Provides a list of module descriptors that match the specified filter.
    /// </summary>
    /// <param name="filter">The filter string used to match module names.</param>
    /// <returns>A list of module descriptors that match the filter.</returns>
    public List<ModuleDescriptor> Provide(string filter)
    {
        var modules = _readProjectContents.Provide().Modules;

        return modules.Where(m => Regex.IsMatch(m.Name!, filter, RegexOptions.CultureInvariant)).ToList();
    }
}
