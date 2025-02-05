namespace DoFramework.Services;

/// <summary>
/// Interface for a service container that allows for registering and retrieving services.
/// </summary>
public interface IServiceContainer : IReadOnlyServiceContainer
{
    /// <summary>
    /// Registers a service with the specified type.
    /// </summary>
    /// <param name="type">The type of the service to register.</param>
    void RegisterService(Type type);

    /// <summary>
    /// Registers a service with the specified abstraction and implementation types.
    /// </summary>
    /// <param name="typeAbstraction">The abstraction type of the service.</param>
    /// <param name="typeImplementation">The implementation type of the service.</param>
    void RegisterService(Type typeAbstraction, Type typeImplementation);

    /// <summary>
    /// Registers a service of the specified type.
    /// </summary>
    /// <typeparam name="TService">The type of the service to register.</typeparam>
    void RegisterService<TService>() where TService : class;

    /// <summary>
    /// Registers a service with the specified abstraction and implementation types.
    /// </summary>
    /// <typeparam name="TAbstraction">The abstraction type of the service.</typeparam>
    /// <typeparam name="TImplementation">The implementation type of the service.</typeparam>
    void RegisterService<TAbstraction, TImplementation>()
        where TAbstraction : class
        where TImplementation : class, TAbstraction;

    /// <summary>
    /// Retrieves all services of the specified base type.
    /// </summary>
    /// <typeparam name="TBaseType">The base type of the services to retrieve.</typeparam>
    /// <returns>A list of services of the specified base type.</returns>
    List<TBaseType> GetServicesByType<TBaseType>() where TBaseType : class;
}
