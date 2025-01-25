namespace DoFramework.Logging;

/// <summary>
/// Represents the severity level of a log message.
/// </summary>
public enum LogLevel
{
    /// <summary>
    /// Trace level for detailed debugging messages.
    /// </summary>
    Trace,

    /// <summary>
    /// Debug level for diagnostic messages.
    /// </summary>
    Debug,

    /// <summary>
    /// Info level for informational messages.
    /// </summary>
    Info,

    /// <summary>
    /// Warning level for potentially harmful situations.
    /// </summary>
    Warning,

    /// <summary>
    /// Error level for error events that might still allow the application to continue running.
    /// </summary>
    Error,

    /// <summary>
    /// Fatal level for very severe error events that will presumably lead the application to abort.
    /// </summary>
    Fatal
}
