namespace DoFramework.Logging;

/// <summary>
/// Interface for console wrapper operations.
/// </summary>
public interface IConsoleWrapper
{
    /// <summary>
    /// Writes the specified value to the console.
    /// </summary>
    /// <param name="value">The value to write.</param>
    void WriteLine(string value);

    /// <summary>
    /// Sets the foreground color of the console.
    /// </summary>
    /// <param name="color">The console color to set.</param>
    void SetForegroundColor(ConsoleColor color);

    /// <summary>
    /// Resets the console colors to their defaults.
    /// </summary>
    void ResetColor();
}
