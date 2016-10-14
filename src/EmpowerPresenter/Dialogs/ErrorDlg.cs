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
	public partial class ErrorDlg : Form
	{
		//////////////////////////////////////////////////
		public ErrorDlg(string error)
		{
			InitializeComponent();

			txtError.Text = error;
			Program.Presenter.RegisterExKeyOwnerForm(this);
		}
		private void btnOK_Click(object sender, EventArgs e)
		{
			this.Close();
		}
		private void llClipboard_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			Clipboard.SetText(txtError.Text);
		}
	}
}