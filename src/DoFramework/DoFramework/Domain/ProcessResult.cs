namespace DoFramework.Domain;

/// <summary>
/// Specifies the various possible outcomes of a process.
/// </summary>
public enum ProcessResult
{
    /// <summary>
    /// Indicates that the process completed successfully.
    /// </summary>
    Completed,

    /// <summary>
    /// Indicates that the process failed.
    /// </summary>
    Failed,

    /// <summary>
    /// Indicates that the process was invalidated.
    /// </summary>
    Invalidated,

    /// <summary>
    /// Indicates that the process was not run.
    /// </summary>
    NotRun,

    /// <summary>
    /// Indicates that the process was not found.
    /// </summary>
    NotFound
}
