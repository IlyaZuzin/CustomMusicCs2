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
        [JsonPropertyName("Last")]
        public Kit_Sound Last { get; set; } = new Kit_Sound();
        [JsonPropertyName("Theme")]
        public string? theme { get; set; }
        [JsonPropertyName("CtSide")]
        public int? CtSide { get; set; }
        [JsonPropertyName("TSide")]
        public int? TSide { get; set; }
        [JsonPropertyName("Volume")]
        public int? Volume { get; set; }
        [JsonPropertyName("DoubleMode")]
        public bool DoubleMode = false;
    }

    class MusicKits
    {
        public static Setting? settings;

        public static void LoadSetting()
        {
            try
            {
                string json = File.ReadAllText("setting.json");
                settings = JsonSerializer.Deserialize<Setting>(json);
            }
            catch (FileNotFoundException)
            {
                SaveSetting();
            }
            catch (System.Text.Json.JsonException)
            {
                SaveSetting();
            }
            if (settings == null) settings = new Setting();
            if (settings.Music_Paths == null) settings.Music_Paths = new List<Music>();
            if (settings.Last == null) settings.Last = new Kit_Sound();
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
            if (settings.Last == null) settings.Last = new Kit_Sound();
            if (settings.Music_Paths == null) settings.Music_Paths = new List<Music>();

            var options = new JsonSerializerOptions { WriteIndented = true };
            File.WriteAllText("setting.json", JsonSerializer.Serialize(settings, options));
        }

        public static void CreateMusicKit(string path)
        {
            string New_Path = path + $"\\{settings?.Last?.name}_MusicKit\\";
            Directory.CreateDirectory(New_Path);
            var options = new JsonSerializerOptions { WriteIndented = true };
            CopyFiles(New_Path);
            settings.Last.path = New_Path;
            File.WriteAllText($"{New_Path}\\profile.json", JsonSerializer.Serialize(settings?.Last, options));
            AddMusicKit(New_Path);
            SaveSetting();
        }

        public static Kit_Sound? AddMusicKit(string path)
        {
            Kit_Sound? load = loadJson(path);

            Music buf = new Music();
            buf.image = load?.image ?? settings?.Last?.image;
            buf.name = load?.name ?? settings?.Last?.name;
            buf.path = load?.path ?? settings?.Last?.path ?? path;
            settings.Music_Paths.Add(buf);
            return load;
        }

        private static void CopyFiles(string? path)
       {
            settings?.Last?.Bomb = CopyFileByFileSave(settings.Last?.Bomb, path, "bomb.wav");
            settings?.Last?.WinRound = CopyFileByFileSave(settings.Last?.WinRound, path, "WinRound.wav");
            settings?.Last?.LoseRound = CopyFileByFileSave(settings.Last?.LoseRound, path, "LoseRound.wav");
            settings?.Last?.deathCam = CopyFileByFileSave(settings.Last?.deathCam, path, "deathCam.wav");
            settings?.Last?.StartAction = CopyFileByFileSave(settings.Last?.StartAction, path, "StartAction.wav");
            settings?.Last?.menu = CopyFileByFileSave(settings.Last?.menu, path, "menu.wav");
            settings?.Last?.TenSecond = CopyFileByFileSave(settings.Last?.TenSecond, path, "TenSeconds.wav");
            settings?.Last?.MVP = CopyFileByFileSave(settings.Last?.MVP, path, "MVP.wav");
            settings?.Last?.StartRound = CopyFileByFileSave(settings.Last?.StartRound, path, "StartRound.wav");
            settings?.Last?.StartGame = CopyFileByFileSave(settings.Last?.StartGame, path, "StartGame.wav");
            settings?.Last?.KillSound = CopyFileByFileSave(settings.Last?.KillSound, path, "KillSound.wav");
            settings?.Last?.EndGame = CopyFileByFileSave(settings.Last?.EndGame, path, "EndGame.wav");
            settings?.Last?.TenSecondRound = CopyFileByFileSave(settings.Last?.TenSecondRound, path, "TenSecondRound.wav");
            settings?.Last?.image = CopyFileByFileSave(settings.Last?.image, path, "Image.png");
       }

        public static string? CopyFileByFileSave(string? StartPath, string Path_To_Copy, string FileName)
        {
            if(!File.Exists(StartPath) && string.IsNullOrEmpty(StartPath)) { return null; }
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
    
        public static void UpdateMusicKit()
        {
            CopyFiles(settings?.Last?.path);
            var options = new JsonSerializerOptions { WriteIndented = true };
            File.WriteAllText($"{settings?.Last?.path}\\profile.json", JsonSerializer.Serialize(settings?.Last, options));
        }
    }

    class Kit_Sound
    {
        [JsonPropertyName("name")]
        public string? name { get; set; }

        [JsonPropertyName("image")]
        public string? image { get; set; }

        [JsonPropertyName("WinRound")]
        public string? WinRound { get; set; }

        [JsonPropertyName("LoseRound")]
        public string? LoseRound { get; set; }

        [JsonPropertyName("menu")]
        public string? menu { get; set; }

        [JsonPropertyName("Bomb")]
        public string? Bomb { get; set; }

        [JsonPropertyName("deathCam")]
        public string? deathCam { get; set; }

        [JsonPropertyName("path")]
        public string? path { get; set; }

        [JsonPropertyName("StartAction")]
        public string? StartAction { get; set; }

        [JsonPropertyName("ChangeTeam")]
        public string? intermission { get; set; }

        [JsonPropertyName("TenSecond")]
        public string? TenSecond { get; set; }
        
        [JsonPropertyName("MVP")]
        public string? MVP { get; set; }

        [JsonPropertyName("StartRound")]
        public string? StartRound { get; set; }

        [JsonPropertyName("StartGame")]
        public string? StartGame { get; set; }

        [JsonPropertyName("KillSound")]
        public string? KillSound { get; set; }

        [JsonPropertyName("EndGame")]
        public string? EndGame { get; set; }

        [JsonPropertyName("TenSecondRound")]
        public string? TenSecondRound { get; set; }
    }

    class Music
    {
        public string? name { get; set; }
        public string? image { get; set; }
        public string? path { get; set; }
    }
}