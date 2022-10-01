using Discord;
using SCP079SoC;
using SCP079SoC.Controllers;

await ConfigManager.Init();

try
{
    TokenUtils.ValidateToken(TokenType.Bot, ConfigManager.BotConfig.Token);
}
catch
{
    Log.LogFatal("Token is invalid!");
}

await DiscordMgr.Init();
await DiscordMgr.Start();