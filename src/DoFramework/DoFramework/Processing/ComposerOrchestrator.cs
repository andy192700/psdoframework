using DoFramework.Data;
using DoFramework.Domain;
using DoFramework.Environment;
using DoFramework.FileSystem;
using DoFramework.Logging;
using DoFramework.Services;
using DoFramework.Types;
using DoFramework.Validators;

namespace DoFramework.Processing;

public class ComposerOrchestrator : IComposerOrchestrator
{
    private readonly IEnvironment _environment;
    private readonly ISetProcessLocation _setProcessLocation;
    private readonly ILogger _logger;
    private readonly IComposerBuilder _composerBuilder;
    private readonly IResolver<ComposerDescriptor> _composerResolver;
    
    public ComposerOrchestrator(
        IEnvironment environment,
        ISetProcessLocation setProcessLocation,
        ILogger logger,
        IComposerBuilder composerBuilder,
        IResolver<ComposerDescriptor> composerResolver)
    {
        _environment = environment;
        _setProcessLocation = setProcessLocation;
        _logger = logger;
        _composerBuilder = composerBuilder;
        _composerResolver = composerResolver;
    }

    public bool Orchestrate(string composerName, IServiceContainer serviceContainer)
    {
        var success = false;

        try
        {
            var descriptor = _composerResolver.Resolve(composerName);

            if (!descriptor.Exists)
            {
                _logger.LogFatal($"Could not find composer {composerName}");

                return success;
            }

            var composer = _composerBuilder.Build(descriptor.Descriptor!);

            composer.Compose(serviceContainer);

            success = true;
        }
        catch (Exception ex)
        {
            _logger.LogFatal($"Error building composer: {composerName}");
            _logger.LogFatal($"{ex.Message}");

            success = false;
        }

        _setProcessLocation.Set(_environment.HomeDir);

        return success;
    }
}
