using DoFramework.Domain;
using DoFramework.FileSystem;
using DoFramework.Mappers;

namespace DoFramework.Data;

/// <summary>
/// Provides project contents by reading and mapping data from a JSON file.
/// </summary>
/// <param name="mapper">The mapper for converting storage format to project contents.</param>
/// <param name="readProcessLocation">The interface for reading the process location.</param>
/// <param name="fileManager">The file manager for handling file operations.</param>
/// <param name="jsonConverter">The JSON converter for deserialization.</param>
public class ReadProjectContents(
    IMapper<ProjectContentsStorage, ProjectContents> mapper,
    IReadProcessLocation readProcessLocation,
    IFileManager fileManager,
    IJsonConverter jsonConverter) : ISimpleDataProvider<ProjectContents>
{
    private readonly IMapper<ProjectContentsStorage, ProjectContents> _mapper = mapper;

    private readonly IReadProcessLocation _readProcessLocation = readProcessLocation;

    private readonly IFileManager _fileManager = fileManager;

    private readonly IJsonConverter _jsonConverter = jsonConverter;

    /// <summary>
    /// Provides the project contents by reading and mapping data from a JSON file located at the process location.
    /// </summary>
    /// <returns>The project contents.</returns>
    public ProjectContents Provide()
    {
        var jsonFilePath = Path.Combine(_readProcessLocation.Read(), "do.json");

        var jsonContent = _fileManager.ReadAllText(jsonFilePath);

        return _mapper.Map(_jsonConverter.Deserialize<ProjectContentsStorage>(jsonContent));
    }
}
