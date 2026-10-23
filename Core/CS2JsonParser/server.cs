using System.Text.Json.Serialization;
using System.IO;
using System.Net;
using System.Text;
using System.Text.Json;
using Files;
namespace CS2Parser{
    public  static class Server
    {
        private static HttpListener listener = new HttpListener();
        private static readonly JsonSerializerOptions JsonOptions = new JsonSerializerOptions{
            PropertyNameCaseInsensitive = true,
            NumberHandling = JsonNumberHandling.AllowReadingFromString | JsonNumberHandling.WriteAsString
        };
        private static CancellationTokenSource cancellationToken = new CancellationTokenSource();
        public static bool start()
        {   
            try
            {
                string Addr = "http://127.0.0.1:3000/";

                listener.Prefixes.Clear();
                listener.Prefixes.Add(Addr);
                listener.Start();

                // Запускаем полностью асин обработку
                Task.Run(() => ReadJsonAsync(cancellationToken.Token));
            }
            catch (Exception ex)
            {
                FileSystem.Log(ex.Message);
            }
            return true;
        }

        private static async Task ReadJsonAsync(CancellationToken token)
        {
            while (!token.IsCancellationRequested && listener.IsListening)
            {
                try
                { 
                    // Ожидаем пакет
                    HttpListenerContext context = await listener.GetContextAsync().ConfigureAwait(false);
                    _ = Task.Run(() => ProcessingRequest(context), token);
                }
                catch (HttpListenerException)
                { 
                    break;
                }
                catch (Exception ex)
                {
                    FileSystem.Log(ex.Message);
                }
            }
        }

        private static async Task ProcessingRequest(HttpListenerContext context)
        {
            HttpListenerRequest request = context.Request;
            try
            {
                PlayerStats stats = new PlayerStats();
                //using (Stream body = request.InputStream)
                //{
                //    stats = await JsonSerializer.DeserializeAsync<PlayerStats>(body, JsonOptions).ConfigureAwait(false);

                //}
                using (Stream body = request.InputStream)
                using (StreamReader reader = new StreamReader(body, Encoding.UTF8))
                {
                    string readableText = await reader.ReadToEndAsync().ConfigureAwait(false);
                    stats = JsonSerializer.Deserialize<PlayerStats>(readableText, JsonOptions);
                }
                Task.Delay(5);
                try
                {
                    // Обязательно отвечаем игре "OK", иначе она перестанет слать пакеты
                    HttpListenerResponse response = context.Response;
                    byte[] buffer = Encoding.UTF8.GetBytes("OK");
                    response.ContentLength64 = buffer.Length;
                    using (Stream output = response.OutputStream)
                    {
                        output.Write(buffer, 0, buffer.Length);
                    }
                    response.Close();
                }
                catch {}
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error parsing JSON: {ex.Message}");
            }
        }
    }
    
}
class PlayerStats
    {
        [JsonPropertyName("provider")]
        public Provider? client { get; set; }

        [JsonPropertyName("round")]
        public Round? round { get; set; }

        [JsonPropertyName("player")]
        public Player? player { get; set; }

        [JsonPropertyName("map")]
        public Map? map { get; set; }
        public class Provider
        {
            [JsonPropertyName("steamid")]
            public long steamid { get; set; }

            [JsonPropertyName("timestamp")]
            public long time { get; set; }
        }

        public class Map
        {
            public string? mode { get; set; }
            public string? name { get; set; }
            public string? phase { get; set; }
            public int round { get; set; }

            [JsonPropertyName("team_ct")]
            public Team? team_ct { get; set; }

            [JsonPropertyName("team_t")]
            public Team? team_t { get; set; }
        }
        public class Team
        {
            public int score { get; set; }
            public int round_losses { get; set; }
            public int timeouts_remaining { get; set; }
            public int matches_won_series { get; set; }
        }
        public class Round_Map { }
        public class Round
        {
            [JsonPropertyName("phase")]
            public string? phase { get; set; }
            [JsonPropertyName("win_team")]
            public string? win_team { get; set; }
            [JsonPropertyName("bomb")]
            public string? bomb { get; set; }
        }

        public class Stats
        {
            [JsonPropertyName("mvps")]
            public int mvp { get; set; }
        }
        public class Player
        {
            public long steamid { get; set; }
            public string? name { get; set; }
            public int slot { get; set; }
            public string? team { get; set; }
            [JsonPropertyName("activity")]
            public string? activity { get; set; }

            [JsonPropertyName("state")]
            public State? state { get; set; }

            [JsonPropertyName("match_stats")]
            public Stats? MVP { get; set; }


        }
        public class State
        {
            public int health { get; set; }
            public int armor { get; set; }
            public bool helmet { get; set; }
            public int flashed { get; set; }
            public int smoked { get; set; }
            public int burning { get; set; }
            public int money { get; set; }

            [JsonPropertyName("round_kills")]
            public int round_kills { get; set; }
            public int round_killshs { get; set; }
            public int equip_value { get; set; }
        }
        public object Clone()
        {
            return MemberwiseClone();
        }
    }

    public class Profile
    {
        public string name { get; set; } = "Unnamed";
        public string? ct_path { get; set; }
        public string? t_path { get; set; }
        public bool Standart_mode { get; set; } = false; 
        public string? global_path { get; set; }

        public Profile Copy()
        {
            return (Profile)this.MemberwiseClone();
        }
    }