using System.Text.Json.Serialization;
using System.IO;
using System.Net;
using System.Text;
using System.Text.Json;
using Files;

namespace CS2Parser{
    public static class Server
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
