using System.IO;
using System.Reflection;
using System.Runtime.Loader;
using Files;
namespace Addons
{
    public class CollectibleAssemblyLoadContext : AssemblyLoadContext
    {
        public CollectibleAssemblyLoadContext() : base(isCollectible: true) 
        {
            // isCollectible: true — это самый важный флаг, разрешающий выгрузку
        }

        protected override Assembly? Load(AssemblyName assemblyName)
        {
            // Возвращаем null, чтобы системные зависимости (типа System.Runtime) 
            // загружались из основного приложения, а не дублировались в аддоне
            return null; 
        }

        public Assembly LoadFromAssemblyPathCustom(string path)
        {
            // Загружаем DLL через поток (FileStream)
            // Это гарантирует, что файл на диске НЕ БУДЕТ заблокирован ОС Windows,
            // и его можно будет перезаписать/удалить даже до вызова Unload()!
            using (var fs = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                return LoadFromStream(fs);
            }
        }
    }
    public static class AddonApi{
        private static Dictionary<string, Addon> addon_dict = new Dictionary<string, Addon>();
        public static void LoadAddon(string addonName)
        {
            string path = "/Addon/" + addonName + "/addon.dll";
            if (Path.Exists(path))
            {
                Addon addon = new Addon(path, addonName);
                addon_dict[addonName] = addon;
            }
        }

        public static void SendIvent(string ivents_name)
        {
            foreach(var addon in addon_dict.Values)
            {
                addon.SendIvent(ivents_name);
            }
        }

        public static void UnloadAddon(string addonName)
        {
            if(addon_dict.ContainsKey(addonName)){
                var addon = addon_dict[addonName];
                addon.CloseAddon();
                addon_dict.Remove(addonName);

                GC.Collect();
                GC.WaitForPendingFinalizers();
                FileSystem.Log($"[Core] Addon '{addonName}' fully unloaded from memory.");
            }
        } 
    }
    public class Addon
    {
        private Dictionary<string, MethodInfo> addonApi = new Dictionary<string, MethodInfo>();
        private string AddonName = " ";
        private CollectibleAssemblyLoadContext? loadContext;
        // private int addonId;
        public Addon(string dllpath, string AddonName)
        {
            this.AddonName = AddonName;
            try
            {
                loadContext = new CollectibleAssemblyLoadContext();
                Assembly addon = loadContext.LoadFromAssemblyPathCustom(dllpath);
                LoadFunc(addon);
                SendIvent("init");
            } catch(Exception ex) { FileSystem.Log(ex.Message);}
        }
        private void LoadFunc(Assembly addon)
        {
            try
            {
                Type? addonFunctions = addon.GetType("Addon.Api");
                if (addonFunctions == null) { return; }
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
                        FileSystem.Log("["+ AddonName +"]Not found func " + func + " in addon"); 
                        // status_ok = false // Инициализация не прошла. Не выходим сразу чтобы найти недостающие функции    
                    }
                    else {addonApi[func] = method;}
                }
            }
            catch (Exception ex) { FileSystem.Log(ex.Message); }
        }
        public void SendIvent(string ivent) { 
            if (!addonApi.ContainsKey(ivent))
            {
                FileSystem.Log($"[{AddonName}]Cannot invoke event '{ivent}': method not loaded or missing.");
                return;
            }
            try
            {
                addonApi[ivent].Invoke(null, null); 
            }
            catch (Exception ex)
            {
                FileSystem.Log($"{AddonName}Error executing event {ivent}: {ex.InnerException?.Message ?? ex.Message}");
            } 
        }   

        public void CloseAddon()
        {
            SendIvent("close");
            addonApi.Clear(); 
            if (loadContext != null)
            {
                loadContext.Unload();
                loadContext = null; 
            }
        }
    }
}
