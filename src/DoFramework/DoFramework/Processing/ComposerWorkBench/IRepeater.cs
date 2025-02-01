namespace DoFramework.Processing;

public interface IRepeater<TInput>
{
    IRepeater<TInput> And(TInput input);
}

public interface IRepeater<TInput1, TInput2> : IRepeater<(TInput1, TInput2)>
{
    IRepeater<TInput1, TInput2> And(TInput1 input1, TInput2 input2);
}