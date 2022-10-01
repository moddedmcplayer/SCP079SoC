namespace SCP079SoC;

using Discord;
using LinqToDB;
using Models.Configuration;

public static class Extensions
{
    public static async Task<GuildConfig> GetConfig(this IGuild guild)
    {
        using (var db = new BotDatabase())
        {
            var guildConfig = await db.GuildConfigs.FirstOrDefaultAsync(x => x.GuildId == guild.Id);
            if (guildConfig is null)
            {
                await db.InsertAsync(new GuildConfig(guild.Id));
                guildConfig = await db.GuildConfigs.FirstOrDefaultAsync(x => x.GuildId == guild.Id);
                
                if (guildConfig is null)
                {
                    Log.Debug($"Could not find inserted guild config with id {guild.Id}");
                    guildConfig = new GuildConfig(guild.Id);
                }
            }
            
            return guildConfig;
        }
    }
}