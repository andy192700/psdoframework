using DoFramework.Services;

namespace DoFramework.Processing;

/// <summary>
/// Represents a service registration repeater that registers types in the service container.
/// </summary>
public class ServiceRegistrationRepeater : Repeater<Type>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ServiceRegistrationRepeater"/> class with the specified service container.
    /// </summary>
    /// <param name="container">The service container used for registering and retrieving services.</param>
    public ServiceRegistrationRepeater(IServiceContainer container) : base(container) { }

    /// <inheritdoc/>
    public override IRepeater<Type> And(Type input)
    {
        _container.RegisterService(input);

        return this;
    }
}
