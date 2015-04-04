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
    public partial class warningFrm : Form
    {
        public warningFrm(String message)
        {
            InitializeComponent();
            warningText.Text = message;
            DialogResult = System.Windows.Forms.DialogResult.Cancel;
        }

        private void continueBtn_Click(object sender, EventArgs e) { DialogResult = System.Windows.Forms.DialogResult.OK; }

        public Boolean Suppressed { get { return reminderCheck.CheckState == CheckState.Checked; } }
    }
}
