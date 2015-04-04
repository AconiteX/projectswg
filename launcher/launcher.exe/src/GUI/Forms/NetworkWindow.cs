/*
 * Created by SharpDevelop.
 * User: rdo
 * Date: 27.02.2013
 * Time: 19:50
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Drawing;
using System.Net.NetworkInformation;
using System.Windows.Forms;

namespace PswgLauncher
{

	
	
	/// <summary>
	/// Description of NetworkWindow.
	/// </summary>
	public partial class NetworkWindow : Form
	{
		
		
		public static int PingTries = 10;
		
		private GuiController Controller;
		
		public NetworkWindow(GuiController gc)
		{
			
			Controller = gc;

			InitializeComponent();
			this.Icon= Controller.GetAppIcon();

		}
		
		private void ButtonCloseClick(object sender, EventArgs e)
		{
			this.Visible = false;
		}
		
		
		void ButtonRunClick(object sender, EventArgs e)
		{
			RunNetworkDiag();
		}
		
		//FIXME: could be improved with classes...
		protected void RunNetworkDiag() {
			
			buttonRun.Enabled = false;
			
			richTextBox1.Clear();
			
			RunPingTest(richTextBox1 , GuiController.PatchServer, "Patch Server");
			RunPingTest(richTextBox1 , GuiController.LoginServer, "Login Server");
			
			richTextBox1.AppendText("More tests to follow soon.");
			
			
			buttonRun.Enabled = true;
			
		}
		
		
		
		
		public static void RunPingTest(RichTextBox rtb, String adx, String title) {
			
			rtb.AppendText("Ping " + title);
			
			PingOptions options = new PingOptions(128,true);
			Ping ping = new Ping();
			byte[] data = new byte[32];
			
			int received = 0;
			long responsetime = 0;
			String AvgRTT = "N/A";
			
			for (int i =0 ; i < PingTries; i++) {
				
				rtb.AppendText(".");
				rtb.Refresh();
				
				PingReply reply = null;
				
				try {
					 reply = ping.Send(adx, 1000, data, options);
				} catch {	
				}
				
				if ((reply != null) && (reply.Status == IPStatus.Success)) {
					received++;
					responsetime += reply.RoundtripTime;
				}
			}
			
			if (received > 0) {
				AvgRTT = (responsetime/received) + "ms";
			} 
			
			rtb.AppendText(String.Format(": {0} Packets sent, {1} Received, avg RTT is {2}.\n", PingTries, received, AvgRTT));
			
			
		}
		
		
		

	}
}
