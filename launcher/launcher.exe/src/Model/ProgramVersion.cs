/*
 * Created by SharpDevelop.
 * User: rdo
 * Date: 07.01.2013
 * Time: 21:28
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;

namespace PswgLauncher
{
	/// <summary>
	/// Description of ProgramVersion.
	/// </summary>
	public class ProgramVersion
	{
		
		public int Major;
		public int Minor;
		public int Maintenance;
		public int Build;
		
		public ProgramVersion(String VersionString)
		{
			
			ParseVersion(VersionString);
			
		}
		
		
		private void ParseVersion(String ver) {
			
			char[] delimiter = {'.'};
			
			String[] tokens = ver.Split(delimiter, 4);
			
			int i = 1;
			foreach (String s in tokens) {
				
				int tmp;
				
				try {
					
					tmp = int.Parse(s);
					
					
				} catch {
					tmp = 0;
				}	
				
				switch (i) {
					case 1:
						Major = tmp;
						break;
					case 2:
						Minor = tmp;
						break;
					case 3:
						Maintenance = tmp;
						break;
					case 4:
						Build = tmp;
						break;
				}

				
				
				i++;
			}
			
			
		}
		
		public override string ToString()
		{
			return string.Format("[ProgramVersion Major={0}, Minor={1}, Maintenance={2}, Build={3}]", Major, Minor, Maintenance, Build);
		}
		
		public string ToCompactString()
		{
			return string.Format("{0}.{1}.{2}.{3}", Major, Minor, Maintenance, Build);
		}
			
			
		public bool IsNewerThan(ProgramVersion ver) {
			
			
			if (ver == null) {
				return true;
			}
				
			if (Major < ver.Major) {
				return false;
			} else if (Major > ver.Major) {
				return true;
			}
				
			if (Minor < ver.Minor) {
				return false;
			} else if (Minor > ver.Minor) {
				return true;
			}
				
			if (Maintenance < ver.Maintenance) {
				return false;
			} else if (Maintenance > ver.Maintenance) {
				return true;
			}
			if (Build < ver.Build) {
				return false;
			} else if (Build < ver.Build) {
				return true;
			}
				
			return true;
				
		}
			
			
			
		
		
	}
}
