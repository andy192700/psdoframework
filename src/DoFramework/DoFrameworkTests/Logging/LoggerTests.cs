using DoFramework.CLI;
using DoFramework.Logging;
using FluentAssertions;
using Moq;

namespace DoFrameworkTests.Logging;

public class LoggerTests
{
    [Theory]
    [InlineAutoMoqData(LogLevel.Trace, ConsoleColor.Gray)]
    [InlineAutoMoqData(LogLevel.Debug, ConsoleColor.Cyan)]
    [InlineAutoMoqData(LogLevel.Info, ConsoleColor.White)]
    [InlineAutoMoqData(LogLevel.Warning, ConsoleColor.Yellow)]
    [InlineAutoMoqData(LogLevel.Error, ConsoleColor.Red)]
    [InlineAutoMoqData(LogLevel.Fatal, ConsoleColor.DarkRed)]
    public void Log_IsLogged(
        LogLevel logLevel, 
        ConsoleColor expectedColor, 
        string message)
    {
        // Arrange
        var mockConsole = new Mock<IConsoleWrapper>();
        var logger = new Logger(mockConsole.Object);

        // Act
        switch (logLevel)
        {
            case LogLevel.Trace:
            {
                logger.LogTrace(message);
                break;
            }
            case LogLevel.Debug:
            {
                logger.LogDebug(message);
                break;
            }
            case LogLevel.Info:
            {
                logger.LogInfo(message);
                break;
            }
            case LogLevel.Warning:
            {
                logger.LogWarning(message);
                break;
            }
            case LogLevel.Error:
            {
                logger.LogError(message);
                break;
            }
            case LogLevel.Fatal:
            {
                logger.LogFatal(message);
                break;
            }
        }

        // Assert
        mockConsole.Verify(c => c.WriteLine(It.IsAny<string>()), Times.Once);

        mockConsole.Verify(c => c.SetForegroundColor(expectedColor), Times.Once);
    }

    [Theory]
    [InlineAutoMoqData(LogLevel.Trace, ConsoleColor.Gray)]
    [InlineAutoMoqData(LogLevel.Debug, ConsoleColor.Cyan)]
    [InlineAutoMoqData(LogLevel.Info, ConsoleColor.White)]
    [InlineAutoMoqData(LogLevel.Warning, ConsoleColor.Yellow)]
    [InlineAutoMoqData(LogLevel.Error, ConsoleColor.Red)]
    [InlineAutoMoqData(LogLevel.Fatal, ConsoleColor.DarkRed)]
    public void Log_IsNotLogged(
        LogLevel logLevel,
        ConsoleColor expectedColor,
        CLIFunctionParameters parameters,
        string message)
    {
        // Arrange
        parameters.Parameters!["silent"] = true;

        var mockConsole = new Mock<IConsoleWrapper>();
        var logger = new Logger(mockConsole.Object);

        logger.Parameters = parameters;

        // Act
        switch (logLevel)
        {
            case LogLevel.Trace:
                {
                    logger.LogTrace(message);
                    break;
                }
            case LogLevel.Debug:
                {
                    logger.LogDebug(message);
                    break;
                }
            case LogLevel.Info:
                {
                    logger.LogInfo(message);
                    break;
                }
            case LogLevel.Warning:
                {
                    logger.LogWarning(message);
                    break;
                }
            case LogLevel.Error:
                {
                    logger.LogError(message);
                    break;
                }
            case LogLevel.Fatal:
                {
                    logger.LogFatal(message);
                    break;
                }
        }

        // Assert
        mockConsole.Verify(c => c.WriteLine(It.IsAny<string>()), Times.Never);

        mockConsole.Verify(c => c.SetForegroundColor(expectedColor), Times.Never);
    }
}