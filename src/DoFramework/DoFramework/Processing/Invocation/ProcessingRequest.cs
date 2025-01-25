namespace DoFramework.Processing;

/// <summary>
/// Represents a processing request with specified processes and arguments.
/// </summary>
public class ProcessingRequest : IProcessingRequest
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ProcessingRequest"/> class with specified processes and arguments.
    /// </summary>
    /// <param name="processes">The array of processes to be included in the request.</param>
    /// <param name="args">The dictionary of arguments for the processing request.</param>
    public ProcessingRequest(string[] processes, Dictionary<string, object> args)
    {
        Args = args;
        Processes = processes;
    }

    /// <summary>
    /// Gets or sets the arguments for the processing request.
    /// </summary>
    /// <value>
    /// A dictionary containing the arguments for the request.
    /// </value>
    public Dictionary<string, object> Args { get; set; }

    /// <summary>
    /// Gets or sets the processes associated with the request.
    /// </summary>
    /// <value>
    /// An array of processes related to the request.
    /// </value>
    public string[] Processes { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="ProcessingRequest"/> class with specified processes and an empty arguments dictionary.
    /// </summary>
    /// <param name="processes">The array of processes to be included in the request.</param>
    public ProcessingRequest(string[] processes) : this(processes, []) { }
}
