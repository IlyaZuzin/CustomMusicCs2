using serialization;
using Files;
using System.IO;
namespace core_settings
{
    public class Setting
    {
        public Settings_Api settings_Api = new Settings_Api();
        public void Load()
        {
            string OutPath = Directory.GetCurrentDirectory() + "\\setting.json";
            try
            {
                settings_Api = Serialize.JsonSetting(OutPath);
            }catch (Exception ex) { FileSystem.Log(ex.Message); } 
        }
    }
    public class Settings_Api
    {
        public List<string> addons = new List<string>();
    }
}