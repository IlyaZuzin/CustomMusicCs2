using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace MusicComanderGUI
{
    public partial class Creater : Form
    {
        private Main form;
        private Kit_Sound? kit = new Kit_Sound();   
        public Creater(Main _form)
        {
            kit.music = new string[13];
            InitializeComponent();
            //LoadJsonSetings();
            form = _form;
        }

        private void LoadJsonSetings()
        {
            kit.image = MusicKitImage.ImageLocation;
            kit.name = NameKit.Text;
            kit.music[(int)MusicIvents.Menu] = MainMenu_dir.Text;
            kit.music[(int)MusicIvents.DeathCam] = DeathCam_dir.Text;
            kit.music[(int)MusicIvents.WinRound] = WinRound_dir.Text;
            kit.music[(int)MusicIvents.LoseRound] = LoseRound_dir.Text;
            kit.music[(int)MusicIvents.Bomb] = Bomb_dir.Text;
            kit.music[(int)MusicIvents.StartAction] = StartAction_dir.Text;
            kit.music[(int)MusicIvents.Mvp] = MVPDir.Text;
            kit.music[(int)MusicIvents.StartGame] = StartGameDir.Text;
            kit.music[(int)MusicIvents.StartRound] = StartRoundDir.Text;
            kit.music[(int)MusicIvents.TenSecondBomb] = TenSecondDir.Text;
            kit.music[(int)MusicIvents.KillSound] = KillSoundDir.Text;
            kit.music[(int)MusicIvents.TenSecondRound] = TenSecondRoundDir.Text;
            kit.music[(int)MusicIvents.EndGame] = EndGameDir.Text;

            MusicKits.SaveSetting();
        }
        private void MusicKitImage_Click(object sender, EventArgs e)
        {

        }

        private void MainMenuSelect_Click(object sender, EventArgs e)
        {
            OpenFileDialog direct = new OpenFileDialog();
            if (direct.ShowDialog() == DialogResult.OK)
            {
                MainMenu_dir.Text = direct.FileName;
            }
        }

        private void StartGameSelect_Click(object sender, EventArgs e)
        {
            OpenFileDialog direct = new OpenFileDialog();
            if (direct.ShowDialog() == DialogResult.OK)
            {
                StartGameDir.Text = direct.FileName;
            }
        }

        private void StartRound_Click(object sender, EventArgs e)
        {
            OpenFileDialog direct = new OpenFileDialog();
            if (direct.ShowDialog() == DialogResult.OK)
            {
                StartRoundDir.Text = direct.FileName;
            }
        }

        private void StartActionSelect_Click(object sender, EventArgs e)
        {
            OpenFileDialog direct = new OpenFileDialog();
            if (direct.ShowDialog() == DialogResult.OK)
            {
                StartAction_dir.Text = direct.FileName;
            }
        }

        private void WinRoundSelect_Click(object sender, EventArgs e)
        {
            OpenFileDialog direct = new OpenFileDialog();
            if (direct.ShowDialog() == DialogResult.OK)
            {
                WinRound_dir.Text = direct.FileName;
            }
        }

        private void LoseRoundSelect_Click(object sender, EventArgs e)
        {
            OpenFileDialog direct = new OpenFileDialog();
            if (direct.ShowDialog() == DialogResult.OK)
            {
                LoseRound_dir.Text = direct.FileName;
            }
        }

        private void MVPSelect_Click(object sender, EventArgs e)
        {
            OpenFileDialog direct = new OpenFileDialog();
            if (direct.ShowDialog() == DialogResult.OK)
            {
                MVPDir.Text = direct.FileName;
            }
        }

        private void BombSelect_Click(object sender, EventArgs e)
        {
            OpenFileDialog direct = new OpenFileDialog();
            if (direct.ShowDialog() == DialogResult.OK)
            {
                Bomb_dir.Text = direct.FileName;
            }
        }

        private void TenSecondSelect_Click(object sender, EventArgs e)
        {
            OpenFileDialog direct = new OpenFileDialog();
            if (direct.ShowDialog() == DialogResult.OK)
            {
                TenSecondDir.Text = direct.FileName;
            }
        }

        private void TenSecondRoundSelect_Click(object sender, EventArgs e)
        {
            OpenFileDialog direct = new OpenFileDialog();
            if (direct.ShowDialog() == DialogResult.OK)
            {
                TenSecondRoundDir.Text = direct.FileName;
            }
        }

        private void EndGameSelect_Click(object sender, EventArgs e)
        {
            OpenFileDialog direct = new OpenFileDialog();
            if (direct.ShowDialog() == DialogResult.OK)
            {
                EndGameDir.Text = direct.FileName;
            }
        }

        private void KillSoundSelect_Click(object sender, EventArgs e)
        {
            OpenFileDialog direct = new OpenFileDialog();
            if (direct.ShowDialog() == DialogResult.OK)
            {
                KillSoundDir.Text = direct.FileName;
            }
        }

        private void DeathCamSelect_Click(object sender, EventArgs e)
        {
            OpenFileDialog direct = new OpenFileDialog();
            if (direct.ShowDialog() == DialogResult.OK)
            {
                DeathCam_dir.Text = direct.FileName;
            }
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

        private void Update_Click(object sender, EventArgs e)
        {
            MusicKits.UpdateMusicKit(kit);
        }

        private void CreateKit_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog direct = new FolderBrowserDialog();
            LoadJsonSetings();
            if (direct.ShowDialog() == DialogResult.OK)
                MusicKits.CreateMusicKit(direct.SelectedPath, NameKit.Text, kit);

            int size = MusicKits.settings.Music_Paths.Count();
            string name = NameKit.Text;
            form.imageList1.Images.Add(Image.FromFile(ImageDir.Text));
            int newImageIndex = form.imageList1.Images.Count - 1;
            ListViewItem item = new ListViewItem(name, newImageIndex);
            form.MusicNumbers.Items.Add(item);
            form.TSide.Items.Add(name);
            form.CtSide.Items.Add(name);
            form.MainMusic.Items.Add(name);
            MusicKits.SaveSetting();
        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
