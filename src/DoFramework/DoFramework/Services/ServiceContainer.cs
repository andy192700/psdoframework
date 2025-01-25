namespace DoFramework.Services;

/// <summary>
/// Service container for registering and retrieving services and their instances.
/// </summary>
public class ServiceContainer : IServiceContainer
{
    /// <summary>
    /// Gets the dictionary of registered service types.
    /// </summary>
    public Dictionary<Type, Type> Services { get; } = [];

    /// <summary>
    /// Gets the dictionary of service instances.
    /// </summary>
    public Dictionary<Type, object> Instances { get; } = [];

    private readonly IObjectBuilder _builder;

    /// <summary>
    /// Initializes a new instance of the <see cref="ServiceContainer"/> class using the default object builder.
    /// </summary>
    public ServiceContainer() : this(new ObjectBuilder()) { }

    /// <summary>
    /// Initializes a new instance of the <see cref="ServiceContainer"/> class using the specified object builder.
    /// </summary>
    /// <param name="objectBuilder">The object builder to use.</param>
    public ServiceContainer(IObjectBuilder objectBuilder)
    {
        _builder = objectBuilder;

        RegisterService<IServiceContainer, ServiceContainer>();

        Instances[typeof(IServiceContainer)] = this;
    }

    /// <summary>
    /// Registers a service of the specified type.
    /// </summary>
    /// <typeparam name="TService">The type of the service to register.</typeparam>
    public void RegisterService<TService>()
        where TService : class
    {
        RegisterService(typeof(TService));
    }

    /// <summary>
    /// Registers a service with the specified abstraction and implementation types.
    /// </summary>
    /// <typeparam name="TAbstraction">The abstraction type of the service.</typeparam>
    /// <typeparam name="TImplementation">The implementation type of the service.</typeparam>
    public void RegisterService<TAbstraction, TImplementation>()
        where TAbstraction : class
        where TImplementation : class, TAbstraction
    {
        RegisterService(typeof(TAbstraction), typeof(TImplementation));
    }

    /// <summary>
    /// Registers a service with the specified type.
    /// </summary>
    /// <param name="type">The type of the service to register.</param>
    public void RegisterService(Type type)
    {
        Register(type);
    }

    /// <summary>
    /// Registers a service with the specified abstraction and implementation types.
    /// </summary>
    /// <param name="typeAbstraction">The abstraction type of the service.</param>
    /// <param name="typeImplementation">The implementation type of the service.</param>
    public void RegisterService(Type typeAbstraction, Type typeImplementation)
    {
        if (typeAbstraction.Equals(typeImplementation))
        {
            throw new Exception($"Implementation and abstraction types must not be the same, failed to register {typeAbstraction.FullName}");
        }

        if (!typeAbstraction.IsAssignableFrom(typeImplementation))
        {
            throw new Exception($"Service of Type {typeImplementation.FullName} does derive from {typeAbstraction.FullName}");
        }

        Register(typeAbstraction, typeImplementation);
    }

    /// <summary>
    /// Retrieves all services of the specified base type.
    /// </summary>
    /// <typeparam name="TBaseType">The base type of the services to retrieve.</typeparam>
    /// <returns>A list of services of the specified base type.</returns>
    public List<TBaseType> GetServicesByType<TBaseType>()
        where TBaseType : class
    {
        var services = new List<TBaseType>();

        foreach (var key in Services.Keys)
        {
            if (typeof(TBaseType).IsAssignableFrom(key))
            {
                services.Add((TBaseType)GetService(key));
            }
        }

        return services;
    }

    /// <summary>
    /// Retrieves a service of the specified type.
    /// </summary>
    /// <typeparam name="TService">The type of the service to retrieve.</typeparam>
    /// <returns>An instance of the specified service type.</returns>
    public TService GetService<TService>()
    {
        if (!Instances.ContainsKey(typeof(TService)))
        {
            Instances[typeof(TService)] = InitialiseService(typeof(TService));
        }

        return (TService)(Instances[typeof(TService)]);
    }

    /// <summary>
    /// Retrieves a service of the specified type.
    /// </summary>
    /// <param name="type">The type of the service to retrieve.</param>
    /// <returns>An instance of the specified service type.</returns>
    public object GetService(Type type)
    {
        if (!Instances.ContainsKey(type))
        {
            Instances[type] = InitialiseService(type);
        }

        return Instances[type];
    }

    private Type ResolveService(Type type)
    {
        foreach (var key in Services.Keys)
        {
            if (key.Equals(type))
            {
                return Services[key];
            }
        }

        throw new Exception($"Service of Type '{type}' could not be resolved.");
    }

    private void Register(Type typeImplementation)
    {
        if (Services.ContainsKey(typeImplementation))
        {
            throw new Exception($"Service of Type '{typeImplementation}' already exists in the container.");
        }
        else
        {
            Services[typeImplementation] = typeImplementation;
        }
    }

    private void Register(Type typeAbstraction, Type typeImplementation)
    {
        if (Services.ContainsKey(typeAbstraction))
        {
            throw new Exception($"Service of Type '{typeAbstraction}' already exists in the container.");
        }
        else
        {
            Services[typeAbstraction] = typeImplementation;
        }
    }

    private object InitialiseService(Type type)
    {
        var resolvedType = ResolveService(type);

        var constructors = resolvedType.GetConstructors();

        if (constructors.Length == 0)
        {
            throw new Exception($"Service of type {type} could not be initalised, could not find any constructors.");
        }

        if (constructors.Length > 1)
        {
            throw new Exception($"Service of type {type} could not be initalised, only one constructor is allowed.");
        }

        var constructorParams = new List<object>();

        foreach (var parameter in constructors[0].GetParameters())
        {
            constructorParams.Add(GetService(parameter.ParameterType));
        }

        return _builder.BuildObject(resolvedType, constructorParams);
    }
}
