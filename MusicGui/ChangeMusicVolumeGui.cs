// ---------------------------------------------------------------------------------------------
// ChangeMusicVolumeGui.cs — переписанное окно регулировки громкости (legacy ChangeMusicVolume.cs).
// Ползунки 0..100 на каждое из 13 событий. Чтение/запись громкостей идут через ICoreApi
// (SetVolume/GetVolume) и profile.json набора; при закрытии изменения сохраняются в профиль
// набора (path перезаписи profile.json известна ядру через Api.ReloadKit, здесь читаем JSON напрямую).
// ---------------------------------------------------------------------------------------------
using System.Text.Json;
using MusicCore;

namespace MusicGui;

public sealed class ChangeMusicVolumeGui : Form
{
    private readonly GuiAddon addon;
    private readonly string kitPath;
    private Kit_Sound? kit;
    private readonly TrackControl[] controls = new TrackControl[EventInfo.Count];

    public ChangeMusicVolumeGui(GuiAddon owner, string pathToFolder)
    {
        addon = owner;
        kitPath = pathToFolder;
        Text = "Volume: " + Path.GetFileName(pathToFolder.TrimEnd('\\', '/'));
        ClientSize = new Size(560, 640);
        StartPosition = FormStartPosition.Manual;
        Location = new Point(200, 80);

        kit = MusicKitsStorage.LoadKit(kitPath);
        BuildUi();
        FormClosed += (_, _) => Save();
    }

    private void BuildUi()
    {
        var scroll = new Panel { Dock = DockStyle.Fill, AutoScroll = true };
        int y = 10;
        for (int i = 0; i < EventInfo.Count; i++)
        {
            var tc = new TrackControl((MusicIvents)i, addon.Api, kit, kitPath);
            tc.Location = new Point(10, y);
            scroll.Controls.Add(tc);
            controls[i] = tc;
            y += 74;
        }
        Controls.Add(scroll);
    }

    private void Save()
    {
        try
        {
            addon.Api.SaveSettings();
            addon.Api.Log($"[MusicGui] Volumes saved for kit {kitPath}");
        }
        catch (Exception ex) { addon.Api.Log("[MusicGui] Save failed: " + ex.Message, LogLevel.Error); }
    }
}

/// <summary>Строка: название события + ползунок громкости + кнопка «тест».</summary>
internal sealed class TrackControl : Panel
{
    private readonly MusicIvents ev;
    private readonly ICoreApi api;
    private readonly string jsonPath;
    private readonly Kit_Sound? kit;
    private Label caption = null!;
    private TrackBar bar = null!;
    private Button test = null!;

    public TrackControl(MusicIvents ev, ICoreApi api, Kit_Sound? kit, string kitDir)
    {
        this.ev = ev;
        this.api = api;
        this.kit = kit;
        jsonPath = Path.Combine(kitDir, "profile.json");
        Size = new Size(520, 68);

        caption = new Label { Text = ev.ToString(), Location = new Point(0, 0), AutoSize = true, Font = new Font("Segoe UI", 9f, FontStyle.Bold) };
        bar = new TrackBar { Location = new Point(0, 20), Width = 420, Minimum = 0, Maximum = 100, TickFrequency = 10 };
        test = new Button { Text = "▶", Location = new Point(430, 18), Width = 40 };
        test.Click += (_, _) => api.PlayMusic(ev);

        int vol = ReadProfileVolume();
        bar.Value = Math.Clamp(vol, 0, 100);
        api.SetVolume(ev, bar.Value / 100f); // применяем сразу (фикс legacy бага про Server.is_Running)
        bar.Scroll += (_, _) =>
        {
            api.SetVolume(ev, bar.Value / 100f);
            WriteProfileVolume(bar.Value);
        };

        Controls.Add(caption);
        Controls.Add(bar);
        Controls.Add(test);
    }

    private int ReadProfileVolume()
    {
        try
        {
            var list = kit?.music?[(int)ev];
            if (list == null || list.Count == 0) return 100;
            return list[0].volume;
        }
        catch { return 100; }
    }

    private void WriteProfileVolume(int value)
    {
        try
        {
            var list = kit?.music?[(int)ev];
            if (list == null) return;
            foreach (var f in list) f.volume = value;
            var options = new JsonSerializerOptions { WriteIndented = true };
            File.WriteAllText(jsonPath, JsonSerializer.Serialize(kit, options));
        }
        catch (Exception ex) { api.Log("[MusicGui] volume write: " + ex.Message, LogLevel.Warning); }
    }
}
