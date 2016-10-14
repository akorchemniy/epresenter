/* ePresenter is licensed under the GPLV3 -- see the 'COPYING' file details.
   Copyright (C) 2006 Alex Korchemniy */
using System;
using System.Drawing;

namespace Vendisoft.Drawing
{
	public class ThemedDrawing
	{
		public static void DrawThemed(Graphics g, Image i, Rectangle drawingRect, ThemeSlices themeSlices)
		{
			int h = drawingRect.Height;
			int w = drawingRect.Width;
			DrawThemed(g, i, drawingRect, themeSlices.topSlice, themeSlices.bottomSlice, themeSlices.rightSlice, themeSlices.leftSlice);
		}
		public static void DrawThemed(Graphics g, Image i, Rectangle drawingRect, int ts, int bs, int rs, int ls)
		{
			int h = drawingRect.Height;
			int w = drawingRect.Width;

			// Fill inside
			g.DrawImage(i,
				new Rectangle(ls, ts, w - ls - rs, h - bs - ts),
				ls, ts, i.Width - ls - rs, i.Height - bs - ts, GraphicsUnit.Pixel);

			// Fill edges
			g.DrawImage(i,
				new Rectangle(0, ts, ls, h - bs - ts),
				0, ts, ls, i.Height - bs - ts, GraphicsUnit.Pixel); // left
			g.DrawImage(i,
				new Rectangle(w - rs, ts, rs, h - bs - ts),
				i.Width - rs, ts, rs, i.Height - bs - ts, GraphicsUnit.Pixel); // right
			g.DrawImage(i,
				new Rectangle(ls, 0, w - ls - rs, ts),
				ls, 0, i.Width - ls - rs, ts, GraphicsUnit.Pixel); // top
			g.DrawImage(i,
				new Rectangle(ls, h - bs, w - ls - rs, bs),
				ls, i.Height - bs, i.Width - ls - rs, bs, GraphicsUnit.Pixel); // bottom

			#region Draw corners
			g.DrawImage(i,
				new Rectangle(0, 0, ls, ts),
				0, 0, ls, ts, GraphicsUnit.Pixel); // top left
			g.DrawImage(i,
				new Rectangle(0, h - bs, ls, bs),
				0, i.Height - bs, ls, bs, GraphicsUnit.Pixel); // bottom left
			g.DrawImage(i,
				new Rectangle(w - rs, 0, rs, ts),
				i.Width - rs, 0, rs, ts, GraphicsUnit.Pixel); // top right
			g.DrawImage(i,
				new Rectangle(w - rs, h - bs, rs, bs),
				i.Width - rs, i.Height - bs, rs, bs, GraphicsUnit.Pixel); // bottom right
			#endregion
		}
	}
	public class ThemeSlices
	{
		public int topSlice;
		public int bottomSlice;
		public int rightSlice;
		public int leftSlice;

		public ThemeSlices(int ts, int bs, int rs, int ls)
		{
			topSlice = ts;
			bottomSlice = bs;
			rightSlice = rs;
			leftSlice = ls;
		}
	}
}
