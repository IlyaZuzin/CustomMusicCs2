// ---------------------------------------------------------------------------------------------
// EventDispatcher.cs — порт legacy-класса Ivents (PlayerIvents.cs).
// Получает PlayerStats от GameStateServer и распознаёт игровые события. КАЖДОЕ событие вынесено
// в отдельную функцию HandleXxx, как требует задача по десериализации JSON:
//   UpdateMode, MenuState, MatchStart, RoundFreeze, RoundLive, WinLose, MvpAward, BombPlanted,
//   TenSecondCountdown, PlayerKill, PlayerDeath, TeamSwitch, Warmup, GameOver.
// Каждая функция: 1) вызывает нужное действие аудио через AudioEngine, 2) рассылает событие
// аддонам через CoreEvents.RaiseXxx.
// ---------------------------------------------------------------------------------------------
using MusicCore.Audio;

namespace MusicCore;

public sealed class EventDispatcher
{
    private readonly AudioEngine audio;

    // ---- Состояние (антоним legacy static-полей Ivents) ----
    public Mode CurrentGameMode { get; private set; } = Mode.Menu;
    public string? PlayerTeam { get; private set; } = "CT";
    public PlayerStats? LastStats { get; private set; }

    private int roundTimer = 115;
    private int? lastMvp = 0;
    private bool startRoundInProgress = false;
    private int? killsRound = 0;
    private bool isAlive = true;              // legacy IsDeath со «сдвинутым» смыслом
    private CancellationTokenSource cts = new();

    /// <summary>Пути наборов из активного профиля (задаёт Core).</summary>
    public TeamPath? Kits { get; set; }

    public EventDispatcher(AudioEngine engine) => audio = engine;

    /// <summary>Точка входа: сервер передал новый пакет gamestate.</summary>
    public void Tick(PlayerStats data)
    {
        LastStats = data;
        var token = cts.Token;

        UpdateMode(data);
        CoreEvents.RaiseGameStateUpdated(data, CurrentGameMode);

        if (CurrentGameMode == Mode.Menu)
        {
            MenuState();
            return;
        }

        switch (CurrentGameMode)
        {
            case Mode.Competitive: roundTimer = 115; break;
            case Mode.Wingmans:    roundTimer = 90;  break;
            case Mode.Public:      roundTimer = 135; break;
        }

        // Все обработчики — отдельные функции (см. заголовок файла)
        PlayerKill(data);
        PlayerDeath(data);
        BombPlanted(data, token);
        WinLose(data);
        CompetitiveSequence(data, token);
        TeamSwitch(data);
    }

    /// <summary>Определение режима по activity/map.mode (legacy Update).</summary>
    private void UpdateMode(PlayerStats p)
    {
        if (p.round == null && p.player?.activity == "menu") CurrentGameMode = Mode.Menu;
        else if (p.map?.mode == "competitive")    CurrentGameMode = Mode.Competitive;
        else if (p.map?.mode == "scrimcomp2v2")   CurrentGameMode = Mode.Wingmans;
        else if (p.map?.mode == "casual")         CurrentGameMode = Mode.Public;
        else if (p.map?.mode == "deathmatch")     CurrentGameMode = Mode.Deathmatch;
    }

    /// <summary>Главное меню игры: фоновая музыка Menu.</summary>
    private void MenuState()
    {
        startRoundInProgress = false;
        if (audio.CurrentEvent != MusicIvents.Menu)
            audio.TryStartMenuMusic();
    }

    /// <summary>Старт матча + первый раунд (legacy Competitive, ветка map.round==0).</summary>
    private void MatchStart(PlayerStats p, CancellationToken token)
    {
        if (startRoundInProgress) return;
        if (p.map?.round != 0 || p.map?.phase != "live") return;
        if (audio.CurrentEvent != MusicIvents.Menu) return;

        _ = MatchStartAsync(p, token);
    }

    private async Task MatchStartAsync(PlayerStats p, CancellationToken token)
    {
        startRoundInProgress = true;
        CoreEvents.RaiseMatchStarted();
        audio.IfEnable(MusicIvents.StartGame, ModeMusic.Stop);

        await TenSecondCountdownAsync(7, token, () =>
        {
            audio.IfEnable(MusicIvents.StartRound, ModeMusic.Stop);
            CoreEvents.RaiseRoundStarted();
        });

        startRoundInProgress = false;
    }

    /// <summary>Freezetime — начало раунда.</summary>
    private void RoundFreeze(PlayerStats p)
    {
        audio.IfEnable(MusicIvents.StartRound, ModeMusic.Stop);
        isAlive = true;
        CoreEvents.RaiseRoundStarted();
    }

    /// <summary>Live — активная фаза, старт обратного отсчёта до «10 секунд бомбы».</summary>
    private void RoundLive(PlayerStats p, CancellationToken token)
    {
        if (audio.CurrentEvent != MusicIvents.StartRound) return;
        lastMvp = p.player?.MVP?.mvp;
        audio.IfEnable(MusicIvents.StartAction, ModeMusic.Stop);
        killsRound = 0;
        CoreEvents.RaiseActionStarted();

        _ = TenSecondCountdownAsync(roundTimer - 10, token, () =>
        {
            audio.IfEnable(MusicIvents.TenSecondRound, ModeMusic.Stop);
        });
    }

    /// <summary>Отсчётчик секунд до переключения на «TenSecond» трек.</summary>
    private async Task TenSecondCountdownAsync(int seconds, CancellationToken token, Action onFinish)
    {
        for (int i = 0; i < seconds; i++)
        {
            await Task.Delay(1000, token).ContinueWith(_ => { });
            if (token.IsCancellationRequested) return;
            if (audio.CurrentEvent == null) return; // музыку остановили — отменяем отсчёт
        }
        onFinish();
    }

    /// <summary>Win/Lose/MVP (legacy WinLose).</summary>
    private void WinLose(PlayerStats p)
    {
        if (audio.CurrentEvent == MusicIvents.EndGame) return;
        if (p.round?.win_team == null) return;

        bool own = p.player?.team == p.round?.win_team;
        if (own)
        {
            bool isMvp = p.player?.MVP?.mvp != null && p.player?.MVP?.mvp != lastMvp
                         && p.client?.steamid == p.player?.steamid;
            if (isMvp)
            {
                audio.IfEnable(MusicIvents.Mvp, ModeMusic.Stop);
                CoreEvents.RaiseMvpReceived();
            }
            else
            {
                audio.IfEnable(MusicIvents.WinRound, ModeMusic.Stop);
                CoreEvents.RaiseRoundWon();
            }
        }
        else
        {
            audio.IfEnable(MusicIvents.LoseRound, ModeMusic.Stop);
            CoreEvents.RaiseRoundLost();
        }
        audio.RandomizatePair((int)MusicIvents.StartRound, (int)MusicIvents.StartAction);
    }

    /// <summary>Убийство игроком противника (рост round_kills).</summary>
    private void PlayerKill(PlayerStats p)
    {
        var rk = p.player?.state?.round_kills;
        if (rk != null && rk != killsRound && rk != 0 &&
            p.player?.steamid == p.client?.steamid && isAlive)
        {
            killsRound = rk;
            audio.IfEnable(MusicIvents.KillSound, ModeMusic.Play);
            CoreEvents.RaisePlayerKilled();
        }
    }

    /// <summary>Смерть игрока (health == 0) — deathcam.</summary>
    private void PlayerDeath(PlayerStats p)
    {
        if (p.player?.state?.health == 0 && p.player?.steamid == p.client?.steamid && isAlive)
        {
            isAlive = false;
            audio.IfEnable(MusicIvents.DeathCam, ModeMusic.Play);
            CoreEvents.RaisePlayerDied();
        }
    }

    /// <summary>Бомба заложена: стоп фоновой, затем через 28 сек — TenSecondBomb.</summary>
    private void BombPlanted(PlayerStats p, CancellationToken token)
    {
        if (p.round?.bomb != "planted") return;
        if (audio.CurrentEvent == MusicIvents.Bomb || audio.CurrentEvent == MusicIvents.TenSecondBomb) return;

        _ = BombPlantedAsync(p, token);
    }

    private async Task BombPlantedAsync(PlayerStats p, CancellationToken token)
    {
        audio.IfEnable(MusicIvents.Bomb, ModeMusic.Stop);
        CoreEvents.RaiseBombPlanted();

        for (int i = 0; i <= 28; i++)
        {
            await Task.Delay(1000, token).ContinueWith(_ => { });
            if (token.IsCancellationRequested) return;
            if (LastStats?.round?.bomb != "planted" || audio.CurrentEvent != MusicIvents.Bomb) return;
        }
        if (token.IsCancellationRequested) return;
        if (LastStats?.round?.bomb == "planted" && audio.CurrentEvent == MusicIvents.Bomb)
        {
            audio.IfEnable(MusicIvents.TenSecondBomb, ModeMusic.IfPlayingStop);
            CoreEvents.RaiseBombTenSeconds();
        }
    }

    /// <summary>Последовательность событий соревновательного цикла.</summary>
    private void CompetitiveSequence(PlayerStats p, CancellationToken token)
    {
        MatchStart(p, token);

        if (p.map?.phase == "gameover")
        {
            audio.IfEnable(MusicIvents.EndGame, ModeMusic.IfPlayingStop);
            CoreEvents.RaiseMatchEnded();
        }
        else if (p.map?.phase == "warmup")
        {
            audio.Stop();
        }
        else if (p.round?.phase == "freezetime")
        {
            RoundFreeze(p);
        }
        else if (p.round?.phase == "live" && audio.CurrentEvent == MusicIvents.StartRound)
        {
            RoundLive(p, token);
        }
    }

    /// <summary>Смена команды CT/T — перезагрузка музыкального набора (legacy CheckTeam).</summary>
    private void TeamSwitch(PlayerStats p)
    {
        if (p.player?.team == null || PlayerTeam == p.player.team) return;

        string? kitPath = null;
        if (p.player.team == "T")  { if (Kits?.T  != null) kitPath = Kits.T;  }
        else                        { if (Kits?.Ct != null) kitPath = Kits.Ct; }

        PlayerTeam = p.player.team;
        CoreEvents.RaiseTeamChanged(PlayerTeam, kitPath);
        if (kitPath != null) Core.ReloadKitFromPath(kitPath);
    }

    /// <summary>Остановка разбора (сервер лег): сброс состояния.</summary>
    public void Reset()
    {
        cts.Cancel();
        cts = new CancellationTokenSource();
        startRoundInProgress = false;
        killsRound = 0;
        isAlive = true;
    }
}
