using DoFramework.Services;

namespace DoFramework.Processing;

/// <summary>
/// Represents an interface for orchestrating composers.
/// </summary>
public interface IComposerOrchestrator
{
    /// <summary>
    /// Orchestrates the specified composer with the given service container.
    /// </summary>
    /// <param name="composerName">The name of the composer to orchestrate.</param>
    /// <param name="serviceContainer">The service container to use for orchestration.</param>
    /// <returns><c>true</c> if the orchestration was successful; otherwise, <c>false</c>.</returns>
    bool Orchestrate(string composerName, IServiceContainer serviceContainer);
}
