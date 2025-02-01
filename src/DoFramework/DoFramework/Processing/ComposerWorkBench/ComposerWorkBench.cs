using DoFramework.Services;

namespace DoFramework.Processing;

public class ComposerWorkBench : IComposerWorkBench
{
    private readonly IServiceContainer _container;

    private IRepeater<Type> _configRepeater => new ConfigurationRepeater(_container);

    private IRepeater<string> _processRepeater => new ProcessRegistrationRepeater(_container);

    private IRepeater<Type> _serviceRepeater => new ServiceRegistrationRepeater(_container);

    private IRepeater<Type, Type> _implServiceRepeater => new ImplementationServiceRepeater(_container);

    public ComposerWorkBench(IServiceContainer container)
    {
        _container = container;
    }

    public IRepeater<Type> Configure(Type configType)
    {
        return _configRepeater.And(configType);
    }

    public IRepeater<string> RegisterProcess(string processName)
    {
        return _processRepeater.And(processName);
    }

    public IRepeater<Type> RegisterService(Type serviceType)
    {
        return _serviceRepeater.And(serviceType);
    }

    public IRepeater<Type, Type> RegisterService(Type serviceType, Type implementationType)
    {
        return _implServiceRepeater.And(serviceType, implementationType);
    }

    public object GetService(Type serviceType)
    {
        return _container.GetService(serviceType);
    }
}
