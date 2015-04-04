/*
 * Created by SharpDevelop.
 * User: rdo
 * Date: 12.05.2013
 * Time: 18:08
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace MiniLauncher
{
	/// <summary>
	/// Description of MainForm.
	/// </summary>
	public partial class MainForm : Form
	{
		
		protected static String ServerFile = "MLservers.txt";
		protected static String UserFile = "MLusers.txt";
		protected static String swgclient = "swgclient_r.exe";
		
		public MainForm()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			InitializeComponent2();
			
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
		}
		
		void InitializeComponent2() {
			button1.Enabled = false;
			//this.comboBox1.SelectedValueChanged += new System.EventHandler(this.ComboBox1SelectedValueChanged);
			this.comboBox1.TextChanged += new System.EventHandler(this.ComboBox1SelectedValueChanged);
			ReadStuff();
		}
		
		void ReadStuff() {
			
			if (File.Exists(UserFile)) {
				try {
					String user = File.ReadAllText(UserFile);
					if (IsValidUser(user.TrimEnd())) {
						textBox1.Text = user.TrimEnd();
					}
				} catch {}
			}
			
			if (File.Exists(ServerFile)) {
				try {
					String[] servers = File.ReadAllLines(ServerFile);
					for (int i = 0; i < servers.Length; i++) {
						if (IsValidServer(servers[i].TrimEnd())) {
							comboBox1.Items.Add(servers[i].TrimEnd());
						}
						if (i == 0) {
							comboBox1.SelectedIndex = 0;
						}						
					}
				} catch{}
			}	
		}
			
		
		void Button1Click(object sender, EventArgs e)
		{
			
			try {
				
				System.IO.File.WriteAllText(UserFile, textBox1.Text);
				
				string[] servers = new String[comboBox1.Items.Count + 1];
				servers[0] = comboBox1.Text;
				for (int i = 0; i < comboBox1.Items.Count; i++) {
					if (servers[0].Equals(comboBox1.Items[i].ToString())) {
						continue;
					}
					servers[i+1] = comboBox1.Items[i].ToString();
				}
				System.IO.File.WriteAllLines(ServerFile, servers);
				
			} catch {
				
			}
			
			if (!File.Exists(swgclient)) {
				return;
			}

			if (!IsValidUser(textBox1.Text)) { return; }
			if (!IsValidServer(comboBox1.Text)) { return; }			
			String[] x = comboBox1.Text.Split(':');
			if (x.Length < 2) { return; }
			
			String args = String.Format("-- -s Station subscriptionFeatures=1 gameFeatures=34374193 -s ClientGame loginServerPort0={0} loginServerAddress0={1} loginClientID={2}", x[1], x[0], textBox1.Text);
        	
        	this.Hide();
            System.Threading.Thread.Sleep(200);

        	try {
	        	this.Hide();
        		System.Diagnostics.Process.Start(swgclient, args);
	            Application.Exit();
        			
        	} catch {
        		
        	}			
			
			
			
		}

		void ComboBox1SelectedValueChanged(object sender, EventArgs e)
		{
			CheckReady();
		}
		

		void TextBox1TextChanged(object sender, EventArgs e)
		{
			CheckReady();
		}
		
		void CheckReady() {
			button1.Enabled = IsValidUser(comboBox1.Text) && IsValidServer(comboBox1.Text);
		}
		
		bool IsValidServer(String text) {
			Match match = Regex.Match(text, @"^[-\.A-Za-z0-9]+:[0-9]{1,5}$");
			return match.Success;			
		}
		
		bool IsValidUser(String text) {
			return (text.Length > 0);
		}
		

	}
}
