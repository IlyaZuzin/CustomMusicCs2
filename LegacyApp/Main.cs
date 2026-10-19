using CustomMusic;
using System.ComponentModel.Design;
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
        public static Profile? loaded_profile;
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
            foreach (Profile profile in MusicKits.settings.profiles)
            {
                Profiles.Items.Add(profile.name);
            }
            SetConsoleLog($"[Settings] Load {newImageIndex} MusicKits");

            int? last_profile_index = MusicKits.settings?.profile;

            // Проверяем: если индекс есть И в списке профилей реально что-то лежит
            if (last_profile_index != null && MusicKits.settings.profiles != null && MusicKits.settings.profiles.Count > last_profile_index.Value)
            {
                loaded_profile = LoadProfile(MusicKits.settings.profiles[last_profile_index.Value]);
            }

            // КРИТИЧЕСКОЕ ИСПРАВЛЕНИЕ: Если профиль всё еще null (первый запуск или сбой), 
            // создаем пустой объект в памяти, чтобы методы не падали!
            if (loaded_profile == null)
            {
                loaded_profile = new Profile();
                loaded_profile.name = "Default"; // Назовем по умолчанию
                SetConsoleLog("[Settings] loaded_profile был null, создан дефолтный профиль.");
            }
        }

        public void SetConsoleLog(string Log)
        {
            if (MusicComanderGUI.Server.Logging) ConsoleLog.Instance?.SendConsoleLog(Log);
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

                    // Создаем ТРИ РАЗНЫХ айтема для ТРЕХ РАЗНЫХ списков:
                    MusicNumbers.Items.Add(new ListViewItem(name, newImageIndex));
                    CtList.Items.Add(new ListViewItem(name, newImageIndex));
                    TList.Items.Add(new ListViewItem(name, newImageIndex));
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
            loaded_profile.Standart_mode = !loaded_profile.Standart_mode;
            EnableStandartMode(loaded_profile.Standart_mode);
        }

        private void EnableStandartMode(bool enable)
        {
            if (enable == true)
            {
                TWorkStation.Visible = false;
                CTWorkStation.Visible = false;
                SoloMod.Visible = true;
            }
            else
            {
                SoloMod.Visible = false;
                TWorkStation.Visible = true;
                CTWorkStation.Visible = true;
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

                    loaded_profile.ct_path = selectedKit.path;

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

                    loaded_profile.t_path = selectedKit.path;

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
            if (String.IsNullOrEmpty(loaded_profile.ct_path))
            {
                return;
            }
            ChangeMusicVolume change = new ChangeMusicVolume(loaded_profile.ct_path);
            change.Show();
        }

        private void TModMK_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(loaded_profile.t_path))
            {
                return;
            }
            ChangeMusicVolume change = new ChangeMusicVolume(loaded_profile.t_path);
            change.Show();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            ConsoleLog console = new ConsoleLog();
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

        private void AddProfile_Click(object sender, EventArgs e)
        {
            Profile new_profile = new Profile();
            if (loaded_profile != null) new_profile = loaded_profile.Copy();
            new_profile.name = ProfileName.Text;
            Profiles.Items.Add(new_profile.name);
            MusicKits.settings.profiles.Add(new_profile);
        }

        private Profile LoadProfile(Profile profile)
        {
            loaded_profile = profile;
            Profiles.Text = profile.name;
            EnableStandartMode(profile.Standart_mode);
            Kit_Sound? buf = MusicKits.loadJson(profile.ct_path);
            CTNameMK.Text = buf.name;
            CTImageMK.ImageLocation = buf.image;

            buf = MusicKits.loadJson(profile.t_path);
            TNameMK.Text = buf.name;
            TImageMK.ImageLocation = buf.image;

            buf = MusicKits.loadJson(profile.t_path);
            SoloMK.Text = buf?.name;
            SoloImage.ImageLocation = buf?.image;

            return profile;
        }

        private void Profiles_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Profiles.SelectedIndex != -1)
            {
                LoadProfile(MusicKits.settings.profiles[Profiles.SelectedIndex]);
                MusicKits.settings.profile = Profiles.SelectedIndex;
            }
        }

        private void Main_FormClosed(object sender, FormClosedEventArgs e)
        {
            MusicKits.SaveSetting();
        }
    }
}
