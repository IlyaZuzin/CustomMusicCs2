// ---------------------------------------------------------------------------------------------
// CoreEvents.cs — статический «информационный центр» ядра. Каждое игровое событие, которое
// ядро распознаёт при разборе gamestate JSON, выливается в отдельную функцию RaiseXxx и в
// публичное C#-событие. Аддоны подписываются на эти события из своего OnLoad() и отписываются
// в OnUnload(). Это позволяет ядру ничего не знать о конкретных аддонах.
// ---------------------------------------------------------------------------------------------
using MusicCore.Api;

namespace MusicCore;

/// <summary>Данные смены команды (CT/T) — используются для перезагрузки набора.</summary>
public sealed class TeamChangedEventArgs : EventArgs
{
    public string Team { get; init; } = "";      // "CT" или "T"
    public string? KitPath { get; init; }        // путь к набору, который ядро уже собирается загрузить
}

/// <summary>Данные обновления состояния игры (каждый пакет gamestate).</summary>
public sealed class GameStateUpdatedEventArgs : EventArgs
{
    public PlayerStats Stats { get; init; } = new();
    public Mode GameMode { get; init; }
}

/// <summary>Данные проигрываемого/останавливаемого трека.</summary>
public sealed class MusicTrackEventArgs : EventArgs
{
    public MusicIvents Event { get; init; }
    public string? Path { get; init; }
}

/// <summary>Данные изменения громкости события.</summary>
public sealed class VolumeChangedEventArgs : EventArgs
{
    public MusicIvents Event { get; init; }
    public float Volume { get; init; }           // 0..1
}

/// <summary>Данные жизненного цикла аддона.</summary>
public sealed class AddonLifecycleEventArgs : EventArgs
{
    public string Name { get; init; } = "";
    public string Path { get; init; } = "";
}

/// <summary>
/// Центр событий ядра. Все функции Raise* вызываются только изнутри ядра (из
/// EventDispatcher / AudioEngine / AddonManager), а аддоны лишь подписываются на них.
/// </summary>
public static class CoreEvents
{
    // ------------------------------------------------------------------
    // Публичные события для аддонов
    // ------------------------------------------------------------------
    public static event EventHandler? ApplicationStarted;          // Setup выполнен, всё готово
    public static event EventHandler? ApplicationClosing;          // Close вызван, приложение сворачивается
    public static event EventHandler? GameOpened;                  // игра запущена / сервер слушает gamestate
    public static event EventHandler? GameClosed;                  // игра закрыта / сервер остановлен
    public static event EventHandler? GameMinimized;               // окно игры свёрнуто
    public static event EventHandler? GameMaximized;               // окно игры восстановлено
    public static event GameStateUpdatedEventHandler? GameStateUpdated; // пришёл новый пакет gamestate
    public static event EventHandler<TeamChangedEventArgs>? TeamChanged;
    public static event EventHandler? RoundStarted;                // freezetime — начался раунд
    public static event EventHandler? ActionStarted;               // live — активная фаза раунда
    public static event EventHandler? RoundWon;                    // раунд выигран
    public static event EventHandler? RoundLost;                   // раунд проигран
    public static event EventHandler? MvpReceived;                 // игрок получил MVP
    public static event EventHandler? PlayerKilled;                // игрок убил противника
    public static event EventHandler? PlayerDied;                  // игрок умер (deathcam)
    public static event EventHandler? BombPlanted;                 // бомба заложена
    public static event EventHandler? BombTenSeconds;              // до взрыва бомбы <= 10 секунд
    public static event EventHandler? MatchStarted;                // матч начался (первый живой раунд)
    public static event EventHandler? MatchEnded;                  // фаза gameover
    public static event EventHandler<MusicTrackEventArgs>? MusicPlayed;
    public static event EventHandler<MusicTrackEventArgs>? MusicStopped;
    public static event EventHandler<VolumeChangedEventArgs>? VolumeChanged;
    public static event EventHandler<AddonLifecycleEventArgs>? AddonLoaded;
    public static event EventHandler<AddonLifecycleEventArgs>? AddonUnloaded;
    public static event LogEventHandler? Log;                      // текстовые сообщения в лог

    public delegate void GameStateUpdatedEventHandler(object? sender, GameStateUpdatedEventArgs e);

    // ------------------------------------------------------------------
    // Методы-«диспетчеры»: по одному на каждое событие. Ядро вызывает их,
    // они безопасно (с try/catch по каждому подписчику) рассылают событие аддонам.
    // ------------------------------------------------------------------

    /// <summary>Рассылает событие всем подписчикам, изолируя исключения каждого аддона.</summary>
    private static void Broadcast<T>(EventHandler<T>? handler, T args, string name) where T : EventArgs
    {
        if (handler == null) return;
        foreach (var d in handler.GetInvocationList())
        {
            try { ((EventHandler<T>)d)(null, args); }
            catch (Exception ex) { WriteLog($"[Events] subscriber of '{name}' threw: {ex.Message}"); }
        }
    }

    private static void Broadcast(EventHandler? handler, string name)
    {
        if (handler == null) return;
        foreach (var d in handler.GetInvocationList())
        {
            try { ((EventHandler)d)(null, EventArgs.Empty); }
            catch (Exception ex) { WriteLog($"[Events] subscriber of '{name}' threw: {ex.Message}"); }
        }
    }

    // ---- Жизненный цикл приложения ----
    internal static void RaiseApplicationStarted() => Broadcast(ApplicationStarted, nameof(ApplicationStarted));
    internal static void RaiseApplicationClosing() => Broadcast(ApplicationClosing, nameof(ApplicationClosing));

    // ---- Состояние игры ----
    internal static void RaiseGameOpened()    => Broadcast(GameOpened, nameof(GameOpened));
    internal static void RaiseGameClosed()    => Broadcast(GameClosed, nameof(GameClosed));
    internal static void RaiseGameMinimized() => Broadcast(GameMinimized, nameof(GameMinimized));
    internal static void RaiseGameMaximized() => Broadcast(GameMaximized, nameof(GameMaximized));

    // ---- Пакеты gamestate ----
    internal static void RaiseGameStateUpdated(PlayerStats stats, Mode mode)
        => Broadcast(GameStateUpdated, new GameStateUpdatedEventArgs { Stats = stats, GameMode = mode }, nameof(GameStateUpdated));

    // ---- Игровые события ----
    internal static void RaiseTeamChanged(string team, string? kitPath)
        => Broadcast(TeamChanged, new TeamChangedEventArgs { Team = team, KitPath = kitPath }, nameof(TeamChanged));

    internal static void RaiseRoundStarted()   => Broadcast(RoundStarted, nameof(RoundStarted));
    internal static void RaiseActionStarted()  => Broadcast(ActionStarted, nameof(ActionStarted));
    internal static void RaiseRoundWon()       => Broadcast(RoundWon, nameof(RoundWon));
    internal static void RaiseRoundLost()      => Broadcast(RoundLost, nameof(RoundLost));
    internal static void RaiseMvpReceived()    => Broadcast(MvpReceived, nameof(MvpReceived));
    internal static void RaisePlayerKilled()   => Broadcast(PlayerKilled, nameof(PlayerKilled));
    internal static void RaisePlayerDied()     => Broadcast(PlayerDied, nameof(PlayerDied));
    internal static void RaiseBombPlanted()    => Broadcast(BombPlanted, nameof(BombPlanted));
    internal static void RaiseBombTenSeconds() => Broadcast(BombTenSeconds, nameof(BombTenSeconds));
    internal static void RaiseMatchStarted()   => Broadcast(MatchStarted, nameof(MatchStarted));
    internal static void RaiseMatchEnded()     => Broadcast(MatchEnded, nameof(MatchEnded));

    // ---- Аудио ----
    internal static void RaiseMusicPlayed(MusicIvents ev, string? path)
        => Broadcast(MusicPlayed, new MusicTrackEventArgs { Event = ev, Path = path }, nameof(MusicPlayed));

    internal static void RaiseMusicStopped(MusicIvents ev, string? path)
        => Broadcast(MusicStopped, new MusicTrackEventArgs { Event = ev, Path = path }, nameof(MusicStopped));

    internal static void RaiseVolumeChanged(MusicIvents ev, float vol)
        => Broadcast(VolumeChanged, new VolumeChangedEventArgs { Event = ev, Volume = vol }, nameof(VolumeChanged));

    // ---- Аддоны ----
    internal static void RaiseAddonLoaded(string name, string path)
        => Broadcast(AddonLoaded, new AddonLifecycleEventArgs { Name = name, Path = path }, nameof(AddonLoaded));

    internal static void RaiseAddonUnloaded(string name, string path)
        => Broadcast(AddonUnloaded, new AddonLifecycleEventArgs { Name = name, Path = path }, nameof(AddonUnloaded));

    // ---- Лог ----
    internal static void WriteLog(string message)
    {
        try { Log?.Invoke(null, new LogEventArgs { Message = message, Time = DateTime.Now }); }
        catch { /* лог не должен ронять ядро */ }
    }
}
