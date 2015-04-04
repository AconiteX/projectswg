/*
 * Created by SharpDevelop.
 * User: rdo
 * Date: 22.12.2012
 * Time: 16:14
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

using PswgLauncher;

namespace PswgLauncher
{
	/// <summary>
	/// Description of PatchCheckerDialog.
	/// </summary>
	public partial class PatchCheckerDialog : Form
	{
		
		private GuiController Controller;
		private Timer timer;

		bool update = false;
		
		public PatchCheckerDialog(GuiController gc)
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			
			this.Controller = gc;
			
			
			InitializeComponent();
			this.Icon= Controller.GetAppIcon();
			this.setTimer();
		
		}
		

       
		
		// need to use this, otherwise the dialog won't close 
		private void setTimer() {
			
			timer = new Timer();
			timer.Interval = 100;
        	timer.Tick += TimerCheckUpdate;
	        timer.Start();
			
		}
		
		private void TimerCheckUpdate(object sender, EventArgs e)
	    {
	        timer.Stop();
	        timer.Dispose();
	        
	        if (checkForUpdate() && update) {
	        		this.DialogResult = DialogResult.Yes;
	        		return;
	        }
	        
	        this.DialogResult = DialogResult.OK;
	        return;
	        
    	}
		
		
		private bool checkForUpdate() {
			
			//FIXME: this is not really pretty. maybe the whole method should be in the controller.
			// --darkk
			PatchChecker patch = new PatchChecker(Controller);
			update = patch.UpdateNeeded;

			if (patch.remoteError) {
				Controller.AddDebugMessage("Remote error while checking for updates.");
			} else if (patch.localError) {
				Controller.AddDebugMessage("Local error while checking for updates, lpatchusr.cfg write problems?");
			} else {
			
				if (update) {
					Controller.AddDebugMessage("Update available!");
					return true;

				} else {
					Controller.AddDebugMessage("Launcher is uptodate.");
					return true;
				}
			
			}
			
			return false;
		}
		

		


	}
}
