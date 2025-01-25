using DoFramework.Domain;
using DoFramework.FileSystem;

namespace DoFramework.Mappers;

/// <summary>
/// Class for mapping a string to a test descriptor.
/// </summary>
public class TestDescriptorMapper : DescriptorMapper<TestDescriptor>
{
    public TestDescriptorMapper(IOSSanitise osSanitise) : base(osSanitise) { }

    /// <summary>
    /// Maps the source string to a test descriptor.
    /// </summary>
    /// <param name="source">The source string.</param>
    /// <returns>The test descriptor mapped from the source string.</returns>
    protected override TestDescriptor MapInternal(string source)
    {
        return new TestDescriptor
        {
            Path = source,
            Name = ReadNameFromPath(source)
        };
    }
}
