namespace SCP079SoC.Models.Configuration;

using Controllers;

public class DatabaseSettings
{
    private DatabaseConfig _cfg = ConfigManager.BotConfig.Database;
    public string GetConnString => string.Concat(new[]
    {
        $"Server={_cfg.Address}; Port={_cfg.Port}; " +
        $"Database={_cfg.DatabaseName}; " +
        $"User Id={_cfg.Username}; " +
        $"Password={_cfg.Password};"
    });
}