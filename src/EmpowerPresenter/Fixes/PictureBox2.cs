using System;
using System.Windows.Forms;

namespace System.Windows.Forms
{
	/// <summary>
	/// Summary description for PictureBox2.
	/// </summary>
	public class PictureBox2 : PictureBox
	{
		public PictureBox2()
		{
			SetStyle(ControlStyles.DoubleBuffer, true);
			SetStyle(ControlStyles.AllPaintingInWmPaint, true);
			SetStyle(ControlStyles.UserPaint, true);
			SetStyle(ControlStyles.Opaque, true);
			this.Paint += new PaintEventHandler(PictureBox2_Paint);
		}

		private void PictureBox2_Paint(object sender, PaintEventArgs e)
		{
			//
		}
	}
}
