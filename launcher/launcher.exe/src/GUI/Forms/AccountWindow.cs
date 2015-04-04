using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace PswgLauncher
{
    public partial class AccountWindow : Form
    {
    	
    	private GuiController Controller;
    	
        public AccountWindow(GuiController gc)
        {
        	this.Controller = gc;
            InitializeComponent();
            this.Icon= Controller.GetAppIcon();
        }
    }
}
