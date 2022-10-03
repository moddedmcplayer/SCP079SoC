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
        var timeNext = TimeSpan.FromTicks((repTime + TimeSpan.FromHours(24).Ticks) - DateTime.UtcNow.Ticks);
        await ReplyAsync(time.TotalHours > 24
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
        if (time.TotalHours < 24)
        {
            await ReplyAsync($"You can give reputation again in {time.Hours} hours, {time.Minutes} minutes, {time.Seconds} seconds.");
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
}