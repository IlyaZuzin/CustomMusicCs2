// ---------------------------------------------------------------------------------------------
// ICoreApi.cs — контракт между ядром и аддонами.
//
// Каждый аддон (DLL в папке Core/addons) обязан:
//   1) экспортировать статический метод `public static ICoreAddon Create()` — ядро вызывает
//      его через рефлексию после загрузки сборки;
//   2) реализовать интерфейс ICoreAddon (OnLoad/OnUnload);
//   3) иметь свойство-метод `static string AddonName` (необязательно, для логов).
//
// Ядро передаёт аддону экземпляр ICoreApi — через него аддон дёргает функции ядра
// (проигрывание музыки, громкость, загрузка наборов, лог). События игровых состояний
// аддон получает через CoreEvents.* (подписка в OnLoad).
// ---------------------------------------------------------------------------------------------
namespace MusicCore.Api;

/// <summary>Аргументы события лога ядра.</summary>
public sealed class LogEventArgs : EventArgs
{
    public string Message { get; init; } = "";
    public DateTime Time { get; init; }
}

/// <summary>Логический уровень для Log()/SetLogLevel.</summary>
public enum LogLevel { Debug = 0, Info = 1, Warning = 2, Error = 3 }

/// <summary>
/// Публичный API ядра для аддонов. Экземпляр создаётся один раз внутри ядра
/// (класс CoreApi) и передаётся в ICoreAddon.OnLoad.
/// </summary>
public interface ICoreApi
{
    // ---------------- Аудио ----------------
    /// <summary>Проиграть трек, привязанный к событию (по текущему загруженному набору).</summary>
    void PlayMusic(MusicIvents ev);
    /// <summary>Остановить воспроизведение текущего трека.</summary>
    void StopMusic();
    /// <summary>Остановить конкретное событие, если оно играет.</summary>
    void StopMusic(MusicIvents ev);
    /// <summary>Универсальный вызов в стиле legacy IfEnable(ev, mode).</summary>
    void IfEnable(MusicIvents ev, ModeMusic mode);
    /// <summary>Громкость события 0..1 (применяется сразу, даже если сервер остановлен).</summary>
    void SetVolume(MusicIvents ev, float volume01);
    /// <summary>Текущая громкость события (0..1).</summary>
    float GetVolume(MusicIvents ev);
    /// <summary>Какое событие играет сейчас (null — тишина).</summary>
    MusicIvents? CurrentEvent { get; }
    /// <summary>Есть ли у текущего набора трек для события.</summary>
    bool HasTrack(MusicIvents ev);

    // ---------------- Музыкальные наборы ----------------
    /// <summary>Загрузить профиль пользователя из settings.json по имени (или null).</summary>
    bool LoadProfile(string profileName);
    /// <summary>Список имён всех сохранённых профилей.</summary>
    IReadOnlyList<string> Profiles { get; }
    /// <summary>Активный профиль (или null).</summary>
    Profile? ActiveProfile { get; }
    /// <summary>Перезагрузить музыкальный набор из profile.json по пути.</summary>
    bool ReloadKit(string kitPath);
    /// <summary>Добавить набор из папки в список настроек.</summary>
    bool ImportKit(string kitFolder);
    /// <summary>Сохранить setting.json.</summary>
    void SaveSettings();

    // ---------------- Состояние ----------------
    /// <summary>Запущен ли gamestate-сервер (игра «открыта»).</summary>
    bool IsServerRunning { get; }
    /// <summary>Текущий игровой режим.</summary>
    Mode CurrentMode { get; }
    /// <summary>Последний пакет PlayerStats или null.</summary>
    PlayerStats? LastStats { get; }
    /// <summary>Команда игрока ("CT"/"T"/null).</summary>
    string? PlayerTeam { get; }

    // ---------------- Сервер / игра ----------------
    /// <summary>Принудительно запустить HTTP-сервер gamestate.</summary>
    void StartServer();
    /// <summary>Остановить HTTP-сервер gamestate.</summary>
    void StopServer();

    // ---------------- Лог ----------------
    /// <summary>Записать сообщение в лог ядра (рассылается подписчикам CoreEvents.Log).</summary>
    void Log(string message, LogLevel level = LogLevel.Info);
    /// <summary>Включён ли подробный лог.</summary>
    bool LoggingEnabled { get; set; }

    // ---------------- Аддоны ----------------
    /// <summary>Имена всех загруженных аддонов.</summary>
    IReadOnlyList<string> LoadedAddons { get; }
}

/// <summary>
/// Контракт аддона. Ядро вызывает Create() через рефлексию, затем OnLoad(api),
/// а при выгрузке — OnUnload(). Все действия аддон выполняет только через api и CoreEvents.
/// </summary>
public interface ICoreAddon
{
    /// <summary>Читаемое имя аддона.</summary>
    string Name { get; }
    /// <summary>Вызывается ядром сразу после загрузки DLL. Здесь подписывайтесь на CoreEvents.</summary>
    void OnLoad(ICoreApi api);
    /// <summary>Вызывается ядром перед выгрузкой DLL. Здесь отписывайтесь от CoreEvents и освобождайте ресурсы.</summary>
    void OnUnload();
}
