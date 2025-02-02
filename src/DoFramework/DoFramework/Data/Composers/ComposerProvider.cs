using DoFramework.Domain;
using System.Text.RegularExpressions;

namespace DoFramework.Data;

/// <summary>
/// Provides a collection of composer descriptors based on a specified filter.
/// </summary>
/// <param name="readProjectContents">The data provider for reading project contents.</param>
public class ComposerProvider(ISimpleDataProvider<ProjectContents> readProjectContents) : IDataCollectionProvider<ComposerDescriptor, string>
{
    private readonly ISimpleDataProvider<ProjectContents> _readProjectContents = readProjectContents;

    /// <inheritdoc/>
    public List<ComposerDescriptor> Provide(string filter)
    {
        var composers = _readProjectContents.Provide().Composers;

        return composers.Where(m => Regex.IsMatch(m.Name!, filter, RegexOptions.CultureInvariant)).ToList();
    }
}

