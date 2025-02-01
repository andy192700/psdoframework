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
}
