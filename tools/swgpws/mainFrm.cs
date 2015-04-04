using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

using Aga.Controls.Tree;
using Aga.Controls.Tree.NodeControls;

namespace SWGPacket_Workshop
{
    public partial class mainFrm : Form
    {
        # region DllImports

        [DllImport("user32.dll", EntryPoint = "HideCaret")]
        public static extern long HideCaret(IntPtr hwnd);
        [DllImport("user32.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern bool CreateCaret(IntPtr hwnd, IntPtr hbmp, int width, int height);
        [DllImport("user32.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern bool ShowCaret(IntPtr hwnd);
        const int MAXFILENAMELEN = 26;
        [DllImport("user32.dll")]
        static extern IntPtr SendMessageW(IntPtr hWndControl, int msgId, IntPtr wParam, editWordBreakProc lParam);
        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        # endregion
        # region Variables

        delegate Int32 editWordBreakProc(String lpch, Int32 ichCurrrent, Int32 cch, Int32 code);

        Color[] nodeColor = new Color[] { Color.Black, Color.Blue, Color.Blue, Color.Purple, Color.Blue, Color.Blue, Color.Blue, Color.Blue, Color.Blue,
            Color.Green, Color.Green, Color.Green, Color.Green, Color.Orange, Color.Orange, Color.Orange, Color.Orange,
            Color.Brown, Color.Brown, Color.Brown, Color.Brown, Color.Blue, Color.Blue, Color.Blue, Color.Blue, 
            Color.Blue, Color.Blue, Color.Blue, Color.Blue, Color.Red, Color.Red };
        TreeView[]          treeViews;   // see mainFrm()
        List<TreeNode>      backingTree = new List<TreeNode>();
        TextBox[]           textBoxes;    // see mainFrm()
        List<PacketCapture> captures = new List<PacketCapture>();
        List<CacheItem>     editCache = new List<CacheItem>();
        CRCLibraryMgr       _lib = new CRCLibraryMgr();
        Int32               _lastPacketTab;
        editWordBreakProc   _editWordBreakProc;
        DatadefCompiler     _compiler;
        Aga.Controls.Tree.TreeViewAdv _analysisTreeView;

        # endregion

        # region Class constuctor

        public mainFrm()
        {
            InitializeComponent();
            indexFrom1ToolStripMenuItem.Checked = Properties.Settings.Default.index;
            ViewTabControl.SelectedIndex = Properties.Settings.Default.view;
            PacketTabControl.SelectedIndex = _lastPacketTab = Properties.Settings.Default.packet;
            treeViews = new TreeView[] { UDPTreeView, SOETreeView };
            for (int i = 0; i < treeViews.Length; i++)
                backingTree.Add(new TreeNode(treeViews[i].Name));

            textBoxes = new TextBox[] { HexText, DelimText };
            
            //searchMenu.Visible = false;
            searchMenu.Visible = Properties.Settings.Default.search;
            searchTypeCombo.SelectedIndex = Properties.Settings.Default.search_type;
            searchText.Text = Properties.Settings.Default.search_last;

            calcMenu.Visible = false;
            littleEndianToolStripMenuItem.Checked = Properties.Settings.Default.little_endian;
            sendToClipboardToolStripMenuItem.Checked = Properties.Settings.Default.send_clipboard;

            _editWordBreakProc = new editWordBreakProc(EditWordBreakProc);
            SendMessageW(ASCIIText.Handle, 0x00D0, IntPtr.Zero, _editWordBreakProc);

            _analysisTreeView = new TreeViewAdv();
            analysisTab.Controls.Add(_analysisTreeView);
            _analysisTreeView.Dock = DockStyle.Fill;
            ViewTabControl.TabPages.Remove(analysisTab);

            String[] types = new String[] { "Name", "Type", "Data", "Value" };
            Int32[] widths = new Int32[] { 200, 75, 125, 102 };
            for (Int32 i = 0; i < 4; i++)
            {
                NodeTextBox col = new NodeTextBox();
                _analysisTreeView.Columns.Add(new TreeColumn(types[i], widths[i]));
                col.ParentColumn = _analysisTreeView.Columns[i];
                col.DataPropertyName = types[i];
                _analysisTreeView.NodeControls.Add(col);
            }

            _analysisTreeView.BorderStyle = BorderStyle.FixedSingle;
            _analysisTreeView.UseColumns = true;
            _analysisTreeView.ContextMenuStrip = contextAnalysis;
            _analysisTreeView.SelectionMode = TreeSelectionMode.MultiSameParent;
        }

        # endregion
        # region Dynamic binding func

        private void loadCodeFiles()
        {
            _compiler = new DatadefCompiler(outputText);
            _compiler.Compile();
        }

        # endregion
        # region Helper func

        private void loadCaptureFile(string file)
        {
            PacketCapture packetCapture = new PacketCapture();
            packetCapture.Open(file);
            file = System.IO.Path.GetFileName(file);
            captures.Add(packetCapture);

            int indexStart = 0;
            if (Properties.Settings.Default.index) indexStart = 1;
            DateTime start = DateTime.Now;
            TreeNode UDPRoot = new TreeNode(file.Length > MAXFILENAMELEN ? file.Substring(0, MAXFILENAMELEN - 3) + "..." : file);
            TreeNode SOERoot = new TreeNode(file.Length > MAXFILENAMELEN ? file.Substring(0, MAXFILENAMELEN - 3) + "..." : file);

            string padLen = new string('0', packetCapture.Length.ToString().Length);
            
            for (int i = 0; i < packetCapture.Length; i++)
            {
                UInt16 opcode = packetCapture.GetSOEOpcode(i);

                String dir = packetCapture.IPAddress == 0 ? "?" : packetCapture.IsIncomming(i) ? "<" : ">";

                TreeNode UDPNode = new TreeNode(
                    string.Format("{0} {1:" + padLen + "} Len:{2}", dir, i + indexStart, packetCapture.GetUDPPDU(i).Length));
                UDPRoot.Nodes.Add(UDPNode);
                UDPNode.Tag = i;

                // Only add relevant SOE segments/data structures
                if (opcode > 0x001E) continue;
                
                TreeNode SOENode = new TreeNode(
                    string.Format("{0} {1:" + padLen + "} {2}", dir, i + indexStart, packetCapture.GetSOEMessageType(i)));
                SOERoot.Nodes.Add(SOENode);
                SOENode.Tag = i;
                SOENode.ForeColor = nodeColor[opcode];
            }
            backingTree[0].Nodes.Add((TreeNode)UDPRoot.Clone());
            backingTree[1].Nodes.Add((TreeNode)SOERoot.Clone());

            UDPTreeView.Nodes.Add(UDPRoot);
            SOETreeView.Nodes.Add(SOERoot);

            Console.WriteLine("{0} nodes created in {1:0.000} sec.", packetCapture.Length, (DateTime.Now - start).TotalSeconds);

            // General testing

        }
        private void packetTreeViewCollapse()
        {
            UDPTreeView.CollapseAll();
            SOETreeView.CollapseAll();
            /* Strapped out until SWG Packets are implemented
             * SWGTreeView.CollapseAll(); //*/
        }
        private TreeNode getRootNode(TreeNode node)
        {
            if (node == null) return null;
            TreeNode parent = node;
            for (int i = node.Level; i > 0; i--)
                parent = parent.Parent;
            DelimText.Enabled = true;
            return parent;
        }
        private TreeNode getCurrentNode() { return treeViews[PacketTabControl.SelectedIndex].SelectedNode; }
        private PacketCapture getCapture()
        {
            TreeNode node = (TreeNode)treeViews[PacketTabControl.SelectedIndex].SelectedNode;
            if (node == null) return null;
            while (node.Level > 0)
                node = (TreeNode)node.Parent;
            return captures[node.Index];
        }
        private void removeNodes(TreeNode parent, Predicate<TreeNode> func = null)
        {
            // Remove cache because it will be invalid
            editCache.Clear();

            // Clone node for performance, .Add() is far faster than .Remove()
            TreeNode clone = (TreeNode)parent.Clone();
            clone.Nodes.Clear();
            for (int i = 0; i < parent.Nodes.Count; i++)
                if (!func(parent.Nodes[i]))
                    clone.Nodes.Add((TreeNode)parent.Nodes[i].Clone());
            parent.TreeView.Nodes.Insert(parent.Index, clone);
            parent.Remove();
            if (parent.IsExpanded) clone.Expand();
        }
        private Boolean showWarning(String setting)
        {
            if (!(Boolean)Properties.Settings.Default[setting]) return true;
            warningFrm warn = new warningFrm(Properties.Resources.ResourceManager.GetString(setting));
            DialogResult result = warn.ShowDialog();
            Properties.Settings.Default[setting] = !warn.Suppressed;
            Properties.Settings.Default.Save();
            return result == System.Windows.Forms.DialogResult.OK;
        }
        private Int32 getSelectedIndex() { return (Int32)treeViews[PacketTabControl.SelectedIndex].SelectedNode.Tag; }
        private String delimitBytesToCB(String str)
        {
            if (!sendToClipboardToolStripMenuItem.Checked) return str;
            String data = "0x";
            for (int i = 0; i < str.Length; i++)
            {
                data += str[i];
                if (i % 2 != 0 && i < str.Length - 1) data += ", 0x";
            }
            Clipboard.SetData(System.Windows.Forms.DataFormats.Text, data);
            return data;
        }

        # endregion
        # region Packet rendering func

        private void updatePacketData()
        {
            TreeNode node = treeViews[PacketTabControl.SelectedIndex].SelectedNode;
            if (node == null || node.Parent == null)
            {
                AddrText.Text = HexText.Text = ASCIIText.Text = DelimText.Text = "";
                ViewTabControl.Enabled = calcMenu.Visible = false;
                //searchMenu.Visible SearchResult.Visible
                return;
            }
            int index = (int)node.Tag;
            PacketCapture capture = getCapture();

            switch (ViewTabControl.SelectedIndex)
            {
                case 0: // Hex/ASCII
                    renderHexASCII(capture);
                    break;
                case 1: // 0x00 Delim
                    renderDelim(capture);
                    break;
                case 2: // Analysis
                    renderAnalysis(capture);
                    break;
                default:
                    break;
            }
            String length;
            String source = capture.GetSourceAddr(index);
            String sourcePort = capture.GetSourcePort(index);
            String dest = capture.GetDestAddr(index);
            String destPort = capture.GetDestPort(index);
            Int32 indexStart = Properties.Settings.Default.index ? 1 : 0;
            switch (PacketTabControl.SelectedIndex)
            {
                case 0: // UDP
                    length = capture.GetUDPPDU(index).Length.ToString();
                    mainStatus.Text = String.Format("Index: {0}  |  Len: {1}  |  Src: {2}:{3}  |  Dst: {4}:{5}", index + (Properties.Settings.Default.index ? 1 : 0), length, source, sourcePort, dest, destPort);
                    break;
                case 1: // SOE
                    String crcSeed = BitConverter.ToString(BitConverter.GetBytes(capture.CRCSeed != 0 ? capture.CRCSeed : capture.GetSOECRCSeed(index)).Reverse<byte>().ToArray<byte>()).Replace("-", " ");
                    length = capture.GetSOEMessage(index).Length.ToString();
                    String encrypted = capture.IsEncrypted(index).ToString().Substring(0, 1);
                    String compressed = capture.IsCompressed(index).ToString().Substring(0, 1);
                    mainStatus.Text = String.Format("Index: {0}  |  Len: {1}  |  CRC: {2}  |  Encr: {3}  |  Comp: {4}  |  Src: {5}:{6}  |  Dst: {7}:{8}", index + indexStart, length, crcSeed, encrypted, compressed, source, sourcePort, dest, destPort);
                    break;
                case 2: // SWG
                    String sequence = "";
                    length = 0.ToString();
                    mainStatus.Text = string.Format("Index: {0}  |  Len: {1}  |  Seq: {2}", index + (Properties.Settings.Default.index ? 1 : 0), length, sequence);
                    break;
            }
            ViewTabControl.Enabled = true;
            searchMenu.Visible = Properties.Settings.Default.search;
            calcMenu.Visible = Properties.Settings.Default.calc;
        }
        private void renderHexASCII(PacketCapture capture)
        {
        	
        	//meh...
        	String[] x = new String[4];
        	
        	switch(PacketTabControl.SelectedIndex) {
        		case 0:
        			x = capture.returnHexASCII((int)UDPTreeView.SelectedNode.Tag, 0 , false);
        			break;
        		case 1:
        			x = capture.returnHexASCII((int)SOETreeView.SelectedNode.Tag, 1 , false);
        			break;
        		case 2:
        			return;
        	}
        	
 
        	AddrText.Text = x[0];
        	HexText.Text = x[1];
        	ASCIIText.Text = x[2];
        }

        private void renderDelim(PacketCapture capture)
        {
        	int index = ((PacketTabControl.SelectedIndex == 0 )? (Int32)UDPTreeView.SelectedNode.Tag : (Int32)SOETreeView.SelectedNode.Tag);
        	string packet = capture.ReturnDelim(index, PacketTabControl.SelectedIndex);
      	
            string text = getFromCache(capture, PacketTabControl.SelectedIndex, ViewTabControl.SelectedIndex, index);
            DelimText.Text = text != null ? text : packet;
        }
        private void renderAnalysis(PacketCapture capture)
        {
            Int32 index = (Int32)SOETreeView.SelectedNode.Tag;
            UInt16 opcode = capture.GetSOEOpcode(index);
            Int32 len = capture.GetSOEMessage(index).Length;
            Byte[] data;

            switch (opcode)
            {
                case 0x0001:
                case 0x0002:
                    data = capture.GetSOEMessage(index);
                    break;
                case 0x000D:
                    data = capture.GetSOEFragmentedMessage(index);
                    break;
                default:
                    data = capture.GetSOEMessage(index).Take<Byte>(len - 3).ToArray<Byte>();
                    break;
            }
            
            Object obj = _compiler.soeFactory(data);
            
            _analysisTreeView.BeginUpdate();
            if (obj != null)
                renderFields(obj);
            else
                _analysisTreeView.Model = new TreeModel();
            _analysisTreeView.EndUpdate();
        }
        private void renderFields(Object obj, DataNode parent = null, TreeModel model = null)
        {
            if (obj == null) return;
            if (model == null) model = new TreeModel();
            _analysisTreeView.Model = model;

            obj.GetType().GetFields();
            FieldInfo[] fields = obj.GetType().GetFields();
            for (Int32 i = 0; i < fields.Length; i++)
            {
                DataNode node = new DataNode();
                node.Name = fields[i].Name.Replace('_', ' ');
                node.Type = fields[i].FieldType.Name;

                node = renderFieldDataFormat(node, fields[i].GetValue(obj));

                if (parent == null)
                    model.Nodes.Add(node);
                else
                    parent.Nodes.Add(node);

                switch (node.Name)
                {
                    case "Message":
                        node.Type = "";
                        renderSubFields(new Byte[][] { ((Byte[])fields[i].GetValue(obj)) }, node, model);
                        break;
                    case "Messages":
                        renderSubFields((Byte[][])fields[i].GetValue(obj), node, model);
                        break;
                    case "Data":
                        if (((DataNode)(parent.Nodes[0])).Data == "0x68A75F0C")
                        {
                            node.Type = "Object";
                            node.Name = ((DataNode)(parent.Nodes[2])).Value + ((DataNode)(parent.Nodes[3])).Value;
                        }
                        renderFields(fields[i].GetValue(obj), node, model);
                        break;
                    case "OpCode":
                        node.Value = "";
                        break;
                    default:
                        if (node.Type == "Object array")
                        {
                            Object[] subObjs = (Object[])fields[i].GetValue(obj);
                            for (Int32 j = 0; j < subObjs.Length; j++)
                            {
                                DataNode parentNode = new DataNode();
                                parentNode.Name = subObjs[j].ToString();
                                node.Nodes.Add(parentNode);
                                renderFields(subObjs[j], parentNode, model);
                            }
                        }
                        break;
                }
            }
        }
        private void renderSubFields(Byte[][] fields, DataNode node, TreeModel model)
        {
            _analysisTreeView.FindNodeByTag(node).Expand();
            for (Int32 i = 0; i < fields.Length; i++)
            {
                DataNode parentNode = new DataNode();
                Object subObj;
                if ((subObj = _compiler.swgFactory(fields[i].Skip<Byte>(2).ToArray<Byte>())) != null ||
                    (subObj = _compiler.soeFactory(fields[i])) != null)
                {
                    parentNode.Name = subObj.ToString();
                    parentNode.Contents = fields[i];
                    node.Nodes.Add(parentNode);
                    renderFields(subObj, parentNode, model);
                }
                else
                {
                    if (fields[i].Length == 0) continue;
                    String opcode = String.Format("0x{0:X8}", BitConverter.ToUInt32(fields[i].Skip<Byte>(2).ToArray<Byte>(), 0));
                    String name = _lib.GetCommandName(opcode);
                    parentNode.Name = name != null ? name + " (undefined)" : "Unknown";
                    parentNode.Data = opcode;
                    parentNode.Contents = fields[i];
                    node.Nodes.Add(parentNode);
                }
            }
        }
        private DataNode renderFieldDataFormat(DataNode node, Object val)
        {
            Int32 length = 2;
            if (node.Type.IndexOf("Int") >= 0) length = Int32.Parse(node.Type.Substring(node.Type.Length - 2)) / 4;

            switch (node.Type)
            {
                case "UInt16":
                case "UInt32":
                case "UInt64":
                case "Int32":
                case "Byte":
                    node.Data = String.Format("0x{0:X" + length + "}", val);
                    node.Value = val.ToString();
                    break;
                case "Byte[]":
                    node.Data = "";
                    node.Value = "";
                    node.Type = "Byte array";
                    break;
                case "Byte[][]":
                    node.Data = "";
                    node.Value = ((Byte[][])val).Length.ToString();
                    node.Type = "";
                    break;
                case "Single":
                case "String":
                    node.Data = "";
                    node.Value = val == null ? "" : val.ToString();
                    break;
                default:
                    if (val.GetType().IsArray) node.Type = "Object array";
                    break;
            }
            return node;
        }

        # endregion
        # region Packet edit cache func
        private string getFromCache(PacketCapture capture, int type, int view, int index)
        {
            CacheItem cacheItem = getCache(capture, type, view, index);
            if (cacheItem != null)
                return cacheItem.Text;
            return null;
        }
        private CacheItem getCache(PacketCapture capture, int type, int view, int index) { return editCache.Find(x => x.PacketCapture == capture && x.Type == type && x.View == view && x.PacketIndex == index); }
        # endregion
        # region Menu event func

            # region File menu

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Multiselect = true;
            dlg.Filter = "Packet capture files (*.cap,*.pcap)|*.cap;*.pcap";
            DialogResult result = dlg.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.OK)
            {
                // open file
                foreach (string filename in dlg.FileNames)
                    loadCaptureFile(filename);
                PacketTabControl.Enabled = true;
            }
        }
        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TreeNode node = getRootNode(getCurrentNode());
            PacketCapture capture = captures[node.Index];
            
            // Remove from backingTree
            foreach (TreeNode bt in backingTree)
                bt.Nodes[node.Index].Remove();
            
            // Remove from cache
            foreach (CacheItem cacheItem in editCache.FindAll(x => x.PacketCapture == capture))
                editCache.Remove(cacheItem);

            // Remove capture
            captures.Remove(capture);

            int index = node.Index;

            foreach (TreeView treeView in treeViews)
                treeView.Nodes[index].Remove();
            if (UDPTreeView.Nodes.Count == 0)
                closeToolStripMenuItem.Visible = PacketTabControl.Enabled = false;
            updatePacketData();
        }
        private void exitToolStripMenuItem_Click(object sender, EventArgs e) { Close(); }
        
        #endregion
            # region Edit menu

        private void editToolStripMenuItem_DropDownOpening(object sender, EventArgs e)
        {
            //exportPacketToolStripMenuItem.Visible = toolStripMenuItem2.Visible = DelimText.Text != "" && ViewTabControl.SelectedIndex == 1;
            //FIXME: check if packet is selected
            exportPacketToolStripMenuItem.Visible = getCapture() == null ? false : true;
            enterAManualCRCSeedToolStripMenuItem.Visible = jumpToToolStripMenuItem.Visible = getCapture() == null ? false : true;
        }
        private void exportToolStripMenuItem_DropDownOpening(object sender, EventArgs e)
        {
        	hexExportToolStripMenuItem1.Enabled = HexText.Text != "" && ViewTabControl.SelectedIndex == 0;
        	asciiExportToolStripMenuItem.Enabled = ASCIIText.Text != "" && ViewTabControl.SelectedIndex == 0;
        	asciiHexExportToolStripMenuItem.Enabled = HexText.Text != "" && ASCIIText.Text != "" && ViewTabControl.SelectedIndex == 0;
        	x00DelimExportToolStripMenuItem.Enabled = DelimText.Text != "" && ViewTabControl.SelectedIndex == 1;
        }
        private void enterAManualCRCSeedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            manualCRCFrm crcDialog = new manualCRCFrm();
            Int32 crcSeed = getCapture().GetSOECRCSeed(getCapture().Length - 1);
            if (crcDialog.CRCSeed == 0) crcDialog.CRCSeed = crcSeed;
            crcDialog.ShowDialog();

            if (crcDialog.Guess)
                getCapture().CRCSeed = (Int32)getCapture().RecoverCRC();
            else
                getCapture().CRCSeed = crcDialog.CRCSeed;
            
            updatePacketData();
        }
        private void indexFrom1ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            indexFrom1ToolStripMenuItem.Checked = !indexFrom1ToolStripMenuItem.Checked;
            Properties.Settings.Default.index = indexFrom1ToolStripMenuItem.Checked;
            Properties.Settings.Default.Save();
        }
        private void resetAllWarningsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.warn_filter = true;
            Properties.Settings.Default.Save();
        }
         private void jumpToToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (getCapture() == null) return;
            jumpPanel.Visible = true;
            jumpText.Focus();
        }

            # endregion
            # region View menu
        
        private void viewToolStripMenuItem_DropDownOpening(object sender, EventArgs e)
        {
            filterToolStripMenuItem.Visible = searchToolStripMenuItem.Visible = toolStripMenuItem4.Visible = PacketTabControl.Enabled;
            searchToolStripMenuItem.Checked = Properties.Settings.Default.search;
            dataTypeCalcToolStripMenuItem.Checked = Properties.Settings.Default.calc;
        }
        
        private void showAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!showWarning("warn_filter")) return;
            for (int i = 0; i < treeViews.Length; i++)
            {
                treeViews[i].BeginUpdate();
                for (int j = 0; j < treeViews[i].Nodes.Count; j++)  // for each 'capture' node
                {
                    Boolean expandedNode = treeViews[i].Nodes[j].IsExpanded;
                    treeViews[i].Nodes.Insert(treeViews[i].Nodes[j].Index, (TreeNode)backingTree[i].Nodes[j].Clone());
                    treeViews[i].Nodes[j + 1].Remove();
                    if (expandedNode) treeViews[i].Nodes[j].Expand();
                }
                treeViews[i].EndUpdate();
            }

        }
        private void incommingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!showWarning("warn_filter")) return;
            foreach (TreeView treeView in treeViews)
                for (int i = 0; i < treeView.Nodes.Count; i++)
                    removeNodes(treeView.Nodes[i], x => x.Text.Substring(0, 1) == ">");
        }
        private void outgoingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!showWarning("warn_filter")) return;
            foreach (TreeView treeView in treeViews)
                for (int i = 0; i < treeView.Nodes.Count; i++)
                    removeNodes(treeView.Nodes[i], x => x.Text.Substring(0, 1) == "<");
        }
        private void searchToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.search = !Properties.Settings.Default.search;
            Properties.Settings.Default.Save();
            //searchMenu.Visible = ViewTabControl.Enabled && Properties.Settings.Default.search;
            searchMenu.Visible = Properties.Settings.Default.search;
        }
        private void hexConversionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.calc = !Properties.Settings.Default.calc;
            Properties.Settings.Default.Save();
            calcMenu.Visible = ViewTabControl.Enabled && Properties.Settings.Default.calc;
        }
        private void compilerOutputToolStripMenuItem_CheckStateChanged(object sender, EventArgs e)
        {
            if (compilerOutputToolStripMenuItem.Checked)
            {
                mainLayout.Visible = mainStatusStrip.Visible = false;
                mainLayout.Dock = DockStyle.None;
                outputText.Dock = DockStyle.Fill;
                outputText.Visible = true;
            }
            else
            {
                outputText.Visible = false;
                outputText.Dock = DockStyle.None;
                mainLayout.Dock = DockStyle.Fill;
                mainLayout.Visible = mainStatusStrip.Visible = true;
            }
        }
        private void compilerOutputToolStripMenuItem_Click(object sender, EventArgs e)
        {
            compilerOutputToolStripMenuItem.Checked = !compilerOutputToolStripMenuItem.Checked;
        }

            # endregion
            # region Help menu

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
            string product = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name;
            MessageBox.Show(product + " " + version, "About", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        # endregion
            # region Search menu

        private void searchText_TextChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.search_last = searchText.Text;
            Properties.Settings.Default.Save();
        }
        private void searchTypeCombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.search_type = searchTypeCombo.SelectedIndex;
            Properties.Settings.Default.Save();
        }
        private void search_Click(object sender, EventArgs e)
        {
            // Iterate nodes, not packets/segments, and perform search over each manually 
            searchText.Text = searchText.Text.Trim();
            search(((ToolStripMenuItem)sender).Text == "Prev");
        }
        private void searchFilter_Click(object sender, EventArgs e)
        {
            if (!showWarning("warn_filter")) return;
            
            if (captures.Count <= 0) { return; }
            
            /*
            if ((treeViews[1].SelectedNode != null) && (treeViews[1].SelectedNode.Level > 0)) {
            	treeViews[1].SelectedNode = getRootNode(treeViews[1].SelectedNode);
            }*/
            treeViews[1].BeginUpdate();
            
            foreach (TreeNode tn in treeViews[1].Nodes) {

	            switch (searchTypeCombo.SelectedItem.ToString())
	            {
	                case "ASCII":
	            		removeNodes(tn, x => captures[tn.Index].GetSOEMessage((Int32)x.Tag).ToLower().IndexOf(searchText.Text.ToLower()) == -1);
	                    break;
	                case "ASCII (CS)":
	                    removeNodes(tn, x => captures[tn.Index].GetSOEMessage((Int32)x.Tag).IndexOf(searchText.Text) == -1);
	                    break;
	                case "Hex":
	                    searchText.Text = searchText.Text.Replace(" ", "").Replace("-", "");
	                    removeNodes(tn, x => captures[tn.Index].GetSOEMessage((Int32)x.Tag).IndexOf(searchText.Text.GetBytesFromHex()) == -1);
	                    break;
	            }
	            
	            
            }
            
            treeViews[1].ExpandAll();

            treeViews[1].EndUpdate();
        }
        private void search(Boolean reverse)
        {
            SearchResult.Visible = true;
            searchText.Text = searchText.Text.Trim();
            if (searchTypeCombo.SelectedItem.ToString() == "Hex")
            {
                searchText.Text = searchText.Text.Replace(" ", "").Replace("-", "");
                if (searchText.Text.GetBytesFromHex() == null)
                {
                    SearchResult.Text = "Unable to parse hex";
                    return;
                }
            }
            Int32 pos = treeViews[PacketTabControl.SelectedIndex].SelectedNode.Index;
            TreeNode parent = treeViews[PacketTabControl.SelectedIndex].SelectedNode.Parent;

            if ((pos > 0 && reverse) || (pos < parent.Nodes.Count - 1 && !reverse))
            {
                SearchResult.Text = "Searching...";
                Int32 index = -1;
                Byte[] data = null;

                while (index == -1 && ((pos > 0 && reverse) || (pos < parent.Nodes.Count - 1 && !reverse)))
                {
                    pos += reverse ? -1 : 1;
                    switch (PacketTabControl.SelectedIndex)
                    {
                        case 0: // UDP
                            data = getCapture().GetUDPPDU((Int32)parent.Nodes[pos].Tag);
                            break;
                        case 1: // SOE
                            data = getCapture().GetSOEMessage((Int32)parent.Nodes[pos].Tag);
                            break;
                    }

                    switch (searchTypeCombo.SelectedItem.ToString())
                    {
                        case "ASCII":
                            index = data.ToLower().IndexOf(searchText.Text.ToLower());
                            break;
                        case "ASCII (CS)":
                            index = data.IndexOf(searchText.Text);
                            break;
                        case "Hex":
                            index = data.IndexOf(searchText.Text.GetBytesFromHex());
                            break;
                    }
                }
                SearchResult.Text = "Match not found";
                if (index > -1)
                {
                    SearchResult.Visible = false;
                    treeViews[PacketTabControl.SelectedIndex].SelectedNode = parent.Nodes[pos];
                    if (searchTypeCombo.SelectedItem.ToString() == "Hex")
                    {
                        String tempCriteria = BitConverter.ToString(searchText.Text.GetBytesFromHex()).Replace('-', ' ');
                        HexText.SelectionStart = HexText.Text.Replace('-', ' ').IndexOf(tempCriteria);
                        HexText.SelectionLength = tempCriteria.Length;
                        HexText.Focus();
                    }
                }
            }
        }

            # endregion
            # region Calc menu

        private void decimalText_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Control)
            {
                if (e.KeyCode == Keys.V)
                {
                    decimalText.Text = (String)Clipboard.GetData(System.Windows.Forms.DataFormats.Text);
                    decimalText.SelectionStart = decimalText.Text.Length;
                }
                else if (e.KeyCode == Keys.A)
                {
                    decimalText.SelectionStart = 0;
                    decimalText.SelectionLength = decimalText.Text.Length;
                }
            }

        }
        private void decimalText_Leave(object sender, EventArgs e)
        {
            Properties.Settings.Default.decimal_text = decimalText.Text;
            Properties.Settings.Default.Save();
        }
        private void decimalText_TextChanged(object sender, EventArgs e)
        {
            if (decimalText.Text.Trim() == "") return;
            Boolean hasOtherAlpha = decimalText.Text.ToLower().Count<Char>(x => x <= 0x2C || x == 0x2F || (x >= 0x3A && x <= 0x60) || x >= 0x67) > 0;
            Boolean hasHexAlpha = decimalText.Text.ToLower().Count<Char>(x => x >= 0x61 && x <= 0x66) > 0;
            Boolean hasDigit = decimalText.Text.ToLower().Count<Char>(x => x >= 0x30 && x <= 0x39 ) > 0;
            Boolean isDecimal = decimalText.Text.Count<Char>(x => x == '.') == 1 
                && !hasOtherAlpha 
                && !hasHexAlpha;
            Boolean isSigned = decimalText.Text.Trim()[0] == '-' && decimalText.Text.Count<Char>(x => x == '-') == 1;
            Boolean isHex = hasHexAlpha 
                && !hasOtherAlpha 
                && decimalText.Text.Count<Char>(x => x == '.' || x == '-') == 0;
            hasOtherAlpha = hasOtherAlpha 
                || decimalText.Text.Count<Char>(x => x == '.') > 1 
                || decimalText.Text.Skip<Char>(1).Count<Char>(x => x == '-') > 0 
                || (hasHexAlpha && decimalText.Text.Count<Char>(x => x == '.' || x == '-') > 0);
            
            if (hasOtherAlpha)
                hexToolStripMenuItem.Visible = int32ToolStripMenuItem.Visible = singleToolStripMenuItem.Visible = false;
            else {
                if (isHex)
                {
                    // HexAlpha present
                    hexToolStripMenuItem.Visible = false;
                    int32ToolStripMenuItem.Visible = decimalText.Text.Length <= 16;
                    if (decimalText.Text.Length > 8) int32ToolStripMenuItem.Text = "UInt64";
                    else if (decimalText.Text.Length > 4) int32ToolStripMenuItem.Text = "UInt32";
                    else int32ToolStripMenuItem.Text = "UInt16";
                    singleToolStripMenuItem.Visible = decimalText.Text.Length <= 8;
                }
                else if (isSigned || isDecimal)
                {
                    // Sign or decimal present
                    hexToolStripMenuItem.Visible = true;
                    int32ToolStripMenuItem.Visible = false;
                    singleToolStripMenuItem.Visible = false;
                }
                else
                {
                    // Treat as an integer
                    hexToolStripMenuItem.Visible = decimalText.Text.Length <= 10 && UInt64.Parse(decimalText.Text) <= UInt32.MaxValue;

                    // Treat as hex
                    int32ToolStripMenuItem.Visible = decimalText.Text.Length <= 16;
                    if (decimalText.Text.Length > 8) int32ToolStripMenuItem.Text = "UInt64";
                    else if (decimalText.Text.Length > 4) int32ToolStripMenuItem.Text = "UInt32";
                    else int32ToolStripMenuItem.Text = "UInt16";

                    singleToolStripMenuItem.Visible = decimalText.Text.Length <= 8;
                }
            }
        }
        private void hexToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // If no '.' or '-' then assume Int32
            Byte[] val;
            if (decimalText.Text.IndexOf('.') > -1 || decimalText.Text.IndexOf('-') > -1)
                val = BitConverter.GetBytes(Single.Parse(decimalText.Text));
            else
                val = BitConverter.GetBytes(UInt32.Parse(decimalText.Text));

            if (!littleEndianToolStripMenuItem.Checked)
                val.Reverse<Byte>();

            decimalText.Text = BitConverter.ToString(val).Replace("-", "");

            delimitBytesToCB(decimalText.Text);
        }
        private void int32ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (decimalText.Text.Length < 16) decimalText.Text += new String('0', 16 - decimalText.Text.Length);
            Byte[] array = Enumerable.Range(0, decimalText.Text.Length)
                     .Where(x => x % 2 == 0)
                     .Select(x => Convert.ToByte(decimalText.Text.Substring(x, 2), 16))
                     .ToArray();

            if (!littleEndianToolStripMenuItem.Checked)
                array.Reverse<Byte>();

            switch (int32ToolStripMenuItem.Text)
            {
                case "UInt64":
                    decimalText.Text = BitConverter.ToUInt64(array, 0).ToString();
                    break;
                case "UInt32":
                    decimalText.Text = BitConverter.ToUInt32(array, 0).ToString();
                    break;
                case "UInt16":
                    decimalText.Text = BitConverter.ToUInt16(array, 0).ToString();
                    break;
            }

            delimitBytesToCB(decimalText.Text);
        }
        private void singleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (decimalText.Text.Length < 8) decimalText.Text += new String('0', 8 - decimalText.Text.Length);
            Byte[] array = Enumerable.Range(0, decimalText.Text.Length)
                     .Where(x => x % 2 == 0)
                     .Select(x => Convert.ToByte(decimalText.Text.Substring(x, 2), 16))
                     .ToArray();

            if (!littleEndianToolStripMenuItem.Checked)
                array.Reverse<Byte>();
            
            decimalText.Text = BitConverter.ToSingle(array, 0).ToString();
        }
        private void crcToolStripMenuItem_Click(object sender, EventArgs e)
        {
            decimalText.Text = BitConverter.ToString(BitConverter.GetBytes(SWGLib.GenerateCRC(decimalText.Text.Trim()))).Replace("-", "");
            delimitBytesToCB(decimalText.Text);
        }
        private void calcOptions_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.little_endian = littleEndianToolStripMenuItem.Checked;
            Properties.Settings.Default.send_clipboard = sendToClipboardToolStripMenuItem.Checked;
            Properties.Settings.Default.Save();
        }

        # endregion
            # region Analysis context menu

        private void copyDataToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StringBuilder sb = new StringBuilder();
            foreach (TreeNodeAdv advnode in _analysisTreeView.SelectedNodes)
            {
                DataNode node = (DataNode)advnode.Tag;
                Int32 pos = 0;

                if (node.Contents.Length < 0xFF)
                    sb.AppendLine(String.Format("0x{0:X2}, # Length", node.Contents.Length));
                else
                    sb.AppendLine(string.Format("0x{0}, # Length", BitConverter.ToString(BitConverter.GetBytes(node.Contents.Length), 0, node.Contents.Length < 0xFFFF ? 2 : 4).Replace("-", ", 0x")));

                sb.AppendLine(String.Format("0x{0}, # Priority\r\n", BitConverter.ToString(node.Contents, pos, 2).Replace("-", ", 0x")));
                pos += 2;
                sb.AppendLine(String.Format("0x{0}{1} # {2}\r\n", BitConverter.ToString(node.Contents, pos, 4).Replace("-", ", 0x"), node.Contents.Length > 4 ? "," : "", node.Name));
                pos += 4;
                if (node.Contents.Length > pos) sb.AppendLine(String.Format("0x{0},\r\n\r\n", BitConverter.ToString(node.Contents, pos).Replace("-", ", 0x")));
            }
            Clipboard.SetText(sb.ToString());
            System.Diagnostics.Process notepad = System.Diagnostics.Process.Start("notepad");
            notepad.WaitForInputIdle();
            IntPtr p = notepad.MainWindowHandle;
            ShowWindow(p, 0x01);
            SendKeys.SendWait("^v");
        }
        private void contextAnalysis_Opening(object sender, CancelEventArgs e)
        {
            // Need to update code, do not display if SOE message type is selected
            try
            {
                if (_analysisTreeView.SelectedNode == null || !((DataNode)_analysisTreeView.SelectedNode.Parent.Tag).Name.Contains("Message"))
                    e.Cancel = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                e.Cancel = true;
            }
        }

            # endregion

        # endregion
        # region Treeview sync func

        private void synchronizeView(Int32 from)
        {
            TreeView[] treeViews = new TreeView[] { UDPTreeView, SOETreeView };
            searchFilter.Visible = from != 1;

            if (from == 1)
            {
                ViewTabControl.TabPages.Remove(analysisTab);
                if (SOETreeView.SelectedNode.Level > 0)
                    UDPTreeView.SelectedNode = UDPTreeView.Nodes[SOETreeView.SelectedNode.Parent.Index].Nodes[(Int32)SOETreeView.SelectedNode.Tag];
                else
                    UDPTreeView.SelectedNode = UDPTreeView.Nodes[SOETreeView.SelectedNode.Index];
                treeViews[0].Focus();
            }
            else
            {
                if (treeViews[0].SelectedNode.Level > 0)
                {
                    ViewTabControl.TabPages.Insert(2, analysisTab);

                    TreeNode parent = treeViews[1].Nodes[treeViews[0].SelectedNode.Parent.Index];
                    System.Collections.IEnumerator en = parent.Nodes.GetEnumerator();
                    while (en.MoveNext())
                        if ((Int32)((TreeNode)en.Current).Tag >= treeViews[0].SelectedNode.Index)
                        {
                            treeViews[1].SelectedNode = (TreeNode)en.Current;
                            goto exit;
                        }
                    treeViews[1].SelectedNode = parent.Nodes[parent.Nodes.Count - 1];
                }
                else
                {
                    treeViews[1].SelectedNode = treeViews[1].Nodes[treeViews[0].SelectedNode.Index];
                    ViewTabControl.TabPages.Remove(analysisTab);
                }
                treeViews[1].Focus();
            }
        exit:
            updatePacketData();
        }

        # endregion
        # region Misc control event func

        private void mainFrm_Shown(object sender, EventArgs e)
        {
            loadCodeFiles();
            if (Properties.Settings.Default.update)
            {
                MessageBox.Show("v. " +
                    System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString() + "\r\n\r\n" +
                    Properties.Resources.update_text.Replace("\t", "     "), "Update", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Properties.Settings.Default.update = false;
                Properties.Settings.Default.Save();
            }

            outputText.TextChanged += new System.EventHandler(outputText_TextChanged);
        }
        private void mainFrm_Resize(object sender, EventArgs e)
        {
            _analysisTreeView.Columns[3].Width = _analysisTreeView.Width - 420;
        }
        private void PacketTabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            treeViews[PacketTabControl.SelectedIndex].Focus();
            searchFilter.Visible = PacketTabControl.SelectedIndex == 1;
            if (treeViews[_lastPacketTab].SelectedNode == null)
                treeViews[_lastPacketTab].SelectedNode = treeViews[_lastPacketTab].Nodes[0];
            Properties.Settings.Default.packet = PacketTabControl.SelectedIndex;
            Properties.Settings.Default.Save();

            // Do syncronization here
            synchronizeView(_lastPacketTab);

            _lastPacketTab = PacketTabControl.SelectedIndex;
        }
        private void ViewTabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            treeViews[PacketTabControl.SelectedIndex].Focus();
            Properties.Settings.Default.view = ViewTabControl.SelectedIndex;
            Properties.Settings.Default.Save(); 
            updatePacketData();
        }
        private void TreeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (((TreeView)sender).Name == "SOETreeView")
            {
                searchFilter.Visible = ((TreeView)sender).SelectedNode.Level > 0;
                if (((TreeView)sender).SelectedNode.Level > 0)
                {
                    if (ViewTabControl.TabPages.Count == 2)
                        ViewTabControl.TabPages.Insert(2, analysisTab);
                }
                else
                {
                    if (ViewTabControl.TabPages.Count == 3)
                        ViewTabControl.TabPages.Remove(analysisTab);
                }
            }
            closeToolStripMenuItem.Visible = true;
            SearchResult.Visible = false;
            updatePacketData();
        }
        private void HexASCIIText_GotFocus(object sender, EventArgs e)
        {
            Bitmap mCaretBmp = new Bitmap(5, 5);
            Graphics gr = Graphics.FromImage(mCaretBmp);
            gr.Clear(Color.Black);
            CreateCaret(((TextBox)sender).Handle, mCaretBmp.GetHbitmap(), 5, 5);
            ShowCaret(((TextBox)sender).Handle);
            gr.Dispose();
        }
        private void DelimText_TextChanged(object sender, EventArgs e)
        {
            if (!DelimText.Enabled) return;

            TreeView treeView = treeViews[PacketTabControl.SelectedIndex];
            if (getRootNode(treeView.SelectedNode) == null || treeView.SelectedNode.Parent == null) return;
            PacketCapture capture = captures[getRootNode(treeView.SelectedNode).Index];
            int index = (int)treeView.SelectedNode.Tag;

            CacheItem cacheItem = getCache(capture, PacketTabControl.SelectedIndex, ViewTabControl.SelectedIndex, index);
            if (cacheItem == null)
                editCache.Add(new CacheItem(capture, PacketTabControl.SelectedIndex, ViewTabControl.SelectedIndex, index, DelimText.Text));
            else
                cacheItem.Text = DelimText.Text;
        }
        private void HexText_MouseUp(object sender, MouseEventArgs e)
        {
            String text = HexText.SelectedText.Replace(" ","").Replace("-","").Replace("\r\n","");
            if (text.Length % 2 == 1)
                text = text.Substring(0, text.Length - 1);
            if (text == "")
            {
                altStatus.Text = "";
                return;
            }
            Byte[] array = Enumerable.Range(0, text.Length)
                     .Where(x => x % 2 == 0)
                     .Select(x => Convert.ToByte(text.Substring(x, 2), 16))
                     .ToArray();
            if (array.Length > 7)
            {
                Double dbl = BitConverter.ToDouble(array, 0);
                if (dbl < 10000)
                    altStatus.Text = dbl.ToString("F4", System.Globalization.CultureInfo.InvariantCulture);
                else
                    altStatus.Text = dbl.ToString("0.####E0", System.Globalization.CultureInfo.InvariantCulture).Replace("E", " x10^");
            }
            else if (array.Length > 3)
                altStatus.Text = BitConverter.ToUInt32(array, 0).ToString();
            else if (array.Length > 1)
                altStatus.Text = BitConverter.ToUInt16(array, 0).ToString();
            else
                altStatus.Text = array[0].ToString();
        }
        private void outputText_TextChanged(object sender, EventArgs e)
        {
            compilerOutputToolStripMenuItem.Checked = true;
        }
        private void outputText_Enter(object sender, EventArgs e)
        {
            outputText.SelectionStart = outputText.TextLength;
            outputText.SelectionLength = 0;
        }
        private void jumpText_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 0x000D)
            {
                Int32 loc;
                Int32 pos = 0;
                if (Int32.TryParse(jumpText.Text, out loc))
                {
                    TreeNode node = treeViews[PacketTabControl.SelectedIndex].SelectedNode;
                    while (node.Level > 0)
                        node = node.Parent;
                    while (pos < node.Nodes.Count)
                        if (Convert.ToInt32(node.Nodes[pos++].Tag) >= loc)
                            break;
                    if (Convert.ToBoolean(Properties.Settings.Default["index"])) --pos;
                    treeViews[PacketTabControl.SelectedIndex].SelectedNode = node.Nodes[--pos];
                    jumpPanel.Visible = false;
                    jumpText.Text = "";
                }
            }
            else if (!char.IsNumber(e.KeyChar) && e.KeyChar != 0x0008)
                e.Handled = true;
        }
        private void jumpText_Leave(object sender, EventArgs e)
        {
            jumpPanel.Visible = false;
            jumpText.Text = "";
        }
        private Int32 EditWordBreakProc(String lpch, Int32 ichCurrrent, Int32 cch, Int32 code) { return ichCurrrent; }

        # endregion

        
        
        void ExportObjects() {
        	
        	Stack<BaselineObject> s = getCapture().GetBaselineObjects();
        	String st = "";
        	
        	while (s.Count > 0) {
        		BaselineObject b = s.Pop();
        		switch (b.Type) {
        			case "BUIO03":
        				SceneObject fc = getCapture().FindCoords(b.bid);
        				st+= b.Type + "\t" + b.ID + "\t" + b.Contents + "\n";
        				//FIXME: use the string formatter, luke
        				if (fc !=null) {
        					st+= "SceneObject" + "\t" + fc.Q1 + "\t" + fc.Q2 + "\t" + fc.Q3 + "\t" + fc.Q4 + "\t" + fc.X + "\t" + fc.Y + "\t" + fc.Z + "\t" + fc.CRC + "\n";
        				} else {
        					st+= "No SceneObject found\n";
        				}
        				break;
        			default:
        				st+= b.Type + "\t" + b.ID + "\n";
        				break;
        		}
        		
        	}
        	
        	ExportString(st);
        	
        }
                
        void ExportString(String text) {

            SaveFileDialog dlg = new SaveFileDialog();
            dlg.Filter = "Text files (*.txt)|*.txt";
            DialogResult result = dlg.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.OK) {
            	using (System.IO.StreamWriter outfile = new System.IO.StreamWriter(dlg.FileName)) {
            		outfile.Write(text);
            	}
            }
        }
        
        void AsciiExportToolStripMenuItemClick(object sender, EventArgs e)
        {
        	int index = ((PacketTabControl.SelectedIndex == 0) ? (Int32) UDPTreeView.SelectedNode.Tag : (Int32) SOETreeView.SelectedNode.Tag);
        	String[] x = getCapture().returnHexASCII(index, PacketTabControl.SelectedIndex , true);
        	ExportString(x[2]);
        }
        
        void HexExportToolStripMenuItem1Click(object sender, EventArgs e)
        {
        	int index = ((PacketTabControl.SelectedIndex == 0) ? (Int32) UDPTreeView.SelectedNode.Tag : (Int32) SOETreeView.SelectedNode.Tag);
        	String[] x = getCapture().returnHexASCII(index, PacketTabControl.SelectedIndex , true);
        	ExportString(x[1]);
        }
        
        void AsciiHexExportToolStripMenuItemClick(object sender, EventArgs e)
        {
        	int index = ((PacketTabControl.SelectedIndex == 0) ? (Int32) UDPTreeView.SelectedNode.Tag : (Int32) SOETreeView.SelectedNode.Tag);
        	String[] x = getCapture().returnHexASCII(index, PacketTabControl.SelectedIndex, true);
        	ExportString(x[3]);
        	
        }
        
        void X00DelimExportToolStripMenuItemClick(object sender, EventArgs e)
        {
        	int index = ((PacketTabControl.SelectedIndex == 0) ? (Int32) UDPTreeView.SelectedNode.Tag : (Int32) SOETreeView.SelectedNode.Tag);        	
        	String x = getCapture().ReturnDelim(index, PacketTabControl.SelectedIndex);
        	ExportString(x);
        }
        
        void ExportWholeCaptureToolStripMenuItemDropDownOpening(object sender, EventArgs e)
        {
        	int selection = PacketTabControl.SelectedIndex;
        	asciiWExportToolStripMenuItem.Enabled = hexWExportToolStripMenuItem1.Enabled =
        		asciiHexWExportToolStripMenuItem.Enabled = x00DelimWExportToolStripMenuItem.Enabled =
        		treeViews.Length >= 2 && treeViews[selection].SelectedNode != null && treeViews[PacketTabControl.SelectedIndex].SelectedNode.Level >= 0;
        	
        }
    
        
        

        
        void HexWExportToolStripMenuItem1Click(object sender, EventArgs e)
        {
        	ExportString(getCapture().GetAllPackages(PacketTabControl.SelectedIndex,1));
        }
        
        void AsciiWExportToolStripMenuItemClick(object sender, EventArgs e)
        {
        	ExportString(getCapture().GetAllPackages(PacketTabControl.SelectedIndex,2));
        }
        
        void AsciiHexWExportToolStripMenuItemClick(object sender, EventArgs e)
        {
        	ExportString(getCapture().GetAllPackages(PacketTabControl.SelectedIndex,3));
        }
        
        void X00DelimWExportToolStripMenuItemClick(object sender, EventArgs e)
        {
        	ExportString(getCapture().GetAllPackages(PacketTabControl.SelectedIndex,4));
        }
        
        // cable request 2013-01-23
        void DelimTextKeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
        {
        	if (e.KeyChar == '\x1') {
        		((TextBox) sender).SelectAll();
        		e.Handled = true;
        	}
        }
        
        void ExportObjectsToolStripMenuItemClick(object sender, EventArgs e)
        {
        	if (getCapture() == null) { return; }
        	ExportObjects();
        }
    }
    public class CacheItem
    {
        public PacketCapture    PacketCapture;
        public int              Type;
        public int              View;
        public int              PacketIndex;
        public string           Text;

        public CacheItem(PacketCapture pcap, int type, int view, int index, string text)
        {
            PacketCapture = pcap;
            Type = type;
            View = view;
            PacketIndex = index;
            Text = text;
        }
    }
    public class DataNode : Node
    {
        public String Name;
        public String Type;
        public String Data;
        public String Value;
        public Byte[] Contents;
    }
}
