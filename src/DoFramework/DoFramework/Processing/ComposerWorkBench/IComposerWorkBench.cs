namespace DoFramework.Processing;

public interface IComposerWorkBench
{
    IRepeater<Type> RegisterService(Type serviceType);

    IRepeater<Type, Type> RegisterService(Type serviceType, Type implementationType);

    IRepeater<Type> Configure(Type  configType);

    IRepeater<string> RegisterProcess(string processName);

    object GetService(Type serviceType);
}