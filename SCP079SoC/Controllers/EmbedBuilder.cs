namespace SCP079SoC.Controllers;

using Discord;

public static class EmbedBuilder
{
    public static async Task<Embed> CreateEmbed(EmbedInfo info)
    {
        return new Discord.EmbedBuilder().WithTitle(info.Title).WithDescription(info.Description).WithColor(info.Color).WithCurrentTimestamp().Build();
    }
    
    public class EmbedInfo
    {
        public string Title = "";
        public string Description = "";
        public Color Color = Color.DarkerGrey;
    }
}