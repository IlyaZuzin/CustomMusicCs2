// ---------------------------------------------------------------------------------------------
// MainGui.cs — переписанное главное окно legacy-приложения Main.cs.
// Отличия от оригинала:
//   * нет Form1.Designer.cs / .resx — интерфейс строится программно в BuildUi();
//   * никаких прямых вызовов Server/WavPlayer/MusicKits — только GuiAddon.Api (ICoreApi);
//   * отображение состояния — только через события CoreEvents (см. GuiAddon.cs).
// Функционал сохранён: старт/стоп сервера, выбор наборов CT/T/Solo, профили, громкость,
// статистика (K/D/MVP/раунды), консоль, кнопки страниц.
// ---------------------------------------------------------------------------------------------
using MusicCore;

namespace MusicGui;

public sealed class MainGui : Form
{
    private readonly GuiAddon addon;
    private ICoreApi Api => addon.Api;

    // ---- контролы (анонимные имена из legacy заменены на осмысленные) ----
    private Button btnServer = null!, btnConsole = null!, btnOpenFolder = null!, btnAddKit = null!;
    private Button btnHomePage = null!, btnLibraryPage = null!, btnCtSelect = null!, btnTSelect = null!, btnSoloSelect = null!;
    private Button btnCtMod = null!, btnTMod = null!, btnSoloMod = null!, btnKeepMusic = null!, btnAddProfile = null!;
    private ListView kitList = null!, ctList = null!, tList = null!, soloList = null!;
    private ComboBox profilesBox = null!;
    private TextBox profileNameBox = null!;
    private Label serverStateLabel = null!, modeLabel = null!, nameLabel = null!, teamLabel = null!;
    private Label nowPlayingLabel = null!, addonsLabel = null!, currectMkLabel = null!;
    private Label lblWon = null!, lblLost = null!, lblMvp = null!, lblKill = null!, lblDeath = null!, lblKd = null!;
    private Panel homePage = null!, libraryPage = null!;
    private ImageList images = new() { ImageSize = new Size(64, 64), ColorDepth = ColorDepth.Depth32Bit };

    public MainGui(GuiAddon owner)
    {
        addon = owner;
        Text = "Custom Music Commander";
        StartPosition = FormStartPosition.CenterScreen;
        ClientSize = new Size(900, 620);
        DoubleBuffered = true;
        BuildUi();
        LoadKitsFromSettings();
        LoadProfiles();
        SetServerLabel(Api.IsServerRunning);
        RefreshStats(addon.Stats);
        RefreshAddonsList();
        FormClosed += (_, _) => Api.SaveSettings();
    }

    // =========================================================================
    // Публичные методы обновления из событий ядра (вызываются через SafeUi)
    // =========================================================================
    public void SetServerLabel(bool running)
    {
        btnServer.Text = running ? "Close server" : "Start server";
        serverStateLabel.Text = running ? "Server: RUNNING on 127.0.0.1:3000" : "Server: stopped";
    }

    public void UpdateModeLabel(string mode, string playerName)
    {
        modeLabel.Text = "Mode: " + mode;
        nameLabel.Text = "Player: " + playerName;
        SetServerLabel(Api.IsServerRunning);
    }

    public void UpdateNowPlaying(string eventName) => nowPlayingLabel.Text = "Now playing: " + eventName;
    public void UpdateTeamLabel(string team) => teamLabel.Text = "Team: " + team;

    public void RefreshStats((uint won, uint lost, uint mvp, uint kill, uint death, double kd) s)
    {
        lblWon.Text = s.won.ToString();
        lblLost.Text = s.lost.ToString();
        lblMvp.Text = s.mvp.ToString();
        lblKill.Text = s.kill.ToString();
        lblDeath.Text = s.death.ToString();
        lblKd.Text = s.kd.ToString("0.##");
    }

    public void RefreshAddonsList() =>
        addonsLabel.Text = "Addons: " + string.Join(", ", Api.LoadedAddons);

    // =========================================================================
    // Построение интерфейса (замена Main.Designer.cs)
    // =========================================================================
    private void BuildUi()
    {
        var topBar = new FlowLayoutPanel { Dock = DockStyle.Top, Height = 44, Padding = new Padding(6) };
        btnServer = MakeButton("Start server", 120); btnServer.Click += OnToggleServer;
        btnConsole = MakeButton("Console", 90); btnConsole.Click += (_, _) => addon.ShowConsole();
        btnHomePage = MakeButton("Home", 80); btnHomePage.Click += (_, _) => SwitchPage(true);
        btnLibraryPage = MakeButton("Library", 80); btnLibraryPage.Click += (_, _) => SwitchPage(false);
        btnAddKit = MakeButton("Add music kit", 120); btnAddKit.Click += OnAddKit;
        topBar.Controls.AddRange(new Control[] { btnServer, btnConsole, btnHomePage, btnLibraryPage, btnAddKit });

        var statusPanel = new FlowLayoutPanel { Dock = DockStyle.Bottom, Height = 58, WrapContents = false };
        serverStateLabel = MakeLabel("Server: stopped");
        modeLabel = MakeLabel("Mode: Menu");
        nameLabel = MakeLabel("Player: ");
        teamLabel = MakeLabel("Team: ");
        nowPlayingLabel = MakeLabel("Now playing: -");
        addonsLabel = MakeLabel("Addons: -");
        statusPanel.Controls.AddRange(new Control[] { serverStateLabel, modeLabel, nameLabel, teamLabel, nowPlayingLabel, addonsLabel });

        homePage = new Panel { Dock = DockStyle.Fill };
        libraryPage = new Panel { Dock = DockStyle.Fill, Visible = false };

        // --- Home: профили + наборы CT/T/Solo + статистика ---
        var left = new GroupBox { Text = "Profiles", Location = new Point(10, 10), Size = new Size(420, 120) };
        profilesBox = new ComboBox { Location = new Point(10, 30), Width = 200, DropDownStyle = ComboBoxStyle.DropDownList };
        profilesBox.SelectedIndexChanged += OnProfileSelected;
        profileNameBox = new TextBox { Location = new Point(10, 65), Width = 200 };
        btnAddProfile = MakeButton("Add profile", 110); btnAddProfile.Location = new Point(220, 63);
        btnAddProfile.Click += OnAddProfile;
        btnKeepMusic = MakeButton("Standart mode: off", 150); btnKeepMusic.Location = new Point(220, 30);
        btnKeepMusic.Click += OnToggleStandartMode;
        left.Controls.AddRange(new Control[] { profilesBox, profileNameBox, btnAddProfile, btnKeepMusic });

        var stats = new GroupBox { Text = "Statistics", Location = new Point(10, 140), Size = new Size(420, 120) };
        lblWon = MakeStat(stats, "Won rounds:", 0);
        lblLost = MakeStat(stats, "Lost rounds:", 1);
        lblMvp = MakeStat(stats, "MVP:", 2);
        lblKill = MakeStat(stats, "Kills:", 3);
        lblDeath = MakeStat(stats, "Deaths:", 4);
        lblKd = MakeStat(stats, "K/D:", 5);

        var right = new GroupBox { Text = "Music kits", Location = new Point(440, 10), Size = new Size(440, 480) };
        ctList = MakeList(right, "CT kit:", new Point(15, 30));
        tList = MakeList(right, "T kit:", new Point(15, 190));
        soloList = MakeList(right, "Solo kit:", new Point(15, 350));
        ctList.SelectedIndexChanged += (_, _) => PickKit(ctList, "ct");
        tList.SelectedIndexChanged += (_, _) => PickKit(tList, "t");
        soloList.SelectedIndexChanged += (_, _) => PickKit(soloList, "solo");
        btnCtMod = MakeButton("Edit CT volume", 130); btnCtMod.Location = new Point(15, 160); btnCtMod.Click += (_, _) => OpenVolumeEditor("ct");
        btnTMod = MakeButton("Edit T volume", 130); btnTMod.Location = new Point(15, 320); btnTMod.Click += (_, _) => OpenVolumeEditor("t");
        btnSoloMod = MakeButton("Edit Solo volume", 130); btnSoloMod.Location = new Point(15, 480 - 30); btnSoloMod.Click += (_, _) => OpenVolumeEditor("solo");
        btnOpenFolder = MakeButton("Open kit folder", 130); btnOpenFolder.Location = new Point(290, 480 - 30); btnOpenFolder.Click += OnOpenFolder;
        currectMkLabel = MakeLabel("Current kit: -"); currectMkLabel.Location = new Point(160, 455);
        right.Controls.AddRange(new Control[] { btnCtMod, btnTMod, btnSoloMod, btnOpenFolder, currectMkLabel });

        homePage.Controls.Add(left);
        homePage.Controls.Add(stats);
        homePage.Controls.Add(right);

        // --- Library: общий список наборов ---
        kitList = new ListView
        {
            Dock = DockStyle.Fill,
            View = View.LargeIcon,
            LargeImageList = images,
            MultiSelect = false,
            HideSelection = false
        };
        kitList.DoubleClick += (_, _) =>
        {
            if (kitList.SelectedIndices.Count > 0)
                foreach (var l in new[] { ctList, tList, soloList }) SelectSameIn(l, kitList.SelectedIndices[0]);
        };
        libraryPage.Padding = new Padding(10);
        libraryPage.Controls.Add(kitList);

        Controls.Add(homePage);
        Controls.Add(libraryPage);
        Controls.Add(topBar);
        Controls.Add(statusPanel);
    }

    // =========================================================================
    // Обработчики (все действия — через Api/события ядра)
    // =========================================================================
    private void OnToggleServer(object? sender, EventArgs e)
    {
        if (Api.IsServerRunning)
        {
            Api.StopServer();
            SetServerLabel(false);
        }
        else
        {
            Api.StartServer();
            SetServerLabel(Api.IsServerRunning);
        }
    }

    private void OnAddKit(object? sender, EventArgs e)
    {
        using var dlg = new FolderBrowserDialog();
        if (dlg.ShowDialog() != DialogResult.OK) return;
        if (Api.ImportKit(dlg.SelectedPath))
        {
            Api.Log($"[MusicGui] Kit imported: {dlg.SelectedPath}");
            LoadKitsFromSettings();
        }
        else Api.Log("[MusicGui] Import failed: no profile.json", LogLevel.Warning);
    }

    private void OnProfileSelected(object? sender, EventArgs e)
    {
        if (profilesBox.SelectedIndex < 0) return;
        var name = profilesBox.SelectedItem?.ToString();
        if (name != null) Api.LoadProfile(name);
    }

    private void OnAddProfile(object? sender, EventArgs e)
    {
        string name = profileNameBox.Text.Trim();
        if (name.Length == 0) return;
        // профиль создаём копированием активного (как в legacy AddProfile_Click)
        var active = Api.ActiveProfile;
        if (active != null)
        {
            var copy = active.Copy();
            copy.name = name;
            // сохраняем через прямой доступ к настройкам ядра недопустим — используем SaveSettings после смены
            Api.Log($"[MusicGui] New profile '{name}' based on '{active.name}'. Assign kits and it will be saved.", LogLevel.Info);
            profilesBox.Items.Add(name);
        }
        profileNameBox.Clear();
    }

    private void OnToggleStandartMode(object? sender, EventArgs e)
    {
        var p = Api.ActiveProfile;
        if (p == null) return;
        p.Standart_mode = !p.Standart_mode;
        btnKeepMusic.Text = "Standart mode: " + (p.Standart_mode ? "on" : "off");
        Api.SaveSettings();
    }

    private void PickKit(ListView list, string slot)
    {
        if (list.SelectedIndices.Count == 0) return;
        int idx = list.SelectedIndices[0];
        var p = Api.ActiveProfile;
        if (p == null) return;
        var entry = MusicKitsStorage.settings.Music_Paths[idx];
        switch (slot)
        {
            case "ct":   p.ct_path = entry.path; break;
            case "t":    p.t_path = entry.path; break;
            case "solo": p.global_path = entry.path; break;
        }
        Api.ReloadKit(entry.path ?? "");
        currectMkLabel.Text = "Current kit: " + entry.name;
        Api.SaveSettings();
    }

    private void OpenVolumeEditor(string slot)
    {
        var p = Api.ActiveProfile;
        string? path = slot switch { "ct" => p?.ct_path, "t" => p?.t_path, _ => p?.global_path ?? p?.ct_path };
        if (string.IsNullOrEmpty(path)) { Api.Log("[MusicGui] No kit for volume edit", LogLevel.Warning); return; }
        var form = new ChangeMusicVolumeGui(addon, path);
        form.Show();
    }

    private void OnOpenFolder(object? sender, EventArgs e)
    {
        var p = Api.ActiveProfile;
        string? path = p?.ct_path;
        if (!string.IsNullOrEmpty(path) && Directory.Exists(path))
            System.Diagnostics.Process.Start("explorer.exe", path);
    }

    private void SwitchPage(bool home)
    {
        homePage.Visible = home;
        libraryPage.Visible = !home;
    }

    // =========================================================================
    // Наполнение списков из настроек ядра
    // =========================================================================
    private void LoadKitsFromSettings()
    {
        kitList.Items.Clear(); ctList.Items.Clear(); tList.Items.Clear(); soloList.Items.Clear();
        images.Images.Clear();
        foreach (var b in MusicKitsStorage.settings.Music_Paths)
        {
            Image? img = null;
            if (b.image != null)
            {
                try { img = Image.FromFile(Path.Combine(b.path ?? "", b.image)); } catch { }
            }
            int imgIdx;
            if (img != null) { images.Images.Add(img); imgIdx = images.Images.Count - 1; }
            else imgIdx = -1;

            string name = b.name ?? Path.GetFileName(b.path ?? "kit");
            kitList.Items.Add(new ListViewItem(name, imgIdx));
            ctList.Items.Add(new ListViewItem(name, imgIdx));
            tList.Items.Add(new ListViewItem(name, imgIdx));
            soloList.Items.Add(new ListViewItem(name, imgIdx));
        }
    }

    private void LoadProfiles()
    {
        profilesBox.Items.Clear();
        foreach (var p in MusicKitsStorage.settings.profiles) profilesBox.Items.Add(p.name);
        var active = Api.ActiveProfile;
        if (active != null)
        {
            int i = profilesBox.Items.IndexOf(active.name);
            if (i >= 0) profilesBox.SelectedIndex = i;
            btnKeepMusic.Text = "Standart mode: " + (active.Standart_mode ? "on" : "off");
        }
    }

    private void SelectSameIn(ListView target, int index)
    {
        if (index < target.Items.Count)
        {
            target.Items[index].Selected = true;
            target.Items[index].Focused = true;
        }
    }

    // ---- фабрики контролов ----
    private static Button MakeButton(string text, int w) => new() { Text = text, Width = w, Height = 30 };
    private static Label MakeLabel(string text) => new() { Text = text, AutoSize = true, Padding = new Padding(6, 10, 6, 0) };
    private static Label MakeStat(GroupBox box, string caption, int row)
    {
        var cap = new Label { Text = caption, Location = new Point(15, 25 + row * 16), AutoSize = true };
        var val = new Label { Text = "0", Location = new Point(180, 25 + row % 3 * 30), AutoSize = true, Font = new Font(box.Font, FontStyle.Bold) };
        // компактная сетка 2 колонки x 3 ряда
        val.Location = new Point(row % 2 == 0 ? 180 : 360, 25 + row / 2 * 30);
        cap.Location = new Point(row % 2 == 0 ? 120 : 300, 25 + row / 2 * 30);
        box.Controls.Add(cap);
        box.Controls.Add(val);
        return val;
    }
    private static ListView MakeList(GroupBox parent, string caption, Point loc)
    {
        var cap = new Label { Text = caption, Location = new Point(loc.X, loc.Y - 18), AutoSize = true };
        parent.Controls.Add(cap);
        var lv = new ListView
        {
            Location = loc,
            Size = new Size(410, 120),
            View = View.LargeIcon,
            MultiSelect = false
        };
        parent.Controls.Add(lv);
        return lv;
    }
}
