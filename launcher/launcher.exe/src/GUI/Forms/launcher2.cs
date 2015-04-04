using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Media;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;

namespace PswgLauncher
{

    public partial class launcher2 : Form
    {

    	int errorcounter = 0;
        
    	private bool InstallerAvailable=false;
  
        public StatusProcessor StatusProcessor;
        private LAUNCHOPTIONS OptionWindow;
        private GuiController Controller;

        private LauncherProgressBar launcherProgressBar1;

        private LauncherButton MinimizeButton;
        private LauncherButton CloseButton;        
        private LauncherButton AcctButton;
        private LauncherButton OptButton;
        private LauncherButton ScanButton;
        private LauncherButton LOptButton;
        private LauncherButton DonateButton;
        private LauncherButton PlayButton;
        
        private LauncherLabel label1;
        private LauncherLabel labelError;
        
        private System.Windows.Forms.Timer timer;
        
        private System.ComponentModel.BackgroundWorker backgroundWorker2;
        private System.ComponentModel.BackgroundWorker backgroundWorkerUpdate;
        private System.ComponentModel.BackgroundWorker backgroundWorkerScan;
        private System.ComponentModel.BackgroundWorker backgroundWorkerScanManual;
        
        public launcher2(GuiController gc)
        {
        	
        	this.Controller = gc;
        	this.StatusProcessor = new StatusProcessor(gc);

        	this.AutoScaleMode = AutoScaleMode.None;
        	
        	InitializeComponent();
        	InitializeComponent2();
        	
        	this.Show();
        	
            Point mouseDownPoint = Point.Empty;
            this.Process();
            
        }
        
        public void InitializeComponent2() {
        	
        	this.webBrowser1.Url = new System.Uri(GuiController.UPDATENOTES, System.UriKind.Absolute);
        	
        	this.backgroundWorkerScan = new BackgroundWorker();
       		this.backgroundWorkerScan.WorkerReportsProgress = true;
        	this.backgroundWorkerScan.WorkerSupportsCancellation = true;
        	this.backgroundWorkerScan.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorkerScan_DoWork);
        	//this.backgroundWorkerScan.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.backgroundWorkerScan_ProgressChanged);
        	//FIXME
        	this.backgroundWorkerScan.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.backgroundWorker2_ProgressChanged);
        	this.backgroundWorkerScan.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorkerScan_RunWorkerCompleted);        	

        	this.backgroundWorkerScanManual = new BackgroundWorker();
       		this.backgroundWorkerScanManual.WorkerReportsProgress = true;
        	this.backgroundWorkerScanManual.WorkerSupportsCancellation = true;
        	this.backgroundWorkerScanManual.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorkerScanManual_DoWork);
        	//this.backgroundWorkerScanManual.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.backgroundWorkerScanManual_ProgressChanged);
        	//FIXME
        	this.backgroundWorkerScanManual.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.backgroundWorker2_ProgressChanged);
        	this.backgroundWorkerScanManual.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorkerScanManual_RunWorkerCompleted);

       		this.backgroundWorkerUpdate = new BackgroundWorker();
       		this.backgroundWorkerUpdate.WorkerReportsProgress = true;
        	this.backgroundWorkerUpdate.WorkerSupportsCancellation = true;
        	this.backgroundWorkerUpdate.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorkerUpdate_DoWork);
        	//this.backgroundWorkerUpdate.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.backgroundWorkerUpdate_ProgressChanged);
        	//FIXME
        	this.backgroundWorkerUpdate.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.backgroundWorker2_ProgressChanged);
        	this.backgroundWorkerUpdate.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorkerUpdate_RunWorkerCompleted);
        	
        	// modify existing components from designer

        	this.Region = System.Drawing.Region.FromHrgn(GuiController.CreateRoundRectRgn( 0, 0, Width, Height, 24, 24));      	
        	this.Icon= Controller.GetAppIcon();
        	this.BackgroundImage = Controller.GetResourceImage("Background_Launcher");

        	MinimizeButton = Controller.SpawnMinimizeButton(new Point(535, 20));
        	CloseButton = Controller.SpawnCloseButton(new Point(562, 8));
        	
        	MinimizeButton.Click += MinimizeClick;
        	CloseButton.Click += CloseClick;
        	
        	this.Controls.Add(MinimizeButton);
        	this.Controls.Add(CloseButton);
        	
        	//add new components
        	
        	launcherProgressBar1 = Controller.SpawnProgressBar(new System.Drawing.Point(23, 452), new System.Drawing.Size(441, 17));
        	this.Controls.Add(launcherProgressBar1);
        	
        	AcctButton = Controller.SpawnStandardButton("My Account", new Point(10, 365	));
        	OptButton = Controller.SpawnStandardButton("Game options", new Point(126, 365 ));
        	ScanButton = Controller.SpawnStandardButton("Scan", new Point(242, 365 ));
        	LOptButton = Controller.SpawnStandardButton("Launcher options", new Point(358, 365));
        	DonateButton = Controller.SpawnStandardButton("Donate", new Point(474, 365));
        	
        	PlayButton = Controller.SpawnPlayButton("Play", new Point(484,434));

        	AcctButton.Click += acct_Click_1;
        	OptButton.Click += options_Click_1;
        	ScanButton.Click += Scan_Click;
        	LOptButton.Click += button2_Click;
        	DonateButton.Click += Donate_Click;
        	PlayButton.Click += PLAY_Click_1;
        	
        	this.Controls.Add(AcctButton);
        	this.Controls.Add(OptButton);
        	this.Controls.Add(ScanButton);
        	this.Controls.Add(LOptButton);
        	this.Controls.Add(DonateButton);
        	this.Controls.Add(PlayButton);
        	
        	label1 = Controller.SpawnLabel("", new Point(23, 415), new Size(260, 15));
        	this.Controls.Add(label1);
        	
        	labelError = Controller.SpawnLabel("", new Point(23, 430), new Size(260, 15));
        	this.Controls.Add(labelError);
        	
        	
        	
        }
        
        private void SetStatus(int Status) {
        	
        	if (Status == null || Status < 0) {
        		Status = StatusProcessor.GetNextStatus();
        	}
        	
        	 Controller.AddDebugMessage("Switching Status to: " + Status);
        	 
        	 if (!StatusProcessor.SetNewState(Status)) {
        	 	return;
        	 }
        	 
        	 Controller.AddDebugMessage("Switched Status to: " + Status);

        	
        	if (Status >= 0) {
        		RefreshStatus(true);
        	}
        	
        }
        

        private void Process() {
        	       	
        	//this is also a bit dodgy... 
        	if (StatusProcessor.Status == (int) StatusProcessor.StatusCodes.UpdatingOK && InstallerAvailable) {
			    String installer = Controller.SwgSavePath + @"\" + GuiController.PATCHER;
				if (File.Exists(installer)) {
				    try {
				    	System.Diagnostics.Process.Start(installer);
						Application.Exit();
						return;
				    } catch (Exception ex) {
				    	//just continue running
				    }
			    }
        	}
        	
        	this.SetStatus(-1);
        	
        	//FIXME: well this is a bit dodgy. Maybe get rid of NoChecksum altogether.
        	//if (StatusProcessor.Status == (int) StatusProcessor.StatusCodes.UpdatingOK) {
        	//	this.SetStatus(-1);
        	//}
        	
        	if (StatusProcessor.Status == (int) StatusProcessor.StatusCodes.Updating) {
            	Controller.AddDebugMessage("Dispatching Worker for Updating.");
            	this.DispatchUpdateWorker();        		
        	}
        	
        	
            if (StatusProcessor.Status == (int) StatusProcessor.StatusCodes.UpdatingChecksum) {        		
        		
        		this.launcherProgressBar1.Value = 0;
        		
            	WebClient wc = new WebClient();
            	wc.Encoding = System.Text.Encoding.UTF8;
            	wc.Credentials = Controller.GetNetworkCredential();

            	Controller.AddDebugMessage("Processing Checksums.");
            	
            	bool rv = false;
				int c = 0;
            	
				while (rv == false) {
					
            		if (c>0) {
						this.SetStatus((int) StatusProcessor.StatusCodes.ChecksumFailed);
            			errorcounter++;
            			UpdateErrors();

		            	DialogResult res = MessageBox.Show("File checksums could not be read from server. Unfortunately we don't have a local copy that we could use instead. You can retry with OK button now or try again later.", "Retry downloading?",MessageBoxButtons.OKCancel);
        		
			        	if (res == DialogResult.Cancel) {
			        		return;
			        	}
            			
            		}
					
					this.SetStatus((int) StatusProcessor.StatusCodes.UpdatingChecksum);
					
					c++;
					rv = this.ProcessChecksums(wc);
				}
            	
				this.launcherProgressBar1.Value = 100;
            	this.SetStatus((int) StatusProcessor.StatusCodes.ChecksumOK);

            }
        	
        	if (StatusProcessor.Status == (int) StatusProcessor.StatusCodes.ChecksumOK) {
        		
        		if (!Controller.RunDirSearch()) {
            		Application.Exit();
            		return;
            	}
        		
        		this.SetStatus(-1);
        	}
            
            if (StatusProcessor.Status == (int) StatusProcessor.StatusCodes.Scanning) {
            	
            	Controller.AddDebugMessage("Dispatching Worker for Scanning.");
            	this.DispatchScanWorker();
            }
            
            if (StatusProcessor.Status == (int) StatusProcessor.StatusCodes.ScanningManual) {
            	Controller.AddDebugMessage("Dispatching Worker for Manual Scanning.");
            	this.DispatchScanManualWorker();
            }
            
            if (StatusProcessor.Status == (int) StatusProcessor.StatusCodes.Patching) {
            	
            	Controller.AddDebugMessage("Dispatching Worker for Patching.");
            	this.DispatchPatchWorker();
            }
        }
        
        private void UpdateErrors() {
        	if (errorcounter > 0) {
        		labelError.Text = "Download Errors: " + errorcounter;
        	} else {
        		labelError.Text = "";
        	}
        }

        private void DispatchWorker(BackgroundWorker bgworker, int targetstatus) {
        	
         	UpdateErrors();
         	
         	if (StatusProcessor.Status != targetstatus) {
        		return;
        	}
        	
         	if (targetstatus == (int)StatusProcessor.StatusCodes.Updating) {
         		launcherProgressBar1.Step = 1;
         		launcherProgressBar1.Value = 0;
         	} else {
        	    launcherProgressBar1.Step = 100 / Controller.SWGFiles.SwgFileTable.Count;
        	    launcherProgressBar1.Value = 0;
         	}
        	
        	bgworker.RunWorkerAsync();

        }

        private void DispatchUpdateWorker() {        	
        	DispatchWorker(backgroundWorkerUpdate, (int)StatusProcessor.StatusCodes.Updating);
        }        
        
        private void DispatchPatchWorker() {        	
        	DispatchWorker(backgroundWorker2, (int)StatusProcessor.StatusCodes.Patching);
        }

        private void DispatchScanWorker() {
        	DispatchWorker(backgroundWorkerScan, (int)StatusProcessor.StatusCodes.Scanning);
        }
        
        private void DispatchScanManualWorker() {
        	DispatchWorker(backgroundWorkerScanManual, (int)StatusProcessor.StatusCodes.ScanningManual);
        }
        

        private bool RefreshStatus(bool updating) {
	
       		if (updating) { 
        		launcherProgressBar1.Text = "";
        	}
        	
        	OptButton.Disable = !File.Exists(Controller.SwgSavePath + @"\swgclientsetup_r.exe");

        	if (OptionWindow != null) {
        		OptionWindow.RefreshButtonState();
        	}
        	
        	PlayButton.Disable = StatusProcessor.GetPlayDisabled();
        	ScanButton.Disable = StatusProcessor.GetScanDisabled();
        	pictureBox2.Image = ((StatusProcessor.IsBusy()) ? Controller.GetResourceImage("small-loading") : null);
        	PlayButton.Text = StatusProcessor.GetPlayText();
        	label1.Text = StatusProcessor.GetLabelText();
        	launcherProgressBar1.ForeColor = label1.ForeColor = StatusProcessor.GetStatusColor();
        	
        	this.Refresh();
        	
        	return true;
        	
        }

        private void backgroundWorkerChecksum_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e) {

        	// only run in scanning State
        	if (StatusProcessor.Status != (int) StatusProcessor.StatusCodes.UpdatingChecksum) {
		    	e.Cancel = true;
		        return;
		    }

        	BackgroundWorker backgroundWorker = sender as BackgroundWorker;
  			if(backgroundWorker != null) {
	    		backgroundWorker.ReportProgress(0);
	    	}

	    	if(backgroundWorker.CancellationPending) {
			     e.Cancel = true;
        	}
			else {
            		backgroundWorker.ReportProgress(100,"Scanning finished.");
			}
        }

        private void backgroundWorkerUpdate_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e) {
        	if (StatusProcessor.Status != (int) StatusProcessor.StatusCodes.Updating) {
		    	e.Cancel = true;
		        return;        		
        	}
        	
        	BackgroundWorker backgroundWorker = sender as BackgroundWorker;
        	
        	if (backgroundWorker != null) {
		        			
				String checksum = "";
				
				String DL = GuiController.PATCHURL + GuiController.PATCHER;
				String ChksumDL = DL + ".md5";
				String Local = Controller.SwgSavePath + @"\" + GuiController.PATCHER;
				String LocalTmp =  Local + ".part";
	
				String lpatchsrv = "";
				
				WebClient wc = new WebClient();
				
				using (StreamReader upstreamVersionStreamReader = new StreamReader(wc.OpenRead(GuiController.PATCHURL + "lpatchinst.cfg"))) {
					
					int i = 0;
					while (lpatchsrv == "" && i < 20 && !backgroundWorker.CancellationPending) {
						backgroundWorker.ReportProgress( 1, "Debug Try determine remote patcher version");
		            	backgroundWorker.ReportProgress( 1, " Try determine remote patcher version");
		            	lpatchsrv = upstreamVersionStreamReader.ReadToEnd();
						i++;
					}
				}
	
				lpatchsrv = lpatchsrv.Trim();

				if (lpatchsrv == "") {
					e.Cancel = true;
					return;
				}
	
				ProgramVersion ThisVersion = new ProgramVersion(Controller.GetProgramVersion());
				ProgramVersion ServerVersion = new ProgramVersion(lpatchsrv);
				
				backgroundWorker.ReportProgress(2,"Debug Local Launcher Version" + Controller.GetProgramVersion() + "|" + ThisVersion.ToCompactString() + " |" + ThisVersion.ToString());
				backgroundWorker.ReportProgress(2,"Debug Server Launcher Version" + lpatchsrv + "|" + ServerVersion.ToCompactString() + " |" + ServerVersion.ToString());
				
		        if (ThisVersion.IsNewerThan(ServerVersion)) {
					backgroundWorker.ReportProgress(99,"Debug Launcher is uptodate, updating finished.");
	            	backgroundWorker.ReportProgress(100,"Updating finished.");
	            	return;
	            }
				
				using (StreamReader upstreamVersionStreamReader = new StreamReader(wc.OpenRead(ChksumDL))) {
					int i = 0;
					while (checksum == "" && i < 20 && !backgroundWorker.CancellationPending) {
						backgroundWorker.ReportProgress( 3, "Debug Try DL updater checksums");
		            	backgroundWorker.ReportProgress( 3, "Try DL updater Checksums");
						checksum = Regex.Replace(upstreamVersionStreamReader.ReadToEnd(),"\n","");
						i++;
					}
				}
				
				if (checksum == "") {
					backgroundWorker.ReportProgress( 3, "Debug problem DL'ing updater checksums!");
					e.Cancel = true;
					return;
				}
				backgroundWorker.ReportProgress( 4, "Debug downloaded updater checksums!");
				backgroundWorker.ReportProgress( 4, "Debug Looking for...." + Local);
				
				if (File.Exists(Local)) {
					if (SWGFile.MatchChecksum(Local,checksum)){
						backgroundWorker.ReportProgress(99,"Debug Found local file, matching.");
						backgroundWorker.ReportProgress(100,"installer available");
						return;
					}
					backgroundWorker.ReportProgress(5,"Debug Found local file, but it did not match.");
				} else {
					backgroundWorker.ReportProgress( 5, "Debug No luck, updater needs downloading");
				}
				
				try {
					
					
					backgroundWorker.ReportProgress(5,"downloading installer");
					backgroundWorker.ReportProgress(5,"Debug downloading installer");
					
					wc.OpenRead(DL);
					Int64 targetsize = Convert.ToInt64(wc.ResponseHeaders["Content-Length"]);
					long Filesize = 0;
					long offset = 0;
					
					while (Filesize < targetsize && !backgroundWorker.CancellationPending) {
						
						int progress = ((int) (100/targetsize * offset) );
						
			        	HTTPDownload(LocalTmp, DL, offset, backgroundWorker, progress, true);			
						
						if (File.Exists(LocalTmp)) {
							FileInfo f = new FileInfo(LocalTmp);
							Filesize = f.Length;
							offset = Filesize;
						}
					}
					
		        	if (SWGFile.MatchChecksum(LocalTmp,checksum) ) {
		        		backgroundWorker.ReportProgress(99, "Debug " + "Downloaded " + LocalTmp);
		        		
		        		if (File.Exists(Local)) {
		        			File.Delete(Local);
		        		}
		        		
		        		File.Move(LocalTmp, Local);
		        		
		        		backgroundWorker.ReportProgress(100, "installer available");
		        		return;
		        		
					} else {
						e.Cancel = true;
						backgroundWorker.ReportProgress(99, "Debug " + "Postpatch checksum mismatch " + LocalTmp);
					}
	        			
		        } catch (Exception ex) {
					e.Cancel = true;
		        	backgroundWorker.ReportProgress(99, "Debug " + "Error patching " + LocalTmp + " " + ex);
	        	}				
        	}

	    	if(backgroundWorker.CancellationPending) {
			     e.Cancel = true;
        	}
			else {
            		backgroundWorker.ReportProgress(100,"Updating finished.");
			}
        }
        
        private void ScanWork(bool checksums, int checkstatus, object sender, System.ComponentModel.DoWorkEventArgs e) {

        	// only run in scanning State
		    if (StatusProcessor.Status != checkstatus) {
		    	e.Cancel = true;
		        return;
		    }

        	BackgroundWorker backgroundWorker = sender as BackgroundWorker;
  			if(backgroundWorker != null) {
	    		backgroundWorker.ReportProgress(0);
	    		Controller.SWGFiles.Scan(checksums, backgroundWorker);
	    	}

	    	if(backgroundWorker.CancellationPending) {
			     e.Cancel = true;
        	}
			else {
            		backgroundWorker.ReportProgress(100,"Scanning finished.");
			}

        }

        private void backgroundWorkerScan_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e) {
        	ScanWork(Controller.checksumOption,(int)StatusProcessor.StatusCodes.Scanning, sender, e);
        }

        private void backgroundWorkerScanManual_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e) {
	    	ScanWork(true,(int)StatusProcessor.StatusCodes.ScanningManual, sender, e);
        }
        
        private void ScanComplete(bool checksum, bool DoContinue, object sender, System.ComponentModel.RunWorkerCompletedEventArgs e) {

        	if (e.Cancelled || e.Error != null) {
        		
        		Debug.WriteLine("Scan cancel/fail: " + (e.Cancelled ? "cancelled" : "") + (e.Error != null ? e.ToString() : ""));
        		this.launcherProgressBar1.Text = "";
        		
        		SetStatus((int) StatusProcessor.StatusCodes.ScanningFailed);
        		
        		return;
        	} 

        	SetStatus((int) StatusProcessor.StatusCodes.ScanningOK);

        	if (checksum) {
        		Controller.SaveScanComplete();
        	}
        	
        	if (DoContinue) {
        		Process();
        	}
        	
        }
        
        private void backgroundWorkerUpdate_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e) {

        	if (e.Cancelled || e.Error != null) {
        		Debug.WriteLine("update cancel/fail: " + (e.Cancelled ? "cancelled" : "") + (e.Error != null ? e.ToString() : ""));
        		this.launcherProgressBar1.Text = "";
        		
        		SetStatus((int) StatusProcessor.StatusCodes.UpdatingFailed);
        		
        	} else {
        		SetStatus((int) StatusProcessor.StatusCodes.UpdatingOK);
        	}
	
			Process();
        }
        
        //FIXME: consolidate with manual
        private void backgroundWorkerScan_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e) {
        	ScanComplete(Controller.checksumOption, true, sender, e);
        }
        
        //FIXME
        private void backgroundWorkerScanManual_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e) {
        	ScanComplete(true, true, sender, e);
        }        
        
        private void backgroundWorker2_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
        	
        	// only run if checksums are present
		    if (StatusProcessor.Status != (int)StatusProcessor.StatusCodes.Patching) {
		    	e.Cancel = true;
		        return;
		    }

        	BackgroundWorker backgroundWorker = sender as BackgroundWorker;
  			if(backgroundWorker != null)
  			{
	    		backgroundWorker.ReportProgress(0);
	   
		        Dictionary<String,SWGFile> files = Controller.SWGFiles.SwgFileTable;
		            
		        float step = (float) 100 / (float) files.Count;
		        int i = 0;
		            
		        foreach (KeyValuePair<String,SWGFile> file in files) {
		        	i++;
		            	
		        	// don't download good files again.
		            if (Controller.SWGFiles.IsGood(file.Key)) {
		            	continue;
		            }
		            	
		        	int progress = Convert.ToInt32(i * step);
		        	//backgroundWorker.ReportProgress( progress, "Debug " + files.Count + " " +progress + " " + i + " "+ step);
		        	bool dlstatus = false;
		        	String theKey = file.Key;
		        	backgroundWorker.ReportProgress( progress, "Debug " + file.Key);
		            backgroundWorker.ReportProgress( progress, file.Key);
					
					while (dlstatus == false && !backgroundWorker.CancellationPending) {
		            	dlstatus = DownloadFile(file.Key, file.Value, backgroundWorker, progress);
		            	//add some breathing room upon failures so launcher stays reactive
		            	if (!dlstatus) { System.Threading.Thread.Sleep(50); }
					}
		            
		            if(backgroundWorker.CancellationPending) {
			      		e.Cancel = true;
			      		break;
		            }
	    		}
            
	    		if(backgroundWorker.CancellationPending) {
			      e.Cancel = true;
			    }
			    else {
            		backgroundWorker.ReportProgress(100,"Patching finished.");
	    		}
  			}
        
        }
        
        private void backgroundWorker2_ProgressChanged(object sender, System.ComponentModel.ProgressChangedEventArgs e)
        {
        	
        	if (e.UserState != null) {
	        	
        		String s = e.UserState as String;
	        	//FIXME: this is a hack. got a better idea?
	        	String[] msg = s.Split(null,2);
	        	
	        	if (msg[0] == "Debug") {
	        		Controller.AddDebugMessage(msg[1]);
	        		return;
	        	}
	        	
	        	this.launcherProgressBar1.Text = s;
	        	this.launcherProgressBar1.Refresh();

	        	if (msg[0] == "Error") {
	        		errorcounter++;
	        		UpdateErrors();
	        	}
	        	
	        	if (msg[0] == "Patched" || msg[0] == "Read" || msg[0] == "OK") {
	        		
	        		if (Controller.SWGFiles.SwgFileTable.ContainsKey(msg[1])) {

	        			//Controller.SWGFiles.AddGoodFile(msg[1]);
	        			//Controller.SWGFiles.SwgFileTable.Remove(msg[1]);
	        		}
	        	}
        	
	        	if (msg[0] == "installer") {
	        		Controller.AddDebugMessage(msg[1]);
	        		if (msg[1] == "available") {
	        			Controller.AddDebugMessage("xyz");
	        			InstallerAvailable = true;
	        		}
	        	}
        	}
        	
        	launcherProgressBar1.Value = ((e.ProgressPercentage > 100) ? 100 : e.ProgressPercentage );
        	RefreshStatus(false);

        }

        private void backgroundWorker2_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
        	
	       	if (e.Cancelled || e.Error != null) {
        		
        		Debug.WriteLine("DL cancel/fail: " + (e.Cancelled ? "cancelled" : "") + (e.Error != null ? e.ToString() : ""));
        		this.launcherProgressBar1.Text = "";
        		SetStatus((int) StatusProcessor.StatusCodes.PatchingFailed);
        		
        	} else {

        		if (Controller.SWGFiles.IsComplete()) {
        			SetStatus((int) StatusProcessor.StatusCodes.PatchingOK);
        		} else {
        			SetStatus((int) StatusProcessor.StatusCodes.PatchingFailed);
        		}
        	}
        }
        

        
        private bool DownloadFile(String file, SWGFile swgfile, BackgroundWorker backgroundWorker, int progress) {
        	
        	String path = Controller.SwgSavePath;
        	String Fullsavepath = path + @"\" + file;
        	//String localsrc = Controller.SwgDir + @"\" + file;
        	String localsrc = Controller.SwgDir;
        	String Fulllocalsrc = localsrc + @"\" + file;
        	String remotesrc = GuiController.MAINURL + "/" + file;
        	long offset = 0;
        	
        	String checksum = swgfile.Checksum;
        	
        	if (!swgfile.MakeDirIfRequired()) {
        		backgroundWorker.ReportProgress( progress, "Debug " + "Couldnt create dir for file " + file);
        		return false;
        	}
        	swgfile.TouchFileIfRequired();

	        backgroundWorker.ReportProgress(progress, "Checking " + file);

        	try {
	        	
	        	if (swgfile.UpdateSavepath(2, false, false)) {
		        	backgroundWorker.ReportProgress( progress, "Debug " + "was already good: " + file);
		        	backgroundWorker.ReportProgress(progress, "OK " + file);
		        	return true;
	        	}
	        	// only get tre files from local storage
	        	if (Regex.IsMatch(file, @"\.tre$", RegexOptions.IgnoreCase)) {
	        		
	        		backgroundWorker.ReportProgress(progress, "Checking SWGDir for " + file);
	        		
	        		if (swgfile.UpdateSwgpath(2)) {
	        		
		        		backgroundWorker.ReportProgress(progress, "Debug " + "Reading " + file);
		        		backgroundWorker.ReportProgress(progress, "Reading " + file);
		        			
						//File.Copy(localsrc, swgdirsave);
						File.Copy(@Fulllocalsrc, @Fullsavepath, true);
		        			
						if (swgfile.UpdateSavepath(2, true, true)) {
		        			backgroundWorker.ReportProgress(progress, "Debug " + "Read " + file);
		        			backgroundWorker.ReportProgress(progress, "Read " + file);
		        			return true;
		        		}
		        			
						backgroundWorker.ReportProgress(progress, "Debug " + "Postcopy checksum mismatch (rare!) " + file);
		        		return false;
		        	} else {
		        		backgroundWorker.ReportProgress(progress, "Debug " + " SWGdir did not qualify for " + file);
	        		}
	        	}
	        	backgroundWorker.ReportProgress(progress, "Debug " + "Patching " + file);
				backgroundWorker.ReportProgress(progress, "Patching " + file);
				
				long Filesize = 0;
				
				FileInfo f = swgfile.GetFileInfo();
				
		        if (f != null && f.Length < swgfile.Filesize) {
			    	if (Controller.ResumeOption) {
			        	offset = f.Length;
			        }
			    }
				
				while (Filesize < swgfile.Filesize && !backgroundWorker.CancellationPending) {
					HTTPDownload(swgfile, remotesrc, offset, backgroundWorker, progress, false);
					System.Threading.Thread.Sleep(50);
					swgfile.UpdateSavepath(1,false,false);

					FileInfo fi = swgfile.GetFileInfo();

					if (fi != null) {
						Filesize = fi.Length;
						offset = Filesize;
					}
					
				}
	        	swgfile.Reset();
	        	
	        	if (swgfile.UpdateSavepath(2,true,true) ) {
	        		backgroundWorker.ReportProgress(progress, "Debug " + "Patched " + file);
	        		backgroundWorker.ReportProgress(progress, "Patched " + file);
	        		return true;
				} else {
					backgroundWorker.ReportProgress(progress, "Debug " + "Postpatch checksum mismatch " + file);
				}
        			
	        } catch (Exception ex) {
        		
	        	backgroundWorker.ReportProgress(progress, "Debug " + "Error patching " + file + " " + ex);
        	}
        	
        	backgroundWorker.ReportProgress(progress, "Debug " + "Error patching " + file);
        	backgroundWorker.ReportProgress(progress, "Error patching " + file);
        	
        	return false;
        }
        
        
        private void HTTPDownload(SWGFile swgfile, String remoteURL, long offset, BackgroundWorker backgroundWorker, int progress, bool showfileprogress) {
        	HTTPDownload(swgfile.Filename, remoteURL, offset, backgroundWorker, progress, showfileprogress);
        }
        	
        private void HTTPDownload(String filename, String remoteURL, long offset, BackgroundWorker backgroundWorker, int progress, bool showfileprogress) {
        	
        	if (backgroundWorker.CancellationPending) { return; }
        	
        	bool append = false;
        	FileMode mode;
        	
        	
        	String filenameshort = filename.Split(Path.DirectorySeparatorChar).Last().ToLower();
        	
			if (Controller.ResumeOption && offset > 0) {
        		
				backgroundWorker.ReportProgress(progress, "Debug " + "Resume from " + offset);
				append = true;
				mode = FileMode.Append;
				
			} else {
				mode = FileMode.Create;
			}
        	
        	using ( FileStream OutputStream = new FileStream(filename, mode) ) {
        		
        		HttpWebRequest WebReq = (HttpWebRequest) HttpWebRequest.Create(new Uri(remoteURL));

        		if ( append ) {
        			WebReq.AddRange(offset);
			    }
        		
				WebReq.Credentials = Controller.GetNetworkCredential();
        		
        		using (HttpWebResponse response = (HttpWebResponse) WebReq.GetResponse()) {
        			using (Stream webStream = response.GetResponseStream()) {
						
						long length = response.ContentLength;
						int bufsiz = 2048;
						byte[] buffer = new byte[bufsiz];
												
						int readcount = webStream.Read(buffer, 0, bufsiz);
						long totalread = offset+readcount;
						
						DateTime lastupdate = DateTime.Now.ToLocalTime();
						DateTime thisupdate;
						float displayprogress = progress;
						
						while (readcount > 0 && !backgroundWorker.CancellationPending) {
							
							thisupdate = DateTime.Now.ToLocalTime();
							//backgroundWorker.ReportProgress(progress, "Debug " + thisupdate.Subtract(lastupdate).TotalSeconds);
							if (thisupdate.Subtract(lastupdate).TotalSeconds > 1) {
								backgroundWorker.ReportProgress((int) displayprogress, filenameshort + " " + totalread + "/"+length);
								lastupdate = thisupdate;
							}
							
							OutputStream.Write(buffer,0,readcount);
							
							readcount = webStream.Read(buffer, 0, bufsiz);
							totalread +=readcount;
							if (showfileprogress) {
								displayprogress = totalread *100 / length;
							}
							

						} 
        			}
        		}
        	}
        }
        
        
        //FIXME: this might as well go in the Controller.
        private bool ProcessChecksums(WebClient wc) {
	
        	bool read = false;
        	
        	try {
        		
        		using (StreamReader sr = new StreamReader(wc.OpenRead(GuiController.MAINURL + "/launcherS.dl.dat"))) {
				
        			read = Controller.SWGFiles.CreateFileList(sr,true);
        		}
        		

        		
        	} catch {}
        	
        	if (!read) {
        		Controller.AddDebugMessage("chksum exception/incomplete while reading from server.");
        	}
        	
        	if (!Controller.SWGFiles.HasFileList) {
        		
        		Controller.AddDebugMessage("can't use local checksums");
        		return false;
        	}

        	Controller.SWGFiles.WriteConfig(Controller.LocalFilelist);
        	
        	return true;
        	
        }
        


        private void PLAY_Click_1(object sender, EventArgs e) {
        	
        	if (StatusProcessor.Status == (int) StatusProcessor.StatusCodes.Updating) {
        		backgroundWorkerUpdate.CancelAsync();
        		return;
        	}
        	
        	//FIXME
        	if (StatusProcessor.Status == (int) StatusProcessor.StatusCodes.UpdatingFailed || StatusProcessor.Status == (int) StatusProcessor.StatusCodes.ChecksumFailed || StatusProcessor.Status == (int) StatusProcessor.StatusCodes.PatchingFailed) {
        		Process();
        		return;
        	}

        	//FIXME
        	if (StatusProcessor.Status != (int) StatusProcessor.StatusCodes.PatchingOK) {
        		Controller.PlaySound("Sound_Error");
        		return;
        	}
        	
        	/*
        	 * this needs some tweaking :P
        	SWGFile swgcl;
        	Controller.SWGFiles.SwgFileTable.TryGetValue("SWGClient_r.exe", out swgcl);
        	Controller.AddDebugMessage(swgcl.Checksum);
        	if (swgcl == null || !compareCheckSum(swgdirsave + @"\SwgClient_r.exe", swgcl.Checksum)) {
        		
        		MessageBox.Show("SwgClient_r.exe is not OK. Please use the scan button to fix.","SwgClient not OK", MessageBoxButtons.OK, MessageBoxIcon.Error);
        		
        		return;
        	}*/
        	

        	// this is actually "play" here now.
        	Controller.PlaySound("Sound_Play");

        	String address = "login1.projectswg.com";
        	String port = "44453";
       	
        	if (Controller.LocalhostOption) {
        		address = "127.0.0.1";
        		port = "44453";
        	}
        	
        	
        	//FIXME: use file list
        	//FIXME: unhandled exception
        	String logincfg = Controller.SwgSavePath + @"\login.cfg";
        	if (File.Exists(logincfg)) {
        		File.Delete(logincfg);
        	}

        	String args = String.Format("-- -s Station subscriptionFeatures=1 gameFeatures=34374193 -s ClientGame loginServerPort0={0} loginServerAddress0={1}", port, address);
        	
        	this.Hide();
            Directory.SetCurrentDirectory(Controller.SwgSavePath);
            System.Threading.Thread.Sleep(200);

        	try {
	        	this.Hide();
        		System.Diagnostics.Process.Start(Controller.FileSWGClient, args);
	            Application.Exit();
        			
        	} catch {
        		
        		GuiController.ShowErrorPermissions("Error starting swgclient_r.exe");
        	}


            return;
        	
        }


        private void Scan_Click(object sender, EventArgs e)
        {
        	
        	//FIXME
            //only scan if complete
            if (StatusProcessor.Status != (int) StatusProcessor.StatusCodes.PatchingOK && StatusProcessor.Status != (int) StatusProcessor.StatusCodes.PatchingFailed)
            {
            	return;
            }
            
            Controller.PlaySound("Sound_Click");
			
			
			SetStatus((int) StatusProcessor.StatusCodes.ScanningManual);
			Process();

        }


        private void button2_Click(object sender, EventArgs e)
        {
        	
        	Controller.PlaySound("Sound_Click");
			
        	if (OptionWindow != null) {
        		OptionWindow.RefreshButtonState();
        		OptionWindow.Show();
        		OptionWindow.BringToFront();
        		return;
        	}
        	
            LAUNCHOPTIONS launchoptions = new LAUNCHOPTIONS(Controller);
            launchoptions.Show();
            OptionWindow = launchoptions;
        }


        Point mouseDownPoint = Point.Empty;
        private void launcher2_MouseDown(object sender, MouseEventArgs e)
        
        {
            mouseDownPoint = new Point(e.X, e.Y);
        
        }

        private void launcher2_MouseClick(object sender, MouseEventArgs e)
        {

        }

        private void launcher2_MouseUp(object sender, MouseEventArgs e)
        {
            mouseDownPoint = Point.Empty;
        }

        private void launcher2_MouseMove(object sender, MouseEventArgs e)
        {
        	if (mouseDownPoint.IsEmpty) {
                return;
        	}
            Form f = sender as Form;
            f.Location = new Point(f.Location.X + (e.X - mouseDownPoint.X), f.Location.Y + (e.Y - mouseDownPoint.Y));
        }

        
        private void Donate_Click(object sender, EventArgs e)
        {
        	Controller.PlaySound("Sound_Click");
            Donate donate = new Donate(Controller);
            donate.Show();
        }
        

        private void MinimizeClick(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }


        private void CloseClick(object sender, EventArgs e)
        {
        	
        	//FIXME
        	if (StatusProcessor.Status == (int) StatusProcessor.StatusCodes.UpdatingChecksum || StatusProcessor.Status == (int) StatusProcessor.StatusCodes.Patching)
            {
               
        		DialogResult res = MessageBox.Show("PSWG Launcher is still patching. Are you sure you want to close PSWG Launcher?", "Close ProjectSWG Launcher?",MessageBoxButtons.YesNo);
        		
        		if (res == DialogResult.No) {
        			return;
        		}
        		
            }
        	
            Application.Exit();
        }

        private void acct_Click_1(object sender, EventArgs e)
        {
        	
        	Controller.PlaySound("Sound_Click");
            AccountWindow acct = new AccountWindow(Controller);
            acct.Show();
        }
        
        private void options_Click_1(object sender, EventArgs e)
        {
            if (File.Exists(Controller.SwgSavePath + @"\SwgClientSetup_r.exe"))
            {
            	Controller.PlaySound("Sound_Click");
                System.Diagnostics.Process.Start(Controller.SwgSavePath + @"\SwgClientSetup_r.exe");
            }
            else
            {
            	Controller.AddDebugMessage("Cannot launch swgclientsetup_r.exe because it is missing.");
            	Controller.PlaySound("Sound_Error");
            }
        }        
        
        
    }
}
       
       

        


       
    

        
    


