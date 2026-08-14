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
            kit.Musics = new MusicSettings[13];
            for (int i =0; i < 13; i++)
            {
                kit.Musics[i] = new MusicSettings();
            }
            InitializeComponent();
            //LoadJsonSetings();
            form = _form;
        }

        private void LoadJsonSetings()
        {
            kit.image = MusicKitImage.ImageLocation;
            kit.name = NameKit.Text;
            kit.Musics[(int)MusicIvents.Menu].path = MainMenu_dir.Text;
            kit.Musics[(int)MusicIvents.DeathCam].path = DeathCam_dir.Text;
            kit.Musics[(int)MusicIvents.WinRound].path = WinRound_dir.Text;
            kit.Musics[(int)MusicIvents.LoseRound].path = LoseRound_dir.Text;
            kit.Musics[(int)MusicIvents.Bomb].path = Bomb_dir.Text;
            kit.Musics[(int)MusicIvents.StartAction].path = StartAction_dir.Text;
            kit.Musics[(int)MusicIvents.Mvp].path = MVPDir.Text;
            kit.Musics[(int)MusicIvents.StartGame].path = StartGameDir.Text;
            kit.Musics[(int)MusicIvents.StartRound].path = StartRoundDir.Text;
            kit.Musics[(int)MusicIvents.TenSecondBomb].path = TenSecondDir.Text;
            kit.Musics[(int)MusicIvents.KillSound].path = KillSoundDir.Text;
            kit.Musics[(int)MusicIvents.TenSecondRound].path = TenSecondRoundDir.Text;
            kit.Musics[(int)MusicIvents.EndGame].path = EndGameDir.Text;

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

        private void MainMenuCheck_CheckedChanged(object sender, EventArgs e)
        {
            Ivents.IsMenu = MainMenuCheck.Checked;
        }

        private void StartGameChecked_CheckedChanged(object sender, EventArgs e)
        {
            Ivents.IsStartGame = StartGameChecked.Checked;
        }

        private void StartRoundChecked_CheckedChanged(object sender, EventArgs e)
        {
            Ivents.IsBomb = BombCheck.Checked;
        }

        private void StartActionChecker_CheckedChanged(object sender, EventArgs e)
        {
            Ivents.IsStartAction = StartActionChecker.Checked;
        }
        private void WinRoundCheck_CheckedChanged(object sender, EventArgs e)
        {
            Ivents.IsWinRound = WinRoundCheck.Checked;
        }
        private void LoseRoundCheck_CheckedChanged(object sender, EventArgs e)
        {
            Ivents.IsLoseRound = LoseRoundCheck.Checked;
        }
        private void MVPChecked_CheckedChanged(object sender, EventArgs e)
        {
            Ivents.IsMVP = MVPChecked.Checked;
        }
        private void BombCheck_CheckedChanged(object sender, EventArgs e)
        {
            Ivents.IsBomb = BombCheck.Checked;
        }
        private void TenSecondChecked_CheckedChanged(object sender, EventArgs e)
        {
            Ivents.IsTenSecond = TenSecondChecked.Checked;
        }

        private void TenSecondRoundChecked_CheckedChanged(object sender, EventArgs e)
        {
            Ivents.IsTenSecondRound = TenSecondChecked.Checked;
        }

        private void EndGameChecked_CheckedChanged(object sender, EventArgs e)
        {
            Ivents.IsEndGame = EndGameChecked.Checked;
        }

        private void KillSoundChecked_CheckedChanged(object sender, EventArgs e)
        {
            Ivents.IsKill = BombCheck.Checked;
        }
        private void DeathCamCheck_CheckedChanged(object sender, EventArgs e)
        {
            Ivents.IsDeath = DeathCamCheck.Checked;
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
        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
