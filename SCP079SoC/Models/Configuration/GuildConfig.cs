namespace SCP079SoC.Models.Configuration;

public class GuildConfig
{
    public ulong GuildId { get; set; }

    public string Prefix { get; set; }

    internal GuildConfig()
    {
    }

    internal GuildConfig(ulong guildId)
    {
        GuildId = guildId;
        Prefix = "!";
    }
}