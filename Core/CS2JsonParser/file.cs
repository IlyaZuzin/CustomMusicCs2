using System.IO;
using System.Reflection;
using System.Text.Json;
namespace Files
{
    public static class FileSystem{
        public static void CopyFile(string p1, string p2)
        {
            try
            {
                if (!isValidPath(p1, p2))
                {
                    File.Copy(p1, p2, true);
                }
            }catch (Exception ex)
            {
                Log(ex.ToString());
            }
        }

        public static void Log(string text)
        {
            isExistsAndCreate("Log.txt");
            using (StreamWriter writer = new StreamWriter("log.txt"))
            {
                writer.WriteLine(MessageWithData(text));
            }
        }

        private static string MessageWithData(string text)
        {
            string currentTime = DateTime.Now.ToString("HH:mm:ss");
            currentTime += " " + text;
            return currentTime;
        }
        
        private static bool isValidPath(string p1, string p2)
        {
            return Path.GetFullPath(p1).Equals(Path.GetFullPath(p2), StringComparison.OrdinalIgnoreCase)  || !File.Exists(p1) || string.IsNullOrEmpty(p2);    
        }

        public static void CheckFiles()
        {
            if (isExistsAndCreate("settings.json"))
            {
                LoadSettings();
            }
        }
        private static bool isExistsAndCreate(string path, string? from_copy = null)
        {
            if (!File.Exists(path))
            {
                try
                {
                    if (from_copy == null)
                        File.Create(path);
                    else
                        CopyFile(from_copy, path);
                }
                catch (Exception ex) { Log(ex.Message); }
                return false;
            }
            else 
                return true;
        }

        private static void LoadSettings()
        {
            string OutPath = Directory.GetCurrentDirectory() + "\\setting.json";
            try
            {
                string json = File.ReadAllText(OutPath);
                Setting? settings = JsonSerializer.Deserialize<Setting>(json);
            }catch (Exception ex) { Log(ex.Message); } 
        }

        private static void SaveSettings()
        {
            
        }
    }

    public class Setting
    {
        
    }
}