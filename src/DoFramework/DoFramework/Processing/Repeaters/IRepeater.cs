namespace DoFramework.Processing;

public interface IRepeater<TInput>
{
    IRepeater<TInput> And(TInput input);
}
