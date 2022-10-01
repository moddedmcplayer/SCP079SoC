namespace SCP079SoC.Controllers;

using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using Enums;

public class DiscordMgr
{
    public static DiscordSocketClient Client;
    public static InteractionService InteractionService;

    public static async Task Init()
    {
        AppDomain.CurrentDomain.ProcessExit += (_, _) => Client.LogoutAsync();
        
        var debugLevel = ConfigManager.BotConfig.DebugLevel.ToLogServerity();

        var discordConfig = new DiscordSocketConfig()
        {
            LogLevel = debugLevel,
            GatewayIntents = GatewayIntents.GuildMembers | GatewayIntents.MessageContent,
            AlwaysDownloadUsers = true,
            MessageCacheSize = 1000
        };
        
        Client = new DiscordSocketClient(discordConfig);
    }
    
    public static async Task Start()
    {
        await Client.LoginAsync(TokenType.Bot, ConfigManager.BotConfig.Token);
        await Client.StartAsync();

        Client.Ready += () =>
        {
            Log.Debug("Discord client ready", DebugLevel.Info);
            return Task.CompletedTask;
        };

        await Task.Delay(-1);
    }
}