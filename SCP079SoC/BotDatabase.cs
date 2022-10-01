namespace SCP079SoC;

using LinqToDB;
using LinqToDB.Data;
using Models.Configuration;

public class BotDatabase : DataConnection
{
    public ITable<GuildConfig> GuildConfigs => this.GetTable<GuildConfig>();
}