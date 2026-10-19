namespace CustomMusic
{
    partial class ConsoleLog
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
            ConsoleLogBox = new TextBox();
            Command = new TextBox();
            Json = new TextBox();
            SuspendLayout();
            // 
            // ConsoleLogBox
            // 
            ConsoleLogBox.Location = new Point(12, 11);
            ConsoleLogBox.Multiline = true;
            ConsoleLogBox.Name = "ConsoleLogBox";
            ConsoleLogBox.ScrollBars = ScrollBars.Both;
            ConsoleLogBox.Size = new Size(541, 582);
            ConsoleLogBox.TabIndex = 0;
            // 
            // Command
            // 
            Command.Location = new Point(12, 599);
            Command.Multiline = true;
            Command.Name = "Command";
            Command.Size = new Size(682, 34);
            Command.TabIndex = 1;
            // 
            // Json
            // 
            Json.Location = new Point(569, 11);
            Json.Multiline = true;
            Json.Name = "Json";
            Json.ScrollBars = ScrollBars.Both;
            Json.Size = new Size(428, 582);
            Json.TabIndex = 2;
            // 
            // ConsoleLog
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1009, 650);
            Controls.Add(Json);
            Controls.Add(Command);
            Controls.Add(ConsoleLogBox);
            Name = "ConsoleLog";
            Text = "Console";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox ConsoleLogBox;
        private TextBox Command;
        private TextBox Json;
    }
}