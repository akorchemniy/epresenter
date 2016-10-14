/* ePresenter is licensed under the GPLV3 -- see the 'COPYING' file details.
   Copyright (C) 2006 Alex Korchemniy */
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace EmpowerPresenter
{
	public partial class DemoVersionOnly : Form
	{
		private Timer t = new Timer();
		public DemoVersionOnly(string featureName)
		{
			InitializeComponent();
			t.Interval = 3000;
			t.Tick += new EventHandler(t_Tick);
			t.Start();

			lblFeature.Text = Loc.Get(featureName);
			Program.Presenter.RegisterExKeyOwnerForm(this);
		}

		void t_Tick(object sender, EventArgs e)
		{
			t.Stop();
			btnOK.Enabled = true;
		}
		private void btnOK_Click(object sender, EventArgs e)
		{
			this.Close();
		}
		private void DemoVersionOnly_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (!btnOK.Enabled)
				e.Cancel = true;
		}
	}
}