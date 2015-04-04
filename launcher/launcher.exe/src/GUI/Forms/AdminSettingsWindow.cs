/*
 * Created by SharpDevelop.
 * User: rdo
 * Date: 23.06.2013
 * Time: 17:58
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Drawing;
using System.Windows.Forms;

using Microsoft.Win32;
using PswgLauncher;

namespace PswgLauncher.GUI.Forms
{
	/// <summary>
	/// Description of MainForm.
	/// </summary>
	public partial class AdminSettingsWindow : Form
	{
		
		protected String OptRunNormal = "Run from\n " + Application.StartupPath + "\nwith no additional privileges (Default Behaviour, Recommended on WinXP)";
		protected String OptRunElevate = "Run from\n " + Application.StartupPath + "\nand ask for Admin privileges (Recommended on Win Vista, 7, 8 and newer)";
		protected String OptRunHome = "Run from\n " + Application.StartupPath + "\nwith no additional privileges and store all files and settings in\n " + Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\PSWG" + ".\n Note: this will take up a few GB of duplicate data *per user*.";
		
		protected String LocateSettings = "Note, to change PSWG Launcher's behaviour, you can always start \n" + Application.ExecutablePath + "\n manually or use the Launcher Options button in PSWG Launcher.";
		
		protected String RegistrySubkey = "SOFTWARE\\ProjectSWG";
		protected String RegistryVName = "RunAsBehaviour";
		
		protected GuiController controller;
		
		public AdminSettingsWindow(GuiController Controller)
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			this.controller = Controller;
			InitializeComponent();
			InitializeComponent2();

		}
		
		protected void InitializeComponent2() {	
			
			radioRunNormal.Text = OptRunNormal;
			radioRunElevated.Text = OptRunElevate;
			LocateSettingsLabel.Text = LocateSettings;
			
			this.Icon= controller.GetAppIcon();
			
			RegistryKey TheKey = Registry.CurrentUser.OpenSubKey(RegistrySubkey, false);
			
			bool needsset = true;
			if (TheKey != null) {
				
				object TheSetting = TheKey.GetValue(RegistryVName);
				
				if (TheSetting != null) {
	
					switch ((int) TheSetting) {
						case 0:
							radioRunNormal.Checked = true;
							needsset = false;
							break;
						case 1:
							radioRunElevated.Checked = true;
							needsset = false;
							break;
					}
					
				}
			}
			
			if (needsset) {
				radioNoSetting.Checked = true;
			}
		}
		
		void ButtonCancelClick(object sender, EventArgs e)
		{
			this.DialogResult = DialogResult.Cancel;
		}
		
		void ButtonSaveClick(object sender, EventArgs e)
		{
			SaveSettings();
			this.DialogResult = DialogResult.OK;
		}
		
		void ButtonSaveAndRestartClick(object sender, EventArgs e)
		{
			SaveSettings();			
			this.DialogResult = DialogResult.Yes;
		}
		
		protected void SaveSettings() {
			int setting = -1;
			
			if (radioRunNormal.Checked) {
				setting = 0;
			} else if (radioRunElevated.Checked) {
				setting = 1;
			} 
			
			if (setting >= 0) {
				try {
					
					RegistryKey TheKey = Registry.CurrentUser.OpenSubKey(RegistrySubkey, true);
					
					if (TheKey == null) {
						TheKey = Registry.CurrentUser.CreateSubKey(RegistrySubkey);
					}
					
					TheKey.SetValue(RegistryVName, setting);
					
				} catch (Exception ex) {
					MessageBox.Show("Error saving setting to registry." + ex.ToString(),"Error",MessageBoxButtons.OK, MessageBoxIcon.Error);
				}
			} else {
				try {
					
					RegistryKey TheKey = Registry.CurrentUser.OpenSubKey(RegistrySubkey, true);
					
					if (TheKey != null) {
						TheKey.DeleteValue(RegistryVName);
					}
					
				} catch {}
			}
			
			controller.NextRunAsMode = setting;
			
		}
		
		void RadioNoSettingClick(object sender, EventArgs e)
		{
			if (controller.RunAsMode == 1) {
				buttonSaveAndRestart.Text = "Save and Exit";
			} else {
				buttonSaveAndRestart.Text = "Save and Restart";
			}
			buttonSave.Enabled = (controller.RunAsMode != -1);
			buttonSaveAndRestart.Enabled =  (controller.RunAsMode != -1);
			Refresh();
		}
		
		void RadioRunNormalClick(object sender, EventArgs e)
		{
			if (controller.RunAsMode == 1) {
				buttonSaveAndRestart.Text = "Save and Exit";
			} else {
				buttonSaveAndRestart.Text = "Save and Restart";
			}
			buttonSave.Enabled = (controller.RunAsMode != 0);
			buttonSaveAndRestart.Enabled = (controller.RunAsMode != 0);			
			Refresh();
		}
		
		void RadioRunElevatedClick(object sender, EventArgs e)
		{
			buttonSaveAndRestart.Text = "Save and Restart";
			buttonSave.Enabled = (controller.RunAsMode != 1);
			buttonSaveAndRestart.Enabled = (controller.RunAsMode != 1);
			Refresh();			
		}
	}
}
