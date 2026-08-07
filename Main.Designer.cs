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
            trackBar1 = new TrackBar();
            textBox2 = new TextBox();
            TSide = new ComboBox();
            textBox1 = new TextBox();
            CtSide = new ComboBox();
            textBox7 = new TextBox();
            textBox6 = new TextBox();
            CtImage = new PictureBox();
            LoadJson = new Button();
            MusicNumbers = new ListView();
            imageList1 = new ImageList(components);
            ServerMenu = new Panel();
            DoubleMode = new CheckBox();
            SaveSettings = new Button();
            Cs2Directory = new Button();
            textBox8 = new TextBox();
            pictureBox1 = new PictureBox();
            textBox3 = new TextBox();
            TImage = new PictureBox();
            MusicKitMenu = new Panel();
            OpenMusicKitForm = new Button();
            imageList2 = new ImageList(components);
            Server = new Button();
            Musickits = new Button();
            ((System.ComponentModel.ISupportInitialize)trackBar1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)CtImage).BeginInit();
            ServerMenu.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)TImage).BeginInit();
            MusicKitMenu.SuspendLayout();
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
            StartServer.Location = new Point(595, 65);
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
            textBox5.Size = new Size(350, 301);
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
            // trackBar1
            // 
            trackBar1.Location = new Point(488, 14);
            trackBar1.Maximum = 100;
            trackBar1.Name = "trackBar1";
            trackBar1.Size = new Size(279, 45);
            trackBar1.SmallChange = 5;
            trackBar1.TabIndex = 34;
            trackBar1.Value = 100;
            trackBar1.Scroll += trackBar1_Scroll;
            // 
            // textBox2
            // 
            textBox2.BackColor = SystemColors.WindowText;
            textBox2.ForeColor = SystemColors.Window;
            textBox2.Location = new Point(392, 334);
            textBox2.Name = "textBox2";
            textBox2.ReadOnly = true;
            textBox2.Size = new Size(100, 23);
            textBox2.TabIndex = 47;
            textBox2.Text = "T";
            // 
            // TSide
            // 
            TSide.BackColor = SystemColors.WindowText;
            TSide.ForeColor = SystemColors.Window;
            TSide.FormattingEnabled = true;
            TSide.Location = new Point(392, 368);
            TSide.Name = "TSide";
            TSide.RightToLeft = RightToLeft.No;
            TSide.Size = new Size(300, 23);
            TSide.TabIndex = 46;
            TSide.Text = "Выбрать";
            TSide.SelectedIndexChanged += TSide_SelectedIndexChanged;
            // 
            // textBox1
            // 
            textBox1.BackColor = SystemColors.WindowText;
            textBox1.ForeColor = SystemColors.Window;
            textBox1.Location = new Point(22, 334);
            textBox1.Name = "textBox1";
            textBox1.ReadOnly = true;
            textBox1.Size = new Size(100, 23);
            textBox1.TabIndex = 45;
            textBox1.Text = "Ct";
            // 
            // CtSide
            // 
            CtSide.BackColor = SystemColors.WindowText;
            CtSide.ForeColor = SystemColors.Window;
            CtSide.FormattingEnabled = true;
            CtSide.Location = new Point(22, 368);
            CtSide.Name = "CtSide";
            CtSide.Size = new Size(300, 23);
            CtSide.TabIndex = 44;
            CtSide.Text = "Выбрать";
            CtSide.SelectedIndexChanged += CtSide_SelectedIndexChanged;
            // 
            // textBox7
            // 
            textBox7.BackColor = SystemColors.WindowText;
            textBox7.ForeColor = SystemColors.Window;
            textBox7.Location = new Point(773, 14);
            textBox7.Name = "textBox7";
            textBox7.Size = new Size(65, 23);
            textBox7.TabIndex = 42;
            // 
            // textBox6
            // 
            textBox6.BackColor = SystemColors.WindowText;
            textBox6.Font = new Font("Segoe UI", 12F);
            textBox6.ForeColor = SystemColors.Window;
            textBox6.HideSelection = false;
            textBox6.Location = new Point(382, 14);
            textBox6.Multiline = true;
            textBox6.Name = "textBox6";
            textBox6.ReadOnly = true;
            textBox6.Size = new Size(100, 29);
            textBox6.TabIndex = 41;
            textBox6.Text = "Громкость";
            // 
            // CtImage
            // 
            CtImage.BorderStyle = BorderStyle.Fixed3D;
            CtImage.Location = new Point(22, 422);
            CtImage.Name = "CtImage";
            CtImage.Size = new Size(300, 300);
            CtImage.SizeMode = PictureBoxSizeMode.StretchImage;
            CtImage.TabIndex = 38;
            CtImage.TabStop = false;
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
            ServerMenu.Controls.Add(SaveSettings);
            ServerMenu.Controls.Add(Cs2Directory);
            ServerMenu.Controls.Add(textBox8);
            ServerMenu.Controls.Add(pictureBox1);
            ServerMenu.Controls.Add(textBox3);
            ServerMenu.Controls.Add(TImage);
            ServerMenu.Controls.Add(textBox5);
            ServerMenu.Controls.Add(trackBar1);
            ServerMenu.Controls.Add(CtImage);
            ServerMenu.Controls.Add(StartServer);
            ServerMenu.Controls.Add(textBox1);
            ServerMenu.Controls.Add(textBox6);
            ServerMenu.Controls.Add(textBox7);
            ServerMenu.Controls.Add(CtSide);
            ServerMenu.Controls.Add(TSide);
            ServerMenu.Controls.Add(textBox2);
            ServerMenu.Location = new Point(0, 0);
            ServerMenu.Name = "ServerMenu";
            ServerMenu.Size = new Size(1054, 785);
            ServerMenu.TabIndex = 48;
            ServerMenu.Paint += ServerMenu_Paint;
            // 
            // DoubleMode
            // 
            DoubleMode.AutoSize = true;
            DoubleMode.ForeColor = SystemColors.ControlLightLight;
            DoubleMode.Location = new Point(382, 245);
            DoubleMode.Name = "DoubleMode";
            DoubleMode.Size = new Size(331, 34);
            DoubleMode.TabIndex = 56;
            DoubleMode.Text = "Включение наборов музыки за каждую команду (Beta).\r\nВ ином случае основной музыкой является Ct\r\n";
            DoubleMode.UseVisualStyleBackColor = true;
            DoubleMode.CheckedChanged += DoubleMode_CheckedChanged;
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
            Cs2Directory.Location = new Point(595, 153);
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
            textBox8.Location = new Point(381, 153);
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
            pictureBox1.Location = new Point(552, 68);
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
            textBox3.Location = new Point(381, 60);
            textBox3.Name = "textBox3";
            textBox3.ReadOnly = true;
            textBox3.Size = new Size(182, 24);
            textBox3.TabIndex = 49;
            textBox3.Text = "Статус сервера";
            // 
            // TImage
            // 
            TImage.BorderStyle = BorderStyle.Fixed3D;
            TImage.Location = new Point(392, 422);
            TImage.Name = "TImage";
            TImage.Size = new Size(300, 300);
            TImage.SizeMode = PictureBoxSizeMode.StretchImage;
            TImage.TabIndex = 48;
            TImage.TabStop = false;
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
            Server.Location = new Point(903, 12);
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
            // Main
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.ActiveCaptionText;
            ClientSize = new Size(1050, 746);
            Controls.Add(Musickits);
            Controls.Add(Server);
            Controls.Add(ServerMenu);
            Controls.Add(MusicKitMenu);
            Controls.Add(splitter1);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            Name = "Main";
            Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)trackBar1).EndInit();
            ((System.ComponentModel.ISupportInitialize)CtImage).EndInit();
            ServerMenu.ResumeLayout(false);
            ServerMenu.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ((System.ComponentModel.ISupportInitialize)TImage).EndInit();
            MusicKitMenu.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion
        private Button StartServer;
        private TextBox textBox5;
        private Splitter splitter1;
        private TrackBar trackBar1;
        private PictureBox CtImage;
        private Button LoadJson;
        private TextBox textBox7;
        private TextBox textBox6;
        private TextBox textBox2;
        private TextBox textBox1;
        private Panel ServerMenu;
        private Panel MusicKitMenu;
        private PictureBox TImage;
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
        public ComboBox CtSide;
        public ComboBox TSide;
    }
}
