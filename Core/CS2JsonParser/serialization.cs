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
            using (StreamReader reader = new StreamReader(body, Encoding.UTF8))
            {
                string json = reader.ReadToEnd();
                playerStats = JsonSerializer.Deserialize(json, PlayerStatsJsonContext.Default.PlayerStats);
            }
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

