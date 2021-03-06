using System.Drawing;
namespace EmpowerPresenter
{
	partial class AnouncementProjectView
	{
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AnouncementProjectView));
			this.themedPanel1 = new Vendisoft.Controls.ThemedPanel();
			this.btnBrightness = new Vendisoft.Controls.ImageButton();
			this.btnSave = new Vendisoft.Controls.ImageButton();
			this.lblMessageTtl = new System.Windows.Forms.Label();
			this.txtMessage = new System.Windows.Forms.TextBox();
			this.lblTutor = new System.Windows.Forms.Label();
			this.btnRemReg = new Vendisoft.Controls.ImageButton();
			this.btnAddReg = new Vendisoft.Controls.ImageButton();
			this.btnChangeFont = new Vendisoft.Controls.ImageButton();
			this.btnImage = new Vendisoft.Controls.ImageButton();
			this.lblInfo = new System.Windows.Forms.Label();
			this.pnlPreviewImage = new System.Windows.Forms.Label();
			this.btnClose = new Vendisoft.Controls.ImageButton();
			this.btnActivate = new Vendisoft.Controls.ImageButton();
			this.themedPanel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// themedPanel1
			// 
			this.themedPanel1.AccessibleDescription = null;
			this.themedPanel1.AccessibleName = null;
			resources.ApplyResources(this.themedPanel1, "themedPanel1");
			this.themedPanel1.BackColor = System.Drawing.Color.Transparent;
			this.themedPanel1.BackgroundImage = null;
			this.themedPanel1.BottomSplice = 50;
			this.themedPanel1.Controls.Add(this.btnBrightness);
			this.themedPanel1.Controls.Add(this.btnSave);
			this.themedPanel1.Controls.Add(this.lblMessageTtl);
			this.themedPanel1.Controls.Add(this.txtMessage);
			this.themedPanel1.Controls.Add(this.lblTutor);
			this.themedPanel1.Controls.Add(this.btnRemReg);
			this.themedPanel1.Controls.Add(this.btnAddReg);
			this.themedPanel1.Controls.Add(this.btnChangeFont);
			this.themedPanel1.Controls.Add(this.btnImage);
			this.themedPanel1.Controls.Add(this.lblInfo);
			this.themedPanel1.Controls.Add(this.pnlPreviewImage);
			this.themedPanel1.Controls.Add(this.btnClose);
			this.themedPanel1.Controls.Add(this.btnActivate);
			this.themedPanel1.Image = global::EmpowerPresenter.Properties.Resources.panelbackground;
			this.themedPanel1.LeftSlice = 50;
			this.themedPanel1.Name = "themedPanel1";
			this.themedPanel1.RightSplice = 50;
			this.themedPanel1.TopSplice = 50;
			// 
			// btnBrightness
			// 
			this.btnBrightness.AccessibleDescription = null;
			this.btnBrightness.AccessibleName = null;
			resources.ApplyResources(this.btnBrightness, "btnBrightness");
			this.btnBrightness.BackColor = System.Drawing.Color.Transparent;
			this.btnBrightness.BackgroundImage = null;
			this.btnBrightness.Cursor = System.Windows.Forms.Cursors.Hand;
			this.btnBrightness.DisabledImg = global::EmpowerPresenter.Properties.Resources.light_d;
			this.btnBrightness.Font = null;
			this.btnBrightness.IsHighlighted = false;
			this.btnBrightness.IsPressed = false;
			this.btnBrightness.IsSelected = false;
			this.btnBrightness.Name = "btnBrightness";
			this.btnBrightness.NormalImg = global::EmpowerPresenter.Properties.Resources.light_n;
			this.btnBrightness.OverImg = global::EmpowerPresenter.Properties.Resources.light_n;
			this.btnBrightness.PressedImg = global::EmpowerPresenter.Properties.Resources.light_n;
			this.btnBrightness.Tag = "Change opacity";
			this.btnBrightness.MouseLeave += new System.EventHandler(this.imgBtn_MouseLeave);
			this.btnBrightness.Click += new System.EventHandler(this.btnBrightness_Click);
			this.btnBrightness.MouseEnter += new System.EventHandler(this.imgBtn_MouseEnter);
			// 
			// btnSave
			// 
			this.btnSave.AccessibleDescription = null;
			this.btnSave.AccessibleName = null;
			resources.ApplyResources(this.btnSave, "btnSave");
			this.btnSave.BackColor = System.Drawing.Color.Transparent;
			this.btnSave.BackgroundImage = null;
			this.btnSave.Cursor = System.Windows.Forms.Cursors.Hand;
			this.btnSave.DisabledImg = global::EmpowerPresenter.Properties.Resources.save_dis;
			this.btnSave.Font = null;
			this.btnSave.IsHighlighted = false;
			this.btnSave.IsPressed = false;
			this.btnSave.IsSelected = false;
			this.btnSave.Name = "btnSave";
			this.btnSave.NormalImg = global::EmpowerPresenter.Properties.Resources.save;
			this.btnSave.OverImg = global::EmpowerPresenter.Properties.Resources.save;
			this.btnSave.PressedImg = global::EmpowerPresenter.Properties.Resources.save;
			this.btnSave.Tag = "Save";
			this.btnSave.MouseLeave += new System.EventHandler(this.imgBtn_MouseLeave);
			this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
			this.btnSave.MouseEnter += new System.EventHandler(this.imgBtn_MouseEnter);
			// 
			// lblMessageTtl
			// 
			this.lblMessageTtl.AccessibleDescription = null;
			this.lblMessageTtl.AccessibleName = null;
			resources.ApplyResources(this.lblMessageTtl, "lblMessageTtl");
			this.lblMessageTtl.Name = "lblMessageTtl";
			// 
			// txtMessage
			// 
			this.txtMessage.AccessibleDescription = null;
			this.txtMessage.AccessibleName = null;
			resources.ApplyResources(this.txtMessage, "txtMessage");
			this.txtMessage.BackgroundImage = null;
			this.txtMessage.Font = null;
			this.txtMessage.Name = "txtMessage";
			this.txtMessage.TextChanged += new System.EventHandler(this.txtMessage_TextChanged);
			// 
			// lblTutor
			// 
			this.lblTutor.AccessibleDescription = null;
			this.lblTutor.AccessibleName = null;
			resources.ApplyResources(this.lblTutor, "lblTutor");
			this.lblTutor.Font = null;
			this.lblTutor.Name = "lblTutor";
			// 
			// btnRemReg
			// 
			this.btnRemReg.AccessibleDescription = null;
			this.btnRemReg.AccessibleName = null;
			resources.ApplyResources(this.btnRemReg, "btnRemReg");
			this.btnRemReg.BackColor = System.Drawing.Color.Transparent;
			this.btnRemReg.BackgroundImage = null;
			this.btnRemReg.Cursor = System.Windows.Forms.Cursors.Hand;
			this.btnRemReg.DisabledImg = global::EmpowerPresenter.Properties.Resources.removenote_dis;
			this.btnRemReg.Font = null;
			this.btnRemReg.IsHighlighted = false;
			this.btnRemReg.IsPressed = false;
			this.btnRemReg.IsSelected = false;
			this.btnRemReg.Name = "btnRemReg";
			this.btnRemReg.NormalImg = global::EmpowerPresenter.Properties.Resources.removenote;
			this.btnRemReg.OverImg = global::EmpowerPresenter.Properties.Resources.removenote;
			this.btnRemReg.PressedImg = global::EmpowerPresenter.Properties.Resources.removenote;
			this.btnRemReg.Tag = "Remove text";
			this.btnRemReg.MouseLeave += new System.EventHandler(this.imgBtn_MouseLeave);
			this.btnRemReg.Click += new System.EventHandler(this.btnRemReg_Click);
			this.btnRemReg.MouseEnter += new System.EventHandler(this.imgBtn_MouseEnter);
			// 
			// btnAddReg
			// 
			this.btnAddReg.AccessibleDescription = null;
			this.btnAddReg.AccessibleName = null;
			resources.ApplyResources(this.btnAddReg, "btnAddReg");
			this.btnAddReg.BackColor = System.Drawing.Color.Transparent;
			this.btnAddReg.BackgroundImage = null;
			this.btnAddReg.Cursor = System.Windows.Forms.Cursors.Hand;
			this.btnAddReg.DisabledImg = global::EmpowerPresenter.Properties.Resources.addnote_dis;
			this.btnAddReg.Font = null;
			this.btnAddReg.IsHighlighted = false;
			this.btnAddReg.IsPressed = false;
			this.btnAddReg.IsSelected = false;
			this.btnAddReg.Name = "btnAddReg";
			this.btnAddReg.NormalImg = global::EmpowerPresenter.Properties.Resources.addnote;
			this.btnAddReg.OverImg = global::EmpowerPresenter.Properties.Resources.addnote;
			this.btnAddReg.PressedImg = global::EmpowerPresenter.Properties.Resources.addnote;
			this.btnAddReg.Tag = "Add text";
			this.btnAddReg.MouseLeave += new System.EventHandler(this.imgBtn_MouseLeave);
			this.btnAddReg.Click += new System.EventHandler(this.btnAddReg_Click);
			this.btnAddReg.MouseEnter += new System.EventHandler(this.imgBtn_MouseEnter);
			// 
			// btnChangeFont
			// 
			this.btnChangeFont.AccessibleDescription = null;
			this.btnChangeFont.AccessibleName = null;
			resources.ApplyResources(this.btnChangeFont, "btnChangeFont");
			this.btnChangeFont.BackColor = System.Drawing.Color.Transparent;
			this.btnChangeFont.BackgroundImage = null;
			this.btnChangeFont.Cursor = System.Windows.Forms.Cursors.Hand;
			this.btnChangeFont.DisabledImg = global::EmpowerPresenter.Properties.Resources.font_d;
			this.btnChangeFont.Font = null;
			this.btnChangeFont.IsHighlighted = false;
			this.btnChangeFont.IsPressed = false;
			this.btnChangeFont.IsSelected = false;
			this.btnChangeFont.Name = "btnChangeFont";
			this.btnChangeFont.NormalImg = global::EmpowerPresenter.Properties.Resources.font_n;
			this.btnChangeFont.OverImg = global::EmpowerPresenter.Properties.Resources.font_n;
			this.btnChangeFont.PressedImg = global::EmpowerPresenter.Properties.Resources.font_n;
			this.btnChangeFont.Tag = "Change font";
			this.btnChangeFont.MouseLeave += new System.EventHandler(this.imgBtn_MouseLeave);
			this.btnChangeFont.Click += new System.EventHandler(this.btnChangeFont_Click);
			this.btnChangeFont.MouseEnter += new System.EventHandler(this.imgBtn_MouseEnter);
			// 
			// btnImage
			// 
			this.btnImage.AccessibleDescription = null;
			this.btnImage.AccessibleName = null;
			resources.ApplyResources(this.btnImage, "btnImage");
			this.btnImage.BackColor = System.Drawing.Color.Transparent;
			this.btnImage.BackgroundImage = null;
			this.btnImage.Cursor = System.Windows.Forms.Cursors.Hand;
			this.btnImage.DisabledImg = global::EmpowerPresenter.Properties.Resources.images_d;
			this.btnImage.Font = null;
			this.btnImage.IsHighlighted = false;
			this.btnImage.IsPressed = false;
			this.btnImage.IsSelected = false;
			this.btnImage.Name = "btnImage";
			this.btnImage.NormalImg = global::EmpowerPresenter.Properties.Resources.images;
			this.btnImage.OverImg = global::EmpowerPresenter.Properties.Resources.images;
			this.btnImage.PressedImg = global::EmpowerPresenter.Properties.Resources.images;
			this.btnImage.Tag = "Change background";
			this.btnImage.MouseLeave += new System.EventHandler(this.imgBtn_MouseLeave);
			this.btnImage.Click += new System.EventHandler(this.btnImage_Click);
			this.btnImage.MouseEnter += new System.EventHandler(this.imgBtn_MouseEnter);
			// 
			// lblInfo
			// 
			this.lblInfo.AccessibleDescription = null;
			this.lblInfo.AccessibleName = null;
			resources.ApplyResources(this.lblInfo, "lblInfo");
			this.lblInfo.BackColor = System.Drawing.Color.Transparent;
			this.lblInfo.Name = "lblInfo";
			// 
			// pnlPreviewImage
			// 
			this.pnlPreviewImage.AccessibleDescription = null;
			this.pnlPreviewImage.AccessibleName = null;
			resources.ApplyResources(this.pnlPreviewImage, "pnlPreviewImage");
			this.pnlPreviewImage.BackColor = System.Drawing.Color.Transparent;
			this.pnlPreviewImage.Cursor = System.Windows.Forms.Cursors.Hand;
			this.pnlPreviewImage.Font = null;
			this.pnlPreviewImage.Name = "pnlPreviewImage";
			this.pnlPreviewImage.Resize += new System.EventHandler(this.pnlPreviewImage_Resize);
			this.pnlPreviewImage.Paint += new System.Windows.Forms.PaintEventHandler(this.pnlPreviewImage_Paint);
			// 
			// btnClose
			// 
			this.btnClose.AccessibleDescription = null;
			this.btnClose.AccessibleName = null;
			resources.ApplyResources(this.btnClose, "btnClose");
			this.btnClose.BackColor = System.Drawing.Color.Transparent;
			this.btnClose.BackgroundImage = null;
			this.btnClose.Cursor = System.Windows.Forms.Cursors.Hand;
			this.btnClose.DisabledImg = global::EmpowerPresenter.Properties.Resources.close_n;
			this.btnClose.Font = null;
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
			// btnActivate
			// 
			this.btnActivate.AccessibleDescription = null;
			this.btnActivate.AccessibleName = null;
			resources.ApplyResources(this.btnActivate, "btnActivate");
			this.btnActivate.BackColor = System.Drawing.Color.Transparent;
			this.btnActivate.BackgroundImage = null;
			this.btnActivate.Cursor = System.Windows.Forms.Cursors.Hand;
			this.btnActivate.DisabledImg = global::EmpowerPresenter.Properties.Resources.activate_btnd;
			this.btnActivate.Font = null;
			this.btnActivate.IsHighlighted = false;
			this.btnActivate.IsPressed = false;
			this.btnActivate.IsSelected = false;
			this.btnActivate.Name = "btnActivate";
			this.btnActivate.NormalImg = global::EmpowerPresenter.Properties.Resources.activate_btnn;
			this.btnActivate.OverImg = global::EmpowerPresenter.Properties.Resources.activate_btnover;
			this.btnActivate.PressedImg = global::EmpowerPresenter.Properties.Resources.activate_btnn;
			this.btnActivate.Click += new System.EventHandler(this.btnActivate_Click);
			// 
			// AnouncementProjectView
			// 
			this.AccessibleDescription = null;
			this.AccessibleName = null;
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			resources.ApplyResources(this, "$this");
			this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(242)))), ((int)(((byte)(242)))), ((int)(((byte)(242)))));
			this.BackgroundImage = null;
			this.Controls.Add(this.themedPanel1);
			this.DoubleBuffered = true;
			this.Font = null;
			this.Name = "AnouncementProjectView";
			this.themedPanel1.ResumeLayout(false);
			this.themedPanel1.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private Vendisoft.Controls.ThemedPanel themedPanel1;
		private Vendisoft.Controls.ImageButton btnActivate;
		private Vendisoft.Controls.ImageButton btnClose;
		private System.Windows.Forms.Label pnlPreviewImage;
		private System.Windows.Forms.Label lblInfo;
		private Vendisoft.Controls.ImageButton btnChangeFont;
		private Vendisoft.Controls.ImageButton btnImage;
		private Vendisoft.Controls.ImageButton btnRemReg;
		private Vendisoft.Controls.ImageButton btnAddReg;
		private System.Windows.Forms.Label lblMessageTtl;
		private System.Windows.Forms.TextBox txtMessage;
		private System.Windows.Forms.Label lblTutor;
		private Vendisoft.Controls.ImageButton btnSave;
		private Vendisoft.Controls.ImageButton btnBrightness;
	}
}
