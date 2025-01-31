using DoFramework.Services;

namespace DoFramework.Processing;

public class ServiceRegistrationRepeater : Repeater<Type>
{
    public ServiceRegistrationRepeater(IServiceContainer container) : base(container) { }

    public override IRepeater<Type> And(Type input)
    {
        _container.RegisterService(input);

        return this;
    }
}