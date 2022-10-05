namespace SCP079SoC.Commands.Text;

using Controllers;
using Discord.Commands;
using Discord.WebSocket;
using Enums;
using MySqlConnector;

public class FunModule : ModuleBase<SocketCommandContext>
{
    [Priority(1)]
    [Command("rep")]
    [Summary("Check your reputation cooldown.")]
    public async Task RepAsync()
    {
        var userData = await Context.User.GetUserData();
        var repTime = userData.UsedRepTime;
        var time = TimeSpan.FromTicks(DateTime.UtcNow.Ticks - repTime);
        var timeNext = TimeSpan.FromTicks((repTime + TimeSpan.FromHours(12).Ticks) - DateTime.UtcNow.Ticks);
        await ReplyAsync(time.TotalHours > 12
            ? "You can give a reputation point!"
            : $"You can give reputation again in {timeNext.Hours} hours, {timeNext.Minutes} minutes, {timeNext.Seconds} seconds.");
    }

    [Priority(0)]
    [Command("rep")]
    [Summary("Give somebody reputation.")]
    public async Task RepAsync(
        [Summary("The user to give rep to.")]
        SocketUser? user)
    {
        if(user == null)
        {
            await ReplyAsync("Cannot find user.");
            return;
        }
        
        var userData = await Context.User.GetUserData();
        var repTime = userData.UsedRepTime;
        Log.AssertNotNull(repTime, "repTime");
        var time = TimeSpan.FromTicks(DateTime.UtcNow.Ticks - repTime);
        var timeNext = TimeSpan.FromTicks((repTime + TimeSpan.FromHours(12).Ticks) - DateTime.UtcNow.Ticks);
        if (time.TotalHours < 12)
        {
            await ReplyAsync($"You can give reputation again in {timeNext.Hours} hours, {timeNext.Minutes} minutes, {timeNext.Seconds} seconds.");
            return;
        }
        
        if(user.Id == Context.User.Id)
        {
            await ReplyAsync("You cannot give reputation to yourself.");
            return;
        }

        try
        {
            await user.GetUserData(); // make sure user exists
            await using (var conn = new MySqlConnection(DBMgr.ConnectionString))
            {
                await conn.OpenAsync();
                await using (var cmd = new MySqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = "UPDATE users SET Reputation = Reputation + 1 WHERE UserId = @id";
                    cmd.Parameters.AddWithValue("@id", user.Id);
                    await cmd.ExecuteNonQueryAsync();
                }
                await using (var cmd = new MySqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = "UPDATE users SET UsedReputationTime = @time WHERE UserId = @id";
                    cmd.Parameters.AddWithValue("@id", Context.User.Id);
                    cmd.Parameters.AddWithValue("@time", DateTime.UtcNow.Ticks.ToString());
                    await cmd.ExecuteNonQueryAsync();
                }
            }
        }
        catch (Exception e)
        {
            Log.Debug(e.ToString(), DebugLevel.Error);
            await ReplyAsync(embed: await ErrorHandler.GetErrorEmbed(e));
        }
        
        await ReplyAsync($"You gave a reputation point to {user.Mention}!");
    }

    [Command("payday")]
    [Alias("pd")]
    [Summary("Get daily money.")]
    public async Task PaydayAsync()
    {
        var userData = await Context.User.GetUserData();
        var pdTime = userData.UsedPaydayTime;
        Log.AssertNotNull(pdTime, "pdTime");
        var time = TimeSpan.FromTicks(DateTime.UtcNow.Ticks - pdTime);
        var timeNext = TimeSpan.FromTicks((pdTime + TimeSpan.FromHours(24).Ticks) - DateTime.UtcNow.Ticks);
        
        if (time.TotalHours < 24)
        {
            await ReplyAsync($"Next payday in {timeNext.Hours} hours, {timeNext.Minutes} minutes, {timeNext.Seconds} seconds.");
            return;
        }

        int leaderboardSpot = 0;
        
        try
        {
            await using (var conn = new MySqlConnection(DBMgr.ConnectionString))
            {
                await conn.OpenAsync();
                await using (var cmd = new MySqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = "UPDATE users SET Money = Money + 10000, UsedPaydayTime = @time WHERE UserId = @id";
                    cmd.Parameters.AddWithValue("@id", userData.Id);
                    cmd.Parameters.AddWithValue("@time", DateTime.UtcNow.Ticks);
                    await cmd.ExecuteNonQueryAsync();
                }
                
                await using (var cmd = new MySqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = "SELECT COUNT(Money) FROM users WHERE money >= @userMoney";
                    cmd.Parameters.AddWithValue("@userMoney", userData.Money);
                    
                    await using (MySqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        reader.Read();
                        leaderboardSpot = reader.GetInt32(0);
                    }
                }
            }
        }
        catch (Exception e)
        {
            Log.Debug(e.ToString(), DebugLevel.Error);
            await ReplyAsync(embed: await ErrorHandler.GetErrorEmbed(e));
        }

        await ReplyAsync("Here, have some 💰! **(+10k$)** \n" +
                         $"You currently have {userData.Money + 10000:#,###0}$. \n" +
                         $"You're currently **#{(leaderboardSpot != 0 ? leaderboardSpot : "idk")}** on the global leaderboard!");
    }
}