namespace DoFramework.Logging;

/// <summary>
/// Class that provides a wrapper for console operations.
/// </summary>
public class ConsoleWrapper : IConsoleWrapper
{
    /// <summary>
    /// Writes the specified value to the console.
    /// </summary>
    /// <param name="value">The value to write.</param>
    public void WriteLine(string value)
    {
        Console.WriteLine(value);
    }

    /// <summary>
    /// Sets the foreground color of the console.
    /// </summary>
    /// <param name="color">The console color to set.</param>
    public void SetForegroundColor(ConsoleColor color)
    {
        Console.ForegroundColor = color;
    }

    /// <summary>
    /// Resets the console colors to their defaults.
    /// </summary>
    public void ResetColor()
    {
        Console.ResetColor();
    }
}
