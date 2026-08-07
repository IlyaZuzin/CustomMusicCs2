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
        public Creater(Main _form)
        {
            InitializeComponent();
            LoadJsonSetings();
            form = _form;
        }

        private void LoadJsonSetings()
        {
            MusicKitImage.ImageLocation = MusicKits.settings?.Last?.image;
            MainMenu_dir.Text = MusicKits.settings?.Last?.menu;
            DeathCam_dir.Text = MusicKits.settings?.Last?.deathCam;
            WinRound_dir.Text = MusicKits.settings?.Last?.WinRound;
            LoseRound_dir.Text = MusicKits.settings?.Last?.LoseRound;
            Bomb_dir.Text = MusicKits.settings?.Last?.Bomb;
            StartAction_dir.Text = MusicKits.settings?.Last.StartAction;
            MVPDir.Text = MusicKits.settings?.Last.MVP;
            StartGameDir.Text = MusicKits.settings?.Last.StartGame;
            StartRoundDir.Text = MusicKits.settings?.Last.StartRound;
            TenSecondDir.Text = MusicKits.settings?.Last.TenSecond;
            KillSoundDir.Text = MusicKits.settings?.Last.KillSound;
            TenSecondRoundDir.Text = MusicKits.settings?.Last.TenSecondRound;
            EndGameDir.Text = MusicKits.settings?.Last.EndGame;

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
                MusicKits.settings?.Last?.menu = direct.FileName;
                MainMenu_dir.Text = MusicKits.settings?.Last?.menu;
            }
        }

        private void StartGameSelect_Click(object sender, EventArgs e)
        {
            OpenFileDialog direct = new OpenFileDialog();
            if (direct.ShowDialog() == DialogResult.OK)
            {
                MusicKits.settings?.Last?.StartGame = direct.FileName;
                StartGameDir.Text = MusicKits.settings?.Last?.StartGame;
            }
        }

        private void StartRound_Click(object sender, EventArgs e)
        {
            OpenFileDialog direct = new OpenFileDialog();
            if (direct.ShowDialog() == DialogResult.OK)
            {
                MusicKits.settings?.Last?.StartRound = direct.FileName;
                StartRoundDir.Text = MusicKits.settings?.Last?.StartRound;
            }
        }

        private void StartActionSelect_Click(object sender, EventArgs e)
        {
            OpenFileDialog direct = new OpenFileDialog();
            if (direct.ShowDialog() == DialogResult.OK)
            {
                MusicKits.settings?.Last?.StartAction = direct.FileName;
                StartAction_dir.Text = MusicKits.settings?.Last?.StartAction;
            }
        }

        private void WinRoundSelect_Click(object sender, EventArgs e)
        {
            OpenFileDialog direct = new OpenFileDialog();
            if (direct.ShowDialog() == DialogResult.OK)
            {
                MusicKits.settings.Last.WinRound = direct.FileName;
                WinRound_dir.Text = MusicKits.settings.Last.WinRound;
            }
        }

        private void LoseRoundSelect_Click(object sender, EventArgs e)
        {
            OpenFileDialog direct = new OpenFileDialog();
            if (direct.ShowDialog() == DialogResult.OK)
            {
                MusicKits.settings.Last.LoseRound = direct.FileName;
                LoseRound_dir.Text = MusicKits.settings.Last.LoseRound;
            }
        }

        private void MVPSelect_Click(object sender, EventArgs e)
        {
            OpenFileDialog direct = new OpenFileDialog();
            if (direct.ShowDialog() == DialogResult.OK)
            {
                MusicKits.settings?.Last?.MVP = direct.FileName;
                MVPDir.Text = MusicKits.settings?.Last?.MVP;
            }
        }

        private void BombSelect_Click(object sender, EventArgs e)
        {
            OpenFileDialog direct = new OpenFileDialog();
            if (direct.ShowDialog() == DialogResult.OK)
            {
                MusicKits.settings?.Last.Bomb = direct.FileName;
                Bomb_dir.Text = MusicKits.settings?.Last.Bomb;
            }
        }

        private void TenSecondSelect_Click(object sender, EventArgs e)
        {
            OpenFileDialog direct = new OpenFileDialog();
            if (direct.ShowDialog() == DialogResult.OK)
            {
                MusicKits.settings?.Last?.TenSecond = direct.FileName;
                TenSecondDir.Text = MusicKits.settings?.Last?.TenSecond;
            }
        }

        private void TenSecondRoundSelect_Click(object sender, EventArgs e)
        {
            OpenFileDialog direct = new OpenFileDialog();
            if (direct.ShowDialog() == DialogResult.OK)
            {
                MusicKits.settings?.Last?.TenSecondRound = direct.FileName;
                TenSecondRoundDir.Text = MusicKits.settings?.Last?.TenSecondRound;
            }
        }

        private void EndGameSelect_Click(object sender, EventArgs e)
        {
            OpenFileDialog direct = new OpenFileDialog();
            if (direct.ShowDialog() == DialogResult.OK)
            {
                MusicKits.settings?.Last?.EndGame = direct.FileName;
                EndGameDir.Text = MusicKits.settings?.Last?.EndGame;
            }
        }

        private void KillSoundSelect_Click(object sender, EventArgs e)
        {
            OpenFileDialog direct = new OpenFileDialog();
            if (direct.ShowDialog() == DialogResult.OK)
            {
                MusicKits.settings?.Last?.KillSound = direct.FileName;
                KillSoundDir.Text = MusicKits.settings?.Last?.KillSound;
            }
        }

        private void DeathCamSelect_Click(object sender, EventArgs e)
        {
            OpenFileDialog direct = new OpenFileDialog();
            if (direct.ShowDialog() == DialogResult.OK)
            {
                MusicKits.settings?.Last?.deathCam = direct.FileName;
                DeathCam_dir.Text = MusicKits.settings?.Last?.deathCam;
            }
        }

        private void SelectImage_Click(object sender, EventArgs e)
        {
            OpenFileDialog direct = new OpenFileDialog();
            if (direct.ShowDialog() == DialogResult.OK)
            {
                MusicKits.settings?.Last?.image = direct.FileName;
                MusicKitImage.ImageLocation = direct.FileName;
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
            MusicKits.UpdateMusicKit();
        }

        private void CreateKit_Click(object sender, EventArgs e)
        {
            MusicKits.settings.Last.name = NameKit.Text;
            FolderBrowserDialog direct = new FolderBrowserDialog();
            if (direct.ShowDialog() == DialogResult.OK)
            {
                MusicKits.CreateMusicKit(direct.SelectedPath);
            }
            int size = MusicKits.settings.Music_Paths.Count();
            string name = MusicKits.settings.Music_Paths[size - 1].name;
            
            form.imageList1.Images.Add(Image.FromFile(MusicKits.settings.Music_Paths[size - 1].image));
            int newImageIndex = form.imageList1.Images.Count - 1;
            ListViewItem item = new ListViewItem(name, newImageIndex);
            form.MusicNumbers.Items.Add(item); ;
        }
    }
}
