namespace SCP079SoC;

using Discord;
using Enums;

public static class Converter
{
    public static readonly Dictionary<DebugLevel, LogSeverity> DebugLevelToLogSeverity = new Dictionary<DebugLevel, LogSeverity>
    {
        { DebugLevel.Debug, LogSeverity.Debug },
        { DebugLevel.Info, LogSeverity.Info },
        { DebugLevel.Warning, LogSeverity.Warning },
        { DebugLevel.Error, LogSeverity.Error }
    };
    
    public static async Task<LogSeverity> ToLogServerity(this DebugLevel debugLevel)
    {
        return DebugLevelToLogSeverity[debugLevel];
    }
}