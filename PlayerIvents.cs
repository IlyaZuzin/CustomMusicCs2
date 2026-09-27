// ----------------------------------------------------------------------------------------------
// Данный файл определяет события, которые происходят во время игры и запускает необходимое аудио
// ----------------------------------------------------------------------------------------------
using System.Text.Json.Serialization;
using System.Windows.Automation;

namespace MusicComanderGUI
{
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

        private static uint WonRound = 0;
        private static uint LoseRound = 0;
        private static uint Kills = 0;
        private static uint Deaths = 0;
        private static uint MVP = 0;

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
                Round_Timer = 115;
     
            else if (gamemode == Mode.Wingmans)
                Round_Timer = 90;
      
            else if (gamemode == Mode.Public)
                Round_Timer = 135;

            Task.Run(() => PlayerIvents(cts));
            Task.Run(() => Bomb(cts));
            Task.Run(() => WinLose(cts));
            Task.Run(() => Competitive(cts));
            Task.Run(() => CheckTeam(cts));
            //Main.Instance?.SetConsoleLog($"Last Player state: {Enum.GetName(typeof(MusicIvents), (int)LastMusic)} \t Mode: {Enum.GetName(typeof(Mode), gamemode)} {Environment.NewLine}Map mode: {player?.map?.mode}");
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
                        Kills++;
                        Main.Instance.Kill.Text = Kills.ToString();
                        Main.Instance.KD.Text = (Kills / Deaths).ToString();
                    }
                }
                if (player?.player?.state?.health == 0 && player?.player?.steamid == player?.client?.steamid && !IsDeath)
                {
                    IsDeath = true;
                    if (IsDeath)
                    {
                        WavPlayer.IfEnable(MusicIvents.DeathCam, ModeMusic.Play);
                        Deaths++;
                        Main.Instance.Death.Text = Deaths.ToString();
                        Main.Instance.KD.Text = (Kills / Deaths).ToString();
                    }
                }
            }
            catch { }
        }

        private static async Task Bomb(CancellationToken cts)
        {
            try
            {
                if (player?.round?.bomb == "planted" && LastMusic != MusicIvents.Bomb && LastMusic != MusicIvents.TenSecondBomb)
                {
                    WavPlayer.IfEnable(MusicIvents.Bomb, ModeMusic.Stop);
                 
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
                        if (teams?.T != null)
                        {
                            await Task.Run(() => WavPlayer.ReloadMusicKit(cts, teams.T), cts);
                        }
                    }
                    else
                    {
                        if (teams?.Ct != null)
                        {
                            await Task.Run(() => WavPlayer.ReloadMusicKit(cts, teams.Ct), cts);
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
                if(LastMusic != MusicIvents.EndGame)
                {
                    if (player?.round != null && player?.round?.win_team != null)
                    {
                        if (player?.player?.team == player?.round?.win_team)
                        {
                            if (player?.player?.MVP?.mvp != null && player?.player?.MVP?.mvp != Mvp && player?.client?.steamid == player?.player?.steamid)
                            {
                                WavPlayer.IfEnable(MusicIvents.Mvp, ModeMusic.Stop);
                                MVP++;
                                Main.Instance.MVP.Text = MVP.ToString();
                            }
                            else
                            {
                                WavPlayer.IfEnable(MusicIvents.WinRound, ModeMusic.Stop);
                                WonRound++;
                                Main.Instance.WonRound.Text = WonRound.ToString();
                            }
                        }
                        else if (player?.player?.team != player?.round?.win_team)
                        {
                            WavPlayer.IfEnable(MusicIvents.LoseRound, ModeMusic.Stop);
                            LoseRound++;
                            Main.Instance.LostRound.Text = LoseRound.ToString();
                        }
                        WavPlayer.Randomizate((int)MusicIvents.StartRound, (int)MusicIvents.StartAction);
                    }
                }
            }
            catch { }
        }

        private static async Task Competitive(CancellationToken cts)
        {
            try { 
                if (!StartRound) { 
                    if (player?.map?.round == 0 && LastMusic == MusicIvents.Menu && player?.map?.phase == "live")
                    {
                        GameEnd = false;
                        StartRound = true;
                        WavPlayer.IfEnable(MusicIvents.StartGame, ModeMusic.Stop);

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

                    else if (player?.map?.phase == "gameover"){
                        WavPlayer.IfEnable(MusicIvents.EndGame, ModeMusic.IfPlayingStop);
                    }
                    else if (player?.map?.phase == "warmup") { 
                        WavPlayer.StopMusic();
                    }

                    else if (player?.round?.phase == "freezetime")
                    {
                        WavPlayer.IfEnable(MusicIvents.StartRound, ModeMusic.Stop);
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
            {   
                teams = Main.teamPath;
                gamemode = Mode.Menu; 
            }
            else if (player?.map?.mode == "competitive")
            {
                teams = Main.teamPath;
                gamemode = Mode.Competitive;
            }
            else if (player?.map?.mode == "scrimcomp2v2")
            {
                teams = Main.teamPath;
                gamemode = Mode.Wingmans;
            }
            else if (player?.map?.mode == "casual")
            {
                teams = Main.teamPath;
                gamemode = Mode.Public;
            }
            else if (player?.map?.mode == "deathmatch")
            {
                teams = Main.teamPath;
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
