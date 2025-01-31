using DoFramework.Processing;
using DoFramework.Services;

namespace DoFramework.Processing;

public class ProcessRegistrationRepeater : Repeater<string>
{
    public ProcessRegistrationRepeater(IServiceContainer container) : base(container) { }

    public override IRepeater<string> And(string input)
    {
        var registry = _container.GetService<IProcessRegistry>();

        registry.RegisterProcess(input);

        return this;
    }
}
