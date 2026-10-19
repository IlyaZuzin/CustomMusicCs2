// ---------------------------------------------------------------------------------------------
// NativeExports.cs — «нативный» слой ядра: 9 экспортируемых функций.
//
//   void Setup();
//   void Inject();
//   void GameOpen();
//   void GameMinimaze();
//   void GameMaximaze();
//   void GameClose();
//   void Close();
//   void LoadAddon(char* nameOrPath);    // ANSI-строка: имя аддона или путь к DLL
//   void UnloadAddon(char* nameOrPath);
//
// Для вызова из C/C++/C#/Python:
//   [DllImport("Core.dll", CallingConvention = CallingConvention.Cdecl)] ...
// Строковые параметры принимаются как const char* (Marshal.PtrToStringAnsi). Ядро само
// находит аддоны в подпапке "addons" рядом со своей DLL, поэтому достаточно имени ("MusicGui").
// ---------------------------------------------------------------------------------------------
using System.Runtime.InteropServices;

namespace MusicCore;

public static class NativeExports
{
    /// <summary>1. Инициализация ядра (настройки, профили, аудио-движок).</summary>
    [UnmanagedCallersOnly(EntryPoint = "Setup", CallConvs = new[] { typeof(CallConvCdecl) })]
    public static void Setup() => Safe(() => Core.Setup());

    /// <summary>2. «Внедрение»: запуск gamestate-сервера, монитора игры и автозагрузка аддонов.</summary>
    [UnmanagedCallersOnly(EntryPoint = "Inject", CallConvs = new[] { typeof(CallConvCdecl) })]
    public static void Inject() => Safe(() => Core.Inject());

    /// <summary>3. Игра открыта/запущена.</summary>
    [UnmanagedCallersOnly(EntryPoint = "GameOpen", CallConvs = new[] { typeof(CallConvCdecl) })]
    public static void GameOpen() => Safe(() => Core.GameOpen());

    /// <summary>4. Игра свёрнута.</summary>
    [UnmanagedCallersOnly(EntryPoint = "GameMinimaze", CallConvs = new[] { typeof(CallConvCdecl) })]
    public static void GameMinimaze() => Safe(() => Core.GameMinimaze());

    /// <summary>5. Игра восстановлена из свёрнутого состояния.</summary>
    [UnmanagedCallersOnly(EntryPoint = "GameMaximaze", CallConvs = new[] { typeof(CallConvCdecl) })]
    public static void GameMaximaze() => Safe(() => Core.GameMaximaze());

    /// <summary>6. Игра закрыта.</summary>
    [UnmanagedCallersOnly(EntryPoint = "GameClose", CallConvs = new[] { typeof(CallConvCdecl) })]
    public static void GameClose() => Safe(() => Core.GameClose());

    /// <summary>7. Полное завершение работы ядра (выгрузка аддонов, сохранение настроек).</summary>
    [UnmanagedCallersOnly(EntryPoint = "Close", CallConvs = new[] { typeof(CallConvCdecl) })]
    public static void Close() => Safe(() => Core.Close());

    /// <summary>8. Загрузить аддон по имени или пути (char*).</summary>
    [UnmanagedCallersOnly(EntryPoint = "LoadAddon", CallConvs = new[] { typeof(CallConvCdecl) })]
    public static void LoadAddon(IntPtr nameOrPath) => Safe(() =>
    {
        string s = Marshal.PtrToStringAnsi(nameOrPath) ?? "";
        Core.LoadAddon(s);
    });

    /// <summary>9. Выгрузить аддон по имени или пути (char*).</summary>
    [UnmanagedCallersOnly(EntryPoint = "UnloadAddon", CallConvs = new[] { typeof(CallConvCdecl) })]
    public static void UnloadAddon(IntPtr nameOrPath) => Safe(() =>
    {
        string s = Marshal.PtrToStringAnsi(nameOrPath) ?? "";
        Core.UnloadAddon(s);
    });

    /// <summary>Ни одна нативная функция не должна «выбрасывать» исключение за границу ABI.</summary>
    private static void Safe(Action action)
    {
        try { action(); }
        catch (Exception ex)
        {
            try { CoreEvents.WriteLog("[Native] " + ex.Message); } catch { }
        }
    }
}
