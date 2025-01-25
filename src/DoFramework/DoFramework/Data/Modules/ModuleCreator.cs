using DoFramework.Domain;
using DoFramework.Logging;

namespace DoFramework.Data;

/// <summary>
/// Responsible for creating and registering a module.
/// </summary>
/// <param name="saveProjectContents">The data creator for saving project contents.</param>
/// <param name="readProjectContents">The data provider for reading project contents.</param>
/// <param name="logger">The logger for logging information.</param>
public class ModuleCreator(
    IDataCreator<ProjectContents> saveProjectContents,
    ISimpleDataProvider<ProjectContents> readProjectContents,
    ILogger logger) : IDataCreator<ModuleDescriptor>
{
    private readonly IDataCreator<ProjectContents> _saveProjectContents = saveProjectContents;
    private readonly ISimpleDataProvider<ProjectContents> _readProjectContents = readProjectContents;
    private readonly ILogger _logger = logger;

    /// <summary>
    /// Creates a new module and registers it.
    /// </summary>
    /// <param name="item">The module descriptor to create and register.</param>
    public void Create(ModuleDescriptor item)
    {
        var contents = _readProjectContents.Provide();

        contents.Modules.Add(item);

        _saveProjectContents.Create(contents);

        _logger.LogInfo($"Module registered at {item.Name} with path {item.Path}");
    }
}
