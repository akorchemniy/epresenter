/* ePresenter is licensed under the GPLV3 -- see the 'COPYING' file details.
   Copyright (C) 2006 Alex Korchemniy */
using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;

namespace EmpowerPresenter.Controls
{
    public class PopupSlider : Control, IPopup, IKeyClient
    {
        public event EventHandler ValueChanged;
        private int starty = 8;
        private int sliderh = 70;
        private int cy = 8;
        private int v = 50;
        private Image sliderImg;
        private int vSave = 50;
        private bool bSliding = false;
        private bool imedUpdate = false;

        public PopupSlider()
        {
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            SetStyle(ControlStyles.ResizeRedraw, true);
            SetStyle(ControlStyles.DoubleBuffer, true);
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);

            InitializeComponent();
            sliderImg = global::EmpowerPresenter.Properties.Resources.slideritem;
        }
        private void InitializeComponent()
        {
            this.BackColor = Color.Transparent;
            this.BackgroundImage = global::EmpowerPresenter.Properties.Resources.sliderbg;
            this.MaximumSize = new System.Drawing.Size(27, 95);
            this.MinimumSize = new System.Drawing.Size(27, 95);
            this.Name = "PopupSlider";
            this.Size = new System.Drawing.Size(27, 95);
        }
        protected override void OnMouseUp(System.Windows.Forms.MouseEventArgs e)
        {
            bSliding = false;
            ValueChanged(null, null);                

            this.Invalidate();
            base.OnMouseUp(e);
        }
        protected override void OnMouseDown(System.Windows.Forms.MouseEventArgs e)
        {
            bSliding = true;
            vSave = v;
            cy = e.Y - 5;
            this.Refresh();
            base.OnMouseDown(e);
        }
        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (bSliding)
            {
                cy = e.Y - 5;
                if (cy < starty)
                    cy = starty;
                if (cy > starty + sliderh)
                    cy = starty + sliderh;
                this.Refresh();

                if (imedUpdate && vSave != Value && ValueChanged != null)
                    ValueChanged(null, null);
            }
            base.OnMouseMove(e);
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            e.Graphics.DrawImage(sliderImg, new Rectangle(5, cy, sliderImg.Width, sliderImg.Height), 0, 0, sliderImg.Width, sliderImg.Height, GraphicsUnit.Pixel);
        }

        public bool ImedUpdate
        {
            get { return imedUpdate; }
            set { imedUpdate = value; }
        }
        public int Value
        {
            get 
            {
                int z = cy - starty;
                v = (int)((double)z / sliderh * 100);
                return 100 - v; 
            }
            set 
            {
                int v = value;
                if (v > 100) v = 100; if (v < 0) v = 0;
                v = 100 - v;
                cy = (int)((double)starty + (v * sliderh / 100));
                this.Refresh();
            }
        }

        #region IPopup Members

        public void Deactivate()
        {
            new DelayedTask(200, delegate { this.Hide(); Program.Presenter.UnregisterPopupWindow(this); });
            Program.Presenter.DeactivateKeyClient(this);
        }

        #endregion

        #region IKeyClient Members

        public void ProccesKeys(Keys c, bool exOwner)
        {
            if (c == Keys.Up)
            {
                Value += 5;
                if (imedUpdate && ValueChanged != null)
                    ValueChanged(null, null);
            }
            else if (c == Keys.Down)
            {
                Value -= 5;
                if (imedUpdate && ValueChanged != null)
                    ValueChanged(null, null);
            }
        }

        #endregion
    }
}
