/*
 * Created by SharpDevelop.
 * User: rdo
 * Date: 02.03.2013
 * Time: 21:51
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace PswgLauncher
{
	/// <summary>
	/// Description of LauncherLabel.
	/// </summary>
	public class LauncherLabel : System.Windows.Forms.Label
	{
		public LauncherLabel()
		{
		}
		
		
		protected override void OnPaint(PaintEventArgs e) {
			//base.OnPaint(e);
			DrawText(e);
		}
		
		
		private void DrawText(PaintEventArgs pe) {
			
			if (base.Text == null || base.Text.Trim() == "" || pe == null || base.Font == null || base.ForeColor == null) {
				return;
			}
			
			PointF DrawPoint = new PointF(0,base.Height /2);
			
			pe.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
			pe.Graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
			
			SolidBrush Brush = new SolidBrush(base.ForeColor);
			FontFamily fontFamily = base.Font.FontFamily;
			StringFormat strf = new StringFormat();
//			strf.Alignment = StringAlignment.;
			strf.LineAlignment = StringAlignment.Center;
			GraphicsPath path = new GraphicsPath();
			
			String drstr = Text.Replace('_', ' ');
			
			
			path.AddString(drstr, fontFamily, (int) FontStyle.Regular, 10.0f, DrawPoint, strf);
			
			
			Pen pen = new Pen(Color.FromArgb(90, 90, 90), 3);
			pe.Graphics.DrawPath(pen,path);
			pe.Graphics.FillPath(Brush,path);			
			
			
			
		}		
		
		
	}
}
