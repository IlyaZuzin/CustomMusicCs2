using Files;
using System.Reflection;
using Microsoft.Win32;
using CS2Parser;
namespace CoreAPI
{
    public static class Core
    {
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
            FileSystem.CheckFiles();
            try {
                Server.start();
            } catch (Exception ex) { FileSystem.Log(ex.Message); }
        }
    }

    public class Addon
    {
        private Assembly addon;
        private Dictionary<string, MethodInfo> addonApi = new Dictionary<string, MethodInfo>();
        
        Addon(Assembly addon)
        {
            this.addon = addon;
        }
        public bool InitAddon()
        {
            bool status_ok = true;
            try
            {
                #nullable enable
                Type? addonFunctions = addon.GetType("Addon.Api");
                #nullable disable
                if (addonFunctions == null) { return false; }
                string[] API = new[]
                {
                    "init", "close", "openUi", "closeUi", "PlayerKill", "PlayerDead", 
                    "StartRound", "StartGame", "Menu", "BombPlanted", "BombDefused", 
                    "RoundEnd", "MVP", "WinRound", "LoseRound", "TenSecondBomb", 
                    "TenSecondRound", "Wingmans", "DeathMatch", "Competitive"
                };
                foreach (string func in API)
                {
                    #nullable enable
                    MethodInfo? method = addonFunctions.GetMethod(func,  BindingFlags.Public | BindingFlags.Static);
                    #nullable disable
                    if (method == null) { 
                        FileSystem.Log("Not found func " + func + " in addon"); 
                        // status_ok = false // Инициализация не прошла. Не выходим сразу чтобы найти недостающие функции    
                    }
                    else {addonApi[func] = method;}
                }
            }
            catch (Exception ex) { FileSystem.Log(ex.Message); }
            return status_ok;
        }
    }   
}
    