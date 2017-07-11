/* ePresenter is licensed under the GPLV3 -- see the 'COPYING' file details.
   Copyright (C) 2006 Alex Korchemniy */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using Vendisoft.Drawing;

namespace EmpowerPresenter.Controls
{
    public class NinePatchButton : Control
    {
        private bool _highlight;
        private bool _pressed;
        private Image _faceImage;
        private Image _highlightImage;
        private Image _pressedImage;
        private Image _disabled;
        private bool _isSelected;

        public NinePatchButton()
        {
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            SetStyle(ControlStyles.ResizeRedraw, true);
            SetStyle(ControlStyles.DoubleBuffer, true);
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);

            ThemeSlices = new ThemeSlices(5, 5, 5, 5);
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
                Rectangle r = new Rectangle(0,0, Width, Height);
                ThemedDrawing.DrawThemed(e.Graphics, _disabled, r, ThemeSlices);
                return;
            }
            
            // Paint - depending on the state
            if (_pressed || _isSelected)
            {
                if (_pressedImage != null)
                {
                    Rectangle r = new Rectangle(0,0, Width, Height);
                    ThemedDrawing.DrawThemed(e.Graphics, _pressedImage, r, ThemeSlices);
                }
            }
            else
            {
                if (_highlight)
                {
                    if (_highlightImage != null)
                    {
                        Rectangle r = new Rectangle(0,0, Width, Height);
                        ThemedDrawing.DrawThemed(e.Graphics, _highlightImage, r, ThemeSlices);
                    }
                }
                else
                {
                    if (_faceImage != null)
                    {
                        Rectangle r = new Rectangle(0,0, Width, Height);
                        ThemedDrawing.DrawThemed(e.Graphics, _faceImage, r, ThemeSlices);
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
        public ThemeSlices ThemeSlices { get; set; }
        #endregion
    }
}
