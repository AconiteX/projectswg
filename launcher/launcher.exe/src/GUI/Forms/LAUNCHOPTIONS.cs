using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Media;
using System.Net;
using System.Security.Principal;
using System.Text;
using System.Windows.Forms;

namespace PswgLauncher
{
    public partial class LAUNCHOPTIONS : Form
    {
    	
    	private GuiController Controller;
    	Point mouseDownPoint = Point.Empty;

    	private LauncherButton MinimizeButton;
    	private LauncherButton CloseButton;
    	
		private LauncherButton AboutButton;    	
    	private LauncherButton SupportButton;
    	private LauncherButton TrefixButton;
    	private LauncherButton NetworkButton;
    	private LauncherButton AdminButton;
    	private LauncherButton SwgDirButton;
    	
    	private bool init = false;
    	
        public LAUNCHOPTIONS(GuiController gc)
        {
        	
        	this.Controller = gc;
        	
        	this.AutoScaleMode = AutoScaleMode.None;        	
        	
            InitializeComponent();
            InitializeComponent2();
            
            
            soundControl.Checked = Controller.soundOption;
            checksumControl.Checked = Controller.checksumOption;
            checkBoxLocalhost.Checked = Controller.LocalhostOption;
            checkBoxResume.Checked = Controller.ResumeOption;
            init = true;
        }
        
        private void InitializeComponent2() {
        	

        	
        	this.Region = System.Drawing.Region.FromHrgn(GuiController.CreateRoundRectRgn( 0, 0, Width, Height, 24, 24));      	
        	this.Icon= Controller.GetAppIcon();
        	this.BackgroundImage = Controller.GetResourceImage("Background_Options");
        	
        	MinimizeButton = Controller.SpawnMinimizeButton(new Point(502, 20));
        	CloseButton = Controller.SpawnCloseButton(new Point(526, 8));        	
        	MinimizeButton.Click += MinimizeClick;
        	CloseButton.Click += CloseClick;
        	this.Controls.Add(MinimizeButton);
        	this.Controls.Add(CloseButton);
        	
        	
        	SupportButton = Controller.SpawnStandardButton("Support", new Point(125, 58));
        	SupportButton.Click += Support_Click;
        	this.Controls.Add(SupportButton);

        	TrefixButton = Controller.SpawnStandardButton("Run Trefix.exe", new Point(300, 58));
        	TrefixButton.Click += button2_Click;
        	TrefixButton.Disable = true;
        	this.Controls.Add(TrefixButton);

        	AboutButton = Controller.SpawnStandardButton("About", new Point(125, 115));
        	AboutButton.Click += About_Click;
        	this.Controls.Add(AboutButton);
        	
        	NetworkButton = Controller.SpawnStandardButton("Network Diag", new Point(300, 115));
        	NetworkButton.Click += Network_Click;
        	this.Controls.Add(NetworkButton);
        	
        	
        	AdminButton = Controller.SpawnStandardButton("Admin Settings", new Point(125, 172));
        	AdminButton.Click += Admin_Click;
        	AdminButton.Disable = true;
        	this.Controls.Add(AdminButton);

        	SwgDirButton = Controller.SpawnStandardButton("open PSWGDir", new Point(300, 172));
        	SwgDirButton.Click += SwgDir_Click;
        	this.Controls.Add(SwgDirButton);
        	
        	RefreshButtonState();
        	
        }
        
        
        public void RefreshButtonState() {

        	TrefixButton.Disable = !File.Exists(Controller.FileTrefix);
        	AdminButton.Disable = !File.Exists(Controller.FileAdmSettings);

        }
        

        private void LAUNCHOPTIONS_Load(object sender, EventArgs e)
        {

        }

        private void CloseClick(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void MinimizeClick(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }
        
        private void LAUNCHOPTIONS_MouseDown(object sender, MouseEventArgs e)
        {
            mouseDownPoint = new Point(e.X, e.Y);
        }

        private void LAUNCHOPTIONS_MouseUp(object sender, MouseEventArgs e)
        {
            mouseDownPoint = Point.Empty;
        }

        private void LAUNCHOPTIONS_MouseClick(object sender, MouseEventArgs e)
        {

        }

        private void LAUNCHOPTIONS_MouseMove(object sender, MouseEventArgs e)
        {
        	if (mouseDownPoint.IsEmpty) {
                return;
        	}
            Form f = sender as Form;
            f.Location = new Point(f.Location.X + (e.X - mouseDownPoint.X), f.Location.Y + (e.Y - mouseDownPoint.Y));
        }



        private void Support_Click(object sender, EventArgs e)
        {
        	Controller.PlaySound("Sound_Click");
            SupportWindow support = new SupportWindow(Controller);
            support.Show();
        }

        
        private void Network_Click(object sender, EventArgs e)
        {
            Controller.LaunchNetwork();
        }
        
        
        
        private void button2_Click(object sender, EventArgs e)
        {
            if (File.Exists(Controller.FileTrefix))
            {
            	Controller.PlaySound("Sound_Click");
            	try {
                	System.Diagnostics.Process.Start(Controller.FileTrefix);
            	} catch {
            		Controller.AddDebugMessage("Could not start " + Controller.FileTrefix + " with elevated privs.");
            	}
            }

            else
            {
            	
            	Controller.PlaySound("Sound_Error");

            }

        }
        
        private void About_Click(object sender, EventArgs e) {
        	
        	DialogResult dr = MessageBox.Show("Running " + ((new WindowsPrincipal(WindowsIdentity.GetCurrent()).IsInRole(WindowsBuiltInRole.Administrator)) ? "with" : "without") + " Elevated Privileges.\n\nBuild " + Controller.GetProgramVersion() + "\n\nButtons created with Star Jedi Font by Davide Canavero, available under\nhttp://projectswg.com/download/star_jedi.zip","Project SWG Launcher",MessageBoxButtons.OK);
        	
        }
        
        private void Admin_Click(object sender, EventArgs e) {
        	Controller.LaunchAdmSettings();
        }
        
        private void SwgDir_Click(object sender, EventArgs e) { 

			System.Diagnostics.Process.Start("explorer.exe", Controller.SwgSavePath);
        	
        }
        	
   

        void ChecksumControlCheckedChanged(object sender, EventArgs e)
        {
        	
        	Controller.checksumOption = checksumControl.Checked;
        	
        }
        

        
        void SoundControlCheckedChanged(object sender, EventArgs e)
        {
        	
        	Controller.soundOption = soundControl.Checked;
        	
        }
        
        void LinkDebugWindowLinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
        	Controller.LaunchDebug();
        }
        

        void CheckBoxLocalhostCheckedChanged(object sender, EventArgs e)
        {
        	
        	if (!init) {
        		return;
        	}
        	
        	if (checkBoxLocalhost.Checked == true) {
        		DialogResult dr = MessageBox.Show("The localhost option is meant for development only.\nIf you enable it, you won't be able to connect to the ProjectSWG game server.\n\nAre you certain you want to connect to locahost?","localhost option",MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
        		
        		if (dr == DialogResult.No) {
        			checkBoxLocalhost.Checked = false;
        			return;
        		}
        	}
        	
        	
        	
        	Controller.LocalhostOption = checkBoxLocalhost.Checked;
        }

        void CheckBoxResumeCheckedChanged(object sender, EventArgs e)
        {
        	Controller.ResumeOption = checkBoxResume.Checked;
        }
        
        
        void LinkMissingFilesClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
        	String missing = "Missing Files " + Environment.NewLine;
        	
        	
        	foreach (KeyValuePair<String,SWGFile> file in Controller.SWGFiles.SwgFileTable) {
        		
        		if (Controller.SWGFiles.IsGood(file.Key)) {
		           	continue;
		        }

        		missing += file.Key + Environment.NewLine;
        		
        	}
        	
        	
        	MessageBox.Show(missing, "Missing Files", MessageBoxButtons.OK);

        	
        	
        }
        
        
        

    }
}
