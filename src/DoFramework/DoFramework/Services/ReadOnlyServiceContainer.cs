namespace DoFramework.Services;

/// <summary>
/// Concrete implementation of a <see cref="IReadOnlyServiceContainer"/> which does not allow service registration.
/// </summary>
public class ReadOnlyServiceContainer : IReadOnlyServiceContainer
{
    private readonly IServiceContainer _serviceContainer;
    
    public ReadOnlyServiceContainer(IServiceContainer serviceContainer)
    {
        _serviceContainer = serviceContainer;
    }

    /// <inheritdoc />
    public TService GetService<TService>()
    {
        return _serviceContainer.GetService<TService>();
    }

    /// <inheritdoc />
    public object GetService(Type type)
    {
        return _serviceContainer.GetService(type);
    }
}
