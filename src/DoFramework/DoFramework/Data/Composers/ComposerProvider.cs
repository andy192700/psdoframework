using DoFramework.Domain;
using System.Text.RegularExpressions;

namespace DoFramework.Data;

public class ComposerProvider(ISimpleDataProvider<ProjectContents> readProjectContents) : IDataCollectionProvider<ComposerDescriptor, string>
{
    private readonly ISimpleDataProvider<ProjectContents> _readProjectContents = readProjectContents;

    public List<ComposerDescriptor> Provide(string filter)
    {
        var composers = _readProjectContents.Provide().Composers;

        return composers.Where(m => Regex.IsMatch(m.Name!, filter, RegexOptions.CultureInvariant)).ToList();
    }
}

