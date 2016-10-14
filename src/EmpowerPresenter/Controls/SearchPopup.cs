/* ePresenter is licensed under the GPLV3 -- see the 'COPYING' file details.
   Copyright (C) 2006 Alex Korchemniy */
using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using System.Collections.Generic;

namespace EmpowerPresenter.Controls
{
	public class SearchPopup : Control, IPopup
	{
		public event EventHandler SearchTermChanged;

		private Vendisoft.Controls.ThemedPanel themedPanel1;
		private Vendisoft.Controls.ImageButton btnClose;
		private TextBox txtSearch;

		////////////////////////////////////////////////////////////////////////
		public SearchPopup()
		{
			SetStyle(ControlStyles.SupportsTransparentBackColor, true);
			SetStyle(ControlStyles.ResizeRedraw, true);
			SetStyle(ControlStyles.DoubleBuffer, true);
			SetStyle(ControlStyles.UserPaint, true);
			SetStyle(ControlStyles.AllPaintingInWmPaint, true);

			InitializeComponent();

			// Register exclusive key owners for key manager
			if (Program.Presenter != null)
			{
				Program.Presenter.RegisterExKeyOwnerControl(txtSearch);
			}
		}
		private void InitializeComponent()
		{
			this.themedPanel1 = new Vendisoft.Controls.ThemedPanel();
			this.txtSearch = new System.Windows.Forms.TextBox();
			this.btnClose = new Vendisoft.Controls.ImageButton();
			this.themedPanel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// themedPanel1
			// 
			this.themedPanel1.BottomSplice = 15;
			this.themedPanel1.Controls.Add(this.btnClose);
			this.themedPanel1.Controls.Add(this.txtSearch);
			this.themedPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.themedPanel1.Image = global::EmpowerPresenter.Properties.Resources.over;
			this.themedPanel1.LeftSlice = 15;
			this.themedPanel1.Location = new System.Drawing.Point(0, 0);
			this.themedPanel1.Name = "themedPanel1";
			this.themedPanel1.RightSplice = 15;
			this.themedPanel1.Size = new System.Drawing.Size(283, 39);
			this.themedPanel1.TabIndex = 0;
			this.themedPanel1.TopSplice = 15;
			// 
			// txtSearch
			// 
			this.txtSearch.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.txtSearch.Location = new System.Drawing.Point(7, 9);
			this.txtSearch.Name = "txtSearch";
			this.txtSearch.Size = new System.Drawing.Size(243, 20);
			this.txtSearch.TabIndex = 0;
			this.txtSearch.TextChanged += new System.EventHandler(this.txtSearch_TextChanged);
			this.txtSearch.KeyPress += new KeyPressEventHandler(txtSearch_KeyPress);
			// 
			// btnClose
			// 
			this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnClose.Cursor = Cursors.Hand;
			this.btnClose.DisabledImg = null;
			this.btnClose.IsHighlighted = false;
			this.btnClose.IsPressed = false;
			this.btnClose.IsSelected = false;
			this.btnClose.Location = new System.Drawing.Point(252, 8);
			this.btnClose.Name = "btnClose";
			this.btnClose.NormalImg = global::EmpowerPresenter.Properties.Resources.close_n;
			this.btnClose.OverImg = global::EmpowerPresenter.Properties.Resources.close_n;
			this.btnClose.PressedImg = global::EmpowerPresenter.Properties.Resources.close_o;
			this.btnClose.Size = new System.Drawing.Size(24, 23);
			this.btnClose.TabIndex = 1;
			this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
			// 
			// SearchPopup
			// 
			this.BackColor = System.Drawing.Color.Transparent;
			this.Controls.Add(this.themedPanel1);
			this.MaximumSize = new System.Drawing.Size(483, 39);
			this.MinimumSize = new System.Drawing.Size(203, 39);
			this.Name = "SearchPopup";
			this.Size = new System.Drawing.Size(283, 39);
			this.themedPanel1.ResumeLayout(false);
			this.themedPanel1.PerformLayout();
			this.ResumeLayout(false);

		}

		private void btnClose_Click(object sender, EventArgs e)
		{
			this.Deactivate();
		}
		private void txtSearch_TextChanged(object sender, EventArgs e)
		{
			if (SearchTermChanged != null)
				SearchTermChanged(null, null);
		}
		private void txtSearch_KeyPress(object sender, KeyPressEventArgs e)
		{
			if (e.KeyChar == (char)Keys.Escape || e.KeyChar == (char)Keys.Enter)
			{
				e.Handled = true;
				this.Deactivate();
			}
		}
		protected override void OnVisibleChanged(EventArgs e)
		{
			if (this.Visible)
				txtSearch.Focus();
			base.OnVisibleChanged(e);
		}

		public List<string> SearchTerms
		{
			get
			{
				return SearchHelper.BreakSearchTerms(txtSearch.Text);
			}
		}

		#region IPopup Members

		public void Deactivate()
		{
			txtSearch.Text = "";
			new DelayedTask(200, delegate { this.Hide(); Program.Presenter.UnregisterPopupWindow(this); });
		}

		#endregion
	}
}
