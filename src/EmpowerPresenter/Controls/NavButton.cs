using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;

namespace EmpowerPresenter
{
	//[System.ComponentModel.DefaultEvent("ButtonClicked")]
	public class NavButton : Control
	{
		public event EventHandler ButtonClicked;

		private bool _highlight;
		private bool _pressed;
		private Image _overBg;
		private Image _selectedBg;
		private Image _faceImage;
		private Image _highlightImage;
		private Image _pressedImage;
		private Image _disabled;
		private bool _isDisabled;
		private bool _isSelected;
		private StringFormat sf;

		public NavButton()
		{
			SetStyle(ControlStyles.SupportsTransparentBackColor, true);
			SetStyle(ControlStyles.ResizeRedraw, true);
			SetStyle(ControlStyles.DoubleBuffer, true);
			SetStyle(ControlStyles.UserPaint, true);
			SetStyle(ControlStyles.AllPaintingInWmPaint, true);

			sf = new StringFormat();
			sf.Alignment = StringAlignment.Center;
			_overBg = EmpowerPresenter.Properties.Resources.navover;
			_selectedBg = EmpowerPresenter.Properties.Resources.navselected;
			this.Size = _overBg.Size;
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
			_pressed = false;
			this.Invalidate();

			base.OnMouseUp(e);

			if (ButtonClicked != null && !IsDisabled)
				ButtonClicked(this, null);
		}

		protected override void OnMouseDown(System.Windows.Forms.MouseEventArgs e)
		{
			if (IsDisabled)
				return;

			_pressed = true;
			this.Refresh();

			base.OnMouseDown(e);
		}

		protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
		{
			// Set : smooth drawing
			e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

			int xoffset = 23;
			int yoffset = 6;
			int txtoffset = 43;

			// Paint disabled state
			if (_isDisabled && _disabled != null)
			{
				// Paint the image
				RectangleF r = new RectangleF(xoffset,yoffset, _disabled.Width, _disabled.Height);
				e.Graphics.DrawImage(_disabled, r);
				e.Graphics.DrawString(Text, Font, Brushes.Gray, new RectangleF(0, txtoffset, _selectedBg.Width, 16), sf);
				return;
			}
			
			// Paint - depending on the state
			if (_pressed || _isSelected)
			{
				e.Graphics.DrawImage(_selectedBg, 0,0,_selectedBg.Width,_selectedBg.Height);
				if (_pressedImage != null)
				{
					RectangleF r = new RectangleF(xoffset,yoffset, _pressedImage.Width, _pressedImage.Height);
					e.Graphics.DrawImage(_pressedImage, r);
				}
				e.Graphics.DrawString(Text, Font, Brushes.Black, new RectangleF(0, txtoffset, _selectedBg.Width, 16), sf);
			}
			else
			{
				if (_highlight)
				{
					e.Graphics.DrawImage(_overBg, 0,0,_overBg.Width,_overBg.Height);
					if (_highlightImage != null)
					{
						RectangleF r = new RectangleF(xoffset,yoffset, _highlightImage.Width, _highlightImage.Height);
						e.Graphics.DrawImage(_highlightImage, r);
					}
					e.Graphics.DrawString(Text, Font, Brushes.Black, new RectangleF(0, txtoffset, _selectedBg.Width, 16), sf);
				}
				else
				{
					if (_faceImage != null)
					{
						RectangleF r = new RectangleF(xoffset,yoffset, _faceImage.Width, _faceImage.Height);
						e.Graphics.DrawImage(_faceImage, r);
					}
					e.Graphics.DrawString(Text, Font, Brushes.Black, new RectangleF(0, txtoffset, _selectedBg.Width, 16), sf);
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

				this.Invalidate();
			}
		}
		public Image Disabled
		{
			get
			{
				return _disabled;
			}
			set
			{
				_disabled = value;

				this.Invalidate();
			}
		}
		public Image Normal
		{
			get
			{
				return _faceImage;
			}
			set
			{
				if (value != null)
					this._faceImage = value;

				this.Invalidate();
			}
		}
		public Image Over
		{
			get
			{
				return _highlightImage;
			}
			set
			{
				if (value != null)
					this._highlightImage = value;

				this.Invalidate();
			}
		}
		public Image Pressed
		{
			get
			{
				return _pressedImage;
			}
			set
			{
				if (value != null)
					this._pressedImage = value;

				this.Invalidate();
			}
		}
		public bool Highlighted
		{
			get
			{
				return _highlight;
			}
			set
			{
				_highlight = value;

				// Refresh the control
				Invalidate();
			}
		}
		public bool IsPressed
		{
			get
			{
				return _pressed;
			}
			set
			{
				_pressed = value;

				// Refresh the control
				Invalidate();
			}
		}
		public bool IsDisabled
		{
			get
			{
				return _isDisabled;
			}
			set
			{
				_isDisabled = value;

				Invalidate();
			}
		}
		#endregion

		public void PerformClick()
		{
			if (this.ButtonClicked != null)
				ButtonClicked(this, null);
		}
	}
}
