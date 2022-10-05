namespace SCP079SoC;

using System.Diagnostics;
using Controllers;
using Discord;
using Enums;
using Models.Configuration;
using Models.Entities;
using MySqlConnector;

public static class DBExtensions
{
    public static async Task CreateUserData(IUser user)
    {
        await using var conn = new MySqlConnection(DBMgr.ConnectionString);
        await conn.OpenAsync();
        await using (var cmd = new MySqlCommand())
        {
            cmd.Connection = conn;
            cmd.CommandText = "INSERT INTO users(UserId, Money, Reputation, UsedReputationTime, UsedPaydayTime) VALUES(@UserId, @Money, @Reputation, @UsedReputationTime, @UsedPaydayTime)";
            cmd.Parameters.AddWithValue("@UserId", user.Id.ToString());
            cmd.Parameters.AddWithValue("@Money", 0);
            cmd.Parameters.AddWithValue("@Reputation", 0);
            cmd.Parameters.AddWithValue("@UsedReputationTime", 0.ToString());
            cmd.Parameters.AddWithValue("@UsedPaydayTime", 0.ToString());

            await cmd.ExecuteNonQueryAsync();
        }
    }
    
    public static async Task<UserEntity> GetUserData(this IUser user)
    {
        await using (var conn = new MySqlConnection(DBMgr.ConnectionString))
        {
            await conn.OpenAsync();
            await using (MySqlCommand cmd = new MySqlCommand())
            {
                cmd.Connection = conn;
                cmd.CommandText = "SELECT * FROM users WHERE UserId = @userid";
                cmd.Parameters.AddWithValue("@userid", user.Id);
                
                await using (MySqlDataReader reader = await cmd.ExecuteReaderAsync())
                {
                    if (!reader.HasRows)
                    {
                        await CreateUserData(user);
                        return await GetUserData(user);
                    }
                    
                    while (reader.Read())
                    {
                        return new UserEntity
                        {
                            Id = user.Id,
                            Money = reader.GetInt64(2),
                            Reputation = reader.GetInt32(3),
                            UsedRepTime = Convert.ToInt64(reader.GetString(4)),
                            UsedPaydayTime = Convert.ToInt64(reader.GetString(5))
                        };
                    }
                }
            }
        }
        
        Log.Debug($"Could not find user for userid {user.Id}, {user.Username}", DebugLevel.Error);
        var stacktrace = new StackTrace();
        Log.Debug($"Stacktrace: {string.Join(",", stacktrace.GetFrames().SelectMany(x => x.ToString()))}");
        return null;
    }
    
    public static async Task CreateGuildConfig(IGuild guild)
    {
        await using var conn = new MySqlConnection(DBMgr.ConnectionString);
        await conn.OpenAsync();
        await using (var cmd = new MySqlCommand())
        {
            cmd.Connection = conn;
            cmd.CommandText = "INSERT INTO guildconfigs(GuildId, Prefix) VALUES(@GuildId, @Prefix)";
            cmd.Parameters.AddWithValue("@GuildId", guild.Id.ToString());
            cmd.Parameters.AddWithValue("@Prefix", '!');

            await cmd.ExecuteNonQueryAsync();
        }
    }
    
    public static async Task<GuildConfig> GetConfig(this IGuild guild)
    {
        await using (var conn = new MySqlConnection(DBMgr.ConnectionString))
        {
            await conn.OpenAsync();
            await using (MySqlCommand cmd = new MySqlCommand())
            {
                cmd.Connection = conn;
                cmd.CommandText = "SELECT * FROM guildconfigs WHERE GuildId = @guildid";
                cmd.Parameters.AddWithValue("@guildid", guild.Id);
                
                await using (MySqlDataReader reader = await cmd.ExecuteReaderAsync())
                {
                    if (!reader.HasRows)
                    {
                        await CreateGuildConfig(guild);
                        return await GetConfig(guild);
                    }
                    
                    while (reader.Read())
                    {
                        return new GuildConfig
                        {
                            GuildId = guild.Id,
                            Prefix = reader.GetString(2)
                        };
                    }
                }
            }
        }
        
        Log.Debug($"Could not find guild for guildid {guild.Id}, {guild.Name}", DebugLevel.Error);
        var stacktrace = new StackTrace();
        Log.Debug($"Stacktrace: {string.Join(",", stacktrace.GetFrames().SelectMany(x => x.ToString()))}");
        return null;
    }
}