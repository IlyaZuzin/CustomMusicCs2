using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace MusicComanderGUI
{
    public enum MusicIvents
    {
        Menu,
        WinRound,
        LoseRound,
        Bomb,
        TenSecondBomb,
        Mvp,
        DeathCam,
        StartGame,
        StartRound,
        StartAction,
        EndGame,
        TenSecondRound,
        KillSound,
    }

    public enum Mode
    {
        Menu,
        Competitive,
        Public,
        Deathmatch,
        Wingmans,
        Null
    }

    class Music
    {
        public string? name { get; set; }
        public string? image { get; set; }
        public string? path { get; set; }
    }

    public class AudioFiles
    {
        public string? path { get; set; }
        public int? next_music { get; set; }
        public int volume { get; set; } = 100;
    }

    public class TeamPath
    {
        public string? T { get; set; }
        public string? Ct { get; set; }
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
}
