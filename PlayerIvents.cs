// ----------------------------------------------------------------------------------------------
// Данный файл определяет события, которые происходят во время игры и запускает необходимое аудио
// ----------------------------------------------------------------------------------------------

using System.Text.Json.Serialization;
using static PlayerStats;
namespace MusicComanderGUI
{
    public enum Mode
    { 
        Menu,
        Competitive,
        Public,
        Deathmatch,
        Wingmans,
        Null
    }

    static class Ivents 
    {
        public static MusicIvents? LastMusic;
        private static int Round_Timer = 115;
        private static int? Mvp = 0;
        public static string? Team = "Ct";
        private static bool StartRound = false;
        private static int? kills_r = 0;
        private static bool? IsCompetitive;
        private static bool IsKill;
        private static bool GameEnd;
        private static bool IsDeath = true;
        public static PlayerStats? player = null;
        private static PlayerStats? currect_player = null;
        private static Mode gamemode = Mode.Menu;
        private static TeamPath? teams;
        public static async Task Tick(PlayerStats NewData, CancellationToken cts)
        {
            player = NewData;
            currect_player = NewData;

            Update(cts);

            if (gamemode == Mode.Menu)
            {
                StartRound = false;
                WavPlayer.IfEnable(MusicIvents.Menu, ModeMusic.Stop);
            }
            else if (gamemode == Mode.Competitive)
            {
                Round_Timer = 115;
                Task.Run(() => PlayerIvents(cts));
                Task.Run(() => Bomb(cts));
                Task.Run(() => WinLose(cts));
                Task.Run(() => Competitive(cts));
                Task.Run(() => CheckTeam(cts));
            }
            else if (gamemode == Mode.Wingmans)
            {
                Round_Timer = 90;
                Task.Run(() => PlayerIvents(cts));
                Task.Run(() => Bomb(cts));
                Task.Run(() => WinLose(cts));
                Task.Run(() => Competitive(cts));
                Task.Run(() => CheckTeam(cts));
            }
            else if (gamemode == Mode.Public)
            {
                Round_Timer = 135;
                Task.Run(() => Competitive(cts));
                Task.Run(() => Bomb(cts));
                Task.Run(() => WinLose(cts));
                Task.Run(() => Competitive(cts));
                Task.Run(() => CheckTeam(cts));
            }
            else if (gamemode == Mode.Deathmatch)
            {
                Round_Timer = 600;
                Task.Run(() => PlayerIvents(cts));
                Task.Run(() => CheckTeam(cts));
            }
            Main.Instance?.SetConsoleLog($"Last Player state: {Enum.GetName(typeof(MusicIvents), (int)LastMusic)} \t Mode: {Enum.GetName(typeof(Mode), gamemode)} {Environment.NewLine}Map mode: {player?.map?.mode}");
        }

        private static async Task PlayerIvents(CancellationToken cts)
        {
            try
            {
                if (kills_r != player?.player?.state?.round_kills && player?.player?.steamid == player?.client?.steamid &&
                    player?.player?.state?.round_kills != null && player?.player?.state?.round_kills != 0)
                {
                    kills_r = player?.player?.state?.round_kills;
                    if (IsKill)
                    {
                        WavPlayer.IfEnable(MusicIvents.DeathCam, ModeMusic.Play);
                    }
                }
                if (player?.player?.state?.health == 0 && player?.player?.steamid == player?.client?.steamid && !IsDeath)
                {
                    IsDeath = true;
                    if (IsDeath)
                    {
                        WavPlayer.IfEnable(MusicIvents.DeathCam, ModeMusic.Play);
                    }
                }
            }
            catch { }
        }

        private static async Task Bomb(CancellationToken cts)
        {
            try
            {
                Main.Instance?.SetConsoleLog($"Проверка бомбы");
                if (player?.round?.bomb == "planted" && LastMusic != MusicIvents.Bomb && LastMusic != MusicIvents.TenSecondBomb)
                {
                    WavPlayer.IfEnable(MusicIvents.Bomb, ModeMusic.Stop);
                    Main.Instance?.SetConsoleLog($"Начался отсчет бомбы");
                    for (int i = 0; i <= 28; i++)
                    {
                        if (cts.IsCancellationRequested || !(currect_player?.round?.bomb == "planted" && LastMusic == MusicIvents.Bomb)) return;
                        System.Threading.Thread.Sleep(1000);
                        Main.Instance?.SetConsoleLog($"{i}");
                    }
                    if (cts.IsCancellationRequested) return;
                    if (currect_player?.round?.bomb == "planted" && LastMusic == MusicIvents.Bomb)
                        WavPlayer.IfEnable(MusicIvents.TenSecondBomb, ModeMusic.IfPlayingStop);
                }
            }
            catch { }
        }

        private static async Task CheckTeam(CancellationToken cts)
        {
            if (Team != player?.player?.team && player?.player?.team != null && MusicKits.settings.DoubleMode)
            {
                try
                {
                    if (player?.player?.team == "T")
                    {
                        if (teams.T != null)
                        {
                            await Task.Run(() => WavPlayer.ReloadMusicKit(cts, teams.T), cts);
                            Main.Instance?.SetConsoleLog($"Включение музыки за Т");
                        }
                    }
                    else
                    {
                        if (teams.Ct != null)
                        {
                            await Task.Run(() => WavPlayer.ReloadMusicKit(cts, teams.Ct), cts);
                            Main.Instance?.SetConsoleLog($"Включение музыки за КТ");
                        }
                    }
                }
                catch { Main.Instance?.SetConsoleLog($"Ошибка при включении"); }
                Team = player?.player?.team;
            }
        }

        private static async Task WinLose(CancellationToken cts)
        {
            try{
                if (player?.player?.team == player?.round?.win_team && player?.round != null && player?.player?.MVP?.mvp != null)
                {
                    if (player?.player?.MVP?.mvp != Mvp && player?.client?.steamid == player?.player?.steamid)
                        WavPlayer.IfEnable(MusicIvents.Mvp, ModeMusic.Stop);
                    else
                        WavPlayer.IfEnable(MusicIvents.WinRound, ModeMusic.Stop);
                }
                else if (player?.player?.team != player?.round?.win_team && player?.round?.win_team != null && player?.player?.team != null)
                    WavPlayer.IfEnable(MusicIvents.LoseRound, ModeMusic.Stop);
            }
            catch { }
        }

        private static async Task Competitive(CancellationToken cts)
        {
            try { 
                if (!StartRound) { 
                    if (gamemode == Mode.Competitive && player?.map?.round == 0 && LastMusic == MusicIvents.Menu && player?.map?.phase == "live")
                    {
                        GameEnd = false;
                        StartRound = true;
                        WavPlayer.IfEnable(MusicIvents.StartGame, ModeMusic.Stop);
                        Main.Instance?.SetConsoleLog($"Начался отсчет начала игры");

                        for (int i = 0; i < 7; i++){
                            Main.Instance?.SetConsoleLog($"{i}");
                            
                            await Task.Delay(1000);
                            if (cts.IsCancellationRequested){
                                StartRound = false;
                                return;
                            }
                        }
                        WavPlayer.IfEnable(MusicIvents.StartRound, ModeMusic.Stop);
                        StartRound = false;
                    }

                    else if (player?.map?.phase == "warmup") { WavPlayer.StopMusic();}

                    else if (player?.map?.phase == "gameover"){
                        WavPlayer.IfEnable(MusicIvents.EndGame, ModeMusic.IfPlayingStop);
                        GameEnd = true;
                    }

                    else if (player?.round?.phase == "freezetime")
                        {
                            WavPlayer.IfEnable(MusicIvents.StartRound, ModeMusic.IfPlayingStop);
                            IsDeath = false;
                        }
                
                    else if (player?.round?.phase == "live" && LastMusic == MusicIvents.StartRound )
                        {
                            Mvp = player?.player?.MVP?.mvp;

                            WavPlayer.IfEnable(MusicIvents.StartAction, ModeMusic.Stop);
                            kills_r = 0;
                            Main.Instance?.SetConsoleLog($"Начался отсчет начала раунла");

                            for (int i = 0; i < Round_Timer - 10; i++)
                            {
                                Main.Instance?.SetConsoleLog($"{i}");
                                await Task.Delay(1000);
                                if (cts.IsCancellationRequested || LastMusic != MusicIvents.StartAction)
                                    return;
                            }
                            WavPlayer.IfEnable(MusicIvents.TenSecondBomb, ModeMusic.Stop);

                        }
                }
            }
            catch { StartRound = false; }
        }

        private static async Task Update(CancellationToken cts)
        {
            if (cts.IsCancellationRequested) return;
            if (player?.round == null && player?.player?.activity == "menu")
                gamemode = Mode.Menu;
            else if (player?.map?.mode == "competitive")
            {
                teams = Main.Comp;
                gamemode = Mode.Competitive;
            }
            else if (player?.map?.mode == "scrimcomp2v2")
            {
                teams = Main.Wingmans;
                gamemode = Mode.Wingmans;
            }
            else if (player?.map?.mode == "casual")
            {
                teams = Main.Public;
                gamemode = Mode.Public;
            }
            else if (player?.map?.mode == "deathmatch")
            {
                teams.Ct = Main.Dm;
                teams.T = Main.Dm;
                gamemode = Mode.Deathmatch;
            }
        }

        public static void Stop()
        {
            GameEnd = false;
            LastMusic = null;
        }

    }
}

// Блядь, а нельзя попроще было сделать. Сука ;<
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