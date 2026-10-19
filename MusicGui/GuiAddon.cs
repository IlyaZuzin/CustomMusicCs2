// ---------------------------------------------------------------------------------------------
// GuiAddon.cs — точка входа аддона. Ядро вызывает:
//   1) MusicGui.GuiAddon.Create() (static, через рефлексию)
//   2) OnLoad(api) — здесь подписываемся на CoreEvents и запускаем WinForms-окно
//      отдельным потоком (STA), потому что ядро живёт в чужом процессе/потоке.
//   3) OnUnload() — закрываем окно, отписываемся от событий, удаляем Form из ALC.
// Окно MainGui полностью перенесено из legacy WinForms-приложения Main.cs, но теперь оно
// НИЧЕГО не знает про сервер/аудио напрямую — только через ICoreApi и CoreEvents.
// ---------------------------------------------------------------------------------------------
using MusicCore.Api;

namespace MusicGui;

public sealed class GuiAddon : ICoreAddon
{
    public static GuiAddon Instance { get; private set; }
    public ICoreApi Api { get; private set; } = null!;

    public string Name => "MusicGui";

    private MainGui? window;
    private Thread? uiThread;
    private ConsoleLogGui? consoleWindow;

    // Фабрика для ядра (вызывается рефлексией)
    public static ICoreAddon Create() => Instance ??= new GuiAddon();

    public void OnLoad(ICoreApi api)
    {
        Api = api;
        api.Log("[MusicGui] OnLoad");

        // Подписка на события ядра (см. документацию CORE_API.md).
        // Все подписки сохраняются в поля — так их можно корректно снять в OnUnload.
        hRoundWon      = (s, e) => { wonRounds++;  SafeUi(() => window?.RefreshStats(Stats)); };
        hRoundLost     = (s, e) => { lostRounds++; SafeUi(() => window?.RefreshStats(Stats)); };
        hMvp           = (s, e) => { mvps++;       SafeUi(() => window?.RefreshStats(Stats)); };
        hPlayerKilled  = (s, e) => { kills++;      SafeUi(() => window?.RefreshStats(Stats)); };
        hPlayerDied    = (s, e) => { deaths++;     SafeUi(() => window?.RefreshStats(Stats)); };
        hGameMinimized = (s, e) => SafeUi(() => { if (window != null) window.WindowState = FormWindowState.Minimized; });
        hGameMaximized = (s, e) => SafeUi(() => { if (window != null) window.WindowState = FormWindowState.Normal; });
        hGameClosed    = (s, e) => SafeUi(() => window?.SetServerLabel(false));

        CoreEvents.ApplicationStarted += OnAppStarted;
        CoreEvents.GameStateUpdated   += OnGameState;
        CoreEvents.RoundWon           += hRoundWon;
        CoreEvents.RoundLost          += hRoundLost;
        CoreEvents.MvpReceived        += hMvp;
        CoreEvents.PlayerKilled       += hPlayerKilled;
        CoreEvents.PlayerDied         += hPlayerDied;
        CoreEvents.MusicPlayed        += OnMusicPlayed;
        CoreEvents.TeamChanged        += OnTeamChanged;
        CoreEvents.GameMinimized      += hGameMinimized;
        CoreEvents.GameMaximized      += hGameMaximized;
        CoreEvents.GameClosed         += hGameClosed;
        CoreEvents.AddonLoaded        += OnAddonChanged;
        CoreEvents.AddonUnloaded      += OnAddonChanged;
        CoreEvents.Log                += OnLogLine;

        // WinForms крутим в отдельном STA-потоке
        uiThread = new Thread(RunUi) { IsBackground = true, Name = "MusicGui-UI" };
        uiThread.SetApartmentState(ApartmentState.STA);
        uiThread.Start();
    }

    private void RunUi()
    {
        try
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            window = new MainGui(this);
            Application.Run(window);
        }
        catch (Exception ex)
        {
            Api?.Log("[MusicGui] UI thread crashed: " + ex.Message, LogLevel.Error);
        }
    }

    public void OnUnload()
    {
        Api?.Log("[MusicGui] OnUnload");

        CoreEvents.ApplicationStarted -= OnAppStarted;
        CoreEvents.GameStateUpdated   -= OnGameState;
        CoreEvents.RoundWon           -= hRoundWon;
        CoreEvents.RoundLost          -= hRoundLost;
        CoreEvents.MvpReceived        -= hMvp;
        CoreEvents.PlayerKilled       -= hPlayerKilled;
        CoreEvents.PlayerDied         -= hPlayerDied;
        CoreEvents.MusicPlayed        -= OnMusicPlayed;
        CoreEvents.TeamChanged        -= OnTeamChanged;
        CoreEvents.GameMinimized      -= hGameMinimized;
        CoreEvents.GameMaximized      -= hGameMaximized;
        CoreEvents.GameClosed         -= hGameClosed;
        CoreEvents.AddonLoaded        -= OnAddonChanged;
        CoreEvents.AddonUnloaded      -= OnAddonChanged;
        CoreEvents.Log                -= OnLogLine;

        try
        {
            consoleWindow?.Close();
            window?.Close();
        }
        catch { }
        try { uiThread?.Join(2000); } catch { }
        window = null;
        consoleWindow = null;
        uiThread = null;
    }

    // ---- Поля-делегаты для корректной отписки ----
    private EventHandler? hRoundWon, hRoundLost, hMvp, hPlayerKilled, hPlayerDied;
    private EventHandler? hGameMinimized, hGameMaximized, hGameClosed;

    // ---- Обработчики ----
    private void OnAppStarted(object? s, EventArgs e) =>
        SafeUi(() => window?.SetServerLabel(Api.IsServerRunning));

    private void OnGameState(object? s, MusicCore.GameStateUpdatedEventArgs e) =>
        SafeUi(() => window?.UpdateModeLabel(e.GameMode.ToString(), e.Stats.player?.name ?? ""));

    private void OnMusicPlayed(object? s, MusicCore.MusicTrackEventArgs e) =>
        SafeUi(() => window?.UpdateNowPlaying(e.Event.ToString()));

    private void OnTeamChanged(object? s, MusicCore.TeamChangedEventArgs e) =>
        SafeUi(() => window?.UpdateTeamLabel(e.Team));

    private void OnAddonChanged(object? s, MusicCore.AddonLifecycleEventArgs e) =>
        SafeUi(() => window?.RefreshAddonsList());

    private void OnLogLine(object? s, LogEventArgs e)
    {
        consoleWindow?.Append(e.Message, e.Time);
    }

    // Счётчики статистики (аналог legacy Kills/Deaths/MVP в Main.cs)
    private uint wonRounds, lostRounds, mvps, kills, deaths;
    public (uint won, uint lost, uint mvp, uint kill, uint death, double kd) Stats =>
        (wonRounds, lostRounds, mvps, kills, deaths, deaths == 0 ? kills : (double)kills / deaths);

    internal void SafeUi(Action a)
    {
        var w = window;
        if (w == null || w.IsDisposed) return;
        try
        {
            if (w.InvokeRequired) w.BeginInvoke(a);
            else a();
        }
        catch { /* окно могло закрываться во время вызова */ }
    }

    internal void ShowConsole()
    {
        if (consoleWindow == null || consoleWindow.IsDisposed)
            consoleWindow = new ConsoleLogGui(this);
        consoleWindow.Show();
    }
}
