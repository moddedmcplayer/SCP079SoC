namespace SCP079SoC.Models.Configuration;

using LinqToDB.Mapping;

[Table(Name = "GuildConfigs")]
public class GuildConfig
{
    [PrimaryKey]
    public ulong GuildId { get; set; }
    
    [Column]
    public char Prefix { get; set; }
    
    internal GuildConfig(ulong guildId)
    {
        GuildId = guildId;
        Prefix = '!';
    }
}