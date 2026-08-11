
using System.IO;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;


namespace MusicComanderGUI
{
    class Server 
    {
        private static HttpListener listener = new HttpListener();
        private static PlayerStats? player = null;
        public static bool is_Running = false;
        public static bool DebugMode = false;
        // Ключи асинхронной функции
        private static CancellationTokenSource? cts;
        private static readonly JsonSerializerOptions JsonOptions = new JsonSerializerOptions{
            PropertyNameCaseInsensitive = true,
            NumberHandling = JsonNumberHandling.AllowReadingFromString | JsonNumberHandling.WriteAsString
        };
   
        // Запуск асинхронного сервера
        public static void Start()
        {
            if (is_Running) { return; }
            try
            {
                string Addr = "http://127.0.0.1:3000/";

                listener.Prefixes.Clear();
                listener.Prefixes.Add(Addr);
                listener.Start();
                cts = new CancellationTokenSource();

                // Запускаем полностью асин обработку
                Task.Run(() => ReadJsonAsync(cts.Token));
                Main.Instance?.SetConsoleLog($"Start server {Addr}");
                MusicKits.settings.Last = MusicKits.loadJson(Main.Ct);
                WavPlayer.SetupMusic();
                is_Running = true; 
            }
            catch (Exception ex)
            {
                Main.Instance?.SetConsoleLog($"Ошибка запуска сервера: {ex.Message}");
                is_Running = false;
            }
        }
        // Тут я думаю и так понятно
        public static void Stop()
        {
            if (!is_Running) return;

            try
            {
                WavPlayer.AudioStop();
                listener?.Stop();
                is_Running = false;
                Ivents.LastMusic = "kill";
                Ivents.Team = "Ct";
                cts?.Cancel();
                Main.Instance?.SetConsoleLog("Сервер остановлен.");
            }
            catch (Exception ex)
            {
                Main.Instance?.SetConsoleLog($"Ошибка при остановке сервера: {ex.Message}");
            }
         }
        // Запускеатель асинхронной функции
        private static async Task ReadJsonAsync(CancellationToken token)
        {
            while (!token.IsCancellationRequested && listener.IsListening)
            {
                try
                { 
                    HttpListenerContext context = await listener.GetContextAsync().ConfigureAwait(false);
                    _ = Task.Run(() => ProcessRequestAsync(context), token);
                }
                catch (HttpListenerException)
                { 
                    break;
                }
                catch (Exception ex)
                {
                    Main.Instance?.SetConsoleLog($"[Ошибка JSON]: {ex.Message}");
                }
            }
        }
        // Сама блядская функция
        private static async Task ProcessRequestAsync(HttpListenerContext context)
        {
            HttpListenerRequest request = context.Request;
            string jsonString = string.Empty;
           
            try
            {
                PlayerStats stats = new PlayerStats();
                using (Stream body = request.InputStream)
                {
                    stats = await JsonSerializer.DeserializeAsync<PlayerStats>(body, JsonOptions).ConfigureAwait(false);
                }

                await Ivents.Tick(stats, cts.Token);
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

        private static string? GetUpdate(string json)
        {
            int index = json.LastIndexOf("update");
            if (index == -1)
                return null;
            
            return json.Substring(index);
        }
    }
}

