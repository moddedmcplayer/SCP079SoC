namespace SCP079SoC.Controllers;

using Discord;

public static class EmbedBuilder
{
    public static async Task<Embed> CreateEmbed(EmbedInfo info)
    {
        var embed = new Discord.EmbedBuilder()
            .WithTitle(info.Title)
            .WithDescription(info.Description)
            .WithColor(info.Color)
            .WithCurrentTimestamp();
        
        if (info is AdvancedEmbedInfo advancedEmbed)
        {
            embed.WithAuthor(advancedEmbed.Author, advancedEmbed.IconUrl)
                .WithFooter(advancedEmbed.Footer);
        }
        
        return embed.Build();
    }
    
    public class EmbedInfo
    {
        public string Title = "";
        public string Description = "";
        public Color Color = Color.DarkerGrey;
    }

    public class AdvancedEmbedInfo : EmbedInfo
    {
        public string Author = "";
        public string IconUrl = "";
        public string Footer = "";
    }
}