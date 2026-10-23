using System.Text.Json;
using System.Text.Json.Serialization;
using CS2Parser;
using System.Net;
using System.IO;
using System.Text;
using Files;

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

        public static Setting JsonSetting(string path)
        {
            string json = File.ReadAllText(path);
            Setting? setting = JsonSerializer.Deserialize(json, SettingJsonContext.Default.Setting);
            if (setting == null) { setting = new Setting(); }
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

    [JsonSerializable(typeof(Setting))]
    public partial class SettingJsonContext : JsonSerializerContext
    {
    }
}

