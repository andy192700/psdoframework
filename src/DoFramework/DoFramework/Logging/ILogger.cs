using DoFramework.CLI;

namespace DoFramework.Logging;

/// <summary>
/// Interface for logging messages with various severity levels.
/// </summary>
public interface ILogger
{
    /// <summary>
    /// CLI Parameters.
    /// </summary>
    CLIFunctionParameters? Parameters { get; set; }

    /// <summary>
    /// Logs a debug message.
    /// </summary>
    /// <param name="message">The debug message to log.</param>
    void LogDebug(string message);

    /// <summary>
    /// Logs an informational message.
    /// </summary>
    /// <param name="message">The informational message to log.</param>
    void LogInfo(string message);

    /// <summary>
    /// Logs a trace message.
    /// </summary>
    /// <param name="message">The trace message to log.</param>
    void LogTrace(string message);

    /// <summary>
    /// Logs an error message.
    /// </summary>
    /// <param name="message">The error message to log.</param>
    void LogError(string message);

    /// <summary>
    /// Logs a fatal error message.
    /// </summary>
    /// <param name="message">The fatal error message to log.</param>
    void LogFatal(string message);

    /// <summary>
    /// Logs a warning message.
    /// </summary>
    /// <param name="message">The warning message to log.</param>
    void LogWarning(string message);
}
