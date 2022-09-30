namespace SCP079SoC.Controllers;

using Models.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public static class ConfigManager
{
    public static BotConfig BotConfig { get; private set; }
    
    public static void Init()
    {
        try
        {
            if (!File.Exists("Settings.json"));
            {
                using (TextWriter tw = new StreamWriter("Settings.json"))
                {
                    tw.Write(JsonConvert.SerializeObject(new BotConfig(), Formatting.Indented));
                }
            }
            
            using (TextReader tr = new StreamReader("Settings.json"))
            {
                BotConfig = JsonConvert.DeserializeObject<BotConfig>(tr.ReadToEnd()) ?? throw new NullReferenceException("Config is null");
            }
        } 
        catch (Exception e)
        {
            Log.LogFatal("Could not load config: " + e);
        }
    }
}