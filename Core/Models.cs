// ---------------------------------------------------------------------------------------------
// Models.cs — общие модели ядра. Сюда входят: перечисление событий музыки (13 штук),
// игровые режимы, DTO для десериализации gamestate JSON из CS2, модель музыкального набора
// (Kit_Sound) и модель профиля. Всё это используют и ядро, и аддоны.
// ---------------------------------------------------------------------------------------------
using System.Text.Json.Serialization;

namespace MusicCore;

/// <summary>Индексы событий (порядок совпадает с legacy-приложением, массив на 13 элементов).</summary>
public enum MusicIvents
{
    Menu = 0,
    WinRound = 1,
    LoseRound = 2,
    Bomb = 3,
    TenSecondBomb = 4,
    Mvp = 5,
    DeathCam = 6,
    StartGame = 7,
    StartRound = 8,
    StartAction = 9,
    EndGame = 10,
    TenSecondRound = 11,
    KillSound = 12,
}

/// <summary>Количество событий в наборе (размер массива дорожек).</summary>
public static class EventInfo
{
    public const int Count = 13;
}

/// <summary>Режимы игры, определяемые по полю map.mode / player.activity из gamestate.</summary>
public enum Mode
{
    Menu,
    Competitive,
    Public,
    Deathmatch,
    Wingmans,
    Null
}

/// <summary>Режим вызова воспроизведения (унаследован из legacy WavPlayer.IfEnable).</summary>
public enum ModeMusic
{
    /// <summary>Остановить текущее и играть, только если трек существует и громкость != 0.</summary>
    IfPlayingStop,
    /// <summary>Просто играть поверх (не останавливая текущий трек).</summary>
    Play,
    /// <summary>Остановить текущий трек и играть новый.</summary>
    Stop
}

// ---------------------------------------------------------------------------
// DTO для gamestate JSON (CS2 gamestate_integration)
// ---------------------------------------------------------------------------

/// <summary>Корень JSON, который присылает игра на http://127.0.0.1:3000/.</summary>
public class PlayerStats
{
    [JsonPropertyName("provider")] public Provider? client { get; set; }
    [JsonPropertyName("round")]     public RoundState? round { get; set; }
    [JsonPropertyName("player")]    public PlayerInfo? player { get; set; }
    [JsonPropertyName("map")]       public MapInfo? map { get; set; }

    public class Provider
    {
        [JsonPropertyName("steamid")]    public long steamid { get; set; }
        [JsonPropertyName("timestamp")]  public long time { get; set; }
    }

    public class MapInfo
    {
        public string? mode { get; set; }
        public string? name { get; set; }
        public string? phase { get; set; }
        public int round { get; set; }
        [JsonPropertyName("team_ct")] public Team? team_ct { get; set; }
        [JsonPropertyName("team_t")]  public Team? team_t { get; set; }
    }

    public class Team
    {
        public int score { get; set; }
        public int round_losses { get; set; }
        public int timeouts_remaining { get; set; }
        public int matches_won_series { get; set; }
    }

    public class RoundState
    {
        [JsonPropertyName("phase")]    public string? phase { get; set; }
        [JsonPropertyName("win_team")] public string? win_team { get; set; }
        [JsonPropertyName("bomb")]     public string? bomb { get; set; }
    }

    public class MatchStats
    {
        [JsonPropertyName("mvps")] public int mvp { get; set; }
    }

    public class PlayerInfo
    {
        public long steamid { get; set; }
        public string? name { get; set; }
        public int slot { get; set; }
        public string? team { get; set; }
        [JsonPropertyName("activity")]    public string? activity { get; set; }
        [JsonPropertyName("state")]       public PlayerState? state { get; set; }
        [JsonPropertyName("match_stats")] public MatchStats? MVP { get; set; }
    }

    public class PlayerState
    {
        public int health { get; set; }
        public int armor { get; set; }
        public bool helmet { get; set; }
        public int flashed { get; set; }
        public int smoked { get; set; }
        public int burning { get; set; }
        public int money { get; set; }
        [JsonPropertyName("round_kills")] public int round_kills { get; set; }
        public int round_killshs { get; set; }
        public int equip_value { get; set; }
    }

    public object Clone() => MemberwiseClone();
}

// ---------------------------------------------------------------------------
// Модели музыкальных наборов и профилей
// ---------------------------------------------------------------------------

/// <summary>Один аудиофайл внутри события набора.</summary>
public class AudioFiles
{
    public string? path { get; set; }
    /// <summary>Индекс трека события StartAction, подставляемого после этого трека.</summary>
    public int? next_music { get; set; }
    public int volume { get; set; } = 100;
}

/// <summary>Набор музыки (profile.json внутри папки кита).</summary>
public class Kit_Sound
{
    [JsonPropertyName("name")]  public string? name { get; set; }
    [JsonPropertyName("image")] public string? image { get; set; }
    /// <summary>Массив из 13 списков — по одному на каждое событие MusicIvents.</summary>
    public List<AudioFiles>[]? music { get; set; } = new List<AudioFiles>[EventInfo.Count];
}

/// <summary>Пары путей «набор для CT / набор для T» внутри профиля.</summary>
public class TeamPath
{
    public string? T { get; set; }
    public string? Ct { get; set; }
}

/// <summary>Профиль пользователя (набор CT + набор T + режим).</summary>
public class Profile
{
    public string name { get; set; } = "Unnamed";
    public string? ct_path { get; set; }
    public string? t_path { get; set; }
    public bool Standart_mode { get; set; } = false;
    public string? global_path { get; set; }

    public Profile Copy() => (Profile)MemberwiseClone();
}
