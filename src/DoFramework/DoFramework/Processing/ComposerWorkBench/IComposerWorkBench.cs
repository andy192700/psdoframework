namespace DoFramework.Processing;

/// <summary>
/// Represents an interface for a composer workbench.
/// </summary>
public interface IComposerWorkBench
{
    /// <summary>
    /// Registers a service with the specified service type.
    /// </summary>
    /// <param name="serviceType">The type of the service to register.</param>
    /// <returns>A repeater that can be used to further configure the registered service.</returns>
    IRepeater<Type> RegisterService(Type serviceType);

    /// <summary>
    /// Registers a service with the specified service type and implementation type.
    /// </summary>
    /// <param name="serviceType">The type of the service to register.</param>
    /// <param name="implementationType">The type of the implementation for the service.</param>
    /// <returns>A repeater that can be used to further configure the registered service.</returns>
    IRepeater<Type, Type> RegisterService(Type serviceType, Type implementationType);

    /// <summary>
    /// Configures the workbench with the specified configuration type.
    /// </summary>
    /// <param name="configType">The type of the configuration.</param>
    /// <returns>A repeater that can be used to further configure the workbench.</returns>
    IRepeater<Type> Configure(Type configType);

    /// <summary>
    /// Registers a process with the specified process name.
    /// </summary>
    /// <param name="processName">The name of the process to register.</param>
    /// <returns>A repeater that can be used to further configure the registered process.</returns>
    IRepeater<string> RegisterProcess(string processName);

    /// <summary>
    /// Gets a service of the specified service type from the workbench.
    /// </summary>
    /// <param name="serviceType">The type of the service to get.</param>
    /// <returns>The service instance.</returns>
    object GetService(Type serviceType);
}
