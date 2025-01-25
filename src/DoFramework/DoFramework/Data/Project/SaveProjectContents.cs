using DoFramework.Domain;
using DoFramework.Environment;
using DoFramework.FileSystem;
using DoFramework.Logging;
using DoFramework.Mappers;

namespace DoFramework.Data;

/// <summary>
/// Responsible for saving project contents by mapping the data and writing it to a JSON file.
/// </summary>
/// <param name="environment">The environment interface for retrieving directory information.</param>
/// <param name="logger">The logger for logging information.</param>
/// <param name="mapper">The mapper for converting project contents to storage format.</param>
/// <param name="fileManager">The file manager for handling file operations.</param>
/// <param name="jsonConverter">The JSON converter for serialization.</param>
public class SaveProjectContents(
    IEnvironment environment,
    ILogger logger,
    IMapper<ProjectContents, ProjectContentsStorage> mapper,
    IFileManager fileManager,
    IJsonConverter jsonConverter) : IDataCreator<ProjectContents>
{
    private readonly IEnvironment _environment = environment;
    private readonly ILogger _logger = logger;
    private readonly IMapper<ProjectContents, ProjectContentsStorage> _mapper = mapper;
    private readonly IFileManager _fileManager = fileManager;
    private readonly IJsonConverter _jsonConverter = jsonConverter;

    /// <summary>
    /// Saves the specified project contents by mapping the data and writing it to a JSON file.
    /// </summary>
    /// <param name="contents">The project contents to be saved.</param>
    public void Create(ProjectContents contents)
    {
        var contentsToSave = _mapper.Map(contents);

        _fileManager.WriteAllText(Path.Combine(_environment.HomeDir, "do.json"), _jsonConverter.Serialize(contentsToSave));

        _logger.LogInfo("Project file updated.");
    }
}
