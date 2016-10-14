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
	public partial class ImageDispStyle : Form
	{
		private ImageDisplayStyle dispStyle = ImageDisplayStyle.SmartFit;

		//////////////////////////////////////////////////////////////////////
		public ImageDispStyle()
		{
			InitializeComponent();
		}

		private void ImageDispStyle_Load(object sender, EventArgs e)
		{
			Style = dispStyle;
		}
		private void rCenter_CheckedChanged(object sender, EventArgs e)
		{
			if (rCenter.Checked)
				Style = ImageDisplayStyle.Center;
		}
		private void rSmartFit_CheckedChanged(object sender, EventArgs e)
		{
			if (rSmartFit.Checked)
				Style = ImageDisplayStyle.SmartFit;
		}
		private void rStretch_CheckedChanged(object sender, EventArgs e)
		{
			if (rStretch.Checked)
				Style = ImageDisplayStyle.Stretch;
		}
		private void btnCancel_Click(object sender, EventArgs e)
		{
			DialogResult = DialogResult.Cancel;
			this.Close();
		}
		private void btnOK_Click(object sender, EventArgs e)
		{
			DialogResult = DialogResult.OK;
			this.Close();
		}

		public ImageDisplayStyle Style
		{
			get { return dispStyle; }
			set
			{
				dispStyle = value;
				switch (dispStyle)
				{
					case ImageDisplayStyle.Center:
						if (rCenter.Checked != true)
							rCenter.Checked = true;
						pbCenter.BringToFront();
						break;
					case ImageDisplayStyle.SmartFit:
						if (rSmartFit.Checked != true)
							rSmartFit.Checked = true;
						pbSmartFit.BringToFront();
						break;
					case ImageDisplayStyle.Stretch:
						if (rStretch.Checked != true)
							rStretch.Checked = true;
						pbStretch.BringToFront();
						break;
				}
			}
		}

	}
}