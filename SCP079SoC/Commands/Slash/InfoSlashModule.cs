namespace SCP079SoC.Commands.Slash;

using Discord.Interactions;

public class InfoSlashModule : InteractionModuleBase<SocketInteractionContext>
{
    [SlashCommand("ping", "Pong!")]
    public async Task PingAsync()
    {
        await RespondAsync("Pong!", null, true, true);
    }
}