using DoFramework.CLI;
using DoFramework.FileSystem;
using DoFramework.Logging;
using DoFramework.Processing;
using DoFramework.Types;
using DoFramework.Validators;

namespace DoFramework.Services;

/// <summary>
/// Provides extension methods for the IServiceContainer interface.
/// </summary>
public static class ServiceContainerExtensions
{
    /// <summary>
    /// Checks and validates the environment.
    /// </summary>
    /// <param name="container">The service container.</param>
    /// <returns>The service container with the validated environment.</returns>
    public static IServiceContainer CheckEnvironment(this IServiceContainer container)
    {
        var fileManager = container.GetService<IFileManager>();

        var processLocationReader = container.GetService<IReadProcessLocation>();

        if (!fileManager.FileExists($"{processLocationReader.Read()}{Environment.Environment.Separator}do.json"))
        {
            throw new Exception("Could not find do.json.");
        }

        return container;
    }

    /// <summary>
    /// Adds application parameters to the service container, writing them to the <see cref="IContext"/>.
    /// </summary>
    /// <param name="container">The service container.</param>
    /// <param name="applicationParams">The application parameters to add.</param>
    /// <returns>The service container with the added application parameters.</returns>
    public static IServiceContainer AddParameters(this IServiceContainer container, Dictionary<string, object> applicationParams)
    {
        container.RegisterService<CLIFunctionParameters>();

        container.GetService<CLIFunctionParameters>().Parameters = applicationParams;

        container.GetService<ILogger>().Parameters = container.GetService<CLIFunctionParameters>();

        container.GetService<IContextWriter>().Write(applicationParams);

        return container;
    }

    /// <summary>
    /// Consumes environment files.
    /// </summary>
    /// <param name="container">The service container.</param>
    /// <returns>The service container after consuming environment files.</returns>
    public static IServiceContainer ConsumeEnvFiles(this IServiceContainer container)
    {
        var envFileConsumer = container.GetService<IConsumeEnvFiles>();

        envFileConsumer.Consume();

        return container;
    }

    /// <summary>
    /// Adds processing services to the specified service container.
    /// </summary>
    /// <param name="container">The service container to which the services will be added.</param>
    /// <param name="processBuilderType">The type of the process builder to be registered.</param>
    /// <returns>The updated service container.</returns>
    public static IServiceContainer AddProcessingServices(this IServiceContainer container, Type processBuilderType)
    {
        container.RegisterService<IProcessInstanceRunner, ProcessInstanceRunner>();
        container.RegisterService<IProcessExecutor, ProcessExecutor>();
        container.RegisterService<IProcessRunner, ProcessRunner>();
        container.RegisterService<IEntryPoint, EntryPoint>();
        container.RegisterService<IFailedReportChecker, FailedReportChecker>();
        container.RegisterService<ILookupType<IProcess>, LookupProcessType>();
        container.RegisterService<IValidator<IProcessingRequest>, ProcessingRequestValidator>();
        container.RegisterService<TypeValidator<IProcess>, ProcessTypeValidator>();
        container.RegisterService(typeof(IProcessBuilder), processBuilderType);

        return container;
    }

    /// <summary>
    /// Adds composer services to the specified service container.
    /// </summary>
    /// <param name="container">The service container to which the services will be added.</param>
    /// <param name="composerType">The type of the composer to be registered.</param>
    /// <returns>The updated service container.</returns>
    public static IServiceContainer AddComposerServices(this IServiceContainer container, Type composerType)
    {
        container.RegisterService<ILookupType<IComposer>, LookupComposerType>();
        container.RegisterService<TypeValidator<IComposer>, ComposerTypeValidator>();
        container.RegisterService<IProcessRegistry, ProcessRegistry>();
        container.RegisterService<IComposerOrchestrator, ComposerOrchestrator>();
        container.RegisterService(typeof(IComposerBuilder), composerType);

        return container;
    }

}
