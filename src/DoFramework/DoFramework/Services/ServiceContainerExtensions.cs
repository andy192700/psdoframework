using DoFramework.CLI;
using DoFramework.FileSystem;
using DoFramework.Logging;
using DoFramework.Processing;

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
    /// Adds application parameters to the service container.
    /// </summary>
    /// <param name="container">The service container.</param>
    /// <param name="applicationParams">The application parameters to add.</param>
    /// <returns>The service container with the added application parameters.</returns>
    public static IServiceContainer AddParameters(this IServiceContainer container, Dictionary<string, object> applicationParams)
    {
        container.RegisterService<CLIFunctionParameters>();

        container.GetService<CLIFunctionParameters>().Parameters = applicationParams;

        container.GetService<ILogger>().Parameters = container.GetService<CLIFunctionParameters>();

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
}
