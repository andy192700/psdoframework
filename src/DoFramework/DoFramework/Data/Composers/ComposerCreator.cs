using DoFramework.Domain;
using DoFramework.Logging;

namespace DoFramework.Data;

/// <summary>
/// Responsible for creating and registering a composer.
/// </summary>
/// <param name="saveProjectContents">The data creator for saving project contents.</param>
/// <param name="readProjectContents">The data provider for reading project contents.</param>
/// <param name="logger">The logger for logging information.</param>
public class ComposerCreator(
    IDataCreator<ProjectContents> saveProjectContents,
    ISimpleDataProvider<ProjectContents> readProjectContents,
    ILogger logger) : IDataCreator<ComposerDescriptor>
{
    private readonly IDataCreator<ProjectContents> _saveProjectContents = saveProjectContents;
    private readonly ISimpleDataProvider<ProjectContents> _readProjectContents = readProjectContents;
    private readonly ILogger _logger = logger;

    /// <inheritdoc/>
    public void Create(ComposerDescriptor item)
    {
        var contents = _readProjectContents.Provide();

        contents.Composers.Add(item);

        _saveProjectContents.Create(contents);

        _logger.LogInfo($"Composer registered at {item.Name} with path {item.Path}");
    }
}
