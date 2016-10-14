using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace EmpowerPresenter
{
	[LicenseProvider( typeof( Xheo.Licensing.ExtendedLicenseProvider ) ) ]
	public class Splash : System.Windows.Forms.Form
	{
		private License _license = null;
		public System.Windows.Forms.Label labelTag;
		public Splash()
		{
#if !DEBUG
			try
			{
				_license = LicenseManager.Validate(typeof(Splash), this);
				Xheo.Licensing.ExtendedLicense xl = (Xheo.Licensing.ExtendedLicense)_license;
				Xheo.Licensing.ActivationLimit al = (Xheo.Licensing.ActivationLimit)xl.GetLimit(typeof(Xheo.Licensing.ActivationLimit));
				if (al.ActivationKey != "")
				{
					Presenter.ProfilerHelper3();
					System.Diagnostics.Trace.WriteLine("Product started in activated mode");
				}
			}
			catch(LicenseException)
			{
				Environment.Exit(0);
			}
#endif

			InitializeComponent();
		}
		protected override void Dispose( bool disposing )
		{
			if (disposing)
			{
				if (this.BackgroundImage != null)
					this.BackgroundImage.Dispose();
				if (_license != null)
					_license.Dispose();
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		private void InitializeComponent()
		{
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(Splash));
			this.labelTag = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// labelTag
			// 
			this.labelTag.BackColor = System.Drawing.Color.Transparent;
			this.labelTag.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
			this.labelTag.ForeColor = System.Drawing.Color.White;
			this.labelTag.Location = new System.Drawing.Point(16, 13);
			this.labelTag.Name = "labelTag";
			this.labelTag.Size = new System.Drawing.Size(392, 19);
			this.labelTag.TabIndex = 0;
			this.labelTag.Text = "Loading...";
			this.labelTag.Visible = false;
			// 
			// Splash
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
			this.ClientSize = new System.Drawing.Size(424, 288);
			this.ControlBox = false;
			this.Controls.Add(this.labelTag);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "Splash";
			this.ShowInTaskbar = false;
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.TopMost = true;
			this.Load += new System.EventHandler(this.Splash_Load);
			this.Paint += new System.Windows.Forms.PaintEventHandler(this.Splash_Paint);
			this.ResumeLayout(false);

		}
		#endregion

		
		private void Splash_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
		{
			// Paint a white border around the form
			Graphics g = e.Graphics;
			Rectangle r = new Rectangle(0,0,Width - 1,Height - 1);
			g.DrawRectangle(new Pen(Color.White), r);

			// Paint the text on the form
			g.DrawString(labelTag.Text, labelTag.Font, Brushes.Black, new RectangleF(new PointF(labelTag.Left, labelTag.Top), new SizeF(labelTag.Width, labelTag.Width)));
		}

		
		public void SetText(string text)
		{
			System.Windows.Forms.Application.DoEvents();

			this.Invalidate();

			this.labelTag.Text = text;
			this.labelTag.Refresh();
			
			System.Windows.Forms.Application.DoEvents();
		}

		
		private void Splash_Load(object sender, System.EventArgs e)
		{
			this.Invalidate();
			System.Windows.Forms.Application.DoEvents();
		}
	}
}
