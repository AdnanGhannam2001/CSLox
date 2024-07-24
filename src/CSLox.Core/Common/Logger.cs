using static System.Console;
using CSLox.Core.Enums;
using System.Diagnostics;

namespace CSLox.Core.Common;

public static class Logger
{
    private static readonly string NewLine = "\n\r";
    private static ConsoleColor LevelToColor(LogLevel level)
    {
        return level switch
        {
            LogLevel.DEBUG => ConsoleColor.DarkGray,
            LogLevel.INFO => ConsoleColor.Green,
            LogLevel.WARN => ConsoleColor.Yellow,
            LogLevel.ERROR => ConsoleColor.Red,
            LogLevel.FATAL => ConsoleColor.DarkRed,
            LogLevel.TRACE => ConsoleColor.Blue,
            _ => throw new UnreachableException()
        };
    }

    private static void Log(LogLevel? level, string message, string? details = null)
    {
        if (level is not null)
        {
            ForegroundColor = LevelToColor((LogLevel) level);
            Write($"[{Enum.GetName(typeof(LogLevel), level)}]\t");
            ResetColor();
        }

        Write(message);

        if (details is not null)
        {
            Write($"{NewLine}\t{details.Replace("\n", "\n\t")}");
        }

        Write(NewLine);
    }

    public static void LogDebug(string message, string? details = null) => Log(LogLevel.DEBUG, message, details);
    public static void LogInfo(string message, string? details = null) => Log(LogLevel.INFO, message, details);
    public static void LogWarn(string message, string? details = null) => Log(LogLevel.WARN, message, details);
    public static void LogError(string message, string? details = null) => Log(LogLevel.ERROR, message, details);
    public static void LogFatal(string message, string? details = null) => Log(LogLevel.FATAL, message, details);
    public static void LogTrace(string message, string? details = null) => Log(LogLevel.TRACE, message, details);
    public static void LogMessage(string message, string? details = null) => Log(null, message, details);
}