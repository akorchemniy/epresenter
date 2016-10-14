/* ePresenter is licensed under the GPLV3 -- see the 'COPYING' file details.
   Copyright (C) 2006 Alex Korchemniy */
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Drawing.Drawing2D;

namespace EmpowerPresenter
{
	public partial class AnouncementProjectView : UserControl, IController, IDragDropClient
	{
		private AnouncementProject proj;
		private EventHandler ehRefresh;
		private GfxContext currentCtx;
		private Dictionary<TextRegionCtrl, EventHandler> moveHandlers = new Dictionary<TextRegionCtrl, EventHandler>();
		private int suspendUpdate = 0;
		private TextRegionCtrl currentRegion = null;
		private double currentScaleFactor = 0;
		private int currentXOffset = 0;
		private EmpowerPresenter.Controls.PopupSlider slider;

		////////////////////////////////////////////////////////////////////////////////
		public AnouncementProjectView()
		{
			InitializeComponent();
			if (Program.ConfigHelper.CurrentLanguage == "ru-RU")
			{
				btnActivate.DisabledImg = global::EmpowerPresenter.Properties.Resources.activate_btndis_ru;
				btnActivate.NormalImg = global::EmpowerPresenter.Properties.Resources.activate_btn_ru;
				btnActivate.OverImg = global::EmpowerPresenter.Properties.Resources.activate_btnover_ru;
				btnActivate.PressedImg = global::EmpowerPresenter.Properties.Resources.activate_btn_ru;
			}

			// Register exclusive key owners
			Program.Presenter.RegisterExKeyOwnerControl(txtMessage);
		}
		internal void RefreshUI()
		{
			currentCtx = proj.GetCurrentGfxContext();
			pnlPreviewImage.Refresh();

			// Update the region editing controls
			if (pnlPreviewImage.Controls.Count != proj.data.lTextRegions.Count)
			{
				DetachRegionControls();
				AttachRegionControls();
				LayoutRegionControls();
			}
			else
				LayoutRegionControls();
		}
		private void AttachRegionControls()
		{
			proj.data.lTextRegions.Reverse();
			foreach (GfxTextRegion tr in proj.data.lTextRegions)
			{ 
				TextRegionCtrl ctl = new TextRegionCtrl();
				ctl.textRegion = tr;

				// Attach events
				EventHandler eh_Move = new EventHandler(ctl_Move);
				moveHandlers.Add(ctl, eh_Move);
				ctl.FinishedMove += eh_Move;

				pnlPreviewImage.Controls.Add(ctl);
			}
		}
		private void DetachRegionControls()
		{
			foreach (TextRegionCtrl c in pnlPreviewImage.Controls)
				c.FinishedMove -= moveHandlers[c];
			moveHandlers.Clear();
			pnlPreviewImage.Controls.Clear();
		}
		private void LayoutRegionControls()
		{
			if (suspendUpdate > 0)
				return;
			suspendUpdate++;

			// Calculate origin (same routine as the paint (smart fit))
			Size nativeSize = DisplayEngine.NativeResolution.Size;
			currentXOffset = 0;
			currentScaleFactor = (double)pnlPreviewImage.Width / nativeSize.Width; // try the standard
			if (pnlPreviewImage.Height - (int)(nativeSize.Height * currentScaleFactor) < 0) // fit the other way
			{
				currentScaleFactor = (double)pnlPreviewImage.Height / nativeSize.Height;
				int destWidth = (int)(nativeSize.Width * currentScaleFactor);
				currentXOffset = (int)((pnlPreviewImage.Width - destWidth) / 2); // offset x
			}

			// Update the location of all the text region controls
			foreach (TextRegionCtrl c in pnlPreviewImage.Controls)
			{
				RectangleF b = c.textRegion.bounds;
				c.Location = new Point((int)(currentXOffset + b.X * currentScaleFactor), (int)(b.Y * currentScaleFactor));
				c.Size = new Size((int)(b.Width * currentScaleFactor), (int)(b.Height * currentScaleFactor));
			}
			suspendUpdate--;
		}
		private void UpdateInfoLabel()
		{
			lblInfo.Text = Loc.Get("Preview:");
		}

		private void ctl_Move(object sender, EventArgs e)
		{
			if (suspendUpdate > 0)
				return;

			suspendUpdate++;
			currentRegion = (TextRegionCtrl)sender;
			txtMessage.Text = currentRegion.textRegion.message;
			txtMessage.Enabled = true;
			btnChangeFont.Enabled = true;

			// Select the current region, deselect others
			foreach (TextRegionCtrl c in pnlPreviewImage.Controls)
				if (c != currentRegion)
					c.Selected = false;
			currentRegion.Selected = true;

			// Update the location
			Rectangle b = currentRegion.Bounds;
			b.X = (int)(((double)b.X - currentXOffset) / currentScaleFactor);
			b.Y = (int)((double)b.Y / currentScaleFactor);
			b.Width = (int)((double)b.Width / currentScaleFactor);
			b.Height = (int)((double)b.Height / currentScaleFactor);
			currentRegion.textRegion.bounds = b;
			proj.RefreshUI();
			suspendUpdate--;
		}
		private void imgBtn_MouseEnter(object sender, EventArgs e)
		{
			lblInfo.Text = Loc.Get(((Control)sender).Tag.ToString());
		}
		private void imgBtn_MouseLeave(object sender, EventArgs e)
		{
			UpdateInfoLabel();
		}
		private void pnlPreviewImage_Paint(object sender, PaintEventArgs e)
		{
			if (currentCtx == null)
				return;

			Size nativeSize = DisplayEngine.NativeResolution.Size;

			// Smart position the preivew image in the panel
			float scaleFactorW = (float)pnlPreviewImage.Width / nativeSize.Width;
			if (pnlPreviewImage.Height - (int)(nativeSize.Height * scaleFactorW) >= 0) // Try first by width
			{
				Matrix mx = new Matrix(scaleFactorW, 0, 0, scaleFactorW, -(scaleFactorW), -(scaleFactorW));
				e.Graphics.Transform = mx;
				DisplayEngine.PaintGfxContext(currentCtx, e.Graphics, true);
			}
			else // Paint by height
			{
				float scaleFactorH = (float)pnlPreviewImage.Height / nativeSize.Height;
				float destWidth = (float)(nativeSize.Width * scaleFactorH);
				float cx = (float)((pnlPreviewImage.Width - destWidth) / 2);
				Matrix mx = new Matrix(scaleFactorH, 0, 0, scaleFactorH, -(scaleFactorH), -(scaleFactorH));
				e.Graphics.Transform = mx;
				e.Graphics.TranslateTransform(cx / scaleFactorH, 0); // div to adjust for transform
				DisplayEngine.PaintGfxContext(currentCtx, e.Graphics, true);
			}
		}
		private void pnlPreviewImage_Resize(object sender, EventArgs e)
		{
			pnlPreviewImage.Refresh();
			LayoutRegionControls();
		}
		private void btnImage_Click(object sender, EventArgs e)
		{
#if DEMO
			new DemoVersionOnly("Changing background").ShowDialog();
#else
			ImageSelection images = new ImageSelection();
			if (DialogResult.OK == images.ShowDialog())
			{
				proj.UpdateBackground(images.SelectedItem.ImageId);
				proj.RefreshUI();
				proj.dirty = true;
			}
#endif
		}
		private void btnAddReg_Click(object sender, EventArgs e)
		{
			// Add a new region
			PresenterFont f = new PresenterFont();
			f.VerticalAlignment = VerticalAlignment.Top;
			f.HorizontalAlignment = HorizontalAlignment.Left;
			GfxTextRegion r = new GfxTextRegion(new Rectangle(30, 30, 300, 150), f, "Sample message");
			proj.data.lTextRegions.Add(r);
			proj.RefreshUI();
			
			// Set the region as current
			foreach (TextRegionCtrl c in pnlPreviewImage.Controls)
			{
				if (c.textRegion == r)
					currentRegion = c;
				c.Selected = c.textRegion == r;
			}
			txtMessage.Text = r.message;
			txtMessage.Enabled = true;
			btnChangeFont.Enabled = true;
			proj.dirty = true;
		}
		private void btnRemReg_Click(object sender, EventArgs e)
		{
			// Remove the current region
			if (currentRegion != null && proj != null && proj.data.lTextRegions.Contains(currentRegion.textRegion))
			{
				proj.data.lTextRegions.Remove(currentRegion.textRegion);
				currentRegion = null;
				proj.RefreshUI();

				txtMessage.Text = "";
				txtMessage.Enabled = false;
				btnChangeFont.Enabled = false;
				proj.dirty = true;
			}
		}
		private void btnChangeFont_Click(object sender, EventArgs e)
		{
#if DEMO
			new DemoVersionOnly("Changing font").ShowDialog();
#else
			if (currentRegion == null)
				return;

			FontSelection fsForm = new FontSelection();
			fsForm.LoadFont(currentRegion.textRegion.font);
			if (fsForm.ShowDialog() == DialogResult.OK)
			{
				currentRegion.textRegion.font = (PresenterFont)fsForm.PresenterFont.Clone();
				proj.RefreshUI();
				proj.dirty = true;
			}
#endif
		}
		private void btnBrightness_Click(object sender, EventArgs e)
		{
			if (slider != null && slider.Visible == true)
				slider.Deactivate();
			else
			{
				if (slider == null)
				{
					slider = new EmpowerPresenter.Controls.PopupSlider();
					slider.ImedUpdate = true;
					slider.ValueChanged += new EventHandler(slider_ValueChanged);
					this.Controls.Add(slider);
				}
				Point ptLoc = this.PointToScreen(new Point(btnBrightness.Location.X - 1, btnBrightness.Location.Y + 25));
				slider.Value = (int)(((double)proj.Opacity + 255) * 100 / 512);
				Program.Presenter.ShowPopupWindow(slider, ptLoc);
				Program.Presenter.ActivateKeyClient(slider);
			}
		}
		private void slider_ValueChanged(object sender, EventArgs e)
		{
			int v = slider.Value;
			if (v > 98) // math fix
				v = 98;
			if (v < 3)
				v = 3;
			proj.Opacity = (int)((double)v * 512 / 100) - 255;
			proj.dirty = true;
		}
		private void txtMessage_TextChanged(object sender, EventArgs e)
		{
			// Update the current region text
			if (currentRegion != null)
			{
				currentRegion.textRegion.message = txtMessage.Text;
				proj.RefreshUI();
				proj.dirty = true;
			}
		}

		private void btnSave_Click(object sender, EventArgs e)
		{
			#if DEMO
			new DemoVersionOnly("Save project").ShowDialog();
			#else
			AnouncementStore anouncementStore = new AnouncementStore();
			if (!proj.bSaved)
			{
				ProjSaveDlg f = new ProjSaveDlg();
				f.ValidateName += delegate(object s, CancelEventArgs ec){ec.Cancel = anouncementStore.NameExists((string)s);};
				if (f.ShowDialog() != DialogResult.OK)
					return;

				proj.data.name = f.FileName;
			}

			anouncementStore.SaveAnouncement(proj.data);
			proj.dirty = false;
			proj.bSaved = true;
			#endif
		}
		private void btnActivate_Click(object sender, EventArgs e)
		{
			proj.Activate();
		}
		private void btnClose_Click(object sender, EventArgs e)
		{
			if (proj.dirty && MessageBox.Show(this, Loc.Get("Project not saved. Save now?"), Loc.Get("Warning"), MessageBoxButtons.YesNo) == DialogResult.Yes)
				btnSave_Click(null, null);

			proj.CloseProject();
		}

		#region IController Members

		public void AttachProject(IProject proj)
		{
			if (this.proj != proj)
			{
				DetachProject();

				this.proj = (AnouncementProject)proj;
				ehRefresh = new EventHandler(project_Refresh);
				this.proj.Refresh += ehRefresh;
			}
			this.RefreshUI();

			// Add a region if this project doesn't have one
			if (this.proj.data.lTextRegions.Count == 0)
				btnAddReg_Click(null, null);
		}
		public void DetachProject()
		{
			DetachRegionControls();
			currentRegion = null;
			txtMessage.Text = "";
			txtMessage.Enabled = false;
			btnChangeFont.Enabled = false;

			if (this.proj != null)
			{
				if (ehRefresh != null)
				{
					this.proj.Refresh -= ehRefresh;
					ehRefresh = null;
				}
				this.proj = null;
			}
		}
		public void DoEditSettings() { }
		private void project_Refresh(object sender, EventArgs e)
		{
			RefreshUI();
		}

		#endregion

		#region IDragDropClient Members

		public bool DragDropHitTest(Point ptScreen, IDataObject dataObj)
		{
			Rectangle r = this.RectangleToScreen(pnlPreviewImage.Bounds);
			if (r.Contains(ptScreen))
			{
				if (dataObj.GetDataPresent(DataFormats.FileDrop))
				{
					// Look for image files
					bool found = false;
					string[] files = (string[])dataObj.GetData(DataFormats.FileDrop);
					foreach (string f in files)
					{
						if (File.Exists(f) && Program.Presenter.dragDropImageFiles.Contains(Path.GetExtension(f).ToLower()))
						{
							found = true;
							break;
						}
					}
					return found;
				}
				else if (dataObj.GetDataPresent(DataFormats.Bitmap))
					return true;
			}
			return false;
		}
		public void OnDragDrop(IDataObject dataObj)
		{
			if (dataObj.GetDataPresent(DataFormats.FileDrop))
			{
				string[] imageFiles = (string[])dataObj.GetData(DataFormats.FileDrop);
				foreach (string file in imageFiles)
				{
					// Skip non-existing files until a valid image is found
					if (!File.Exists(file))
						continue;

					// Skip non-images
					if (!Program.Presenter.dragDropImageFiles.Contains(Path.GetExtension(file).ToLower()))
						continue;

					Image i = Image.FromFile(file);
					proj.UpdateBackground(i);
					proj.RefreshUI();
					break;
				}
			}
			else if (dataObj.GetDataPresent(DataFormats.Bitmap))
			{
				Image i = (Image)dataObj.GetData(DataFormats.Bitmap);
				proj.UpdateBackground(i);
				proj.RefreshUI();
			}
		}

		#endregion
	}
}
