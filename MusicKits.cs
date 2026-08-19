// ---------------------------------------------------------------------------------------
// Файл MusicKits.cs содержит класс MusicKit, через который вызываем функции сохранения и
// загрузки конфигураций наборов.
// ---------------------------------------------------------------------------------------

using NAudio.Gui;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

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
        [JsonPropertyName("Wingmans")]
        public Teams? Wingmans { get; set; }
        [JsonPropertyName("Public")]
        public Teams? Public { get; set; }
        [JsonPropertyName("Volume")]
        public int[] Volume { get; set; } = new int[13];
        [JsonPropertyName("DoubleMode")]
        public bool DoubleMode = false;
    }

    class MusicKits
    {
        public static Setting? settings;

        public static void LoadSetting()
        {
            try {
                string json = File.ReadAllText("setting.json");
                settings = JsonSerializer.Deserialize<Setting>(json);
            }
            catch (FileNotFoundException) { 
                if (settings == null) settings = new Setting();
                for(int i =0; i < 13; i++)
                {
                    settings?.Volume[i] = 100;
                }
                SaveSetting();
            }
            catch (System.Text.Json.JsonException){
                if (settings == null) settings = new Setting();
                for(int i = 0; i < 13; i++)
                {
                    settings?.Volume[i] = 100;
                }
                SaveSetting();
            }

            if (settings.Music_Paths == null) settings.Music_Paths = new List<Music>();
            if (settings.Competitivie == null) settings.Competitivie = new Teams();
            if (settings.Public == null) settings.Public = new Teams();
            if (settings.Wingmans == null) settings.Wingmans = new Teams();
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
            if (settings.Wingmans == null) settings.Wingmans = new Teams();
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
            for (int  i = 0; i < 13; i++)
            {
                music.music[i] = CopyFileByFileSave(music.music[i], path, Enum.GetName(typeof(MusicIvents), i) + ".wav");
            }
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

        public string[]? music { get; set; } = new string[13];
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