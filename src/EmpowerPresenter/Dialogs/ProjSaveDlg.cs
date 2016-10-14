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
	public partial class ProjSaveDlg : Form
	{
		public event System.ComponentModel.CancelEventHandler ValidateName;

		////////////////////////////////////////////////////////////////
		public ProjSaveDlg()
		{
			InitializeComponent();
			Program.Presenter.RegisterExKeyOwnerForm(this);
		}

		private void txtName_TextChanged(object sender, EventArgs e)
		{
			CancelEventArgs a = new CancelEventArgs();
			if (ValidateName != null)
				ValidateName(txtName.Text, a);
			if (a.Cancel)
			{
				errorProvider1.SetError(txtName, Loc.Get("A project with this name already exists"));
				btnOK.Enabled = false;
			}
			else
			{
				errorProvider1.SetError(txtName, "");
				btnOK.Enabled = true;
			}
		}
		private void btnOK_Click(object sender, EventArgs e)
		{
			DialogResult = DialogResult.OK;
			this.Close();
		}
		private void btnCancel_Click(object sender, EventArgs e)
		{
			DialogResult = DialogResult.Cancel;
			this.Close();
		}

		public string FileName
		{
			get { return txtName.Text; }
		}
	}
}