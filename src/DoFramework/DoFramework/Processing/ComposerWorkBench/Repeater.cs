using DoFramework.Services;

namespace DoFramework.Processing;

/// <summary>
/// Represents an abstract repeater that works with a single input type.
/// </summary>
/// <typeparam name="TInput">The type of the input.</typeparam>
public abstract class Repeater<TInput> : IRepeater<TInput>
{
    /// <summary>
    /// The service container used for registering and retrieving services.
    /// </summary>
    protected readonly IServiceContainer _container;

    /// <summary>
    /// Initializes a new instance of the <see cref="Repeater{TInput}"/> class with the specified service container.
    /// </summary>
    /// <param name="container">The service container used for registering and retrieving services.</param>
    public Repeater(IServiceContainer container)
    {
        _container = container;
    }

    /// <inheritdoc/>
    public abstract IRepeater<TInput> And(TInput input);
}
