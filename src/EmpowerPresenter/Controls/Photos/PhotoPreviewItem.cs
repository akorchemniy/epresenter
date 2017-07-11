/* ePresenter is licensed under the GPLV3 -- see the 'COPYING' file details.
   Copyright (C) 2006 Alex Korchemniy */
using System;
using System.Drawing.Imaging;
using System.Drawing;
using System.Windows.Forms;

namespace EmpowerPresenter.Controls.Photos
{
    public class PhotoPreviewItem : Control
    {
        public event EventHandler ItemClicked;
        protected bool hot = false;
        protected bool selected = false;
        private PhotoInfo pi;

        public PhotoPreviewItem()
        {
            SetStyle(ControlStyles.ResizeRedraw, true);
            SetStyle(ControlStyles.DoubleBuffer, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.UserPaint, true);

            this.Width = 222; // 11 pix padding (211 img)
            this.Height = 160; // 2 pix padding (158 img)
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter (e);
            if (!Parent.Focused)
                Parent.Focus();
            Cursor = Cursors.Hand;
            hot = true;
            this.Invalidate();
        }
        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave (e);
            Cursor = Cursors.Default;
            hot = false;
            this.Invalidate();
        }
        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (ItemClicked != null)
                ItemClicked(this, null);
            base.OnMouseDown (e);
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint (e);

            DrawBase(e.Graphics);
        }
        protected override void OnClick(EventArgs e)
        {
            base.OnClick (e);
            this.selected = true;
            this.Invalidate();
        }
        protected virtual void DrawBase(Graphics g)
        {
            // Paint image
            if (pi != null && pi.Preview != null)
            {
                if (pi.Preview.Height != 158 || pi.Preview.Width != 211)
                    throw(new FormatException("Images should be 211x158 pixels. Strict rule"));

                g.DrawImage(pi.Preview, new Rectangle(0,0,211,158), 0,0,211,158, GraphicsUnit.Pixel);
            }

            // Draw border
            if (selected)
            {
                Pen p = new Pen(Color.Black, 3);
                p.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
                g.DrawRectangle(p, 1, 1, 211 - 3, 158 - 3);
            }
            else
            {
                if (hot)
                    g.DrawRectangle(Pens.White, 0, 0, 211, 158);
                else
                    g.DrawRectangle(Pens.Black, 0, 0, 211, 158);
            }

            #region Paint name
//            if (pd != null && paintName)
//            {
//                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
//                Font f = new Font("Arial", 10F, FontStyle.Bold | FontStyle.Italic);
//                string paintText = pd.name;
//                if (pd.name == "")
//                    paintText = System.IO.Path.GetFileNameWithoutExtension(pd.filename);
//                SizeF sz = g.MeasureString(paintText, f);
//                g.DrawString(paintText, f, Brushes.White, 
//                    new RectangleF((blockw - sz.Width)/2, 144, sz.Width, sz.Height));
//            }
            #endregion
        }

        public bool Selected
        {
            get{return selected;}
            set
            {
                selected = value;
                this.Invalidate();
            }
        }
        public PhotoInfo PhotoInfo
        {
            get{return pi;}
            set
            {
                pi = value;
                this.Invalidate();
            }
        }
    }
}
