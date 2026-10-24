// ---------------------------------------------------------------------------------------
// Файл MusicKits.cs содержит класс MusicKit, через который вызываем функции сохранения и
// загрузки конфигураций наборов.
// ---------------------------------------------------------------------------------------

using CustomMusic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
namespace MusicComanderGUI
{
    class Setting
    {
        [JsonPropertyName("Music_Paths")]
        public List<Music> Music_Paths { get; set; } = new List<Music>();
        [JsonPropertyName("LastProfile")]
        public int? profile { get; set; }
        [JsonPropertyName("Profiles")]
        public List<Profile> profiles { get; set; } = new List<Profile>();

    }

    class MusicKits
    {
        public static Setting? settings;
        private static string? OutPath;
        public static void LoadSetting()
        {
            OutPath = Directory.GetCurrentDirectory() + "\\setting.json";
            try {
                string json = File.ReadAllText(OutPath);
                settings = JsonSerializer.Deserialize<Setting>(json);
            }
            catch (FileNotFoundException) {
                if (settings == null) settings = new Setting();
                SaveSetting();
            }
            catch (System.Text.Json.JsonException){
                if (settings == null) settings = new Setting();
                SaveSetting();
            }

            if (settings?.Music_Paths == null) settings?.Music_Paths = new List<Music>();
            if (settings.profiles == null) settings.profiles = new List<Profile>();
        }

        public static Kit_Sound? loadJson(string? path)
        {
            if (path == null)
            {
                throw new Exception("Path empty!");
            }
            try
            {
                Directory.SetCurrentDirectory(path);
                path += "\\profile.json";
                string json = File.ReadAllText(path);
                SaveSetting();
                return JsonSerializer.Deserialize<Kit_Sound>(json);
            }
            catch (Exception ex){
                if (Server.Logging) { ConsoleLog.Instance?.SendConsoleLog(ex.Message); }
                return null; 
            }
        }

        public static void SaveSetting()
        {
            if (settings == null) settings = new Setting();
            if (settings.Music_Paths == null) settings.Music_Paths = new List<Music>();
            if (settings.profiles == null) settings.profiles = new List<Profile>();
            var options = new JsonSerializerOptions { WriteIndented = true };
            File.WriteAllText(OutPath, JsonSerializer.Serialize(settings, options));
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
            settings?.Music_Paths.Add(buf);
            return load;
        }

        public static void CopyFiles(string path, Kit_Sound music)
        {
            for (int ivents = 0; ivents < 13; ivents++)
            {
                for (int count = 0; count < music?.music?[ivents].Count; count++)
                {
                    string FileName = Enum.GetName(typeof(MusicIvents), ivents) + " "  + count.ToString() + ".wav";
                    string? FilePath = music?.music[ivents][count].path;
                    music?.music[ivents][count].path = CopyFileByFileSave(FilePath, path, FileName);
                }
            }
            music?.image = CopyFileByFileSave(music?.image, path, "Image.png");
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
                    return FileName;
                }
                catch(Exception ex)
                {
                    Main.Instance?.SetConsoleLog($"Ошибка при копировании {FileName} {ex}");
                    return null;
                }
            }
        }
    }

    public class Kit_Sound
    {
        [JsonPropertyName("name")]
        public string? name { get; set; }

        [JsonPropertyName("image")]
        public string? image { get; set; }

        public List<AudioFiles>[]? music { get; set; } = new List<AudioFiles>[13];
    }
}