/* ePresenter is licensed under the GPLV3 -- see the 'COPYING' file details.
   Copyright (C) 2006 Alex Korchemniy */
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace EmpowerPresenter
{
    public class SongPreviewControl : Control
    {
        private SongProject proj;
        private EventHandler ehRefresh;
        private Dictionary<int, string> slideText = null;
        private Dictionary<int, Rectangle> slideRectMapping = new Dictionary<int, Rectangle>();
        private int highlightedItem;

        // Scrolling
        private double scrollerPos = 0; // This is a percetage of the page
        private double scrollerRatio = 1;
        private int scrollerHeight = 0;
        private int totalSlidesHeight = 0;
        private bool scrolling = false;
        private int scrollMDownY = 0;
        private double scrollPosAtMDown = 0; // scroll position at mouse down

        ////////////////////////////////////////////////////////////////////////////////
        public SongPreviewControl()
        {
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            SetStyle(ControlStyles.ResizeRedraw, true);
            SetStyle(ControlStyles.DoubleBuffer, true);
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.UserMouse, true);
        }
        public void AttachProject(SongProject proj)
        {
            if (proj != this.proj)
            {
                DetachProject();

                this.proj = proj;
                ehRefresh = new EventHandler(proj_refresh);
                this.proj.Refresh += ehRefresh;

                RefreshData();
            }
        }
        public void DetachProject()
        {
            if (proj != null && ehRefresh != null)
                proj.Refresh -= ehRefresh;
            this.proj = null;
            ehRefresh = null;

            // Reset variables
            if (slideText != null)
                slideText.Clear();
            slideText = null;
            slideRectMapping.Clear();
            highlightedItem = -1;
            scrollerPos = 0;
            scrollerRatio = 1;
            scrollerHeight = 0;
        }
        public void RefreshData()
        {
            slideText = proj.GetInlineRepresentation();
            CalculateItems();
            this.Refresh();
        }
        private void CalculateItems()
        {
            /// Function summary:
            /// We need to get the rectangles for each verse and store them for hit detection
            /// and easy painting.

            slideRectMapping.Clear();
            if (proj == null || proj.songSlides.Count == 0 || proj.currentSlideNum == -1 || slideText == null)
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
            int ixCurrent = 0;
            while (ixCurrent < slideText.Count)
            {
                string t = slideText[ixCurrent];
                int itemH = (int)g.MeasureString(t, this.Font, textWidth).Height + textPadding;
                Rectangle r = new Rectangle(indent, cy, this.Width - indent, itemH);
                slideRectMapping.Add(ixCurrent, r);
                cy += itemH + spacing;
                ixCurrent++;
            }

            // Calculate scroller ratio
            Rectangle lastSlide = slideRectMapping[slideText.Count - 1];
            totalSlidesHeight = lastSlide.Y + lastSlide.Height;
            double sr = (double)Height / totalSlidesHeight;
            if (sr < 1)
            {
                scrollerRatio = sr;
                scrollerHeight = (int)(Height * sr);
            }
            else
                scrollerRatio = 1;

            CenterToCurrentSlide();
        }
        private void CenterToCurrentSlide()
        {
            /// Function summary:
            /// This function is responsible for centering the current verse...
            /// There need to be two built in exceptions: too close to the start or end

            if (proj == null || proj.songSlides.Count == 0 || proj.currentSlideNum == -1 || slideText == null)
                return;

            // Measure current verse
            int spacing = 1;
            int currentHeight = slideRectMapping[proj.currentSlideNum].Height;
            int cy = (int)(((double)Height - currentHeight) / 2.5);

            // Check if this is in the last slide range and don't overshoot by testing with the current slide centered
            bool useLast = false;
            if (scrollerRatio < 1)
            {
                int ix2 = proj.currentSlideNum;
                int cy2 = cy;
                while (ix2 < slideText.Count)
                {
                    int h = slideRectMapping[ix2].Height;
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
                // Calculate which verse to snap to by centering the current slide and tiling up
                int ix = proj.currentSlideNum - 1;
                while (ix > -1 && cy > 0) // greedy loop... cy is negative after finished
                {
                    int h = slideRectMapping[ix].Height;
                    cy -= h + spacing;
                    ix--;
                }
                ix++; // fix greedy loop

                SnapToSlide(ix);
            }
            else
                SnapToSlide(slideText.Count - 1);
        }
        private void SnapToSlide(int x)
        {
            if (slideText == null)
                return;

            // Calculate scroller range, position
            int scrollerRange = this.Height - scrollerHeight; // Remember that we are measuring from the center of the scroller
            scrollerPos = (slideRectMapping[x].Y / ((double)totalSlidesHeight - this.Height));
            if (scrollerPos > 1)
                scrollerPos = 1; // fix overscroll
        }

        private void proj_refresh(object sender, EventArgs e)
        {
            CenterToCurrentSlide();
            this.Refresh();
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

            if (proj == null || slideText == null || slideText.Count == 0 || proj.currentSlideNum == -1)
                return;

            int spacing = 1;
            int indent = 16;
            int textwidth = this.Width - spacing - indent;

            // Paint items
            int adjust = HitAdjust;
            for (int i = 0; i < slideText.Count; i++)
            {
                Rectangle r = slideRectMapping[i];
                r.Y -= adjust;
                string t = slideText[i];
                Rectangle r2 = r; r2.X = 13; // offset the text painting

                // Paint highlight for highlighted verse
                if (i == highlightedItem)
                    e.Graphics.FillRectangle(Brushes.LightGray, r);

                // Paint background for the selected verse
                if (i == proj.currentSlideNum)
                    e.Graphics.FillRectangle(Brushes.LightSteelBlue, r);

                // Calculate text color
                int proximity = Math.Abs(i - proj.currentSlideNum);
                double percentage = (double)proximity / 3; // range of the color fade is 3
                if (percentage > 1)
                    percentage = 1; // Cut off at 3
                int grayValue = (int)(percentage * 90); // 0 is black ... after 3 all is at 90/255 gray
                Color gray = Color.FromArgb(grayValue, grayValue, grayValue);
                SolidBrush sb = new SolidBrush(gray);

                // Text
                e.Graphics.DrawString(t, this.Font, sb, new Rectangle(spacing + indent, r.Y + 2, textwidth, r.Height)); // Verse text
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
            highlightedItem = -1;
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
            foreach(int i in slideRectMapping.Keys)
            {
                Rectangle r2 = slideRectMapping[i];
                r2.Y -= HitAdjust;
                if (r2.Contains(e.Location))
                {
                    highlightedItem = i;
                    this.Refresh();
                    return;
                }
            }
            highlightedItem = -1;
            this.Refresh();
        }
        protected override void OnMouseDoubleClick(MouseEventArgs e)
        {
            base.OnMouseDoubleClick(e);

            int slidenum = -1;
            foreach (int i in slideRectMapping.Keys)
            {
                Rectangle r2 = slideRectMapping[i];
                r2.Y -= HitAdjust;
                if (r2.Contains(e.Location))
                    slidenum = i;
            }
            if (slidenum > -1)
            {
                proj.GotoSlide(slidenum); // This will trigger this control to refresh
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

            int slidenum = -1;
            foreach (int i in slideRectMapping.Keys)
            {
                Rectangle r2 = slideRectMapping[i];
                r2.Y -= HitAdjust;
                if (r2.Contains(e.Location))
                    slidenum = i;
            }
            if (slidenum > -1)
                proj.GotoSlide(slidenum); // This will trigger this control to refresh
        }
        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            this.scrolling = false;
        }
        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);
            this.Focus(); // Make sure we have focus for scrollwheel to work
        }
        protected override void OnMouseWheel(MouseEventArgs e)
        {
            base.OnMouseWheel(e);
            scrollerPos -= ((double)e.Delta * 22 / 120 / totalSlidesHeight);

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
                    return (int)(((double)totalSlidesHeight - this.Height) * scrollerPos); 
            }
        }
    }
}