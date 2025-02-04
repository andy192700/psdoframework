using DoFramework.Domain;

namespace DoFramework.Processing;

/// <summary>
/// Represents an interface for building composers.
/// </summary>
public interface IComposerBuilder
{
    /// <summary>
    /// Builds a composer based on the provided composer descriptor.
    /// </summary>
    /// <param name="composerDescriptor">The descriptor of the composer to build.</param>
    /// <returns>The built composer.</returns>
    IComposer Build(ComposerDescriptor composerDescriptor);
}
