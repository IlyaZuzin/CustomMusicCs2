namespace MusicComanderGUI
{
    partial class Main
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Main));
            StartServer = new Button();
            textBox5 = new TextBox();
            splitter1 = new Splitter();
            textBox2 = new TextBox();
            textBox1 = new TextBox();
            LoadJson = new Button();
            MusicNumbers = new ListView();
            imageList1 = new ImageList(components);
            ServerMenu = new Panel();
            DoubleMode = new CheckBox();
            MainMusic = new ComboBox();
            SaveSettings = new Button();
            Cs2Directory = new Button();
            textBox8 = new TextBox();
            pictureBox1 = new PictureBox();
            textBox3 = new TextBox();
            MusicKitMenu = new Panel();
            OpenMusicKitForm = new Button();
            imageList2 = new ImageList(components);
            Server = new Button();
            Musickits = new Button();
            Settings = new Button();
            SettingsMenu = new Panel();
            MenuVolumeText = new TextBox();
            MenuVolume = new HScrollBar();
            textBox16 = new TextBox();
            Mode_select = new ComboBox();
            DeathcamVolume = new HScrollBar();
            DeathcamVolumeText = new TextBox();
            KillVolumeText = new TextBox();
            EndGameVolume = new HScrollBar();
            KillVolume = new HScrollBar();
            TenSecondRoundVolume = new HScrollBar();
            EndGameVolumeText = new TextBox();
            TenSecondRoundVolumeText = new TextBox();
            TenSecondBombVolume = new HScrollBar();
            TenSecondBombVolumeText = new TextBox();
            BombVolumeText = new TextBox();
            BombVolume = new HScrollBar();
            MvpVolumeText = new TextBox();
            MvpVolume = new HScrollBar();
            LoseRoundVolumeText = new TextBox();
            StartGameVolumeText = new TextBox();
            LoseRoundVolume = new HScrollBar();
            StartGameVolume = new HScrollBar();
            WinRoundVolumeText = new TextBox();
            WinRoundVolume = new HScrollBar();
            StartActionVolumeText = new TextBox();
            StartActionVolume = new HScrollBar();
            StartRoundVolume = new HScrollBar();
            StartRoundVolumeText = new TextBox();
            textBox10 = new TextBox();
            textBox9 = new TextBox();
            Bomb = new TextBox();
            WinRound = new TextBox();
            textBox11 = new TextBox();
            StartAction = new TextBox();
            textBox15 = new TextBox();
            textBox13 = new TextBox();
            textBox4 = new TextBox();
            DeathCam = new TextBox();
            LoseRound = new TextBox();
            textBox12 = new TextBox();
            CtSide = new ComboBox();
            DeathMatch = new Panel();
            TSide = new ComboBox();
            ServerMenu.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            MusicKitMenu.SuspendLayout();
            SettingsMenu.SuspendLayout();
            DeathMatch.SuspendLayout();
            SuspendLayout();
            // 
            // StartServer
            // 
            StartServer.BackColor = Color.Transparent;
            StartServer.BackgroundImageLayout = ImageLayout.None;
            StartServer.FlatAppearance.BorderSize = 0;
            StartServer.FlatAppearance.MouseDownBackColor = Color.Transparent;
            StartServer.FlatAppearance.MouseOverBackColor = Color.Transparent;
            StartServer.FlatStyle = FlatStyle.Flat;
            StartServer.Font = new Font("Segoe UI", 9F);
            StartServer.ForeColor = Color.White;
            StartServer.Image = (Image)resources.GetObject("StartServer.Image");
            StartServer.Location = new Point(597, 18);
            StartServer.Name = "StartServer";
            StartServer.Size = new Size(115, 25);
            StartServer.TabIndex = 3;
            StartServer.Text = "Start";
            StartServer.UseVisualStyleBackColor = false;
            StartServer.Click += StartServer_Click;
            // 
            // textBox5
            // 
            textBox5.BackColor = SystemColors.WindowText;
            textBox5.Font = new Font("Segoe UI", 10F);
            textBox5.ForeColor = SystemColors.Window;
            textBox5.Location = new Point(12, 12);
            textBox5.Multiline = true;
            textBox5.Name = "textBox5";
            textBox5.ReadOnly = true;
            textBox5.ScrollBars = ScrollBars.Vertical;
            textBox5.Size = new Size(350, 309);
            textBox5.TabIndex = 5;
            // 
            // splitter1
            // 
            splitter1.Location = new Point(0, 0);
            splitter1.Name = "splitter1";
            splitter1.Size = new Size(3, 746);
            splitter1.TabIndex = 33;
            splitter1.TabStop = false;
            // 
            // textBox2
            // 
            textBox2.BackColor = SystemColors.WindowText;
            textBox2.ForeColor = SystemColors.Window;
            textBox2.Location = new Point(580, 47);
            textBox2.Name = "textBox2";
            textBox2.ReadOnly = true;
            textBox2.Size = new Size(100, 23);
            textBox2.TabIndex = 47;
            textBox2.Text = "T";
            // 
            // textBox1
            // 
            textBox1.BackColor = SystemColors.WindowText;
            textBox1.ForeColor = SystemColors.Window;
            textBox1.Location = new Point(22, 49);
            textBox1.Name = "textBox1";
            textBox1.ReadOnly = true;
            textBox1.Size = new Size(100, 23);
            textBox1.TabIndex = 45;
            textBox1.Text = "Ct";
            // 
            // LoadJson
            // 
            LoadJson.BackColor = Color.Transparent;
            LoadJson.FlatAppearance.MouseDownBackColor = Color.Transparent;
            LoadJson.FlatAppearance.MouseOverBackColor = Color.Transparent;
            LoadJson.FlatStyle = FlatStyle.Flat;
            LoadJson.ForeColor = SystemColors.Control;
            LoadJson.Location = new Point(903, 643);
            LoadJson.Name = "LoadJson";
            LoadJson.Size = new Size(135, 79);
            LoadJson.TabIndex = 1;
            LoadJson.Text = "Загрузить набор музыки";
            LoadJson.UseVisualStyleBackColor = false;
            LoadJson.Click += LoadJson_Click;
            // 
            // MusicNumbers
            // 
            MusicNumbers.BackColor = Color.Black;
            MusicNumbers.ForeColor = SystemColors.Window;
            MusicNumbers.LargeImageList = imageList1;
            MusicNumbers.Location = new Point(9, 12);
            MusicNumbers.MultiSelect = false;
            MusicNumbers.Name = "MusicNumbers";
            MusicNumbers.Size = new Size(888, 606);
            MusicNumbers.TabIndex = 0;
            MusicNumbers.UseCompatibleStateImageBehavior = false;
            MusicNumbers.SelectedIndexChanged += MusicNumbers_SelectedIndexChanged;
            // 
            // imageList1
            // 
            imageList1.ColorDepth = ColorDepth.Depth32Bit;
            imageList1.ImageStream = (ImageListStreamer)resources.GetObject("imageList1.ImageStream");
            imageList1.TransparentColor = Color.Transparent;
            imageList1.Images.SetKeyName(0, "Коллаж-1.3.jpg");
            // 
            // ServerMenu
            // 
            ServerMenu.Controls.Add(DoubleMode);
            ServerMenu.Controls.Add(MainMusic);
            ServerMenu.Controls.Add(SaveSettings);
            ServerMenu.Controls.Add(Cs2Directory);
            ServerMenu.Controls.Add(textBox8);
            ServerMenu.Controls.Add(pictureBox1);
            ServerMenu.Controls.Add(textBox3);
            ServerMenu.Controls.Add(textBox5);
            ServerMenu.Controls.Add(StartServer);
            ServerMenu.Location = new Point(0, 0);
            ServerMenu.Name = "ServerMenu";
            ServerMenu.Size = new Size(1054, 785);
            ServerMenu.TabIndex = 48;
            // 
            // DoubleMode
            // 
            DoubleMode.AutoSize = true;
            DoubleMode.ForeColor = SystemColors.ControlLightLight;
            DoubleMode.Location = new Point(382, 157);
            DoubleMode.Name = "DoubleMode";
            DoubleMode.Size = new Size(331, 49);
            DoubleMode.TabIndex = 56;
            DoubleMode.Text = "Включение наборов музыки за каждую команду (Beta).\r\nВ ином случае основной набор выберите ниже\r\n\r\n";
            DoubleMode.UseVisualStyleBackColor = true;
            DoubleMode.CheckedChanged += DoubleMode_CheckedChanged;
            // 
            // MainMusic
            // 
            MainMusic.BackColor = SystemColors.WindowText;
            MainMusic.ForeColor = SystemColors.Window;
            MainMusic.FormattingEnabled = true;
            MainMusic.Location = new Point(382, 210);
            MainMusic.Name = "MainMusic";
            MainMusic.Size = new Size(300, 23);
            MainMusic.TabIndex = 196;
            MainMusic.Text = "Выбрать";
            MainMusic.SelectedIndexChanged += MainMusic_SelectedIndexChanged;
            // 
            // SaveSettings
            // 
            SaveSettings.BackColor = SystemColors.ActiveCaptionText;
            SaveSettings.FlatStyle = FlatStyle.Flat;
            SaveSettings.ForeColor = SystemColors.ControlLightLight;
            SaveSettings.Location = new Point(858, 691);
            SaveSettings.Name = "SaveSettings";
            SaveSettings.Size = new Size(164, 31);
            SaveSettings.TabIndex = 55;
            SaveSettings.Text = "Сохранить настройки";
            SaveSettings.UseVisualStyleBackColor = false;
            SaveSettings.Click += SaveSettings_Click;
            // 
            // Cs2Directory
            // 
            Cs2Directory.FlatStyle = FlatStyle.Flat;
            Cs2Directory.ForeColor = SystemColors.Control;
            Cs2Directory.Location = new Point(595, 65);
            Cs2Directory.Name = "Cs2Directory";
            Cs2Directory.Size = new Size(97, 23);
            Cs2Directory.TabIndex = 54;
            Cs2Directory.Text = "Выбрать";
            Cs2Directory.UseVisualStyleBackColor = true;
            Cs2Directory.Click += Cs2Directory_Click;
            // 
            // textBox8
            // 
            textBox8.BackColor = SystemColors.InactiveCaptionText;
            textBox8.ForeColor = SystemColors.Window;
            textBox8.HideSelection = false;
            textBox8.Location = new Point(381, 65);
            textBox8.Multiline = true;
            textBox8.Name = "textBox8";
            textBox8.ReadOnly = true;
            textBox8.Size = new Size(193, 70);
            textBox8.TabIndex = 53;
            textBox8.Text = "Важно! Если вы пользуетесь программой первый раз. Выберите папку \"Game\" в директории кс 2  ";
            // 
            // pictureBox1
            // 
            pictureBox1.Image = (Image)resources.GetObject("pictureBox1.Image");
            pictureBox1.Location = new Point(554, 21);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(22, 22);
            pictureBox1.TabIndex = 50;
            pictureBox1.TabStop = false;
            // 
            // textBox3
            // 
            textBox3.BackColor = SystemColors.WindowText;
            textBox3.BorderStyle = BorderStyle.None;
            textBox3.Font = new Font("Sitka Text", 14.2499981F, FontStyle.Regular, GraphicsUnit.Point, 0);
            textBox3.ForeColor = SystemColors.Window;
            textBox3.HideSelection = false;
            textBox3.Location = new Point(383, 13);
            textBox3.Name = "textBox3";
            textBox3.ReadOnly = true;
            textBox3.Size = new Size(182, 24);
            textBox3.TabIndex = 49;
            textBox3.Text = "Статус сервера";
            // 
            // MusicKitMenu
            // 
            MusicKitMenu.Controls.Add(OpenMusicKitForm);
            MusicKitMenu.Controls.Add(MusicNumbers);
            MusicKitMenu.Controls.Add(LoadJson);
            MusicKitMenu.Location = new Point(0, 0);
            MusicKitMenu.Name = "MusicKitMenu";
            MusicKitMenu.Size = new Size(1051, 746);
            MusicKitMenu.TabIndex = 0;
            // 
            // OpenMusicKitForm
            // 
            OpenMusicKitForm.FlatAppearance.MouseDownBackColor = Color.Transparent;
            OpenMusicKitForm.FlatAppearance.MouseOverBackColor = Color.Transparent;
            OpenMusicKitForm.FlatStyle = FlatStyle.Flat;
            OpenMusicKitForm.ForeColor = SystemColors.Control;
            OpenMusicKitForm.ImageAlign = ContentAlignment.BottomCenter;
            OpenMusicKitForm.Location = new Point(706, 643);
            OpenMusicKitForm.Name = "OpenMusicKitForm";
            OpenMusicKitForm.Size = new Size(146, 79);
            OpenMusicKitForm.TabIndex = 2;
            OpenMusicKitForm.Text = "Создание/Мод. набора музыки";
            OpenMusicKitForm.UseVisualStyleBackColor = true;
            OpenMusicKitForm.Click += OpenMusicKitForm_Click;
            // 
            // imageList2
            // 
            imageList2.ColorDepth = ColorDepth.Depth32Bit;
            imageList2.ImageStream = (ImageListStreamer)resources.GetObject("imageList2.ImageStream");
            imageList2.TransparentColor = Color.Transparent;
            imageList2.Images.SetKeyName(0, "StatusOn.png");
            imageList2.Images.SetKeyName(1, "StatusOff.png");
            // 
            // Server
            // 
            Server.BackColor = Color.Black;
            Server.FlatAppearance.MouseDownBackColor = Color.Transparent;
            Server.FlatAppearance.MouseOverBackColor = Color.Transparent;
            Server.FlatStyle = FlatStyle.Flat;
            Server.Image = (Image)resources.GetObject("Server.Image");
            Server.Location = new Point(903, 8);
            Server.Name = "Server";
            Server.Size = new Size(135, 135);
            Server.TabIndex = 51;
            Server.UseVisualStyleBackColor = false;
            Server.Click += Server_Click;
            // 
            // Musickits
            // 
            Musickits.FlatAppearance.MouseDownBackColor = Color.Transparent;
            Musickits.FlatAppearance.MouseOverBackColor = Color.Transparent;
            Musickits.FlatStyle = FlatStyle.Flat;
            Musickits.Image = (Image)resources.GetObject("Musickits.Image");
            Musickits.Location = new Point(903, 153);
            Musickits.Name = "Musickits";
            Musickits.Size = new Size(135, 135);
            Musickits.TabIndex = 51;
            Musickits.UseVisualStyleBackColor = true;
            Musickits.Click += Musickits_Click;
            // 
            // Settings
            // 
            Settings.FlatAppearance.MouseDownBackColor = Color.Transparent;
            Settings.FlatAppearance.MouseOverBackColor = Color.Transparent;
            Settings.FlatStyle = FlatStyle.Flat;
            Settings.Image = (Image)resources.GetObject("Settings.Image");
            Settings.Location = new Point(903, 294);
            Settings.Name = "Settings";
            Settings.Size = new Size(135, 135);
            Settings.TabIndex = 52;
            Settings.UseVisualStyleBackColor = true;
            Settings.Click += Settings_Click;
            // 
            // SettingsMenu
            // 
            SettingsMenu.Controls.Add(MenuVolumeText);
            SettingsMenu.Controls.Add(MenuVolume);
            SettingsMenu.Controls.Add(textBox16);
            SettingsMenu.Controls.Add(Mode_select);
            SettingsMenu.Controls.Add(DeathcamVolume);
            SettingsMenu.Controls.Add(DeathcamVolumeText);
            SettingsMenu.Controls.Add(KillVolumeText);
            SettingsMenu.Controls.Add(EndGameVolume);
            SettingsMenu.Controls.Add(KillVolume);
            SettingsMenu.Controls.Add(TenSecondRoundVolume);
            SettingsMenu.Controls.Add(EndGameVolumeText);
            SettingsMenu.Controls.Add(TenSecondRoundVolumeText);
            SettingsMenu.Controls.Add(TenSecondBombVolume);
            SettingsMenu.Controls.Add(TenSecondBombVolumeText);
            SettingsMenu.Controls.Add(BombVolumeText);
            SettingsMenu.Controls.Add(BombVolume);
            SettingsMenu.Controls.Add(MvpVolumeText);
            SettingsMenu.Controls.Add(MvpVolume);
            SettingsMenu.Controls.Add(LoseRoundVolumeText);
            SettingsMenu.Controls.Add(StartGameVolumeText);
            SettingsMenu.Controls.Add(LoseRoundVolume);
            SettingsMenu.Controls.Add(StartGameVolume);
            SettingsMenu.Controls.Add(WinRoundVolumeText);
            SettingsMenu.Controls.Add(WinRoundVolume);
            SettingsMenu.Controls.Add(StartActionVolumeText);
            SettingsMenu.Controls.Add(StartActionVolume);
            SettingsMenu.Controls.Add(StartRoundVolume);
            SettingsMenu.Controls.Add(StartRoundVolumeText);
            SettingsMenu.Controls.Add(textBox10);
            SettingsMenu.Controls.Add(textBox9);
            SettingsMenu.Controls.Add(Bomb);
            SettingsMenu.Controls.Add(WinRound);
            SettingsMenu.Controls.Add(textBox11);
            SettingsMenu.Controls.Add(StartAction);
            SettingsMenu.Controls.Add(textBox15);
            SettingsMenu.Controls.Add(textBox13);
            SettingsMenu.Controls.Add(textBox4);
            SettingsMenu.Controls.Add(DeathCam);
            SettingsMenu.Controls.Add(LoseRound);
            SettingsMenu.Controls.Add(textBox12);
            SettingsMenu.Controls.Add(CtSide);
            SettingsMenu.Controls.Add(DeathMatch);
            SettingsMenu.Location = new Point(0, 0);
            SettingsMenu.Name = "SettingsMenu";
            SettingsMenu.Size = new Size(1057, 747);
            SettingsMenu.TabIndex = 57;
            SettingsMenu.Paint += SettingsMenu_Paint;
            // 
            // MenuVolumeText
            // 
            MenuVolumeText.Location = new Point(529, 121);
            MenuVolumeText.Name = "MenuVolumeText";
            MenuVolumeText.Size = new Size(45, 23);
            MenuVolumeText.TabIndex = 195;
            // 
            // MenuVolume
            // 
            MenuVolume.LargeChange = 1;
            MenuVolume.Location = new Point(125, 121);
            MenuVolume.Name = "MenuVolume";
            MenuVolume.Size = new Size(401, 23);
            MenuVolume.TabIndex = 194;
            MenuVolume.Value = 100;
            MenuVolume.Scroll += MenuVolume_Scroll;
            // 
            // textBox16
            // 
            textBox16.BackColor = SystemColors.ActiveCaptionText;
            textBox16.ForeColor = Color.White;
            textBox16.Location = new Point(22, 121);
            textBox16.Name = "textBox16";
            textBox16.Size = new Size(98, 23);
            textBox16.TabIndex = 193;
            textBox16.Text = "Menu";
            // 
            // Mode_select
            // 
            Mode_select.BackColor = Color.Black;
            Mode_select.ForeColor = SystemColors.Window;
            Mode_select.FormattingEnabled = true;
            Mode_select.Items.AddRange(new object[] { "Соревновательный / Премьер", "Обычный", "Бой насмерть", "Напарники" });
            Mode_select.Location = new Point(22, 20);
            Mode_select.Name = "Mode_select";
            Mode_select.Size = new Size(300, 23);
            Mode_select.TabIndex = 0;
            Mode_select.Text = "Режим";
            Mode_select.SelectedIndexChanged += Mode_SelectedIndexChanged;
            // 
            // DeathcamVolume
            // 
            DeathcamVolume.LargeChange = 1;
            DeathcamVolume.Location = new Point(125, 469);
            DeathcamVolume.Name = "DeathcamVolume";
            DeathcamVolume.Size = new Size(401, 23);
            DeathcamVolume.TabIndex = 190;
            DeathcamVolume.Value = 100;
            DeathcamVolume.Scroll += DeathcamVolume_Scroll;
            // 
            // DeathcamVolumeText
            // 
            DeathcamVolumeText.Location = new Point(529, 469);
            DeathcamVolumeText.Name = "DeathcamVolumeText";
            DeathcamVolumeText.Size = new Size(45, 23);
            DeathcamVolumeText.TabIndex = 191;
            // 
            // KillVolumeText
            // 
            KillVolumeText.Location = new Point(529, 440);
            KillVolumeText.Name = "KillVolumeText";
            KillVolumeText.Size = new Size(45, 23);
            KillVolumeText.TabIndex = 188;
            // 
            // EndGameVolume
            // 
            EndGameVolume.LargeChange = 1;
            EndGameVolume.Location = new Point(125, 411);
            EndGameVolume.Name = "EndGameVolume";
            EndGameVolume.Size = new Size(401, 23);
            EndGameVolume.TabIndex = 184;
            EndGameVolume.Value = 100;
            EndGameVolume.Scroll += EndGameVolume_Scroll;
            // 
            // KillVolume
            // 
            KillVolume.LargeChange = 1;
            KillVolume.Location = new Point(125, 440);
            KillVolume.Name = "KillVolume";
            KillVolume.Size = new Size(401, 23);
            KillVolume.TabIndex = 187;
            KillVolume.Value = 100;
            KillVolume.Scroll += KillVolume_Scroll;
            // 
            // TenSecondRoundVolume
            // 
            TenSecondRoundVolume.LargeChange = 1;
            TenSecondRoundVolume.Location = new Point(125, 382);
            TenSecondRoundVolume.Name = "TenSecondRoundVolume";
            TenSecondRoundVolume.Size = new Size(401, 23);
            TenSecondRoundVolume.TabIndex = 181;
            TenSecondRoundVolume.Value = 100;
            TenSecondRoundVolume.Scroll += TenSecondRoundVolume_Scroll;
            // 
            // EndGameVolumeText
            // 
            EndGameVolumeText.Location = new Point(529, 411);
            EndGameVolumeText.Name = "EndGameVolumeText";
            EndGameVolumeText.Size = new Size(45, 23);
            EndGameVolumeText.TabIndex = 185;
            // 
            // TenSecondRoundVolumeText
            // 
            TenSecondRoundVolumeText.Location = new Point(529, 382);
            TenSecondRoundVolumeText.Name = "TenSecondRoundVolumeText";
            TenSecondRoundVolumeText.Size = new Size(45, 23);
            TenSecondRoundVolumeText.TabIndex = 182;
            // 
            // TenSecondBombVolume
            // 
            TenSecondBombVolume.LargeChange = 1;
            TenSecondBombVolume.Location = new Point(125, 353);
            TenSecondBombVolume.Name = "TenSecondBombVolume";
            TenSecondBombVolume.Size = new Size(401, 23);
            TenSecondBombVolume.TabIndex = 178;
            TenSecondBombVolume.Value = 100;
            TenSecondBombVolume.Scroll += TenSecondBombVolume_Scroll;
            // 
            // TenSecondBombVolumeText
            // 
            TenSecondBombVolumeText.Location = new Point(529, 353);
            TenSecondBombVolumeText.Name = "TenSecondBombVolumeText";
            TenSecondBombVolumeText.Size = new Size(45, 23);
            TenSecondBombVolumeText.TabIndex = 179;
            // 
            // BombVolumeText
            // 
            BombVolumeText.Location = new Point(529, 324);
            BombVolumeText.Name = "BombVolumeText";
            BombVolumeText.Size = new Size(45, 23);
            BombVolumeText.TabIndex = 176;
            // 
            // BombVolume
            // 
            BombVolume.LargeChange = 1;
            BombVolume.Location = new Point(125, 324);
            BombVolume.Name = "BombVolume";
            BombVolume.Size = new Size(401, 23);
            BombVolume.TabIndex = 175;
            BombVolume.Value = 100;
            BombVolume.Scroll += BombVolume_Scroll;
            // 
            // MvpVolumeText
            // 
            MvpVolumeText.Location = new Point(529, 295);
            MvpVolumeText.Name = "MvpVolumeText";
            MvpVolumeText.Size = new Size(45, 23);
            MvpVolumeText.TabIndex = 173;
            // 
            // MvpVolume
            // 
            MvpVolume.LargeChange = 1;
            MvpVolume.Location = new Point(125, 295);
            MvpVolume.Name = "MvpVolume";
            MvpVolume.Size = new Size(401, 23);
            MvpVolume.TabIndex = 172;
            MvpVolume.Value = 100;
            MvpVolume.Scroll += MvpVolume_Scroll;
            // 
            // LoseRoundVolumeText
            // 
            LoseRoundVolumeText.Location = new Point(529, 266);
            LoseRoundVolumeText.Name = "LoseRoundVolumeText";
            LoseRoundVolumeText.Size = new Size(45, 23);
            LoseRoundVolumeText.TabIndex = 170;
            // 
            // StartGameVolumeText
            // 
            StartGameVolumeText.Location = new Point(529, 150);
            StartGameVolumeText.Name = "StartGameVolumeText";
            StartGameVolumeText.Size = new Size(45, 23);
            StartGameVolumeText.TabIndex = 110;
            // 
            // LoseRoundVolume
            // 
            LoseRoundVolume.LargeChange = 1;
            LoseRoundVolume.Location = new Point(125, 266);
            LoseRoundVolume.Name = "LoseRoundVolume";
            LoseRoundVolume.Size = new Size(401, 23);
            LoseRoundVolume.TabIndex = 169;
            LoseRoundVolume.Value = 100;
            LoseRoundVolume.Scroll += LoseRoundVolume_Scroll;
            // 
            // StartGameVolume
            // 
            StartGameVolume.LargeChange = 1;
            StartGameVolume.Location = new Point(125, 150);
            StartGameVolume.Name = "StartGameVolume";
            StartGameVolume.Size = new Size(401, 23);
            StartGameVolume.TabIndex = 109;
            StartGameVolume.Value = 100;
            StartGameVolume.Scroll += StartGameVolume_Scroll;
            // 
            // WinRoundVolumeText
            // 
            WinRoundVolumeText.Location = new Point(529, 237);
            WinRoundVolumeText.Name = "WinRoundVolumeText";
            WinRoundVolumeText.Size = new Size(45, 23);
            WinRoundVolumeText.TabIndex = 131;
            // 
            // WinRoundVolume
            // 
            WinRoundVolume.LargeChange = 1;
            WinRoundVolume.Location = new Point(125, 237);
            WinRoundVolume.Name = "WinRoundVolume";
            WinRoundVolume.Size = new Size(401, 23);
            WinRoundVolume.TabIndex = 130;
            WinRoundVolume.Value = 100;
            WinRoundVolume.Scroll += WinRoundVolume_Scroll;
            // 
            // StartActionVolumeText
            // 
            StartActionVolumeText.Location = new Point(529, 208);
            StartActionVolumeText.Name = "StartActionVolumeText";
            StartActionVolumeText.Size = new Size(45, 23);
            StartActionVolumeText.TabIndex = 124;
            // 
            // StartActionVolume
            // 
            StartActionVolume.LargeChange = 1;
            StartActionVolume.Location = new Point(125, 208);
            StartActionVolume.Name = "StartActionVolume";
            StartActionVolume.Size = new Size(401, 23);
            StartActionVolume.TabIndex = 123;
            StartActionVolume.Value = 100;
            StartActionVolume.Scroll += StartActionVolume_Scroll;
            // 
            // StartRoundVolume
            // 
            StartRoundVolume.LargeChange = 1;
            StartRoundVolume.Location = new Point(125, 179);
            StartRoundVolume.Name = "StartRoundVolume";
            StartRoundVolume.Size = new Size(401, 23);
            StartRoundVolume.TabIndex = 116;
            StartRoundVolume.Value = 100;
            StartRoundVolume.Scroll += StartRoundVolume_Scroll;
            // 
            // StartRoundVolumeText
            // 
            StartRoundVolumeText.Location = new Point(529, 179);
            StartRoundVolumeText.Name = "StartRoundVolumeText";
            StartRoundVolumeText.Size = new Size(45, 23);
            StartRoundVolumeText.TabIndex = 117;
            // 
            // textBox10
            // 
            textBox10.BackColor = SystemColors.ActiveCaptionText;
            textBox10.ForeColor = Color.White;
            textBox10.Location = new Point(22, 150);
            textBox10.Name = "textBox10";
            textBox10.Size = new Size(98, 23);
            textBox10.TabIndex = 98;
            textBox10.Text = "StartGame";
            // 
            // textBox9
            // 
            textBox9.BackColor = SystemColors.ActiveCaptionText;
            textBox9.ForeColor = Color.White;
            textBox9.Location = new Point(22, 295);
            textBox9.Name = "textBox9";
            textBox9.Size = new Size(97, 23);
            textBox9.TabIndex = 100;
            textBox9.Text = "MVP";
            // 
            // Bomb
            // 
            Bomb.BackColor = SystemColors.ActiveCaptionText;
            Bomb.ForeColor = Color.White;
            Bomb.Location = new Point(22, 324);
            Bomb.Name = "Bomb";
            Bomb.Size = new Size(97, 23);
            Bomb.TabIndex = 92;
            Bomb.Text = "Bomb";
            // 
            // WinRound
            // 
            WinRound.BackColor = SystemColors.ActiveCaptionText;
            WinRound.ForeColor = Color.White;
            WinRound.Location = new Point(21, 237);
            WinRound.Name = "WinRound";
            WinRound.Size = new Size(98, 23);
            WinRound.TabIndex = 86;
            WinRound.Text = "WinRound";
            // 
            // textBox11
            // 
            textBox11.BackColor = SystemColors.ActiveCaptionText;
            textBox11.ForeColor = Color.White;
            textBox11.Location = new Point(21, 353);
            textBox11.Name = "textBox11";
            textBox11.Size = new Size(98, 23);
            textBox11.TabIndex = 102;
            textBox11.Text = "TenSecond";
            // 
            // StartAction
            // 
            StartAction.BackColor = SystemColors.ActiveCaptionText;
            StartAction.ForeColor = Color.White;
            StartAction.Location = new Point(22, 208);
            StartAction.Name = "StartAction";
            StartAction.Size = new Size(97, 23);
            StartAction.TabIndex = 94;
            StartAction.Text = "StartAction";
            // 
            // textBox15
            // 
            textBox15.BackColor = SystemColors.ActiveCaptionText;
            textBox15.ForeColor = Color.White;
            textBox15.Location = new Point(21, 382);
            textBox15.Name = "textBox15";
            textBox15.Size = new Size(98, 23);
            textBox15.TabIndex = 108;
            textBox15.Text = "TenSecondRound";
            // 
            // textBox13
            // 
            textBox13.BackColor = SystemColors.ActiveCaptionText;
            textBox13.ForeColor = Color.White;
            textBox13.Location = new Point(22, 411);
            textBox13.Name = "textBox13";
            textBox13.Size = new Size(97, 23);
            textBox13.TabIndex = 106;
            textBox13.Text = "EndGame";
            // 
            // textBox4
            // 
            textBox4.BackColor = SystemColors.ActiveCaptionText;
            textBox4.ForeColor = Color.White;
            textBox4.Location = new Point(21, 179);
            textBox4.Name = "textBox4";
            textBox4.Size = new Size(98, 23);
            textBox4.TabIndex = 96;
            textBox4.Text = "StartRound";
            // 
            // DeathCam
            // 
            DeathCam.BackColor = SystemColors.ActiveCaptionText;
            DeathCam.ForeColor = Color.White;
            DeathCam.Location = new Point(21, 469);
            DeathCam.Name = "DeathCam";
            DeathCam.Size = new Size(98, 23);
            DeathCam.TabIndex = 88;
            DeathCam.Text = "DeathCam";
            // 
            // LoseRound
            // 
            LoseRound.BackColor = SystemColors.ActiveCaptionText;
            LoseRound.ForeColor = Color.White;
            LoseRound.Location = new Point(21, 266);
            LoseRound.Name = "LoseRound";
            LoseRound.Size = new Size(98, 23);
            LoseRound.TabIndex = 90;
            LoseRound.Text = "LoseRound";
            // 
            // textBox12
            // 
            textBox12.BackColor = SystemColors.ActiveCaptionText;
            textBox12.ForeColor = Color.White;
            textBox12.Location = new Point(21, 440);
            textBox12.Name = "textBox12";
            textBox12.Size = new Size(98, 23);
            textBox12.TabIndex = 104;
            textBox12.Text = "KillSound";
            // 
            // CtSide
            // 
            CtSide.BackColor = SystemColors.WindowText;
            CtSide.ForeColor = SystemColors.Window;
            CtSide.FormattingEnabled = true;
            CtSide.Location = new Point(22, 76);
            CtSide.Name = "CtSide";
            CtSide.Size = new Size(300, 23);
            CtSide.TabIndex = 44;
            CtSide.Text = "Выбрать";
            CtSide.SelectedIndexChanged += CtSide_SelectedIndexChanged;
            // 
            // DeathMatch
            // 
            DeathMatch.Controls.Add(textBox1);
            DeathMatch.Controls.Add(textBox2);
            DeathMatch.Controls.Add(TSide);
            DeathMatch.Location = new Point(0, 0);
            DeathMatch.Name = "DeathMatch";
            DeathMatch.Size = new Size(1048, 115);
            DeathMatch.TabIndex = 192;
            // 
            // TSide
            // 
            TSide.BackColor = SystemColors.WindowText;
            TSide.ForeColor = SystemColors.Window;
            TSide.FormattingEnabled = true;
            TSide.Location = new Point(580, 76);
            TSide.Name = "TSide";
            TSide.RightToLeft = RightToLeft.No;
            TSide.Size = new Size(317, 23);
            TSide.TabIndex = 46;
            TSide.Text = "Выбрать";
            TSide.SelectedIndexChanged += TSide_SelectedIndexChanged;
            // 
            // Main
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.ActiveCaptionText;
            ClientSize = new Size(1050, 746);
            Controls.Add(Server);
            Controls.Add(Musickits);
            Controls.Add(Settings);
            Controls.Add(ServerMenu);
            Controls.Add(SettingsMenu);
            Controls.Add(MusicKitMenu);
            Controls.Add(splitter1);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            Name = "Main";
            Text = "Form1";
            ServerMenu.ResumeLayout(false);
            ServerMenu.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            MusicKitMenu.ResumeLayout(false);
            SettingsMenu.ResumeLayout(false);
            SettingsMenu.PerformLayout();
            DeathMatch.ResumeLayout(false);
            DeathMatch.PerformLayout();
            ResumeLayout(false);
        }

        #endregion
        private Button StartServer;
        private TextBox textBox5;
        private Splitter splitter1;
        private Button LoadJson;
        private TextBox textBox2;
        private TextBox textBox1;
        private Panel ServerMenu;
        private Panel MusicKitMenu;
        private TextBox textBox3;
        private PictureBox pictureBox1;
        private ImageList imageList2;
        private Button Server;
        private Button Musickits;
        private Button OpenMusicKitForm;
        public ImageList imageList1;
        public ListView MusicNumbers;
        private Button Cs2Directory;
        private TextBox textBox8;
        private Button SaveSettings;
        private CheckBox DoubleMode;
        private Button Settings;
        private Panel SettingsMenu;
        private ComboBox Mode_select;
        public ComboBox TSide;
        public ComboBox CtSide;
        private TextBox textBox10;
        private TextBox textBox9;
        private TextBox textBox15;
        private TextBox textBox11;
        private TextBox StartAction;
        private TextBox textBox12;
        private TextBox WinRound;
        private TextBox textBox13;
        private TextBox Bomb;
        private TextBox textBox4;
        private TextBox LoseRound;
        private TextBox DeathCam;
        private TextBox StartGameVolumeText;
        private TextBox StartRoundVolumeText;
        private TextBox StartActionVolumeText;
        private TextBox DeathcamVolumeText;
        private TextBox KillVolumeText;
        private TextBox EndGameVolumeText;
        private TextBox TenSecondRoundVolumeText;
        private TextBox TenSecondBombVolumeText;
        private TextBox BombVolumeText;
        private TextBox MvpVolumeText;
        private TextBox LoseRoundVolumeText;
        private TextBox WinRoundVolumeText;
        private HScrollBar DeathcamVolume;
        private HScrollBar EndGameVolume;
        private HScrollBar KillVolume;
        private HScrollBar TenSecondRoundVolume;
        private HScrollBar TenSecondBombVolume;
        private HScrollBar BombVolume;
        private HScrollBar MvpVolume;
        private HScrollBar LoseRoundVolume;
        private HScrollBar StartGameVolume;
        private HScrollBar WinRoundVolume;
        private HScrollBar StartActionVolume;
        private HScrollBar StartRoundVolume;
        private Panel DeathMatch;
        private TextBox MenuVolumeText;
        private HScrollBar MenuVolume;
        private TextBox textBox16;
        public ComboBox MainMusic;
    }
}
