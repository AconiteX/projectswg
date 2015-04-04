/*
 * Created by SharpDevelop.
 * User: rdo
 * Date: 22.12.2012
 * Time: 16:14
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
namespace PswgLauncher
{
	partial class PatchCheckerDialog
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
			this.label1 = new System.Windows.Forms.Label();
			this.buttonRun = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(13, 13);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(320, 25);
			this.label1.TabIndex = 0;
			this.label1.Text = "Please wait while we\'re checking for Launcher Updates.";
			// 
			// buttonRun
			// 
			this.buttonRun.Enabled = false;
			this.buttonRun.Location = new System.Drawing.Point(13, 119);
			this.buttonRun.Name = "buttonRun";
			this.buttonRun.Size = new System.Drawing.Size(75, 23);
			this.buttonRun.TabIndex = 2;
			this.buttonRun.Text = "Continue";
			this.buttonRun.UseVisualStyleBackColor = true;

			// 
			// PatchCheckerDialog
			// 
			this.ClientSize = new System.Drawing.Size(345, 153);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.buttonRun);
			this.Name = "PatchCheckerDialog";
			this.ResumeLayout(false);
		}
		private System.Windows.Forms.Button buttonRun;
		private System.Windows.Forms.Label label1;
		

	}
}
