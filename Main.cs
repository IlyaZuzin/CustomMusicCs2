
using Microsoft.VisualBasic.Logging;
using System.IO;
using System.Windows.Documents;

namespace MusicComanderGUI
{
    public partial class Main : Form
    {
        public static Main? Instance { get; private set; }
        public static string? Dm;
        public static TeamPath? Comp = new TeamPath();
        public static TeamPath? Public = new TeamPath();
        public static Mode mode;
        public Main()
        {
            InitializeComponent();
            Instance = this;
            MusicKits.LoadSetting();
            MusicNumbers.SelectedIndexChanged += MusicNumbers_SelectedIndexChanged;
            int? volume = MusicKits.settings?.Volume;

            int newImageIndex = 0;
            if (MusicKits.settings?.Music_Paths != null)
            {
                foreach (var b in MusicKits.settings?.Music_Paths)
                {
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
                            ListViewItem item = new ListViewItem(b.name, newImageIndex);
                            MusicNumbers.Items.Add(item);

                        }
                        catch { }
                    }
                    else
                    {
                        SetConsoleLog($"Not found profile {b.name}");
                    }
                }
            }
            if (MusicKits.settings?.Competitivie?.Ct != null && MusicKits.settings?.Competitivie?.Ct != -1)
            {
                int? index = MusicKits.settings?.Competitivie?.Ct;
                CtSide.SelectedIndex = index.Value;
                Comp.Ct = MusicKits.settings.Music_Paths[index.Value].path;
            }
            if (MusicKits.settings?.Competitivie?.T != null && MusicKits.settings?.Competitivie?.T != -1)
            {
                int? index = MusicKits.settings?.Competitivie?.T;
                TSide.SelectedIndex = index.Value;
                Comp.T = MusicKits.settings.Music_Paths[index.Value].path;

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

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            textBox7.Text = trackBar1.Value.ToString();
            MusicKits.settings?.Volume = trackBar1.Value;
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
                        CtSelectMusic(Comp.Ct);
                        break;
                    case Mode.Public:
                        MusicKits.settings.Public.Ct = selectedIndex;
                        Public.Ct = MusicKits.settings.Music_Paths[selectedIndex].path;
                        CtSelectMusic(Public.Ct);
                        break;
                    case Mode.Deathmatch:
                        Dm = MusicKits.settings.Music_Paths[selectedIndex].path;
                        MusicKits.settings.Deathmatch = selectedIndex;
                        CtSelectMusic(Dm);
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
                        CtSelectMusic(Comp.T);
                        break;
                    case Mode.Public:
                        MusicKits.settings.Public.T = selectedIndex;
                        Public.T = MusicKits.settings.Music_Paths[selectedIndex].path;
                        CtSelectMusic(Public.T);
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

        private void ServerMenu_Paint(object sender, PaintEventArgs e)
        {

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

        private void LoseRound_TextChanged(object sender, EventArgs e)
        {

        }

        private void Competitive_Paint(object sender, PaintEventArgs e)
        {

        }

        private void textBox17_TextChanged(object sender, EventArgs e)
        {

        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void checkBox2_CheckedChanged_1(object sender, EventArgs e)
        {

        }

        private void CtSelectMusic(string path)
        {
            Kit_Sound? LoadedKit = MusicKits.loadJson(path);
            if (LoadedKit?.Musics[(int)MusicIvents.Bomb]?.volume != null)
                CtBombVolume.Value = LoadedKit.Musics[(int)MusicIvents.Bomb].volume;
            if (LoadedKit?.Musics[(int)MusicIvents.Mvp]?.volume != null)
                CtMvpVolume.Value = LoadedKit.Musics[(int)MusicIvents.Mvp].volume;
            if (LoadedKit?.Musics[(int)MusicIvents.WinRound]?.volume != null)
                CtWinRoundVolume.Value = LoadedKit.Musics[(int)MusicIvents.WinRound].volume;
            if (LoadedKit?.Musics[(int)MusicIvents.LoseRound]?.volume != null)
                CtLoseRoundVolume.Value = LoadedKit.Musics[(int)MusicIvents.LoseRound].volume;
            if (LoadedKit?.Musics[(int)MusicIvents.EndGame]?.volume != null)
                CtEndGameVolume.Value = LoadedKit.Musics[(int)MusicIvents.EndGame].volume;
            if (LoadedKit?.Musics[(int)MusicIvents.StartGame]?.volume != null)
                CtStartGameVolume.Value = LoadedKit.Musics[(int)MusicIvents.StartGame].volume;
            if (LoadedKit?.Musics[(int)MusicIvents.StartRound]?.volume != null)
                CtStartRoundVolume.Value = LoadedKit.Musics[(int)MusicIvents.StartRound].volume;
            if (LoadedKit?.Musics[(int)MusicIvents.StartAction]?.volume != null)
                CtStartActionVolume.Value = LoadedKit.Musics[(int)MusicIvents.StartAction].volume;
            if (LoadedKit?.Musics[(int)MusicIvents.KillSound]?.volume != null)
                CtKillVolume.Value = LoadedKit.Musics[(int)MusicIvents.KillSound].volume;
            if (LoadedKit?.Musics[(int)MusicIvents.DeathCam]?.volume != null)
                CtDeathcamVolume.Value = LoadedKit.Musics[(int)MusicIvents.DeathCam].volume;
            if (LoadedKit?.Musics[(int)MusicIvents.TenSecondBomb]?.volume != null)
                CtTenSecondBombVolume.Value = LoadedKit.Musics[(int)MusicIvents.TenSecondBomb].volume;
            if (LoadedKit?.Musics[(int)MusicIvents.TenSecondRound]?.volume != null)
                CtTenSecondRoundVolume.Value = LoadedKit.Musics[(int)MusicIvents.TenSecondRound].volume;

            CtBombVolumeText.Text = LoadedKit.Musics[(int)MusicIvents.Bomb].volume.ToString();
            CtMvpVolumeText.Text = LoadedKit.Musics[(int)MusicIvents.Mvp].volume.ToString();
            CtWinRoundVolumeText.Text = LoadedKit.Musics[(int)MusicIvents.WinRound].volume.ToString();
            CtLoseRoundVolumeText.Text = LoadedKit.Musics[(int)MusicIvents.LoseRound].volume.ToString();
            CtEndGameVolumeText.Text = LoadedKit.Musics[(int)MusicIvents.EndGame].volume.ToString();
            CtStartGameVolumeText.Text = LoadedKit.Musics[(int)MusicIvents.StartGame].volume.ToString();
            CtStartRoundVolumeText.Text = LoadedKit.Musics[(int)MusicIvents.StartRound].volume.ToString();
            CtStartActionVolumeText.Text = LoadedKit.Musics[(int)MusicIvents.StartAction].volume.ToString();
            CtKillVolumeText.Text = LoadedKit.Musics[(int)MusicIvents.KillSound].volume.ToString();
            CtDeathcamVolumeText.Text = LoadedKit.Musics[(int)MusicIvents.DeathCam].volume.ToString();
            CtTenSecondBombVolumeText.Text = LoadedKit.Musics[(int)MusicIvents.TenSecondBomb].volume.ToString();
            CtTenSecondRoundVolumeText.Text = LoadedKit.Musics[(int)MusicIvents.TenSecondRound].volume.ToString();


            CtBombVolumeEnable.Checked = LoadedKit.Musics[(int)MusicIvents.Bomb].IsEnable;
            CtMvpVolumeEnable.Checked = LoadedKit.Musics[(int)MusicIvents.Mvp].IsEnable;
            CtWinRoundVolumeEnable.Checked = LoadedKit.Musics[(int)MusicIvents.WinRound].IsEnable;
            CtLoseRoundVolumeEnable.Checked = LoadedKit.Musics[(int)MusicIvents.LoseRound].IsEnable;
            CtEndGameVolumeEnable.Checked = LoadedKit.Musics[(int)MusicIvents.EndGame].IsEnable;
            CtStartGameVolumeEnable.Checked = LoadedKit.Musics[(int)MusicIvents.StartGame].IsEnable;
            CtStartRoundVolumeEnable.Checked = LoadedKit.Musics[(int)MusicIvents.StartRound].IsEnable;
            CtStartActionVolumeEnable.Checked = LoadedKit.Musics[(int)MusicIvents.StartAction].IsEnable;
            CtKillVolumeEnable.Checked = LoadedKit.Musics[(int)MusicIvents.KillSound].IsEnable;
            CtDeathcamVolumeEnable.Checked = LoadedKit.Musics[(int)MusicIvents.DeathCam].IsEnable;
            CtTenSecondBombVolumeEnable.Checked = LoadedKit.Musics[(int)MusicIvents.TenSecondBomb].IsEnable;
            CtTenSecondRoundVolumeEnable.Checked = LoadedKit.Musics[(int)MusicIvents.TenSecondRound].IsEnable;
        }

        private void TSelectMusic(string path)
        {
            Kit_Sound LoadedKit = MusicKits.loadJson(path);
            if (LoadedKit == null)
            {
                return;
            }
            if (LoadedKit?.Musics[(int)MusicIvents.Bomb]?.volume != null)
                TBombVolume.Value = LoadedKit.Musics[(int)MusicIvents.Bomb].volume;
            if (LoadedKit?.Musics[(int)MusicIvents.Mvp]?.volume != null)
                TMvpVolume.Value = LoadedKit.Musics[(int)MusicIvents.Mvp].volume;
            if (LoadedKit?.Musics[(int)MusicIvents.WinRound]?.volume != null)
                TWinRoundVolume.Value = LoadedKit.Musics[(int)MusicIvents.WinRound].volume;
            if (LoadedKit?.Musics[(int)MusicIvents.LoseRound]?.volume != null)
                TLoseRoundVolume.Value = LoadedKit.Musics[(int)MusicIvents.LoseRound].volume;
            if (LoadedKit?.Musics[(int)MusicIvents.EndGame]?.volume != null)
                TEndGameVolume.Value = LoadedKit.Musics[(int)MusicIvents.EndGame].volume;
            if (LoadedKit?.Musics[(int)MusicIvents.StartGame]?.volume != null)
                TStartGameVolume.Value = LoadedKit.Musics[(int)MusicIvents.StartGame].volume;
            if (LoadedKit?.Musics[(int)MusicIvents.StartRound]?.volume != null)
                TStartRoundVolume.Value = LoadedKit.Musics[(int)MusicIvents.StartRound].volume;
            if (LoadedKit?.Musics[(int)MusicIvents.StartAction]?.volume != null)
                TStartActionVolume.Value = LoadedKit.Musics[(int)MusicIvents.StartAction].volume;
            if (LoadedKit?.Musics[(int)MusicIvents.KillSound]?.volume != null)
                TKillVolume.Value = LoadedKit.Musics[(int)MusicIvents.KillSound].volume;
            if (LoadedKit?.Musics[(int)MusicIvents.DeathCam]?.volume != null)
                TDeathcamVolume.Value = LoadedKit.Musics[(int)MusicIvents.DeathCam].volume;
            if (LoadedKit?.Musics[(int)MusicIvents.TenSecondBomb]?.volume != null)
                TTenSecondBombVolume.Value = LoadedKit.Musics[(int)MusicIvents.TenSecondBomb].volume;
            if (LoadedKit?.Musics[(int)MusicIvents.TenSecondRound]?.volume != null)
                TTenSecondRoundVolume.Value = LoadedKit.Musics[(int)MusicIvents.TenSecondRound].volume;

            if (LoadedKit?.Musics[(int)MusicIvents.Bomb]?.volume != null)
                TBombVolumeText.Text = LoadedKit.Musics[(int)MusicIvents.Bomb].volume.ToString();
            if (LoadedKit?.Musics[(int)MusicIvents.Mvp]?.volume != null)
                TMvpVolumeText.Text = LoadedKit.Musics[(int)MusicIvents.Mvp].volume.ToString();
            if (LoadedKit?.Musics[(int)MusicIvents.WinRound]?.volume != null)
                TWinRoundVolumeText.Text = LoadedKit.Musics[(int)MusicIvents.WinRound].volume.ToString();
            if (LoadedKit?.Musics[(int)MusicIvents.LoseRound]?.volume != null)
                TLoseRoundVolumeText.Text = LoadedKit.Musics[(int)MusicIvents.LoseRound].volume.ToString();
            if (LoadedKit?.Musics[(int)MusicIvents.EndGame]?.volume != null)
                TEndGameVolumeText.Text = LoadedKit.Musics[(int)MusicIvents.EndGame].volume.ToString();
            if (LoadedKit?.Musics[(int)MusicIvents.StartGame]?.volume != null)
                TStartGameVolumeText.Text = LoadedKit.Musics[(int)MusicIvents.StartGame].volume.ToString();
            if (LoadedKit?.Musics[(int)MusicIvents.StartRound]?.volume != null)
                TStartRoundVolumeText.Text = LoadedKit.Musics[(int)MusicIvents.StartRound].volume.ToString();
            if (LoadedKit?.Musics[(int)MusicIvents.StartAction]?.volume != null)
                TStartActionVolumeText.Text = LoadedKit.Musics[(int)MusicIvents.StartAction].volume.ToString();
            if (LoadedKit?.Musics[(int)MusicIvents.KillSound]?.volume != null)
                TKillVolumeText.Text = LoadedKit.Musics[(int)MusicIvents.KillSound].volume.ToString();
            if (LoadedKit?.Musics[(int)MusicIvents.DeathCam]?.volume != null)
                TDeathcamVolumeText.Text = LoadedKit.Musics[(int)MusicIvents.DeathCam].volume.ToString();
            if (LoadedKit?.Musics[(int)MusicIvents.TenSecondBomb]?.volume != null)
                TTenSecondBombVolumeText.Text = LoadedKit.Musics[(int)MusicIvents.TenSecondBomb].volume.ToString();
            if (LoadedKit?.Musics[(int)MusicIvents.TenSecondRound]?.volume != null)
                TTenSecondRoundVolumeText.Text = LoadedKit.Musics[(int)MusicIvents.TenSecondRound].volume.ToString();

            TBombVolumeEnable.Checked = LoadedKit.Musics[(int)MusicIvents.Bomb].IsEnable;
            TMvpVolumeEnable.Checked = LoadedKit.Musics[(int)MusicIvents.Mvp].IsEnable;
            TWinRoundVolumeEnable.Checked = LoadedKit.Musics[(int)MusicIvents.WinRound].IsEnable;
            TLoseRoundVolumeEnable.Checked = LoadedKit.Musics[(int)MusicIvents.LoseRound].IsEnable;
            TEndGameVolumeEnable.Checked = LoadedKit.Musics[(int)MusicIvents.EndGame].IsEnable;
            TStartGameVolumeEnable.Checked = LoadedKit.Musics[(int)MusicIvents.StartGame].IsEnable;
            TStartRoundVolumeEnable.Checked = LoadedKit.Musics[(int)MusicIvents.StartRound].IsEnable;
            TStartActionVolumeEnable.Checked = LoadedKit.Musics[(int)MusicIvents.StartAction].IsEnable;
            TKillVolumeEnable.Checked = LoadedKit.Musics[(int)MusicIvents.KillSound].IsEnable;
            TDeathcamVolumeEnable.Checked = LoadedKit.Musics[(int)MusicIvents.DeathCam].IsEnable;
            TTenSecondBombVolumeEnable.Checked = LoadedKit.Musics[(int)MusicIvents.TenSecondBomb].IsEnable;
            TTenSecondRoundVolumeEnable.Checked = LoadedKit.Musics[(int)MusicIvents.TenSecondRound].IsEnable;
        }

        private void Mode_SelectedIndexChanged(object sender, EventArgs e)
        {
            int selectedIndex = Mode_select.SelectedIndex + 1;
            switch (selectedIndex)
            {
                case (int)Mode.Competitive:
                    mode = Mode.Competitive;
                    Competitive.Visible = true;
                    if (MusicKits.settings?.Music_Paths[0].path != null)
                    {
                        CtSelectMusic(MusicKits.settings?.Music_Paths[MusicKits.settings?.Competitivie?.Ct ?? 0].path);
                        TSelectMusic(MusicKits.settings?.Music_Paths[MusicKits.settings?.Competitivie?.T ?? 0].path);
                    }
                    break;
                case (int)Mode.Public:
                    Competitive.Visible = true;
                    mode = Mode.Public;
                    if (MusicKits.settings?.Music_Paths[0].path != null)
                    {
                        CtSelectMusic(MusicKits.settings?.Music_Paths[MusicKits.settings?.Public?.Ct ?? 0].path);
                        TSelectMusic(MusicKits.settings?.Music_Paths[MusicKits.settings?.Public?.T ?? 0].path);
                    }

                    break;
                case (int)Mode.Deathmatch:
                    Competitive.Visible = false;
                    mode = Mode.Deathmatch;
                    if (MusicKits.settings?.Music_Paths[0].path != null)
                    {
                        CtSelectMusic(MusicKits.settings?.Music_Paths[MusicKits.settings?.Deathmatch ?? 0].path);
                    }
                    break;
            }
        }

        private void CtStartGameVolume_Scroll(object sender, ScrollEventArgs e)
        {

        }
    }
}
