/*
 * Created by SharpDevelop.
 * User: rdo
 * Date: 19.02.2013
 * Time: 19:18
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;

namespace PswgLauncher
{
	/// <summary>
	/// Description of SWGFile.
	/// </summary>
	public class SWGFile
	{
	
		public enum Strictness :int {
			DeleteFile = -1,
			AlwaysOK = 0,
			ScanIfRequired = 1,
			AlwaysScan = 2
		}
		
		public enum FileStatus :int {
			Unknown = -1,
			Exists = 0,
			FilesizeOK = 1,
			ChecksumOK = 2
		}
		
		public int StrictnessLevel {
			get; private set;
		}
		
		public int SwgdirStatusLevel {
			get; private set;
		}
		
		public int SavepathStatusLevel {
			get; private set;
		}
		
		public String Filename {
			get; private set; 
		}
		
		public String Checksum {
			get; private set;
		}
		
		public int Filesize {
			get; private set;
		}

		private GuiController Controller;
		
		public SWGFile(String File, int Strict, String MDFiveSum, int Size, GuiController ctrl)
		{
			Filename = File;
			StrictnessLevel = Strict;			
			Checksum = MDFiveSum;
			Filesize = Size;
			
			SwgdirStatusLevel = -1;
			SavepathStatusLevel = -1;
			
			this.Controller = ctrl;
			
		}
		
		public override String ToString() {
			return StrictnessLevel + " " + Checksum + " " + Filesize.ToString() + " " + Filename;
		}


		public void Reset() {
			SwgdirStatusLevel = -1;
			SavepathStatusLevel = -1;
		}
		
		public FileInfo GetFileInfo() {
			String path = Controller.SwgSavePath + @"\" + this.Filename;
			if (!File.Exists(path)) {
				return null;
			}
			return new FileInfo(path);
		}
		
		public bool UpdateSavepath(int RequiredLevel, bool overrideAlwaysOK, bool forceScan) {
			//int rl = Math.Max(RequiredLevel, StrictnessLevel);			
			int rl = Math.Min(RequiredLevel, StrictnessLevel);
			if (overrideAlwaysOK && StrictnessLevel == (int) Strictness.AlwaysOK) {
				rl = 2;
			} else if ( (forceScan || Controller.checksumOption) && StrictnessLevel == (int) Strictness.ScanIfRequired) {
				rl = 2;
			}
			
			//Empty files are always good, they just need their dir created.
			if (( this.Filesize <=0) && (File.Exists(Controller.SwgSavePath + @"\" + this.Filename))) {
				return true;
			}
			
			if (SavepathStatusLevel == (int)FileStatus.Unknown) {
				if (File.Exists(Controller.SwgSavePath + @"\" + this.Filename)) {
					SavepathStatusLevel++;
					if (rl == 0) { return true; }
				} else {
					return false;
				}				
			}
			
			if (SavepathStatusLevel == (int)FileStatus.Exists) {
				FileInfo f = new FileInfo(Controller.SwgSavePath + @"\" + this.Filename);
				if (f.Length == this.Filesize) {
					SavepathStatusLevel++;
					if (rl == 1) { return true; }
				} else {
					return false;
				}
			}
			
			if (SavepathStatusLevel == (int)FileStatus.FilesizeOK) {
				if (SWGFile.MatchChecksum(Controller.SwgSavePath + @"\" + this.Filename, this.Checksum)) {
					SavepathStatusLevel++;					
					return true;
				}
			}
			
			return false;
		}
		
		public bool UpdateSwgpath(int RequiredLevel) {
			
			int rl = RequiredLevel;
			
			if (SwgdirStatusLevel >= rl) { return true; }
			
			if (SwgdirStatusLevel == (int) FileStatus.Unknown) {
				if (File.Exists(Controller.SwgDir + @"\" + this.Filename)) {
					SwgdirStatusLevel++;
					if (SwgdirStatusLevel >= rl) { return true; }
				} else {
					return false;
				}				
			}
			
			if (SwgdirStatusLevel == (int) FileStatus.Exists) {
				FileInfo f = new FileInfo(Controller.SwgDir + @"\" + this.Filename);
				if (f.Length == this.Filesize) {
					SwgdirStatusLevel++;
					if (SwgdirStatusLevel >= rl) { return true; }
				} else {
					return false;
				}
			}
			
			if (SwgdirStatusLevel == (int) FileStatus.FilesizeOK) {
				if (SWGFile.MatchChecksum(Controller.SwgDir + @"\" + this.Filename, this.Checksum)) {
					SwgdirStatusLevel++;					
					if (SwgdirStatusLevel >= rl) { return true; }
				}
			}
			
			return false;
			
		}
		
		public bool IsGood() {
			if (Filesize <= 0) {
				return true;
			}
			return (SavepathStatusLevel >= StrictnessLevel);
		}

		public bool SrcIsGood() {
			return (SwgdirStatusLevel >= (int)FileStatus.ChecksumOK);
		}
		
		public bool MakeDirIfRequired() {
        	
        	if (!Filename.Contains("/")) {
        		return true;
        	}
        	
        	// split on last /
        	Regex r = new Regex(@"^(.+)\/([^/]+)$");
        	
        	Match m1 = r.Match(Filename);
        	
        	if (!m1.Success) {
        		return true;
        	}
	
        	try {
        		Directory.CreateDirectory(Controller.SwgSavePath + @"\" + m1.Groups[1].Value);
        	} catch (Exception e) {
        		
        		return false;
        	}
        	
        	return true;
			
		}
		
		public void TouchFileIfRequired() {
			if (File.Exists(Controller.SwgSavePath + @"\" + Filename)) {
				return;
			}
			FileStream x = File.Create(Controller.SwgSavePath + @"\" + Filename);
			x.Close();
		}
		
		//FIXME: hack for re-usability. should be in a helper class.
        public static bool MatchChecksum(string Path, string chk) {

			if (!File.Exists(Path)) {
				return false;
			}
			
			System.IO.FileStream FileCheck = System.IO.File.OpenRead(Path);                
			System.Security.Cryptography.MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
			byte[] md5Hash = md5.ComputeHash(FileCheck);                
			FileCheck.Close();
			                
			string Calc =   BitConverter.ToString(md5Hash).Replace("-", "").ToLower();
			if (Calc.TrimEnd() == chk.ToLower().TrimEnd()) {
				return true;
			}
			
			return false;
		}
		
		
		
		
	}
}
