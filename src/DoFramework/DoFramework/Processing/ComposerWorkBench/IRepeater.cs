namespace DoFramework.Processing;

/// <summary>
/// Represents a repeater interface for a single input type.
/// </summary>
/// <typeparam name="TInput">The type of the input.</typeparam>
public interface IRepeater<TInput>
{
    /// <summary>
    /// Adds the specified input to the repeater.
    /// </summary>
    /// <param name="input">The input to add.</param>
    /// <returns>The repeater with the added input.</returns>
    IRepeater<TInput> And(TInput input);
}

/// <summary>
/// Represents a repeater interface for two input types.
/// </summary>
/// <typeparam name="TInput1">The type of the first input.</typeparam>
/// <typeparam name="TInput2">The type of the second input.</typeparam>
public interface IRepeater<TInput1, TInput2> : IRepeater<(TInput1, TInput2)>
{
    /// <summary>
    /// Adds the specified inputs to the repeater.
    /// </summary>
    /// <param name="input1">The first input to add.</param>
    /// <param name="input2">The second input to add.</param>
    /// <returns>The repeater with the added inputs.</returns>
    IRepeater<TInput1, TInput2> And(TInput1 input1, TInput2 input2);
}
