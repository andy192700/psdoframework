using DoFramework.Domain;
using DoFramework.Logging;

namespace DoFramework.Data;

public class ComposerCreator(
    IDataCreator<ProjectContents> saveProjectContents,
    ISimpleDataProvider<ProjectContents> readProjectContents,
    ILogger logger) : IDataCreator<ComposerDescriptor>
{
    private readonly IDataCreator<ProjectContents> _saveProjectContents = saveProjectContents;
    private readonly ISimpleDataProvider<ProjectContents> _readProjectContents = readProjectContents;
    private readonly ILogger _logger = logger;


    public void Create(ComposerDescriptor item)
    {
        var contents = _readProjectContents.Provide();

        contents.Composers.Add(item);

        _saveProjectContents.Create(contents);

        _logger.LogInfo($"Composer registered at {item.Name} with path {item.Path}");
    }
}
