using DoFramework.Domain;

namespace DoFramework.Data;

/// <summary>
/// Defines a method to resolve a descriptor based on a specified module name.
/// </summary>
/// <typeparam name="TDescriptor">The type of descriptor to resolve, which must implement the IDescriptor interface.</typeparam>
public interface IResolver<TDescriptor> where TDescriptor : IDescriptor
{
    /// <summary>
    /// Resolves a descriptor based on the specified module name.
    /// </summary>
    /// <param name="module">The name of the module to resolve.</param>
    /// <returns>A resolution result containing the resolved descriptor.</returns>
    ResolutionResult<TDescriptor> Resolve(string module);
}
