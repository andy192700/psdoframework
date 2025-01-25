using DoFramework.Domain;

namespace DoFramework.Data;

/// <summary>
/// Represents the result of a resolution operation for a descriptor.
/// </summary>
/// <typeparam name="TDescriptor">The type of descriptor being resolved, which must implement the IDescriptor interface.</typeparam>
/// <param name="exists">A value indicating whether the descriptor exists.</param>
/// <param name="path">The path to the descriptor, if it exists.</param>
/// <param name="descriptor">The resolved descriptor.</param>
public class ResolutionResult<TDescriptor>(bool exists, string? path, TDescriptor descriptor) where TDescriptor : IDescriptor
{
    /// <summary>
    /// Gets or sets the resolved descriptor.
    /// </summary>
    public TDescriptor? Descriptor { get; set; } = descriptor;

    /// <summary>
    /// Gets or sets a value indicating whether the descriptor exists.
    /// </summary>
    public bool Exists { get; set; } = exists;

    /// <summary>
    /// Gets or sets the path to the descriptor, if it exists.
    /// </summary>
    public string? Path { get; set; } = path;
}
