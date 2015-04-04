/*
 * Created by SharpDevelop.
 * User: rdo
 * Date: 22.12.2012
 * Time: 18:46
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace PswgLauncher
{
	/// <summary>
	/// Description of Class1.
	/// </summary>
	public class SWGFileList
	{
		
		private GuiController Controller;

		private Dictionary<String,SWGFile> _swgfiletable;
		public Dictionary<String,SWGFile> SwgFileTable {
			get {
				return _swgfiletable;
			}
		}
		
		private int _timestamp;
		public int FileTableTimestamp {
			get {
				return _timestamp;
			}
		}
		
		private bool _hasFileList = false;
		public bool HasFileList {
			get {
				return _hasFileList;
			}
		}
		
		public bool ChecksumsDirty {
			get; private set;
		}
		
		public SWGFileList(GuiController gc)
		{
			
			Controller = gc;
			
			_swgfiletable = new Dictionary<String, SWGFile>();
			
		}
		
		
		
		//FIXME: I think the next 5 methods can be simplified...
		private String ReadMasterChecksum(StreamReader SR) {
			
			String line;
			
			Regex r1 = new Regex(@"^([0-9a-fA-F]{32})\s+(-)$");
			
			// bug out if we can't read a line...
			if ( ( line=SR.ReadLine() ) ==null) {
				return null;
			}
			Match m1 = r1.Match(line);
			//bug out if first line doesn't hold the whole things checksum...
			if (!m1.Success) {
				return null;
			}
			
			return m1.Groups[1].Value;
			
		}
		
		private int RetrieveLineNumber(StreamReader SR) {
			String line;

			if ( ( line=SR.ReadLine()) == null) {
				return -1;
			}
			
			if (!System.Text.RegularExpressions.Regex.IsMatch(line, @"^[0-9]+$")) {
				return -1;
			}
			
			return int.Parse(line);
			
		}
		
		private int RetrieveTimestamp(StreamReader SR) {
			String line;

			if ( ( line=SR.ReadLine()) == null) {
				return -1;
			}
			
			if (!System.Text.RegularExpressions.Regex.IsMatch(line, @"^[0-9]{10,}$")) {
				return -1;
			}
			
			return int.Parse(line);
			
		}
		
		private bool HasBegin(StreamReader SR) {
			
			String line;
			
			//next line should be "BEGIN"
			if ( ( line=SR.ReadLine()) == null) {
				return false;
			}
			
			if (!line.Equals("BEGIN")) {
				return false;
			}
			
			return true;
		}
		
		private bool IsEnd(String line) {
			
			
			//next line should be "END"
			if ( line == null) {
				return false;
			}
			
			if (!line.Equals("END")) {
				return false;
			}
			
			return true;
		}
		
		public Dictionary<String,SWGFile> LoopChecksums(StreamReader SR) {
			
			String line;
			List<String> text = new List<String>();
			Dictionary<String,SWGFile> NewChecksums = new Dictionary<String,SWGFile>();
			
			Regex regex = new Regex(@"^([0-9]+)\s+([0-9a-fA-F]{32})\s+([0-9]+)\s+(\S+)$");
			
			while ((line = SR.ReadLine()) != null) {
        			Match match = regex.Match(line);
        			
        			if (match.Success) {
        				//Debug.WriteLine("Found " + match.Groups[2].Value + ":" + match.Groups[1].Value);
        				
        				SWGFile swgfile = new SWGFile(match.Groups[4].Value, int.Parse(match.Groups[1].Value) , match.Groups[2].Value, int.Parse(match.Groups[3].Value), Controller);
        				
        				NewChecksums.Add(match.Groups[4].Value, swgfile);
        				
        				text.Add(line) ;
        				continue;
        			}
        			
        			if (IsEnd(line)) {
        				break;
        			} 
        		
        			Controller.AddDebugMessage("malformed Checksum list." + line);
        			return null;
        			
        	}
			
			return NewChecksums;
			
		}
		

		public bool CreateFileList(StreamReader SR, bool isDownload) {
			
			String Checksum;
			int LineNumber;
			int MasterTimestamp;
			if ( (LineNumber = RetrieveLineNumber(SR)) <= 0 )  {
				Controller.AddDebugMessage("remote file is empty.");
				return false;
			}
			
			if ( (MasterTimestamp = RetrieveTimestamp(SR)) <= 0)  {
				Controller.AddDebugMessage("remote file is malformed.");
				return false;
			}
			
			if (this.HasFileList && this._timestamp != null && this._timestamp >= MasterTimestamp) {
				Controller.AddDebugMessage("new list timestamp less or equal, ignoring new list. " + _timestamp + " vs " + MasterTimestamp);
				return true;
			}
			
			if (!HasBegin(SR)) {
				Controller.AddDebugMessage("remote file is malformed.");
				return false;
			}

			Dictionary<String,SWGFile> NewDictionary = LoopChecksums(SR);
			
			if (NewDictionary == null)  {
				Controller.AddDebugMessage("Couldn't read file list from "  + ((isDownload) ? "server" : "disk") );
				return false;
			}
			
			if (NewDictionary.Count != LineNumber) {
				Controller.AddDebugMessage("Line Number mismatch" );
				return false;				
			}

			this._swgfiletable = NewDictionary;
			this._timestamp = MasterTimestamp;
			this._hasFileList = true;
			
			if (isDownload) {
				ChecksumsDirty = true;
			}
			Controller.AddDebugMessage("Successfully read file list from " + ((isDownload) ? "server" : "disk"));
			return true;
			
		}
		
		public bool WriteConfig(String filename) {
			if (!this._hasFileList) {
				Controller.AddDebugMessage("There's no file list here.");
				return false;
			}
			
			RijndaelManaged aes = new RijndaelManaged();
			byte[] key = ASCIIEncoding.UTF8.GetBytes(GuiController.EncKey);
			
			try {
				
				
				using (FileStream fs = new FileStream(filename,FileMode.Create)) {
					using (CryptoStream cs = new CryptoStream(fs, aes.CreateEncryptor(key, key), CryptoStreamMode.Write)) {
						using (StreamWriter file = new StreamWriter(cs)) {
					
							file.WriteLine(this._swgfiletable.Count.ToString());
							file.WriteLine(this._timestamp.ToString());
							file.WriteLine("BEGIN");
							
							foreach (KeyValuePair<String,SWGFile> kv in this._swgfiletable) {
								file.WriteLine(kv.Value.ToString());
							}
							file.WriteLine("END");
							
							aes.Clear();
						}
					}
				} 
			} catch (Exception e) {
				Controller.AddDebugMessage("Something went wrong writing file list." + e.ToString());
				return false;
			} 
			
			
			return true;
		}
		
		public void ReadLocalConfigUnEnc() {

			if (!File.Exists(Controller.LocalFilelist)) {
				Controller.AddDebugMessage("No " + Controller.LocalFilelist + " present, needs downloading.");
				return;
			}
			
			RijndaelManaged aes = new RijndaelManaged();
			byte[] key = ASCIIEncoding.UTF8.GetBytes(GuiController.EncKey);			
			
			try {
				Controller.AddDebugMessage("Reading " + Controller.LocalFilelist);
				using (FileStream fs = new FileStream(Controller.LocalFilelist,FileMode.Open)) {
					
						using (StreamReader sr = new StreamReader(fs)) {
							CreateFileList(sr,false);
						}
					
				}
						
			} catch (Exception e) {
				Controller.AddDebugMessage("No luck reading " + Controller.LocalFilelist + ", needs downloading.");
			}
			
		}
		
		public void ReadLocalConfig() {
			
			if (!File.Exists(Controller.LocalFilelist)) {
				Controller.AddDebugMessage("No " + Controller.LocalFilelist + " present, needs downloading.");
				return;
			}
			
			RijndaelManaged aes = new RijndaelManaged();
			byte[] key = ASCIIEncoding.UTF8.GetBytes(GuiController.EncKey);			
			
			try {
				Controller.AddDebugMessage("Reading " + Controller.LocalFilelist);
				using (FileStream fs = new FileStream(Controller.LocalFilelist,FileMode.Open)) {
					using (CryptoStream cs = new CryptoStream(fs, aes.CreateDecryptor(key, key), CryptoStreamMode.Read)) {
						using (StreamReader sr = new StreamReader(cs)) {
							CreateFileList(sr,false);
						}
					}
				}
						
			} catch (Exception e) {
				Controller.AddDebugMessage("No luck reading " + Controller.LocalFilelist + ", needs downloading.");
			}

		}
		
		public bool IsGood(String filename) {
			if (filename == null) {
				return false;
			}
			
			if (! _swgfiletable.ContainsKey(filename)) {
				return false;
			}
			
			SWGFile swgfile;
			_swgfiletable.TryGetValue(filename, out swgfile);
			
			if (swgfile == null) {
				return false;
			}
			
			return swgfile.IsGood();
		}


        public bool IsComplete() {
        	
        	foreach (KeyValuePair<String,SWGFile> file in _swgfiletable) {
        		
				if (file.Value.IsGood()) {
		           	continue;
		        }

        		return false;
        		
        	}
        	
        	return true;

        }

		
		public void ResetGoodFiles() {
			
			foreach (KeyValuePair<String,SWGFile> file in _swgfiletable) {
				
				file.Value.Reset();
				
			}
			
		}
		
		public void Scan(bool WithChecksums, BackgroundWorker bgWorker) {
			
			ResetGoodFiles();
			
			float step = (float) 100 / (float) _swgfiletable.Count;
			float progress = 0;
			
			DateTime lastupdate = DateTime.Now.ToLocalTime();
			DateTime thisupdate;
			
			foreach (KeyValuePair<String,SWGFile> file in _swgfiletable) {
				
				progress += step;
				int progressDisplay = Convert.ToInt32(progress);

				file.Value.UpdateSavepath(2, false, (WithChecksums || ChecksumsDirty));
				
				thisupdate = DateTime.Now.ToLocalTime();
				if (thisupdate.Subtract(lastupdate).TotalSeconds > 1) {
					bgWorker.ReportProgress( progressDisplay, file.Key);
					lastupdate = thisupdate;
				}
				
			}
			
			//FIXME: this might not be pretty if stuff is interrupted.
			ChecksumsDirty = false;
			
		}
		
		
		//FIXME: move to utility class...
		static byte[] GetBytes(String str)
		{
			
		    byte[] bytes = new byte[str.Length * sizeof(char)];
    		System.Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
    		
    		return bytes;

		}
		
		

		
	
	}
	

	
}
