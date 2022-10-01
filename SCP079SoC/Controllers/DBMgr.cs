namespace SCP079SoC.Controllers;

using LinqToDB;
using LinqToDB.Data;
using Models.Configuration;

public static class DBMgr
{
    public static async Task Init()
    {
        DataConnection.DefaultSettings = new DatabaseSettings();

        await using (var db = new BotDatabase())
        {
            try
            {
                _ = db.GetTable<GuildConfig>().Any();
            }
            catch
            {
                await db.CreateTableAsync<GuildConfig>();
            }
        }
    }
}