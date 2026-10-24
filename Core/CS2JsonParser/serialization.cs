using System.Text.Json;
using System.Text.Json.Serialization;
using CS2Parser;
using System.Net;
using System.IO;
using core_settings;

namespace serialization
{
    public static class Serialize{
        public static PlayerStats? JsonPlayerData(HttpListenerRequest httpListenerRequest)
        {
            PlayerStats? playerStats;
            using (Stream body = httpListenerRequest.InputStream)
                playerStats = JsonSerializer.Deserialize(body, PlayerStatsJsonContext.Default.PlayerStats);
            
            return playerStats;
        } 

        public static Settings_Api JsonSetting(string path)
        {
            string json = File.ReadAllText(path);
            Settings_Api? setting = JsonSerializer.Deserialize(json, SettingJsonContext.Default.Settings_Api);
            if (setting == null) { setting = new Settings_Api(); }
            return setting;
        }
    }

    
    [JsonSerializable(typeof(PlayerStats))]
    [JsonSerializable(typeof(PlayerStats.Provider))]
    [JsonSerializable(typeof(PlayerStats.Map))]
    [JsonSerializable(typeof(PlayerStats.Team))]
    [JsonSerializable(typeof(PlayerStats.Round))]
    [JsonSerializable(typeof(PlayerStats.Player))]
    [JsonSerializable(typeof(PlayerStats.State))]
    [JsonSerializable(typeof(PlayerStats.Stats))]
    public partial class PlayerStatsJsonContext : JsonSerializerContext
    {
    }

    [JsonSerializable(typeof(Settings_Api))]
    public partial class SettingJsonContext : JsonSerializerContext
    {
    }
}

