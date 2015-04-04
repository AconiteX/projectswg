/*
 * Created by SharpDevelop.
 * User: rdo
 * Date: 26.12.2012
 * Time: 21:36
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Drawing;
using System.Windows.Forms;

using PswgLauncher;

namespace PswgLauncher
{
	/// <summary>
	/// Description of DebugWindow.
	/// </summary>
	public partial class DebugWindow : Form
	{
		
		private GuiController Controller;
		
		public DebugWindow(GuiController gc)
		{
			
			Controller = gc;

			InitializeComponent();
			this.Icon= Controller.GetAppIcon();
			
		}
		
		private void ButtonCloseClick(object sender, EventArgs e)
		{
			this.Visible = false;
		}
		
		public void AddText(String message) {
			textBox1.AppendText(message + Environment.NewLine);
		}
		
		
		void DebugWindowLoad(object sender, EventArgs e)
		{
			
		}
		
		void DebugWindowFormClosing(object sender, FormClosingEventArgs e)
		{
			this.Visible = false;
			return;
		}
		
		void ButtonCopyClick(object sender, EventArgs e)
		{
			textBox1.SelectAll();
			textBox1.Copy();
			textBox1.SelectionLength = 0;
		}
		
		void ButtonSaveClick(object sender, EventArgs e)
		{

            SaveFileDialog dlg = new SaveFileDialog();
            dlg.Filter = "Text files (*.txt)|*.txt";
            DialogResult result = dlg.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.OK) {
            	try {
	            	using (System.IO.StreamWriter outfile = new System.IO.StreamWriter(dlg.FileName)) {
	            		outfile.Write(textBox1.Text);
	            	}
            	} catch {
            		MessageBox.Show("Error Writing file.", "Write Error", MessageBoxButtons.OK,MessageBoxIcon.Error);
            	}
            }
		}
		

	}
}
