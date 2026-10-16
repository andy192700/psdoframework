namespace DoFramework.Logging;

/// <summary>
/// Class that provides a wrapper for console operations.
/// Uses ANSI escape sequences for portable, pipeline-safe logging.
/// </summary>
public class ConsoleWrapper : IConsoleWrapper
{
    private ConsoleColor _currentForegroundColor = ConsoleColor.White;

    /// <summary>
    /// Writes the specified value to the console using ANSI color formatting.
    /// </summary>
    /// <param name="value">The value to write.</param>
    public void WriteLine(string value)
    {
        var ansiColor = _currentForegroundColor switch
        {
            ConsoleColor.Black => "30",
            ConsoleColor.DarkRed => "31",
            ConsoleColor.DarkGreen => "32",
            ConsoleColor.DarkYellow => "33",
            ConsoleColor.DarkBlue => "34",
            ConsoleColor.DarkMagenta => "35",
            ConsoleColor.DarkCyan => "36",
            ConsoleColor.Gray => "37",
            ConsoleColor.Red => "91",
            ConsoleColor.Green => "92",
            ConsoleColor.Yellow => "93",
            ConsoleColor.Blue => "94",
            ConsoleColor.Magenta => "95",
            ConsoleColor.Cyan => "96",
            ConsoleColor.White => "97",
            _ => "97"
        };

        var ansi = $"\u001b[{ansiColor}m{value}\u001b[0m";

        Console.Out.WriteLine(ansi);
    }

    /// <summary>
    /// Sets the foreground color for subsequent writes.
    /// </summary>
    /// <param name="color">The console color to set.</param>
    public void SetForegroundColor(ConsoleColor color)
    {
        _currentForegroundColor = color;
    }

    /// <summary>
    /// Resets the foreground color to default (White).
    /// </summary>
    public void ResetColor()
    {
        _currentForegroundColor = ConsoleColor.White;
    }
}