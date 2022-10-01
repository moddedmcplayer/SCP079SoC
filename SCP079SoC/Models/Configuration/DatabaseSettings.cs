namespace SCP079SoC.Models.Configuration;

using Controllers;
using LinqToDB;
using LinqToDB.Configuration;

public class DatabaseSettings : ILinqToDBSettings
{
    public IEnumerable<IDataProviderSettings> DataProviders
        => Enumerable.Empty<IDataProviderSettings>();

    public string DefaultConfiguration => "SqlServer";
    public string DefaultDataProvider => "SqlServer";

    private DatabaseConfig _cfg = ConfigManager.BotConfig.Database;
    public IEnumerable<IConnectionStringSettings> ConnectionStrings => new[]
    {
        new ConnectionStringSettings(_cfg.DatabaseName,
            $"Server={_cfg.Address}; Port={_cfg.Port}; " +
            $"Database={_cfg.DatabaseName}; " +
            $"User Id={_cfg.Username}; " +
            $"Password={_cfg.Password};",
            ProviderName.MySql)
    };
}