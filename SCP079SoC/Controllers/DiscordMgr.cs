namespace SCP079SoC.Controllers;

using System.Reflection;
using Discord;
using Discord.Commands;
using Discord.Interactions;
using Discord.WebSocket;
using Enums;

public class DiscordMgr
{
    public static DiscordSocketClient Client;
    public static InteractionService InteractionService;
    public static CommandService CommandService;

    public static async Task Init()
    {
        AppDomain.CurrentDomain.ProcessExit += (_, _) => Client.LogoutAsync();
        
        var debugLevel = await ConfigManager.BotConfig.DebugLevel.ToLogServerity();

        var discordConfig = new DiscordSocketConfig()
        {
            LogLevel = debugLevel,
            GatewayIntents = GatewayIntents.All,
            AlwaysDownloadUsers = true,
            MessageCacheSize = 1000,
#if DEBUG
            UseInteractionSnowflakeDate = false
#endif
        };

        Client = new DiscordSocketClient(discordConfig);
        InteractionService = new InteractionService(Client, new InteractionServiceConfig());
        CommandService = new CommandService(new CommandServiceConfig()
        {
            LogLevel = debugLevel,
            CaseSensitiveCommands = false,
            DefaultRunMode = Discord.Commands.RunMode.Async
        });
    }
    
    public static async Task Start()
    {
        await Client.LoginAsync(TokenType.Bot, ConfigManager.BotConfig.Token);
        await Client.StartAsync();

        // Just make sure discord never gets fast enough to ready and register commands before modules register
        Client.Ready += async () =>
        {
            Log.Debug("Discord client ready", DebugLevel.Info);
#if !DEBUG
            await InteractionService.RegisterCommandsGloballyAsync();
#endif
        };

        InteractionService.Log += LogDiscord;
        Client.Log += LogDiscord;
            
        
        Client.MessageReceived += HandleCommand;
        Client.InteractionCreated += HandleInteraction;

        await CommandService.AddModulesAsync(assembly: Assembly.GetEntryAssembly(), 
            services: null);
        await InteractionService.AddModulesAsync(assembly: Assembly.GetEntryAssembly(), 
            services: null);
        
        await Task.Delay(-1);
    }

    private static async Task HandleCommand(SocketMessage socketMessage)
    {
        var message = socketMessage as SocketUserMessage;
        if (message is null || message.Author.IsBot) 
            return;

        Log.Debug($"Handling command: {message}");
        var guild = (socketMessage.Channel as ITextChannel)?.Guild;
        string prefix = guild is null ? "!" : (await guild.GetConfig()).Prefix;
        int argumentPosition = 0;
        if (!(message.HasStringPrefix(prefix, ref argumentPosition) || 
              message.HasMentionPrefix(Client.CurrentUser, ref argumentPosition)))
            return;
        
        var context = new SocketCommandContext(Client, message);
        await CommandService.ExecuteAsync(
            context: context, 
            argPos: argumentPosition,
            services: null);
    }
    
    private static async Task LogDiscord(LogMessage msg)
    {
        Log.LogColored("[DEBUG][Discord]", ConsoleColor.DarkBlue, msg.Message + msg.Exception, ConsoleColor.DarkGreen, false,
            3 <= (int)ConfigManager.BotConfig.DebugLevel);
    }
    
    private static async Task HandleInteraction(SocketInteraction interaction)
    {
        try
        {
            var context = new SocketInteractionContext(Client, interaction);
            await InteractionService.ExecuteCommandAsync(context, null);
            Log.Debug($"{interaction.User.Username} used an interaction: {interaction.Type.ToString()}");
        }
        catch (Exception e)
        {
            Log.Debug(e.ToString(), DebugLevel.Error);
            if (interaction.Type is InteractionType.ApplicationCommand)
            {
                try
                {
                    await interaction.RespondAsync(embed: await ErrorHandler.GetErrorEmbed(e));
                }
                catch
                {
                    // ignored
                }
            }
        }
    }
}