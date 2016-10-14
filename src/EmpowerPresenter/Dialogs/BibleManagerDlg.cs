/* ePresenter is licensed under the GPLV3 -- see the 'COPYING' file details.
   Copyright (C) 2006 Alex Korchemniy */
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace EmpowerPresenter.Dialogs
{
	public partial class BibleManagerDlg : Form
	{
		////////////////////////////////////////////////////////////////////////
		public BibleManagerDlg()
		{
			// TODO: log ceip
			// TODO: create list of bibles

			InitializeComponent();
		}
		private void UpdateBtnState()
		{
			btnRemoveBib.Enabled = lbBibles.SelectedIndex != -1;
		}

		// Painting
		Brush brushNormal;
		Brush brushSelected;
		Font boldFont;
		private void lbBibles_DrawItem(object sender, DrawItemEventArgs e)
		{
			if (e.Index < 0 || e.Index >= lbBibles.Items.Count)
				return;

			if (boldFont == null)
				boldFont = new Font(this.Font, FontStyle.Bold);
			if (brushNormal == null)
				brushNormal = new SolidBrush(Color.FromArgb(223, 223, 223));
			if (brushSelected == null)
				brushSelected = new SolidBrush(Color.FromArgb(255, 204, 91));

			ePresenterBible pli = (ePresenterBible)lbBibles.Items[e.Index];

			// Draw the background
			if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
				e.Graphics.FillRectangle(brushSelected, e.Bounds.X + 1, e.Bounds.Y + 1, e.Bounds.Width - 2, e.Bounds.Height - 2);
			else
				e.Graphics.FillRectangle(brushNormal, e.Bounds.X + 1, e.Bounds.Y + 1, e.Bounds.Width - 2, e.Bounds.Height - 2);

			// Draw the name
			StringFormat sf = new StringFormat();
			sf.Trimming = StringTrimming.EllipsisCharacter;
			e.Graphics.DrawString(pli.ToString(), boldFont, Brushes.Black, new Rectangle(e.Bounds.X + 30, e.Bounds.Y + 9, e.Bounds.Width - 35, this.Font.Height + 2), sf);

			// Draw the image
			Image i = global::EmpowerPresenter.Properties.Resources.bib16;
			e.Graphics.DrawImage(i, new Rectangle(e.Bounds.X + 6, e.Bounds.Y + 8, i.Width, i.Height), new Rectangle(0, 0, i.Width, i.Height), GraphicsUnit.Pixel);
		}

		// Events
		private void lbBibles_SelectedIndexChanged(object sender, EventArgs e)
		{
			UpdateBtnState();
		}
		private void btnAddBib_Click(object sender, EventArgs e)
		{
			// TODO: show popup menu
		}
		private void btnRemoveBib_Click(object sender, EventArgs e)
		{
			// TODO: confirm
			// TODO: remove data
			// TODO: remove from list
		}
		private void Info_MouseEnter(object sender, EventArgs e)
		{
			lblInfo.Text = Loc.Get(((Control)sender).Tag.ToString());
		}
		private void Info_MouseLeave(object sender, EventArgs e)
		{
			lblInfo.Text = "";
		}
	}
}