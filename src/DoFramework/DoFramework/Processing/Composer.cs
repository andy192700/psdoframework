namespace DoFramework.Processing;

public abstract class Composer<TProcess> : IComposer where TProcess : IProcess
{
    public string ProcessName { get; } = nameof(TProcess);

    public abstract void Compose(IComposerWorkBench workBench);
}