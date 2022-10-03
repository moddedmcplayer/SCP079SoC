namespace SCP079SoC.Controllers;

using Models.Configuration;
using Models.Entities;
using MySqlConnector;

public static class DBMgr
{
    private static DatabaseSettings DbCfg = new DatabaseSettings();
    public static string ConnectionString => DbCfg.GetConnString;
    
    public static async Task Init()
    {
        await using var conn = new MySqlConnection(ConnectionString);
        await conn.OpenAsync();
        await using (var command = new MySqlCommand())
        {
            command.Connection = conn;
            command.CommandText =
                "CREATE TABLE IF NOT EXISTS GuildConfigs(Id INTEGER AUTO_INCREMENT PRIMARY KEY, GuildId TEXT, Prefix TEXT)";
            await command.ExecuteNonQueryAsync();
        }
        await using (var command = new MySqlCommand())
        {
            command.Connection = conn;
            command.CommandText =
                "CREATE TABLE IF NOT EXISTS Users(Id INTEGER AUTO_INCREMENT PRIMARY KEY, UserId TEXT, Reputation INTEGER, UsedReputationTime TEXT)";
            await command.ExecuteNonQueryAsync();
        }
    }
}