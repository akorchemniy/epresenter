/* ePresenter is licensed under the GPLV3 -- see the 'COPYING' file details.
   Copyright (C) 2006 Alex Korchemniy */
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace EmpowerPresenter
{
	public partial class DualDispInfo : Form
	{
		//////////////////////////////////////////////////
		public DualDispInfo()
		{
			InitializeComponent();
			Program.Presenter.RegisterExKeyOwnerForm(this);
		}
		private void btnActivate_Click(object sender, EventArgs e)
		{
			this.Close();
		}
		private void llDispInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			try
			{
				string loc = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "help\\setting_display.html");
				System.Diagnostics.Process.Start(loc);
				this.Close();
			}
			catch { }
		}
	}
}