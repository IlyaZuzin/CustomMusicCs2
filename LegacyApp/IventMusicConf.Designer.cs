namespace CustomMusic
{
    partial class IventMusicConf
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(IventMusicConf));
            LeftList = new ListBox();
            keep = new PictureBox();
            SaveSettings = new Button();
            SingleMode = new Panel();
            SingleDelete = new Button();
            SingleList = new ListBox();
            SingleAdd = new Button();
            DoubleMode = new Panel();
            RightIventsName = new TextBox();
            RightList = new ListBox();
            RightAdd = new Button();
            LeftDelete = new Button();
            RightDelete = new Button();
            AddLeft = new Button();
            VolumeText = new TextBox();
            LeftIventsName = new TextBox();
            Volume = new TrackBar();
            VolumeTe = new TextBox();
            ((System.ComponentModel.ISupportInitialize)keep).BeginInit();
            SingleMode.SuspendLayout();
            DoubleMode.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)Volume).BeginInit();
            SuspendLayout();
            // 
            // LeftList
            // 
            LeftList.FormattingEnabled = true;
            LeftList.Location = new Point(12, 27);
            LeftList.Name = "LeftList";
            LeftList.Size = new Size(303, 304);
            LeftList.TabIndex = 2;
            LeftList.SelectedIndexChanged += LeftList_SelectedIndexChanged;
            // 
            // keep
            // 
            keep.Image = (Image)resources.GetObject("keep.Image");
            keep.Location = new Point(331, 32);
            keep.Name = "keep";
            keep.Size = new Size(35, 35);
            keep.SizeMode = PictureBoxSizeMode.StretchImage;
            keep.TabIndex = 3;
            keep.TabStop = false;
            keep.Click += keep_Click;
            // 
            // SaveSettings
            // 
            SaveSettings.Location = new Point(549, 368);
            SaveSettings.Name = "SaveSettings";
            SaveSettings.Size = new Size(137, 23);
            SaveSettings.TabIndex = 10;
            SaveSettings.Text = "Сохранить";
            SaveSettings.UseVisualStyleBackColor = true;
            SaveSettings.Click += SaveSettings_Click;
            // 
            // SingleMode
            // 
            SingleMode.Controls.Add(SingleDelete);
            SingleMode.Controls.Add(SingleList);
            SingleMode.Controls.Add(SingleAdd);
            SingleMode.Location = new Point(0, 0);
            SingleMode.Name = "SingleMode";
            SingleMode.Size = new Size(704, 362);
            SingleMode.TabIndex = 11;
            // 
            // SingleDelete
            // 
            SingleDelete.Location = new Point(617, 61);
            SingleDelete.Name = "SingleDelete";
            SingleDelete.Size = new Size(75, 23);
            SingleDelete.TabIndex = 18;
            SingleDelete.Text = "Удалить";
            SingleDelete.UseVisualStyleBackColor = true;
            SingleDelete.Click += SingleDelete_Click;
            // 
            // SingleList
            // 
            SingleList.FormattingEnabled = true;
            SingleList.Location = new Point(12, 32);
            SingleList.Name = "SingleList";
            SingleList.Size = new Size(599, 319);
            SingleList.TabIndex = 13;
            SingleList.SelectedIndexChanged += SingleList_SelectedIndexChanged;
            // 
            // SingleAdd
            // 
            SingleAdd.Location = new Point(617, 32);
            SingleAdd.Name = "SingleAdd";
            SingleAdd.Size = new Size(75, 23);
            SingleAdd.TabIndex = 17;
            SingleAdd.Text = "Добавить";
            SingleAdd.UseVisualStyleBackColor = true;
            SingleAdd.Click += SingleAdd_Click;
            // 
            // DoubleMode
            // 
            DoubleMode.Controls.Add(RightIventsName);
            DoubleMode.Controls.Add(RightList);
            DoubleMode.Controls.Add(RightAdd);
            DoubleMode.Controls.Add(LeftDelete);
            DoubleMode.Controls.Add(RightDelete);
            DoubleMode.Controls.Add(AddLeft);
            DoubleMode.Controls.Add(keep);
            DoubleMode.Controls.Add(LeftList);
            DoubleMode.Location = new Point(0, 0);
            DoubleMode.Name = "DoubleMode";
            DoubleMode.Size = new Size(704, 362);
            DoubleMode.TabIndex = 0;
            // 
            // RightIventsName
            // 
            RightIventsName.Location = new Point(383, 3);
            RightIventsName.Name = "RightIventsName";
            RightIventsName.Size = new Size(100, 23);
            RightIventsName.TabIndex = 19;
            RightIventsName.Text = "Громкость";
            // 
            // RightList
            // 
            RightList.FormattingEnabled = true;
            RightList.Location = new Point(383, 27);
            RightList.Name = "RightList";
            RightList.Size = new Size(303, 304);
            RightList.TabIndex = 14;
            // 
            // RightAdd
            // 
            RightAdd.Location = new Point(530, 337);
            RightAdd.Name = "RightAdd";
            RightAdd.Size = new Size(75, 23);
            RightAdd.TabIndex = 13;
            RightAdd.Text = "Добавить";
            RightAdd.UseVisualStyleBackColor = true;
            RightAdd.Click += RightAdd_Click;
            // 
            // LeftDelete
            // 
            LeftDelete.Location = new Point(93, 337);
            LeftDelete.Name = "LeftDelete";
            LeftDelete.Size = new Size(75, 23);
            LeftDelete.TabIndex = 12;
            LeftDelete.Text = "Удалить";
            LeftDelete.UseVisualStyleBackColor = true;
            LeftDelete.Click += LeftDelete_Click;
            // 
            // RightDelete
            // 
            RightDelete.Location = new Point(611, 337);
            RightDelete.Name = "RightDelete";
            RightDelete.Size = new Size(75, 23);
            RightDelete.TabIndex = 11;
            RightDelete.Text = "Удалить";
            RightDelete.UseVisualStyleBackColor = true;
            RightDelete.Click += RightDelete_Click;
            // 
            // AddLeft
            // 
            AddLeft.Location = new Point(12, 337);
            AddLeft.Name = "AddLeft";
            AddLeft.Size = new Size(75, 23);
            AddLeft.TabIndex = 10;
            AddLeft.Text = "Добавить";
            AddLeft.UseVisualStyleBackColor = true;
            AddLeft.Click += AddLeft_Click;
            // 
            // VolumeText
            // 
            VolumeText.Location = new Point(390, 369);
            VolumeText.Name = "VolumeText";
            VolumeText.Size = new Size(100, 23);
            VolumeText.TabIndex = 15;
            // 
            // LeftIventsName
            // 
            LeftIventsName.Location = new Point(12, 3);
            LeftIventsName.Name = "LeftIventsName";
            LeftIventsName.Size = new Size(100, 23);
            LeftIventsName.TabIndex = 16;
            LeftIventsName.Text = "Громкость";
            // 
            // Volume
            // 
            Volume.Location = new Point(118, 368);
            Volume.Maximum = 100;
            Volume.Name = "Volume";
            Volume.Size = new Size(257, 45);
            Volume.TabIndex = 17;
            Volume.Scroll += Volume_Scroll;
            // 
            // VolumeTe
            // 
            VolumeTe.Location = new Point(12, 369);
            VolumeTe.Name = "VolumeTe";
            VolumeTe.Size = new Size(100, 23);
            VolumeTe.TabIndex = 18;
            VolumeTe.Text = "Громкость";
            // 
            // IventMusicConf
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(704, 401);
            Controls.Add(VolumeText);
            Controls.Add(SaveSettings);
            Controls.Add(VolumeTe);
            Controls.Add(LeftIventsName);
            Controls.Add(Volume);
            Controls.Add(DoubleMode);
            Controls.Add(SingleMode);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "IventMusicConf";
            Text = "Настройка";
            ((System.ComponentModel.ISupportInitialize)keep).EndInit();
            SingleMode.ResumeLayout(false);
            DoubleMode.ResumeLayout(false);
            DoubleMode.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)Volume).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private ListBox listBox1;
        private ListBox listBox2;
        private ListBox LeftList;
        private PictureBox keep;
        private Button SaveSettings;
        private Panel SingleMode;
        private Panel DoubleMode;
        private Button RightAdd;
        private Button LeftDelete;
        private Button RightDelete;
        private Button AddLeft;
        private Button SingleDelete;
        private ListBox SingleList;
        private Button SingleAdd;
        private ListBox RightList;
        private TextBox VolumeText;
        private TextBox LeftIventsName;
        private TextBox VolumeTe;
        private TrackBar Volume;
        private TextBox RightIventsName;
    }
}