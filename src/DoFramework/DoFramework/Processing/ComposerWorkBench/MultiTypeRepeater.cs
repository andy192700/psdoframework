using DoFramework.Services;

namespace DoFramework.Processing;

/// <summary>
/// Represents an abstract repeater that works with two input types.
/// </summary>
/// <typeparam name="TInput1">The type of the first input.</typeparam>
/// <typeparam name="TInput2">The type of the second input.</typeparam>
public abstract class MultiTypeRepeater<TInput1, TInput2> : IRepeater<TInput1, TInput2>
{
    /// <summary>
    /// The service container used for registering and retrieving services.
    /// </summary>
    protected readonly IServiceContainer _container;

    /// <summary>
    /// Initializes a new instance of the <see cref="MultiTypeRepeater{TInput1, TInput2}"/> class with the specified service container.
    /// </summary>
    /// <param name="container">The service container used for registering and retrieving services.</param>
    public MultiTypeRepeater(IServiceContainer container)
    {
        _container = container;
    }

    /// <inheritdoc/>
    public IRepeater<(TInput1, TInput2)> And((TInput1, TInput2) input)
    {
        return And(input.Item1, input.Item2);
    }

    /// <inheritdoc/>
    public abstract IRepeater<TInput1, TInput2> And(TInput1 input1, TInput2 input2);
}
