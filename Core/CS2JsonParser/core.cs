using Files;
using System.Reflection;
using Microsoft.Win32;
using CS2Parser;
using Addons;
using core_settings;
namespace CoreAPI
{
    public static class Core
    {
        private static Setting setting = new Setting();
        private static List<Addon> addons = new List<Addon>();
        public static bool setup()
        {
            var dialog = new OpenFolderDialog
            {
                Title = "Выберите папку CS2",
                InitialDirectory = @"C:\"
            };
            if (dialog.ShowDialog() == true)
            {
                string selectedFolder = dialog.FolderName;
                string configPath = "\\csgo\\cfg\\gamestate_integration_Cs2core.cfg";
                selectedFolder += configPath;
                try { FileSystem.CopyFile("/settings/config.cfg", selectedFolder); }
                catch(Exception ex) { FileSystem.Log(ex.Message); }
                return true;
            }
            return false;
            
        }

        public static void init()
        {
            setting.Load();
            try {
                Server.start();
                foreach (var addon in setting.settings_Api.addons) 
                    AddonApi.LoadAddon(addon);
            } catch (Exception ex) { FileSystem.Log(ex.Message); }
        }
    }
}
    