using DoFramework.Services;

namespace DoFramework.Processing;

public interface IComposerWorkBench
{
    IRepeater<Type> RegisterService(Type serviceType);

    IRepeater<Type> Configure(Type  configType);

    IRepeater<string> RegisterProcess(string processName);

    object GetService(Type serviceType);
}