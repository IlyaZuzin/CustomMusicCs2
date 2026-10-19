// ---------------------------------------------------------------------------------------------
// MusicKitsStorage.cs — порт legacy-класса MusicKits. Работа с setting.json (список наборов +
// профили) и profile.json внутри папок наборов. Пути разрешаются относительно папки набора,
// поэтому при загрузке profile.json мы выставляем рабочую директорию на папку кита (как в legacy).
// ---------------------------------------------------------------------------------------------
using System.Text.Json;
using System.Text.Json.Serialization;

namespace MusicCore;

public class Setting
{
    [JsonPropertyName("Music_Paths")] public List<KitEntry> Music_Paths { get; set; } = new();
    [JsonPropertyName("LastProfile")] public int? profile { get; set; }
    [JsonPropertyName("Profiles")]    public List<Profile> profiles { get; set; } = new();
}

/// <summary>Запись реестра наборов: имя/картинка/путь папки.</summary>
public class KitEntry
{
    public string? name { get; set; }
    public string? image { get; set; }
    public string? path { get; set; }
}

public static class MusicKitsStorage
{
    public static Setting settings = new();
    private static string settingPath = Path.Combine(AppContext.BaseDirectory, "setting.json");

    public static void LoadSetting()
    {
        try
        {
            string json = File.ReadAllText(settingPath);
            settings = JsonSerializer.Deserialize<Setting>(json) ?? new Setting();
        }
        catch (FileNotFoundException) { SaveSetting(); }
        catch (JsonException) { settings = new Setting(); SaveSetting(); }

        settings.Music_Paths ??= new();
        settings.profiles ??= new();
    }

    public static void SaveSetting()
    {
        var options = new JsonSerializerOptions { WriteIndented = true };
        File.WriteAllText(settingPath, JsonSerializer.Serialize(settings, options));
    }

    /// <summary>Загрузить profile.json из папки набора (путь может быть к папке или к файлу).</summary>
    public static Kit_Sound? LoadKit(string? folderOrFile)
    {
        if (string.IsNullOrEmpty(folderOrFile)) return null;
        string jsonPath = Directory.Exists(folderOrFile)
            ? Path.Combine(folderOrFile, "profile.json")
            : folderOrFile;
        string kitDir = Path.GetDirectoryName(jsonPath) ?? AppContext.BaseDirectory;

        var prev = Directory.GetCurrentDirectory();
        try
        {
            Directory.SetCurrentDirectory(kitDir); // относительные пути wav inside profile.json
            string json = File.ReadAllText(jsonPath);
            return JsonSerializer.Deserialize<Kit_Sound>(json);
        }
        catch (Exception ex)
        {
            CoreEvents.WriteLog($"LoadKit error ({jsonPath}): {ex.Message}");
            return null;
        }
        finally
        {
            try { Directory.SetCurrentDirectory(prev); } catch { }
        }
    }

    /// <summary>Добавить набор из папки в реестр настроек.</summary>
    public static bool ImportKit(string folder)
    {
        var kit = LoadKit(folder);
        if (kit == null) return false;
        settings.Music_Paths.Add(new KitEntry { name = kit.name, image = kit.image, path = folder });
        SaveSetting();
        return true;
    }

    /// <summary>Создать новый набор: копирование файлов + profile.json в папку.</summary>
    public static string CreateMusicKit(string parentPath, string name, Kit_Sound music)
    {
        string newDir = Path.Combine(parentPath, name);
        Directory.CreateDirectory(newDir);
        CopyFiles(newDir, music);
        var options = new JsonSerializerOptions { WriteIndented = true };
        File.WriteAllText(Path.Combine(newDir, "profile.json"), JsonSerializer.Serialize(music, options));
        settings.Music_Paths.Add(new KitEntry { name = music.name, image = music.image, path = newDir });
        SaveSetting();
        return newDir;
    }

    private static void CopyFiles(string dir, Kit_Sound music)
    {
        for (int ev = 0; ev < EventInfo.Count; ev++)
        {
            var list = music.music?[ev];
            if (list == null) continue;
            for (int c = 0; c < list.Count; c++)
            {
                string fileName = $"{(MusicIvents)ev} {c}.wav";
                list[c].path = CopyFile(list[c].path, dir, fileName);
            }
        }
        music.image = CopyFile(music.image, dir, "Image.png");
    }

    public static string? CopyFile(string? source, string destDir, string fileName)
    {
        if (string.IsNullOrEmpty(source) || !File.Exists(source)) return null;
        try
        {
            string dest = Path.Combine(destDir, fileName);
            if (Path.GetFullPath(source).Equals(Path.GetFullPath(dest), StringComparison.OrdinalIgnoreCase))
                return source;
            File.Copy(source, dest, true);
            return fileName;
        }
        catch (Exception ex)
        {
            CoreEvents.WriteLog($"Ошибка при копировании {fileName}: {ex}");
            return null;
        }
    }
}
