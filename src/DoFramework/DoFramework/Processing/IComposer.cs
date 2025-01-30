using DoFramework.Services;

namespace DoFramework.Processing;

public interface IComposer
{
    void Compose(IServiceContainer container);
}