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
            splitter1 = new Splitter();
            imageList1 = new ImageList(components);
            imageList2 = new ImageList(components);
            Menu = new Panel();
            Console = new Button();
            Server = new Button();
            KeepMusic = new PictureBox();
            SoloMod = new Panel();
            SoloList = new ListView();
            SoloModMk = new Button();
            SoloImage = new PictureBox();
            SoloSelectMk = new Button();
            textBox2 = new TextBox();
            SoloMK = new TextBox();
            TWorkStation = new Panel();
            TList = new ListView();
            TModMK = new Button();
            TSelectMK = new Button();
            TNameMK = new TextBox();
            textBox32 = new TextBox();
            TImageMK = new PictureBox();
            textBox30 = new TextBox();
            CTWorkStation = new Panel();
            CtList = new ListView();
            CTModMK = new Button();
            CTSelectMK = new Button();
            CTNameMK = new TextBox();
            textBox31 = new TextBox();
            CTImageMK = new PictureBox();
            Select = new TextBox();
            textBox28 = new TextBox();
            textBox27 = new TextBox();
            OpenFolder = new Button();
            CurrectMK = new TextBox();
            textBox17 = new TextBox();
            ProfileText = new TextBox();
            textBox6 = new TextBox();
            Profiles = new ComboBox();
            Library = new Panel();
            Python = new Button();
            OpenMusicKitForm = new Button();
            MusicNumbers = new ListView();
            LoadJson = new Button();
            textBox19 = new TextBox();
            textBox20 = new TextBox();
            textBox21 = new TextBox();
            textBox22 = new TextBox();
            textBox23 = new TextBox();
            textBox24 = new TextBox();
            textBox25 = new TextBox();
            textBox26 = new TextBox();
            imageList3 = new ImageList(components);
            pictureBox4 = new PictureBox();
            pictureBox5 = new PictureBox();
            HomePage = new Button();
            LibraryPage = new Button();
            WonRound = new TextBox();
            LostRound = new TextBox();
            Death = new TextBox();
            Kill = new TextBox();
            Kill_Hs = new TextBox();
            KD = new TextBox();
            MVP = new TextBox();
            bindingSource1 = new BindingSource(components);
            Menu.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)KeepMusic).BeginInit();
            SoloMod.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)SoloImage).BeginInit();
            TWorkStation.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)TImageMK).BeginInit();
            CTWorkStation.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)CTImageMK).BeginInit();
            Library.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox4).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox5).BeginInit();
            ((System.ComponentModel.ISupportInitialize)bindingSource1).BeginInit();
            SuspendLayout();
            // 
            // splitter1
            // 
            splitter1.Location = new Point(0, 0);
            splitter1.Name = "splitter1";
            splitter1.Size = new Size(3, 765);
            splitter1.TabIndex = 33;
            splitter1.TabStop = false;
            // 
            // imageList1
            // 
            imageList1.ColorDepth = ColorDepth.Depth32Bit;
            imageList1.ImageStream = (ImageListStreamer)resources.GetObject("imageList1.ImageStream");
            imageList1.TransparentColor = Color.Transparent;
            imageList1.Images.SetKeyName(0, "Коллаж-1.3.jpg");
            // 
            // imageList2
            // 
            imageList2.ColorDepth = ColorDepth.Depth32Bit;
            imageList2.ImageStream = (ImageListStreamer)resources.GetObject("imageList2.ImageStream");
            imageList2.TransparentColor = Color.Transparent;
            imageList2.Images.SetKeyName(0, "StatusOn.png");
            imageList2.Images.SetKeyName(1, "StatusOff.png");
            // 
            // Menu
            // 
            Menu.Controls.Add(Console);
            Menu.Controls.Add(Server);
            Menu.Controls.Add(KeepMusic);
            Menu.Controls.Add(SoloMod);
            Menu.Controls.Add(TWorkStation);
            Menu.Controls.Add(CTWorkStation);
            Menu.Controls.Add(textBox28);
            Menu.Controls.Add(textBox27);
            Menu.Controls.Add(OpenFolder);
            Menu.Controls.Add(CurrectMK);
            Menu.Controls.Add(textBox17);
            Menu.Location = new Point(0, 0);
            Menu.Name = "Menu";
            Menu.Size = new Size(1200, 800);
            Menu.TabIndex = 34;
            Menu.Paint += Menu_Paint;
            // 
            // Console
            // 
            Console.Location = new Point(9, 699);
            Console.Name = "Console";
            Console.Size = new Size(194, 23);
            Console.TabIndex = 51;
            Console.Text = "OpenConsole";
            Console.UseVisualStyleBackColor = true;
            Console.Click += button1_Click_1;
            // 
            // Server
            // 
            Server.Location = new Point(133, 19);
            Server.Name = "Server";
            Server.Size = new Size(201, 23);
            Server.TabIndex = 50;
            Server.Text = "StartServer";
            Server.UseVisualStyleBackColor = true;
            Server.Click += button1_Click;
            // 
            // KeepMusic
            // 
            KeepMusic.Image = (Image)resources.GetObject("KeepMusic.Image");
            KeepMusic.Location = new Point(958, 76);
            KeepMusic.Name = "KeepMusic";
            KeepMusic.Size = new Size(23, 23);
            KeepMusic.SizeMode = PictureBoxSizeMode.StretchImage;
            KeepMusic.TabIndex = 0;
            KeepMusic.TabStop = false;
            KeepMusic.Click += KeepMusic_Click;
            // 
            // SoloMod
            // 
            SoloMod.Controls.Add(SoloList);
            SoloMod.Controls.Add(SoloModMk);
            SoloMod.Controls.Add(SoloImage);
            SoloMod.Controls.Add(SoloSelectMk);
            SoloMod.Controls.Add(textBox2);
            SoloMod.Controls.Add(SoloMK);
            SoloMod.Location = new Point(218, 105);
            SoloMod.Name = "SoloMod";
            SoloMod.Size = new Size(461, 582);
            SoloMod.TabIndex = 52;
            SoloMod.Visible = false;
            // 
            // SoloList
            // 
            SoloList.AllowDrop = true;
            SoloList.BackColor = Color.Black;
            SoloList.ForeColor = Color.Transparent;
            SoloList.LargeImageList = imageList1;
            SoloList.Location = new Point(17, 96);
            SoloList.MultiSelect = false;
            SoloList.Name = "SoloList";
            SoloList.Size = new Size(428, 431);
            SoloList.TabIndex = 62;
            SoloList.UseCompatibleStateImageBehavior = false;
            SoloList.Visible = false;
            // 
            // SoloModMk
            // 
            SoloModMk.Location = new Point(285, 541);
            SoloModMk.Name = "SoloModMk";
            SoloModMk.Size = new Size(160, 23);
            SoloModMk.TabIndex = 64;
            SoloModMk.Text = "Изменить набор";
            SoloModMk.UseVisualStyleBackColor = true;
            SoloModMk.Click += SoloModMk_Click;
            // 
            // SoloImage
            // 
            SoloImage.ErrorImage = (Image)resources.GetObject("SoloImage.ErrorImage");
            SoloImage.Location = new Point(16, 96);
            SoloImage.Name = "SoloImage";
            SoloImage.Size = new Size(429, 429);
            SoloImage.SizeMode = PictureBoxSizeMode.StretchImage;
            SoloImage.TabIndex = 59;
            SoloImage.TabStop = false;
            // 
            // SoloSelectMk
            // 
            SoloSelectMk.Location = new Point(16, 541);
            SoloSelectMk.Name = "SoloSelectMk";
            SoloSelectMk.Size = new Size(160, 23);
            SoloSelectMk.TabIndex = 63;
            SoloSelectMk.Text = "Сменить набор";
            SoloSelectMk.UseVisualStyleBackColor = true;
            SoloSelectMk.Click += SoloSelectMk_Click;
            // 
            // textBox2
            // 
            textBox2.Location = new Point(16, 57);
            textBox2.Name = "textBox2";
            textBox2.Size = new Size(90, 23);
            textBox2.TabIndex = 60;
            textBox2.Text = "Набор музыки";
            // 
            // SoloMK
            // 
            SoloMK.Location = new Point(116, 57);
            SoloMK.Name = "SoloMK";
            SoloMK.Size = new Size(329, 23);
            SoloMK.TabIndex = 61;
            // 
            // TWorkStation
            // 
            TWorkStation.BackColor = Color.Transparent;
            TWorkStation.BackgroundImage = (Image)resources.GetObject("TWorkStation.BackgroundImage");
            TWorkStation.Controls.Add(TList);
            TWorkStation.Controls.Add(TModMK);
            TWorkStation.Controls.Add(TSelectMK);
            TWorkStation.Controls.Add(TNameMK);
            TWorkStation.Controls.Add(textBox32);
            TWorkStation.Controls.Add(TImageMK);
            TWorkStation.Controls.Add(textBox30);
            TWorkStation.Location = new Point(676, 105);
            TWorkStation.Name = "TWorkStation";
            TWorkStation.Size = new Size(464, 582);
            TWorkStation.TabIndex = 39;
            // 
            // TList
            // 
            TList.AllowDrop = true;
            TList.BackColor = Color.Black;
            TList.ForeColor = Color.Transparent;
            TList.LargeImageList = imageList1;
            TList.Location = new Point(19, 96);
            TList.MultiSelect = false;
            TList.Name = "TList";
            TList.Size = new Size(428, 431);
            TList.TabIndex = 56;
            TList.UseCompatibleStateImageBehavior = false;
            TList.Visible = false;
            TList.SelectedIndexChanged += TList_SelectedIndexChanged;
            // 
            // TModMK
            // 
            TModMK.Location = new Point(287, 541);
            TModMK.Name = "TModMK";
            TModMK.Size = new Size(160, 23);
            TModMK.TabIndex = 57;
            TModMK.Text = "Изменить набор";
            TModMK.UseVisualStyleBackColor = true;
            TModMK.Click += TModMK_Click;
            // 
            // TSelectMK
            // 
            TSelectMK.Location = new Point(18, 541);
            TSelectMK.Name = "TSelectMK";
            TSelectMK.Size = new Size(160, 23);
            TSelectMK.TabIndex = 56;
            TSelectMK.Text = "Сменить набор";
            TSelectMK.UseVisualStyleBackColor = true;
            TSelectMK.Click += TSelectMK_Click;
            // 
            // TNameMK
            // 
            TNameMK.Location = new Point(118, 57);
            TNameMK.Name = "TNameMK";
            TNameMK.Size = new Size(329, 23);
            TNameMK.TabIndex = 54;
            // 
            // textBox32
            // 
            textBox32.Location = new Point(18, 57);
            textBox32.Name = "textBox32";
            textBox32.Size = new Size(90, 23);
            textBox32.TabIndex = 53;
            textBox32.Text = "Набор музыки";
            // 
            // TImageMK
            // 
            TImageMK.ErrorImage = (Image)resources.GetObject("TImageMK.ErrorImage");
            TImageMK.Location = new Point(18, 96);
            TImageMK.Name = "TImageMK";
            TImageMK.Size = new Size(429, 429);
            TImageMK.SizeMode = PictureBoxSizeMode.StretchImage;
            TImageMK.TabIndex = 52;
            TImageMK.TabStop = false;
            // 
            // textBox30
            // 
            textBox30.Location = new Point(18, 21);
            textBox30.Name = "textBox30";
            textBox30.Size = new Size(90, 23);
            textBox30.TabIndex = 1;
            textBox30.Text = "Т сторона";
            // 
            // CTWorkStation
            // 
            CTWorkStation.BackColor = Color.Transparent;
            CTWorkStation.BackgroundImage = (Image)resources.GetObject("CTWorkStation.BackgroundImage");
            CTWorkStation.Controls.Add(CtList);
            CTWorkStation.Controls.Add(CTModMK);
            CTWorkStation.Controls.Add(CTSelectMK);
            CTWorkStation.Controls.Add(CTNameMK);
            CTWorkStation.Controls.Add(textBox31);
            CTWorkStation.Controls.Add(CTImageMK);
            CTWorkStation.Controls.Add(Select);
            CTWorkStation.Location = new Point(215, 105);
            CTWorkStation.Name = "CTWorkStation";
            CTWorkStation.Size = new Size(464, 582);
            CTWorkStation.TabIndex = 38;
            // 
            // CtList
            // 
            CtList.AllowDrop = true;
            CtList.BackColor = Color.Black;
            CtList.ForeColor = Color.Transparent;
            CtList.LargeImageList = imageList1;
            CtList.Location = new Point(16, 96);
            CtList.MultiSelect = false;
            CtList.Name = "CtList";
            CtList.Size = new Size(428, 431);
            CtList.TabIndex = 4;
            CtList.UseCompatibleStateImageBehavior = false;
            CtList.Visible = false;
            // 
            // CTModMK
            // 
            CTModMK.Location = new Point(284, 541);
            CTModMK.Name = "CTModMK";
            CTModMK.Size = new Size(160, 23);
            CTModMK.TabIndex = 55;
            CTModMK.Text = "Изменить набор";
            CTModMK.UseVisualStyleBackColor = true;
            CTModMK.Click += CTModMK_Click;
            // 
            // CTSelectMK
            // 
            CTSelectMK.Location = new Point(15, 541);
            CTSelectMK.Name = "CTSelectMK";
            CTSelectMK.Size = new Size(160, 23);
            CTSelectMK.TabIndex = 54;
            CTSelectMK.Text = "Сменить набор";
            CTSelectMK.UseVisualStyleBackColor = true;
            CTSelectMK.Click += CTSelectMK_Click;
            // 
            // CTNameMK
            // 
            CTNameMK.Location = new Point(115, 57);
            CTNameMK.Name = "CTNameMK";
            CTNameMK.Size = new Size(329, 23);
            CTNameMK.TabIndex = 53;
            // 
            // textBox31
            // 
            textBox31.Location = new Point(15, 57);
            textBox31.Name = "textBox31";
            textBox31.Size = new Size(90, 23);
            textBox31.TabIndex = 52;
            textBox31.Text = "Набор музыки";
            // 
            // CTImageMK
            // 
            CTImageMK.ErrorImage = (Image)resources.GetObject("CTImageMK.ErrorImage");
            CTImageMK.Location = new Point(15, 96);
            CTImageMK.Name = "CTImageMK";
            CTImageMK.Size = new Size(429, 429);
            CTImageMK.SizeMode = PictureBoxSizeMode.StretchImage;
            CTImageMK.TabIndex = 51;
            CTImageMK.TabStop = false;
            // 
            // Select
            // 
            Select.Location = new Point(15, 21);
            Select.Name = "Select";
            Select.Size = new Size(90, 23);
            Select.TabIndex = 0;
            Select.Text = "КТ сторона";
            // 
            // textBox28
            // 
            textBox28.Location = new Point(987, 76);
            textBox28.Name = "textBox28";
            textBox28.Size = new Size(153, 23);
            textBox28.TabIndex = 49;
            textBox28.Text = "Связать наборы музыки";
            // 
            // textBox27
            // 
            textBox27.Location = new Point(230, 76);
            textBox27.Name = "textBox27";
            textBox27.Size = new Size(160, 23);
            textBox27.TabIndex = 0;
            textBox27.Text = "Конфигуратор наборов";
            // 
            // OpenFolder
            // 
            OpenFolder.Location = new Point(778, 701);
            OpenFolder.Name = "OpenFolder";
            OpenFolder.Size = new Size(161, 23);
            OpenFolder.TabIndex = 40;
            OpenFolder.Text = "Открыть папку набора ";
            OpenFolder.UseVisualStyleBackColor = true;
            OpenFolder.Click += OpenFolder_Click;
            // 
            // CurrectMK
            // 
            CurrectMK.Location = new Point(327, 701);
            CurrectMK.Name = "CurrectMK";
            CurrectMK.Size = new Size(433, 23);
            CurrectMK.TabIndex = 39;
            // 
            // textBox17
            // 
            textBox17.Location = new Point(212, 700);
            textBox17.Multiline = true;
            textBox17.Name = "textBox17";
            textBox17.ReadOnly = true;
            textBox17.Size = new Size(96, 24);
            textBox17.TabIndex = 38;
            textBox17.Text = "Сейчас играет";
            textBox17.TextChanged += textBox17_TextChanged;
            // 
            // ProfileText
            // 
            ProfileText.Font = new Font("Segoe UI", 10F);
            ProfileText.Location = new Point(521, 17);
            ProfileText.Multiline = true;
            ProfileText.Name = "ProfileText";
            ProfileText.Size = new Size(96, 25);
            ProfileText.TabIndex = 0;
            ProfileText.Text = "Профиль";
            // 
            // textBox6
            // 
            textBox6.Font = new Font("Segoe UI", 10F);
            textBox6.Location = new Point(68, 17);
            textBox6.Multiline = true;
            textBox6.Name = "textBox6";
            textBox6.Size = new Size(41, 25);
            textBox6.TabIndex = 1;
            textBox6.Text = "[GSI] status";
            textBox6.TextChanged += textBox6_TextChanged;
            // 
            // Profiles
            // 
            Profiles.FlatStyle = FlatStyle.Flat;
            Profiles.Font = new Font("Segoe UI", 10F);
            Profiles.FormattingEnabled = true;
            Profiles.Location = new Point(647, 17);
            Profiles.Name = "Profiles";
            Profiles.Size = new Size(493, 25);
            Profiles.TabIndex = 2;
            // 
            // Library
            // 
            Library.Controls.Add(Python);
            Library.Controls.Add(OpenMusicKitForm);
            Library.Controls.Add(MusicNumbers);
            Library.Controls.Add(LoadJson);
            Library.Location = new Point(0, 0);
            Library.Name = "Library";
            Library.Size = new Size(1185, 765);
            Library.TabIndex = 37;
            // 
            // Python
            // 
            Python.BackColor = Color.Transparent;
            Python.FlatAppearance.MouseDownBackColor = Color.Transparent;
            Python.FlatAppearance.MouseOverBackColor = Color.Transparent;
            Python.FlatStyle = FlatStyle.Flat;
            Python.ForeColor = SystemColors.Control;
            Python.ImageAlign = ContentAlignment.BottomCenter;
            Python.Location = new Point(1026, 657);
            Python.Name = "Python";
            Python.Size = new Size(146, 79);
            Python.TabIndex = 3;
            Python.Text = "Создать набор на основе файлов от Valve (Python)";
            Python.UseVisualStyleBackColor = false;
            Python.Click += Python_Click;
            // 
            // OpenMusicKitForm
            // 
            OpenMusicKitForm.BackColor = Color.Transparent;
            OpenMusicKitForm.FlatAppearance.MouseDownBackColor = Color.Transparent;
            OpenMusicKitForm.FlatAppearance.MouseOverBackColor = Color.Transparent;
            OpenMusicKitForm.FlatStyle = FlatStyle.Flat;
            OpenMusicKitForm.ForeColor = SystemColors.Control;
            OpenMusicKitForm.ImageAlign = ContentAlignment.BottomCenter;
            OpenMusicKitForm.Location = new Point(860, 657);
            OpenMusicKitForm.Name = "OpenMusicKitForm";
            OpenMusicKitForm.Size = new Size(146, 79);
            OpenMusicKitForm.TabIndex = 2;
            OpenMusicKitForm.Text = "Создание/Мод. набора музыки";
            OpenMusicKitForm.UseVisualStyleBackColor = false;
            OpenMusicKitForm.Click += OpenMusicKitForm_Click;
            // 
            // MusicNumbers
            // 
            MusicNumbers.AllowDrop = true;
            MusicNumbers.BackColor = Color.Black;
            MusicNumbers.ForeColor = Color.Transparent;
            MusicNumbers.LargeImageList = imageList1;
            MusicNumbers.Location = new Point(212, 48);
            MusicNumbers.MultiSelect = false;
            MusicNumbers.Name = "MusicNumbers";
            MusicNumbers.Size = new Size(960, 603);
            MusicNumbers.TabIndex = 0;
            MusicNumbers.UseCompatibleStateImageBehavior = false;
            MusicNumbers.SelectedIndexChanged += MusicNumbers_SelectedIndexChanged;
            // 
            // LoadJson
            // 
            LoadJson.BackColor = Color.Transparent;
            LoadJson.FlatAppearance.MouseDownBackColor = Color.Transparent;
            LoadJson.FlatAppearance.MouseOverBackColor = Color.Transparent;
            LoadJson.FlatStyle = FlatStyle.Flat;
            LoadJson.ForeColor = SystemColors.Control;
            LoadJson.Location = new Point(694, 657);
            LoadJson.Name = "LoadJson";
            LoadJson.Size = new Size(146, 79);
            LoadJson.TabIndex = 1;
            LoadJson.Text = "Загрузить набор музыки";
            LoadJson.UseVisualStyleBackColor = false;
            LoadJson.Click += LoadJson_Click;
            // 
            // textBox19
            // 
            textBox19.Location = new Point(13, 209);
            textBox19.Name = "textBox19";
            textBox19.Size = new Size(193, 23);
            textBox19.TabIndex = 41;
            textBox19.Text = "Статистика за сеанс";
            // 
            // textBox20
            // 
            textBox20.Location = new Point(13, 238);
            textBox20.Name = "textBox20";
            textBox20.Size = new Size(115, 23);
            textBox20.TabIndex = 42;
            textBox20.Text = "Выйграно раундов";
            // 
            // textBox21
            // 
            textBox21.Location = new Point(13, 267);
            textBox21.Name = "textBox21";
            textBox21.Size = new Size(115, 23);
            textBox21.TabIndex = 43;
            textBox21.Text = "Проиграно раундов";
            // 
            // textBox22
            // 
            textBox22.Location = new Point(13, 296);
            textBox22.Name = "textBox22";
            textBox22.Size = new Size(115, 23);
            textBox22.TabIndex = 44;
            textBox22.Text = "Смертей";
            // 
            // textBox23
            // 
            textBox23.Location = new Point(13, 325);
            textBox23.Name = "textBox23";
            textBox23.Size = new Size(115, 23);
            textBox23.TabIndex = 45;
            textBox23.Text = "Убийств";
            // 
            // textBox24
            // 
            textBox24.Location = new Point(13, 354);
            textBox24.Name = "textBox24";
            textBox24.Size = new Size(115, 23);
            textBox24.TabIndex = 46;
            textBox24.Text = "Убийств в голову";
            // 
            // textBox25
            // 
            textBox25.Location = new Point(13, 383);
            textBox25.Name = "textBox25";
            textBox25.Size = new Size(115, 23);
            textBox25.TabIndex = 47;
            textBox25.Text = "Средний к/д";
            // 
            // textBox26
            // 
            textBox26.Location = new Point(13, 412);
            textBox26.Name = "textBox26";
            textBox26.Size = new Size(115, 23);
            textBox26.TabIndex = 48;
            textBox26.Text = "MVP";
            // 
            // imageList3
            // 
            imageList3.ColorDepth = ColorDepth.Depth32Bit;
            imageList3.ImageSize = new Size(16, 16);
            imageList3.TransparentColor = Color.Transparent;
            // 
            // pictureBox4
            // 
            pictureBox4.Image = (Image)resources.GetObject("pictureBox4.Image");
            pictureBox4.InitialImage = (Image)resources.GetObject("pictureBox4.InitialImage");
            pictureBox4.Location = new Point(13, 76);
            pictureBox4.Name = "pictureBox4";
            pictureBox4.Size = new Size(39, 39);
            pictureBox4.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox4.TabIndex = 50;
            pictureBox4.TabStop = false;
            pictureBox4.Click += pictureBox4_Click;
            // 
            // pictureBox5
            // 
            pictureBox5.Image = (Image)resources.GetObject("pictureBox5.Image");
            pictureBox5.Location = new Point(13, 139);
            pictureBox5.Name = "pictureBox5";
            pictureBox5.Size = new Size(39, 39);
            pictureBox5.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox5.TabIndex = 51;
            pictureBox5.TabStop = false;
            // 
            // HomePage
            // 
            HomePage.Font = new Font("Segoe UI", 15F);
            HomePage.Location = new Point(68, 76);
            HomePage.Name = "HomePage";
            HomePage.Size = new Size(129, 39);
            HomePage.TabIndex = 52;
            HomePage.Text = "Главная ";
            HomePage.UseVisualStyleBackColor = true;
            HomePage.Click += HomePage_Click;
            // 
            // LibraryPage
            // 
            LibraryPage.Font = new Font("Segoe UI", 15F);
            LibraryPage.Location = new Point(68, 139);
            LibraryPage.Name = "LibraryPage";
            LibraryPage.Size = new Size(129, 39);
            LibraryPage.TabIndex = 54;
            LibraryPage.Text = "Библиотека";
            LibraryPage.UseVisualStyleBackColor = true;
            LibraryPage.Click += LibraryPage_Click;
            // 
            // WonRound
            // 
            WonRound.Location = new Point(134, 238);
            WonRound.Name = "WonRound";
            WonRound.Size = new Size(72, 23);
            WonRound.TabIndex = 50;
            // 
            // LostRound
            // 
            LostRound.Location = new Point(134, 267);
            LostRound.Name = "LostRound";
            LostRound.Size = new Size(72, 23);
            LostRound.TabIndex = 51;
            // 
            // Death
            // 
            Death.Location = new Point(134, 296);
            Death.Name = "Death";
            Death.Size = new Size(72, 23);
            Death.TabIndex = 52;
            // 
            // Kill
            // 
            Kill.Location = new Point(133, 325);
            Kill.Name = "Kill";
            Kill.Size = new Size(73, 23);
            Kill.TabIndex = 53;
            // 
            // Kill_Hs
            // 
            Kill_Hs.Location = new Point(133, 354);
            Kill_Hs.Name = "Kill_Hs";
            Kill_Hs.Size = new Size(73, 23);
            Kill_Hs.TabIndex = 54;
            // 
            // KD
            // 
            KD.Location = new Point(133, 383);
            KD.Name = "KD";
            KD.Size = new Size(73, 23);
            KD.TabIndex = 55;
            // 
            // MVP
            // 
            MVP.Location = new Point(133, 412);
            MVP.Name = "MVP";
            MVP.Size = new Size(73, 23);
            MVP.TabIndex = 56;
            // 
            // Main
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.ActiveCaptionText;
            ClientSize = new Size(1184, 765);
            Controls.Add(ProfileText);
            Controls.Add(textBox6);
            Controls.Add(textBox19);
            Controls.Add(textBox25);
            Controls.Add(textBox26);
            Controls.Add(textBox24);
            Controls.Add(textBox23);
            Controls.Add(textBox22);
            Controls.Add(textBox21);
            Controls.Add(textBox20);
            Controls.Add(HomePage);
            Controls.Add(LibraryPage);
            Controls.Add(Profiles);
            Controls.Add(pictureBox4);
            Controls.Add(pictureBox5);
            Controls.Add(MVP);
            Controls.Add(KD);
            Controls.Add(Kill_Hs);
            Controls.Add(Death);
            Controls.Add(Kill);
            Controls.Add(LostRound);
            Controls.Add(WonRound);
            Controls.Add(Menu);
            Controls.Add(Library);
            Controls.Add(splitter1);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            Name = "Main";
            Text = "Form1";
            Menu.ResumeLayout(false);
            Menu.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)KeepMusic).EndInit();
            SoloMod.ResumeLayout(false);
            SoloMod.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)SoloImage).EndInit();
            TWorkStation.ResumeLayout(false);
            TWorkStation.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)TImageMK).EndInit();
            CTWorkStation.ResumeLayout(false);
            CTWorkStation.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)CTImageMK).EndInit();
            Library.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)pictureBox4).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox5).EndInit();
            ((System.ComponentModel.ISupportInitialize)bindingSource1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private Splitter splitter1;
        private ImageList imageList2;
        public ImageList imageList1;
        private Panel Menu;
        private TextBox ProfileText;
        private ComboBox Profiles;
        private TextBox textBox6;
        private TextBox textBox17;
        private TextBox textBox19;
        private Button OpenFolder;
        private TextBox textBox28;
        private TextBox textBox27;
        private TextBox textBox26;
        private TextBox textBox25;
        private TextBox textBox24;
        private TextBox textBox23;
        private TextBox textBox22;
        private TextBox textBox21;
        private TextBox textBox20;
        private Panel TWorkStation;
        private Panel CTWorkStation;
        private PictureBox KeepMusic;
        private PictureBox pictureBox5;
        private Button TModMK;
        private PictureBox pictureBox4;
        private Button TSelectMK;
        private TextBox TNameMK;
        private TextBox textBox32;
        private PictureBox TImageMK;
        private TextBox textBox30;
        private Button CTModMK;
        private Button CTSelectMK;
        private TextBox CTNameMK;
        private TextBox textBox31;
        private PictureBox CTImageMK;
        private TextBox Select;
        private ImageList imageList3;
        private Button HomePage;
        private Button LibraryPage;
        private Button LoadJson;
        public ListView MusicNumbers;
        private Button OpenMusicKitForm;
        private Button Python;
        private Panel Library;
        private BindingSource bindingSource1;
        private Button Server;
        public TextBox WonRound;
        public TextBox LostRound;
        public TextBox Death;
        public TextBox Kill;
        public TextBox Kill_Hs;
        public TextBox KD;
        public TextBox MVP;
        public ListView TList;
        public ListView CtList;
        private Button Console;
        public TextBox CurrectMK;
        private Panel SoloMod;
        public ListView SoloList;
        private Button SoloModMk;
        private PictureBox SoloImage;
        private Button SoloSelectMk;
        private TextBox textBox2;
        private TextBox SoloMK;
    }
}
