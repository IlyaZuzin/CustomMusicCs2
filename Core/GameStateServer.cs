// ---------------------------------------------------------------------------------------------
// GameStateServer.cs — порт legacy-класса Server.cs. HTTP-сервер на 127.0.0.1:3000, принимает
// JSON gamestate от CS2, десериализует его и отдаёт в EventDispatcher. Никакой бизнес-логики
// событий здесь нет — только приём пакетов и ответ "OK".
// ---------------------------------------------------------------------------------------------
using System.Net;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace MusicCore;

public sealed class GameStateServer
{
    private HttpListener listener = new();
    private CancellationTokenSource? cts;
    public bool IsRunning { get; private set; }

    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true,
        NumberHandling = JsonNumberHandling.AllowReadingFromString | JsonNumberHandling.WriteAsString
    };

    public event Action<PlayerStats>? PacketReceived;
    public event Action<string>? RawPacketReceived;   // для логирования сырого JSON

    public bool Start(string prefix = "http://127.0.0.1:3000/")
    {
        if (IsRunning) return true;
        try
        {
            listener = new HttpListener();
            listener.Prefixes.Add(prefix);
            listener.Start();
            cts = new CancellationTokenSource();
            _ = Task.Run(() => AcceptLoopAsync(cts.Token));
            IsRunning = true;
            CoreEvents.WriteLog($"Start server {prefix}");
            return true;
        }
        catch (Exception ex)
        {
            CoreEvents.WriteLog($"Ошибка запуска сервера: {ex.Message}");
            IsRunning = false;
            return false;
        }
    }

    public void Stop()
    {
        if (!IsRunning) return;
        try
        {
            cts?.Cancel();
            listener?.Stop();
            IsRunning = false;
            CoreEvents.WriteLog("Сервер остановлен.");
        }
        catch (Exception ex)
        {
            CoreEvents.WriteLog($"Ошибка при остановке сервера: {ex.Message}");
        }
    }

    private async Task AcceptLoopAsync(CancellationToken token)
    {
        while (!token.IsCancellationRequested && listener.IsListening)
        {
            try
            {
                var context = await listener.GetContextAsync().ConfigureAwait(false);
                _ = Task.Run(() => ProcessRequestAsync(context), token);
            }
            catch (HttpListenerException) { break; }
            catch (Exception ex) { CoreEvents.WriteLog($"[Ошибка JSON]: {ex.Message}"); }
        }
    }

    private async Task ProcessRequestAsync(HttpListenerContext context)
    {
        try
        {
            string readableText;
            using (var body = context.Request.InputStream)
            using (var reader = new StreamReader(body, Encoding.UTF8))
            {
                readableText = await reader.ReadToEndAsync().ConfigureAwait(false);
            }

            RawPacketReceived?.Invoke(readableText);

            PlayerStats? stats = JsonSerializer.Deserialize<PlayerStats>(readableText, JsonOptions);
            if (stats != null) PacketReceived?.Invoke(stats);

            // Обязательно отвечаем игре "OK", иначе она перестанет слать пакеты
            byte[] buffer = Encoding.UTF8.GetBytes("OK");
            context.Response.ContentLength64 = buffer.Length;
            using (var output = context.Response.OutputStream)
            {
                await output.WriteAsync(buffer, 0, buffer.Length).ConfigureAwait(false);
            }
            context.Response.Close();
        }
        catch (Exception ex)
        {
            CoreEvents.WriteLog($"Error parsing JSON: {ex.Message}");
            try { context.Response.Abort(); } catch { }
        }
    }
}
