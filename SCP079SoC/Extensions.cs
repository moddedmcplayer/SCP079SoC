namespace SCP079SoC;

using Discord;
using Discord.WebSocket;
using Models.Configuration;

public static class Extensions
{
    public static async Task<GuildConfig> GetConfig(this IGuild guild)
    {
        return new GuildConfig();
    }
}