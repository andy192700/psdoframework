using DoFramework.Domain;
using DoFramework.FileSystem;
using System.IO;

namespace DoFramework.Mappers;

/// <summary>
/// Abstract class for mapping a string to a descriptor.
/// </summary>
/// <typeparam name="TDescriptor">The type of the descriptor.</typeparam>
public abstract class DescriptorMapper<TDescriptor> : IMapper<string, TDescriptor> where TDescriptor : IDescriptor
{
    private readonly IOSSanitise _osSanitise;

    public DescriptorMapper(IOSSanitise osSanitise)
    {
        _osSanitise = osSanitise;
    }

    /// <summary>
    /// Maps a string to a descriptor.
    /// </summary>
    /// <param name="source">The source string.</param>
    /// <returns>The descriptor mapped from the source string.</returns>
    public TDescriptor Map(string source) 
        => MapInternal(_osSanitise.Sanitise(source));

    /// <summary>
    /// Maps the source string to a descriptor (to be implemented by subclasses).
    /// </summary>
    /// <param name="source">The source string.</param>
    /// <returns>The descriptor mapped from the source string.</returns>
    protected abstract TDescriptor MapInternal(string source);

    /// <summary>
    /// Reads the name from a file path.
    /// </summary>
    /// <param name="path">The file path.</param>
    /// <returns>The name read from the file path.</returns>
    protected string ReadNameFromPath(string path)
    {
        var pathSplit = path.Split(Environment.Environment.Separator);

        return pathSplit[pathSplit.Length - 1].Split('.')[0];
    }
}
