using DoFramework.Domain;
using DoFramework.FileSystem;

namespace DoFramework.Mappers;

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
