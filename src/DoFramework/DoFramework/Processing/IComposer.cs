namespace DoFramework.Processing;

/// <summary>
/// Represents a composer interface for composing workbenches.
/// </summary>
public interface IComposer
{
    /// <summary>
    /// Composes the specified workbench.
    /// </summary>
    /// <param name="workBench">The workbench to compose.</param>
    void Compose(IComposerWorkBench workBench);
}
