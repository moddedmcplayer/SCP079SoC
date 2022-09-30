namespace SCP079SoC.Models.Configuration;

using Enums;

public class BotConfig
{
    public string Token { get; set; }
    public DebugLevel DebugLevel { get; set; }

    internal BotConfig()
    {
        Token = "NULL";
        DebugLevel = DebugLevel.Debug;
    }
}