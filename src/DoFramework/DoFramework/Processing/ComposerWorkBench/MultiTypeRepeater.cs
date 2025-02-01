using DoFramework.Services;

namespace DoFramework.Processing;

public abstract class MultiTypeRepeater<TInput1, TInput2> : IRepeater<TInput1, TInput2>
{
    protected readonly IServiceContainer _container;

    public MultiTypeRepeater(IServiceContainer container)
    {
        _container = container;
    }

    public IRepeater<(TInput1, TInput2)> And((TInput1, TInput2) input)
    {
        return And(input.Item1, input.Item2);
    }
    public abstract IRepeater<TInput1, TInput2> And(TInput1 input1, TInput2 input2);
}