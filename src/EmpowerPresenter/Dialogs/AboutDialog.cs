/* ePresenter is licensed under the GPLV3 -- see the 'COPYING' file details.
   Copyright (C) 2006 Alex Korchemniy */
using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace EmpowerPresenter
{
	public class AboutDialog : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Button bClose;

		////////////////////////////////////////////////////////////////////////////////
		public AboutDialog()
		{
			InitializeComponent();

			Program.Presenter.RegisterExKeyOwnerForm(this);
		}
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (this.BackgroundImage != null)
					this.BackgroundImage.Dispose();
			}
			base.Dispose (disposing);
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AboutDialog));
			this.bClose = new System.Windows.Forms.Button();
			this.panel1 = new System.Windows.Forms.Panel();
			this.label1 = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// bClose
			// 
			this.bClose.AccessibleDescription = null;
			this.bClose.AccessibleName = null;
			resources.ApplyResources(this.bClose, "bClose");
			this.bClose.BackColor = System.Drawing.Color.Transparent;
			this.bClose.BackgroundImage = null;
			this.bClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.bClose.Name = "bClose";
			this.bClose.UseVisualStyleBackColor = false;
			this.bClose.Click += new System.EventHandler(this.bClose_Click);
			// 
			// panel1
			// 
			this.panel1.AccessibleDescription = null;
			this.panel1.AccessibleName = null;
			resources.ApplyResources(this.panel1, "panel1");
			this.panel1.BackColor = System.Drawing.Color.Transparent;
			this.panel1.BackgroundImage = null;
			this.panel1.Cursor = System.Windows.Forms.Cursors.Hand;
			this.panel1.Font = null;
			this.panel1.Name = "panel1";
			this.panel1.Click += new System.EventHandler(this.panel1_Click);
			// 
			// label1
			// 
			this.label1.AccessibleDescription = null;
			this.label1.AccessibleName = null;
			resources.ApplyResources(this.label1, "label1");
			this.label1.BackColor = System.Drawing.Color.Transparent;
			this.label1.ForeColor = System.Drawing.Color.Black;
			this.label1.Name = "label1";
			// 
			// AboutDialog
			// 
			this.AccessibleDescription = null;
			this.AccessibleName = null;
			resources.ApplyResources(this, "$this");
			this.BackColor = System.Drawing.SystemColors.Control;
			this.CancelButton = this.bClose;
			this.Controls.Add(this.label1);
			this.Controls.Add(this.panel1);
			this.Controls.Add(this.bClose);
			this.Font = null;
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "AboutDialog";
			this.ShowInTaskbar = false;
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
			this.TopMost = true;
			this.ResumeLayout(false);

		}
		#endregion

		private void linkCreator_LinkClicked(object sender, System.Windows.Forms.LinkLabelLinkClickedEventArgs e)
		{
			System.Diagnostics.Process.Start("http://www.vendisoft.biz");
		}
		private void bClose_Click(object sender, System.EventArgs e)
		{
			this.Close();
			this.Dispose();
		}
		private void panel1_Click(object sender, System.EventArgs e)
		{
			System.Diagnostics.Process.Start("http://www.vendisoft.biz");
		}
	}
}
