namespace PswgLauncher
{
    partial class DirSearch
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
        	this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
        	this.textBox2 = new System.Windows.Forms.TextBox();
        	this.textBox1 = new System.Windows.Forms.TextBox();
        	this.SuspendLayout();
        	// 
        	// textBox2
        	// 
        	this.textBox2.BackColor = System.Drawing.Color.White;
        	this.textBox2.Location = new System.Drawing.Point(41, 275);
        	this.textBox2.Margin = new System.Windows.Forms.Padding(2);
        	this.textBox2.Name = "textBox2";
        	this.textBox2.ReadOnly = true;
        	this.textBox2.Size = new System.Drawing.Size(290, 20);
        	this.textBox2.TabIndex = 7;
        	this.textBox2.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
        	// 
        	// textBox1
        	// 
        	this.textBox1.BackColor = System.Drawing.Color.White;
        	this.textBox1.ForeColor = System.Drawing.Color.Blue;
        	this.textBox1.Location = new System.Drawing.Point(41, 246);
        	this.textBox1.Margin = new System.Windows.Forms.Padding(2);
        	this.textBox1.Name = "textBox1";
        	this.textBox1.ReadOnly = true;
        	this.textBox1.Size = new System.Drawing.Size(291, 20);
        	this.textBox1.TabIndex = 2;
        	this.textBox1.Text = "PENDING DIRECTORY SEARCH";
        	this.textBox1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
        	// 
        	// DirSearch
        	// 
        	this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
        	this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        	this.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange;
        	this.BackColor = System.Drawing.SystemColors.Control;
        	this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
        	this.ClientSize = new System.Drawing.Size(375, 406);
        	this.Controls.Add(this.textBox2);
        	this.Controls.Add(this.textBox1);
        	this.DoubleBuffered = true;
        	this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
        	this.Margin = new System.Windows.Forms.Padding(2);
        	this.MaximumSize = new System.Drawing.Size(375, 406);
        	this.MinimumSize = new System.Drawing.Size(375, 406);
        	this.Name = "DirSearch";
        	this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
        	this.Text = "Please select your Star Wars Galxies folder";
        	this.MouseClick += new System.Windows.Forms.MouseEventHandler(this.LAUNCHOPTIONS_MouseClick);
        	this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.LAUNCHOPTIONS_MouseDown);
        	this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.LAUNCHOPTIONS_MouseMove);
        	this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.LAUNCHOPTIONS_MouseUp);
        	this.ResumeLayout(false);
        	this.PerformLayout();
        }
        private System.Windows.Forms.TextBox textBox1;

        #endregion

        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.TextBox textBox2;
        
    }
}

