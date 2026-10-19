// ---------------------------------------------------------------------------------------------
// AddonManager.cs — менеджер аддонов ядра. Аддоны лежат в папке <ядро>/addons/*.dll.
// Каждый аддон загружается в ИЗОЛИРОВАНном AssemblyLoadContext (Collectible), что позволяет
// выгружать DLL по требованию (UnloadAddon) и грузить неограниченное количество аддонов.
// Контракт аддона: public static ICoreAddon Create() + реализация ICoreAddon (OnLoad/OnUnload).
// Интерфейсы (MusicCore.Api) всегда берутся из контекста ядра — сборка аддона переопределять
// их не может (ниже это реализовано через LoadAssemblyOverride-логику в CoreLoadContext).
// ---------------------------------------------------------------------------------------------
using System.Reflection;
using System.Runtime.Loader;
using MusicCore.Api;

namespace MusicCore.Addons;

/// <summary>Запись о загруженном аддоне.</summary>
internal sealed class LoadedAddon
{
    public string Name = "";
    public string Path = "";
    public AssemblyLoadContext? Context;
    public ICoreAddon? Instance;
}

public sealed class AddonManager
{
    private readonly Dictionary<string, LoadedAddon> addons = new(StringComparer.OrdinalIgnoreCase);
    private readonly object gate = new();
    private readonly string addonsDir;
    private readonly ICoreApi api;

    /// <summary>Имя файла конфигурации внутри папки ядра со списком автозагружаемых аддонов.</summary>
    private const string AutoLoadListFile = "addons.autoload.txt";

    public AddonManager(ICoreApi coreApi)
    {
        api = coreApi;
        addonsDir = Path.Combine(AppContext.BaseDirectory, "addons");
        Directory.CreateDirectory(addonsDir);
    }

    public IReadOnlyList<string> LoadedNames
    {
        get { lock (gate) return addons.Values.Select(a => a.Name).ToList(); }
    }

    /// <summary>
    /// Загрузить аддон. Аргумент — либо имя ("MusicGui"), либо путь к файлу
    /// ("addons/MusicGui.dll", полный или относительный). Повторная загрузка того же
    /// имени — no-op (аддон уже активен).
    /// </summary>
    public bool Load(string nameOrPath)
    {
        if (string.IsNullOrWhiteSpace(nameOrPath))
        {
            CoreEvents.WriteLog("[Addons] Load(): пустой аргумент");
            return false;
        }

        string dllPath = ResolvePath(nameOrPath);
        if (dllPath == null || !File.Exists(dllPath))
        {
            CoreEvents.WriteLog($"[Addons] Аддон не найден: {nameOrPath} (папка: {addonsDir})");
            return false;
        }

        lock (gate)
        {
            if (addons.ContainsKey(dllPath)) return true; // уже загружен

            var ctx = new CollectibleLoadContext(dllPath);
            try
            {
                var assembly = ctx.LoadFromAssemblyPath(dllPath);

                // 1) ищем статический метод Create()
                MethodInfo? factory = assembly.GetTypes()
                    .SelectMany(t => t.GetMethods(BindingFlags.Public | BindingFlags.Static))
                    .FirstOrDefault(m => m.Name == "Create" && m.ReturnType != typeof(void)
                                      && typeof(ICoreAddon).IsAssignableFrom(m.ReturnType)
                                      && m.GetParameters().Length == 0);

                ICoreAddon? instance = null;
                if (factory != null)
                {
                    instance = factory.Invoke(null, null) as ICoreAddon;
                }
                else
                {
                    // 2) fallback: тип с беспараметровым ctor, реализующий ICoreAddon
                    var type = assembly.GetTypes().FirstOrDefault(t =>
                        typeof(ICoreAddon).IsAssignableFrom(t) && !t.IsAbstract && t.GetConstructor(Type.EmptyTypes) != null);
                    if (type != null) instance = Activator.CreateInstance(type) as ICoreAddon;
                }

                if (instance == null)
                {
                    CoreEvents.WriteLog($"[Addons] В {Path.GetFileName(dllPath)} нет 'static ICoreAddon Create()' или ICoreAddon-типа");
                    ctx.Unload();
                    return false;
                }

                var record = new LoadedAddon
                {
                    Name = SafeName(instance.Name, dllPath),
                    Path = dllPath,
                    Context = ctx,
                    Instance = instance
                };

                instance.OnLoad(api);

                addons[dllPath] = record;
                CoreEvents.WriteLog($"[Addons] Загружен аддон '{record.Name}' ({Path.GetFileName(dllPath)})");
                CoreEvents.RaiseAddonLoaded(record.Name, dllPath);
                return true;
            }
            catch (Exception ex)
            {
                CoreEvents.WriteLog($"[Addons] Ошибка загрузки {nameOrPath}: {ex.Message}");
                try { ctx.Unload(); } catch { }
                return false;
            }
        }
    }

    /// <summary>Выгрузить аддон по имени или пути. Вызывает OnUnload, затем AssemblyLoadContext.Unload().</summary>
    public bool Unload(string nameOrPath)
    {
        lock (gate)
        {
            LoadedAddon? rec = addons.Values.FirstOrDefault(a =>
                string.Equals(a.Name, nameOrPath, StringComparison.OrdinalIgnoreCase) ||
                string.Equals(a.Path, Normalize(nameOrPath), StringComparison.OrdinalIgnoreCase) ||
                string.Equals(Path.GetFileNameWithoutExtension(a.Path), nameOrPath, StringComparison.OrdinalIgnoreCase));

            if (rec == null)
            {
                CoreEvents.WriteLog($"[Addons] Аддон не загружен: {nameOrPath}");
                return false;
            }

            try { rec.Instance?.OnUnload(); }
            catch (Exception ex) { CoreEvents.WriteLog($"[Addons] OnUnload('{rec.Name}') threw: {ex.Message}"); }

            addons.Remove(rec.Path);
            try { rec.Context?.Unload(); }
            catch (Exception ex) { CoreEvents.WriteLog($"[Addons] Context.Unload('{rec.Name}') failed: {ex.Message}"); }

            CoreEvents.WriteLog($"[Addons] Выгружен аддон '{rec.Name}'");
            CoreEvents.RaiseAddonUnloaded(rec.Name, rec.Path);
            return true;
        }
    }

    /// <summary>Выгрузить всё (вызывается из Close).</summary>
    public void UnloadAll()
    {
        lock (gate)
        {
            foreach (var path in addons.Keys.ToList()) Unload(path);
        }
    }

    /// <summary>Автозагрузка аддонов из addons.autoload.txt (по одному имени/пути на строку).</summary>
    public void AutoLoad()
    {
        string list = Path.Combine(AppContext.BaseDirectory, AutoLoadListFile);
        if (!File.Exists(list)) return;
        foreach (var raw in File.ReadAllLines(list))
        {
            var line = raw.Trim();
            if (line.Length == 0 || line.StartsWith('#')) continue;
            Load(line);
        }
    }

    // ------------------------------------------------------------------
    // Вспомогательное разрешение путей: «имя» -> addons/<имя>.dll
    // ------------------------------------------------------------------
    private string? ResolvePath(string nameOrPath)
    {
        // абсолютный путь к файлу
        if (Path.IsPathRooted(nameOrPath) && File.Exists(nameOrPath)) return Path.GetFullPath(nameOrPath);

        // относительно папки addons как к файлу
        string asFile = Path.GetFullPath(Path.Combine(addonsDir, nameOrPath));
        if (File.Exists(asFile)) return asFile;

        // как имя без расширения
        string asDll = Path.GetFullPath(Path.Combine(addonsDir, nameOrPath + ".dll"));
        if (File.Exists(asDll)) return asDll;

        // как имя с расширением, но без пути в корне addons
        string fileName = Path.GetFileName(nameOrPath);
        if (!fileName.EndsWith(".dll", StringComparison.OrdinalIgnoreCase)) fileName += ".dll";
        string byName = Path.GetFullPath(Path.Combine(addonsDir, fileName));
        if (File.Exists(byName)) return byName;

        return null;
    }

    private static string Normalize(string p) => Path.GetFullPath(p.Contains('/') || p.Contains('\\') ? p : Path.Combine(AppContext.BaseDirectory, "addons", p));

    private static string SafeName(string fromAddon, string dllPath) =>
        string.IsNullOrWhiteSpace(fromAddon) ? Path.GetFileNameWithoutExtension(dllPath) : fromAddon.Trim();
}

/// <summary>
/// Собираемый ALC: грузит только саму DLL аддона; общие типы (MusicCore.Api.*) резолвятся
/// из уже загруженной сборки ядра, чтобы не было дублей типов и кастования.
/// </summary>
internal sealed class CollectibleLoadContext : AssemblyLoadContext
{
    private readonly AssemblyDependencyResolver resolver;

    public CollectibleLoadContext(string pluginPath) : base(isCollectible: true)
    {
        resolver = new AssemblyDependencyResolver(pluginPath);
    }

    protected override Assembly? Load(AssemblyName assemblyName)
    {
        // никогда не перегружаем само ядро и его API
        var already = Default.Assemblies.FirstOrDefault(a => a.GetName().Name == assemblyName.Name);
        if (already != null) return null;

        string? path = resolver.ResolveAssemblyToPath(assemblyName);
        return path != null ? LoadFromAssemblyPath(path) : null;
    }
}
