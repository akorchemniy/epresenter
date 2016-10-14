/* ePresenter is licensed under the GPLV3 -- see the 'COPYING' file details.
   Copyright (C) 2006 Alex Korchemniy */
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace EmpowerPresenter
{
	public class FullSongPreviewControl : ScrollableControl
	{
		private List<SongVerse> lSongVerses;
		private List<string> searchTerms;
		private int songnum = -1;

		public FullSongPreviewControl()
		{
			SetStyle(ControlStyles.SupportsTransparentBackColor, true);
			SetStyle(ControlStyles.ResizeRedraw, true);
			SetStyle(ControlStyles.DoubleBuffer, true);
			SetStyle(ControlStyles.UserPaint, true);
			SetStyle(ControlStyles.AllPaintingInWmPaint, true);
		}
		public void Init(List<SongVerse> verses, int songnum, List<string> searchTerms)
		{
			lSongVerses = verses;
			this.searchTerms = searchTerms;
			this.songnum = songnum;
			this.Refresh();
		}
		public void Clear()
		{
			if (lSongVerses != null)
				lSongVerses.Clear();
			songnum = -1;
			this.Refresh();
		}

		protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
		{
			if (songnum == -1 || lSongVerses == null || lSongVerses.Count < 1)
				return;

			int cy = 0;
			cy = PaintSong(e.Graphics);

			// size check
			if (cy > this.AutoScrollMinSize.Height) 
			{
				this.AutoScrollMinSize = new Size(this.AutoScrollMinSize.Width, cy + 20);
				this.Refresh();
			}
			else if (cy < this.Height) // Get rid of scrollbars
			{
				this.AutoScrollMinSize = new Size(this.AutoScrollMinSize.Width, cy + 20);
			}
		}
		private int PaintSong(Graphics g)
		{
			int w = this.Width;
			if (w > 350)
				w = 350;
			int cx = (int)(((double)this.Width - w) / 2);
			int margin = 5;
			int itemSpacing = 4;
			int cy = margin;

			for (int i = 0; i < lSongVerses.Count; i++)
			{
				// Build the item to paint
				SongVerse sv = lSongVerses[i];
				string s = "";
				if (i == 0)
					s += songnum + ". ";
				if (sv.IsChorus)
					s += Loc.Get("Chorus:") + " ";
				else
				{
					if (i != 0)
						s += sv.VerseNumber + ". ";
				}
				s += sv.Text;

				// Measure string
				int h = (int)g.MeasureString(s, this.Font, w).Height;
				int xadjust = 0;
				if (sv.IsChorus) // Chorus indent
				{
					w -= 15;
					xadjust = 15;
				}
				Rectangle r = new Rectangle(cx + xadjust, cy, w, h);

				// Measure and fill highlights
				StringFormat sf = new StringFormat();
				sf.SetMeasurableCharacterRanges(SearchHelper.FindCharacterRanges(s, searchTerms));
				foreach (Region rg in g.MeasureCharacterRanges(s, this.Font, r, sf))
					g.FillRegion(new SolidBrush(Color.FromArgb(100, Color.Yellow)), rg);

				
				g.DrawString(s, this.Font, Brushes.Black, r);
				cy += h + itemSpacing;
			}
			return cy;
		}

	}
}
