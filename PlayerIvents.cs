using System.Text.Json.Serialization;
using static PlayerStats;
namespace MusicComanderGUI
{
    public enum Mode
    { 
        Menu,
        Competitive,
        Public,
        Deathmatch
    }

    static class Ivents 
    {
        public static string? LastMusic = "kill";
        private static float volume = 1.0f;
        private static int Round_Timer = 115;
        public static bool IsMenu = true;
        private static int? Mvp = 0;
        private static bool GameEnd = false;
        public static string? Team = "Ct";
        public static bool IsBomb = true;
        public static bool IsStartGame = true;
        public static bool IsEndGame = true;
        public static bool IsStartAction = true;
        public static bool IsStartRound = true;
        public static bool IsMVP = true;
        public static bool IsWinRound = true;
        public static bool IsTenSecondRound = true;
        public static bool IsLoseRound = true;
        public static bool IsTenSecond = true;
        public static bool IsDeath = true;
        public static bool isDead = true;
        public static bool IsKill = true;
        private static bool StartRound = false;
        private static int? kills_r = 0;
        private static bool? IsCompetitive;
        public static PlayerStats? player = null;
        private static PlayerStats? currect_player = null;
        private static Mode gamemode = Mode.Menu;

        public static async Task Tick(PlayerStats NewData, CancellationToken cts)
        {
            player = NewData;
            currect_player = NewData;

            Update(cts);

            if (gamemode == Mode.Menu)
            {
                StartRound = false;
                WavPlayer.IfEnable("menu", IsMenu);
            }
            else if (gamemode == Mode.Competitive)
            {
                Round_Timer = 115;
                Task.Run(() => PlayerIvents(cts));
                Task.Run(() => Bomb(cts));
                Task.Run(() => WinLose(cts));
                Task.Run(() => Competitive(cts));
            }
            else if (gamemode == Mode.Public)
            {
                Round_Timer = 135;
                Task.Run(() => Competitive(cts));
                Task.Run(() => Bomb(cts));
                Task.Run(() => WinLose(cts));
                Task.Run(() => Competitive(cts));
            }
            else if (gamemode == Mode.Deathmatch)
            {
                Round_Timer = 600;
                Task.Run(() => PlayerIvents(cts));
            }
            
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
                        if (WavPlayer.Music["kill"] != null)
                        {
                            WavPlayer.Music["kill"].file?.Position = 0;
                            WavPlayer.Music["kill"].player?.Play();
                            WavPlayer.Music["kill"].player?.Volume = volume;
                        }
                    }
                }
                if (player?.player?.state?.health == 0 && player?.player?.steamid == player?.client?.steamid && !isDead)
                {
                    isDead = true;
                    if (IsDeath)
                    {
                        if (WavPlayer.Music["deathCam"] != null)
                        {
                            WavPlayer.Music["deathCam"].file?.Position = 0;
                            WavPlayer.Music["deathCam"].player?.Play();
                            WavPlayer.Music["deathCam"].player?.Volume = volume;
                        }
                    }
                }
            }
            catch { }
            if (Team != player?.player?.team && player?.player?.team != null && MusicKits.settings.DoubleMode)
            {
                try
                {
                    if (player?.player?.team == "T")
                    {
                        if (Main.T != null)
                        {
                            MusicKits.settings.Last = MusicKits.loadJson(Main.T);
                            await Task.Run(() => WavPlayer.ReloadMusicKit(cts), cts);
                            Main.Instance?.SetConsoleLog($"Включение музыки за Т");
                        }
                    }
                    else
                    {
                        if (Main.Ct != null)
                        {
                            MusicKits.settings.Last = MusicKits.loadJson(Main.Ct);
                            await Task.Run(() => WavPlayer.ReloadMusicKit(cts), cts);
                            Main.Instance?.SetConsoleLog($"Включение музыки за КТ");
                        }
                    }
                }
                catch { Main.Instance?.SetConsoleLog($"Ошибка при включении"); }
                Team = player?.player?.team;
            }
        }

        private static async Task Bomb(CancellationToken cts)
        {
            try
            {
                Main.Instance?.SetConsoleLog($"Проверка бомбы");
                if (player?.round?.bomb == "planted" && LastMusic != "bomb" && LastMusic != "TenSecond")
                {
                    WavPlayer.IfEnable("bomb", IsBomb);
                    Main.Instance?.SetConsoleLog($"Начался отсчет бомбы");
                    for (int i = 0; i <= 28; i++)
                    {
                        if (cts.IsCancellationRequested || !(currect_player?.round?.bomb == "planted" && LastMusic == "bomb")) return;
                        System.Threading.Thread.Sleep(1000);
                        Main.Instance?.SetConsoleLog($"{i}");
                    }
                    if (cts.IsCancellationRequested) return;
                    if (currect_player?.round?.bomb == "planted" && LastMusic == "bomb")
                        WavPlayer.IfEnable("TenSecond", IsTenSecond);

                }
            }
            catch { }
        }

        private static async Task WinLose(CancellationToken cts)
        {
            try{
                if (player?.player?.team == player?.round?.win_team && player?.round != null && player?.player?.MVP?.mvp != null)
                {
                    if (player?.player?.MVP?.mvp != Mvp && player?.client?.steamid == player?.player?.steamid)
                        WavPlayer.IfEnable("MVP", IsWinRound);
                    else
                        WavPlayer.IfEnable("WinRound", IsWinRound);
                }
                else if (player?.player?.team != player?.round?.win_team && player?.round?.win_team != null && player?.player?.team != null)
                    WavPlayer.IfEnable("LoseRound", IsLoseRound);
            }
            catch { }
        }

        private static async Task Competitive(CancellationToken cts)
        {
            try { 
                if (!StartRound) { 
                    if (gamemode == Mode.Competitive && player?.map?.round == 0 && LastMusic == "menu" && player?.map?.phase == "live")
                    {
                        GameEnd = false;
                        StartRound = true;
                        WavPlayer.IfEnable("StartGame", IsStartGame);
                        Main.Instance?.SetConsoleLog($"Начался отсчет начала игры");

                        for (int i = 0; i < 8; i++){
                            Main.Instance?.SetConsoleLog($"{i}");
                            
                            await Task.Delay(1000);
                            if (cts.IsCancellationRequested){
                                StartRound = false;
                                return;
                            }
                        }
                        WavPlayer.IfEnable("StartRound", IsStartRound);
                        StartRound = false;
                    }

                    else if (player?.map?.phase == "warmup") { WavPlayer.StopMusic();}

                    else if (player?.map?.phase == "gameover"){
                       
                        WavPlayer.IfEnable("EndGame", IsEndGame);
                        GameEnd = true;
                    }

                    else if (player?.round?.phase == "freezetime")
                        {
                            WavPlayer.IfEnable("StartRound", IsStartRound);
                            isDead = false;
                        }
                
                    else if (player?.round?.phase == "live" && LastMusic == "StartRound" )
                        {
                            Mvp = player?.player?.MVP?.mvp;

                            WavPlayer.IfEnable("StartAction", IsStartAction);
                            kills_r = 0;
                            Main.Instance?.SetConsoleLog($"Начался отсчет начала раунла");

                            for (int i = 0; i < Round_Timer - 10; i++)
                            {
                                Main.Instance?.SetConsoleLog($"{i}");
                                await Task.Delay(1000);
                                if (cts.IsCancellationRequested || LastMusic != "StartAction")
                                    return;
                            }
                            WavPlayer.IfEnable("TenSecondRound", IsTenSecondRound);

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
                gamemode = Mode.Competitive;
            else if (player?.map?.mode == "casual")
                gamemode = Mode.Deathmatch;
            else if (player?.map?.mode == "deathmatch")
                gamemode = Mode.Deathmatch;
        }

        public static void Stop()
        {
            GameEnd = false;
            Ivents.LastMusic = "kill";
            Ivents.Team = "Ct";
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