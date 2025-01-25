using DoFramework.Domain;

namespace DoFramework.Processing;

/// <summary>
/// Defines an interface for building a process based on a process descriptor.
/// </summary>
public interface IProcessBuilder
{
    /// <summary>
    /// Builds a process based on the given descriptor.
    /// </summary>
    /// <param name="descriptor">The descriptor that specifies the process to build.</param>
    /// <returns>An instance of the process built from the descriptor.</returns>
    IProcess Build(ProcessDescriptor descriptor);
}
