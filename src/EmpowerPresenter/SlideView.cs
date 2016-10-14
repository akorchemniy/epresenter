using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace EmpowerPresenter
{
	/// <summary>
	/// Summary description for SlideView.
	/// </summary>
	public class SlideView : System.Windows.Forms.Form
	{
		private System.ComponentModel.Container components = null;

		public SlideView()
		{
			InitializeComponent();
		}

		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		private void InitializeComponent()
		{
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.AutoScroll = true;
			this.ClientSize = new System.Drawing.Size(720, 547);
			this.Name = "";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "";
			this.Resize += new System.EventHandler(this.SlideView_Resize);
			this.Closing += new System.ComponentModel.CancelEventHandler(this.SlideView_Closing);

		}
		#endregion

		public void BindToSlideShow(ISlideShowBase slideShow)
		{
			// 300,225
			while(this.Controls.Count > 0)
			{
				PictureBox c = (PictureBox)this.Controls[0];

				if (c.Image != null)
					c.Image.Dispose();

				c.Dispose();
			}

			this.Controls.Clear();

			if (slideShow == null)
				return;

			int x = 5; int y = 5;

			PictureBox pb;
			while(slideShow.IsFinished == false)
			{
				pb = new PictureBox();
				pb.Size = new Size(412, 309);
				pb.Location = new Point(x,y);
				pb.SizeMode = PictureBoxSizeMode.StretchImage;
				pb.BorderStyle = BorderStyle.FixedSingle;
				pb.Image = slideShow.GetCurrentSlide();
				this.Controls.Add(pb);
				slideShow.NextSlide();
				x = x + pb.Width + 5;
				if (x + pb.Width > this.ClientSize.Width)
				{
					x = 5;
					y = y + pb.Height + 5;
				}
			}

			slideShow = null;
			this.Show();
		}

		public void GoToLast()
		{
			this.ScrollControlIntoView(this.Controls[this.Controls.Count -1]);
		}
		private void SlideView_Resize(object sender, System.EventArgs e)
		{
			int x = 5;
			int y = 5;
			foreach(PictureBox pb in this.Controls)
			{
				pb.Location = new Point(x,y);
				x = x + pb.Width + 5;
				if (x + pb.Width > this.ClientSize.Width)
				{
					x = 5;
					y = y + pb.Height + 5;
				}
			}
		}

		private void SlideView_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			e.Cancel = true;
			this.Hide();
			BindToSlideShow(null);
		}
	}
}
