
using System.Reflection.Metadata;
using System.Runtime.InteropServices;
using CoreAPI;
namespace MyNativeLib;
// TODO: Еще не все функции инициализированны
public static class NativeFunctions
{
    [UnmanagedCallersOnly(EntryPoint = "InitProgram")]
    public static bool InitProgram()
    {
        // Запускается только когда приложение запустилось
        // Запускаем работу приложения, проверяем все ли файлы загружены,
        // В противном случае просит путь к папке игры

        Core.init();
        return true;
    }

    [UnmanagedCallersOnly(EntryPoint = "LoadAddon")]
    public static bool LoadAddon()
    {
        return true; // Возращаем, подошел ли аддон и готов ли он к работе
    }

    [UnmanagedCallersOnly(EntryPoint = "Setup")]
    public static bool Setup()
    {
        // Срабатывает один раз после установки
        // Если все удачно возращаем true в ином false
        return Core.setup();
    }

    [UnmanagedCallersOnly(EntryPoint = "CloseProgram")]
    public static void CloseProgram()
    {
        // Ничего не возращает
        // Вызывает деструкторы основых методов
    }
}
