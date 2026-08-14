using NAudio.Wave;
using System.ComponentModel.Design.Serialization;
using static PlayerStats;
using static System.Windows.Forms.AxHost;
namespace MusicComanderGUI
{
    static class WavPlayer
    {
        private static float volume = 1.0f;
        public static SoundPlayer[]? music;

        public static async void SetupMusic(Kit_Sound load)
        { 
            AudioStop();
            
            music = new SoundPlayer[13];

            for (int i = 0; i < 13; i++) {
                // Если путь к файлу не задан, просто пропускаем или пишем null
                if (string.IsNullOrEmpty(load.Musics[i].path))
                {
                    music[i] = null;
                    continue; // Переходим к следующему треку, а не выходим из цикла!
                }

                try
                {
                    WaveOutEvent player = new WaveOutEvent();
                    AudioFileReader file = new AudioFileReader(load.Musics[i].path);
                    file.Volume = load.Musics[i].volume / 100;
                    // Настройка зацикливания для определенных треков
                    if (i ==  (int)MusicIvents.Menu || i == (int)MusicIvents.StartRound)
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

                    music[i] = buf;
                    Main.Instance?.SetConsoleLog($"[GSI]: Звук '{Enum.GetName(typeof(MusicIvents), i)}' успешно загружен.");
                }
                catch (Exception ex)
                {
                    Main.Instance?.SetConsoleLog($"[Ошибка загрузки {Enum.GetName(typeof(MusicIvents), i)}]: {ex.Message}");
                    music[i] = null;
                }
            }
        }

        public static void PlayMusic(MusicIvents state)
        {
            if (Ivents.LastMusic == state) return;
            Main.Instance?.SetConsoleLog($"[GSI]: Звук '{music[(int)state].file?.FileName}' успешно запущен.");
            StopMusic();
            if (music[(int)state] != null)
            {
                music[(int)state].file.Position = 0;
                music[(int)state].file.Volume = volume;
                music[(int)state].player.Play();
            }
        }

        public static void StopMusic()
        {
            try
            {
                if (music[(int)Ivents.LastMusic] != null)
                {
                    music[(int)Ivents.LastMusic].player?.Stop();
                    music[(int)Ivents.LastMusic].file?.Position = 0;
                }
            }catch(Exception ex) {
                Main.Instance?.SetConsoleLog($"{ex.Message}");
            }
        }

        public static void SetVolume(MusicIvents mode, float i)
        {
                if (music[(int)mode] != null)
                    music[(int)mode]?.file?.Volume = i;
        }

        public static void IfEnable(MusicIvents song, bool Check)
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
            if(music == null) return;
            foreach (var b in music)
            {
                if (b == null) { continue; }
                b.player?.Stop();
                b.player?.Dispose();
                b.file?.Dispose();
            }
        }

        public static async void ReloadMusicKit(CancellationToken cts, string path)
        {
            Kit_Sound load = MusicKits.loadJson(path);
            AudioStop();
            await Task.Run(() => SetupMusic(load), cts);
            await Task.Delay(500);
            if (music[(int)Ivents.LastMusic] != null)
            {
                music[(int)Ivents.LastMusic].file.Position = 0;
                music[(int)Ivents.LastMusic].file.Volume = volume;
                music[(int)Ivents.LastMusic].player.Play();
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