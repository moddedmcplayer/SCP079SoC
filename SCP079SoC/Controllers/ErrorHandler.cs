namespace SCP079SoC.Controllers;

using Discord;

public static class ErrorHandler
{
    public static async Task<Embed> GetErrorEmbed(Exception? ex = null)
    {
        return await EmbedBuilder.CreateEmbed(new EmbedBuilder.EmbedInfo()
        {
            Title = "An Error occured",
            Description = ex?.ToString() ?? "Unknown",
            Color = Color.Red
        });
    }
}