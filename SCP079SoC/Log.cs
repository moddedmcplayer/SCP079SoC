namespace SCP079SoC;

using Controllers;
using Enums;

public static class Log
{
    public static void Debug(string message, DebugLevel level = DebugLevel.Debug)
    {
        if((int)level <= (int)ConfigManager.BotConfig.DebugLevel)
        {
            string prefix = level switch
            {
                DebugLevel.Debug => "[Debug]",
                DebugLevel.Info => "[INFO]",
                DebugLevel.Warning => "[WARNING]",
                DebugLevel.Error => "[ERROR]",
                _ => "[MESSAGE]",
            };
            
            ConsoleColor MessageColor = level switch
            {
                DebugLevel.Debug => ConsoleColor.Green,
                DebugLevel.Info => ConsoleColor.White,
                DebugLevel.Warning => ConsoleColor.Yellow,
                DebugLevel.Error => ConsoleColor.Red,
                _ => ConsoleColor.Cyan,
            };
            
            ConsoleColor PrefixColor = level switch
            {
                DebugLevel.Debug => ConsoleColor.DarkGreen,
                DebugLevel.Info => ConsoleColor.Gray,
                DebugLevel.Warning => ConsoleColor.DarkYellow,
                DebugLevel.Error => ConsoleColor.DarkMagenta,
                _ => ConsoleColor.DarkCyan,
            };
            
            LogColored(prefix, PrefixColor, message, MessageColor);
        }
    }
    
    public static void LogFatal(string message, int exitCode = 0)
    {
        LogColored("[FATAL]", ConsoleColor.DarkRed, message, ConsoleColor.Red);
        Environment.Exit(exitCode);
    }

    public static void LogColored(string? prefix, ConsoleColor? prefixColor, string message, ConsoleColor messageColor, bool addSpacing = true)
    {
        if (prefix is not null && prefixColor is not null)
        {
            Console.ForegroundColor = prefixColor.Value;
            if (addSpacing)
                prefix += " ";
            Console.Write(prefix);
        }
        Console.ForegroundColor = messageColor;
        Console.WriteLine(message);
        Console.ForegroundColor = ConsoleColor.White;
    }
}