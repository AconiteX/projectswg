using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SWGPacket_Workshop
{
    public partial class manualCRCFrm : Form
    {
        Int32 _crcSeed;
        Boolean _guess = false;

        public Boolean Guess
        {
            get { return _guess; }
        }

        public manualCRCFrm()
        {
            InitializeComponent();
        }

        public Int32 CRCSeed
        {
            get { return _crcSeed; }
            set
            {
                _crcSeed = value;
                crcText.Text = string.Format("0x{0:X8}", _crcSeed);
            }
        }
        public String CRCSeedText
        {
            get { return crcText.Text.ToUpper().Trim(); }
        }

        private void okBtn_Click(object sender, EventArgs e)
        {
            if (crcText.Text.ToLower().Trim() == "guess")
                _guess = true;
            else
                Int32.TryParse(crcText.Text.Replace("0x", ""), System.Globalization.NumberStyles.HexNumber, System.Globalization.CultureInfo.InvariantCulture, out _crcSeed);
            
            Close();
        }
        private void clearBtn_Click(object sender, EventArgs e)
        {
            CRCSeed = 0;
        }
    }
}
