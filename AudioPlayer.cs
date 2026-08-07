using NAudio.Wave;
using System.ComponentModel.Design.Serialization;
using static PlayerStats;
using static System.Windows.Forms.AxHost;
namespace MusicComanderGUI
{
    static class WavPlayer
    {
        private static float volume = 1.0f;
        public static Dictionary<string, SoundPlayer> Music = new Dictionary<string, SoundPlayer>();

        public static async void SetupMusic()
        {
            AudioStop();
            Music.Clear();

            // Словарь сопоставления состояний и путей к файлам из настроек
            var settingsMap = new Dictionary<string, string>
            {
                { "menu", MusicKits.settings?.Last?.menu },
                { "WinRound", MusicKits.settings?.Last?.WinRound },
                { "LoseRound", MusicKits.settings?.Last?.LoseRound },
                { "bomb", MusicKits.settings?.Last?.Bomb },
                { "deathCam", MusicKits.settings?.Last?.deathCam },
                { "StartAction", MusicKits.settings?.Last?.StartAction },
                { "StartRound", MusicKits.settings?.Last?.StartRound },
                { "intermission", MusicKits.settings?.Last?.intermission },
                { "MVP", MusicKits.settings?.Last?.MVP },
                { "TenSecond", MusicKits.settings?.Last?.TenSecond },
                { "StartGame", MusicKits.settings?.Last?.StartGame },
                { "kill", MusicKits.settings?.Last?.KillSound },
                { "EndGame", MusicKits.settings?.Last?.EndGame },
                { "TenSecondRound", MusicKits.settings?.Last?.TenSecondRound }
            };

            foreach (var kvp in settingsMap)
            {
                string state = kvp.Key;
                string filePath = kvp.Value;

                // Если путь к файлу не задан, просто пропускаем или пишем null
                if (string.IsNullOrEmpty(filePath))
                {
                    Music[state] = null;
                    continue; // Переходим к следующему треку, а не выходим из цикла!
                }

                try
                {
                    WaveOutEvent player = new WaveOutEvent();
                    AudioFileReader file = new AudioFileReader(filePath);

                    // Настройка зацикливания для определенных треков
                    if (state == "menu" || state == "StartRound")
                    {
                        LoopStream loop = new LoopStream(file);
                        player.Init(loop);
                    }
                    else
                    {
                        player.Init(file);
                    }

                    SoundPlayer buf = new SoundPlayer
                    {
                        file = file,
                        player = player
                    };

                    Music[state] = buf;
                    Main.Instance?.SetConsoleLog($"[GSI]: Звук '{state}' успешно загружен.");
                }
                catch (Exception ex)
                {
                    Main.Instance?.SetConsoleLog($"[Ошибка загрузки {state}]: {ex.Message}");
                    Music[state] = null;
                }
            }
        }

        public static void PlayMusic(string state)
        {
            if (Ivents.LastMusic == state) return;
            StopMusic();
            Main.Instance?.SetConsoleLog($"[GSI]: Звук '{Music[state].file?.FileName}' успешно запущен.");
            if (Music[state] != null)
            {
                Music[state].file.Position = 0;
                Music[state].file.Volume = volume;
                Music[state].player.Play();
            }
        }

        public static void StopMusic()
        {
            if (Music[Ivents.LastMusic] != null)
            {
                Music[Ivents.LastMusic].player?.Stop();
                Music[Ivents.LastMusic].file?.Position = 0;
            }
        }

        public static void SetVolume(float i)
        {
                if (Ivents.LastMusic != null && Server.is_Running && Music[Ivents.LastMusic] != null)
                {
                    Music[Ivents.LastMusic]?.file?.Volume = i;
                }
                volume = i;
        }

        public static void IfEnable(string song, bool Check)
        {
            if (Check)
                PlayMusic(song);
            else
                StopMusic();
            Main.Instance?.SetConsoleLog($"[GSI]: Звук '{song}' успешно запущен.");
            Ivents.LastMusic = song;
        }

        public static void AudioStop()
        {
            foreach (var b in Music)
            {
                if (b.Value == null) { continue; }
                b.Value?.player?.Stop();
                b.Value?.player?.Dispose();
                b.Value?.file.Dispose();
            }
        }

        public static async void ReloadMusicKit(CancellationToken cts)
        {
            AudioStop();
            await Task.Run(() => SetupMusic(), cts);
            await Task.Delay(500);
            if (Music[Ivents.LastMusic] != null)
            {
                
                Music[Ivents.LastMusic].file.Position = 0;
                Music[Ivents.LastMusic].file.Volume = volume;
                Music[Ivents.LastMusic].player.Play();
            }
        }
    }

    class SoundPlayer
    {
        public AudioFileReader? file { get; set; }

        public WaveOutEvent? player { get; set; }
    }

    class LoopStream : WaveStream
    {
        WaveStream sourceStream;


        public LoopStream(WaveStream sourceStream)
        {
            this.sourceStream = sourceStream;
            this.EnableLooping = true;
        }

        /// <summary>
        /// Use this to turn looping on or off
        /// </summary>
        public bool EnableLooping { get; set; }

        /// <summary>
        /// Return source stream's wave format
        /// </summary>
        public override WaveFormat WaveFormat
        {
            get { return sourceStream.WaveFormat; }
        }

        /// <summary>
        /// LoopStream simply returns
        /// </summary>
        public override long Length
        {
            get { return sourceStream.Length; }
        }

        /// <summary>
        /// LoopStream simply passes on positioning to source stream
        /// </summary>
        public override long Position
        {
            get { return sourceStream.Position; }
            set { sourceStream.Position = value; }
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            int totalBytesRead = 0;

            while (totalBytesRead < count)
            {
                int bytesRead = sourceStream.Read(buffer, offset + totalBytesRead, count - totalBytesRead);
                if (bytesRead == 0)
                {
                    if (sourceStream.Position == 0 || !EnableLooping)
                    {
                        // something wrong with the source stream
                        break;
                    }
                    // loop
                    sourceStream.Position = 0;
                }
                totalBytesRead += bytesRead;
            }
            return totalBytesRead;
        }
    }
}