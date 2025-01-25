using DoFramework.Domain;
using DoFramework.Environment;
using System.Text;

namespace DoFramework.FileSystem;

/// <summary>
/// Abstract class for creating descriptor files.
/// </summary>
/// <typeparam name="TDescriptor">The type of the descriptor.</typeparam>
/// <param name="environment">The environment instance.</param>
/// <param name="fileManager">The file manager instance.</param>
public abstract class DescriptorFileCreator<TDescriptor>(IEnvironment environment, IFileManager fileManager)
    : IDescriptorFileCreator<TDescriptor> where TDescriptor : IDescriptor
{
    private readonly IEnvironment _environment = environment;
    private readonly IFileManager _fileManager = fileManager;

    /// <summary>
    /// Creates a descriptor file.
    /// </summary>
    /// <param name="descriptor">The descriptor instance.</param>
    public void Create(TDescriptor descriptor)
    {
        var path = $"{descriptor.GetDirectory(_environment)}{Environment.Environment.Separator}{descriptor.Path}";

        if (!_fileManager.ParentDirectoryExists(path))
        {
            _fileManager.CreateParentDirectory(path);
        }

        if (!_fileManager.FileExists(path))
        {
            var sb = new StringBuilder();

            PopulateDefinition(sb, descriptor);

            _fileManager.WriteAllText(path, sb.ToString());
        }
    }

    /// <summary>
    /// Populates the definition of the descriptor.
    /// </summary>
    /// <param name="sb">The StringBuilder instance.</param>
    /// <param name="descriptor">The descriptor instance.</param>
    protected abstract void PopulateDefinition(StringBuilder sb, TDescriptor descriptor);
}
