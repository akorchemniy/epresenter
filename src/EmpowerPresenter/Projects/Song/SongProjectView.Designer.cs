using System.Drawing;
namespace EmpowerPresenter
{
	partial class SongProjectView
	{
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SongProjectView));
			this.themedPanel1 = new Vendisoft.Controls.ThemedPanel();
			this.btnOptions = new Vendisoft.Controls.ImageButton();
			this.songPreviewControl1 = new EmpowerPresenter.SongPreviewControl();
			this.pnlPreviewImage = new System.Windows.Forms.Label();
			this.lblInfo = new System.Windows.Forms.Label();
			this.btnBrightness = new Vendisoft.Controls.ImageButton();
			this.btnFont = new Vendisoft.Controls.ImageButton();
			this.btnEditMusic = new Vendisoft.Controls.ImageButton();
			this.btnImage = new Vendisoft.Controls.ImageButton();
			this.btnClose = new Vendisoft.Controls.ImageButton();
			this.btnNext = new Vendisoft.Controls.ImageButton();
			this.btnPrev = new Vendisoft.Controls.ImageButton();
			this.btnActivate = new Vendisoft.Controls.ImageButton();
			this.themedPanel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// themedPanel1
			// 
			this.themedPanel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(242)))), ((int)(((byte)(242)))), ((int)(((byte)(242)))));
			this.themedPanel1.BottomSplice = 50;
			this.themedPanel1.Controls.Add(this.btnOptions);
			this.themedPanel1.Controls.Add(this.songPreviewControl1);
			this.themedPanel1.Controls.Add(this.pnlPreviewImage);
			this.themedPanel1.Controls.Add(this.lblInfo);
			this.themedPanel1.Controls.Add(this.btnBrightness);
			this.themedPanel1.Controls.Add(this.btnFont);
			this.themedPanel1.Controls.Add(this.btnEditMusic);
			this.themedPanel1.Controls.Add(this.btnImage);
			this.themedPanel1.Controls.Add(this.btnClose);
			this.themedPanel1.Controls.Add(this.btnNext);
			this.themedPanel1.Controls.Add(this.btnPrev);
			this.themedPanel1.Controls.Add(this.btnActivate);
			resources.ApplyResources(this.themedPanel1, "themedPanel1");
			this.themedPanel1.Image = global::EmpowerPresenter.Properties.Resources.panelbackground;
			this.themedPanel1.LeftSlice = 50;
			this.themedPanel1.Name = "themedPanel1";
			this.themedPanel1.RightSplice = 50;
			this.themedPanel1.TopSplice = 50;
			// 
			// btnOptions
			// 
			this.btnOptions.BackColor = System.Drawing.Color.Transparent;
			this.btnOptions.Cursor = System.Windows.Forms.Cursors.Hand;
			this.btnOptions.DisabledImg = global::EmpowerPresenter.Properties.Resources.cog;
			this.btnOptions.IsHighlighted = false;
			this.btnOptions.IsPressed = false;
			this.btnOptions.IsSelected = false;
			resources.ApplyResources(this.btnOptions, "btnOptions");
			this.btnOptions.Name = "btnOptions";
			this.btnOptions.NormalImg = global::EmpowerPresenter.Properties.Resources.cog;
			this.btnOptions.OverImg = global::EmpowerPresenter.Properties.Resources.cog;
			this.btnOptions.PressedImg = global::EmpowerPresenter.Properties.Resources.cog;
			this.btnOptions.Tag = "Settings";
			this.btnOptions.MouseLeave += new System.EventHandler(this.imgBtn_MouseLeave);
			this.btnOptions.Click += new System.EventHandler(this.btnOptions_Click);
			this.btnOptions.MouseEnter += new System.EventHandler(this.imgBtn_MouseEnter);
			// 
			// songPreviewControl1
			// 
			resources.ApplyResources(this.songPreviewControl1, "songPreviewControl1");
			this.songPreviewControl1.BackColor = System.Drawing.Color.Transparent;
			this.songPreviewControl1.Name = "songPreviewControl1";
			this.songPreviewControl1.Click += new System.EventHandler(this.songPreviewControl1_Click);
			// 
			// pnlPreviewImage
			// 
			resources.ApplyResources(this.pnlPreviewImage, "pnlPreviewImage");
			this.pnlPreviewImage.BackColor = System.Drawing.Color.Transparent;
			this.pnlPreviewImage.Cursor = System.Windows.Forms.Cursors.Hand;
			this.pnlPreviewImage.Name = "pnlPreviewImage";
			this.pnlPreviewImage.MouseDown += new System.Windows.Forms.MouseEventHandler(pnlPreviewImage_MouseDown);
			this.pnlPreviewImage.Resize += new System.EventHandler(this.pnlPreviewImage_Resize);
			this.pnlPreviewImage.Paint += new System.Windows.Forms.PaintEventHandler(this.pnlPreviewImage_Paint);
			// 
			// lblInfo
			// 
			resources.ApplyResources(this.lblInfo, "lblInfo");
			this.lblInfo.BackColor = System.Drawing.Color.Transparent;
			this.lblInfo.Name = "lblInfo";
			// 
			// btnBrightness
			// 
			this.btnBrightness.BackColor = System.Drawing.Color.Transparent;
			this.btnBrightness.Cursor = System.Windows.Forms.Cursors.Hand;
			this.btnBrightness.DisabledImg = global::EmpowerPresenter.Properties.Resources.light_d;
			this.btnBrightness.IsHighlighted = false;
			this.btnBrightness.IsPressed = false;
			this.btnBrightness.IsSelected = false;
			resources.ApplyResources(this.btnBrightness, "btnBrightness");
			this.btnBrightness.Name = "btnBrightness";
			this.btnBrightness.NormalImg = global::EmpowerPresenter.Properties.Resources.light_n;
			this.btnBrightness.OverImg = global::EmpowerPresenter.Properties.Resources.light_n;
			this.btnBrightness.PressedImg = global::EmpowerPresenter.Properties.Resources.light_n;
			this.btnBrightness.Tag = "Change opacity";
			this.btnBrightness.MouseLeave += new System.EventHandler(this.imgBtn_MouseLeave);
			this.btnBrightness.Click += new System.EventHandler(this.btnBrightness_Click);
			this.btnBrightness.MouseEnter += new System.EventHandler(this.imgBtn_MouseEnter);
			// 
			// btnFont
			// 
			this.btnFont.BackColor = System.Drawing.Color.Transparent;
			this.btnFont.Cursor = System.Windows.Forms.Cursors.Hand;
			this.btnFont.DisabledImg = global::EmpowerPresenter.Properties.Resources.font_d;
			this.btnFont.IsHighlighted = false;
			this.btnFont.IsPressed = false;
			this.btnFont.IsSelected = false;
			resources.ApplyResources(this.btnFont, "btnFont");
			this.btnFont.Name = "btnFont";
			this.btnFont.NormalImg = global::EmpowerPresenter.Properties.Resources.font_n;
			this.btnFont.OverImg = global::EmpowerPresenter.Properties.Resources.font_n;
			this.btnFont.PressedImg = global::EmpowerPresenter.Properties.Resources.font_n;
			this.btnFont.Tag = "Change font";
			this.btnFont.MouseLeave += new System.EventHandler(this.imgBtn_MouseLeave);
			this.btnFont.Click += new System.EventHandler(this.btnFont_Click);
			this.btnFont.MouseEnter += new System.EventHandler(this.imgBtn_MouseEnter);
			// 
			// btnEditMusic
			// 
			this.btnEditMusic.BackColor = System.Drawing.Color.Transparent;
			this.btnEditMusic.Cursor = System.Windows.Forms.Cursors.Hand;
			this.btnEditMusic.DisabledImg = global::EmpowerPresenter.Properties.Resources.editmusic_d;
			this.btnEditMusic.IsHighlighted = false;
			this.btnEditMusic.IsPressed = false;
			this.btnEditMusic.IsSelected = false;
			resources.ApplyResources(this.btnEditMusic, "btnEditMusic");
			this.btnEditMusic.Name = "btnEditMusic";
			this.btnEditMusic.NormalImg = global::EmpowerPresenter.Properties.Resources.editmusic_n;
			this.btnEditMusic.OverImg = global::EmpowerPresenter.Properties.Resources.editmusic_n;
			this.btnEditMusic.PressedImg = global::EmpowerPresenter.Properties.Resources.editmusic_n;
			this.btnEditMusic.Tag = "Edit song";
			this.btnEditMusic.MouseLeave += new System.EventHandler(this.imgBtn_MouseLeave);
			this.btnEditMusic.Click += new System.EventHandler(this.btnEditMusic_Click);
			this.btnEditMusic.MouseEnter += new System.EventHandler(this.imgBtn_MouseEnter);
			// 
			// btnImage
			// 
			this.btnImage.BackColor = System.Drawing.Color.Transparent;
			this.btnImage.Cursor = System.Windows.Forms.Cursors.Hand;
			this.btnImage.DisabledImg = global::EmpowerPresenter.Properties.Resources.images_d;
			this.btnImage.IsHighlighted = false;
			this.btnImage.IsPressed = false;
			this.btnImage.IsSelected = false;
			resources.ApplyResources(this.btnImage, "btnImage");
			this.btnImage.Name = "btnImage";
			this.btnImage.NormalImg = global::EmpowerPresenter.Properties.Resources.images;
			this.btnImage.OverImg = global::EmpowerPresenter.Properties.Resources.images;
			this.btnImage.PressedImg = global::EmpowerPresenter.Properties.Resources.images;
			this.btnImage.Tag = "Change background";
			this.btnImage.MouseLeave += new System.EventHandler(this.imgBtn_MouseLeave);
			this.btnImage.Click += new System.EventHandler(this.btnImage_Click);
			this.btnImage.MouseEnter += new System.EventHandler(this.imgBtn_MouseEnter);
			// 
			// btnClose
			// 
			resources.ApplyResources(this.btnClose, "btnClose");
			this.btnClose.BackColor = System.Drawing.Color.Transparent;
			this.btnClose.Cursor = System.Windows.Forms.Cursors.Hand;
			this.btnClose.DisabledImg = global::EmpowerPresenter.Properties.Resources.close_n;
			this.btnClose.IsHighlighted = false;
			this.btnClose.IsPressed = false;
			this.btnClose.IsSelected = false;
			this.btnClose.Name = "btnClose";
			this.btnClose.NormalImg = global::EmpowerPresenter.Properties.Resources.close_n;
			this.btnClose.OverImg = global::EmpowerPresenter.Properties.Resources.close_o;
			this.btnClose.PressedImg = global::EmpowerPresenter.Properties.Resources.close_n;
			this.btnClose.Tag = "Close";
			this.btnClose.MouseLeave += new System.EventHandler(this.imgBtn_MouseLeave);
			this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
			this.btnClose.MouseEnter += new System.EventHandler(this.imgBtn_MouseEnter);
			// 
			// btnNext
			// 
			this.btnNext.BackColor = System.Drawing.Color.Transparent;
			this.btnNext.Cursor = System.Windows.Forms.Cursors.Hand;
			this.btnNext.DisabledImg = global::EmpowerPresenter.Properties.Resources.greenright_d;
			this.btnNext.IsHighlighted = false;
			this.btnNext.IsPressed = false;
			this.btnNext.IsSelected = false;
			resources.ApplyResources(this.btnNext, "btnNext");
			this.btnNext.Name = "btnNext";
			this.btnNext.NormalImg = global::EmpowerPresenter.Properties.Resources.greenright_n;
			this.btnNext.OverImg = global::EmpowerPresenter.Properties.Resources.greenright_n;
			this.btnNext.PressedImg = global::EmpowerPresenter.Properties.Resources.greenright_n;
			this.btnNext.Click += new System.EventHandler(this.btnNext_Click);
			// 
			// btnPrev
			// 
			this.btnPrev.BackColor = System.Drawing.Color.Transparent;
			this.btnPrev.Cursor = System.Windows.Forms.Cursors.Hand;
			this.btnPrev.DisabledImg = global::EmpowerPresenter.Properties.Resources.greenleft_d;
			this.btnPrev.IsHighlighted = false;
			this.btnPrev.IsPressed = false;
			this.btnPrev.IsSelected = false;
			resources.ApplyResources(this.btnPrev, "btnPrev");
			this.btnPrev.Name = "btnPrev";
			this.btnPrev.NormalImg = global::EmpowerPresenter.Properties.Resources.greenleft_n;
			this.btnPrev.OverImg = global::EmpowerPresenter.Properties.Resources.greenleft_n;
			this.btnPrev.PressedImg = global::EmpowerPresenter.Properties.Resources.greenleft_n;
			this.btnPrev.Click += new System.EventHandler(this.btnPrev_Click);
			// 
			// btnActivate
			// 
			this.btnActivate.BackColor = System.Drawing.Color.Transparent;
			this.btnActivate.Cursor = System.Windows.Forms.Cursors.Hand;
			this.btnActivate.DisabledImg = global::EmpowerPresenter.Properties.Resources.activate_btnd;
			this.btnActivate.IsHighlighted = false;
			this.btnActivate.IsPressed = false;
			this.btnActivate.IsSelected = false;
			resources.ApplyResources(this.btnActivate, "btnActivate");
			this.btnActivate.Name = "btnActivate";
			this.btnActivate.NormalImg = global::EmpowerPresenter.Properties.Resources.activate_btnn;
			this.btnActivate.OverImg = ((System.Drawing.Image)(resources.GetObject("btnActivate.OverImg")));
			this.btnActivate.PressedImg = global::EmpowerPresenter.Properties.Resources.activate_btnn;
			this.btnActivate.Click += new System.EventHandler(this.btnActivate_Click);
			// 
			// SongProjectView
			// 
			resources.ApplyResources(this, "$this");
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(242)))), ((int)(((byte)(242)))), ((int)(((byte)(242)))));
			this.Controls.Add(this.themedPanel1);
			this.DoubleBuffered = true;
			this.Name = "SongProjectView";
			this.themedPanel1.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private Vendisoft.Controls.ThemedPanel themedPanel1;
		private Vendisoft.Controls.ImageButton btnActivate;
		private Vendisoft.Controls.ImageButton btnFont;
		private Vendisoft.Controls.ImageButton btnEditMusic;
		private Vendisoft.Controls.ImageButton btnImage;
		private Vendisoft.Controls.ImageButton btnClose;
		private Vendisoft.Controls.ImageButton btnNext;
		private Vendisoft.Controls.ImageButton btnPrev;
		private Vendisoft.Controls.ImageButton btnBrightness;
		private System.Windows.Forms.Label lblInfo;
		private System.Windows.Forms.Label pnlPreviewImage;
		private SongPreviewControl songPreviewControl1;
		private Vendisoft.Controls.ImageButton btnOptions;
	}
}
