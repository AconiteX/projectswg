namespace SWGPacket_Workshop
{
    partial class mainFrm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
        	this.components = new System.ComponentModel.Container();
        	this.mainLayout = new System.Windows.Forms.TableLayoutPanel();
        	this.ViewTabControl = new System.Windows.Forms.TabControl();
        	this.HexASCIITab = new System.Windows.Forms.TabPage();
        	this.HexViewLayout = new System.Windows.Forms.Panel();
        	this.HexASCIIViewLayout = new System.Windows.Forms.TableLayoutPanel();
        	this.AddrText = new System.Windows.Forms.TextBox();
        	this.HexText = new System.Windows.Forms.TextBox();
        	this.ASCIIText = new System.Windows.Forms.TextBox();
        	this.vScrollBar1 = new System.Windows.Forms.VScrollBar();
        	this.searchMenu = new System.Windows.Forms.MenuStrip();
        	this.searchText = new System.Windows.Forms.ToolStripTextBox();
        	this.searchTypeCombo = new System.Windows.Forms.ToolStripComboBox();
        	this.searchPrev = new System.Windows.Forms.ToolStripMenuItem();
        	this.searchNext = new System.Windows.Forms.ToolStripMenuItem();
        	this.searchFilter = new System.Windows.Forms.ToolStripMenuItem();
        	this.SearchResult = new System.Windows.Forms.ToolStripMenuItem();
        	this.DelimTab = new System.Windows.Forms.TabPage();
        	this.DelimText = new System.Windows.Forms.TextBox();
        	this.calcMenu = new System.Windows.Forms.MenuStrip();
        	this.decimalText = new System.Windows.Forms.ToolStripTextBox();
        	this.hexToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        	this.int32ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        	this.singleToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        	this.crcToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        	this.calcOptionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        	this.littleEndianToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        	this.sendToClipboardToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        	this.analysisTab = new System.Windows.Forms.TabPage();
        	this.PacketTabControl = new System.Windows.Forms.TabControl();
        	this.UDPTab = new System.Windows.Forms.TabPage();
        	this.UDPTreeView = new System.Windows.Forms.TreeView();
        	this.SOETab = new System.Windows.Forms.TabPage();
        	this.SOETreeView = new System.Windows.Forms.TreeView();
        	this.jumpPanel = new System.Windows.Forms.Panel();
        	this.jumpText = new System.Windows.Forms.TextBox();
        	this.mainMenu = new System.Windows.Forms.MenuStrip();
        	this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        	this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        	this.closeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        	this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
        	this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        	this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        	this.enterAManualCRCSeedToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        	this.indexFrom1ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        	this.resetAllWarningsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        	this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
        	this.exportPacketToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        	this.asciiExportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        	this.hexExportToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
        	this.asciiHexExportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        	this.x00DelimExportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        	this.exportWholeCaptureToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        	this.asciiWExportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        	this.hexWExportToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
        	this.asciiHexWExportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        	this.x00DelimWExportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        	this.jumpToToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        	this.viewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        	this.filterToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        	this.showAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        	this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripSeparator();
        	this.incommingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        	this.outgoingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        	this.searchToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        	this.toolStripMenuItem4 = new System.Windows.Forms.ToolStripSeparator();
        	this.dataTypeCalcToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        	this.compilerOutputToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        	this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        	this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        	this.mainStatusStrip = new System.Windows.Forms.StatusStrip();
        	this.mainStatus = new System.Windows.Forms.ToolStripStatusLabel();
        	this.altStatus = new System.Windows.Forms.ToolStripStatusLabel();
        	this.outputText = new System.Windows.Forms.TextBox();
        	this.contextAnalysis = new System.Windows.Forms.ContextMenuStrip(this.components);
        	this.copyDataToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        	this.exportObjectsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        	this.mainLayout.SuspendLayout();
        	this.ViewTabControl.SuspendLayout();
        	this.HexASCIITab.SuspendLayout();
        	this.HexViewLayout.SuspendLayout();
        	this.HexASCIIViewLayout.SuspendLayout();
        	this.searchMenu.SuspendLayout();
        	this.DelimTab.SuspendLayout();
        	this.calcMenu.SuspendLayout();
        	this.PacketTabControl.SuspendLayout();
        	this.UDPTab.SuspendLayout();
        	this.SOETab.SuspendLayout();
        	this.jumpPanel.SuspendLayout();
        	this.mainMenu.SuspendLayout();
        	this.mainStatusStrip.SuspendLayout();
        	this.contextAnalysis.SuspendLayout();
        	this.SuspendLayout();
        	// 
        	// mainLayout
        	// 
        	this.mainLayout.ColumnCount = 2;
        	this.mainLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 226F));
        	this.mainLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 536F));
        	this.mainLayout.Controls.Add(this.ViewTabControl, 1, 0);
        	this.mainLayout.Controls.Add(this.PacketTabControl, 0, 0);
        	this.mainLayout.Dock = System.Windows.Forms.DockStyle.Fill;
        	this.mainLayout.Location = new System.Drawing.Point(0, 24);
        	this.mainLayout.Name = "mainLayout";
        	this.mainLayout.RowCount = 1;
        	this.mainLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
        	this.mainLayout.Size = new System.Drawing.Size(762, 413);
        	this.mainLayout.TabIndex = 4;
        	// 
        	// ViewTabControl
        	// 
        	this.ViewTabControl.Appearance = System.Windows.Forms.TabAppearance.FlatButtons;
        	this.ViewTabControl.Controls.Add(this.HexASCIITab);
        	this.ViewTabControl.Controls.Add(this.DelimTab);
        	this.ViewTabControl.Controls.Add(this.analysisTab);
        	this.ViewTabControl.Dock = System.Windows.Forms.DockStyle.Fill;
        	this.ViewTabControl.Enabled = false;
        	this.ViewTabControl.Font = new System.Drawing.Font("Calibri", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        	this.ViewTabControl.Location = new System.Drawing.Point(226, 0);
        	this.ViewTabControl.Margin = new System.Windows.Forms.Padding(0, 0, 2, 0);
        	this.ViewTabControl.Name = "ViewTabControl";
        	this.ViewTabControl.Padding = new System.Drawing.Point(0, 0);
        	this.ViewTabControl.SelectedIndex = 0;
        	this.ViewTabControl.Size = new System.Drawing.Size(534, 413);
        	this.ViewTabControl.TabIndex = 0;
        	this.ViewTabControl.SelectedIndexChanged += new System.EventHandler(this.ViewTabControl_SelectedIndexChanged);
        	// 
        	// HexASCIITab
        	// 
        	this.HexASCIITab.Controls.Add(this.HexViewLayout);
        	this.HexASCIITab.Controls.Add(this.searchMenu);
        	this.HexASCIITab.Location = new System.Drawing.Point(4, 25);
        	this.HexASCIITab.Margin = new System.Windows.Forms.Padding(0);
        	this.HexASCIITab.Name = "HexASCIITab";
        	this.HexASCIITab.Size = new System.Drawing.Size(526, 384);
        	this.HexASCIITab.TabIndex = 0;
        	this.HexASCIITab.Text = "Hex/ASCII";
        	this.HexASCIITab.UseVisualStyleBackColor = true;
        	// 
        	// HexViewLayout
        	// 
        	this.HexViewLayout.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
        	this.HexViewLayout.Controls.Add(this.HexASCIIViewLayout);
        	this.HexViewLayout.Controls.Add(this.vScrollBar1);
        	this.HexViewLayout.Dock = System.Windows.Forms.DockStyle.Fill;
        	this.HexViewLayout.Location = new System.Drawing.Point(0, 0);
        	this.HexViewLayout.Name = "HexViewLayout";
        	this.HexViewLayout.Size = new System.Drawing.Size(526, 384);
        	this.HexViewLayout.TabIndex = 0;
        	// 
        	// HexASCIIViewLayout
        	// 
        	this.HexASCIIViewLayout.ColumnCount = 5;
        	this.HexASCIIViewLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 38F));
        	this.HexASCIIViewLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 8F));
        	this.HexASCIIViewLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 332F));
        	this.HexASCIIViewLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
        	this.HexASCIIViewLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 116F));
        	this.HexASCIIViewLayout.Controls.Add(this.AddrText, 0, 0);
        	this.HexASCIIViewLayout.Controls.Add(this.HexText, 2, 0);
        	this.HexASCIIViewLayout.Controls.Add(this.ASCIIText, 4, 0);
        	this.HexASCIIViewLayout.Dock = System.Windows.Forms.DockStyle.Fill;
        	this.HexASCIIViewLayout.Location = new System.Drawing.Point(0, 0);
        	this.HexASCIIViewLayout.Margin = new System.Windows.Forms.Padding(0);
        	this.HexASCIIViewLayout.Name = "HexASCIIViewLayout";
        	this.HexASCIIViewLayout.RowCount = 1;
        	this.HexASCIIViewLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
        	this.HexASCIIViewLayout.Size = new System.Drawing.Size(508, 382);
        	this.HexASCIIViewLayout.TabIndex = 1;
        	// 
        	// AddrText
        	// 
        	this.AddrText.BorderStyle = System.Windows.Forms.BorderStyle.None;
        	this.AddrText.Dock = System.Windows.Forms.DockStyle.Fill;
        	this.AddrText.Font = new System.Drawing.Font("Lucida Console", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        	this.AddrText.Location = new System.Drawing.Point(1, 1);
        	this.AddrText.Margin = new System.Windows.Forms.Padding(1);
        	this.AddrText.Multiline = true;
        	this.AddrText.Name = "AddrText";
        	this.AddrText.ReadOnly = true;
        	this.AddrText.Size = new System.Drawing.Size(36, 380);
        	this.AddrText.TabIndex = 2;
        	this.AddrText.GotFocus += new System.EventHandler(this.HexASCIIText_GotFocus);
        	// 
        	// HexText
        	// 
        	this.HexText.BorderStyle = System.Windows.Forms.BorderStyle.None;
        	this.HexText.Dock = System.Windows.Forms.DockStyle.Fill;
        	this.HexText.Font = new System.Drawing.Font("Lucida Console", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        	this.HexText.Location = new System.Drawing.Point(47, 1);
        	this.HexText.Margin = new System.Windows.Forms.Padding(1);
        	this.HexText.Multiline = true;
        	this.HexText.Name = "HexText";
        	this.HexText.ReadOnly = true;
        	this.HexText.Size = new System.Drawing.Size(330, 380);
        	this.HexText.TabIndex = 0;
        	this.HexText.GotFocus += new System.EventHandler(this.HexASCIIText_GotFocus);
        	this.HexText.MouseUp += new System.Windows.Forms.MouseEventHandler(this.HexText_MouseUp);
        	// 
        	// ASCIIText
        	// 
        	this.ASCIIText.BorderStyle = System.Windows.Forms.BorderStyle.None;
        	this.ASCIIText.Dock = System.Windows.Forms.DockStyle.Fill;
        	this.ASCIIText.Font = new System.Drawing.Font("Lucida Console", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        	this.ASCIIText.Location = new System.Drawing.Point(393, 1);
        	this.ASCIIText.Margin = new System.Windows.Forms.Padding(1);
        	this.ASCIIText.Multiline = true;
        	this.ASCIIText.Name = "ASCIIText";
        	this.ASCIIText.ReadOnly = true;
        	this.ASCIIText.Size = new System.Drawing.Size(114, 380);
        	this.ASCIIText.TabIndex = 1;
        	this.ASCIIText.GotFocus += new System.EventHandler(this.HexASCIIText_GotFocus);
        	// 
        	// vScrollBar1
        	// 
        	this.vScrollBar1.Dock = System.Windows.Forms.DockStyle.Right;
        	this.vScrollBar1.Enabled = false;
        	this.vScrollBar1.LargeChange = 0;
        	this.vScrollBar1.Location = new System.Drawing.Point(508, 0);
        	this.vScrollBar1.Maximum = 0;
        	this.vScrollBar1.Name = "vScrollBar1";
        	this.vScrollBar1.Size = new System.Drawing.Size(16, 382);
        	this.vScrollBar1.SmallChange = 0;
        	this.vScrollBar1.TabIndex = 0;
        	// 
        	// searchMenu
        	// 
        	this.searchMenu.BackColor = System.Drawing.SystemColors.Control;
        	this.searchMenu.Font = new System.Drawing.Font("Calibri", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        	this.searchMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
        	        	        	this.searchText,
        	        	        	this.searchTypeCombo,
        	        	        	this.searchPrev,
        	        	        	this.searchNext,
        	        	        	this.searchFilter,
        	        	        	this.SearchResult});
        	this.searchMenu.Location = new System.Drawing.Point(0, 0);
        	this.searchMenu.Name = "searchMenu";
        	this.searchMenu.Size = new System.Drawing.Size(526, 25);
        	this.searchMenu.TabIndex = 1;
        	this.searchMenu.Text = "menuStrip2";
        	this.searchMenu.Visible = false;
        	// 
        	// searchText
        	// 
        	this.searchText.Name = "searchText";
        	this.searchText.Size = new System.Drawing.Size(225, 21);
        	this.searchText.TextChanged += new System.EventHandler(this.searchText_TextChanged);
        	// 
        	// searchTypeCombo
        	// 
        	this.searchTypeCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
        	this.searchTypeCombo.Items.AddRange(new object[] {
        	        	        	"ASCII",
        	        	        	"ASCII (CS)",
        	        	        	"Hex"});
        	this.searchTypeCombo.Name = "searchTypeCombo";
        	this.searchTypeCombo.Size = new System.Drawing.Size(75, 21);
        	this.searchTypeCombo.SelectedIndexChanged += new System.EventHandler(this.searchTypeCombo_SelectedIndexChanged);
        	// 
        	// searchPrev
        	// 
        	this.searchPrev.Name = "searchPrev";
        	this.searchPrev.Size = new System.Drawing.Size(40, 21);
        	this.searchPrev.Text = "Prev";
        	this.searchPrev.Click += new System.EventHandler(this.search_Click);
        	// 
        	// searchNext
        	// 
        	this.searchNext.Name = "searchNext";
        	this.searchNext.Size = new System.Drawing.Size(41, 21);
        	this.searchNext.Text = "Next";
        	this.searchNext.Click += new System.EventHandler(this.search_Click);
        	// 
        	// searchFilter
        	// 
        	this.searchFilter.Name = "searchFilter";
        	this.searchFilter.Size = new System.Drawing.Size(44, 21);
        	this.searchFilter.Text = "Filter";
        	this.searchFilter.Click += new System.EventHandler(this.searchFilter_Click);
        	// 
        	// SearchResult
        	// 
        	this.SearchResult.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
        	this.SearchResult.BackColor = System.Drawing.SystemColors.Control;
        	this.SearchResult.Enabled = false;
        	this.SearchResult.ForeColor = System.Drawing.SystemColors.ControlText;
        	this.SearchResult.Name = "SearchResult";
        	this.SearchResult.Size = new System.Drawing.Size(65, 21);
        	this.SearchResult.Text = "Not found";
        	this.SearchResult.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
        	this.SearchResult.Visible = false;
        	// 
        	// DelimTab
        	// 
        	this.DelimTab.Controls.Add(this.DelimText);
        	this.DelimTab.Controls.Add(this.calcMenu);
        	this.DelimTab.Location = new System.Drawing.Point(4, 25);
        	this.DelimTab.Margin = new System.Windows.Forms.Padding(0);
        	this.DelimTab.Name = "DelimTab";
        	this.DelimTab.Size = new System.Drawing.Size(526, 384);
        	this.DelimTab.TabIndex = 1;
        	this.DelimTab.Text = "0x00 Delim";
        	this.DelimTab.UseVisualStyleBackColor = true;
        	// 
        	// DelimText
        	// 
        	this.DelimText.Dock = System.Windows.Forms.DockStyle.Fill;
        	this.DelimText.Font = new System.Drawing.Font("Lucida Console", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        	this.DelimText.Location = new System.Drawing.Point(0, 0);
        	this.DelimText.Multiline = true;
        	this.DelimText.Name = "DelimText";
        	this.DelimText.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
        	this.DelimText.Size = new System.Drawing.Size(526, 384);
        	this.DelimText.TabIndex = 3;
        	this.DelimText.TextChanged += new System.EventHandler(this.DelimText_TextChanged);
        	this.DelimText.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.DelimTextKeyPress);
        	// 
        	// calcMenu
        	// 
        	this.calcMenu.BackColor = System.Drawing.SystemColors.Control;
        	this.calcMenu.Font = new System.Drawing.Font("Calibri", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        	this.calcMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
        	        	        	this.decimalText,
        	        	        	this.hexToolStripMenuItem,
        	        	        	this.int32ToolStripMenuItem,
        	        	        	this.singleToolStripMenuItem,
        	        	        	this.crcToolStripMenuItem,
        	        	        	this.calcOptionToolStripMenuItem});
        	this.calcMenu.Location = new System.Drawing.Point(0, 0);
        	this.calcMenu.Name = "calcMenu";
        	this.calcMenu.Size = new System.Drawing.Size(526, 25);
        	this.calcMenu.TabIndex = 4;
        	this.calcMenu.Visible = false;
        	// 
        	// decimalText
        	// 
        	this.decimalText.MaxLength = 256;
        	this.decimalText.Name = "decimalText";
        	this.decimalText.ShortcutsEnabled = false;
        	this.decimalText.Size = new System.Drawing.Size(250, 21);
        	this.decimalText.Leave += new System.EventHandler(this.decimalText_Leave);
        	this.decimalText.KeyUp += new System.Windows.Forms.KeyEventHandler(this.decimalText_KeyUp);
        	this.decimalText.TextChanged += new System.EventHandler(this.decimalText_TextChanged);
        	// 
        	// hexToolStripMenuItem
        	// 
        	this.hexToolStripMenuItem.Name = "hexToolStripMenuItem";
        	this.hexToolStripMenuItem.Size = new System.Drawing.Size(37, 21);
        	this.hexToolStripMenuItem.Text = "Hex";
        	this.hexToolStripMenuItem.Visible = false;
        	this.hexToolStripMenuItem.Click += new System.EventHandler(this.hexToolStripMenuItem_Click);
        	// 
        	// int32ToolStripMenuItem
        	// 
        	this.int32ToolStripMenuItem.Name = "int32ToolStripMenuItem";
        	this.int32ToolStripMenuItem.Size = new System.Drawing.Size(51, 21);
        	this.int32ToolStripMenuItem.Text = "UInt32";
        	this.int32ToolStripMenuItem.Visible = false;
        	this.int32ToolStripMenuItem.Click += new System.EventHandler(this.int32ToolStripMenuItem_Click);
        	// 
        	// singleToolStripMenuItem
        	// 
        	this.singleToolStripMenuItem.Name = "singleToolStripMenuItem";
        	this.singleToolStripMenuItem.Size = new System.Drawing.Size(47, 21);
        	this.singleToolStripMenuItem.Text = "Single";
        	this.singleToolStripMenuItem.Visible = false;
        	this.singleToolStripMenuItem.Click += new System.EventHandler(this.singleToolStripMenuItem_Click);
        	// 
        	// crcToolStripMenuItem
        	// 
        	this.crcToolStripMenuItem.Name = "crcToolStripMenuItem";
        	this.crcToolStripMenuItem.Size = new System.Drawing.Size(37, 21);
        	this.crcToolStripMenuItem.Text = "CRC";
        	this.crcToolStripMenuItem.Click += new System.EventHandler(this.crcToolStripMenuItem_Click);
        	// 
        	// calcOptionToolStripMenuItem
        	// 
        	this.calcOptionToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
        	        	        	this.littleEndianToolStripMenuItem,
        	        	        	this.sendToClipboardToolStripMenuItem});
        	this.calcOptionToolStripMenuItem.Name = "calcOptionToolStripMenuItem";
        	this.calcOptionToolStripMenuItem.Size = new System.Drawing.Size(55, 21);
        	this.calcOptionToolStripMenuItem.Text = "Options";
        	// 
        	// littleEndianToolStripMenuItem
        	// 
        	this.littleEndianToolStripMenuItem.Checked = true;
        	this.littleEndianToolStripMenuItem.CheckOnClick = true;
        	this.littleEndianToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
        	this.littleEndianToolStripMenuItem.Name = "littleEndianToolStripMenuItem";
        	this.littleEndianToolStripMenuItem.Size = new System.Drawing.Size(156, 22);
        	this.littleEndianToolStripMenuItem.Text = "Little endian";
        	this.littleEndianToolStripMenuItem.Click += new System.EventHandler(this.calcOptions_Click);
        	// 
        	// sendToClipboardToolStripMenuItem
        	// 
        	this.sendToClipboardToolStripMenuItem.Checked = true;
        	this.sendToClipboardToolStripMenuItem.CheckOnClick = true;
        	this.sendToClipboardToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
        	this.sendToClipboardToolStripMenuItem.Name = "sendToClipboardToolStripMenuItem";
        	this.sendToClipboardToolStripMenuItem.Size = new System.Drawing.Size(156, 22);
        	this.sendToClipboardToolStripMenuItem.Text = "Send to clipboard";
        	this.sendToClipboardToolStripMenuItem.Click += new System.EventHandler(this.calcOptions_Click);
        	// 
        	// analysisTab
        	// 
        	this.analysisTab.Location = new System.Drawing.Point(4, 25);
        	this.analysisTab.Margin = new System.Windows.Forms.Padding(0);
        	this.analysisTab.Name = "analysisTab";
        	this.analysisTab.Size = new System.Drawing.Size(526, 384);
        	this.analysisTab.TabIndex = 2;
        	this.analysisTab.Text = "Analysis";
        	this.analysisTab.UseVisualStyleBackColor = true;
        	// 
        	// PacketTabControl
        	// 
        	this.PacketTabControl.Appearance = System.Windows.Forms.TabAppearance.FlatButtons;
        	this.PacketTabControl.Controls.Add(this.UDPTab);
        	this.PacketTabControl.Controls.Add(this.SOETab);
        	this.PacketTabControl.Dock = System.Windows.Forms.DockStyle.Fill;
        	this.PacketTabControl.Enabled = false;
        	this.PacketTabControl.Font = new System.Drawing.Font("Calibri", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        	this.PacketTabControl.Location = new System.Drawing.Point(0, 0);
        	this.PacketTabControl.Margin = new System.Windows.Forms.Padding(0);
        	this.PacketTabControl.Multiline = true;
        	this.PacketTabControl.Name = "PacketTabControl";
        	this.PacketTabControl.Padding = new System.Drawing.Point(0, 0);
        	this.PacketTabControl.SelectedIndex = 0;
        	this.PacketTabControl.Size = new System.Drawing.Size(226, 413);
        	this.PacketTabControl.TabIndex = 7;
        	this.PacketTabControl.SelectedIndexChanged += new System.EventHandler(this.PacketTabControl_SelectedIndexChanged);
        	// 
        	// UDPTab
        	// 
        	this.UDPTab.Controls.Add(this.UDPTreeView);
        	this.UDPTab.Location = new System.Drawing.Point(4, 25);
        	this.UDPTab.Margin = new System.Windows.Forms.Padding(0);
        	this.UDPTab.Name = "UDPTab";
        	this.UDPTab.Size = new System.Drawing.Size(218, 384);
        	this.UDPTab.TabIndex = 0;
        	this.UDPTab.Text = "UDP";
        	this.UDPTab.UseVisualStyleBackColor = true;
        	// 
        	// UDPTreeView
        	// 
        	this.UDPTreeView.Dock = System.Windows.Forms.DockStyle.Fill;
        	this.UDPTreeView.Font = new System.Drawing.Font("Lucida Console", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        	this.UDPTreeView.FullRowSelect = true;
        	this.UDPTreeView.HideSelection = false;
        	this.UDPTreeView.Indent = 12;
        	this.UDPTreeView.ItemHeight = 11;
        	this.UDPTreeView.Location = new System.Drawing.Point(0, 0);
        	this.UDPTreeView.Margin = new System.Windows.Forms.Padding(1);
        	this.UDPTreeView.Name = "UDPTreeView";
        	this.UDPTreeView.ShowLines = false;
        	this.UDPTreeView.ShowPlusMinus = false;
        	this.UDPTreeView.ShowRootLines = false;
        	this.UDPTreeView.Size = new System.Drawing.Size(218, 384);
        	this.UDPTreeView.TabIndex = 0;
        	this.UDPTreeView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.TreeView_AfterSelect);
        	// 
        	// SOETab
        	// 
        	this.SOETab.Controls.Add(this.SOETreeView);
        	this.SOETab.Location = new System.Drawing.Point(4, 25);
        	this.SOETab.Margin = new System.Windows.Forms.Padding(0);
        	this.SOETab.Name = "SOETab";
        	this.SOETab.Size = new System.Drawing.Size(218, 384);
        	this.SOETab.TabIndex = 1;
        	this.SOETab.Text = "SOE";
        	this.SOETab.UseVisualStyleBackColor = true;
        	// 
        	// SOETreeView
        	// 
        	this.SOETreeView.Dock = System.Windows.Forms.DockStyle.Fill;
        	this.SOETreeView.Font = new System.Drawing.Font("Lucida Console", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        	this.SOETreeView.FullRowSelect = true;
        	this.SOETreeView.HideSelection = false;
        	this.SOETreeView.Indent = 12;
        	this.SOETreeView.ItemHeight = 11;
        	this.SOETreeView.Location = new System.Drawing.Point(0, 0);
        	this.SOETreeView.Margin = new System.Windows.Forms.Padding(1);
        	this.SOETreeView.Name = "SOETreeView";
        	this.SOETreeView.ShowLines = false;
        	this.SOETreeView.ShowPlusMinus = false;
        	this.SOETreeView.ShowRootLines = false;
        	this.SOETreeView.Size = new System.Drawing.Size(218, 384);
        	this.SOETreeView.TabIndex = 2;
        	this.SOETreeView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.TreeView_AfterSelect);
        	// 
        	// jumpPanel
        	// 
        	this.jumpPanel.Controls.Add(this.jumpText);
        	this.jumpPanel.Location = new System.Drawing.Point(18, 60);
        	this.jumpPanel.Margin = new System.Windows.Forms.Padding(4);
        	this.jumpPanel.Name = "jumpPanel";
        	this.jumpPanel.Padding = new System.Windows.Forms.Padding(8);
        	this.jumpPanel.Size = new System.Drawing.Size(88, 36);
        	this.jumpPanel.TabIndex = 1;
        	this.jumpPanel.Visible = false;
        	// 
        	// jumpText
        	// 
        	this.jumpText.Dock = System.Windows.Forms.DockStyle.Fill;
        	this.jumpText.Location = new System.Drawing.Point(8, 8);
        	this.jumpText.Name = "jumpText";
        	this.jumpText.Size = new System.Drawing.Size(72, 21);
        	this.jumpText.TabIndex = 0;
        	this.jumpText.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
        	this.jumpText.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.jumpText_KeyPress);
        	this.jumpText.Leave += new System.EventHandler(this.jumpText_Leave);
        	// 
        	// mainMenu
        	// 
        	this.mainMenu.BackColor = System.Drawing.SystemColors.Control;
        	this.mainMenu.Font = new System.Drawing.Font("Calibri", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        	this.mainMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
        	        	        	this.fileToolStripMenuItem,
        	        	        	this.editToolStripMenuItem,
        	        	        	this.viewToolStripMenuItem,
        	        	        	this.helpToolStripMenuItem});
        	this.mainMenu.Location = new System.Drawing.Point(0, 0);
        	this.mainMenu.Name = "mainMenu";
        	this.mainMenu.Size = new System.Drawing.Size(762, 24);
        	this.mainMenu.TabIndex = 5;
        	this.mainMenu.Text = "menuStrip1";
        	// 
        	// fileToolStripMenuItem
        	// 
        	this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
        	        	        	this.openToolStripMenuItem,
        	        	        	this.closeToolStripMenuItem,
        	        	        	this.toolStripMenuItem1,
        	        	        	this.exitToolStripMenuItem});
        	this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
        	this.fileToolStripMenuItem.Size = new System.Drawing.Size(36, 20);
        	this.fileToolStripMenuItem.Text = "&File";
        	// 
        	// openToolStripMenuItem
        	// 
        	this.openToolStripMenuItem.Name = "openToolStripMenuItem";
        	this.openToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
        	this.openToolStripMenuItem.Size = new System.Drawing.Size(144, 22);
        	this.openToolStripMenuItem.Text = "&Open...";
        	this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
        	// 
        	// closeToolStripMenuItem
        	// 
        	this.closeToolStripMenuItem.Name = "closeToolStripMenuItem";
        	this.closeToolStripMenuItem.Size = new System.Drawing.Size(144, 22);
        	this.closeToolStripMenuItem.Text = "Close";
        	this.closeToolStripMenuItem.Visible = false;
        	this.closeToolStripMenuItem.Click += new System.EventHandler(this.closeToolStripMenuItem_Click);
        	// 
        	// toolStripMenuItem1
        	// 
        	this.toolStripMenuItem1.Name = "toolStripMenuItem1";
        	this.toolStripMenuItem1.Size = new System.Drawing.Size(141, 6);
        	// 
        	// exitToolStripMenuItem
        	// 
        	this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
        	this.exitToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.F4)));
        	this.exitToolStripMenuItem.Size = new System.Drawing.Size(144, 22);
        	this.exitToolStripMenuItem.Text = "E&xit";
        	this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
        	// 
        	// editToolStripMenuItem
        	// 
        	this.editToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
        	        	        	this.enterAManualCRCSeedToolStripMenuItem,
        	        	        	this.indexFrom1ToolStripMenuItem,
        	        	        	this.resetAllWarningsToolStripMenuItem,
        	        	        	this.toolStripMenuItem2,
        	        	        	this.exportPacketToolStripMenuItem,
        	        	        	this.exportWholeCaptureToolStripMenuItem,
        	        	        	this.exportObjectsToolStripMenuItem,
        	        	        	this.jumpToToolStripMenuItem});
        	this.editToolStripMenuItem.Name = "editToolStripMenuItem";
        	this.editToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
        	this.editToolStripMenuItem.Text = "&Edit";
        	this.editToolStripMenuItem.DropDownOpening += new System.EventHandler(this.editToolStripMenuItem_DropDownOpening);
        	// 
        	// enterAManualCRCSeedToolStripMenuItem
        	// 
        	this.enterAManualCRCSeedToolStripMenuItem.Name = "enterAManualCRCSeedToolStripMenuItem";
        	this.enterAManualCRCSeedToolStripMenuItem.Size = new System.Drawing.Size(199, 22);
        	this.enterAManualCRCSeedToolStripMenuItem.Text = "Enter a manual CRC seed...";
        	this.enterAManualCRCSeedToolStripMenuItem.Click += new System.EventHandler(this.enterAManualCRCSeedToolStripMenuItem_Click);
        	// 
        	// indexFrom1ToolStripMenuItem
        	// 
        	this.indexFrom1ToolStripMenuItem.Name = "indexFrom1ToolStripMenuItem";
        	this.indexFrom1ToolStripMenuItem.Size = new System.Drawing.Size(199, 22);
        	this.indexFrom1ToolStripMenuItem.Text = "1-Based Indexing";
        	this.indexFrom1ToolStripMenuItem.Click += new System.EventHandler(this.indexFrom1ToolStripMenuItem_Click);
        	// 
        	// resetAllWarningsToolStripMenuItem
        	// 
        	this.resetAllWarningsToolStripMenuItem.Name = "resetAllWarningsToolStripMenuItem";
        	this.resetAllWarningsToolStripMenuItem.Size = new System.Drawing.Size(199, 22);
        	this.resetAllWarningsToolStripMenuItem.Text = "&Reset all warnings";
        	this.resetAllWarningsToolStripMenuItem.Click += new System.EventHandler(this.resetAllWarningsToolStripMenuItem_Click);
        	// 
        	// toolStripMenuItem2
        	// 
        	this.toolStripMenuItem2.Name = "toolStripMenuItem2";
        	this.toolStripMenuItem2.Size = new System.Drawing.Size(196, 6);
        	// 
        	// exportPacketToolStripMenuItem
        	// 
        	this.exportPacketToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
        	        	        	this.asciiExportToolStripMenuItem,
        	        	        	this.hexExportToolStripMenuItem1,
        	        	        	this.asciiHexExportToolStripMenuItem,
        	        	        	this.x00DelimExportToolStripMenuItem});
        	this.exportPacketToolStripMenuItem.Name = "exportPacketToolStripMenuItem";
        	this.exportPacketToolStripMenuItem.Size = new System.Drawing.Size(199, 22);
        	this.exportPacketToolStripMenuItem.Text = "&Export packet...";
        	this.exportPacketToolStripMenuItem.DropDownOpening += new System.EventHandler(this.exportToolStripMenuItem_DropDownOpening);
        	this.exportPacketToolStripMenuItem.Click += new System.EventHandler(this.jumpText_Leave);
        	// 
        	// asciiExportToolStripMenuItem
        	// 
        	this.asciiExportToolStripMenuItem.Name = "asciiExportToolStripMenuItem";
        	this.asciiExportToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
        	this.asciiExportToolStripMenuItem.Text = "Ascii";
        	this.asciiExportToolStripMenuItem.Click += new System.EventHandler(this.AsciiExportToolStripMenuItemClick);
        	// 
        	// hexExportToolStripMenuItem1
        	// 
        	this.hexExportToolStripMenuItem1.Name = "hexExportToolStripMenuItem1";
        	this.hexExportToolStripMenuItem1.Size = new System.Drawing.Size(152, 22);
        	this.hexExportToolStripMenuItem1.Text = "Hex";
        	this.hexExportToolStripMenuItem1.Click += new System.EventHandler(this.HexExportToolStripMenuItem1Click);
        	// 
        	// asciiHexExportToolStripMenuItem
        	// 
        	this.asciiHexExportToolStripMenuItem.Name = "asciiHexExportToolStripMenuItem";
        	this.asciiHexExportToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
        	this.asciiHexExportToolStripMenuItem.Text = "Ascii + Hex";
        	this.asciiHexExportToolStripMenuItem.Click += new System.EventHandler(this.AsciiHexExportToolStripMenuItemClick);
        	// 
        	// x00DelimExportToolStripMenuItem
        	// 
        	this.x00DelimExportToolStripMenuItem.Name = "x00DelimExportToolStripMenuItem";
        	this.x00DelimExportToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
        	this.x00DelimExportToolStripMenuItem.Text = "0x00 Delim";
        	this.x00DelimExportToolStripMenuItem.Click += new System.EventHandler(this.X00DelimExportToolStripMenuItemClick);
        	// 
        	// exportWholeCaptureToolStripMenuItem
        	// 
        	this.exportWholeCaptureToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
        	        	        	this.asciiWExportToolStripMenuItem,
        	        	        	this.hexWExportToolStripMenuItem1,
        	        	        	this.asciiHexWExportToolStripMenuItem,
        	        	        	this.x00DelimWExportToolStripMenuItem});
        	this.exportWholeCaptureToolStripMenuItem.Name = "exportWholeCaptureToolStripMenuItem";
        	this.exportWholeCaptureToolStripMenuItem.Size = new System.Drawing.Size(199, 22);
        	this.exportWholeCaptureToolStripMenuItem.Text = "Export whole capture";
        	this.exportWholeCaptureToolStripMenuItem.DropDownOpening += new System.EventHandler(this.ExportWholeCaptureToolStripMenuItemDropDownOpening);
        	// 
        	// asciiWExportToolStripMenuItem
        	// 
        	this.asciiWExportToolStripMenuItem.Name = "asciiWExportToolStripMenuItem";
        	this.asciiWExportToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
        	this.asciiWExportToolStripMenuItem.Text = "Ascii";
        	this.asciiWExportToolStripMenuItem.Click += new System.EventHandler(this.AsciiWExportToolStripMenuItemClick);
        	// 
        	// hexWExportToolStripMenuItem1
        	// 
        	this.hexWExportToolStripMenuItem1.Name = "hexWExportToolStripMenuItem1";
        	this.hexWExportToolStripMenuItem1.Size = new System.Drawing.Size(152, 22);
        	this.hexWExportToolStripMenuItem1.Text = "Hex";
        	this.hexWExportToolStripMenuItem1.Click += new System.EventHandler(this.HexWExportToolStripMenuItem1Click);
        	// 
        	// asciiHexWExportToolStripMenuItem
        	// 
        	this.asciiHexWExportToolStripMenuItem.Name = "asciiHexWExportToolStripMenuItem";
        	this.asciiHexWExportToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
        	this.asciiHexWExportToolStripMenuItem.Text = "Ascii + Hex";
        	this.asciiHexWExportToolStripMenuItem.Click += new System.EventHandler(this.AsciiHexWExportToolStripMenuItemClick);
        	// 
        	// x00DelimWExportToolStripMenuItem
        	// 
        	this.x00DelimWExportToolStripMenuItem.Name = "x00DelimWExportToolStripMenuItem";
        	this.x00DelimWExportToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
        	this.x00DelimWExportToolStripMenuItem.Text = "0x00 Delim";
        	this.x00DelimWExportToolStripMenuItem.Click += new System.EventHandler(this.X00DelimWExportToolStripMenuItemClick);
        	// 
        	// jumpToToolStripMenuItem
        	// 
        	this.jumpToToolStripMenuItem.Name = "jumpToToolStripMenuItem";
        	this.jumpToToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.J)));
        	this.jumpToToolStripMenuItem.Size = new System.Drawing.Size(199, 22);
        	this.jumpToToolStripMenuItem.Text = "Jump to...";
        	this.jumpToToolStripMenuItem.Click += new System.EventHandler(this.jumpToToolStripMenuItem_Click);
        	// 
        	// viewToolStripMenuItem
        	// 
        	this.viewToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
        	        	        	this.filterToolStripMenuItem,
        	        	        	this.searchToolStripMenuItem,
        	        	        	this.toolStripMenuItem4,
        	        	        	this.dataTypeCalcToolStripMenuItem,
        	        	        	this.compilerOutputToolStripMenuItem});
        	this.viewToolStripMenuItem.Name = "viewToolStripMenuItem";
        	this.viewToolStripMenuItem.Size = new System.Drawing.Size(42, 20);
        	this.viewToolStripMenuItem.Text = "&View";
        	this.viewToolStripMenuItem.DropDownOpening += new System.EventHandler(this.viewToolStripMenuItem_DropDownOpening);
        	// 
        	// filterToolStripMenuItem
        	// 
        	this.filterToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
        	        	        	this.showAllToolStripMenuItem,
        	        	        	this.toolStripMenuItem3,
        	        	        	this.incommingToolStripMenuItem,
        	        	        	this.outgoingToolStripMenuItem});
        	this.filterToolStripMenuItem.Name = "filterToolStripMenuItem";
        	this.filterToolStripMenuItem.Size = new System.Drawing.Size(170, 22);
        	this.filterToolStripMenuItem.Text = "&Filter";
        	this.filterToolStripMenuItem.Visible = false;
        	// 
        	// showAllToolStripMenuItem
        	// 
        	this.showAllToolStripMenuItem.Name = "showAllToolStripMenuItem";
        	this.showAllToolStripMenuItem.Size = new System.Drawing.Size(126, 22);
        	this.showAllToolStripMenuItem.Text = "Show &All";
        	this.showAllToolStripMenuItem.Click += new System.EventHandler(this.showAllToolStripMenuItem_Click);
        	// 
        	// toolStripMenuItem3
        	// 
        	this.toolStripMenuItem3.Name = "toolStripMenuItem3";
        	this.toolStripMenuItem3.Size = new System.Drawing.Size(123, 6);
        	// 
        	// incommingToolStripMenuItem
        	// 
        	this.incommingToolStripMenuItem.Name = "incommingToolStripMenuItem";
        	this.incommingToolStripMenuItem.Size = new System.Drawing.Size(126, 22);
        	this.incommingToolStripMenuItem.Text = "&Incomming";
        	this.incommingToolStripMenuItem.Click += new System.EventHandler(this.incommingToolStripMenuItem_Click);
        	// 
        	// outgoingToolStripMenuItem
        	// 
        	this.outgoingToolStripMenuItem.Name = "outgoingToolStripMenuItem";
        	this.outgoingToolStripMenuItem.Size = new System.Drawing.Size(126, 22);
        	this.outgoingToolStripMenuItem.Text = "&Outgoing";
        	this.outgoingToolStripMenuItem.Click += new System.EventHandler(this.outgoingToolStripMenuItem_Click);
        	// 
        	// searchToolStripMenuItem
        	// 
        	this.searchToolStripMenuItem.Name = "searchToolStripMenuItem";
        	this.searchToolStripMenuItem.Size = new System.Drawing.Size(170, 22);
        	this.searchToolStripMenuItem.Text = "&Search";
        	this.searchToolStripMenuItem.Visible = false;
        	this.searchToolStripMenuItem.Click += new System.EventHandler(this.searchToolStripMenuItem_Click);
        	// 
        	// toolStripMenuItem4
        	// 
        	this.toolStripMenuItem4.Name = "toolStripMenuItem4";
        	this.toolStripMenuItem4.Size = new System.Drawing.Size(167, 6);
        	this.toolStripMenuItem4.Visible = false;
        	// 
        	// dataTypeCalcToolStripMenuItem
        	// 
        	this.dataTypeCalcToolStripMenuItem.Name = "dataTypeCalcToolStripMenuItem";
        	this.dataTypeCalcToolStripMenuItem.Size = new System.Drawing.Size(170, 22);
        	this.dataTypeCalcToolStripMenuItem.Text = "Data type calculator";
        	this.dataTypeCalcToolStripMenuItem.Visible = false;
        	this.dataTypeCalcToolStripMenuItem.Click += new System.EventHandler(this.hexConversionToolStripMenuItem_Click);
        	// 
        	// compilerOutputToolStripMenuItem
        	// 
        	this.compilerOutputToolStripMenuItem.Name = "compilerOutputToolStripMenuItem";
        	this.compilerOutputToolStripMenuItem.Size = new System.Drawing.Size(170, 22);
        	this.compilerOutputToolStripMenuItem.Text = "Compiler output";
        	this.compilerOutputToolStripMenuItem.CheckStateChanged += new System.EventHandler(this.compilerOutputToolStripMenuItem_CheckStateChanged);
        	this.compilerOutputToolStripMenuItem.Click += new System.EventHandler(this.compilerOutputToolStripMenuItem_Click);
        	// 
        	// helpToolStripMenuItem
        	// 
        	this.helpToolStripMenuItem.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
        	this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
        	        	        	this.aboutToolStripMenuItem});
        	this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
        	this.helpToolStripMenuItem.Size = new System.Drawing.Size(41, 20);
        	this.helpToolStripMenuItem.Text = "&Help";
        	// 
        	// aboutToolStripMenuItem
        	// 
        	this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
        	this.aboutToolStripMenuItem.Size = new System.Drawing.Size(111, 22);
        	this.aboutToolStripMenuItem.Text = "&About...";
        	this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
        	// 
        	// mainStatusStrip
        	// 
        	this.mainStatusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
        	        	        	this.mainStatus,
        	        	        	this.altStatus});
        	this.mainStatusStrip.Location = new System.Drawing.Point(0, 437);
        	this.mainStatusStrip.Name = "mainStatusStrip";
        	this.mainStatusStrip.Size = new System.Drawing.Size(762, 22);
        	this.mainStatusStrip.TabIndex = 6;
        	// 
        	// mainStatus
        	// 
        	this.mainStatus.AutoSize = false;
        	this.mainStatus.Font = new System.Drawing.Font("Calibri", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        	this.mainStatus.Name = "mainStatus";
        	this.mainStatus.Size = new System.Drawing.Size(627, 17);
        	this.mainStatus.Spring = true;
        	this.mainStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
        	// 
        	// altStatus
        	// 
        	this.altStatus.AutoSize = false;
        	this.altStatus.Font = new System.Drawing.Font("Calibri", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        	this.altStatus.Name = "altStatus";
        	this.altStatus.Size = new System.Drawing.Size(120, 17);
        	this.altStatus.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
        	// 
        	// outputText
        	// 
        	this.outputText.BackColor = System.Drawing.Color.Black;
        	this.outputText.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
        	this.outputText.Location = new System.Drawing.Point(240, 59);
        	this.outputText.Multiline = true;
        	this.outputText.Name = "outputText";
        	this.outputText.ReadOnly = true;
        	this.outputText.Size = new System.Drawing.Size(204, 98);
        	this.outputText.TabIndex = 7;
        	this.outputText.Visible = false;
        	this.outputText.Enter += new System.EventHandler(this.outputText_Enter);
        	// 
        	// contextAnalysis
        	// 
        	this.contextAnalysis.Font = new System.Drawing.Font("Calibri", 8.25F);
        	this.contextAnalysis.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
        	        	        	this.copyDataToolStripMenuItem});
        	this.contextAnalysis.Name = "contextAnalysis";
        	this.contextAnalysis.Size = new System.Drawing.Size(98, 26);
        	this.contextAnalysis.Opening += new System.ComponentModel.CancelEventHandler(this.contextAnalysis_Opening);
        	// 
        	// copyDataToolStripMenuItem
        	// 
        	this.copyDataToolStripMenuItem.Name = "copyDataToolStripMenuItem";
        	this.copyDataToolStripMenuItem.Size = new System.Drawing.Size(97, 22);
        	this.copyDataToolStripMenuItem.Text = "Copy";
        	this.copyDataToolStripMenuItem.Click += new System.EventHandler(this.copyDataToolStripMenuItem_Click);
        	// 
        	// exportObjectsToolStripMenuItem
        	// 
        	this.exportObjectsToolStripMenuItem.Name = "exportObjectsToolStripMenuItem";
        	this.exportObjectsToolStripMenuItem.Size = new System.Drawing.Size(199, 22);
        	this.exportObjectsToolStripMenuItem.Text = "Export Objects";
        	this.exportObjectsToolStripMenuItem.Click += new System.EventHandler(this.ExportObjectsToolStripMenuItemClick);
        	// 
        	// mainFrm
        	// 
        	this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
        	this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        	this.ClientSize = new System.Drawing.Size(762, 459);
        	this.Controls.Add(this.jumpPanel);
        	this.Controls.Add(this.outputText);
        	this.Controls.Add(this.mainLayout);
        	this.Controls.Add(this.mainStatusStrip);
        	this.Controls.Add(this.mainMenu);
        	this.DoubleBuffered = true;
        	this.Font = new System.Drawing.Font("Calibri", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        	this.KeyPreview = true;
        	this.MainMenuStrip = this.mainMenu;
        	this.MinimumSize = new System.Drawing.Size(770, 486);
        	this.Name = "mainFrm";
        	this.Text = "SPWS";
        	this.Shown += new System.EventHandler(this.mainFrm_Shown);
        	this.Resize += new System.EventHandler(this.mainFrm_Resize);
        	this.mainLayout.ResumeLayout(false);
        	this.ViewTabControl.ResumeLayout(false);
        	this.HexASCIITab.ResumeLayout(false);
        	this.HexASCIITab.PerformLayout();
        	this.HexViewLayout.ResumeLayout(false);
        	this.HexASCIIViewLayout.ResumeLayout(false);
        	this.HexASCIIViewLayout.PerformLayout();
        	this.searchMenu.ResumeLayout(false);
        	this.searchMenu.PerformLayout();
        	this.DelimTab.ResumeLayout(false);
        	this.DelimTab.PerformLayout();
        	this.calcMenu.ResumeLayout(false);
        	this.calcMenu.PerformLayout();
        	this.PacketTabControl.ResumeLayout(false);
        	this.UDPTab.ResumeLayout(false);
        	this.SOETab.ResumeLayout(false);
        	this.jumpPanel.ResumeLayout(false);
        	this.jumpPanel.PerformLayout();
        	this.mainMenu.ResumeLayout(false);
        	this.mainMenu.PerformLayout();
        	this.mainStatusStrip.ResumeLayout(false);
        	this.mainStatusStrip.PerformLayout();
        	this.contextAnalysis.ResumeLayout(false);
        	this.ResumeLayout(false);
        	this.PerformLayout();
        }
        private System.Windows.Forms.ToolStripMenuItem exportObjectsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem x00DelimWExportToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem asciiHexWExportToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem hexWExportToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem asciiWExportToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exportWholeCaptureToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem x00DelimExportToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem asciiHexExportToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem hexExportToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem asciiExportToolStripMenuItem;

        #endregion

        // private System.Windows.Forms.TreeView SOETreeView;
        private System.Windows.Forms.TreeView SOETreeView;
        private System.Windows.Forms.TableLayoutPanel mainLayout;
        private System.Windows.Forms.MenuStrip mainMenu;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.StatusStrip mainStatusStrip;
        private System.Windows.Forms.ToolStripStatusLabel mainStatus;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem indexFrom1ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem closeToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem exportPacketToolStripMenuItem;
        private System.Windows.Forms.TabControl ViewTabControl;
        private System.Windows.Forms.TabPage HexASCIITab;
        private System.Windows.Forms.TabControl PacketTabControl;
        private System.Windows.Forms.TabPage UDPTab;
        private System.Windows.Forms.TabPage SOETab;
        private System.Windows.Forms.Panel HexViewLayout;
        private System.Windows.Forms.TableLayoutPanel HexASCIIViewLayout;
        private System.Windows.Forms.TextBox HexText;
        private System.Windows.Forms.TextBox ASCIIText;
        private System.Windows.Forms.VScrollBar vScrollBar1;
        private System.Windows.Forms.TreeView UDPTreeView;
        private System.Windows.Forms.ToolStripMenuItem enterAManualCRCSeedToolStripMenuItem;
        private System.Windows.Forms.TextBox AddrText;
        private System.Windows.Forms.ToolStripStatusLabel altStatus;
        private System.Windows.Forms.ToolStripMenuItem viewToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem filterToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem showAllToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem3;
        private System.Windows.Forms.ToolStripMenuItem incommingToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem outgoingToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem resetAllWarningsToolStripMenuItem;
        private System.Windows.Forms.MenuStrip searchMenu;
        private System.Windows.Forms.ToolStripTextBox searchText;
        private System.Windows.Forms.ToolStripComboBox searchTypeCombo;
        private System.Windows.Forms.ToolStripMenuItem searchNext;
        private System.Windows.Forms.ToolStripMenuItem searchToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem searchPrev;
        private System.Windows.Forms.ToolStripMenuItem SearchResult;
        private System.Windows.Forms.TabPage DelimTab;
        private System.Windows.Forms.TextBox DelimText;
        private System.Windows.Forms.MenuStrip calcMenu;
        private System.Windows.Forms.ToolStripTextBox decimalText;
        private System.Windows.Forms.ToolStripMenuItem hexToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem4;
        private System.Windows.Forms.ToolStripMenuItem dataTypeCalcToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem int32ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem singleToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem crcToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem calcOptionToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem littleEndianToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem sendToClipboardToolStripMenuItem;
        private System.Windows.Forms.TabPage analysisTab;
        private System.Windows.Forms.ToolStripMenuItem compilerOutputToolStripMenuItem;
        private System.Windows.Forms.TextBox outputText;
        private System.Windows.Forms.ToolStripMenuItem searchFilter;
        private System.Windows.Forms.ContextMenuStrip contextAnalysis;
        private System.Windows.Forms.ToolStripMenuItem copyDataToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem jumpToToolStripMenuItem;
        private System.Windows.Forms.Panel jumpPanel;
        private System.Windows.Forms.TextBox jumpText;

        

    }
}

