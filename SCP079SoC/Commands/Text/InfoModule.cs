namespace SCP079SoC.Commands.Text;

using Discord;
using Discord.Commands;
using EmbedBuilder = Controllers.EmbedBuilder;

public class InfoModule : ModuleBase<SocketCommandContext>
{
    [Command("help")]
    [Summary("Displays help.")]
    public async Task HelpAsync(
        [Summary("The command to display help for.")]
        string command = "")
    {
        await ReplyAsync(embed: await EmbedBuilder.CreateEmbed(new EmbedBuilder.AdvancedEmbedInfo()
        {
            Title = "Shank people like a british",
            Description = @"__**Help**__ 
                          `help`",
            Color = Color.Red,
            Author = "SCP 079 SoC Help Menu",
            IconUrl = Context.Client.CurrentUser.GetAvatarUrl(),
            Footer = "You can use the `help` command to get help for a specific command.",
        }));
    }
}