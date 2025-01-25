using DoFramework.Domain;
using DoFramework.FileSystem;

namespace DoFramework.Mappers;

/// <summary>
/// Class for mapping a string to a process descriptor.
/// </summary>
public class ProcessDescriptorMapper : DescriptorMapper<ProcessDescriptor>
{
    public ProcessDescriptorMapper(IOSSanitise osSanitise) : base(osSanitise) { }

    /// <summary>
    /// Maps the source string to a process descriptor.
    /// </summary>
    /// <param name="source">The source string.</param>
    /// <returns>The process descriptor mapped from the source string.</returns>
    protected override ProcessDescriptor MapInternal(string source)
    {
        return new ProcessDescriptor
        {
            Path = source,
            Name = ReadNameFromPath(source)
        };
    }
}
