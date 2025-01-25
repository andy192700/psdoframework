using DoFramework.Domain;
using DoFramework.Environment;
using DoFramework.FileSystem;
using DoFramework.Logging;

namespace DoFramework.Data;

/// <summary>
/// Responsible for deleting a test and updating the project contents accordingly.
/// </summary>
/// <param name="saveProjectContents">The data creator for saving project contents.</param>
/// <param name="readProjectContents">The data provider for reading project contents.</param>
/// <param name="environment">The environment interface for retrieving directory information.</param>
/// <param name="fileManager">The file manager for handling file operations.</param>
/// <param name="logger">The logger for logging information and warnings.</param>
public class TestDeletor(
    IDataCreator<ProjectContents> saveProjectContents,
    ISimpleDataProvider<ProjectContents> readProjectContents,
    IEnvironment environment,
    IFileManager fileManager,
    ILogger logger) : IDataDeletor<TestDescriptor>
{
    private readonly IDataCreator<ProjectContents> _saveProjectContents = saveProjectContents;
    private readonly ISimpleDataProvider<ProjectContents> _readProjectContents = readProjectContents;
    private readonly IEnvironment _environment = environment;
    private readonly IFileManager _fileManager = fileManager;
    private readonly ILogger _logger = logger;

    /// <summary>
    /// Deletes the specified test and updates the project contents.
    /// </summary>
    /// <param name="item">The test descriptor to be deleted.</param>
    public void Delete(TestDescriptor item)
    {
        ProjectContents contents = _readProjectContents.Provide();

        var descriptor = contents.Tests.FirstOrDefault(x => x.Name!.Equals(item.Name));

        if (descriptor is not null)
        {
            contents.Tests.Remove(descriptor);

            _saveProjectContents.Create(contents);

            _fileManager.DeleteFile($"{_environment.TestsDir}{Environment.Environment.Separator}{descriptor.Path!}");

            _logger.LogInfo($"Test: {item.Name} removed.");
        }
        else
        {
            _logger.LogWarning($"Test: {item.Name} could not be located for removal.");
        }
    }
}
