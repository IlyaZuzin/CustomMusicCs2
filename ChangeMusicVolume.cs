using MusicComanderGUI;
using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Text.Json;
using System.Windows.Forms;
using static System.Windows.Forms.Design.AxImporter;

namespace CustomMusic
{
    public partial class ChangeMusicVolume : Form
    {
        private Kit_Sound? kit;
        private string? path;
        public ChangeMusicVolume(string? path)
        {
            kit = MusicKits.loadJson(path);
            MusicKitImage.ImageLocation = kit?.image;
            NameKit.Text = kit?.name; 
            InitializeComponent();
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

        private void button1_Click(object sender, EventArgs e)
        {
            MusicKits.CopyFiles(path, kit);
            var options = new JsonSerializerOptions { WriteIndented = true };
            File.WriteAllText($"{path}\\profile.json", JsonSerializer.Serialize(kit, options));
        }
    }
}
