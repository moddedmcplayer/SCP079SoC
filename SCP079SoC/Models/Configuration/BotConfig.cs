namespace SCP079SoC.Models.Configuration;

using Enums;

public class BotConfig
{
    public string Token { get; set; }
    public DebugLevel DebugLevel { get; set; }
    public DatabaseConfig Database { get; set; }
    
    internal BotConfig()
    {
        Token = "NULL";
        Database = new DatabaseConfig();
        DebugLevel = DebugLevel.Debug;
    }
}

public class DatabaseConfig
{
    public string Address { get; set; } = "127.0.0.1";
    public string Port { get; set; } = "3306";
    public string Username { get; set; } = "root";
    public string Password { get; set; } = "password";
    public string DatabaseName { get; set; } = "botdb";
}