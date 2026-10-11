using CustomMusic;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Xml.Linq;

namespace MusicComanderGUI
{
    public partial class Creater : Form
    {
        private Main form;
        private Kit_Sound kit = new Kit_Sound();
        public Creater(Main _form)
        { 
            InitializeComponent();
            //LoadJsonSetings();
            form = _form;

            for (int i = 0; i < kit.music?.Length; i++) {
                kit.music[i] = new List<AudioFiles>();
            }

        }

        //private void LoadJsonSetings()
        //{
        //    kit.image = MusicKitImage.ImageLocation;
        //    kit.name = NameKit.Text;
        //    kit.music[(int)MusicIvents.Menu] = MainMenu_dir.Text;
        //    kit.music[(int)MusicIvents.DeathCam] = DeathCam_dir.Text;
        //    kit.music[(int)MusicIvents.WinRound] = WinRound_dir.Text;
        //    kit.music[(int)MusicIvents.LoseRound] = LoseRound_dir.Text;
        //    kit.music[(int)MusicIvents.Bomb] = Bomb_dir.Text;
        //    kit.music[(int)MusicIvents.StartAction] = StartAction_dir.Text;
        //    kit.music[(int)MusicIvents.Mvp] = MVPDir.Text;
        //    kit.music[(int)MusicIvents.StartGame] = StartGameDir.Text;
        //    kit.music[(int)MusicIvents.StartRound] = StartRoundDir.Text;
        //    kit.music[(int)MusicIvents.TenSecondBomb] = TenSecondDir.Text;
        //    kit.music[(int)MusicIvents.KillSound] = KillSoundDir.Text;
        //    kit.music[(int)MusicIvents.TenSecondRound] = TenSecondRoundDir.Text;
        //    kit.music[(int)MusicIvents.EndGame] = EndGameDir.Text;

        //    MusicKits.SaveSetting();
        //}

        private void MusicKitImage_Click(object sender, EventArgs e)
        {

        }

        private void MainMenuSelect_Click(object sender, EventArgs e)
        {
            IventMusicConf conf = new IventMusicConf(kit.music[(int)MusicIvents.Menu], "Menu");
            conf.Show();
        }

        private void StartGameSelect_Click(object sender, EventArgs e)
        {
            IventMusicConf conf = new IventMusicConf(kit.music[(int)MusicIvents.StartGame], "StartGame");
            conf.Show();
        }

        private void StartRound_Click(object sender, EventArgs e)
        {
            IventMusicConf conf = new IventMusicConf(kit.music[(int)MusicIvents.StartRound], "StartRound", kit.music[(int)MusicIvents.StartAction], "StartAction");
            conf.Show();
        }

        private void StartActionSelect_Click(object sender, EventArgs e)
        {
            IventMusicConf conf = new IventMusicConf(kit.music[(int)MusicIvents.StartRound], "StartRound", kit.music[(int)MusicIvents.StartAction], "StartAction");
            conf.Show();
        }

        private void WinRoundSelect_Click(object sender, EventArgs e)
        {
            IventMusicConf conf = new IventMusicConf(kit.music[(int)MusicIvents.WinRound], "WinRound");
            conf.Show();
        }

        private void LoseRoundSelect_Click(object sender, EventArgs e)
        {
            IventMusicConf conf = new IventMusicConf(kit.music[(int)MusicIvents.LoseRound], "LoseRound");
            conf.Show();
        }

        private void MVPSelect_Click(object sender, EventArgs e)
        {
            IventMusicConf conf = new IventMusicConf(kit.music[(int)MusicIvents.Mvp], "Mvp");
            conf.Show();
        }

        private void BombSelect_Click(object sender, EventArgs e)
        {
            IventMusicConf conf = new IventMusicConf(kit.music[(int)MusicIvents.Bomb], "Bomb");
            conf.Show();
        }

        private void TenSecondSelect_Click(object sender, EventArgs e)
        {
            IventMusicConf conf = new IventMusicConf(kit.music[(int)MusicIvents.TenSecondBomb], "TenSecondBomb");
            conf.Show();
        }

        private void TenSecondRoundSelect_Click(object sender, EventArgs e)
        {
            IventMusicConf conf = new IventMusicConf(kit.music[(int)MusicIvents.TenSecondRound], "TenSecondRound");
            conf.Show();
        }

        private void EndGameSelect_Click(object sender, EventArgs e)
        {
            IventMusicConf conf = new IventMusicConf(kit.music[(int)MusicIvents.EndGame], "EndGame");
            conf.Show();
        }

        private void KillSoundSelect_Click(object sender, EventArgs e)
        {
            IventMusicConf conf = new IventMusicConf(kit.music[(int)MusicIvents.KillSound], "KillSound");
            conf.Show();
        }

        private void DeathCamSelect_Click(object sender, EventArgs e)
        {
            IventMusicConf conf = new IventMusicConf(kit.music[(int)MusicIvents.DeathCam], "DeathCam");
            conf.Show();
        }

        private void SelectImage_Click(object sender, EventArgs e)
        {
            OpenFileDialog direct = new OpenFileDialog();
            if (direct.ShowDialog() == DialogResult.OK)
            {
                MusicKitImage.ImageLocation = direct.FileName;
                ImageDir.Text = direct.FileName;
            }
        }

        private void CreateKit_Click(object sender, EventArgs e)
        {
            try
            {
                FolderBrowserDialog direct = new FolderBrowserDialog();
                kit.image = MusicKitImage.ImageLocation;
                kit.name = NameKit.Text;
                if (NameKit.Text == null || !Path.Exists(ImageDir.Text))
                {
                    MessageBox.Show(
                        "Отсутсвует название набора или \nнекорректный путь до изображения",
                        "Недостаток аргументов",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error
                        );
                    return;
                }
                if (direct.ShowDialog() == DialogResult.OK)
                {
                    string path = Path.Combine(direct.SelectedPath, NameKit.Text);
                    if (!Path.Exists(path))
                        MusicKits.CreateMusicKit(direct.SelectedPath, NameKit.Text, kit);
                    else
                    {
                        DialogResult result = MessageBox.Show(
                        "Данный набор музыки уже существет, хотите перезаписать",
                        "Копия",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Error
                        );
                        if (result == DialogResult.Yes)
                        {
                            Directory.Delete(path);
                            MusicKits.CreateMusicKit(direct.SelectedPath, NameKit.Text, kit);
                        }
                        else
                            return;
                    }
                }
                int size = MusicKits.settings.Music_Paths.Count();
                string name = NameKit.Text;
                form.imageList1.Images.Add(Image.FromFile(ImageDir.Text));
                int newImageIndex = form.imageList1.Images.Count - 1;
                ListViewItem item = new ListViewItem(name, newImageIndex);
                form.MusicNumbers.Items.Add(item);
                MusicKits.SaveSetting();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    ex.Message,
                    "Неизвестаная ошибка",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                    );
            }
        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void NameKit_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(NameKit.Text))
            {
                TextErr.SetError(NameKit, "Вставьте название набора");
            }
            else
            {
                TextErr.SetError(NameKit, string.Empty);
            }
        }

        private void ImageDir_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(NameKit.Text))
            {
                ImageErr.SetError(SelectImage, "Выберите путь");
            }
            else
            {
                TextErr.SetError(SelectImage, string.Empty);
            }
            if (Path.Exists(ImageDir.Text))
            {
                TextErr.SetError(SelectImage, "Файл не найден");
            }
            else
            {
                TextErr.SetError(SelectImage, string.Empty);
            }
        }

        private void textBox12_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
