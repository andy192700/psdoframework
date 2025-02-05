using DoFramework.Services;

namespace DoFramework.Processing;

/// <summary>
/// Represents a service repeater that registers implementation types for specified service types.
/// </summary>
public class ImplementationServiceRepeater : MultiTypeRepeater<Type, Type>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ImplementationServiceRepeater"/> class with the specified service container.
    /// </summary>
    /// <param name="container">The service container used for registering services and their implementations.</param>
    public ImplementationServiceRepeater(IServiceContainer container) : base(container) { }

    /// <inheritdoc/>
    public override IRepeater<Type, Type> And(Type serviceType, Type implementationType)
    {
        _container.RegisterService(serviceType, implementationType);

        return this;
    }
}
