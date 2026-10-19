// ---------------------------------------------------------------------------------------------
// GameWindowMonitor.cs — определяет состояние окна CS2 (cs2.exe): открыто / свёрнуто /
// восстановлено / закрыто, и порождает соответствующие события ядра. Опрашивает WinAPI раз в
// 2 секунды; при завершении процесса игры вызывает Core.GameClose().
// Использует только user32/kernel32 через P/Invoke — без внешних зависимостей.
// ---------------------------------------------------------------------------------------------
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace MusicCore;

internal static class GameWindowMonitor
{
    [DllImport("user32.dll")] private static extern IntPtr FindWindow(string? lpClassName, string lpWindowName);
    [DllImport("user32.dll")] private static extern bool IsIconic(IntPtr hWnd);
    [DllImport("user32.dll")] private static extern bool IsWindowVisible(IntPtr hWnd);

    private static Timer? timer;
    private static bool attached = false;
    private static bool lastMinimized = true;   // «свёрнуто» как базовое состояние — первый апхолст даст Maximize

    public static void Attach()
    {
        if (attached) return;
        attached = true;
        lastMinimized = !IsGameForeground();
        timer = new Timer(_ => Poll(), null, TimeSpan.FromSeconds(2), TimeSpan.FromSeconds(2));
    }

    public static void Detach()
    {
        attached = false;
        timer?.Dispose();
        timer = null;
    }

    private static Process? FindGameProcess()
    {
        try
        {
            foreach (var p in Process.GetProcessesByName("cs2")) return p;
            foreach (var p in Process.GetProcessesByName("csgo")) return p; // legacy
        }
        catch { }
        return null;
    }

    private static bool IsGameForeground()
    {
        var p = FindGameProcess();
        if (p == null) return false;
        try
        {
            IntPtr hWnd = p.MainWindowHandle;
            if (hWnd == IntPtr.Zero)
                hWnd = FindWindow(null, "Counter-Strike 2");
            if (hWnd == IntPtr.Zero) return false;
            return IsWindowVisible(hWnd) && !IsIconic(hWnd);
        }
        catch { return false; }
    }

    private static void Poll()
    {
        if (!attached) return;
        try
        {
            var game = FindGameProcess();
            if (game == null)
            {
                // игра закрылась
                if (Core.Server.IsRunning) Core.GameClose();
                return;
            }

            IntPtr hWnd = game.MainWindowHandle;
            if (hWnd == IntPtr.Zero) hWnd = FindWindow(null, "Counter-Strike 2");
            if (hWnd == IntPtr.Zero) return;

            bool minimized = IsIconic(hWnd);
            if (minimized == lastMinimized) return;
            lastMinimized = minimized;

            if (minimized) Core.GameMinimaze();
            else Core.GameMaximaze();
        }
        catch (Exception ex)
        {
            CoreEvents.WriteLog("[GameMonitor] " + ex.Message);
        }
    }
}
