using CustomMusic;
using System.Diagnostics;
using System.IO;
namespace MusicComanderGUI
{
    public partial class Main : Form
    {
        public static Main? Instance { get; private set; }
        public static string? Dm;
        public static TeamPath? teamPath = new TeamPath();
        public static string? main;
        public static Mode mode;
        public static string? PlayingPath;
        public Main()
        {
            InitializeComponent();
            this.ResizeRedraw = true;
            this.DoubleBuffered = true;
            Instance = this;
            MusicKits.LoadSetting();
            MusicNumbers.SelectedIndexChanged += MusicNumbers_SelectedIndexChanged;
            CtList.SelectedIndexChanged += CtList_SelectedIndexChanged;
            TList.SelectedIndexChanged += TList_SelectedIndexChanged;
            MusicNumbers.LargeImageList = imageList1;
            CtList.LargeImageList = imageList1;
            TList.LargeImageList = imageList1;
            int newImageIndex = 0;
            SetConsoleLog("[Settings] Loading settings");
            if (MusicKits.settings?.Music_Paths != null)
            {
                for (int i = 0; i < MusicKits.settings.Music_Paths.Count; i++)
                {
                    var b = MusicKits.settings.Music_Paths[i];
                    string path = Path.Combine(b.path, "profile.json");
                    string image;
                    if (b.image != null)
                    {
                        image = Path.Combine(b.path, b.image);
                    }
                    else
                    {
                        SetConsoleLog($"[Settings] Not found image {b?.name}");
                        MusicKits.settings.Music_Paths.RemoveAt(i);
                        if (i != 0)
                            i--;
                        continue;
                    }
                    if (Path.Exists(path) &&
                        Path.Exists(image) &&
                        b.name != null)
                    {
                        try
                        {
                            imageList1.Images.Add(Image.FromFile(image));
                            newImageIndex = imageList1.Images.Count - 1;
                            ListViewItem item1 = new ListViewItem(b.name, newImageIndex);
                            ListViewItem item2 = new ListViewItem(b.name, newImageIndex);
                            ListViewItem item3 = new ListViewItem(b.name, newImageIndex);

                            MusicNumbers.Items.Add(item1);
                            CtList.Items.Add(item2);
                            TList.Items.Add(item3);
                            SetConsoleLog($"[Settings] {b?.name} loaded");
                        }
                        catch
                        {
                            SetConsoleLog($"[Settings] Error while load music kit {b?.name}");
                            MusicKits.settings.Music_Paths.RemoveAt(i);
                            if (i != 0)
                                i--;
                        }
                    }
                    else
                    {
                        SetConsoleLog($"[Settings] Not found \"profile.json\" {b?.name}");
                        MusicKits.settings.Music_Paths.RemoveAt(i);
                        if (i != 0)
                            i--;
                    }
                }
            }

            if (MusicKits.settings.DoubleMode == false)
            {

            }
            SetConsoleLog($"[Settings] Load {newImageIndex} MusicKits");

            MusicNumbers.LargeImageList = imageList1;
        }

        public void SetConsoleLog(string Log)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new Action(() => SetConsoleLog(Log)));
                return;
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
                    CtList.Items.Add(item);
                    TList.Items.Add(item);
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
                main = MusicKits.settings.Music_Paths[index].path;

            }
            catch (Exception ex)
            {
                // Безопасный вывод ошибки без обращения к MusicKits.settings.Last
                SetConsoleLog($"Ошибка при выборе набора: {ex.Message}");
            }

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
                MusicKits.CopyFileByFileSave("MusicKit.cfg", path, "gamestate_integration_mymusic.cfg");
            }
        }

        private void SaveSettings_Click(object sender, EventArgs e)
        {
            MusicKits.SaveSetting();
        }

        private void Python_Click(object sender, EventArgs e)
        {
            var start = new ProcessStartInfo();
            start.FileName = "python";
            start.Arguments = AppContext.BaseDirectory + "Python/ValveMusicKit.py";
            using var process = Process.Start(start);
        }

        private void Menu_Paint(object sender, PaintEventArgs e)
        {

        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox17_TextChanged(object sender, EventArgs e)
        {

        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {

        }

        private void HomePage_Click(object sender, EventArgs e)
        {
            Menu.Visible = true;
            Library.Visible = false;
        }

        private void LibraryPage_Click(object sender, EventArgs e)
        {
            Menu.Visible = false;
            Library.Visible = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (MusicComanderGUI.Server.is_Running)
            {
                MusicComanderGUI.Server.Stop();
                Server.Text = "Start server";
            }
            else
            {
                MusicComanderGUI.Server.Start();
                if (MusicComanderGUI.Server.is_Running)
                    Server.Text = "Close server";
            }
        }

        private void KeepMusic_Click(object sender, EventArgs e)
        {
            if (MusicKits.settings.DoubleMode == true)
            {
                TWorkStation.Visible = false;
                CTWorkStation.Visible = false;
                MusicKits.settings.DoubleMode = false;
                SoloMod.Visible = true;
            }
            else
            {
                SoloMod.Visible = false;
                TWorkStation.Visible = true;
                CTWorkStation.Visible = true;
                MusicKits.settings.DoubleMode = true;
            }

        }

        private void CTSelectMK_Click(object sender, EventArgs e)
        {
            if (CtList.Visible == false) CtList.Visible = true;
            else CtList.Visible = false;
        }

        private void CtList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (CtList.SelectedIndices.Count == 0) return;

            try
            {
                int index = CtList.SelectedIndices[0];

                if (index >= 0 && index < MusicKits.settings.Music_Paths.Count)
                {
                    var selectedKit = MusicKits.settings.Music_Paths[index];

                    teamPath.Ct = selectedKit.path;

                    CTImageMK.ImageLocation = Path.Combine(selectedKit.path, selectedKit.image);
                    
                    CTNameMK.Text = selectedKit.name;
                    CtList.Visible = false;
                }
            }
            catch (Exception ex)
            {
                SetConsoleLog($"[Error] CtList selection failed: {ex.Message}");
            }
        }

        private void TList_SelectedIndexChanged(object sender, EventArgs e)
        {
            // ИСПРАВЛЕНО: проверяем именно TList
            if (TList.SelectedIndices.Count == 0) return;

            try
            {
                // ИСПРАВЛЕНО: берем индекс из TList, а не из MusicNumbers
                int index = TList.SelectedIndices[0];

                if (index >= 0 && index < MusicKits.settings.Music_Paths.Count)
                {
                    var selectedKit = MusicKits.settings.Music_Paths[index];

                    teamPath.T = selectedKit.path;

                    // Если в настройках лежит только имя файла (например, "icon.png"), склеиваем путь:
                    TImageMK.ImageLocation = Path.Combine(selectedKit.path, selectedKit.image);

                    TNameMK.Text = selectedKit.name;
                    TList.Visible = false;
                }
            }
            catch (Exception ex)
            {
                SetConsoleLog($"[Error] TList selection failed: {ex.Message}");
            }
        }

        private void TSelectMK_Click(object sender, EventArgs e)
        {
            if (TList.Visible == false) TList.Visible = true;
            else TList.Visible = false;
        }

        private void CTModMK_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(teamPath.Ct))
            {
                return;
            }
            ChangeMusicVolume change = new ChangeMusicVolume(teamPath.Ct);
        }

        private void TModMK_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(teamPath.T))
            {
                return;
            }
            ChangeMusicVolume change = new ChangeMusicVolume(teamPath.T);
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            CustomMusic.ConsoleLog console = new CustomMusic.ConsoleLog();
            console.Show();
        }

        private void OpenFolder_Click(object sender, EventArgs e)
        {
            if (PlayingPath != null)
                Process.Start("explorer.exe", PlayingPath);

        }

        private void SoloSelectMk_Click(object sender, EventArgs e)
        {
            if (SoloList.Visible == false) SoloList.Visible = true;
            else SoloList.Visible = false;
        }

        private void SoloModMk_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(main)) return;
            ChangeMusicVolume change = new ChangeMusicVolume(main);
        }
    }
}
