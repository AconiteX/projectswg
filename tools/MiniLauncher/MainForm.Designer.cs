/*
 * Created by SharpDevelop.
 * User: rdo
 * Date: 12.05.2013
 * Time: 18:08
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
namespace MiniLauncher
{
	partial class MainForm
	{
		/// <summary>
		/// Designer variable used to keep track of non-visual components.
		/// </summary>
		private System.ComponentModel.IContainer components = null;
		
		/// <summary>
		/// Disposes resources used by the form.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing) {
				if (components != null) {
					components.Dispose();
				}
			}
			base.Dispose(disposing);
		}
		
		/// <summary>
		/// This method is required for Windows Forms designer support.
		/// Do not change the method contents inside the source code editor. The Forms designer might
		/// not be able to load this method if it was changed manually.
		/// </summary>
		private void InitializeComponent()
		{
			this.comboBox1 = new System.Windows.Forms.ComboBox();
			this.labelServer = new System.Windows.Forms.Label();
			this.textBox1 = new System.Windows.Forms.TextBox();
			this.labelUsername = new System.Windows.Forms.Label();
			this.button1 = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// comboBox1
			// 
			this.comboBox1.FormattingEnabled = true;
			this.comboBox1.Location = new System.Drawing.Point(13, 39);
			this.comboBox1.Name = "comboBox1";
			this.comboBox1.Size = new System.Drawing.Size(265, 21);
			this.comboBox1.TabIndex = 0;
			// 
			// labelServer
			// 
			this.labelServer.Location = new System.Drawing.Point(13, 13);
			this.labelServer.Name = "labelServer";
			this.labelServer.Size = new System.Drawing.Size(100, 23);
			this.labelServer.TabIndex = 1;
			this.labelServer.Text = "Server";
			// 
			// textBox1
			// 
			this.textBox1.Location = new System.Drawing.Point(13, 100);
			this.textBox1.Name = "textBox1";
			this.textBox1.Size = new System.Drawing.Size(265, 20);
			this.textBox1.TabIndex = 2;
			this.textBox1.TextChanged += new System.EventHandler(this.TextBox1TextChanged);
			// 
			// labelUsername
			// 
			this.labelUsername.Location = new System.Drawing.Point(13, 71);
			this.labelUsername.Name = "labelUsername";
			this.labelUsername.Size = new System.Drawing.Size(100, 23);
			this.labelUsername.TabIndex = 3;
			this.labelUsername.Text = "Username";
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(13, 150);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(75, 23);
			this.button1.TabIndex = 4;
			this.button1.Text = "Connect";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new System.EventHandler(this.Button1Click);
			// 
			// MainForm
			// 
			this.AcceptButton = this.button1;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(300, 186);
			this.Controls.Add(this.button1);
			this.Controls.Add(this.labelUsername);
			this.Controls.Add(this.textBox1);
			this.Controls.Add(this.labelServer);
			this.Controls.Add(this.comboBox1);
			this.MaximumSize = new System.Drawing.Size(310, 220);
			this.MinimumSize = new System.Drawing.Size(310, 220);
			this.Name = "MainForm";
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
			this.Text = "MiniLauncher";
			this.ResumeLayout(false);
			this.PerformLayout();
		}
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.Label labelUsername;
		private System.Windows.Forms.TextBox textBox1;
		private System.Windows.Forms.Label labelServer;
		private System.Windows.Forms.ComboBox comboBox1;
	}
}
