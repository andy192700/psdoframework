namespace DoFramework.Processing;

/// <summary>
/// Defines an interface for a processing request.
/// </summary>
public interface IProcessingRequest
{
    /// <summary>
    /// Gets or sets the arguments for the processing request.
    /// </summary>
    /// <value>
    /// A dictionary containing the arguments for the request.
    /// </value>
    Dictionary<string, object> Args { get; set; }

    /// <summary>
    /// Gets or sets the processes associated with the request.
    /// </summary>
    /// <value>
    /// An array of processes related to the request.
    /// </value>
    string[] Processes { get; set; }
}
