namespace DoFramework.Services;

/// <summary>
/// Read only contract for an <see cref="IServiceContainer"/>
/// </summary>
public interface IReadOnlyServiceContainer
{
    /// <summary>
    /// Retrieves a service of the specified type.
    /// </summary>
    /// <typeparam name="TService">The type of the service to retrieve.</typeparam>
    /// <returns>An instance of the specified service type.</returns>
    TService GetService<TService>();

    /// <summary>
    /// Retrieves a service of the specified type.
    /// </summary>
    /// <param name="type">The type of the service to retrieve.</param>
    /// <returns>An instance of the specified service type.</returns>
    object GetService(Type type);

    /// <summary>
    /// Checks to see if a service of the has been registered.
    /// </summary>
    /// <typeparam name="TService">The type of the service to check.</typeparam>
    /// <returns>A boolean indicating that the Service Container has the service registered.</returns>
    bool HasService<TService>();

    /// <summary>
    /// Checks to see if a service of the has been registered.
    /// </summary>
    /// <param name="type">The type of the service to check.</param>
    /// <returns>A boolean indicating that the Service Container has the service registered.</returns>
    bool HasService(Type type);
}
