namespace PswgLauncher
{
    partial class LAUNCHOPTIONS
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
        	this.label1 = new System.Windows.Forms.Label();
        	this.label2 = new System.Windows.Forms.Label();
        	this.soundControl = new System.Windows.Forms.CheckBox();
        	this.checksumControl = new System.Windows.Forms.CheckBox();
        	this.linkDebugWindow = new System.Windows.Forms.LinkLabel();
        	this.checkBoxLocalhost = new System.Windows.Forms.CheckBox();
        	this.linkMissingFiles = new System.Windows.Forms.LinkLabel();
        	this.checkBoxResume = new System.Windows.Forms.CheckBox();
        	this.SuspendLayout();
        	// 
        	// label1
        	// 
        	this.label1.AutoSize = true;
        	this.label1.BackColor = System.Drawing.Color.Transparent;
        	this.label1.ForeColor = System.Drawing.Color.Crimson;
        	this.label1.Location = new System.Drawing.Point(188, 329);
        	this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
        	this.label1.Name = "label1";
        	this.label1.Size = new System.Drawing.Size(0, 13);
        	this.label1.TabIndex = 16;
        	// 
        	// label2
        	// 
        	this.label2.AutoSize = true;
        	this.label2.BackColor = System.Drawing.Color.Transparent;
        	this.label2.ForeColor = System.Drawing.Color.Aqua;
        	this.label2.Location = new System.Drawing.Point(338, 329);
        	this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
        	this.label2.Name = "label2";
        	this.label2.Size = new System.Drawing.Size(0, 13);
        	this.label2.TabIndex = 17;
        	// 
        	// soundControl
        	// 
        	this.soundControl.BackColor = System.Drawing.Color.Transparent;
        	this.soundControl.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
        	this.soundControl.Location = new System.Drawing.Point(126, 228);
        	this.soundControl.Name = "soundControl";
        	this.soundControl.Size = new System.Drawing.Size(104, 24);
        	this.soundControl.TabIndex = 18;
        	this.soundControl.Text = "Enable Sound";
        	this.soundControl.UseVisualStyleBackColor = false;
        	this.soundControl.CheckedChanged += new System.EventHandler(this.SoundControlCheckedChanged);
        	// 
        	// checksumControl
        	// 
        	this.checksumControl.BackColor = System.Drawing.Color.Transparent;
        	this.checksumControl.Location = new System.Drawing.Point(126, 258);
        	this.checksumControl.Name = "checksumControl";
        	this.checksumControl.Size = new System.Drawing.Size(212, 24);
        	this.checksumControl.TabIndex = 19;
        	this.checksumControl.Text = "Always check existing files checksums";
        	this.checksumControl.UseMnemonic = false;
        	this.checksumControl.UseVisualStyleBackColor = false;
        	this.checksumControl.CheckedChanged += new System.EventHandler(this.ChecksumControlCheckedChanged);
        	// 
        	// linkDebugWindow
        	// 
        	this.linkDebugWindow.BackColor = System.Drawing.Color.Transparent;
        	this.linkDebugWindow.Location = new System.Drawing.Point(126, 348);
        	this.linkDebugWindow.Name = "linkDebugWindow";
        	this.linkDebugWindow.Size = new System.Drawing.Size(123, 18);
        	this.linkDebugWindow.TabIndex = 20;
        	this.linkDebugWindow.TabStop = true;
        	this.linkDebugWindow.Text = "Open Debug Window";
        	this.linkDebugWindow.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.LinkDebugWindowLinkClicked);
        	// 
        	// checkBoxLocalhost
        	// 
        	this.checkBoxLocalhost.BackColor = System.Drawing.Color.Transparent;
        	this.checkBoxLocalhost.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
        	this.checkBoxLocalhost.Location = new System.Drawing.Point(126, 318);
        	this.checkBoxLocalhost.Name = "checkBoxLocalhost";
        	this.checkBoxLocalhost.Size = new System.Drawing.Size(212, 24);
        	this.checkBoxLocalhost.TabIndex = 21;
        	this.checkBoxLocalhost.Text = "Connect Localhost (Dev only option)";
        	this.checkBoxLocalhost.UseVisualStyleBackColor = false;
        	this.checkBoxLocalhost.CheckedChanged += new System.EventHandler(this.CheckBoxLocalhostCheckedChanged);
        	// 
        	// linkMissingFiles
        	// 
        	this.linkMissingFiles.BackColor = System.Drawing.Color.Transparent;
        	this.linkMissingFiles.Location = new System.Drawing.Point(126, 366);
        	this.linkMissingFiles.Name = "linkMissingFiles";
        	this.linkMissingFiles.Size = new System.Drawing.Size(123, 18);
        	this.linkMissingFiles.TabIndex = 22;
        	this.linkMissingFiles.TabStop = true;
        	this.linkMissingFiles.Text = "List Missing Files";
        	this.linkMissingFiles.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.LinkMissingFilesClicked);
        	// 
        	// checkBoxResume
        	// 
        	this.checkBoxResume.BackColor = System.Drawing.Color.Transparent;
        	this.checkBoxResume.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
        	this.checkBoxResume.Location = new System.Drawing.Point(126, 288);
        	this.checkBoxResume.Name = "checkBoxResume";
        	this.checkBoxResume.Size = new System.Drawing.Size(158, 24);
        	this.checkBoxResume.TabIndex = 23;
        	this.checkBoxResume.Text = "Try to resume downloads";
        	this.checkBoxResume.UseVisualStyleBackColor = false;
        	this.checkBoxResume.CheckedChanged += new System.EventHandler(this.CheckBoxResumeCheckedChanged);
        	// 
        	// LAUNCHOPTIONS
        	// 
        	this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
        	this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        	this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
        	this.ClientSize = new System.Drawing.Size(562, 409);
        	this.Controls.Add(this.checkBoxResume);
        	this.Controls.Add(this.linkMissingFiles);
        	this.Controls.Add(this.checkBoxLocalhost);
        	this.Controls.Add(this.linkDebugWindow);
        	this.Controls.Add(this.checksumControl);
        	this.Controls.Add(this.soundControl);
        	this.Controls.Add(this.label2);
        	this.Controls.Add(this.label1);
        	this.DoubleBuffered = true;
        	this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
        	this.Margin = new System.Windows.Forms.Padding(2);
        	this.Name = "LAUNCHOPTIONS";
        	this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
        	this.Text = "Launcher Options";
        	this.Load += new System.EventHandler(this.LAUNCHOPTIONS_Load);
        	this.MouseClick += new System.Windows.Forms.MouseEventHandler(this.LAUNCHOPTIONS_MouseClick);
        	this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.LAUNCHOPTIONS_MouseDown);
        	this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.LAUNCHOPTIONS_MouseMove);
        	this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.LAUNCHOPTIONS_MouseUp);
        	this.ResumeLayout(false);
        	this.PerformLayout();
        }
        private System.Windows.Forms.CheckBox checkBoxResume;
        private System.Windows.Forms.LinkLabel linkMissingFiles;
        private System.Windows.Forms.CheckBox checkBoxLocalhost;
        private System.Windows.Forms.LinkLabel linkDebugWindow;
        private System.Windows.Forms.CheckBox checksumControl;
        private System.Windows.Forms.CheckBox soundControl;

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
    }
}