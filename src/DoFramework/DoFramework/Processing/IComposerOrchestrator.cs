using DoFramework.Services;

namespace DoFramework.Processing;

public interface IComposerOrchestrator
{
    bool Orchestrate(string composerName, IServiceContainer serviceContainer);
}
