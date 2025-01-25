using DoFramework.CLI;

namespace DoFramework.Logging;

/// <summary>
/// Class for logging messages with various severity levels to the console.
/// </summary>
public class Logger : ILogger
{
    public CLIFunctionParameters? Parameters { get; set; }

    private readonly IConsoleWrapper _consoleWrapper;

    public Logger(IConsoleWrapper consoleWrapper)
    {
        _consoleWrapper = consoleWrapper;
    }

    /// <inheritdoc />
    public void LogDebug(string message)
    {
        Log(LogLevel.Debug, message);
    }

    /// <inheritdoc />
    public void LogError(string message)
    {
        Log(LogLevel.Error, message);
    }

    /// <inheritdoc />
    public void LogFatal(string message)
    {
        Log(LogLevel.Fatal, message);
    }

    /// <inheritdoc />
    public void LogInfo(string message)
    {
        Log(LogLevel.Info, message);
    }

    /// <inheritdoc />
    public void LogTrace(string message)
    {
        Log(LogLevel.Trace, message);
    }

    /// <inheritdoc />
    public void LogWarning(string message)
    {
        Log(LogLevel.Warning, message);
    }

    /// <inheritdoc />
    private void Log(LogLevel logLevel, string message)
    {
        var doLog = Parameters is null ? true : !Parameters.ParseSwitch("silent");

        if (doLog)
        {
            var color = logLevel switch
            {
                LogLevel.Trace => ConsoleColor.Gray,
                LogLevel.Debug => ConsoleColor.Cyan,
                LogLevel.Info => ConsoleColor.White,
                LogLevel.Warning => ConsoleColor.Yellow,
                LogLevel.Error => ConsoleColor.Red,
                LogLevel.Fatal => ConsoleColor.DarkRed,
                _ => ConsoleColor.White,
            };

            _consoleWrapper.SetForegroundColor(color);

            var timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");

            _consoleWrapper.WriteLine($"[{timestamp}] [{logLevel.ToString().ToUpper()}]: {message}");

            _consoleWrapper.ResetColor();
        }
    }
}
