using DoFramework.Services;

namespace DoFramework.Processing;

/// <summary>
/// Represents a composer workbench that provides methods to register and configure services and processes.
/// </summary>
public class ComposerWorkBench : IComposerWorkBench
{
    private readonly IServiceContainer _container;

    private IRepeater<Type> _configRepeater => new ConfigurationRepeater(_container);

    private IRepeater<string> _processRepeater => new ProcessRegistrationRepeater(_container);

    private IRepeater<Type> _serviceRepeater => new ServiceRegistrationRepeater(_container);

    private IRepeater<Type, Type> _implServiceRepeater => new ImplementationServiceRepeater(_container);

    /// <summary>
    /// Initializes a new instance of the <see cref="ComposerWorkBench"/> class.
    /// </summary>
    /// <param name="container">The service container.</param>
    public ComposerWorkBench(IServiceContainer container)
    {
        _container = container;
    }

    /// <inheritdoc/>
    public IRepeater<Type> Configure(Type configType)
    {
        return _configRepeater.And(configType);
    }

    /// <inheritdoc/>
    public IRepeater<string> RegisterProcess(string processName)
    {
        return _processRepeater.And(processName);
    }

    /// <inheritdoc/>
    public IRepeater<Type> RegisterService(Type serviceType)
    {
        return _serviceRepeater.And(serviceType);
    }

    /// <inheritdoc/>
    public IRepeater<Type, Type> RegisterService(Type serviceType, Type implementationType)
    {
        return _implServiceRepeater.And(serviceType, implementationType);
    }

    /// <inheritdoc/>
    public object GetService(Type serviceType)
    {
        return _container.GetService(serviceType);
    }
}

