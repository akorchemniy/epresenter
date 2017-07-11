/* ePresenter is licensed under the GPLV3 -- see the 'COPYING' file details.
   Copyright (C) 2006 Alex Korchemniy */
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace EmpowerPresenter
{
    public class BibVersePreviewControl : Control
    {
        private BibleProject proj;
        private EventHandler ehRefresh;
        private Dictionary<Rectangle, BibleVerse> hitMapping = new Dictionary<Rectangle, BibleVerse>();
        private Dictionary<int, Rectangle> verseRectMapping = new Dictionary<int, Rectangle>();
        private BibleVerse highlightedVerse;
        private int currentChapter = 0;

        // Scrolling
        private double scrollerPos = 0; // This is a percetage of the page
        private double scrollerRatio = 1;
        private int scrollerHeight = 0;
        private int totalVerseHeight = 0;
        private bool scrolling = false;
        private int scrollMDownY = 0;
        private double scrollPosAtMDown = 0; // scroll position at mouse down

        // Search term highlighting
        private List<string> searchTerms = new List<string>();
        internal bool allowGrabFocus = true; // TODO: fix this hack

        ////////////////////////////////////////////////////////////////////////////////
        public BibVersePreviewControl()
        {
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            SetStyle(ControlStyles.ResizeRedraw, true);
            SetStyle(ControlStyles.DoubleBuffer, true);
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.UserMouse, true);
        }
        public void AttachProject(BibleProject proj)
        {
            if (proj != this.proj)
            {
                DetachProject();

                this.proj = proj;
                ehRefresh = new EventHandler(proj_refresh);
                this.proj.Refresh += ehRefresh;

                CalculateItems();
                this.Refresh();
            }
        }
        public void DetachProject()
        {
            if (proj != null && ehRefresh != null)
                proj.Refresh -= ehRefresh;
            this.proj = null;
            ehRefresh = null;

            // Reset variables
            hitMapping.Clear();
            verseRectMapping.Clear();
            highlightedVerse = null;
            scrollerPos = 0;
            scrollerRatio = 1;
            scrollerHeight = 0;
            searchTerms.Clear();
        }
        public void UpdateSearchTerms(List<string> terms)
        {
            searchTerms.Clear();
            foreach (string s in terms)
                searchTerms.Add(s);
            this.Refresh();
        }
        private void CalculateItems()
        {
            /// Function summary:
            /// We need to get the rectangles for each verse and store them for hit detection
            /// and easy painting.

            hitMapping.Clear();
            verseRectMapping.Clear();
            if (proj == null || proj.bibVerses.Count == 0 || proj.currentVerseNum == -1)
                return;

            // Init
            Bitmap b = new Bitmap(1, 1); 
            Graphics g = Graphics.FromImage(b);
            int spacing = 1;
            int indent = 16;
            int textPadding = 4;
            int textWidth = this.Width - spacing - indent;

            // Tile downward
            int cy = spacing;
            int ixCurrent = 1;
            while (ixCurrent <= proj.bibVerses.Count)
            {
                BibleVerse bv = proj.bibVerses[ixCurrent];
                currentChapter = bv.RefChapter;
                int itemH = (int)g.MeasureString(bv.RefVerse + ". " + bv.Text, this.Font, textWidth).Height + textPadding;
                Rectangle r = new Rectangle(indent, cy, this.Width - indent, itemH);
                hitMapping.Add(r, bv);
                verseRectMapping.Add(ixCurrent, r);
                cy += itemH + spacing;
                ixCurrent++;
            }

            // Calculate scroller ratio
            Rectangle lastVerse = verseRectMapping[proj.bibVerses.Count];
            totalVerseHeight = lastVerse.Y + lastVerse.Height;
            double sr = (double)Height / totalVerseHeight;
            if (sr < 1)
            {
                scrollerRatio = sr;
                scrollerHeight = (int)(Height * sr);
            }
            else
                scrollerRatio = 1;

            CenterToCurrentVerse();
        }
        private void CenterToCurrentVerse()
        {
            /// Function summary:
            /// This function is responsible for centering the current verse...
            /// There need to be two built in exceptions: too close to the start or end

            if (proj == null || proj.bibVerses.Count == 0 || proj.currentVerseNum == -1)
                return;

            // Measure current verse
            int spacing = 1;
            int currentHeight = verseRectMapping[proj.currentVerseNum].Height;
            int cy = (int)(((double)Height - currentHeight) / 2.5);

            // Check if this is in the last verse range and don't overshoot by testing with the current verse centered
            bool useLast = false;
            if (scrollerRatio < 1)
            {
                int ix2 = proj.currentVerseNum;
                int cy2 = cy;
                while (ix2 <= proj.bibVerses.Count)
                {
                    int h = verseRectMapping[ix2].Height;
                    cy2 += h + spacing;
                    ix2++;
                    if (cy2 > this.Height) // there is no space left at the end... no need to worry
                        break;
                }

                int remainingspace = this.Height - cy2;
                if (remainingspace > 0)
                    useLast = true;
            }

            if (!useLast)
            {
                // Calculate which verse to snap to by centering the current verse and tiling up
                int ix = proj.currentVerseNum - 1;
                while (ix > 0 && cy > 0) // greedy loop... cy is negative after finished
                {
                    int h = verseRectMapping[ix].Height;
                    cy -= h + spacing;
                    ix--;
                }
                ix++; // fix greedy loop

                SnapToVerse(ix);
            }
            else
                SnapToVerse(proj.bibVerses.Count);
        }
        private void SnapToVerse(int x)
        {
            // Calculate scroller range, position
            int scrollerRange = this.Height - scrollerHeight; // Remember that we are measuring from the center of the scroller
            scrollerPos = (verseRectMapping[x].Y / ((double)totalVerseHeight - this.Height));
            if (scrollerPos > 1)
                scrollerPos = 1; // fix overscroll
        }

        private void proj_refresh(object sender, EventArgs e)
        {
            // Check translation changes && chapter changes
            if (hitMapping != null && hitMapping.Count > 0) // Update only if init
            {
                BibleVerse bv = hitMapping[verseRectMapping[1]];
                if (bv.RefVersion != Program.ConfigHelper.BiblePrimaryTranslation ||
                    currentChapter != proj.bibVerses[1].RefChapter)
                {
                    CalculateItems();
                    this.Refresh();
                }
                else
                {
                    CenterToCurrentVerse();
                    this.Refresh();
                }
            }
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            CalculateItems();
            this.Refresh();
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            /// Function summary:
            /// - Items are layed out based on scroll position
            /// - Items that are closer to the current verse number are painted in a darker color
            /// - Scroll bar is painted based on scrollpos

            if (hitMapping == null || proj == null || proj.bibVerses.Count == 0 || proj.currentVerseNum == -1)
                return;

            int spacing = 1;
            int indent = 16;
            int textwidth = this.Width - spacing - indent;
            Color lightBlue = Color.FromArgb(201, 215, 233);
            Color blue = Color.FromArgb(164, 186, 217);

            // Paint items
            int adjust = HitAdjust;
            for (int i = 1; i <= proj.bibVerses.Count; i++)
            {
                BibleVerse bv = proj.bibVerses[i];
                string paintText = bv.RefVerse + ". " + bv.Text;
                Rectangle r = verseRectMapping[i];
                r.Y -= adjust;
                Rectangle rText = new Rectangle(spacing + indent, r.Y + 2, textwidth, r.Height);

                // Search term support
                StringFormat sf = new StringFormat();
                sf.SetMeasurableCharacterRanges(SearchHelper.FindCharacterRanges(paintText, searchTerms));

                // Paint highlight for highlighted verse
                if (bv == highlightedVerse)
                    e.Graphics.FillRectangle(Brushes.LightGray, r);

                // Paint for verse selection range
                if (bv.RefVerse >= proj.selectionRangeStartVerse && bv.RefVerse <= proj.selectionRangeEndVerse)
                    e.Graphics.FillRectangle(new SolidBrush(lightBlue), r);

                // Paint background for the selected verse
                if (i == proj.currentVerseNum)
                    e.Graphics.FillRectangle(new SolidBrush(blue), r);

                // Fill in the highlights
                foreach (Region rg in e.Graphics.MeasureCharacterRanges(paintText, this.Font, rText, sf))
                    e.Graphics.FillRegion(new SolidBrush(Color.FromArgb(100, Color.Orange)), rg);

                // Calculate text color
                int proximity = Math.Abs(i - proj.currentVerseNum);
                double percentage = (double)proximity / 3; // range of the color fade is 3
                if (percentage > 1)
                    percentage = 1; // Cut off at 3
                int grayValue = (int)(percentage * 90); // 0 is black ... after 3 all is at 90/255 gray
                Color gray = Color.FromArgb(grayValue, grayValue, grayValue);
                SolidBrush sb = new SolidBrush(gray);

                // Text
                e.Graphics.DrawString(paintText, this.Font, sb, rText); // Verse text
            }

            // Paint the scrollbar
            if (scrollerRatio < 1)
            {
                if (scrollerHeight < 42)
                    scrollerHeight = 42;
                int scrollery = ScrollerY;
                Image srollerimg = global::EmpowerPresenter.Properties.Resources.scrollslider;
                Rectangle st = new Rectangle(0, 0, srollerimg.Width, srollerimg.Width);
                Rectangle sm = new Rectangle(0, srollerimg.Width, srollerimg.Width, srollerimg.Height - 2 * srollerimg.Width);
                Rectangle sb = new Rectangle(0, srollerimg.Height - srollerimg.Width, srollerimg.Width, srollerimg.Width);
                e.Graphics.DrawImage(srollerimg, new Rectangle(0, scrollery, 16, srollerimg.Width), st, GraphicsUnit.Pixel);
                e.Graphics.DrawImage(srollerimg, new Rectangle(0, scrollery + srollerimg.Width, 16, scrollerHeight - 2 * srollerimg.Width), sm, GraphicsUnit.Pixel);
                e.Graphics.DrawImage(srollerimg, new Rectangle(0, scrollery + scrollerHeight - srollerimg.Width, 16, srollerimg.Width), sb, GraphicsUnit.Pixel);
                Image dotsimg = global::EmpowerPresenter.Properties.Resources.scrollerdots;
                int dotspos = (int)((scrollerHeight - dotsimg.Height) / 2);
                e.Graphics.DrawImage(dotsimg, new Rectangle(4, dotspos + scrollery, dotsimg.Width, dotsimg.Height),
                    new Rectangle(0, 0, dotsimg.Width, dotsimg.Height), GraphicsUnit.Pixel);
            }
        }
        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            highlightedVerse = null;
            this.Refresh();
        }
        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            
            if (scrolling)
            {
                int yDiff = e.Y - scrollMDownY;
                int scrollerRange = this.Height - scrollerHeight; // Remember that we are measuring from the center of the scroller
                this.scrollerPos = (scrollPosAtMDown + ((double)yDiff / scrollerRange));

                // Bounds check
                if (scrollerPos < 0)
                    scrollerPos = 0;
                if (scrollerPos > 1)
                    scrollerPos = 1;

                this.Refresh();
            }

            // Update cursor
            Rectangle scrollerRect = new Rectangle(0, ScrollerY, 16, scrollerHeight);
            if (e.X > 16)
                this.Cursor = Cursors.Hand;
            else
            {
                if (scrollerRect.Contains(e.Location))
                    this.Cursor = Cursors.Hand;
                else
                    this.Cursor = Cursors.Default;
            }

            // Update the mouse states
            foreach(Rectangle r in hitMapping.Keys)
            {
                Rectangle r2 = r;
                r2.Y -= HitAdjust;
                if (r2.Contains(e.Location))
                {
                    highlightedVerse = hitMapping[r];
                    this.Refresh();
                    return;
                }
            }
            highlightedVerse = null;
            this.Refresh();
        }
        protected override void OnMouseDoubleClick(MouseEventArgs e)
        {
            base.OnMouseDoubleClick(e);

            BibleVerse bv = null;
            foreach (Rectangle r in hitMapping.Keys)
            {
                Rectangle r2 = r;
                r2.Y -= HitAdjust;
                if (r2.Contains(e.Location))
                    bv = hitMapping[r];
            }
            if (bv != null)
            {
                proj.GotoVerse(bv.RefVerse); // This will trigger this control to refresh
                proj.Activate();
            }
        }
        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left)
                return; // Swallow non left clicks

            base.OnMouseDown(e);
            Rectangle scrollerRect = new Rectangle(0, ScrollerY, 16, scrollerHeight);

            // Update cursor
            if (e.X <= 16)
            {
                if (scrollerRect.Contains(e.Location))
                {
                    this.scrolling = true;
                    scrollMDownY = e.Y;
                    scrollPosAtMDown = scrollerPos;
                }
            }

            BibleVerse bv = null;
            foreach (Rectangle r in hitMapping.Keys)
            {
                Rectangle r2 = r;
                r2.Y -= HitAdjust;
                if (r2.Contains(e.Location))
                    bv = hitMapping[r];
            }
            if (bv != null)
                proj.GotoVerse(bv.RefVerse); // This will trigger this control to refresh
        }
        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            this.scrolling = false;
        }
        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);
            if (allowGrabFocus)
                this.Focus(); // Make sure we have focus for scrollwheel to work
        }
        protected override void OnMouseWheel(MouseEventArgs e)
        {
            base.OnMouseWheel(e);
            scrollerPos -= ((double)e.Delta * 22 / 120 / totalVerseHeight);

            // Bounds check
            if (scrollerPos < 0)
                scrollerPos = 0;
            if (scrollerPos > 1)
                scrollerPos = 1;

            this.Refresh();
        }

        private int ScrollerY
        {
            get 
            {
                int scrollerRange = this.Height - scrollerHeight; // Remember that we are measuring from the center of the scroller
                return (int)(scrollerRange * scrollerPos); 
            }
        }
        private int HitAdjust
        {
            get 
            {
                if (scrollerRatio == 1)
                    return 0;
                else
                    return (int)(((double)totalVerseHeight - this.Height) * scrollerPos); 
            }
        }
    }
}