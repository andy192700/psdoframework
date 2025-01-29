namespace DoFramework.Services;

/// <summary>
/// Interface for a service container that allows for registering and retrieving services.
/// </summary>
public interface IServiceContainer
{
    /// <summary>
    /// Gets the dictionary of registered service types.
    /// </summary>
    Dictionary<Type, Type> Services { get; }

    /// <summary>
    /// Gets the dictionary of service instances.
    /// </summary>
    Dictionary<Type, object> Instances { get; }

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
    /// Retrieves all services of the specified base type.
    /// </summary>
    /// <typeparam name="TBaseType">The base type of the services to retrieve.</typeparam>
    /// <returns>A list of services of the specified base type.</returns>
    List<TBaseType> GetServicesByType<TBaseType>() where TBaseType : class;

    /// <summary>
    /// Configures an Object, registering it to this container.
    /// </summary>
    /// <typeparam name="TObject">The type to be registered and populated.</typeparam>
    void Configure(Type type);
}
