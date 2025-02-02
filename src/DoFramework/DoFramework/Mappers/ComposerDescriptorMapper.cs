using DoFramework.Domain;
using DoFramework.FileSystem;

namespace DoFramework.Mappers;

/// <summary>
/// Class for mapping a string to a composer descriptor.
/// </summary>
public class ComposerDescriptorMapper : DescriptorMapper<ComposerDescriptor>
{
    public ComposerDescriptorMapper(IOSSanitise osSanitise) : base(osSanitise) { }

    /// <inheritdoc/>
    protected override ComposerDescriptor MapInternal(string source)
    {
        return new ComposerDescriptor
        {
            Path = source,
            Name = ReadNameFromPath(source)
        };
    }
}
