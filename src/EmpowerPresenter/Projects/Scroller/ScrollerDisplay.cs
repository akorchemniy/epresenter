/* ePresenter is licensed under the GPLV3 -- see the 'COPYING' file details.
   Copyright (C) 2006 Alex Korchemniy */
using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.IO;

namespace EmpowerPresenter
{
	public class ScrollerDisplay : System.Windows.Forms.Form, IDisplayer
	{
		private EventHandler eh_animTick = null;
		private int BOX_PADDING = 12;

		//////////////////////////////////////////////////////////////////////////////
		public ScrollerDisplay()
		{
			SetStyle(ControlStyles.ResizeRedraw, true);
			SetStyle(ControlStyles.DoubleBuffer, true);
			SetStyle(ControlStyles.AllPaintingInWmPaint, true);
			SetStyle(ControlStyles.UserPaint, true);

			InitializeComponent();
			this.TopMost = true;

			// Set up the animation timer
			tAnimation.Interval = 150;
			eh_animTick = new EventHandler(tAnimation_Tick);
			tAnimation.Tick += eh_animTick;

			this.VisibleChanged += new EventHandler(ScrollerDisplay_VisibleChanged);
			this.FormClosing += new FormClosingEventHandler(ScrollerDisplay_FormClosing);
			this.Paint += new PaintEventHandler(ScrollerDisplay_Paint);
            Properties.Settings.Default.PropertyChanged += new PropertyChangedEventHandler(Default_PropertyChanged);
			Microsoft.Win32.SystemEvents.DisplaySettingsChanged += new EventHandler(SystemEvents_DisplaySettingsChanged);
		}

		protected override void Dispose(bool disposing)
		{
			if (tAnimation != null && eh_animTick != null)
			{
				tAnimation.Tick -= eh_animTick; // clear the event handler reference
				eh_animTick = null;
			}
			base.Dispose(disposing);
		}
		#region Windows Form Designer generated code
		private void InitializeComponent()
		{
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ScrollerDisplay));
			this.SuspendLayout();
			// 
			// ScrollerDisplay
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(8, 20);
			this.BackColor = System.Drawing.Color.Black;
			this.ClientSize = new System.Drawing.Size(576, 41);
			this.Font = new System.Drawing.Font("Arial", 13F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.ForeColor = System.Drawing.Color.White;
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.KeyPreview = true;
			this.Name = "ScrollerDisplay";
			this.ShowInTaskbar = false;
			this.TopMost = true;
			this.ResumeLayout(false);

		}
		#endregion
		
        private void SystemEvents_DisplaySettingsChanged(object sender, EventArgs e)
		{
			UpdateWindowProperties();
		}
        private void Default_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            UpdateWindowProperties();
        }
        private void UpdateWindowProperties()
		{
			// Calc height
			int h = 34;
			if (proj != null)
				h = (int)(proj.font.SizeInPoints * 2);

			// Position at the bottom of the current display
			Screen s = Screen.AllScreens[Program.ConfigHelper.PresentationMonitor];
			this.Size = new Size(s.Bounds.Width, h);
			this.Location = new Point(s.Bounds.X, s.Bounds.Y + s.Bounds.Height - h);
		}
		private void ScrollerDisplay_VisibleChanged(object sender, EventArgs e)
		{
			UpdateWindowProperties();
			this.BringToFront();
		}
		private void ScrollerDisplay_FormClosing(object sender, FormClosingEventArgs e)
		{
			IDisplayManager dispmanager = (IDisplayManager)Program.Presenter;
			dispmanager.RemoveDisplayer(this.GetType());
		}
		private void ScrollerDisplay_Paint(object sender, PaintEventArgs e)
		{
			if (proj == null)
				return;
			int BOX_TOP = 3;

			// Get font
			Brush b = null;
			if (proj.font.Color == Color.Transparent)
				b = Brushes.White;
			else
				b = new SolidBrush(proj.font.Color);
			Font f = new Font(proj.font.FontFamily, proj.font.SizeInPoints, proj.font.FontStyle);
			BOX_PADDING = proj.font.sizeInPoints;
			int LEFT_BOX_WIDTH = (int)e.Graphics.MeasureString(proj.LeftMessage, proj.font.GdiFont).Width;
			int RIGHT_BOX_WIDTH = (int)e.Graphics.MeasureString(proj.RightMessage, proj.font.GdiFont).Width;

			// Find the inside box width
			int insideWidth = this.Width - 2 * BOX_PADDING; // Padding on extreme right and left
			int indentAmount = BOX_PADDING;
			if (proj.LeftMessage != "")
			{
				insideWidth -= LEFT_BOX_WIDTH + BOX_PADDING;
				indentAmount = LEFT_BOX_WIDTH + BOX_PADDING;
			}
			if (proj.RightMessage != "")
				insideWidth -= RIGHT_BOX_WIDTH + BOX_PADDING;

			// Paint the center text
			if (animationStepCount > 0)
			{
				// Draw the trailing string
				e.Graphics.DrawString(proj.Message, f, b, new PointF(indentAmount + messageLength - animationStep * ANIM_STEP_SIZE, BOX_TOP));
				// Draw the leading string
				e.Graphics.DrawString(proj.Message, f, b, new PointF(indentAmount - animationStep * ANIM_STEP_SIZE, BOX_TOP));
			}
			else
			{
				// Draw string in place
				Rectangle centerR = new Rectangle(indentAmount, BOX_TOP, insideWidth, this.Height);
				StringFormat sfCenter = new StringFormat();
				sfCenter.Alignment = StringAlignment.Center;
				e.Graphics.DrawString(proj.Message, f, b, centerR, sfCenter);
			}

			// Paint the side text
			if (proj.LeftMessage != "")
			{
				e.Graphics.FillRectangle(Brushes.Black, new Rectangle(0, 0, BOX_PADDING + LEFT_BOX_WIDTH, this.Height));
				e.Graphics.DrawString(proj.LeftMessage, f, b, new PointF((float)BOX_PADDING / 2, BOX_TOP));
			}
			if (proj.RightMessage != "")
			{
				StringFormat sf = new StringFormat();
				sf.Alignment = StringAlignment.Near;
				e.Graphics.FillRectangle(Brushes.Black, new Rectangle(this.Width - BOX_PADDING - RIGHT_BOX_WIDTH, 0, BOX_PADDING + RIGHT_BOX_WIDTH, this.Height));
				e.Graphics.DrawString(proj.RightMessage, f, b, new Rectangle(this.Width - (int)(BOX_PADDING / 2.0f) - RIGHT_BOX_WIDTH, BOX_TOP, RIGHT_BOX_WIDTH + BOX_PADDING, this.Height), sf);
			}
		}

		// Animation support
		private Timer tAnimation = new Timer();
		private const int ANIM_STEP_SIZE = 5;
		private int animationStep = 0;
		private int animationStepCount = 0;
		private int messageLength = 0;
		private void tAnimation_Tick(object sender, EventArgs e)
		{
			// Force repaint
			this.Refresh();

			// Continue animation
			animationStep++;
			if (animationStep > animationStepCount)
				animationStep = 0;
		}
		private void InitText()
		{
			animationStep = 0;
			animationStepCount = 0;
			tAnimation.Stop();

			Bitmap b = new Bitmap(1, 1);
			Graphics g = Graphics.FromImage(b);

			// Get font
			Font f = new Font(proj.font.FontFamily, proj.font.SizeInPoints, proj.font.FontStyle);
			BOX_PADDING = proj.font.sizeInPoints;

			// Find the inside box width
			int LEFT_BOX_WIDTH = (int)g.MeasureString(proj.LeftMessage, proj.font.GdiFont).Width;
			int RIGHT_BOX_WIDTH = (int)g.MeasureString(proj.RightMessage, proj.font.GdiFont).Width;
			int insideWidth = this.Width - 2 * BOX_PADDING;
			if (proj.LeftMessage != "")
				insideWidth -= LEFT_BOX_WIDTH + BOX_PADDING;
			if (proj.RightMessage != "")
				insideWidth -= RIGHT_BOX_WIDTH + BOX_PADDING;

			// Measure the text for scrolling support
			SizeF szMessageLength = g.MeasureString(proj.Message, f);
			messageLength = (int)szMessageLength.Width;
			if (messageLength > insideWidth)
			{
				animationStepCount = messageLength / ANIM_STEP_SIZE;
				tAnimation.Start();
			}
		}

		#region IDisplayer Members

		private ScrollerProject proj;
		private EventHandler ehRefresh;

		public void AttachProject(IProject proj)
		{
			if (this.proj != proj)
			{
				DetachProject();

				this.proj = (ScrollerProject)proj;
				ehRefresh = new EventHandler(project_Refresh);
				this.proj.Refresh += ehRefresh;
			}
			this.Refresh();
		}
		public void DetachProject()
		{
			if (this.proj != null)
			{
				if (ehRefresh != null)
				{
					this.proj.Refresh -= ehRefresh;
					ehRefresh = null;
				}
			}
			this.proj = null;
		}
		public void ShowDisplay()
		{
			this.Visible = true;
		}
		public void HideDisplay()
		{
			this.Visible = false;
			Program.Presenter.DeactiveDisplayer();
		}
		public void CloseDisplay()
		{
			DetachProject();
			this.Close();
		}
		public bool ExclusiveDisplay() { return false; }
		public bool ResidentDisplay() { return false; }

		private void project_Refresh(object sender, EventArgs e)
		{
			UpdateWindowProperties();
			InitText();
			this.Refresh(); // The new message will be painted automatically
		}

		#endregion

    }
}