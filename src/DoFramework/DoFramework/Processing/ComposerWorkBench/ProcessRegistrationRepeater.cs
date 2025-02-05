using DoFramework.Services;

namespace DoFramework.Processing;

/// <summary>
/// Represents a repeater that registers processes using a process registry.
/// </summary>
public class ProcessRegistrationRepeater : Repeater<string>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ProcessRegistrationRepeater"/> class with the specified service container.
    /// </summary>
    /// <param name="container">The service container used for registering and retrieving services.</param>
    public ProcessRegistrationRepeater(IServiceContainer container) : base(container) { }

    /// <inheritdoc/>
    public override IRepeater<string> And(string input)
    {
        var registry = _container.GetService<IProcessRegistry>();

        registry.RegisterProcess(input);

        return this;
    }
}

