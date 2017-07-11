/* ePresenter is licensed under the GPLV3 -- see the 'COPYING' file details.
   Copyright (C) 2006 Alex Korchemniy */
using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using EmpowerPresenter.Controls;

namespace Vendisoft.Controls
{
    public class ImageButton : Control
    {
        private bool _highlight;
        private bool _pressed;
        private Image _faceImage;
        private Image _highlightImage;
        private Image _pressedImage;
        private Image _disabled;
        private bool _isSelected;

        public ImageButton()
        {
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            SetStyle(ControlStyles.ResizeRedraw, true);
            SetStyle(ControlStyles.DoubleBuffer, true);
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
        }

        #region Control Events
        protected override void OnMouseEnter(System.EventArgs e)
        {
            _pressed = false;
            _highlight = true;
            this.Refresh();

            // pass to the base
            base.OnMouseEnter(e);
        }
        protected override void OnMouseLeave(System.EventArgs e)
        {
            _pressed = false;
            _highlight = false;
            this.Refresh();

            // pass to the base
            base.OnMouseLeave(e);
        }
        protected override void OnMouseUp(System.Windows.Forms.MouseEventArgs e)
        {
            base.OnMouseUp(e);

            _pressed = false;
            this.Invalidate();
        }
        protected override void OnMouseDown(System.Windows.Forms.MouseEventArgs e)
        {
            base.OnMouseDown(e);

            if (!Enabled)
                return;

            _pressed = true;
            this.Refresh();
        }
        protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
        {
            // Set : smooth drawing
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            // Paint disabled state
            if (!this.Enabled && _disabled != null)
            {
                // Paint the image
                RectangleF r = new RectangleF(0,0, _disabled.Width, _disabled.Height);
                e.Graphics.DrawImage(_disabled, r);
                return;
            }
            
            // Paint - depending on the state
            if (_pressed || _isSelected)
            {
                if (_pressedImage != null)
                {
                    RectangleF r = new RectangleF(0,0, _pressedImage.Width, _pressedImage.Height);
                    e.Graphics.DrawImage(_pressedImage, r);
                }
            }
            else
            {
                if (_highlight)
                {
                    if (_highlightImage != null)
                    {
                        RectangleF r = new RectangleF(0,0, _highlightImage.Width, _highlightImage.Height);
                        e.Graphics.DrawImage(_highlightImage, r);
                    }
                }
                else
                {
                    if (_faceImage != null)
                    {
                        RectangleF r = new RectangleF(0,0, _faceImage.Width, _faceImage.Height);
                        e.Graphics.DrawImage(_faceImage, r);
                    }
                }
            }
        }
        #endregion

        #region Public Properties
        public bool IsSelected
        {
            get
            {
                return _isSelected;
            }
            set
            {
                _isSelected = value;

                if (Enabled)this.Invalidate();
            }
        }
        public Image DisabledImg
        {
            get
            {
                return _disabled;
            }
            set
            {
                _disabled = value;

                if (Enabled)this.Invalidate();
            }
        }
        public Image NormalImg
        {
            get
            {
                return _faceImage;
            }
            set
            {
                if (value != null)
                    this._faceImage = value;

                if (Enabled)this.Invalidate();
            }
        }
        public Image OverImg
        {
            get
            {
                return _highlightImage;
            }
            set
            {
                if (value != null)
                    this._highlightImage = value;

                if (Enabled)this.Invalidate();
            }
        }
        public Image PressedImg
        {
            get
            {
                return _pressedImage;
            }
            set
            {
                if (value != null)
                    this._pressedImage = value;

                if (Enabled)this.Invalidate();
            }
        }
        public bool IsHighlighted
        {
            get{return _highlight;}
            set
            {
                _highlight = value;

                // Refresh the control
                if (Enabled)Invalidate();
            }
        }
        public bool IsPressed
        {
            get{return _pressed;}
            set
            {
                _pressed = value;

                // Refresh the control
                Invalidate();
            }
        }
        #endregion
    }

    public class NavButton : NinePatchButton
    {
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            e.Graphics.DrawString(this.Text, this.Font, Brushes.Black, new PointF(28, 6));

            // Draw the indicator
            if (this.Tag as string != null)
            {
                Image i = null;
                switch ((string)this.Tag) // Select the image based on the tag value
                {
                    case "0":
                        i = this.Enabled ? global::EmpowerPresenter.Properties.Resources.bib16 : global::EmpowerPresenter.Properties.Resources.bib16d;
                        break;
                    case "1":
                        i = this.Enabled ? global::EmpowerPresenter.Properties.Resources.music16 : global::EmpowerPresenter.Properties.Resources.music16d;
                        break;
                    case "2":
                        i = this.Enabled ? global::EmpowerPresenter.Properties.Resources.ppt16 : global::EmpowerPresenter.Properties.Resources.ppt16d;
                        break;
                    case "3":
                        i = this.Enabled ? global::EmpowerPresenter.Properties.Resources.video16 : global::EmpowerPresenter.Properties.Resources.video16d;
                        break;
                    case "4":
                        i = this.Enabled ? global::EmpowerPresenter.Properties.Resources.image : global::EmpowerPresenter.Properties.Resources.image;
                        break;
                    case "5":
                        i = this.Enabled ? global::EmpowerPresenter.Properties.Resources.comment_edit : global::EmpowerPresenter.Properties.Resources.comment_edit;
                        break;
                    case "6":
                        i = this.Enabled ? global::EmpowerPresenter.Properties.Resources.comment_edit : global::EmpowerPresenter.Properties.Resources.comment_edit;
                        break;
                }
                if (i != null)
                    e.Graphics.DrawImage(i, new Rectangle(6, 6, i.Width, i.Height), new Rectangle(0, 0, i.Width, i.Height), GraphicsUnit.Pixel);
            }
        }
    }
}
