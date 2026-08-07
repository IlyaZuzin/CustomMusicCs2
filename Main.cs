
using Microsoft.VisualBasic.Logging;
using System.IO;

namespace MusicComanderGUI
{
    public partial class Main : Form
    {
        public static Main? Instance { get; private set; }
        public static string? T;
        public static string? Ct;

        public Main()
        {
            InitializeComponent();
            Instance = this;
            MusicKits.LoadSetting();
            MusicNumbers.SelectedIndexChanged += MusicNumbers_SelectedIndexChanged;
            int? volume = MusicKits.settings?.Volume;
            if (volume != null)
            {
                WavPlayer.SetVolume(volume.Value / 100);
                trackBar1.Value = volume.Value;
                textBox7.Text = trackBar1.Value.ToString();
            }

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
            if (MusicKits.settings?.CtSide != null && MusicKits.settings?.CtSide != -1)
            {
                int? index = MusicKits.settings?.CtSide;
                CtSide.SelectedIndex = index.Value;
            }
            if (MusicKits.settings?.TSide != null && MusicKits.settings?.TSide != -1)
            {
                int? index = MusicKits.settings?.TSide;
                TSide.SelectedIndex = index.Value;
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
            WavPlayer.SetVolume((float)trackBar1.Value / 100);
            textBox7.Text = trackBar1.Value.ToString();
            MusicKits.settings?.Volume = trackBar1.Value;
        }

        private void LoadJson_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog direct = new FolderBrowserDialog();
            if (direct.ShowDialog() == DialogResult.OK)
            {
                Kit_Sound? load = MusicKits.AddMusicKit(direct.SelectedPath);
                LoadJsonSetings(load);
                int size = MusicKits.settings.Music_Paths.Count();
                string name = MusicKits.settings.Music_Paths[size - 1].name;
                imageList1.Images.Add(Image.FromFile(MusicKits.settings.Music_Paths[size - 1].image));
                int newImageIndex = imageList1.Images.Count - 1;
                ListViewItem item = new ListViewItem(name, newImageIndex);
                MusicNumbers.Items.Add(item);
                TSide.Items.Add(name);
                CtSide.Items.Add(name);
            }
        }

        private void LoadJsonSetings(Kit_Sound? load)
        {
            MusicKits.settings?.Last = load;
            MusicKits.SaveSetting();
        }

        private void MusicNumbers_SelectedIndexChanged(object sender, EventArgs e)
        {
            // 1. Обязательная проверка: если выбор снят, ничего не делаем
            if (MusicNumbers.SelectedIndices.Count == 0)
                return;

            try
            {
                // 2. Получаем индекс выбранного элемента
                int index = MusicNumbers.SelectedIndices[0];

                // 3. Безопасно проверяем, существует ли такой индекс в списке
                if (MusicKits.settings?.Last != null)
                {
                    // Выводим лог об успешном выборе (вместо сообщения об ошибке)
                    Kit_Sound load = MusicKits.loadJson(MusicKits.settings.Music_Paths[index].path);
                    LoadJsonSetings(load);
                    SetConsoleLog($"Успешно выбран набор: {MusicKits.settings.Last.name}");
                }
                else
                {
                    SetConsoleLog("Ошибка: Загруженный набор данных пуст (null).");
                }
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
                Ct = MusicKits.settings?.Music_Paths[selectedIndex].path;
                LoadJsonSetings(MusicKits.loadJson(Ct));
                CtImage.ImageLocation = MusicKits.settings?.Music_Paths[selectedIndex].image;
                MusicKits.settings?.CtSide = selectedIndex;
            }
        }

        private void TSide_SelectedIndexChanged(object sender, EventArgs e)
        {
            int selectedIndex = TSide.SelectedIndex;
            if (selectedIndex != -1)
            {
                T = MusicKits.settings?.Music_Paths[selectedIndex].path;
                LoadJsonSetings(MusicKits.loadJson(T));
                TImage.ImageLocation = MusicKits.settings?.Music_Paths[selectedIndex].image;
                MusicKits.settings?.TSide = selectedIndex;
            }
        }

        private void Musickits_Click(object sender, EventArgs e)
        {
            MusicKitMenu.Visible = true;
            ServerMenu.Visible = false;
        }

        private void Server_Click(object sender, EventArgs e)
        {
            MusicKitMenu.Visible = false;
            ServerMenu.Visible = true;
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
    }
}
