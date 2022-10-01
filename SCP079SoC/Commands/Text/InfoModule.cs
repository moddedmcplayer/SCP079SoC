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
        await ReplyAsync(embed: await EmbedBuilder.CreateEmbed(new EmbedBuilder.EmbedInfo()
        {
            Title = "Help",
            Description = "`help`",
            Color = Color.Blue
        }));
    }
}