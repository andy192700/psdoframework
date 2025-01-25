namespace DoFramework.Domain;

/// <summary>
/// Represents a report for a process, including its descriptor, result, timing information, and duration.
/// </summary>
public class ProcessReport
{
    /// <summary>
    /// Gets or sets the descriptor associated with the process.
    /// </summary>
    public ProcessDescriptor? Descriptor { get; set; }

    /// <summary>
    /// Gets or sets the result of the process. Default value is <see cref="ProcessResult.NotRun"/>.
    /// </summary>
    public ProcessResult ProcessResult { get; set; } = ProcessResult.NotRun;

    /// <summary>
    /// Gets or sets the start time of the process.
    /// </summary>
    public DateTime? StartTime { get; set; }

    /// <summary>
    /// Gets or sets the end time of the process.
    /// </summary>
    public DateTime? EndTime { get; set; }

    /// <summary>
    /// Gets or sets the name of the process.
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// Gets the duration of the process in seconds, calculated as the difference between <see cref="EndTime"/> and <see cref="StartTime"/>.
    /// </summary>
    public int? Duration
    {
        get
        {
            if (StartTime.HasValue && EndTime.HasValue)
            {
                TimeSpan timeDifference = EndTime.Value - StartTime.Value;
                return (int)timeDifference.TotalSeconds;
            }
            return null;
        }
    }
}
