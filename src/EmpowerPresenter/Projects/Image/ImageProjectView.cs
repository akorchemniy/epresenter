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
using System.Runtime.InteropServices;
using Microsoft.Win32;

namespace EmpowerPresenter
{
	public partial class ImageProjectView : UserControl, IController
	{
		private ImageProject proj;
		private EventHandler ehRefresh;
		private Image slideCache;
		private Brush brushNormal;
		private Brush brushSelected;
		private Font boldFont;

		////////////////////////////////////////////////////////////////////////////////
		public ImageProjectView()
		{
			InitializeComponent();
			if (Program.ConfigHelper.CurrentLanguage == "ru-RU")
			{
				btnActivate.DisabledImg = global::EmpowerPresenter.Properties.Resources.activate_btndis_ru;
				btnActivate.NormalImg = global::EmpowerPresenter.Properties.Resources.activate_btn_ru;
				btnActivate.OverImg = global::EmpowerPresenter.Properties.Resources.activate_btnover_ru;
				btnActivate.PressedImg = global::EmpowerPresenter.Properties.Resources.activate_btn_ru;
			}

			// Register exclusive key owners for key manager
			if (Program.Presenter != null)
			{
				Program.Presenter.RegisterExKeyOwnerControl(lbImages);
			}
		}
		internal void RefreshUI()
		{
			slideCache = proj.GetCurrentImage();
			pnlPreviewImage.Refresh();

			UpdateFileList();
			UpdateInfoLabel();

			// Update navigation button
			btnActivate.Enabled = proj.names.Count > 0;
			btnPrev.Enabled = !(proj.currentIndex < 1);
			btnPrev.Refresh();
			btnNext.Enabled = !(proj.currentIndex == proj.names.Count - 1) && !(proj.names.Count == 0);
			btnNext.Refresh();
		}
		private void UpdateFileList()
		{
			// Determine if a rebuild is necessary by comparing
			bool rebuild = false;
			if (lbImages.Items.Count == proj.names.Count)
			{
				for (int i = 0; i < proj.names.Count; i++)
				{
					if ((string)lbImages.Items[i] != proj.names[i])
					{
						rebuild = true;
						break;
					}
				}
			}
			else
				rebuild = true;

			// Rebuild if necessary
			if (rebuild)
			{
				lbImages.SuspendLayout();
				lbImages.Items.Clear();
				foreach (string s in proj.names)
					lbImages.Items.Add(s);
				lbImages.ResumeLayout();
			}

			// Select current item
			if (proj.currentIndex != -1 && proj.names.Count > 0)
			{
				string currentName = proj.names[proj.currentIndex];
				lbImages.SelectedItem = currentName;
			}
		}
		private void UpdateInfoLabel()
		{
			if (proj == null || proj.currentIndex == -1 || proj.names.Count == 0)
				lblInfo.Text = Loc.Get("Preview:");
			else
				lblInfo.Text = proj.names[proj.currentIndex];
		}

		private void btnPrev_Click(object sender, EventArgs e)
		{
			proj.GoPrevSlide();
		}
		private void btnNext_Click(object sender, EventArgs e)
		{
			proj.GoNextSlide();
		}
		private void imgBtn_MouseEnter(object sender, EventArgs e)
		{
			lblInfo.Text = Loc.Get(((Control)sender).Tag.ToString());
		}
		private void imgBtn_MouseLeave(object sender, EventArgs e)
		{
			UpdateInfoLabel();
		}
		private void listBox1_DrawItem(object sender, DrawItemEventArgs e)
		{
			if (e.Index == -1)
				return;

			if (boldFont == null)
				boldFont = new Font(this.Font, FontStyle.Bold);
			if (brushNormal == null)
				brushNormal = new SolidBrush(Color.FromArgb(223, 223, 223));
			if (brushSelected == null)
				brushSelected = new SolidBrush(Color.FromArgb(255, 204, 91));

			string s = (string)lbImages.Items[e.Index];

			// Draw the background
			if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
				e.Graphics.FillRectangle(brushSelected, e.Bounds.X + 1, e.Bounds.Y + 1, e.Bounds.Width - 2, e.Bounds.Height - 2);
			else
				e.Graphics.FillRectangle(brushNormal, e.Bounds.X + 1, e.Bounds.Y + 1, e.Bounds.Width - 2, e.Bounds.Height - 2);

			// Draw the name
			StringFormat sf = new StringFormat();
			sf.Trimming = StringTrimming.EllipsisCharacter;
			e.Graphics.DrawString(s, boldFont, Brushes.Black, new Rectangle(e.Bounds.X + 7, e.Bounds.Y + 7, e.Bounds.Width - 10, this.Font.Height + 2), sf);
		}
		private void lbImages_SelectedIndexChanged(object sender, EventArgs e)
		{
			proj.currentIndex = lbImages.SelectedIndex;
			proj.RefreshUI();

			btnWallpaper.Enabled = btnRemove.Enabled = lbImages.SelectedIndex != -1;
		}
		private void btnAdd_Click(object sender, EventArgs e)
		{
			try
			{
				Program.Presenter.BeginExKeyOwner();
				OpenFileDialog f = new OpenFileDialog();
				f.Title = Loc.Get("Select image to add");
				f.Multiselect = true;
				f.Filter = Loc.Get("Supported formats") + " | *.jpg;*.png;*.bmp;*.gif | JPEG | *.jpg | GIF | *.gif | PNG | *.png | BMP | *.bmp";
				if (f.ShowDialog() == DialogResult.OK)
				{
					proj.AddFiles(f.FileNames);
				}
			}
			finally { Program.Presenter.EndExKeyOwner(); }
		}
		private void btnRemove_Click(object sender, EventArgs e)
		{
			if (lbImages.SelectedIndex == -1)
				return;

			string n = (string)lbImages.SelectedItem;
			proj.RemoveImage(n); // NOTE: list will be refresh automatically
		}
		private void btnOptions_Click(object sender, EventArgs e)
		{
			ImageDispStyle f = new ImageDispStyle();
			f.Style = proj.Style;
			if (f.ShowDialog() == DialogResult.OK)
			{
				proj.Style = f.Style;
				proj.RefreshUI();
			}
		}
		private void btnWallpaper_Click(object sender, EventArgs e)
		{
			SetAsWallpaper();
		}
		private void pnlPreviewImage_Paint(object sender, PaintEventArgs e)
		{
			if (slideCache == null)
				return;

			// Smart position the preivew image in the panel
			double scaleFactorW = (double)pnlPreviewImage.Width / slideCache.Width;
			if (pnlPreviewImage.Height - (int)(slideCache.Height * scaleFactorW) >= 0) // Try first by width
			{
				int destHeight = (int)(slideCache.Height * scaleFactorW);
				e.Graphics.DrawImage(slideCache, new Rectangle(0, 0, pnlPreviewImage.Width, destHeight),
					new Rectangle(0, 0, slideCache.Width, slideCache.Height), GraphicsUnit.Pixel);
			}
			else // Paint by height
			{
				double scaleFactorH = (double)pnlPreviewImage.Height / slideCache.Height;
				int destWidth = (int)(slideCache.Width * scaleFactorH);
				int cx = (int)((pnlPreviewImage.Width - destWidth) / 2);
				e.Graphics.DrawImage(slideCache, new Rectangle(cx, 0, destWidth, pnlPreviewImage.Height),
					new Rectangle(0, 0, slideCache.Width, slideCache.Height), GraphicsUnit.Pixel);
			}
		}
		private void pnlPreviewImage_Resize(object sender, EventArgs e)
		{
			pnlPreviewImage.Refresh();
		}
		private void pnlPreviewImage_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left && btnNext.Enabled)
				proj.GoNextSlide();
			else if (e.Button == MouseButtons.Right && btnPrev.Enabled)
				proj.GoPrevSlide();
		}
		private void btnActivate_Click(object sender, EventArgs e)
		{
			proj.Activate();
		}
		private void btnClose_Click(object sender, EventArgs e)
		{
			proj.CloseProject();
		}

		// Background support
		private void SetAsWallpaper()
		{
			try
			{
				// update the registry
				RegistryKey key = Registry.CurrentUser.OpenSubKey("Control Panel\\Desktop", true);
				key.SetValue(@"WallpaperStyle", "1");
				key.SetValue(@"TileWallpaper", "0");
				key.Close();

				// save temporary files
				string path = Path.GetTempFileName() + ".bmp";
				slideCache.Save(path, System.Drawing.Imaging.ImageFormat.Bmp);

				// update the system
				SystemParametersInfo(SPI_SETDESKWALLPAPER, 0, path, SPIF_UPDATEINIFILE | SPIF_SENDWININICHANGE);
			}
			catch (Exception ex) { System.Diagnostics.Trace.WriteLine(ex.ToString()); }
		}
		const int SPI_SETDESKWALLPAPER = 20;
		const int SPIF_UPDATEINIFILE = 0x01;
		const int SPIF_SENDWININICHANGE = 0x02;
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		static extern int SystemParametersInfo(int uAction, int uParam, string lpvParam, int fuWinIni);

		#region IController Members

		public void AttachProject(IProject proj)
		{
			if (this.proj != proj)
			{
				DetachProject();

				this.proj = (ImageProject)proj;
				ehRefresh = new EventHandler(project_Refresh);
				this.proj.Refresh += ehRefresh;
			}
			this.RefreshUI();
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
				this.proj = null;
			}
		}
		public void DoEditSettings() { btnOptions_Click(null, null); }
		private void project_Refresh(object sender, EventArgs e)
		{
			RefreshUI();
		}

		#endregion

	}
}
