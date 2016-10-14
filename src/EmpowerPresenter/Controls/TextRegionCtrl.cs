/* ePresenter is licensed under the GPLV3 -- see the 'COPYING' file details.
   Copyright (C) 2006 Alex Korchemniy */
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace EmpowerPresenter
{
	public class TextRegionCtrl : Control
	{
		public event EventHandler FinishedMove;

		private bool resizing = false;
		private bool moving = false;
		private bool selected = false;
		private Point ptLastMouseDown = Point.Empty;
		private Rectangle rStart = Rectangle.Empty;
		private DragPos dragPos = DragPos.None;
		internal GfxTextRegion textRegion = null;
		private System.ComponentModel.IContainer components = null;

		////////////////////////////////////////////////////////////////////////
		public TextRegionCtrl()
		{
			SetStyle(ControlStyles.SupportsTransparentBackColor, true);
			SetStyle(ControlStyles.ResizeRedraw, true);
			SetStyle(ControlStyles.DoubleBuffer, true);
			SetStyle(ControlStyles.UserPaint, true);
			SetStyle(ControlStyles.AllPaintingInWmPaint, true);

			InitializeComponent();
			this.BackColor = Color.Transparent;
			this.Cursor = Cursors.Hand;
		}
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}
		private void InitializeComponent()
		{
			components = new System.ComponentModel.Container();
		}

		protected override void OnPaint(PaintEventArgs pe)
		{
			Graphics g = pe.Graphics;

			// Draw the selected outline
			if (selected)
			{
				Pen pGray = new Pen(Brushes.LightGray);
				pGray.Width = 3;
				g.DrawRectangle(pGray, new Rectangle(3, 3, this.Width - 7, this.Height - 7));
			}

			// Draw the lines around the text
			Pen p = new Pen(Color.Black);
			p.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
			Rectangle r = this.ClientRectangle;
			g.DrawRectangle(p, 3, 3, r.Width - 7, r.Height - 7);

			// Draw the drag handles
			g.FillRectangle(Brushes.White, 0, 0, 6, 6);
			g.DrawRectangle(Pens.Black, 0, 0, 6, 6);
			g.FillRectangle(Brushes.White, r.Width - 7, 0, 6, 6);
			g.DrawRectangle(Pens.Black, r.Width - 7, 0, 6, 6);
			g.FillRectangle(Brushes.White, r.Width - 7, r.Height - 7, 6, 6);
			g.DrawRectangle(Pens.Black, r.Width - 7, r.Height - 7, 6, 6);
			g.FillRectangle(Brushes.White, 0, r.Height - 7, 6, 6);
			g.DrawRectangle(Pens.Black, 0, r.Height - 7, 6, 6);

			// Draw the text
			//StringFormat sf = new StringFormat(StringFormatFlags.FitBlackBox);
			//sf.Trimming = StringTrimming.EllipsisCharacter;
			//g.DrawString(textRegion.message, this.Font, new SolidBrush(this.ForeColor), new Rectangle(5, 5, r.Width - 11, r.Height - 11));
		}
		protected override void OnMouseMove(MouseEventArgs e)
		{
			if (resizing)
			{
				Rectangle r = this.Bounds;
				switch (dragPos)
				{
					case DragPos.TopLeft:
						r.X = rStart.X + this.PointToScreen(e.Location).X - ptLastMouseDown.X;
						r.Y = rStart.Y + this.PointToScreen(e.Location).Y - ptLastMouseDown.Y;
						r.Width = rStart.Width - this.PointToScreen(e.Location).X + ptLastMouseDown.X;
						r.Height = rStart.Height - this.PointToScreen(e.Location).Y + ptLastMouseDown.Y;
						break;
					case DragPos.Top:
						r.Y = rStart.Y + this.PointToScreen(e.Location).Y - ptLastMouseDown.Y;
						r.Height = rStart.Height - this.PointToScreen(e.Location).Y + ptLastMouseDown.Y;
						break;
					case DragPos.TopRight:
						r.Y = rStart.Y + this.PointToScreen(e.Location).Y - ptLastMouseDown.Y;
						r.Width = rStart.Width + this.PointToScreen(e.Location).X - ptLastMouseDown.X;
						r.Height = rStart.Height - this.PointToScreen(e.Location).Y + ptLastMouseDown.Y;
						break;
					case DragPos.Right:
						r.Width = rStart.Width + this.PointToScreen(e.Location).X - ptLastMouseDown.X;
						break;
					case DragPos.BottomRight:
						r.Width = rStart.Width + this.PointToScreen(e.Location).X - ptLastMouseDown.X;
						r.Height = rStart.Height + this.PointToScreen(e.Location).Y - ptLastMouseDown.Y;
						break;
					case DragPos.Bottom:
						r.Height = rStart.Height + this.PointToScreen(e.Location).Y - ptLastMouseDown.Y;
						break;
					case DragPos.BottomLeft:
						r.X = rStart.X + this.PointToScreen(e.Location).X - ptLastMouseDown.X;
						r.Width = rStart.Width - this.PointToScreen(e.Location).X + ptLastMouseDown.X;
						r.Height = rStart.Height + this.PointToScreen(e.Location).Y - ptLastMouseDown.Y;
						break;
					case DragPos.Left:
						r.X = rStart.X + this.PointToScreen(e.Location).X - ptLastMouseDown.X;
						r.Width = rStart.Width - this.PointToScreen(e.Location).X + ptLastMouseDown.X;
						break;
				}
				this.Bounds = r;
			}
			else if (moving)
			{
				int x = rStart.X - ptLastMouseDown.X + this.PointToScreen(e.Location).X;
				int y = rStart.Y - ptLastMouseDown.Y + this.PointToScreen(e.Location).Y;
				this.Location = new Point(x, y);
			}
			else
			{
				// Hit testing and update cursor
				if (e.X < 6)
				{
					if (e.Y < 6)
						this.Cursor = Cursors.SizeNWSE;
					else if (e.Y > this.ClientSize.Height - 6)
						this.Cursor = Cursors.SizeNESW;
					else
						this.Cursor = Cursors.SizeWE;
				}
				else if (e.X > this.ClientSize.Width - 6)
				{
					if (e.Y < 6)
						this.Cursor = Cursors.SizeNESW;
					else if (e.Y > this.ClientSize.Height - 6)
						this.Cursor = Cursors.SizeNWSE;
					else
						this.Cursor = Cursors.SizeWE;
				}
				else
				{
					if (e.Y < 6)
						this.Cursor = Cursors.SizeNS;
					else if (e.Y > this.ClientSize.Height - 6)
						this.Cursor = Cursors.SizeNS;
					else
						this.Cursor = Cursors.Hand;
				}
			}
			base.OnMouseMove(e);
		}
		protected override void OnMouseDown(MouseEventArgs e)
		{
			rStart = this.Bounds;
			ptLastMouseDown = this.PointToScreen(e.Location);
			if (e.X < 6)
			{
				if (e.Y < 6)
					dragPos = DragPos.TopLeft;
				else if (e.Y > this.ClientSize.Height - 6)
					dragPos = DragPos.BottomLeft;
				else
					dragPos = DragPos.Left;
			}
			else if (e.X > this.ClientSize.Width - 6)
			{
				if (e.Y < 6)
					dragPos = DragPos.TopRight;
				else if (e.Y > this.ClientSize.Height - 6)
					dragPos = DragPos.BottomRight;
				else
					dragPos = DragPos.Right;
			}
			else
			{
				if (e.Y < 6)
					dragPos = DragPos.Top;
				else if (e.Y > this.ClientSize.Height - 6)
					dragPos = DragPos.Bottom;
				else
					dragPos = DragPos.Drag;
			}
			resizing = dragPos != DragPos.Drag;
			moving = dragPos == DragPos.Drag;
			base.OnMouseDown(e);

		}
		protected override void OnMouseUp(MouseEventArgs e)
		{
			resizing = false;
			moving = false;
			dragPos = DragPos.None;

			if (FinishedMove != null)
				FinishedMove(this, null);

			base.OnMouseUp(e);
		}
		protected override void OnMouseLeave(EventArgs e)
		{
			resizing = false;
			moving = false;
			dragPos = DragPos.None;
			base.OnMouseLeave(e);
		}

		public string Message
		{
			get
			{
				return textRegion.message;
			}
			set
			{
				textRegion.message = value;
				this.Refresh();
			}
		}
		public bool Selected
		{
			get { return selected; }
			set { if (selected != value) { selected = value; this.Refresh(); } }
		}
	}
	public enum DragPos
	{
		TopLeft, Top, TopRight, Right, BottomRight, Bottom, BottomLeft, Left, Drag, None
	}
}
