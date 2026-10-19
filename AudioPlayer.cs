// -------------------------------------------------------------------------------
// Данные файл представляет собой класс, через который мы будем выгружать в память
// аудиофайлы типа *.wav (для большей быстроспособности) и запускать их
// -------------------------------------------------------------------------------
using CustomMusic;
using NAudio.Wave;
using System.Windows.Ink;
namespace MusicComanderGUI
{
    static class WavPlayer
    {
        public static SoundPlayer[]? music;
        private static Random rnd = new Random();
        private static Kit_Sound loaded = new Kit_Sound();
        public static void SetupMusic(Kit_Sound? load)
        {
            AudioStop();
            if (load == null) return;
            loaded = load;
            music = new SoundPlayer[13];
            ChangeMusicRandom();
            // FIX: "Отсутствует звук при загрузке приложения" — сразу запускаем фоновую
            // музыку меню, чтобы аудио-устройство инициализировалось без необходимости
            // вручную менять громкость.
            TryStartMenuMusic();
        }

        // Запускает музыку в главном меню, если сервер запущен, а музыка ещё не играет
        public static void TryStartMenuMusic()
        {
            try
            {
                if (!Server.is_Running || music == null) return;
                if (music[(int)MusicIvents.Menu] == null) return;
                if (Ivents.LastMusic == MusicIvents.Menu && music[(int)MusicIvents.Menu].player != null) return;
                music[(int)MusicIvents.Menu].file.Position = 0;
                music[(int)MusicIvents.Menu].player.Play();
                Ivents.LastMusic = MusicIvents.Menu;
                if (Server.Logging) ConsoleLog.Instance?.SendConsoleLog("Menu music started on setup");
            }
            catch (Exception ex)
            {
                if (Server.Logging) ConsoleLog.Instance?.SendConsoleLog("TryStartMenuMusic error: " + ex.Message);
            }
        }

        public static void PlayMusic(MusicIvents state)
        {
            try
            {
                if (Server.Logging) ConsoleLog.Instance?.SendConsoleLog("Sucsesful play music: " + state.ToString());

                if (music[(int)state] != null)
                {
                    music[(int)state].file.Position = 0;
                    music[(int)state].player.Play();
                }

            }
            catch (Exception ex) { 
                if (Server.Logging) ConsoleLog.Instance?.SendConsoleLog("Something went wrong: " + state.ToString() + "ERROR: " + ex.Message);
            }
        }

        public static void StopMusic()
        {
            try
            {
                if (music == null) return;
                if (Ivents.LastMusic == null) return;
                if (music[(int)Ivents.LastMusic] != null)
                {
                    music[(int)Ivents.LastMusic].player?.Stop();
                    music[(int)Ivents.LastMusic].file?.Position = 0;
                    if (Server.Logging) ConsoleLog.Instance?.SendConsoleLog($"Last music {Ivents.LastMusic.ToString()} stopped");
                }
                if (Ivents.LastMusic != MusicIvents.StartAction && Ivents.LastMusic != MusicIvents.StartAction)
                    RandomMusic(Ivents.LastMusic);
            }
            catch(Exception ex) {
                if (Server.Logging) ConsoleLog.Instance?.SendConsoleLog($"{ex.Message}");
            }
        }

        public static void SetVolume(MusicIvents mode, float volume)
        {
            if (music[(int)mode] != null)
                music[(int)mode]?.file?.Volume = volume;
            if (Server.Logging) ConsoleLog.Instance?.SendConsoleLog($"Set volume {mode.ToString()} = {volume.ToString()}");
        }

        public static void IfEnable(MusicIvents song, ModeMusic mode)
        {
            if (Server.Logging) ConsoleLog.Instance?.SendConsoleLog($"Try enable {song.ToString()} Mode: {mode.ToString()}");
            if (Ivents.LastMusic == song) return;
            // Если текущая музыка отсутсвует, то не выключаем предыдущий аудиофайл. Очень не стабильно!
            if (mode == ModeMusic.IfPlayingStop)
            {
                if (music[(int)song] != null && music[(int)song].file?.Volume != 0)
                {
                    StopMusic();
                    PlayMusic(song);
                }
                Ivents.LastMusic = song;
            }
            // На свой страх и риск: таким образом мы тереям контроль над музыкой
            else if(mode == ModeMusic.Play)
            {
                PlayMusic(song);
            }
            else if(mode == ModeMusic.Stop)
            {
                StopMusic();
                PlayMusic(song);
                Ivents.LastMusic = song;
            }
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
            Kit_Sound? load = MusicKits.loadJson(path);
            if (load == null) { if (Server.Logging) ConsoleLog.Instance?.SendConsoleLog("Null to reload " + path); return; }
            AudioStop();
            SetupMusic(load);
            // FIX: Если текущее событие не было загружено в новом наборе (music == null),
            // SetupMusic не запустит музыку и она «исчезнет». В таком случае
            // запускаем фоновую музыку меню.
            if (Ivents.LastMusic == null || music[(int)Ivents.LastMusic] == null)
            {
                TryStartMenuMusic();
                if (Server.Logging) ConsoleLog.Instance?.SendConsoleLog("Reload: last event missing in kit, fallback to Menu music");
                return;
            }
            if (music[(int)Ivents.LastMusic] != null)
            {
                music[(int)Ivents.LastMusic].file.Position = 0;
                music[(int)Ivents.LastMusic].player.Play();
            }
            if (Server.Logging) ConsoleLog.Instance?.SendConsoleLog("Reload MusicKit " + path);
            Main.Instance?.CurrectMK.Text = load.name;
            Main.PlayingPath = path;
        }

        public static void changevolume(MusicIvents id, float vol)
        {
            if (music == null || music[(int)id] == null) return;
            vol /= 100.0f;
            // FIX: раньше громкость применялась только при запущенном сервере
            // (Server.is_Running), поэтому «при загрузке приложения отсутствовал звук,
            // пока не изменишь громкость». Теперь volume применяется всегда —
            // AudioFileReader.Volume работает и на остановленном проигрывателе.
            music[(int)id].file.Volume = vol;
        }

        private static void ChangeMusicRandom()
        {
            for (int i = 0; i < 13; i++)
            {
                try { 
                    if (loaded.music == null || loaded.music[i] == null || loaded.music[i].Count == 0)
                    {
                        music[i] = null;
                        continue;
                    }
                    // FIX: rnd.Next(count - 1) никогда не выбирал последний трек набора,
                    // а при count == 1 кидал исключение и обнулял всю дорожку события.
                    int random_music = rnd.Next(loaded.music[i].Count);
                    if (string.IsNullOrEmpty(loaded.music[i][random_music].path) && music[i] != null)
                    {
                        music[i] = null;
                        continue;
                    }
                    music[i] = LoadMusic(i,random_music);
                }
                catch (Exception ex)
                {
                    if (Server.Logging) ConsoleLog.Instance?.SendConsoleLog($"[Ошибка загрузки {Enum.GetName(typeof(MusicIvents), i)}]: {ex.Message}");
                    music[i] = null;
                }
            }
        }

        private static SoundPlayer LoadMusic(int ivents_id, int load_music)
        {
            SoundPlayer rand = new SoundPlayer();
            try
            {
                if (music[ivents_id] != null)
                {
                    music[ivents_id].file.Dispose();
                    music[ivents_id].player.Dispose();
                }
                WaveOutEvent player = new WaveOutEvent();
                AudioFileReader file = new AudioFileReader(loaded.music[ivents_id][load_music].path);
                float vol;
                if (loaded.music[ivents_id][load_music].volume != null)
                    vol = loaded.music[ivents_id][load_music].volume / 100.0f;
                else
                    vol = 1.0f;
                file.Volume = vol;
                if (ivents_id == (int)MusicIvents.Menu || ivents_id == (int)MusicIvents.StartRound)
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

                rand = buf;
                if (Server.Logging) ConsoleLog.Instance?.SendConsoleLog($"Random select: {load_music.ToString()}, path: {loaded.music[ivents_id][load_music].path}");
                if (Server.Logging) ConsoleLog.Instance?.SendConsoleLog($"Volume for {Enum.GetName(typeof(MusicIvents), ivents_id)} = {vol}");
                if (Server.Logging) ConsoleLog.Instance?.SendConsoleLog($"Music'{Enum.GetName(typeof(MusicIvents), ivents_id)}' sucsessful load. [{Enum.GetName(typeof(MusicIvents), ivents_id)}] ");
                }
            catch (Exception ex)
            {
                throw ex;
            }
            return rand;
        }

        public static void RandomMusic(MusicIvents? id)
        {
            if (id == null) return;
            // FIX: защита от null/пустого списка дорожек в наборе
            if (loaded.music == null || loaded.music[(int)id] == null) return;
            int size = loaded.music[(int)id].Count;
            if (size <= 1) { 
                return;
            }
            int rand = rnd.Next(size);
            music[(int)id] = LoadMusic((int)id, rand);
        }

        public static void Randomizate(int first, int second)
        {
            // FIX: защита от null/пустого списка дорожек в наборе
            if (loaded.music == null || loaded.music[first] == null || loaded.music[first].Count == 0) return;
            int rand = rnd.Next(loaded.music[first].Count);
            music[first] = LoadMusic(first, rand);
            int? next = loaded.music[first][rand].next_music;
            if (next != null)
            {
                if (loaded.music[second] != null && next.Value >= 0 && next.Value < loaded.music[second].Count)
                    music[second] = LoadMusic(second, next.Value);
            }
            RandomMusic(MusicIvents.DeathCam);
            RandomMusic(MusicIvents.KillSound);
        }
    }
}

// -------------------------------------------------------------------------
// Данное перечисление отвечает за выбор режима при вызове функции IfEnable.
// Он позволяет унивирсальнее работать с ивентами 
// Add: В планах к нему прикрепить кастомные события от пользователя
// -------------------------------------------------------------------------
enum ModeMusic
{
    IfPlayingStop,
    Stop,
    Play
}

class SoundPlayer
{
    public AudioFileReader? file { get; set; }
    public WaveOutEvent? player { get; set; }
}


// -------------------------------------------------
// Этот класс позволяет создавать зацикленные звуки.
// -------------------------------------------------
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