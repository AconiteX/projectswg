/*
 * Created by SharpDevelop.
 * User: rdo
 * Date: 17.03.2013
 * Time: 14:16
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Drawing;

namespace PswgLauncher
{
	/// <summary>
	/// Description of StatusProcessor.
	/// </summary>
	public class StatusProcessor
	{
		
		/* 
		 * FIXME: the mess was moved from launcher2 to this class.
		 * The data should probably get a proper object representation.
		 * 
		 */
		
        public enum StatusCodes :int {
        	
			NoStatus = 0,
			Updating = 1,
			UpdatingFailed = 2,
			UpdatingOK = 3,
        	UpdatingChecksum = 4,
        	ChecksumFailed = 5,
        	ChecksumOK = 6,
        	Scanning = 7,
        	ScanningManual = 8,
        	ScanningFailed = 9,
        	ScanningOK = 10,
        	Patching = 11,
        	PatchingFailed = 12,
        	PatchingOK = 13
        	
        }
		
		
		private String[] PlayTexts = new String[] {
			"updating",
			"skip update",
			"skip update",
			"Get Checksums",
			"DL Checksums",
			"Retry Checksums",
			"Scan",
			"Scanning",
			"Scanning",
			"Retry Scan",
			"Patch",
			"Patching",
			"Retry Patch",
			"Play"
			
		};
		
		private String[] LabelTexts = new String[] {
			
			"launcher needs checking for updates",
			"checking for launcher updates",
			"launcher updating failed.",
			"checksums need checking",
			"DL'ing Checksums",
			"Checksum DL failed.",
			"Checksums loaded.",
			"Scanning",
			"Scanning",
			"Scanning Failed",
			"Scanning complete!",
			"Patching",
			"Patching Failed",
			"Ready to play!"
			
		};
		
		private GuiController Controller;
		
		private int _status;

		public int Status {
			get {
				return _status;
			}
			set {
				if (!IsValidChange(value)) {
					return;
				}
				
				_status = value;
				
			}
		}
		
		public StatusProcessor(GuiController ctrl)
		{
			_status = (int) StatusCodes.NoStatus;
			this.Controller = ctrl;
		}
		
		public bool SetNewState(int newstatus) {
			
			if (!IsValidChange(newstatus)) {
				return false;
			}
			
			Status = newstatus;
			
			return true;
		}
		
		
		public bool IsValidChange(int newstatus) {

        	if (newstatus < (int) StatusCodes.NoStatus || newstatus > (int) StatusCodes.PatchingOK) {
        		return false;
        	}
			
			if (Array.IndexOf(GetValidTargetStatus(_status),newstatus) >= 0 ) {
				return true;
			}
			
			return false;
			
		}
		
		public int[] GetValidTargetStatus(int currentstatus) {
			
			switch (currentstatus) {
				case (int) StatusCodes.NoStatus:
					return new int[] { (int) StatusCodes.Updating };
					break;
				case (int) StatusCodes.Updating:
					return new int[] { (int) StatusCodes.UpdatingOK, (int) StatusCodes.UpdatingFailed };
					break;
	        	case (int) StatusCodes.UpdatingOK:
				case (int) StatusCodes.UpdatingFailed:
					return new int[] { (int) StatusCodes.UpdatingChecksum };
					break;
	        	case (int) StatusCodes.UpdatingChecksum:
					return new int[] { (int) StatusCodes.ChecksumOK, (int) StatusCodes.ChecksumFailed };
					break;
	        	case (int) StatusCodes.ChecksumFailed:
					return new int[] { (int) StatusCodes.UpdatingChecksum };
					break;
	        	case (int) StatusCodes.ChecksumOK:
					return new int[] { (int) StatusCodes.Scanning, (int) StatusCodes.ScanningManual };
					break;
	        	case (int) StatusCodes.Scanning:
					return new int[] { (int) StatusCodes.ScanningOK, (int) StatusCodes.ScanningFailed };
					break;
	        	case (int) StatusCodes.ScanningManual:
					return new int[] { (int) StatusCodes.ScanningOK, (int) StatusCodes.ScanningFailed };
					break;					
	        	case (int) StatusCodes.ScanningFailed:
					return new int[] { (int) StatusCodes.Scanning, (int) StatusCodes.ScanningManual };
					break;
	        	case (int) StatusCodes.ScanningOK:
					return new int[] { (int) StatusCodes.Patching };
					break;
	        	case (int) StatusCodes.Patching:
					return new int[] { (int) StatusCodes.PatchingOK, (int) StatusCodes.PatchingFailed };		
					break;
	        	case (int) StatusCodes.PatchingFailed:
					return new int[] { (int) StatusCodes.Patching, (int) StatusCodes.Scanning, (int) StatusCodes.ScanningManual };
					break;
	        	case (int) StatusCodes.PatchingOK:
					return new int[] { (int) StatusCodes.Scanning, (int) StatusCodes.ScanningManual };
					break;				
			}
			
			return new int[] { };
			
		}
		
		public int GetNextStatus() {

			switch (_status) {
				case (int) StatusCodes.NoStatus:
					return (int) StatusCodes.Updating;
					break;
				case (int) StatusCodes.UpdatingFailed:
	        	case (int) StatusCodes.UpdatingOK:
					return (int) StatusCodes.UpdatingChecksum;
					break;
	        	case (int) StatusCodes.ChecksumFailed:
					return (int) StatusCodes.UpdatingChecksum;
					break;
	        	case (int) StatusCodes.ChecksumOK:
					return (int) StatusCodes.Scanning;
					break;
	        	case (int) StatusCodes.ScanningFailed:
					return (int) StatusCodes.Scanning;
					break;
	        	case (int) StatusCodes.ScanningOK:
					return (int) StatusCodes.Patching;
					break;
	        	case (int) StatusCodes.PatchingFailed:
					return (int) StatusCodes.Patching;
					break;
			}
			
			return -1;
		}
		
		public bool GetPlayDisabled() {
			
			switch(_status) {
				
				//case (int) StatusCodes.Updating:					
				case (int) StatusCodes.UpdatingChecksum:
				case (int) StatusCodes.Scanning:
				case (int) StatusCodes.ScanningManual:
				case (int) StatusProcessor.StatusCodes.Patching:
					return true;
					break;
			}
			
			return false;
			
		}
		
		
		public bool GetScanDisabled() {
			
			switch(_status) {
					
				case (int) StatusCodes.Updating:
				case (int) StatusCodes.UpdatingChecksum:
				case (int) StatusCodes.ChecksumFailed:
				case (int) StatusCodes.Scanning:
				case (int) StatusCodes.ScanningManual:
				case (int) StatusProcessor.StatusCodes.Patching:
					return true;
					break;
			}
			
			return false;
		}
		
		
		public bool IsBusy() {

			switch(_status) {
				case (int) StatusCodes.Updating:
				case (int) StatusCodes.UpdatingChecksum:
				case (int) StatusCodes.Scanning:
				case (int) StatusCodes.ScanningManual:
				case (int) StatusProcessor.StatusCodes.Patching:
					return true;
					break;
			}
			
			return false;			
		}
		
		public String GetPlayText() {
			
			return PlayTexts[_status];
			
		}
		
		public String GetLabelText() {
			return LabelTexts[_status] + "(" + _status + ")";
		}
		
		
		public Color GetStatusColor() {
			
			switch(_status) {
				case (int) StatusCodes.UpdatingFailed:	
				case (int) StatusCodes.ChecksumFailed:
				case (int) StatusCodes.ScanningFailed:
				case (int) StatusCodes.PatchingFailed:
					return Color.Red;
					break;
				
				case (int) StatusCodes.PatchingOK:
					return Color.Aqua;
					break;
					
					
			}
			
			return Color.Blue;
			
		}
		
	}
	
}
