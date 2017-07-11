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
    public partial class BibleProjectView : UserControl, IController, IDragDropClient
    {
        private BibleProject proj;
        private EventHandler ehRefresh;
        private GfxContext currentCtx;
        private EmpowerPresenter.Controls.PopupSlider slider;

        //////////////////////////////////////////////////////////////////////////////////////////
        public BibleProjectView()
        {
            InitializeComponent();
            if (Program.ConfigHelper.CurrentLanguage == "ru-RU")
            {
                btnActivate.DisabledImg = global::EmpowerPresenter.Properties.Resources.activate_btndis_ru;
                btnActivate.NormalImg = global::EmpowerPresenter.Properties.Resources.activate_btn_ru;
                btnActivate.OverImg = global::EmpowerPresenter.Properties.Resources.activate_btnover_ru;
                btnActivate.PressedImg = global::EmpowerPresenter.Properties.Resources.activate_btn_ru;
            }
        }

        // internal
        internal void RefreshUI()
        {
            // Update the list binding & display picture
            currentCtx = proj.GetCurrentGfxContext();
            pnlPreviewImage.Refresh();

            UpdateInfoLabel();
            UpdateNavigationButtons();
        }
        private void UpdateInfoLabel()
        {
            if (proj == null || proj.bibVerses.Count == 0 || proj.currentVerseNum == -1)
                lblInfo.Text = "";
            else
                lblInfo.Text = proj.bibVerses[proj.currentVerseNum].ToString();
        }        
        private void UpdateNavigationButtons()
        {
            /// Note: we are allowing the user to browse the entire book, traversing chapter boundaries
            if (proj.bibVerses.Count > 0)
            {
                btnPrev.Enabled = !proj.IsFirst;
                btnNext.Enabled = !proj.IsLast;
            }
        }
        internal void ShowSearchPopup()
        {
            searchPopup.Show();
        }
        internal BibleRenderingFormat GetCurrentFormat()
        {
            BibleRenderingFormat format = BibleRenderingFormat.SingleVerse;
            if (Program.ConfigHelper.BibleNumVerses == 3)
                format = BibleRenderingFormat.FontFit;
            else if (Program.ConfigHelper.BibleSecondaryTranslation != "")
                format = BibleRenderingFormat.MultiTranslation;
            else if (Program.ConfigHelper.BibleNumVerses == 2)
                format = BibleRenderingFormat.DoubleVerse;
            return format;
        }

        // Event handlers
        private void btnActivate_Click(object sender, EventArgs e)
        {
            proj.Activate();
        }
        private void btnPrev_Click(object sender, EventArgs e)
        {
            proj.GoPrevSlide();
        }
        private void btnNext_Click(object sender, EventArgs e)
        {
            proj.GoNextSlide();
        }
        private void btnImage_Click(object sender, EventArgs e)
        {
#if DEMO
            new DemoVersionOnly("Changing background").ShowDialog();
#else
            ImageSelection images = new ImageSelection();
            if (DialogResult.OK == images.ShowDialog())
            {
                Program.ConfigHelper.BibleImage = images.SelectedItem.ImageId;

                proj.UpdateBackgroundImage(images.SelectedItem.FullSizeImage);
            }
#endif
        }
        private void btnFont_Click(object sender, EventArgs e)
        {
#if DEMO
            new DemoVersionOnly("Changing font").ShowDialog();
#else
            FontSelection fsForm = new FontSelection();
            fsForm.LoadFont(PresenterFont.GetFontFromDatabase(-1));
            fsForm.cbDoubleSpace.Visible = false;
            fsForm.gbAlignment.Visible = (GetCurrentFormat() != BibleRenderingFormat.MultiTranslation);
            if (fsForm.ShowDialog() == DialogResult.OK)
            {
                PresenterFont.SaveFontToDatabase(-1, fsForm.PresenterFont);
                proj.RefreshData();
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
                slider.Value = (int)(((double)Program.ConfigHelper.BibleImageOpacity + 255) * 100 / 512);
                Program.Presenter.ShowPopupWindow(slider, ptLoc);
                Program.Presenter.ActivateKeyClient(slider);
            }
        }
        private void slider_ValueChanged(object sender, EventArgs e)
        {
            // Calculate
            int v = slider.Value;
            if (v > 98) // math fix
                v = 98;
            if (v < 3)
                v = 3;
            int opacityval = (int)((double)v * 512 / 100) - 255;

            // Refresh
            proj.UpdateOpacity(opacityval);

            // Save
            Program.ConfigHelper.BibleImageOpacity = opacityval;
        }
        private void imgBtn_MouseEnter(object sender, EventArgs e)
        {
            lblInfo.Text = Loc.Get(((Control)sender).Tag.ToString());
        }
        private void imgBtn_MouseLeave(object sender, EventArgs e)
        {
            UpdateInfoLabel();
        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            proj.CloseProject();
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
        }
        private void pnlPreviewImage_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && btnNext.Enabled)
                proj.GoNextSlide();
            else if (e.Button == MouseButtons.Right && btnPrev.Enabled)
                proj.GoPrevSlide();
        }
        private void bibVersePreviewControl1_Click(object sender, EventArgs e)
        {
            // TODO: register for keys?
        }
        private void searchPopup_SearchTermChanged(object sender, EventArgs e)
        {
            if (this.proj != null)
                bibVersePreviewControl1.UpdateSearchTerms(searchPopup.SearchTerms);
        }
        private void searchPopup_VisibleChanged(object sender, System.EventArgs e)
        {
            bibVersePreviewControl1.allowGrabFocus = !searchPopup.Visible;
        }

        // Properties
        public IProject Project
        {
            get { return proj; }
        }

        #region IController Members

        public void AttachProject(IProject proj)
        {
            if (this.proj != proj)
            {
                DetachProject();

                this.proj = (BibleProject)proj;
                if ((object)proj as ISlideShow == null)
                    throw (new ApplicationException("The project must implement ISlideShow"));
                ehRefresh = new EventHandler(project_refresh);
                this.proj.Refresh += ehRefresh;
                
                bibVersePreviewControl1.AttachProject(this.proj);
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
            }
            this.proj = null;
            bibVersePreviewControl1.DetachProject();
        }
        public void DoEditSettings() { }
        private void project_refresh(object sender, EventArgs e)
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
                    proj.UpdateBackgroundImage(i);
                    break;
                }
            }
            else if (dataObj.GetDataPresent(DataFormats.Bitmap))
            {
                Image i = (Image)dataObj.GetData(DataFormats.Bitmap);
                proj.UpdateBackgroundImage(i);
            }
        }

        #endregion
    }
}
