using DoFramework.Domain;
using System.Text.RegularExpressions;

namespace DoFramework.Data;

/// <summary>
/// Provides a collection of process descriptors based on a specified filter.
/// </summary>
/// <param name="readProjectContents">The data provider for reading project contents.</param>
public class ProcessProvider(ISimpleDataProvider<ProjectContents> readProjectContents) : IDataCollectionProvider<ProcessDescriptor, string>
{
    private readonly ISimpleDataProvider<ProjectContents> _readProjectContents = readProjectContents;

    /// <summary>
    /// Provides a list of process descriptors that match the specified filter.
    /// </summary>
    /// <param name="filter">The filter string used to match process names.</param>
    /// <returns>A list of process descriptors that match the filter.</returns>
    public List<ProcessDescriptor> Provide(string filter)
    {
        var processes = _readProjectContents.Provide().Processes;

        return processes.Where(proc => Regex.IsMatch(proc.Name!, filter, RegexOptions.CultureInvariant)).ToList();
    }
}
