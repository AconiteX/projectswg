/*
 * Created by SharpDevelop.
 * User: rdo
 * Date: 08.01.2013
 * Time: 19:51
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace PswgLauncher
{
	
	
	
	/// <summary>
	/// Description of LauncherButton.
	/// </summary>
	public class LauncherButton : PictureBox, IButtonControl
	{
		
		
		private bool clicked = false;
		private bool _disabled = false;
		private bool hover = false;
		
		private Image _ImageNormal;
		private Image _ImageClick;
		private Image _ImageHover;
		private Image _ImageDisable;

		private Color _TextColorNormal;
		private Color _TextColorHover;
		private Color _TextColorClick;
		private Color _TextColorDisable;

		//FIXME: This is a little hackish
		public int Smaller1 { get; set; }
		public int Smaller2 { get; set; }
		
		public bool Disable {
			get { return _disabled; }
			set {
				_disabled = value;
				
				this.ChangeState();
				
			}
		}
		
		
		[Category("Appearance")]
		[Description("Image to show when button is active and not clicked.")]
		public Image ImageNormal {
			get { return _ImageNormal; }
			
			set {
				_ImageNormal = value;
				this.ChangeState();
			}
		}

		[Category("Appearance")]
		[Description("Image to show when button is active clicked.")]
		public Image ImageClick {
			get { return _ImageClick; }
			set {
				_ImageClick = value;
				this.ChangeState();
			}
		}
		
		[Category("Appearance")]
		[Description("Image to show when button is hovered.")]
		public Image ImageHover {
			get { return _ImageHover; }
			set {
				_ImageHover = value;
				this.ChangeState();
			}
		}		
		
		[Category("Appearance")]
		[Description("Image to show when button is disabled.")]
		public Image ImageDisable {
			get { return _ImageDisable; }
			set {
				_ImageDisable = value;
				this.ChangeState();
			}
		}
		
		
		[Category("Appearance")]
		[Description("Normal Text Color")]
		public Color TextColorNormal {
			get { return _TextColorNormal; }
			set {
				_TextColorNormal = value;
				this.ChangeTextState();
			}
		}

		[Category("Appearance")]
		[Description("Hover Text Color")]
		public Color TextColorHover {
			get { return _TextColorHover; }
			set {
				_TextColorHover = value;
				this.ChangeTextState();
			}
		}

		
		[Category("Appearance")]
		[Description("Clicked Text Color")]
		public Color TextColorClick {
			get { return _TextColorClick; }
			set {
				_TextColorClick = value;
				this.ChangeTextState();
			}
		}
		
		
		[Category("Appearance")]
		[Description("Clicked Text Color")]
		public Color TextColorDisable {
			get { return _TextColorDisable; }
			set {
				_TextColorDisable = value;
				this.ChangeTextState();
			}
		}
		
		
		
		[Browsable(true)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
		[Category("Appearance")]
		[Description("Text to be displayed")]
		public override string Text {
			get {
				return base.Text;
			}
			set {
				base.Text = value;
				
			}
		}
		
		[Browsable(true)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
		[Category("Appearance")]
		[Description("Font to use for text")]
		public override Font Font
		{
		    get
		    {
		        return base.Font;
		    }
		    set
		    {
		        base.Font = value;
		        this.ChangeTextState();
		    }
		}
		
		
		
		
		public LauncherButton() 
		{
			Smaller1 = 11;
			Smaller2 = 14;
		}
		
		//implementing interface
		private DialogResult _DialogResult;
				
		public DialogResult DialogResult {
			get {
				return _DialogResult;
			}
			set {
				_DialogResult = value;
			}
		}
		
		private bool _IsDefault = false;
		public void NotifyDefault(bool value)
		{
			_IsDefault = value;
		}
		
		public void PerformClick()
		{
			base.OnClick(EventArgs.Empty);
		}
		
		
		
		protected override void OnPaint(PaintEventArgs pe) {
			
			base.OnPaint(pe);
			
			if (Text == null || Text.Trim() == "" || pe == null || base.Font == null) {
				return;
			}
			
			

			PointF DrawPoint = new PointF(base.Image.Width /2,base.Image.Height /2);
			
			pe.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
			pe.Graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
			
			SolidBrush Brush = new SolidBrush(base.ForeColor);
			FontFamily fontFamily = base.Font.FontFamily;
			StringFormat strf = new StringFormat();
			strf.Alignment = StringAlignment.Center;
			strf.LineAlignment = StringAlignment.Center;
			GraphicsPath path = new GraphicsPath();
			
			if (Text.Length > Smaller2) {
				path.AddString(Text, fontFamily, (int) FontStyle.Regular, 8.0f, DrawPoint, strf);
			} else if (Text.Length > Smaller1) {
				path.AddString(Text, fontFamily, (int) FontStyle.Regular, 10.0f, DrawPoint, strf);
			} else {
				path.AddString(Text, fontFamily, (int) FontStyle.Regular, 12.0f, DrawPoint, strf);
			}
			
			
			Pen pen = new Pen(Color.FromArgb(90, 90, 90), 3);
			pe.Graphics.DrawPath(pen,path);
			pe.Graphics.FillPath(Brush,path);
			
			//pe.Graphics.DrawString(base.Text, base.Font, Brush, DrawPoint);
			
			
		}
		
		protected override void OnTextChanged(EventArgs e) {
			Refresh();
			base.OnTextChanged(e);
		}
		
		
		
		protected override void OnMouseMove(MouseEventArgs e) {
			
			hover = true;
			ChangeState();
			
			base.OnMouseMove(e);
			
		}
		
		protected override void OnMouseLeave(EventArgs e) {
			hover = false;
			ChangeState();
			base.OnMouseLeave(e);

		}
		
		protected override void OnMouseDown(MouseEventArgs e) {
			base.Focus();
			clicked = true;
			ChangeState();
			base.OnMouseDown(e);
		}
		
		protected override void OnMouseUp(MouseEventArgs e) {
			clicked = false;
			
			ChangeState();
			base.OnMouseUp(e);
			
		}
		
		
		
		
		
		
		protected void ChangeState() {
			
			ChangeImageState();
			ChangeTextState();
				
		}
		
		
		protected void ChangeImageState() {
			
			
			if (_disabled) {
				
				if ((_ImageDisable != null)) {
					base.Image = _ImageDisable;
					return;
				}
				
			}
			
			if (clicked) {
				
				if ((_ImageClick != null)) {
					base.Image = _ImageClick;
					return;
				}
				
			}
			
			if (hover) {
				
				if ((_ImageHover != null)) {
					base.Image = _ImageHover;
					return;
				}
				
			}

			
			
			if ((_ImageNormal != null)) {
				base.Image = _ImageNormal;
				return;
			}
			
		}
		
		
		protected void ChangeTextState() {
			
			
			if (_disabled) {
				
				if ((_TextColorDisable != null)) {
					base.ForeColor = _TextColorDisable;
					
					return;
				}
				
			}

			if (clicked) {
				
				if ((_TextColorClick != null)) {
					base.ForeColor = _TextColorClick;
					return;
				}
				
			}
			
			if (hover) {
				
				if ((_TextColorHover != null)) {
					base.ForeColor = _TextColorHover;
					return;
				}
				
			}
			
			if ((_TextColorNormal != null)) {
				base.ForeColor = _TextColorNormal;
				return;
			}


			
			
		}
		
		
		//keyboard handling... needs some rewriting.
		
		private const int WM_KEYDOWN = 0x0100;
		private const int WM_KEYUP = 0x0101;
		private bool holdingSpace = false;
		public override bool PreProcessMessage(ref Message msg)
		{
		    if (msg.Msg == WM_KEYUP)
		    {
		        if (holdingSpace)
		        {
		            if ((int)msg.WParam == (int)Keys.Space)
		            {
		                OnMouseUp(null);
		                PerformClick();
		            }
		            else if ((int)msg.WParam == (int)Keys.Escape
		                || (int)msg.WParam == (int)Keys.Tab)
		            {
		                holdingSpace = false;
		                OnMouseUp(null);
		            }
		        }
		        return true;
		    }
		    else if (msg.Msg == WM_KEYDOWN)
		    {
		        if ((int)msg.WParam == (int)Keys.Space)
		        {
		            holdingSpace = true;
		            OnMouseDown(null);
		        }
		        else if ((int)msg.WParam == (int)Keys.Enter)
		        {
		            PerformClick();
		        }
		        return true;
		    }
		    else
		        return base.PreProcessMessage(ref msg);
		}
		
		protected override void OnLostFocus(EventArgs e)
		{
		    holdingSpace = false;
		    OnMouseUp(null);
		    base.OnLostFocus(e);
		}
				
				


		
		//changed description
		
		[Description("Controls how the LauncherButton will handle image placement and control sizing.")]
		public new PictureBoxSizeMode SizeMode {
			get { return base.SizeMode; }
			set { base.SizeMode = value; }
		}
		
		[Description("Controls what type of border the LauncherButton should have.")]
		public new BorderStyle BorderStyle {
			get { return base.BorderStyle; }
			set { base.BorderStyle = value; }
		}
		

		//removed stuff		
		
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public new Image Image { get { return base.Image; } set { base.Image = value; } }
		
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public new ImageLayout BackgroundImageLayout {
			get { return base.BackgroundImageLayout; }
			set { base.BackgroundImageLayout = value; }
		}
		
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public new Image BackgroundImage {
			get { return base.BackgroundImage; }
			set { base.BackgroundImage = value; }
		}
		
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public new String ImageLocation {
			get { return base.ImageLocation; }
			set { base.ImageLocation = value; }
		}
		
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public new Image ErrorImage {
			get { return base.ErrorImage; }
			set { base.ErrorImage = value; }
		}
		
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public new Image InitialImage {
			get { return base.InitialImage; }
			set { base.InitialImage = value; }
		}
		
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public new bool WaitOnLoad {
			get { return base.WaitOnLoad; }
			set { base.WaitOnLoad = value; }
		}
		
		
		
		
	}
}
