using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Net;
using System.Media;



namespace PswgLauncher
{

    public partial class DirSearch : Form
    {
    	private GuiController Controller;
    	private String SwgDir;
    	private bool GotValidDir;

    	private LauncherButton MinimizeButton;
    	private LauncherButton CloseButton;
    	    	
    	private LauncherButton DirButton;
    	private LauncherButton NextButton;
    	
    	Point mouseDownPoint = Point.Empty;
    	
        public DirSearch(GuiController gc)
        {
        	
        	this.Controller = gc;
			this.AutoScaleMode = AutoScaleMode.None;
            InitializeComponent();
            InitializeComponent2();
        }
      
        
        private void InitializeComponent2() {
        	
        	
        	
        	this.Region = System.Drawing.Region.FromHrgn(GuiController.CreateRoundRectRgn( 0, 0, Width, Height, 24, 24));      	
        	this.Icon= Controller.GetAppIcon();
        	this.BackgroundImage = Controller.GetResourceImage("Background_DirSearch");

        	MinimizeButton = Controller.SpawnMinimizeButton(new Point(this.Width - 60, 20));
        	CloseButton = Controller.SpawnCloseButton(new Point(this.Width - 40, 8));        	
        	MinimizeButton.Click += MinimizeClick;
        	CloseButton.Click += CloseClick;
        	this.Controls.Add(MinimizeButton);
        	this.Controls.Add(CloseButton);
        	
        	DirButton = Controller.SpawnStandardButton("Browse", new Point(128, 305	));
        	NextButton = Controller.SpawnStandardButton("Next", new Point(251, 355 ));
        	NextButton.Enabled = false;

        	DirButton.Click += BrowseButton_Click;
        	NextButton.Click += NextButton1_Click;
        	
        	this.Controls.Add(DirButton);
        	this.Controls.Add(NextButton);
        	
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
        

        private void BrowseButton_Click(object sender, EventArgs e)  //to browse for SWG install dir
        {
        	Controller.PlaySound("Sound_Click");
            
            folderBrowserDialog1.ShowDialog();  //opens the file browse window
            SwgDir = folderBrowserDialog1.SelectedPath; // gathers data and writes it to swggetdir
            textBox2.Text = SwgDir;  // displays data to textbox1
            
            //FIXME this is a hack, clearly...
            string[] validfilenames = new String[] {
            	
            	"BugTool.exe",
				"SwgClient_r.exe",
				"SwgClientSetup_r.exe",
				"TreFix.exe",
				"*.tre",
				"*.toc",
				"dbghelp*.dll",
				"LP_Diagnostics.exe",
				"lp_manifest.cache",
				"favicon.ico",
				"Canada_SWG_Manual_French.pdf",
				"SWG_JP_*.pdf",
				"SWGExpPack_MG_*.pdf",
				"SWG-CoA Troubleshooting Guide v1b.rtf",
				"SWG troubleshooting guide.rtf",
				"SOE TOU*.doc",
				"Activision SLA*.doc",
				"characterlist_*.txt",
				"preload.cfg",
				"live.cfg",
				"client.cfg",
				"login.cfg",
				"SWGVoiceService.exe",
				"*vivox*",
				"lp_dldat.ctl",
				"local_machine_options.default",
				"local_machine_options.low",
				"options.default",
				"options.low",
				"SwgClient_r.exe-stage.*"
            };
            
            
            bool isvaliddirectory = false;
            
            try {
                    	
            	
                    	
                foreach (String f in validfilenames) {
                	string[] files = Directory.GetFiles(SwgDir, f, SearchOption.AllDirectories);
                    		
                    if (files.Length > 0) {
                    			
                    	isvaliddirectory = true;
                    	break;
                    }
                }
                           
            } catch {
				isvaliddirectory = false;            	
            } 
            
            
            textBox1.ForeColor = ((isvaliddirectory) ? Color.Green : Color.Red );
            textBox1.Text = ((isvaliddirectory) ? "SWG INSTALLATION FOUND" : "INVALID STAR WARS GALAXIES DIRECTORY" );
            GotValidDir = isvaliddirectory;
            NextButton.Enabled = isvaliddirectory;
          
        }


        private void NextButton1_Click(object sender, EventArgs e)  //used to get to launcher if directory is valid
        {
            if (GotValidDir) {
            	
            	Controller.PlaySound("Sound_Click");
                Controller.SwgDir = SwgDir;
                this.DialogResult = DialogResult.OK;

            }
            else  //this prevents non valid directories from working
            {
                
            	DialogResult dr = MessageBox.Show("You haven't selected a valid SWG installation location.","No valid SWG Dir selected",MessageBoxButtons.OK);
                
            }

        }

        private void CloseClick(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void MinimizeClick(object sender, EventArgs e)
        {
        	this.WindowState = FormWindowState.Minimized;
             
        }
       
    }
}
