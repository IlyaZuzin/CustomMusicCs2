// ---------------------------------------------------------------------------------------------
// Core.cs — фасад ядра. Здесь живут 9 «экспортируемых» функций (см. NativeExports.cs) и вся
// композиция: AudioEngine + GameStateServer + EventDispatcher + AddonManager + профиль.
// Также здесь реализован CoreApi — экземпляр ICoreApi, передаваемый каждому аддону.
// Все Raise* - методы CoreEvents internal, поэтому вызовы из этого файла легальны.
// ---------------------------------------------------------------------------------------------
using MusicCore.Addons;
using MusicCore.Api;
using MusicCore.Audio;

namespace MusicCore;

public static class Core
{
    // Ядро НЕ использует WinForms (нет UseWindowsForms в Core.csproj), поэтому таймеры —
    // System.Threading.Timer, а не Forms.Timer.
    internal static AudioEngine Audio { get; private set; } = new();
    internal static GameStateServer Server { get; private set; } = new();
    internal static EventDispatcher Dispatcher { get; private set; } = null!;
    internal static AddonManager Addons { get; private set; } = null!;
    internal static CoreApi Api { get; private set; } = new();

    public static Profile? ActiveProfile { get; private set; }
    public static string? PlayingKitPath { get; private set; }

    /// <summary>Сменить активный профиль и перезагрузить его набор.</summary>
    public static bool SwitchProfile(string profileName)
    {
        var prof = MusicKitsStorage.settings.profiles?.FirstOrDefault(p =>
            string.Equals(p.name, profileName, StringComparison.OrdinalIgnoreCase));
        if (prof == null) return false;

        int idx = MusicKitsStorage.settings.profiles.IndexOf(prof);
        MusicKitsStorage.settings.profile = idx;
        ActiveProfile = prof;
        Dispatcher.Kits = BuildTeamPath();
        ReloadKitFromPath(prof.Standart_mode ? (prof.global_path ?? prof.ct_path) : prof.ct_path);
        CoreEvents.WriteLog($"[Core] Profile switched: {profileName}");
        return true;
    }

    private static bool initialized = false;
    private static readonly object gate = new();

    // ============================ 1. Setup ============================
    // Инициализация ядра: настройки, профили, аудио-набор, подписки сервера.
    public static void Setup()
    {
        lock (gate)
        {
            if (initialized) return;
            initialized = true;

            MusicKitsStorage.LoadSetting();

            // Активный профиль из setting.json (или первый/дефолтный)
            int? idx = MusicKitsStorage.settings.profile;
            var profiles = MusicKitsStorage.settings.profiles;
            if (idx != null && profiles != null && idx.Value >= 0 && idx.Value < profiles.Count)
                ActiveProfile = profiles[idx.Value];
            else if (profiles != null && profiles.Count > 0)
                ActiveProfile = profiles[0];
            else
                ActiveProfile = new Profile { name = "Default" };

            Dispatcher = new EventDispatcher(Audio);
            Addons = new AddonManager(Api);

            // Связка сервер -> диспетчер
            Server.PacketReceived += stats => Dispatcher.Tick(stats);
            Server.RawPacketReceived += raw =>
            {
                if (Logging.LoggingOn) Logging.WriteLine("[RawJSON] " + raw);
            };

            // Загружаем набор активного профиля (CT как основной) и сразу стартуем меню-музыку
            ReloadKitFromPath(ActiveProfile.ct_path ?? ActiveProfile.global_path);
            Dispatcher.Kits = BuildTeamPath();

            CoreEvents.WriteLog("[Core] Setup complete");
            CoreEvents.RaiseApplicationStarted();
        }
    }

    // ============================ 2. Inject ============================
    // «Внедрение» ядра в игру: запуск gamestate-сервера и всех аддонов из папки addons.
    public static void Inject()
    {
        EnsureSetup();
        lock (gate)
        {
            bool ok = Server.Start();
            if (ok)
            {
                GameWindowMonitor.Attach();          // следит за сворачиванием/закрытием игры
                CoreEvents.RaiseGameOpened();
                CoreEvents.WriteLog("[Core] Inject: server up, game monitor attached");
            }
            // Автозагрузка аддонов: сначала по списку addons.autoload.txt, иначе все *.dll из addons/
            Addons.AutoLoad();
        }
    }

    // ============================ 3. GameOpen ============================
    // Игра открыта (запущена): возобновляем приём gamestate и фоновую музыку.
    public static void GameOpen()
    {
        EnsureSetup();
        lock (gate)
        {
            if (!Server.IsRunning) Server.Start();
            Audio.TryStartMenuMusic();
            CoreEvents.RaiseGameOpened();
            CoreEvents.WriteLog("[Core] GameOpen");
        }
    }

    // ============================ 4. GameMinimaze ============================
    // Игра свёрнута: ставим музыку на паузу (stop), чтобы не мешать и не жечь CPU.
    public static void GameMinimaze()
    {
        EnsureSetup();
        Audio.Stop();
        CoreEvents.RaiseGameMinimized();
        CoreEvents.WriteLog("[Core] GameMinimaze: music paused");
    }

    // ============================ 5. GameMaximaze ============================
    // Игра восстановлена: возвращаем фоновую музыку меню.
    public static void GameMaximaze()
    {
        EnsureSetup();
        Audio.TryStartMenuMusic();
        CoreEvents.RaiseGameMaximized();
        CoreEvents.WriteLog("[Core] GameMaximaze: menu music resumed");
    }

    // ============================ 6. GameClose ============================
    // Игра закрыта: останавливаем сервер и диспетчер, глушим звук.
    public static void GameClose()
    {
        EnsureSetup();
        lock (gate)
        {
            GameWindowMonitor.Detach();
            Server.Stop();
            Audio.StopAll();
            Dispatcher.Reset();
            CoreEvents.RaiseGameClosed();
            CoreEvents.WriteLog("[Core] GameClose");
        }
    }

    // ============================ 7. Close ============================
    // Полное завершение работы ядра: выгрузка аддонов, сохранение настроек, освобождение всего.
    public static void Close()
    {
        lock (gate)
        {
            if (!initialized) return;
            CoreEvents.RaiseApplicationClosing();

            try { Addons?.UnloadAll(); } catch { }
            try { GameWindowMonitor.Detach(); } catch { }
            try { Server.Stop(); } catch { }
            try { Audio.StopAll(); } catch { }
            try { MusicKitsStorage.SaveSetting(); } catch { }

            CoreEvents.WriteLog("[Core] Closed");
            initialized = false;
        }
    }

    // ==================== 8/9. LoadAddon / UnloadAddon ====================
    // Аргумент — имя аддона ("MusicGui") или путь к DLL ("addons/MusicGui.dll").
    // Физически аддоны лежат в подпапке "addons" рядом с ядром.
    public static void LoadAddon(string nameOrPath)
    {
        EnsureSetup();
        Addons.Load(nameOrPath);
    }

    public static void UnloadAddon(string nameOrPath)
    {
        EnsureSetup();
        Addons.Unload(nameOrPath);
    }

    // =====================================================================
    // Внутреннее
    // =====================================================================
    private static void EnsureSetup()
    {
        if (!initialized) Setup();
    }

    internal static TeamPath BuildTeamPath()
    {
        var p = ActiveProfile;
        return new TeamPath
        {
            Ct = p?.Standart_mode ? (p.global_path ?? p.ct_path) : p?.ct_path,
            T  = p?.Standart_mode ? (p.global_path ?? p.t_path)  : p?.t_path,
        };
    }

    internal static void ReloadKitFromPath(string? path)
    {
        if (string.IsNullOrEmpty(path)) return;
        var kit = MusicKitsStorage.LoadKit(path);
        if (kit == null) { CoreEvents.WriteLog($"[Core] Kit not loaded: {path}"); return; }
        PlayingKitPath = path;
        if (Server.IsRunning) Audio.ReloadKit(kit);
        else Audio.Setup(kit);
        CoreEvents.WriteLog($"[Core] Kit loaded: {kit.name} ({path})");
    }

    internal static bool LoggingEnabled
    {
        get => Logging.LoggingOn;
        set => Logging.LoggingOn = value;
    }
}

/// <summary>Реализация ICoreApi для аддонов — тонкая обёртка над внутренностями ядра.</summary>
internal sealed class CoreApi : ICoreApi
{
    public void PlayMusic(MusicIvents ev) => Core.Audio.Play(ev);
    public void StopMusic() => Core.Audio.Stop();
    public void StopMusic(MusicIvents ev) => Core.Audio.Stop(ev);
    public void IfEnable(MusicIvents ev, ModeMusic mode) => Core.Audio.IfEnable(ev, mode);
    public void SetVolume(MusicIvents ev, float volume01) => Core.Audio.SetVolume(ev, volume01);
    public float GetVolume(MusicIvents ev) => Core.Audio.GetVolume(ev);
    public MusicIvents? CurrentEvent => Core.Audio.CurrentEvent;
    public bool HasTrack(MusicIvents ev) => Core.Audio.HasTrack(ev);

    public bool LoadProfile(string profileName) => Core.SwitchProfile(profileName);

    public IReadOnlyList<string> Profiles =>
        MusicKitsStorage.settings.profiles?.Select(p => p.name).ToList() ?? new List<string>();

    public Profile? ActiveProfile => Core.ActiveProfile;

    public bool ReloadKit(string kitPath)
    {
        if (string.IsNullOrEmpty(kitPath) || !Directory.Exists(kitPath)) return false;
        Core.ReloadKitFromPath(kitPath);
        return true;
    }

    public bool ImportKit(string kitFolder) => MusicKitsStorage.ImportKit(kitFolder);
    public void SaveSettings() => MusicKitsStorage.SaveSetting();

    public bool IsServerRunning => Core.Server.IsRunning;
    public Mode CurrentMode => Core.Dispatcher?.GameMode ?? Mode.Null;
    public PlayerStats? LastStats => Core.Dispatcher?.LastStats;
    public string? PlayerTeam => Core.Dispatcher?.PlayerTeam;

    public void StartServer() => Core.Server.Start();
    public void StopServer() => Core.Server.Stop();

    public void Log(string message, LogLevel level = LogLevel.Info) =>
        CoreEvents.WriteLog($"[{level}] {message}");

    public bool LoggingEnabled { get => Core.LoggingEnabled; set => Core.LoggingEnabled = value; }

    public IReadOnlyList<string> LoadedAddons => Core.Addons?.LoadedNames ?? new List<string>();
}

/// <summary>Простое логирование ядра (текстовый файл + событие Log для GUI-аддонов).</summary>
internal static class Logging
{
    public static bool LoggingOn = false;
    private static readonly object fileGate = new();
    private static readonly string LogFile = Path.Combine(AppContext.BaseDirectory, "core.log");

    public static void WriteLine(string message)
    {
        CoreEvents.WriteLog(message);
        if (!LoggingOn) return;
        try
        {
            lock (fileGate) File.AppendAllText(LogFile, $"[{DateTime.Now:HH:mm:ss.fff}] {message}{Environment.NewLine}");
        }
        catch { }
    }
}
