using DoFramework.Domain;
using DoFramework.FileSystem;

namespace DoFramework.Mappers;

/// <summary>
/// Class for mapping a string to a module descriptor.
/// </summary>
public class ModuleDescriptorMapper : DescriptorMapper<ModuleDescriptor>
{
    public ModuleDescriptorMapper(IOSSanitise osSanitise) : base(osSanitise) { }

    /// <summary>
    /// Maps the source string to a module descriptor.
    /// </summary>
    /// <param name="source">The source string.</param>
    /// <returns>The module descriptor mapped from the source string.</returns>
    protected override ModuleDescriptor MapInternal(string source)
    {
        return new ModuleDescriptor
        {
            Path = source,
            Name = ReadNameFromPath(source)
        };
    }
}
