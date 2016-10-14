/* ePresenter is licensed under the GPLV3 -- see the 'COPYING' file details.
   Copyright (C) 2006 Alex Korchemniy */
using System;
using System.Drawing.Imaging;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Windows.Forms;

namespace EmpowerPresenter.Controls.Photos
{
	[Designer(typeof(PhotoContainerDesigner))]
	public class PhotoPreviewContainer : Panel	
	{
		private int columnCounter = 0;
		public event EventHandler ItemClicked;
		public event EventHandler ItemDoubleClicked;
		private string cat = "";

		public PhotoPreviewContainer()
		{
			this.AutoScroll = true;
			this.ControlAdded += new ControlEventHandler(PhotoPreviewContainer_ControlAdded);
			this.ControlRemoved += new ControlEventHandler(PhotoPreviewContainer_ControlRemoved);
		}
		public void LoadImages(ArrayList piArray, string cat)
		{
			this.cat = cat;
			this.Controls.Clear();

			piArray.Reverse(); // Items are added in reverse order
			foreach(PhotoInfo i in piArray)
			{
				PhotoPreviewItem ppi = new PhotoPreviewItem();
				ppi.PhotoInfo = i;
				this.Controls.Add(ppi);
			}
			this.LayoutControls();
		}
		public void LayoutControls()
		{
			int ax = this.AutoScrollPosition.X + 10; // 10 pixel adjustment
			int ay = this.AutoScrollPosition.Y + 10; // 10 pixel adjustment
			int xstep = 222;
			int ystep = 168;
			int cx_step = 0;
			int cy_step = 0;

			foreach(Control c in this.Controls)
			{
				if (c.GetType() == typeof(PhotoPreviewItem))
				{
					c.Location = new Point(ax + xstep * cx_step, ay + ystep * cy_step);
					cx_step++;
					if (this.Width < ((cx_step+1) * 222 + 10))
					{
						columnCounter = cx_step;
						cx_step = 0;
						cy_step++;
					}
				}
			}
		}
		protected override void OnMouseEnter(EventArgs e)
		{
			this.Focus();
			base.OnMouseEnter (e);
		}
		protected override void OnResize(EventArgs e)
		{
			base.OnResize (e);

			// Double check for layouts
			double col = ((double)Width - 10) / 222;
			if ((int)col != columnCounter)
				LayoutControls();
		}
		private void PhotoPreviewContainer_ControlAdded(object sender, ControlEventArgs e)
		{
			if (e.Control is PhotoPreviewItem)
			{
				((PhotoPreviewItem)e.Control).ItemClicked += new EventHandler(PhotoPreviewContainer_ItemClicked);
				((PhotoPreviewItem)e.Control).DoubleClick += new EventHandler(PhotoPreviewContainer_DoubleClick);
			}
		}

		void PhotoPreviewContainer_DoubleClick(object sender, EventArgs e)
		{
			// Buble events up
			if (this.ItemDoubleClicked != null)
				this.ItemDoubleClicked(sender, null);
		}
		private void PhotoPreviewContainer_ControlRemoved(object sender, ControlEventArgs e)
		{
			this.LayoutControls();
		}
		private void PhotoPreviewContainer_ItemClicked(object sender, EventArgs e)
		{
			// Buble events up
			if (this.ItemClicked != null)
				this.ItemClicked(sender, null);
		}

		public string CurrentCategory
		{get{return cat;}set{cat=value;}}
	}
	internal class PhotoContainerDesigner : System.Windows.Forms.Design.ParentControlDesigner
	{
		public override System.ComponentModel.Design.DesignerVerbCollection Verbs
		{
			get
			{
				DesignerVerbCollection vc = new DesignerVerbCollection(new DesignerVerb[]{
																							 new DesignerVerb("Perform Layout", new EventHandler(OnPerformLayout))																						 });
				return vc;
			}
		}

		public void OnPerformLayout(object sender, EventArgs e)
		{
			((PhotoPreviewContainer)this.Control).LayoutControls();
		}
	}
}
