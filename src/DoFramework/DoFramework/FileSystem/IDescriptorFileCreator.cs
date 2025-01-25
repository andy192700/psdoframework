using DoFramework.Domain;

namespace DoFramework.FileSystem;

/// <summary>
/// Interface for creating descriptor files.
/// </summary>
/// <typeparam name="TDescriptor">The type of the descriptor.</typeparam>
public interface IDescriptorFileCreator<TDescriptor> where TDescriptor : IDescriptor
{
    /// <summary>
    /// Creates a descriptor file.
    /// </summary>
    /// <param name="descriptor">The descriptor instance.</param>
    public void Create(TDescriptor descriptor);
}
