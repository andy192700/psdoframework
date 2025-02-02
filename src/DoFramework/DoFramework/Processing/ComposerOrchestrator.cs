using DoFramework.Data;
using DoFramework.Domain;
using DoFramework.Environment;
using DoFramework.FileSystem;
using DoFramework.Logging;
using DoFramework.Services;

namespace DoFramework.Processing;

/// <summary>
/// Represents a composer orchestrator that handles the orchestration of composers.
/// </summary>
public class ComposerOrchestrator : IComposerOrchestrator
{
    private readonly IEnvironment _environment;
    private readonly ISetProcessLocation _setProcessLocation;
    private readonly ILogger _logger;
    private readonly IComposerBuilder _composerBuilder;
    private readonly IResolver<ComposerDescriptor> _composerResolver;
    private readonly IContext _context;

    /// <summary>
    /// Initializes a new instance of the <see cref="ComposerOrchestrator"/> class.
    /// </summary>
    /// <param name="environment">The environment.</param>
    /// <param name="setProcessLocation">The process location setter.</param>
    /// <param name="logger">The logger.</param>
    /// <param name="composerBuilder">The composer builder.</param>
    /// <param name="composerResolver">The composer resolver.</param>
    /// <param name="session">The session.</param>
    public ComposerOrchestrator(
        IEnvironment environment,
        ISetProcessLocation setProcessLocation,
        ILogger logger,
        IComposerBuilder composerBuilder,
        IResolver<ComposerDescriptor> composerResolver,
        IContext context)
    {
        _environment = environment;
        _setProcessLocation = setProcessLocation;
        _logger = logger;
        _composerBuilder = composerBuilder;
        _composerResolver = composerResolver;
        _context = context;
    }

    /// <inheritdoc/>
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

            composer.Compose(new ComposerWorkBench(serviceContainer));

            _context.SetComposedBy(composerName);

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
