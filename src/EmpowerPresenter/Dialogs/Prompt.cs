/* ePresenter is licensed under the GPLV3 -- see the 'COPYING' file details.
   Copyright (C) 2006 Alex Korchemniy */
using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace EmpowerPresenter
{
	public class Prompt : System.Windows.Forms.Form
	{
		#region Designer

		private System.Windows.Forms.Button _OK;
		public System.Windows.Forms.Label lblPrompt;
		public System.Windows.Forms.TextBox _input;
		private System.Windows.Forms.Button _cancel;
		#endregion

		public Prompt()
		{
			InitializeComponent();
			Program.Presenter.RegisterExKeyOwnerForm(this);
		}


		#region Windows Form Designer generated code
		private void InitializeComponent()
		{
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Prompt));
			this.lblPrompt = new System.Windows.Forms.Label();
			this._input = new System.Windows.Forms.TextBox();
			this._OK = new System.Windows.Forms.Button();
			this._cancel = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// lblPrompt
			// 
			this.lblPrompt.AccessibleDescription = null;
			this.lblPrompt.AccessibleName = null;
			resources.ApplyResources(this.lblPrompt, "lblPrompt");
			this.lblPrompt.Name = "lblPrompt";
			// 
			// _input
			// 
			this._input.AccessibleDescription = null;
			this._input.AccessibleName = null;
			resources.ApplyResources(this._input, "_input");
			this._input.BackgroundImage = null;
			this._input.Font = null;
			this._input.Name = "_input";
			// 
			// _OK
			// 
			this._OK.AccessibleDescription = null;
			this._OK.AccessibleName = null;
			resources.ApplyResources(this._OK, "_OK");
			this._OK.BackgroundImage = null;
			this._OK.Font = null;
			this._OK.Name = "_OK";
			this._OK.Click += new System.EventHandler(this._OK_Click);
			// 
			// _cancel
			// 
			this._cancel.AccessibleDescription = null;
			this._cancel.AccessibleName = null;
			resources.ApplyResources(this._cancel, "_cancel");
			this._cancel.BackgroundImage = null;
			this._cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this._cancel.Font = null;
			this._cancel.Name = "_cancel";
			this._cancel.Click += new System.EventHandler(this._cancel_Click);
			// 
			// Prompt
			// 
			this.AcceptButton = this._OK;
			this.AccessibleDescription = null;
			this.AccessibleName = null;
			resources.ApplyResources(this, "$this");
			this.BackgroundImage = null;
			this.CancelButton = this._cancel;
			this.Controls.Add(this._OK);
			this.Controls.Add(this._input);
			this.Controls.Add(this.lblPrompt);
			this.Controls.Add(this._cancel);
			this.Font = null;
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "Prompt";
			this.ResumeLayout(false);
			this.PerformLayout();

		}
		#endregion

		private void _cancel_Click(object sender, System.EventArgs e)
		{
			DialogResult = DialogResult.Cancel;
			this.Close();
		}

		private void _OK_Click(object sender, System.EventArgs e)
		{
			DialogResult = DialogResult.OK;
			this.Close();
		}
	}
}
