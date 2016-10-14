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
	public class Settings : System.Windows.Forms.Form
	{
		#region Designer

		private EmpowerPresenter.DisplaySelection displaySelection1;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.Panel pnlBottom;
		private System.Windows.Forms.Button btnClose;
		private System.Windows.Forms.Panel pnlBottomDiv;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.ComboBox _primaryTranslation;
		private System.Windows.Forms.ComboBox _secondaryTranslation;
		private TabControl tabControl1;
		private TabPage tpDisplay;
		private TabPage tpOther;
		private PictureBox pbDisplayWarning;
		private Label label4;
		private LinkLabel llDisplayWarning;
		private CheckBox cbBackdrop;
		private CheckBox cbHideContent;
		private GroupBox gbSongs;
		private Vendisoft.Controls.ImageButton btnImage;
		private Vendisoft.Controls.ImageButton btnFont;
		private Label label8;
		private Label label7;
		private Label label6;
		private Label label5;
		private Vendisoft.Controls.ImageButton btnOptions;
		private EmpowerPresenter.Controls.PopupSlider sliderOpacity;
		private System.Windows.Forms.Button btnSave;
		#endregion

		//////////////////////////////////////////////////////////////////////////////////////
		public Settings()
		{
			InitializeComponent();

			try
			{
				displaySelection1.CurrentScreenIndex = Program.ConfigHelper.PresentationMonitor;
			}
			catch{displaySelection1.CurrentScreenIndex = 0;}
			setPrimaryTranslation(Program.ConfigHelper.BiblePrimaryTranslation);
			setSecondaryTranslation(Program.ConfigHelper.BibleSecondaryTranslation);
			cbBackdrop.Checked = Program.ConfigHelper.UseBlackBackdrop;
			cbHideContent.Checked = Program.ConfigHelper.HideContentOnMin;
			sliderOpacity.Value = (int)(((double)Program.ConfigHelper.SongDefaultOpacity + 255) * 100 / 512);
			sliderOpacity.ValueChanged += new EventHandler(sliderOpacity_ValueChanged);

			Program.Presenter.RegisterExKeyOwnerForm(this);
		}

		private void setPrimaryTranslation(string value)
		{
			switch(value)
			{
				case "RST":
					_primaryTranslation.SelectedIndex =1;
					break;
				case "UK":
					_primaryTranslation.SelectedIndex =2;
					break;
				default:
					_primaryTranslation.SelectedIndex =0;
					break;
			}
		}
		private void setSecondaryTranslation(string value)
		{
			switch(value)
			{
				case "KJV":
					_secondaryTranslation.SelectedIndex = 1;
					break;
				case "RST":
					_secondaryTranslation.SelectedIndex =2;
					break;
				case "UK":
					_secondaryTranslation.SelectedIndex =3;
					break;
				default:
					_secondaryTranslation.SelectedIndex =0;
					break;
			}
		}
		private string getPrimaryTranslation()
		{
			switch(_primaryTranslation.SelectedIndex)
			{
				case 0:
					return "KJV";
				case 1:
					return "RST";
				case 2:
					return "UK";
				default:
					return "KJV";
			}
		}
		private string getSecondaryTranslation()
		{
			switch(_secondaryTranslation.SelectedIndex)
			{
				case 0:
					return "";
				case 1:
					return "KJV";
				case 2:
					return "RST";
				case 3:
					return "UK";
				default:
					return "";
			}
		}

		#region Windows Form Designer generated code
		private void InitializeComponent()
		{
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Settings));
			this.displaySelection1 = new EmpowerPresenter.DisplaySelection();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this._secondaryTranslation = new System.Windows.Forms.ComboBox();
			this._primaryTranslation = new System.Windows.Forms.ComboBox();
			this.label2 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.pnlBottom = new System.Windows.Forms.Panel();
			this.btnSave = new System.Windows.Forms.Button();
			this.btnClose = new System.Windows.Forms.Button();
			this.pnlBottomDiv = new System.Windows.Forms.Panel();
			this.tabControl1 = new System.Windows.Forms.TabControl();
			this.tpDisplay = new System.Windows.Forms.TabPage();
			this.llDisplayWarning = new System.Windows.Forms.LinkLabel();
			this.pbDisplayWarning = new System.Windows.Forms.PictureBox();
			this.label4 = new System.Windows.Forms.Label();
			this.tpOther = new System.Windows.Forms.TabPage();
			this.gbSongs = new System.Windows.Forms.GroupBox();
			this.sliderOpacity = new EmpowerPresenter.Controls.PopupSlider();
			this.btnOptions = new Vendisoft.Controls.ImageButton();
			this.label8 = new System.Windows.Forms.Label();
			this.label7 = new System.Windows.Forms.Label();
			this.label6 = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.btnFont = new Vendisoft.Controls.ImageButton();
			this.btnImage = new Vendisoft.Controls.ImageButton();
			this.cbHideContent = new System.Windows.Forms.CheckBox();
			this.cbBackdrop = new System.Windows.Forms.CheckBox();
			this.groupBox2.SuspendLayout();
			this.pnlBottom.SuspendLayout();
			this.tabControl1.SuspendLayout();
			this.tpDisplay.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.pbDisplayWarning)).BeginInit();
			this.tpOther.SuspendLayout();
			this.gbSongs.SuspendLayout();
			this.SuspendLayout();
			// 
			// displaySelection1
			// 
			this.displaySelection1.CurrentScreenIndex = 0;
			resources.ApplyResources(this.displaySelection1, "displaySelection1");
			this.displaySelection1.Name = "displaySelection1";
			// 
			// groupBox2
			// 
			resources.ApplyResources(this.groupBox2, "groupBox2");
			this.groupBox2.Controls.Add(this._secondaryTranslation);
			this.groupBox2.Controls.Add(this._primaryTranslation);
			this.groupBox2.Controls.Add(this.label2);
			this.groupBox2.Controls.Add(this.label1);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.TabStop = false;
			// 
			// _secondaryTranslation
			// 
			resources.ApplyResources(this._secondaryTranslation, "_secondaryTranslation");
			this._secondaryTranslation.Items.AddRange(new object[] {
            resources.GetString("_secondaryTranslation.Items"),
            resources.GetString("_secondaryTranslation.Items1"),
            resources.GetString("_secondaryTranslation.Items2"),
            resources.GetString("_secondaryTranslation.Items3")});
			this._secondaryTranslation.Name = "_secondaryTranslation";
			// 
			// _primaryTranslation
			// 
			resources.ApplyResources(this._primaryTranslation, "_primaryTranslation");
			this._primaryTranslation.Items.AddRange(new object[] {
            resources.GetString("_primaryTranslation.Items"),
            resources.GetString("_primaryTranslation.Items1"),
            resources.GetString("_primaryTranslation.Items2")});
			this._primaryTranslation.Name = "_primaryTranslation";
			// 
			// label2
			// 
			resources.ApplyResources(this.label2, "label2");
			this.label2.Name = "label2";
			// 
			// label1
			// 
			resources.ApplyResources(this.label1, "label1");
			this.label1.Name = "label1";
			// 
			// pnlBottom
			// 
			this.pnlBottom.BackColor = System.Drawing.SystemColors.ControlLightLight;
			this.pnlBottom.Controls.Add(this.btnSave);
			this.pnlBottom.Controls.Add(this.btnClose);
			this.pnlBottom.Controls.Add(this.pnlBottomDiv);
			resources.ApplyResources(this.pnlBottom, "pnlBottom");
			this.pnlBottom.Name = "pnlBottom";
			// 
			// btnSave
			// 
			resources.ApplyResources(this.btnSave, "btnSave");
			this.btnSave.BackColor = System.Drawing.SystemColors.Control;
			this.btnSave.Name = "btnSave";
			this.btnSave.UseVisualStyleBackColor = false;
			this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
			// 
			// btnClose
			// 
			resources.ApplyResources(this.btnClose, "btnClose");
			this.btnClose.BackColor = System.Drawing.SystemColors.Control;
			this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnClose.Name = "btnClose";
			this.btnClose.UseVisualStyleBackColor = false;
			this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
			// 
			// pnlBottomDiv
			// 
			this.pnlBottomDiv.BackColor = System.Drawing.Color.Gray;
			resources.ApplyResources(this.pnlBottomDiv, "pnlBottomDiv");
			this.pnlBottomDiv.Name = "pnlBottomDiv";
			// 
			// tabControl1
			// 
			this.tabControl1.Controls.Add(this.tpDisplay);
			this.tabControl1.Controls.Add(this.tpOther);
			resources.ApplyResources(this.tabControl1, "tabControl1");
			this.tabControl1.Name = "tabControl1";
			this.tabControl1.SelectedIndex = 0;
			// 
			// tpDisplay
			// 
			this.tpDisplay.Controls.Add(this.llDisplayWarning);
			this.tpDisplay.Controls.Add(this.pbDisplayWarning);
			this.tpDisplay.Controls.Add(this.label4);
			this.tpDisplay.Controls.Add(this.displaySelection1);
			resources.ApplyResources(this.tpDisplay, "tpDisplay");
			this.tpDisplay.Name = "tpDisplay";
			this.tpDisplay.UseVisualStyleBackColor = true;
			// 
			// llDisplayWarning
			// 
			resources.ApplyResources(this.llDisplayWarning, "llDisplayWarning");
			this.llDisplayWarning.Name = "llDisplayWarning";
			this.llDisplayWarning.TabStop = true;
			this.llDisplayWarning.UseCompatibleTextRendering = true;
			this.llDisplayWarning.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.llDisplayWarning_LinkClicked);
			// 
			// pbDisplayWarning
			// 
			this.pbDisplayWarning.Image = global::EmpowerPresenter.Properties.Resources.icon_alert;
			resources.ApplyResources(this.pbDisplayWarning, "pbDisplayWarning");
			this.pbDisplayWarning.Name = "pbDisplayWarning";
			this.pbDisplayWarning.TabStop = false;
			// 
			// label4
			// 
			resources.ApplyResources(this.label4, "label4");
			this.label4.Name = "label4";
			// 
			// tpOther
			// 
			this.tpOther.Controls.Add(this.gbSongs);
			this.tpOther.Controls.Add(this.cbHideContent);
			this.tpOther.Controls.Add(this.cbBackdrop);
			this.tpOther.Controls.Add(this.groupBox2);
			resources.ApplyResources(this.tpOther, "tpOther");
			this.tpOther.Name = "tpOther";
			this.tpOther.UseVisualStyleBackColor = true;
			// 
			// gbSongs
			// 
			this.gbSongs.Controls.Add(this.sliderOpacity);
			this.gbSongs.Controls.Add(this.btnOptions);
			this.gbSongs.Controls.Add(this.label8);
			this.gbSongs.Controls.Add(this.label7);
			this.gbSongs.Controls.Add(this.label6);
			this.gbSongs.Controls.Add(this.label5);
			this.gbSongs.Controls.Add(this.btnFont);
			this.gbSongs.Controls.Add(this.btnImage);
			resources.ApplyResources(this.gbSongs, "gbSongs");
			this.gbSongs.Name = "gbSongs";
			this.gbSongs.TabStop = false;
			// 
			// sliderOpacity
			// 
			resources.ApplyResources(this.sliderOpacity, "sliderOpacity");
			this.sliderOpacity.BackColor = System.Drawing.Color.Transparent;
			this.sliderOpacity.ImedUpdate = true;
			this.sliderOpacity.MaximumSize = new System.Drawing.Size(27, 95);
			this.sliderOpacity.MinimumSize = new System.Drawing.Size(27, 95);
			this.sliderOpacity.Name = "sliderOpacity";
			this.sliderOpacity.Value = 0;
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
			this.btnOptions.Click += new System.EventHandler(this.btnOptions_Click);
			// 
			// label8
			// 
			resources.ApplyResources(this.label8, "label8");
			this.label8.Cursor = System.Windows.Forms.Cursors.Hand;
			this.label8.Name = "label8";
			this.label8.Click += new System.EventHandler(this.btnOptions_Click);
			// 
			// label7
			// 
			resources.ApplyResources(this.label7, "label7");
			this.label7.Cursor = System.Windows.Forms.Cursors.Hand;
			this.label7.Name = "label7";
			// 
			// label6
			// 
			resources.ApplyResources(this.label6, "label6");
			this.label6.Cursor = System.Windows.Forms.Cursors.Hand;
			this.label6.Name = "label6";
			this.label6.Click += new System.EventHandler(this.btnFont_Click);
			// 
			// label5
			// 
			resources.ApplyResources(this.label5, "label5");
			this.label5.Cursor = System.Windows.Forms.Cursors.Hand;
			this.label5.Name = "label5";
			this.label5.Click += new System.EventHandler(this.btnImage_Click);
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
			this.btnFont.Click += new System.EventHandler(this.btnFont_Click);
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
			this.btnImage.Click += new System.EventHandler(this.btnImage_Click);
			// 
			// cbHideContent
			// 
			resources.ApplyResources(this.cbHideContent, "cbHideContent");
			this.cbHideContent.Name = "cbHideContent";
			this.cbHideContent.UseVisualStyleBackColor = true;
			// 
			// cbBackdrop
			// 
			resources.ApplyResources(this.cbBackdrop, "cbBackdrop");
			this.cbBackdrop.Name = "cbBackdrop";
			this.cbBackdrop.UseVisualStyleBackColor = true;
			// 
			// Settings
			// 
			this.AcceptButton = this.btnSave;
			resources.ApplyResources(this, "$this");
			this.CancelButton = this.btnClose;
			this.Controls.Add(this.tabControl1);
			this.Controls.Add(this.pnlBottom);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "Settings";
			this.ShowInTaskbar = false;
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
			this.groupBox2.ResumeLayout(false);
			this.pnlBottom.ResumeLayout(false);
			this.tabControl1.ResumeLayout(false);
			this.tpDisplay.ResumeLayout(false);
			this.tpDisplay.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.pbDisplayWarning)).EndInit();
			this.tpOther.ResumeLayout(false);
			this.tpOther.PerformLayout();
			this.gbSongs.ResumeLayout(false);
			this.gbSongs.PerformLayout();
			this.ResumeLayout(false);

		}
		#endregion

		private void btnSave_Click(object sender, System.EventArgs e)
		{
			Program.ConfigHelper.PresentationMonitor = displaySelection1.CurrentScreenIndex;
			Program.ConfigHelper.BiblePrimaryTranslation = getPrimaryTranslation();
			Program.ConfigHelper.BibleSecondaryTranslation = getSecondaryTranslation();
			Program.ConfigHelper.UseBlackBackdrop = cbBackdrop.Checked;
			Program.ConfigHelper.HideContentOnMin = cbHideContent.Checked;

			DialogResult = DialogResult.OK;
			this.Close();
		}
		private void btnClose_Click(object sender, System.EventArgs e)
		{
			DialogResult = DialogResult.Cancel;
			this.Close();
		}
		private void llDisplayWarning_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			try
			{
				string loc = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "help\\setting_display.html");
				System.Diagnostics.Process.Start(loc);
			}catch { }
		}
		private void btnImage_Click(object sender, EventArgs e)
		{
			#if DEMO
			new DemoVersionOnly("Changing background").ShowDialog();
			#else
			ImageSelection images = new ImageSelection();
			if (DialogResult.OK == images.ShowDialog())
			{
				Program.ConfigHelper.SongDefaultImage = images.SelectedItem.ImageId;
				Program.ConfigHelper.NotifySongDefaultsChanged();
			}
			#endif
		}
		private void btnFont_Click(object sender, EventArgs e)
		{
			#if DEMO
			new DemoVersionOnly("Changing font").ShowDialog();
			#else
			FontSelection fsForm = new FontSelection();
			fsForm.LoadFont(PresenterFont.GetFontFromDatabase(-2));
			if (fsForm.ShowDialog() == DialogResult.OK)
				PresenterFont.SaveFontToDatabase(-2, fsForm.PresenterFont);
			Program.ConfigHelper.NotifySongDefaultsChanged();
			#endif
		}
		private void btnOptions_Click(object sender, EventArgs e)
		{
			SongFormatDlg f = new SongFormatDlg();
			f.LoadFromStr(Program.ConfigHelper.SongDefaultFormat);
			if (f.ShowDialog() == DialogResult.OK)
			{
				#if DEMO
				new DemoVersionOnly("Changing format").ShowDialog();
				#else
				Program.ConfigHelper.SongDefaultFormat = f.SaveToStr();
				Program.ConfigHelper.NotifySongDefaultsChanged();
				#endif
			}
		}
		private void sliderOpacity_ValueChanged(object sender, EventArgs e)
		{
			int v = sliderOpacity.Value;
			if (v > 98) // math fix
				v = 98;
			if (v < 3)
				v = 3;
			int opacityval = (int)((double)v * 512 / 100) - 255;

			// Save
			#if DEMO
			new DemoVersionOnly("Changing defaults").ShowDialog();
			#else
			Program.ConfigHelper.SongDefaultOpacity = opacityval;
			Program.ConfigHelper.NotifySongDefaultsChanged();
			#endif
		}
	}
}