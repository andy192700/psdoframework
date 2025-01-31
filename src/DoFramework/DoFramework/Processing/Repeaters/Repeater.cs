using DoFramework.Services;

namespace DoFramework.Processing;

public abstract class Repeater<TInput> : IRepeater<TInput>
{
    protected readonly IServiceContainer _container;

    public Repeater(IServiceContainer container)
    {
        _container = container;
    }

    public abstract IRepeater<TInput> And(TInput input);
}
