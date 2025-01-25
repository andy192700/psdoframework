namespace DoFramework.Processing;

/// <summary>
/// Defines an interface for a process dispatcher.
/// </summary>
public interface IProcessDispatcher
{
    /// <summary>
    /// Dispatches the specified processing request.
    /// </summary>
    /// <param name="processingRequest">The request to be processed.</param>
    void Dispatch(IProcessingRequest processingRequest);
}
