using System.IO;

namespace MusicComanderGUI
{
    public partial class Main : Form
    {
        public static Main? Instance { get; private set; }
        public static string? Dm;
        public static TeamPath? Comp = new TeamPath();
        public static TeamPath? Public = new TeamPath();
        public static TeamPath? Wingmans = new TeamPath();
        public static string? main;
        public static Mode mode;
        public Main()
        {
            InitializeComponent();
            Instance = this;
            MusicKits.LoadSetting();
            MusicNumbers.SelectedIndexChanged += MusicNumbers_SelectedIndexChanged;
            Load();
            int newImageIndex = 0;
            if (MusicKits.settings?.Music_Paths != null)
            {
                for (int i = 0; i < MusicKits.settings.Music_Paths.Count - 1; i++)
                {
                    var b = MusicKits.settings.Music_Paths[i];

                    if (Path.Exists(b.path + "\\profile.json"))
                    {
                        try
                        {
                            if (Path.Exists(b.image))
                            {
                                imageList1.Images.Add(Image.FromFile(b.image));
                                newImageIndex = imageList1.Images.Count - 1;
                            }
                            else
                            {
                                newImageIndex++;
                            }
                            TSide.Items.Add(b.name);
                            CtSide.Items.Add(b.name);
                            MainMusic.Items.Add(b.name);

                            ListViewItem item = new ListViewItem(b.name, newImageIndex);
                            MusicNumbers.Items.Add(item);
                        }
                        catch { }
                    }
                    else
                    {
                        i--;
                        MusicKits.settings.Music_Paths.RemoveAt(i);
                    }
                }
            }
            if (MusicKits.settings?.Competitivie?.Ct != null && MusicKits.settings?.Competitivie?.Ct != -1)
            {
                int? index = MusicKits.settings?.Competitivie?.Ct;
                CtSide.SelectedIndex = index.Value;
                Comp?.Ct = MusicKits.settings?.Music_Paths[index.Value].path;
            }
            if (MusicKits.settings?.Competitivie?.T != null && MusicKits.settings?.Competitivie?.T != -1)
            {
                int? index = MusicKits.settings?.Competitivie?.T;
                TSide.SelectedIndex = index.Value;
                Comp?.T = MusicKits.settings?.Music_Paths[index.Value].path;
            }
            if (MusicKits.settings?.Public?.Ct != null && MusicKits.settings?.Public?.Ct != -1)
            {
                int? index = MusicKits.settings?.Public?.Ct;
                CtSide.SelectedIndex = index.Value;
                Public?.Ct = MusicKits.settings?.Music_Paths[index.Value].path;
            }
            if (MusicKits.settings?.Public?.T != null && MusicKits.settings?.Public?.T != -1)
            {
                int? index = MusicKits.settings?.Public?.T;
                TSide.SelectedIndex = index.Value;
                Public?.T = MusicKits.settings?.Music_Paths[index.Value].path;
            }
            if (MusicKits.settings?.Deathmatch != null && MusicKits.settings?.Deathmatch != -1)
            {
                int? index = MusicKits.settings?.Competitivie?.Ct;
                CtSide.SelectedIndex = index.Value;
                Dm = MusicKits.settings?.Music_Paths[index.Value].path;
            }

            MusicNumbers.LargeImageList = imageList1;
        }

        public void SetConsoleLog(string Log)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new Action(() => SetConsoleLog(Log)));
                return;
            }
            textBox5.AppendText(Log + Environment.NewLine);

        }

        private void StartServer_Click(object sender, EventArgs e)
        {
            if (!MusicComanderGUI.Server.is_Running)
            {
                MusicComanderGUI.Server.Start();
                if (MusicComanderGUI.Server.is_Running)
                {
                    StartServer.Text = "End";
                    pictureBox1.Image = imageList2.Images[0];
                }
            }
            else
            {
                MusicComanderGUI.Server.Stop();
                if (!MusicComanderGUI.Server.is_Running)
                {
                    StartServer.Text = "Start";
                    pictureBox1.Image = imageList2.Images[1];

                }
            }
        }

        private void LoadJson_Click(object sender, EventArgs e)
        {
            try
            {
                FolderBrowserDialog direct = new FolderBrowserDialog();
                if (direct.ShowDialog() == DialogResult.OK)
                {
                    Kit_Sound? load = MusicKits.AddMusicKit(direct.SelectedPath);
                    int size = MusicKits.settings.Music_Paths.Count() - 1;
                    string name = MusicKits.settings.Music_Paths[size].name;
                    if (load?.image == null)
                        imageList1.Images.Add(SystemIcons.Application.ToBitmap());
                    else
                        imageList1.Images.Add(Image.FromFile(load.image));

                    int newImageIndex = imageList1.Images.Count - 1;
                    ListViewItem item = new ListViewItem(name, newImageIndex);
                    MusicNumbers.Items.Add(item);
                    TSide.Items.Add(name);
                    CtSide.Items.Add(name);
                    MainMusic.Items.Add(name);
                }
            }
            catch (Exception ex) { SetConsoleLog(ex.Message); }
        }

        private void MusicNumbers_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (MusicNumbers.SelectedIndices.Count == 0)
                return;

            try
            {
                int index = MusicNumbers.SelectedIndices[0];
                Kit_Sound load = MusicKits.loadJson(MusicKits.settings.Music_Paths[index].path);
            }
            catch (Exception ex)
            {
                // Безопасный вывод ошибки без обращения к MusicKits.settings.Last
                SetConsoleLog($"Ошибка при выборе набора: {ex.Message}");
            }

        }

        private void CtSide_SelectedIndexChanged(object sender, EventArgs e)
        {
            int selectedIndex = CtSide.SelectedIndex;
            if (selectedIndex != -1)
            {
                switch (mode)
                {
                    case Mode.Competitive:
                        MusicKits.settings.Competitivie.Ct = selectedIndex;
                        Comp.Ct = MusicKits.settings.Music_Paths[selectedIndex].path;
                        break;
                    case Mode.Public:
                        MusicKits.settings.Public.Ct = selectedIndex;
                        Public.Ct = MusicKits.settings.Music_Paths[selectedIndex].path;
                        break;
                    case Mode.Deathmatch:
                        Dm = MusicKits.settings.Music_Paths[selectedIndex].path;
                        MusicKits.settings.Deathmatch = selectedIndex;
                        break;
                    case Mode.Wingmans:
                        Wingmans.Ct = MusicKits.settings.Music_Paths[selectedIndex].path;
                        MusicKits.settings.Wingmans.Ct = selectedIndex;
                        break;
                }
            }
        }

        private void TSide_SelectedIndexChanged(object sender, EventArgs e)
        {
            int selectedIndex = TSide.SelectedIndex;
            if (selectedIndex != -1)
            {
                switch (mode)
                {
                    case Mode.Competitive:
                        MusicKits.settings.Competitivie.T = selectedIndex;
                        Comp.T = MusicKits.settings.Music_Paths[selectedIndex].path;
                        break;
                    case Mode.Public:
                        MusicKits.settings.Public.T = selectedIndex;
                        Public.T = MusicKits.settings.Music_Paths[selectedIndex].path;
                        break;
                    case Mode.Wingmans:
                        Wingmans.T = MusicKits.settings.Music_Paths[selectedIndex].path;
                        MusicKits.settings.Wingmans.T = selectedIndex;
                        break;
                }
            }
        }

        private void Musickits_Click(object sender, EventArgs e)
        {
            MusicKitMenu.Visible = true;
            ServerMenu.Visible = false;
            SettingsMenu.Visible = false;
        }

        private void Server_Click(object sender, EventArgs e)
        {
            MusicKitMenu.Visible = false;
            ServerMenu.Visible = true;
            SettingsMenu.Visible = false;
        }

        private void OpenMusicKitForm_Click(object sender, EventArgs e)
        {
            Creater form = new Creater(this);
            form.ShowDialog();
        }

        private void Cs2Directory_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog direct = new FolderBrowserDialog();
            if (direct.ShowDialog() == DialogResult.OK)
            {
                string path = direct.SelectedPath;
                path += "\\csgo\\cfg\\";
                SetConsoleLog(MusicKits.CopyFileByFileSave("MusicKit.cfg", path, "gamestate_integration_mymusic.cfg"));
            }
        }

        private void SaveSettings_Click(object sender, EventArgs e)
        {
            MusicKits.SaveSetting();
        }


        private void DoubleMode_CheckedChanged(object sender, EventArgs e)
        {
            MusicKits.settings?.DoubleMode = DoubleMode.Checked;
        }

        private void Settings_Click(object sender, EventArgs e)
        {
            MusicKitMenu.Visible = true;
            ServerMenu.Visible = false;
            SettingsMenu.Visible = true;
        }

        private void Mode_SelectedIndexChanged(object sender, EventArgs e)
        {
            int selectedIndex = Mode_select.SelectedIndex + 1;
            switch (selectedIndex)
            {
                case (int)Mode.Competitive:
                    mode = Mode.Competitive;
                    if (MusicKits.settings?.Competitivie?.Ct != null && MusicKits.settings?.Competitivie?.Ct != -1)
                    {
                        int? index = MusicKits.settings?.Competitivie?.Ct;
                        CtSide.SelectedIndex = index.Value;
                        Comp?.Ct = MusicKits.settings?.Music_Paths[index.Value].path;
                    }
                    if (MusicKits.settings?.Competitivie?.T != null && MusicKits.settings?.Competitivie?.T != -1)
                    {
                        int? index = MusicKits.settings?.Competitivie?.T;
                        TSide.SelectedIndex = index.Value;
                        Comp?.T = MusicKits.settings?.Music_Paths[index.Value].path;
                    }
                    DeathMatch.Visible = true;
                    break;
                case (int)Mode.Public:
                    mode = Mode.Public;
                    if (MusicKits.settings?.Public?.Ct != null && MusicKits.settings?.Public?.Ct != -1)
                    {
                        int? index = MusicKits.settings?.Public?.Ct;
                        CtSide.SelectedIndex = index.Value;
                        Public?.Ct = MusicKits.settings?.Music_Paths[index.Value].path;
                    }
                    if (MusicKits.settings?.Public?.T != null && MusicKits.settings?.Public?.T != -1)
                    {
                        int? index = MusicKits.settings?.Public?.T;
                        TSide.SelectedIndex = index.Value;
                        Public?.T = MusicKits.settings?.Music_Paths[index.Value].path;
                    }
                    DeathMatch.Visible = true;
                    break;
                case (int)Mode.Deathmatch:
                    mode = Mode.Deathmatch;
                    if (MusicKits.settings?.Deathmatch != null && MusicKits.settings?.Deathmatch != -1)
                    {
                        int? index = MusicKits.settings?.Competitivie?.Ct;
                        CtSide.SelectedIndex = index.Value;
                        Dm = MusicKits.settings?.Music_Paths[index.Value].path;
                    }
                    DeathMatch.Visible = false;
                    break;
                case (int)Mode.Wingmans:
                    mode = Mode.Deathmatch;
                    if (MusicKits.settings?.Wingmans?.Ct != null && MusicKits.settings?.Wingmans?.Ct != -1)
                    {
                        int? index = MusicKits.settings?.Public?.Ct;
                        CtSide.SelectedIndex = index.Value;
                        Wingmans?.Ct = MusicKits.settings?.Music_Paths[index.Value].path;
                    }
                    if (MusicKits.settings?.Wingmans?.T != null && MusicKits.settings?.Wingmans?.T != -1)
                    {
                        int? index = MusicKits.settings?.Public?.T;
                        TSide.SelectedIndex = index.Value;
                        Wingmans?.T = MusicKits.settings?.Music_Paths[index.Value].path;
                    }
                    DeathMatch.Visible = true;
                    break;
            }
        }

        private void StartGameVolume_Scroll(object sender, ScrollEventArgs e)
        {
            StartGameVolumeText.Text = StartGameVolume.Value.ToString();
            MusicKits.settings.Volume[(int)MusicIvents.StartGame] = StartGameVolume.Value;
            WavPlayer.changevolume(MusicIvents.StartGame, StartGameVolume.Value);
        }

        private void MenuVolume_Scroll(object sender, ScrollEventArgs e)
        {
            MenuVolumeText.Text = MenuVolume.Value.ToString();
            MusicKits.settings.Volume[(int)MusicIvents.Menu] = MenuVolume.Value;
            WavPlayer.changevolume(MusicIvents.Menu, MenuVolume.Value);
        }

        private void StartRoundVolume_Scroll(object sender, ScrollEventArgs e)
        {
            StartRoundVolumeText.Text = StartRoundVolume.Value.ToString();
            MusicKits.settings.Volume[(int)MusicIvents.StartRound] = StartRoundVolume.Value;
            WavPlayer.changevolume(MusicIvents.StartRound, StartRoundVolume.Value);
        }

        private void StartActionVolume_Scroll(object sender, ScrollEventArgs e)
        {
            StartActionVolumeText.Text = StartActionVolume.Value.ToString();
            MusicKits.settings.Volume[(int)MusicIvents.StartAction] = StartActionVolume.Value;
            WavPlayer.changevolume(MusicIvents.StartAction, StartActionVolume.Value);
        }

        private void WinRoundVolume_Scroll(object sender, ScrollEventArgs e)
        {
            WinRoundVolumeText.Text = WinRoundVolume.Value.ToString();
            MusicKits.settings.Volume[(int)MusicIvents.WinRound] = WinRoundVolume.Value;
            WavPlayer.changevolume(MusicIvents.WinRound, WinRoundVolume.Value);
        }

        private void LoseRoundVolume_Scroll(object sender, ScrollEventArgs e)
        {
            LoseRoundVolumeText.Text = LoseRoundVolume.Value.ToString();
            MusicKits.settings.Volume[(int)MusicIvents.LoseRound] = LoseRoundVolume.Value;
            WavPlayer.changevolume(MusicIvents.LoseRound, LoseRoundVolume.Value);
        }

        private void MvpVolume_Scroll(object sender, ScrollEventArgs e)
        {
            MvpVolumeText.Text = MvpVolume.Value.ToString();
            MusicKits.settings.Volume[(int)MusicIvents.Mvp] = MvpVolume.Value;
            WavPlayer.changevolume(MusicIvents.Mvp, MvpVolume.Value);
        }

        private void BombVolume_Scroll(object sender, ScrollEventArgs e)
        {
            BombVolumeText.Text = BombVolume.Value.ToString();
            MusicKits.settings.Volume[(int)MusicIvents.Bomb] = BombVolume.Value;
            WavPlayer.changevolume(MusicIvents.Bomb, BombVolume.Value);
        }

        private void TenSecondBombVolume_Scroll(object sender, ScrollEventArgs e)
        {
            TenSecondBombVolumeText.Text = TenSecondBombVolume.Value.ToString();
            MusicKits.settings.Volume[(int)MusicIvents.TenSecondBomb] = TenSecondBombVolume.Value;
            WavPlayer.changevolume(MusicIvents.TenSecondBomb, TenSecondBombVolume.Value);
        }

        private void TenSecondRoundVolume_Scroll(object sender, ScrollEventArgs e)
        {
            TenSecondRoundVolumeText.Text = TenSecondRoundVolume.Value.ToString();
            MusicKits.settings.Volume[(int)MusicIvents.TenSecondRound] = TenSecondRoundVolume.Value;
            WavPlayer.changevolume(MusicIvents.TenSecondRound, TenSecondRoundVolume.Value);
        }

        private void EndGameVolume_Scroll(object sender, ScrollEventArgs e)
        {
            EndGameVolumeText.Text = EndGameVolume.Value.ToString();
            MusicKits.settings.Volume[(int)MusicIvents.EndGame] = EndGameVolume.Value;
            WavPlayer.changevolume(MusicIvents.EndGame, EndGameVolume.Value);
        }

        private void KillVolume_Scroll(object sender, ScrollEventArgs e)
        {
            KillVolumeText.Text = KillVolume.Value.ToString();
            MusicKits.settings.Volume[(int)MusicIvents.KillSound] = KillVolume.Value;
            WavPlayer.changevolume(MusicIvents.KillSound, KillVolume.Value);
        }

        private void DeathcamVolume_Scroll(object sender, ScrollEventArgs e)
        {
            DeathcamVolumeText.Text = DeathcamVolume.Value.ToString();
            MusicKits.settings.Volume[(int)MusicIvents.DeathCam] = DeathcamVolume.Value;
            WavPlayer.changevolume(MusicIvents.DeathCam, DeathcamVolume.Value);
        }

        private void Load()
        {
            StartGameVolume.Value = MusicKits.settings.Volume[(int)MusicIvents.StartGame];
            StartGameVolumeText.Text = MusicKits.settings.Volume[(int)MusicIvents.StartGame].ToString();

            MenuVolume.Value = MusicKits.settings.Volume[(int)MusicIvents.Menu];
            MenuVolumeText.Text = MusicKits.settings.Volume[(int)MusicIvents.Menu].ToString();

            StartRoundVolume.Value = MusicKits.settings.Volume[(int)MusicIvents.StartRound];
            StartRoundVolumeText.Text = MusicKits.settings.Volume[(int)MusicIvents.StartRound].ToString();

            StartActionVolume.Value = MusicKits.settings.Volume[(int)MusicIvents.StartAction];
            StartActionVolumeText.Text = MusicKits.settings.Volume[(int)MusicIvents.StartAction].ToString();

            WinRoundVolume.Value = MusicKits.settings.Volume[(int)MusicIvents.WinRound];
            WinRoundVolumeText.Text = MusicKits.settings.Volume[(int)MusicIvents.WinRound].ToString();

            LoseRoundVolume.Value = MusicKits.settings.Volume[(int)MusicIvents.LoseRound];
            LoseRoundVolumeText.Text = MusicKits.settings.Volume[(int)MusicIvents.LoseRound].ToString();

            MvpVolume.Value = MusicKits.settings.Volume[(int)MusicIvents.Mvp];
            MvpVolumeText.Text = MusicKits.settings.Volume[(int)MusicIvents.Mvp].ToString();

            BombVolume.Value = MusicKits.settings.Volume[(int)MusicIvents.Bomb];
            BombVolumeText.Text = MusicKits.settings.Volume[(int)MusicIvents.Bomb].ToString();

            TenSecondBombVolume.Value = MusicKits.settings.Volume[(int)MusicIvents.TenSecondBomb];
            TenSecondBombVolumeText.Text = MusicKits.settings.Volume[(int)MusicIvents.TenSecondBomb].ToString();

            TenSecondRoundVolume.Value = MusicKits.settings.Volume[(int)MusicIvents.TenSecondRound];
            TenSecondRoundVolumeText.Text = MusicKits.settings.Volume[(int)MusicIvents.TenSecondRound].ToString();

            EndGameVolume.Value = MusicKits.settings.Volume[(int)MusicIvents.EndGame];
            EndGameVolumeText.Text = MusicKits.settings.Volume[(int)MusicIvents.EndGame].ToString();

            KillVolume.Value = MusicKits.settings.Volume[(int)MusicIvents.KillSound];
            KillVolumeText.Text = MusicKits.settings.Volume[(int)MusicIvents.KillSound].ToString();

            DeathcamVolume.Value = MusicKits.settings.Volume[(int)MusicIvents.DeathCam];
            DeathcamVolumeText.Text = MusicKits.settings.Volume[(int)MusicIvents.DeathCam].ToString();
        }

        private void MainMusic_SelectedIndexChanged(object sender, EventArgs e)
        {
            int selectedIndex = MainMusic.SelectedIndex;
            main = MusicKits.settings.Music_Paths[selectedIndex].path;
        }

        private void SettingsMenu_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
