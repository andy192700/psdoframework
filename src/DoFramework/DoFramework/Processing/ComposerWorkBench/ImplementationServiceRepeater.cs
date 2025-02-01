using DoFramework.Services;

namespace DoFramework.Processing;

public class ImplementationServiceRepeater : MultiTypeRepeater<Type, Type>
{
    public ImplementationServiceRepeater(IServiceContainer container) : base(container) { }

    public override IRepeater<Type, Type> And(Type serviceType, Type implementationType)
    {
        _container.RegisterService(serviceType, implementationType);

        return this;
    }
}
