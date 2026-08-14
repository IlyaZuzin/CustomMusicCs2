using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using static System.Windows.Forms.Design.AxImporter;

namespace MusicComanderGUI
{
    class Setting
    {
        [JsonPropertyName("Music_Paths")]
        public List<Music> Music_Paths { get; set; } = new List<Music>();
        [JsonPropertyName("Competitive")]
        public Teams? Competitivie { get; set; }
        [JsonPropertyName("Deathmatch")]
        public int? Deathmatch { get; set; }
        [JsonPropertyName("Public")]
        public Teams? Public { get; set; }
        [JsonPropertyName("Volume")]
        public int? Volume { get; set; }
        [JsonPropertyName("DoubleMode")]
        public bool DoubleMode = false;
    }

    class MusicKits
    {
        public static Setting? settings;
        private static Mode gamemode;

        public static void LoadSetting()
        {
            try {
                string json = File.ReadAllText("setting.json");
                settings = JsonSerializer.Deserialize<Setting>(json);
            }
            catch (FileNotFoundException) { 
                SaveSetting();
            }
            catch (System.Text.Json.JsonException){
                SaveSetting();
            }

            if (settings == null) settings = new Setting();
            if (settings.Music_Paths == null) settings.Music_Paths = new List<Music>();
            if (settings.Competitivie == null) settings.Competitivie = new Teams();
            if (settings.Public == null) settings.Public = new Teams();
        }

        public static Kit_Sound? loadJson(string path)
        {
            try
            {
                path += "\\profile.json";
                string json = File.ReadAllText(path);
                return JsonSerializer.Deserialize<Kit_Sound>(json);
            }
            catch { return null; }
        }

        public static void SaveSetting()
        {
            if (settings == null) settings = new Setting();
            if (settings.Music_Paths == null) settings.Music_Paths = new List<Music>();
            if (settings.Competitivie == null) settings.Competitivie = new Teams();
            if (settings.Public == null) settings.Public = new Teams();
            var options = new JsonSerializerOptions { WriteIndented = true };
            File.WriteAllText("setting.json", JsonSerializer.Serialize(settings, options));
        }

        public static string CreateMusicKit(string path, string name, Kit_Sound music)
        {
            string New_Path = path + $"\\{name}\\";
            Directory.CreateDirectory(New_Path);
            var options = new JsonSerializerOptions { WriteIndented = true };
            CopyFiles(New_Path, music);
            File.WriteAllText($"{New_Path}\\profile.json", JsonSerializer.Serialize(music, options));
            AddMusicKit(New_Path);
            SaveSetting();
            return New_Path;
        }

        public static Kit_Sound? AddMusicKit(string path)
        {
            Kit_Sound? load = loadJson(path);

            Music buf = new Music();
            buf.image = load?.image;
            buf.name = load?.name;
            buf.path = path;
            settings.Music_Paths.Add(buf);
            return load;
        }

        private static void CopyFiles(string path, Kit_Sound music)
        {
            music.Musics[(int)MusicIvents.Bomb].path = CopyFileByFileSave(music.Musics[(int)MusicIvents.Bomb].path, path, "bomb.wav");
            music.Musics[(int)MusicIvents.WinRound].path = CopyFileByFileSave(music.Musics[(int)MusicIvents.WinRound].path, path, "WinRound.wav");
            music.Musics[(int)MusicIvents.LoseRound].path = CopyFileByFileSave(music.Musics[(int)MusicIvents.LoseRound].path, path, "LoseRound.wav");
            music.Musics[(int)MusicIvents.DeathCam].path = CopyFileByFileSave(music.Musics[(int)MusicIvents.DeathCam].path, path, "deathCam.wav");
            music.Musics[(int)MusicIvents.StartAction].path = CopyFileByFileSave(music.Musics[(int)MusicIvents.StartAction].path, path, "StartAction.wav");
            music.Musics[(int)MusicIvents.Menu].path = CopyFileByFileSave(music.Musics[(int)MusicIvents.Menu].path, path, "menu.wav");
            music.Musics[(int)MusicIvents.TenSecondBomb].path = CopyFileByFileSave(music.Musics[(int)MusicIvents.TenSecondBomb].path, path, "TenSecondsBomb.wav");
            music.Musics[(int)MusicIvents.Mvp].path = CopyFileByFileSave(music.Musics[(int)MusicIvents.Mvp].path, path, "MVP.wav");
            music.Musics[(int)MusicIvents.StartRound].path = CopyFileByFileSave(music.Musics[(int)MusicIvents.StartRound].path, path, "StartRound.wav");
            music.Musics[(int)MusicIvents.StartGame].path = CopyFileByFileSave(music.Musics[(int)MusicIvents.StartGame].path, path, "StartGame.wav");
            music.Musics[(int)MusicIvents.KillSound].path = CopyFileByFileSave(music.Musics[(int)MusicIvents.KillSound].path, path, "KillSound.wav");
            music.Musics[(int)MusicIvents.EndGame].path = CopyFileByFileSave(music.Musics[(int)MusicIvents.EndGame].path, path, "EndGame.wav");
            music.Musics[(int)MusicIvents.TenSecondRound].path = CopyFileByFileSave(music.Musics[(int)MusicIvents.TenSecondRound].path, path, "TenSecondRound.wav");
            music.image = CopyFileByFileSave(music.image, path, "Image.png");
       }

        public static string? CopyFileByFileSave(string? StartPath, string Path_To_Copy, string FileName)
        {
            if(!File.Exists(StartPath) || string.IsNullOrEmpty(StartPath)) { return null; }
            else
            {
                try
                {
                    if (Path.GetFullPath(StartPath).Equals(Path.GetFullPath(Path_To_Copy + FileName), StringComparison.OrdinalIgnoreCase))
                        return StartPath;
                    
                    File.Copy(StartPath, Path_To_Copy + FileName, true);
                    return Path_To_Copy + FileName;
                }
                catch(Exception ex)
                {
                    Main.Instance?.SetConsoleLog($"Ошибка при копировании {FileName} {ex}");
                    return null;
                }
            }

        }
    
        public static void UpdateMusicKit(Kit_Sound update)
        { 
            CopyFiles(update.path, update);
            var options = new JsonSerializerOptions { WriteIndented = true };
            File.WriteAllText($"{update.path}\\profile.json", JsonSerializer.Serialize(update, options));
        }
    }

    class Kit_Sound
    {
        [JsonPropertyName("name")]
        public string? name { get; set; }

        [JsonPropertyName("image")]
        public string? image { get; set; }

        [JsonPropertyName("path")]
        public string? path { get; set; }

        public MusicSettings?[] Musics { get; set; } = new MusicSettings[13];
    }

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
        KillSound,
        EndGame,
        TenSecondRound,
    }

    class Music
    {
        public string? name { get; set; }
        public string? image { get; set; }
        public string? path { get; set; }
    }
    
    class MusicSettings
    {
        public string? path { get; set; }
        public int volume { get; set; } = 100;
        public bool IsEnable { get; set; } = true;
    }

    public class Teams
    {
        public int? T { get; set; }
        public int? Ct {  get; set; }
    }

    public class TeamPath
    {
        public string? T { get; set; }
        public string? Ct { get; set; }
    }
}