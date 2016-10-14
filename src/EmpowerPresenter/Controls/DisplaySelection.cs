/* ePresenter is licensed under the GPLV3 -- see the 'COPYING' file details.
   Copyright (C) 2006 Alex Korchemniy */
using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;

namespace EmpowerPresenter
{
	public class DisplaySelection : System.Windows.Forms.Control
	{
		public event EventHandler SelectionChanged;

		private Hashtable h = new Hashtable();
		private int currentScreen = 0;
		private double lowestp = 0;
		private double highestp = 0;
		private double rightp = 0;
		private double leftp = 0;
		private Font f = new Font("Arial", 24F, FontStyle.Bold);
		private StringFormat sfCentered;

		public DisplaySelection()
		{
			SetStyle(ControlStyles.ResizeRedraw, true);
			sfCentered = new StringFormat();
			sfCentered.Alignment = StringAlignment.Center;
			sfCentered.LineAlignment = StringAlignment.Center;

			#region Determine the scale
			foreach(Screen s in Screen.AllScreens)
			{
				// heighest
				if (s.Bounds.Top < highestp)
					highestp = s.Bounds.Y;

				// left
				if (s.Bounds.Left < leftp)
					leftp = s.Bounds.X;

				// right
				if (s.Bounds.Right > rightp)
					rightp = s.Bounds.Right;

				// lowest
				if (s.Bounds.Bottom > lowestp)
					lowestp = s.Bounds.Bottom;
			}
			#endregion
		}

		protected override void OnMouseMove(MouseEventArgs e)
		{
			bool inside = false;
			Point p = new Point(e.X, e.Y);
			foreach(Rectangle r in h.Keys)
			{
				if (r.Contains(p))
					inside = true;
			}
			if (inside)
				Cursor = Cursors.Hand;
			else
				Cursor = Cursors.Default;

			base.OnMouseMove (e);
		}
		protected override void OnMouseDown(MouseEventArgs e)
		{
			Point p = new Point(e.X, e.Y);
			foreach(Rectangle r in h.Keys)
			{
				if (r.Contains(p))
				{
					currentScreen = (int)h[r];
					this.Invalidate();
					if (SelectionChanged != null)
						SelectionChanged(null, null);
				}
			}
			base.OnMouseDown (e);
		}
		protected override void OnResize(EventArgs e)
		{
			this.h.Clear();
			base.OnResize (e);
		}
		protected override void OnPaint(PaintEventArgs pe)
		{
			Graphics g = pe.Graphics;

			// Calc scale
			double p = 0; // scale factor
			double totalh = Math.Abs(highestp - lowestp);
			double totalw = Math.Abs(rightp - leftp);
			double pw = this.ClientSize.Width / (totalw * 1.3);
			double ph = this.ClientSize.Height / (totalh * 1.3);
			p = (pw < ph) ? pw : ph;

			// Start with standard original
			double xmid = this.ClientSize.Width / 2;
			double ymid = this.ClientSize.Height / 2;

			// Adjustments
			double yadj = (lowestp + highestp) / 2;
			double xadj = (rightp + leftp) / 2;
			xmid -= xadj * p;
			ymid -= yadj * p;
			
			// Calculate rectangles
			if (h.Count == 0)
			{
				for(int i = 0; i < Screen.AllScreens.Length; i++)
				{
					// Translate the location
					Screen s = Screen.AllScreens[i];
					Point location = new Point(Convert.ToInt32(xmid + s.Bounds.X * p), Convert.ToInt32(ymid + s.Bounds.Y * p));
					Size size = new Size(Convert.ToInt32(s.Bounds.Width * p), Convert.ToInt32(s.Bounds.Height * p));
					Rectangle r = new Rectangle(location, size);
					h.Add(r, i);
				}
			}

			// Draw all rectangles
			foreach(Rectangle r in h.Keys)
			{
				int index = (int)h[r];
				string s = Convert.ToString(index + 1);
				r.Inflate(-2,-2);
				if (index == currentScreen)
				{
					Pen pen = new Pen(Color.LightSteelBlue, 2);
					g.FillRectangle(Brushes.LightGray, r);
					g.DrawRectangle(pen, r);
					g.DrawString(s, f, Brushes.DimGray, new RectangleF(r.Location, r.Size), sfCentered);
				}
				else
				{
					Pen pen = new Pen(Color.DarkGray, 2);
					g.FillRectangle(Brushes.Gray, r);
					g.DrawRectangle(pen, r);
					g.DrawString(s, f, Brushes.DimGray, new RectangleF(r.Location, r.Size), sfCentered);
				}
			}

			base.OnPaint(pe);
		}

		public int CurrentScreenIndex
		{
			get{return currentScreen;}
			set
			{
				if (Screen.AllScreens.Length < currentScreen)
					return;

				currentScreen = value;
				this.Invalidate();
			}
		}
	}
}
