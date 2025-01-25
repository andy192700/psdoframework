using DoFramework.Domain;
using System.Text.RegularExpressions;

namespace DoFramework.Data;

/// <summary>
/// Provides a collection of test descriptors based on a specified filter.
/// </summary>
/// <param name="readProjectContents">The data provider for reading project contents.</param>
public class TestProvider(ISimpleDataProvider<ProjectContents> readProjectContents) : IDataCollectionProvider<TestDescriptor, string>
{
    private readonly ISimpleDataProvider<ProjectContents> _readProjectContents = readProjectContents;

    /// <summary>
    /// Provides a list of test descriptors that match the specified filter.
    /// </summary>
    /// <param name="filter">The filter string used to match test names.</param>
    /// <returns>A list of test descriptors that match the filter.</returns>
    public List<TestDescriptor> Provide(string filter)
    {
        List<TestDescriptor> tests = _readProjectContents.Provide().Tests;

        return tests.Where(test => Regex.IsMatch(test.Name!, filter, RegexOptions.CultureInvariant)).ToList();
    }
}
