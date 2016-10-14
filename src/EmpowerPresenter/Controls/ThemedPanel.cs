/* ePresenter is licensed under the GPLV3 -- see the 'COPYING' file details.
   Copyright (C) 2006 Alex Korchemniy */
using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;

using Vendisoft.Drawing;
using EmpowerPresenter;

namespace Vendisoft.Controls
{
	public class ThemedPanel : Panel, EmpowerPresenter.IPopup
	{
		private Image _faceImage;

		private int ls = 0;
		private int rs = 0;
		private int bs = 0;
		private int ts = 0;

		public ThemedPanel()
		{
			SetStyle(ControlStyles.SupportsTransparentBackColor, true);
			SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
			SetStyle(ControlStyles.ResizeRedraw, true);
			SetStyle(ControlStyles.UserPaint, true);
			SetStyle(ControlStyles.AllPaintingInWmPaint, true);
		}
		
		#region Control Events
		protected override void OnPaintBackground(PaintEventArgs e)
		{
			base.OnPaintBackground(e);
			e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
			if (this.Enabled)
				Draw(e.Graphics, _faceImage);
		}
		protected virtual void Draw(Graphics g, Image i)
		{
			if (i == null)
				return;

			ThemedDrawing.DrawThemed(g, i, this.ClientRectangle, ts, bs, rs, ls);
		}
		#endregion

		#region Public Properties
		public Image Image
		{
			get
			{
				return _faceImage;
			}
			set
			{
				if (value != null)
					this._faceImage = value;

				this.Invalidate();
			}
		}
		public int TopSplice
		{
			get
			{
				return ts;
			}
			set
			{
				ts = value;
				this.Invalidate();
			}
		}
		public int BottomSplice
		{
			get
			{
				return bs;
			}
			set
			{
				bs = value;
				this.Invalidate();
			}
		}
		public int RightSplice
		{
			get
			{
				return rs;
			}
			set
			{
				rs = value;
				this.Invalidate();
			}
		}
		public int LeftSlice
		{
			get
			{
				return ls;
			}
			set
			{
				ls = value;
				this.Invalidate();
			}
		}

		#endregion

		#region IPopup Members
		public void Deactivate()
		{
			new DelayedTask(200, delegate { this.Hide(); Program.Presenter.UnregisterPopupWindow(this); });
		}
		#endregion

		protected override void WndProc(ref System.Windows.Forms.Message m)
		{
			if (m.Msg == 0x0F)
			{
				this.Invalidate();
			}

			base.WndProc(ref m);
		}
	}
}
