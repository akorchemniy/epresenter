/* ePresenter is licensed under the GPLV3 -- see the 'COPYING' file details.
   Copyright (C) 2006 Alex Korchemniy */
using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace EmpowerPresenter
{
    public class FontSelection : System.Windows.Forms.Form
    {
        #region Designer Variables

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        internal System.Windows.Forms.GroupBox gbAlignment;
        internal System.Windows.Forms.GroupBox gbEffects;
        private System.Windows.Forms.ComboBox txtFont;
        private System.Windows.Forms.ComboBox txtFontStyle;
        private System.Windows.Forms.TextBox txtSize;
        private System.Windows.Forms.ListBox lbFontStyle;
        private System.Windows.Forms.ListBox lbSize;
        private System.Windows.Forms.PictureBox pbTopLeft;
        private System.Windows.Forms.PictureBox pbTopCenter;
        private System.Windows.Forms.PictureBox pbTopRight;
        private System.Windows.Forms.PictureBox pbMiddleRight;
        private System.Windows.Forms.PictureBox pbMiddleLeft;
        private System.Windows.Forms.PictureBox pbMiddleCenter;
        private System.Windows.Forms.PictureBox pbBottomCenter;
        private System.Windows.Forms.PictureBox pbBottomLeft;
        private System.Windows.Forms.PictureBox pbBottomRight;
        internal System.Windows.Forms.CheckBox cbShadow;
        internal System.Windows.Forms.CheckBox cbOutline;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.GroupBox groupBox2;
        private Syncfusion.Windows.Forms.Tools.ButtonEdit txtColor;
        private Syncfusion.Windows.Forms.Tools.ButtonEditChildButton btnShowPicker;
        private Syncfusion.Windows.Forms.PopupControlContainer popupControlContainer1;
        private Syncfusion.Windows.Forms.ColorUIControl colorUI;
        private System.Windows.Forms.CheckBox cbAutomatic;
        private System.Windows.Forms.ListBox lbFonts;
        internal Syncfusion.Windows.Forms.ColorPickerButton cpOutline;
        internal Syncfusion.Windows.Forms.ColorPickerButton cpShadow;
        internal System.Windows.Forms.CheckBox cbDoubleSpace;
        private System.ComponentModel.IContainer components;

        #endregion

        //////////////////////////////////////////////////////////////////
        public FontSelection()
        {
#if DEMO
            new DemoVersionOnly("Changing font").ShowDialog();
#else
            InitializeComponent();
            Program.Presenter.RegisterExKeyOwnerForm(this);

            // set auto complete
            txtFont.DataSource = lbFonts.Items;
            txtFontStyle.DataSource = lbFontStyle.Items;

            // set tag values
            pbTopLeft.Tag        = new FontAlignment(HorizontalAlignment.Left, VerticalAlignment.Top);
            pbTopCenter.Tag        = new FontAlignment(HorizontalAlignment.Center, VerticalAlignment.Top);
            pbTopRight.Tag        = new FontAlignment(HorizontalAlignment.Right, VerticalAlignment.Top);
            pbMiddleLeft.Tag    = new FontAlignment(HorizontalAlignment.Left, VerticalAlignment.Middle);
            pbMiddleCenter.Tag    = new FontAlignment(HorizontalAlignment.Center, VerticalAlignment.Middle);
            pbMiddleRight.Tag    = new FontAlignment(HorizontalAlignment.Right, VerticalAlignment.Middle);
            pbBottomLeft.Tag    = new FontAlignment(HorizontalAlignment.Left, VerticalAlignment.Bottom);
            pbBottomCenter.Tag    = new FontAlignment(HorizontalAlignment.Center, VerticalAlignment.Bottom);
            pbBottomRight.Tag    = new FontAlignment(HorizontalAlignment.Right, VerticalAlignment.Bottom);

            // load the fonts
            Graphics g = Graphics.FromImage(new Bitmap(1,1));
            FontFamily[] ff = FontFamily.GetFamilies(g);
            lbFonts.BeginUpdate();
            for(int i = 0; i < ff.Length; i++)
            {
                lbFonts.Items.Add(ff[i]);
            }
            lbFonts.EndUpdate();

            this.CancelButton = btnClose;
#endif
        }
        #region Windows Form Designer generated code
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FontSelection));
            this.label1 = new System.Windows.Forms.Label();
            this.txtFont = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtFontStyle = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtSize = new System.Windows.Forms.TextBox();
            this.lbFontStyle = new System.Windows.Forms.ListBox();
            this.lbSize = new System.Windows.Forms.ListBox();
            this.cbShadow = new System.Windows.Forms.CheckBox();
            this.cbOutline = new System.Windows.Forms.CheckBox();
            this.gbAlignment = new System.Windows.Forms.GroupBox();
            this.pbMiddleLeft = new System.Windows.Forms.PictureBox();
            this.pbMiddleRight = new System.Windows.Forms.PictureBox();
            this.pbTopLeft = new System.Windows.Forms.PictureBox();
            this.pbBottomCenter = new System.Windows.Forms.PictureBox();
            this.pbTopRight = new System.Windows.Forms.PictureBox();
            this.pbTopCenter = new System.Windows.Forms.PictureBox();
            this.pbBottomRight = new System.Windows.Forms.PictureBox();
            this.pbBottomLeft = new System.Windows.Forms.PictureBox();
            this.pbMiddleCenter = new System.Windows.Forms.PictureBox();
            this.gbEffects = new System.Windows.Forms.GroupBox();
            this.cpShadow = new Syncfusion.Windows.Forms.ColorPickerButton();
            this.cpOutline = new Syncfusion.Windows.Forms.ColorPickerButton();
            this.cbDoubleSpace = new System.Windows.Forms.CheckBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.txtColor = new Syncfusion.Windows.Forms.Tools.ButtonEdit();
            this.btnShowPicker = new Syncfusion.Windows.Forms.Tools.ButtonEditChildButton();
            this.popupControlContainer1 = new Syncfusion.Windows.Forms.PopupControlContainer(this.components);
            this.colorUI = new Syncfusion.Windows.Forms.ColorUIControl();
            this.cbAutomatic = new System.Windows.Forms.CheckBox();
            this.lbFonts = new System.Windows.Forms.ListBox();
            this.gbAlignment.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbMiddleLeft)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbMiddleRight)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbTopLeft)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbBottomCenter)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbTopRight)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbTopCenter)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbBottomRight)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbBottomLeft)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbMiddleCenter)).BeginInit();
            this.gbEffects.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtColor)).BeginInit();
            this.txtColor.SuspendLayout();
            this.popupControlContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AccessibleDescription = null;
            this.label1.AccessibleName = null;
            resources.ApplyResources(this.label1, "label1");
            this.label1.Font = null;
            this.label1.Name = "label1";
            // 
            // txtFont
            // 
            this.txtFont.AccessibleDescription = null;
            this.txtFont.AccessibleName = null;
            resources.ApplyResources(this.txtFont, "txtFont");
            this.txtFont.BackgroundImage = null;
            this.txtFont.DropDownStyle = System.Windows.Forms.ComboBoxStyle.Simple;
            this.txtFont.Font = null;
            this.txtFont.Name = "txtFont";
            this.txtFont.Validating += new System.ComponentModel.CancelEventHandler(this.txtFont_Validating);
            this.txtFont.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtFont_KeyPress);
            // 
            // label2
            // 
            this.label2.AccessibleDescription = null;
            this.label2.AccessibleName = null;
            resources.ApplyResources(this.label2, "label2");
            this.label2.Font = null;
            this.label2.Name = "label2";
            // 
            // txtFontStyle
            // 
            this.txtFontStyle.AccessibleDescription = null;
            this.txtFontStyle.AccessibleName = null;
            resources.ApplyResources(this.txtFontStyle, "txtFontStyle");
            this.txtFontStyle.BackgroundImage = null;
            this.txtFontStyle.DropDownStyle = System.Windows.Forms.ComboBoxStyle.Simple;
            this.txtFontStyle.Font = null;
            this.txtFontStyle.Name = "txtFontStyle";
            this.txtFontStyle.Validating += new System.ComponentModel.CancelEventHandler(this.txtFontStyle_Validating);
            this.txtFontStyle.Validated += new System.EventHandler(this.txtFontStyle_Validated);
            this.txtFontStyle.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtFontStyle_KeyPress);
            // 
            // label3
            // 
            this.label3.AccessibleDescription = null;
            this.label3.AccessibleName = null;
            resources.ApplyResources(this.label3, "label3");
            this.label3.Font = null;
            this.label3.Name = "label3";
            // 
            // txtSize
            // 
            resources.ApplyResources(this.txtSize, "txtSize");
            this.txtSize.BackgroundImage = null;
            this.txtSize.Name = "txtSize";
            this.txtSize.TextChanged += new EventHandler(txtSize_TextChanged);
            this.txtSize.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtSize_KeyPress);
            // 
            // lbFontStyle
            // 
            this.lbFontStyle.AccessibleDescription = null;
            this.lbFontStyle.AccessibleName = null;
            resources.ApplyResources(this.lbFontStyle, "lbFontStyle");
            this.lbFontStyle.BackgroundImage = null;
            this.lbFontStyle.Font = null;
            this.lbFontStyle.Items.AddRange(new object[] {
            resources.GetString("lbFontStyle.Items"),
            resources.GetString("lbFontStyle.Items1"),
            resources.GetString("lbFontStyle.Items2"),
            resources.GetString("lbFontStyle.Items3")});
            this.lbFontStyle.Name = "lbFontStyle";
            this.lbFontStyle.TabStop = false;
            this.lbFontStyle.SelectedIndexChanged += new System.EventHandler(this.lbFontStyle_SelectedIndexChanged);
            // 
            // lbSize
            // 
            this.lbSize.AccessibleDescription = null;
            this.lbSize.AccessibleName = null;
            resources.ApplyResources(this.lbSize, "lbSize");
            this.lbSize.BackgroundImage = null;
            this.lbSize.Font = null;
            this.lbSize.Items.AddRange(new object[] {
            resources.GetString("lbSize.Items"),
            resources.GetString("lbSize.Items1"),
            resources.GetString("lbSize.Items2"),
            resources.GetString("lbSize.Items3"),
            resources.GetString("lbSize.Items4"),
            resources.GetString("lbSize.Items5"),
            resources.GetString("lbSize.Items6"),
            resources.GetString("lbSize.Items7"),
            resources.GetString("lbSize.Items8"),
            resources.GetString("lbSize.Items9"),
            resources.GetString("lbSize.Items10"),
            resources.GetString("lbSize.Items11"),
            resources.GetString("lbSize.Items12"),
            resources.GetString("lbSize.Items13"),
            resources.GetString("lbSize.Items14"),
            resources.GetString("lbSize.Items15"),
            resources.GetString("lbSize.Items16"),
            resources.GetString("lbSize.Items17"),
            resources.GetString("lbSize.Items18"),
            resources.GetString("lbSize.Items19"),
            resources.GetString("lbSize.Items20"),
            resources.GetString("lbSize.Items21")});
            this.lbSize.Name = "lbSize";
            this.lbSize.TabStop = false;
            this.lbSize.SelectedIndexChanged += new System.EventHandler(this.lbSize_SelectedIndexChanged);
            // 
            // cbShadow
            // 
            this.cbShadow.AccessibleDescription = null;
            this.cbShadow.AccessibleName = null;
            resources.ApplyResources(this.cbShadow, "cbShadow");
            this.cbShadow.BackgroundImage = null;
            this.cbShadow.Checked = true;
            this.cbShadow.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbShadow.Font = null;
            this.cbShadow.Name = "cbShadow";
            this.cbShadow.CheckStateChanged += new System.EventHandler(this.cbShadow_CheckStateChanged);
            // 
            // cbOutline
            // 
            this.cbOutline.AccessibleDescription = null;
            this.cbOutline.AccessibleName = null;
            resources.ApplyResources(this.cbOutline, "cbOutline");
            this.cbOutline.BackgroundImage = null;
            this.cbOutline.Checked = true;
            this.cbOutline.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbOutline.Font = null;
            this.cbOutline.Name = "cbOutline";
            this.cbOutline.CheckStateChanged += new System.EventHandler(this.cbOutline_CheckStateChanged);
            // 
            // gbAlignment
            // 
            this.gbAlignment.AccessibleDescription = null;
            this.gbAlignment.AccessibleName = null;
            resources.ApplyResources(this.gbAlignment, "gbAlignment");
            this.gbAlignment.BackgroundImage = null;
            this.gbAlignment.Controls.Add(this.pbMiddleLeft);
            this.gbAlignment.Controls.Add(this.pbMiddleRight);
            this.gbAlignment.Controls.Add(this.pbTopLeft);
            this.gbAlignment.Controls.Add(this.pbBottomCenter);
            this.gbAlignment.Controls.Add(this.pbTopRight);
            this.gbAlignment.Controls.Add(this.pbTopCenter);
            this.gbAlignment.Controls.Add(this.pbBottomRight);
            this.gbAlignment.Controls.Add(this.pbBottomLeft);
            this.gbAlignment.Controls.Add(this.pbMiddleCenter);
            this.gbAlignment.Font = null;
            this.gbAlignment.Name = "gbAlignment";
            this.gbAlignment.TabStop = false;
            // 
            // pbMiddleLeft
            // 
            this.pbMiddleLeft.AccessibleDescription = null;
            this.pbMiddleLeft.AccessibleName = null;
            resources.ApplyResources(this.pbMiddleLeft, "pbMiddleLeft");
            this.pbMiddleLeft.BackColor = System.Drawing.Color.Transparent;
            this.pbMiddleLeft.BackgroundImage = null;
            this.pbMiddleLeft.Font = null;
            this.pbMiddleLeft.ImageLocation = null;
            this.pbMiddleLeft.Name = "pbMiddleLeft";
            this.pbMiddleLeft.TabStop = false;
            this.pbMiddleLeft.MouseLeave += new System.EventHandler(this.pbAlignmentMouseLeave);
            this.pbMiddleLeft.Click += new System.EventHandler(this.pbAlignmentClick);
            this.pbMiddleLeft.MouseEnter += new System.EventHandler(this.pbAlignmentMouseEnter);
            // 
            // pbMiddleRight
            // 
            this.pbMiddleRight.AccessibleDescription = null;
            this.pbMiddleRight.AccessibleName = null;
            resources.ApplyResources(this.pbMiddleRight, "pbMiddleRight");
            this.pbMiddleRight.BackColor = System.Drawing.Color.Transparent;
            this.pbMiddleRight.BackgroundImage = null;
            this.pbMiddleRight.Font = null;
            this.pbMiddleRight.ImageLocation = null;
            this.pbMiddleRight.Name = "pbMiddleRight";
            this.pbMiddleRight.TabStop = false;
            this.pbMiddleRight.MouseLeave += new System.EventHandler(this.pbAlignmentMouseLeave);
            this.pbMiddleRight.Click += new System.EventHandler(this.pbAlignmentClick);
            this.pbMiddleRight.MouseEnter += new System.EventHandler(this.pbAlignmentMouseEnter);
            // 
            // pbTopLeft
            // 
            this.pbTopLeft.AccessibleDescription = null;
            this.pbTopLeft.AccessibleName = null;
            resources.ApplyResources(this.pbTopLeft, "pbTopLeft");
            this.pbTopLeft.BackColor = System.Drawing.Color.Transparent;
            this.pbTopLeft.BackgroundImage = null;
            this.pbTopLeft.Font = null;
            this.pbTopLeft.ImageLocation = null;
            this.pbTopLeft.Name = "pbTopLeft";
            this.pbTopLeft.TabStop = false;
            this.pbTopLeft.MouseLeave += new System.EventHandler(this.pbAlignmentMouseLeave);
            this.pbTopLeft.Click += new System.EventHandler(this.pbAlignmentClick);
            this.pbTopLeft.MouseEnter += new System.EventHandler(this.pbAlignmentMouseEnter);
            // 
            // pbBottomCenter
            // 
            this.pbBottomCenter.AccessibleDescription = null;
            this.pbBottomCenter.AccessibleName = null;
            resources.ApplyResources(this.pbBottomCenter, "pbBottomCenter");
            this.pbBottomCenter.BackColor = System.Drawing.Color.Transparent;
            this.pbBottomCenter.BackgroundImage = null;
            this.pbBottomCenter.Font = null;
            this.pbBottomCenter.ImageLocation = null;
            this.pbBottomCenter.Name = "pbBottomCenter";
            this.pbBottomCenter.TabStop = false;
            this.pbBottomCenter.MouseLeave += new System.EventHandler(this.pbAlignmentMouseLeave);
            this.pbBottomCenter.Click += new System.EventHandler(this.pbAlignmentClick);
            this.pbBottomCenter.MouseEnter += new System.EventHandler(this.pbAlignmentMouseEnter);
            // 
            // pbTopRight
            // 
            this.pbTopRight.AccessibleDescription = null;
            this.pbTopRight.AccessibleName = null;
            resources.ApplyResources(this.pbTopRight, "pbTopRight");
            this.pbTopRight.BackColor = System.Drawing.Color.Transparent;
            this.pbTopRight.BackgroundImage = null;
            this.pbTopRight.Font = null;
            this.pbTopRight.ImageLocation = null;
            this.pbTopRight.Name = "pbTopRight";
            this.pbTopRight.TabStop = false;
            this.pbTopRight.MouseLeave += new System.EventHandler(this.pbAlignmentMouseLeave);
            this.pbTopRight.Click += new System.EventHandler(this.pbAlignmentClick);
            this.pbTopRight.MouseEnter += new System.EventHandler(this.pbAlignmentMouseEnter);
            // 
            // pbTopCenter
            // 
            this.pbTopCenter.AccessibleDescription = null;
            this.pbTopCenter.AccessibleName = null;
            resources.ApplyResources(this.pbTopCenter, "pbTopCenter");
            this.pbTopCenter.BackColor = System.Drawing.Color.Transparent;
            this.pbTopCenter.BackgroundImage = null;
            this.pbTopCenter.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pbTopCenter.Font = null;
            this.pbTopCenter.ImageLocation = null;
            this.pbTopCenter.Name = "pbTopCenter";
            this.pbTopCenter.TabStop = false;
            this.pbTopCenter.MouseLeave += new System.EventHandler(this.pbAlignmentMouseLeave);
            this.pbTopCenter.Click += new System.EventHandler(this.pbAlignmentClick);
            this.pbTopCenter.MouseEnter += new System.EventHandler(this.pbAlignmentMouseEnter);
            // 
            // pbBottomRight
            // 
            this.pbBottomRight.AccessibleDescription = null;
            this.pbBottomRight.AccessibleName = null;
            resources.ApplyResources(this.pbBottomRight, "pbBottomRight");
            this.pbBottomRight.BackColor = System.Drawing.Color.Transparent;
            this.pbBottomRight.BackgroundImage = null;
            this.pbBottomRight.Font = null;
            this.pbBottomRight.ImageLocation = null;
            this.pbBottomRight.Name = "pbBottomRight";
            this.pbBottomRight.TabStop = false;
            this.pbBottomRight.MouseLeave += new System.EventHandler(this.pbAlignmentMouseLeave);
            this.pbBottomRight.Click += new System.EventHandler(this.pbAlignmentClick);
            this.pbBottomRight.MouseEnter += new System.EventHandler(this.pbAlignmentMouseEnter);
            // 
            // pbBottomLeft
            // 
            this.pbBottomLeft.AccessibleDescription = null;
            this.pbBottomLeft.AccessibleName = null;
            resources.ApplyResources(this.pbBottomLeft, "pbBottomLeft");
            this.pbBottomLeft.BackColor = System.Drawing.Color.Transparent;
            this.pbBottomLeft.BackgroundImage = null;
            this.pbBottomLeft.Font = null;
            this.pbBottomLeft.ImageLocation = null;
            this.pbBottomLeft.Name = "pbBottomLeft";
            this.pbBottomLeft.TabStop = false;
            this.pbBottomLeft.MouseLeave += new System.EventHandler(this.pbAlignmentMouseLeave);
            this.pbBottomLeft.Click += new System.EventHandler(this.pbAlignmentClick);
            this.pbBottomLeft.MouseEnter += new System.EventHandler(this.pbAlignmentMouseEnter);
            // 
            // pbMiddleCenter
            // 
            this.pbMiddleCenter.AccessibleDescription = null;
            this.pbMiddleCenter.AccessibleName = null;
            resources.ApplyResources(this.pbMiddleCenter, "pbMiddleCenter");
            this.pbMiddleCenter.BackColor = System.Drawing.Color.Transparent;
            this.pbMiddleCenter.BackgroundImage = null;
            this.pbMiddleCenter.Font = null;
            this.pbMiddleCenter.ImageLocation = null;
            this.pbMiddleCenter.Name = "pbMiddleCenter";
            this.pbMiddleCenter.TabStop = false;
            this.pbMiddleCenter.MouseLeave += new System.EventHandler(this.pbAlignmentMouseLeave);
            this.pbMiddleCenter.Click += new System.EventHandler(this.pbAlignmentClick);
            this.pbMiddleCenter.MouseEnter += new System.EventHandler(this.pbAlignmentMouseEnter);
            // 
            // gbEffects
            // 
            this.gbEffects.AccessibleDescription = null;
            this.gbEffects.AccessibleName = null;
            resources.ApplyResources(this.gbEffects, "gbEffects");
            this.gbEffects.BackgroundImage = null;
            this.gbEffects.Controls.Add(this.cpShadow);
            this.gbEffects.Controls.Add(this.cpOutline);
            this.gbEffects.Controls.Add(this.cbShadow);
            this.gbEffects.Controls.Add(this.cbOutline);
            this.gbEffects.Controls.Add(this.cbDoubleSpace);
            this.gbEffects.Font = null;
            this.gbEffects.Name = "gbEffects";
            this.gbEffects.TabStop = false;
            // 
            // cpShadow
            // 
            this.cpShadow.AccessibleDescription = null;
            this.cpShadow.AccessibleName = null;
            resources.ApplyResources(this.cpShadow, "cpShadow");
            this.cpShadow.BackColor = System.Drawing.Color.White;
            this.cpShadow.BackgroundImage = null;
            this.cpShadow.ColorUISize = new System.Drawing.Size(208, 230);
            this.cpShadow.Font = null;
            this.cpShadow.Name = "cpShadow";
            this.cpShadow.SelectedColorGroup = Syncfusion.Windows.Forms.ColorUISelectedGroup.None;
            this.cpShadow.UseVisualStyleBackColor = false;
            this.cpShadow.ColorSelected += new System.EventHandler(this.cpShadow_ColorSelected);
            // 
            // cpOutline
            // 
            this.cpOutline.AccessibleDescription = null;
            this.cpOutline.AccessibleName = null;
            resources.ApplyResources(this.cpOutline, "cpOutline");
            this.cpOutline.BackColor = System.Drawing.Color.Black;
            this.cpOutline.BackgroundImage = null;
            this.cpOutline.ColorGroups = ((Syncfusion.Windows.Forms.ColorUIGroups)((Syncfusion.Windows.Forms.ColorUIGroups.CustomColors | Syncfusion.Windows.Forms.ColorUIGroups.StandardColors)));
            this.cpOutline.ColorUISize = new System.Drawing.Size(208, 230);
            this.cpOutline.Font = null;
            this.cpOutline.Name = "cpOutline";
            this.cpOutline.SelectedColor = System.Drawing.Color.Black;
            this.cpOutline.SelectedColorGroup = Syncfusion.Windows.Forms.ColorUISelectedGroup.StandardColors;
            this.cpOutline.UseVisualStyleBackColor = false;
            this.cpOutline.ColorSelected += new System.EventHandler(this.cpOutline_ColorSelected);
            // 
            // cbDoubleSpace
            // 
            this.cbDoubleSpace.AccessibleDescription = null;
            this.cbDoubleSpace.AccessibleName = null;
            resources.ApplyResources(this.cbDoubleSpace, "cbDoubleSpace");
            this.cbDoubleSpace.BackgroundImage = null;
            this.cbDoubleSpace.Checked = true;
            this.cbDoubleSpace.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbDoubleSpace.Font = null;
            this.cbDoubleSpace.Name = "cbDoubleSpace";
            this.cbDoubleSpace.CheckedChanged += new System.EventHandler(this.cbDoubleSpace_CheckedChanged);
            // 
            // btnSave
            // 
            this.btnSave.AccessibleDescription = null;
            this.btnSave.AccessibleName = null;
            resources.ApplyResources(this.btnSave, "btnSave");
            this.btnSave.BackgroundImage = null;
            this.btnSave.Font = null;
            this.btnSave.Name = "btnSave";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnClose
            // 
            this.btnClose.AccessibleDescription = null;
            this.btnClose.AccessibleName = null;
            resources.ApplyResources(this.btnClose, "btnClose");
            this.btnClose.BackgroundImage = null;
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Font = null;
            this.btnClose.Name = "btnClose";
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.AccessibleDescription = null;
            this.groupBox2.AccessibleName = null;
            resources.ApplyResources(this.groupBox2, "groupBox2");
            this.groupBox2.BackgroundImage = null;
            this.groupBox2.Controls.Add(this.txtColor);
            this.groupBox2.Font = null;
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.TabStop = false;
            // 
            // txtColor
            // 
            this.txtColor.AccessibleDescription = null;
            this.txtColor.AccessibleName = null;
            resources.ApplyResources(this.txtColor, "txtColor");
            this.txtColor.BackgroundImage = null;
            this.txtColor.Border3DStyle = System.Windows.Forms.Border3DStyle.Flat;
            this.txtColor.Buttons.Add(this.btnShowPicker);
            this.txtColor.Controls.Add(this.btnShowPicker);
            this.txtColor.FlatBorderColor = System.Drawing.Color.Black;
            this.txtColor.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.txtColor.Font = null;
            this.txtColor.Name = "txtColor";
            this.txtColor.SelectionLength = 0;
            this.txtColor.SelectionStart = 0;
            this.txtColor.ShowTextBox = true;
            this.txtColor.TabStop = false;
            // 
            // 
            // 
            this.txtColor.TextBox.AccessibleDescription = null;
            this.txtColor.TextBox.AccessibleName = null;
            this.txtColor.TextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("txtColor.TextBox.Anchor")));
            this.txtColor.TextBox.BackColor = System.Drawing.SystemColors.ScrollBar;
            this.txtColor.TextBox.BackgroundImage = null;
            this.txtColor.TextBox.BackgroundImageLayout = ((System.Windows.Forms.ImageLayout)(resources.GetObject("txtColor.TextBox.BackgroundImageLayout")));
            this.txtColor.TextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtColor.TextBox.Cursor = System.Windows.Forms.Cursors.Default;
            this.txtColor.TextBox.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("txtColor.TextBox.Dock")));
            this.txtColor.TextBox.Font = null;
            this.txtColor.TextBox.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("txtColor.TextBox.ImeMode")));
            this.txtColor.TextBox.Location = ((System.Drawing.Point)(resources.GetObject("txtColor.TextBox.Location")));
            this.txtColor.TextBox.MaxLength = ((int)(resources.GetObject("txtColor.TextBox.MaxLength")));
            this.txtColor.TextBox.Multiline = ((bool)(resources.GetObject("txtColor.TextBox.Multiline")));
            this.txtColor.TextBox.Name = "";
            this.txtColor.TextBox.PasswordChar = ((char)(resources.GetObject("txtColor.TextBox.PasswordChar")));
            this.txtColor.TextBox.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("txtColor.TextBox.RightToLeft")));
            this.txtColor.TextBox.ScrollBars = ((System.Windows.Forms.ScrollBars)(resources.GetObject("txtColor.TextBox.ScrollBars")));
            this.txtColor.TextBox.Size = ((System.Drawing.Size)(resources.GetObject("txtColor.TextBox.Size")));
            this.txtColor.TextBox.TabIndex = ((int)(resources.GetObject("txtColor.TextBox.TabIndex")));
            this.txtColor.TextBox.TextAlign = ((System.Windows.Forms.HorizontalAlignment)(resources.GetObject("txtColor.TextBox.TextAlign")));
            this.txtColor.TextBox.WordWrap = ((bool)(resources.GetObject("txtColor.TextBox.WordWrap")));
            this.txtColor.TextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtColor_TextBox_KeyPress);
            this.txtColor.TextBox.Enter += new System.EventHandler(this.TextBox_Enter);
            this.txtColor.TextBox.Click += new System.EventHandler(this.TextBox_Click);
            this.txtColor.TextBoxBindings = this.txtColor.TextBox.DataBindings;
            // 
            // btnShowPicker
            // 
            this.btnShowPicker.AccessibleDescription = null;
            this.btnShowPicker.AccessibleName = null;
            resources.ApplyResources(this.btnShowPicker, "btnShowPicker");
            this.btnShowPicker.BackgroundImage = null;
            this.btnShowPicker.ButtonEditParent = this.txtColor;
            this.btnShowPicker.Font = null;
            this.btnShowPicker.Click += new System.EventHandler(this.btnShowPicker_Click);
            // 
            // popupControlContainer1
            // 
            this.popupControlContainer1.AccessibleDescription = null;
            this.popupControlContainer1.AccessibleName = null;
            resources.ApplyResources(this.popupControlContainer1, "popupControlContainer1");
            this.popupControlContainer1.BackgroundImage = null;
            this.popupControlContainer1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.popupControlContainer1.Controls.Add(this.colorUI);
            this.popupControlContainer1.Controls.Add(this.cbAutomatic);
            this.popupControlContainer1.Font = null;
            this.popupControlContainer1.Name = "popupControlContainer1";
            this.popupControlContainer1.ParentControl = this.txtColor;
            this.popupControlContainer1.CloseUp += new Syncfusion.Windows.Forms.PopupClosedEventHandler(this.popupControlContainer1_CloseUp);
            // 
            // colorUI
            // 
            this.colorUI.AccessibleDescription = null;
            this.colorUI.AccessibleName = null;
            resources.ApplyResources(this.colorUI, "colorUI");
            this.colorUI.BackgroundImage = null;
            this.colorUI.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.colorUI.ColorGroups = ((Syncfusion.Windows.Forms.ColorUIGroups)((Syncfusion.Windows.Forms.ColorUIGroups.CustomColors | Syncfusion.Windows.Forms.ColorUIGroups.StandardColors)));
            this.colorUI.Font = null;
            this.colorUI.Name = "colorUI";
            this.colorUI.SelectedColor = System.Drawing.Color.Transparent;
            this.colorUI.ColorSelected += new System.EventHandler(this.colorUI_ColorSelected);
            // 
            // cbAutomatic
            // 
            this.cbAutomatic.AccessibleDescription = null;
            this.cbAutomatic.AccessibleName = null;
            resources.ApplyResources(this.cbAutomatic, "cbAutomatic");
            this.cbAutomatic.BackgroundImage = null;
            this.cbAutomatic.Font = null;
            this.cbAutomatic.Name = "cbAutomatic";
            this.cbAutomatic.CheckStateChanged += new System.EventHandler(this.cbAutomatic_CheckStateChanged);
            // 
            // lbFonts
            // 
            this.lbFonts.AccessibleDescription = null;
            this.lbFonts.AccessibleName = null;
            resources.ApplyResources(this.lbFonts, "lbFonts");
            this.lbFonts.BackgroundImage = null;
            this.lbFonts.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.lbFonts.Font = null;
            this.lbFonts.Name = "lbFonts";
            this.lbFonts.TabStop = false;
            this.lbFonts.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.lbFonts_DrawItem);
            this.lbFonts.SelectedIndexChanged += new System.EventHandler(this.lbFonts_SelectedIndexChanged);
            // 
            // FontSelection
            // 
            this.AccessibleDescription = null;
            this.AccessibleName = null;
            resources.ApplyResources(this, "$this");
            this.BackgroundImage = null;
            this.ControlBox = false;
            this.Controls.Add(this.popupControlContainer1);
            this.Controls.Add(this.lbFonts);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.gbAlignment);
            this.Controls.Add(this.lbFontStyle);
            this.Controls.Add(this.txtFont);
            this.Controls.Add(this.txtFontStyle);
            this.Controls.Add(this.txtSize);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.lbSize);
            this.Controls.Add(this.gbEffects);
            this.Controls.Add(this.groupBox2);
            this.Font = null;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = null;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FontSelection";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FontSelection_KeyDown);
            this.Load += new System.EventHandler(this.FontSelection_Load);
            this.gbAlignment.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pbMiddleLeft)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbMiddleRight)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbTopLeft)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbBottomCenter)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbTopRight)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbTopCenter)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbBottomRight)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbBottomLeft)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbMiddleCenter)).EndInit();
            this.gbEffects.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.txtColor)).EndInit();
            this.txtColor.ResumeLayout(false);
            this.popupControlContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }
        #endregion

        private void FontSelection_Load(object sender, System.EventArgs e)
        {
            //refreshUI(); TODO: clean up
        }
        private void FontSelection_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                btnClose_Click(null, null);
        }

        public void LoadFont(PresenterFont f)
        {
            if (f != null)
                currentFont = (PresenterFont)f.Clone();

            // Select font
            try
            {
                lbFonts.SelectedItem = f.FontFamily;
                txtFont.Text = ((FontFamily)lbFonts.SelectedItem).Name;
            }
            catch { lbFonts.SelectedItem = new FontFamily("Verdana"); }
            
            // Select font style
            if (f.Italic && f.Bold && lbFontStyle.FindStringExact("Bold Italic") != -1)
                lbFontStyle.SelectedIndex = lbFontStyle.FindStringExact("Bold Italic");
            else if (f.Italic && lbFontStyle.FindStringExact("Italic") != -1)
                lbFontStyle.SelectedIndex = lbFontStyle.FindStringExact("Italic");
            else if (f.Bold && lbFontStyle.FindStringExact("Bold") != -1)
                lbFontStyle.SelectedIndex = lbFontStyle.FindStringExact("Bold");
            else if (lbFontStyle.FindStringExact("Regular") != -1)
                lbFontStyle.SelectedIndex = lbFontStyle.FindStringExact("Regular");

            // font size
            int sizeIndex = lbSize.FindStringExact(f.SizeInPoints.ToString());
            if (sizeIndex != -1)
                lbSize.SelectedIndex = sizeIndex;
            else
            {
                string sizeString = f.SizeInPoints.ToString();
                txtSize.Text = sizeString;
            }

            cbShadow.Checked = f.Shadow;
            cbOutline.Checked = f.Outline;
            cbDoubleSpace.Checked = f.DoubleSpace;
            cpShadow.BackColor = f.ShadowColor;
            cpOutline.BackColor = f.OutlineColor;
            cpShadow.SelectedColor = f.ShadowColor;
            cpOutline.SelectedColor = f.OutlineColor;

            refreshAlignment();
            refreshColor();
        }
        private void refreshColor()
        {
            cbAutomatic.Checked = (currentFont.Color == Color.Transparent);
            colorUI.SelectedColor = currentFont.Color;

            // select tab
            if (currentFont.Color.IsNamedColor)
                colorUI.SelectedColorGroup = Syncfusion.Windows.Forms.ColorUISelectedGroup.StandardColors;
            else
                colorUI.SelectedColorGroup = Syncfusion.Windows.Forms.ColorUISelectedGroup.CustomColors;

            if (currentFont.Color != Color.Transparent)
            {
                txtColor.TextBox.Text = "";
                txtColor.TextBox.BackColor = currentFont.Color;
            }
            else
            {
                txtColor.TextBox.Text = cbAutomatic.Text;
                txtColor.TextBox.BackColor = Color.White;
            }

            if (currentFont.Outline)
                cpOutline.Visible = !(currentFont.Color == Color.Transparent);
            else
                cpOutline.Visible = false;

            if (currentFont.Shadow)
                cpShadow.Visible = !(currentFont.Color == Color.Transparent);
            else
                cpShadow.Visible = false;
        }
        private void refreshAlignment()
        {
            refreshAlignmentFocus(pbTopLeft);
            refreshAlignmentFocus(pbTopCenter);
            refreshAlignmentFocus(pbTopRight);
            refreshAlignmentFocus(pbMiddleLeft);
            refreshAlignmentFocus(pbMiddleCenter);
            refreshAlignmentFocus(pbMiddleRight);
            refreshAlignmentFocus(pbBottomLeft);
            refreshAlignmentFocus(pbBottomCenter);
            refreshAlignmentFocus(pbBottomRight);
        }
        private void refreshAlignmentFocus(PictureBox pb)
        {
            if (((FontAlignment)pb.Tag).Equals(currentFont.FontAlignment))
            {
                // highlight
                pb.BackColor = Color.PaleGoldenrod;
                pb.BorderStyle = BorderStyle.FixedSingle;
            }
            else
            {
                // unselect
                pb.BackColor = Color.Transparent;
                pb.BorderStyle = BorderStyle.None;
            }
        }

        #region Control Events

        // Font
        private void lbFonts_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            lbFontStyle.Items.Clear();
            FontFamily ff = (FontFamily)lbFonts.SelectedItem;
            if (ff.IsStyleAvailable(FontStyle.Regular))
                lbFontStyle.Items.Add("Regular");
            if(ff.IsStyleAvailable(FontStyle.Bold)) 
                lbFontStyle.Items.Add("Bold");
            if(ff.IsStyleAvailable(FontStyle.Italic)) 
                lbFontStyle.Items.Add("Italic");
            if(ff.IsStyleAvailable(FontStyle.Bold | FontStyle.Italic)) 
                lbFontStyle.Items.Add("Bold Italic");
            
            if (txtFont.ContainsFocus == false)
                txtFont.Text = ((FontFamily)lbFonts.SelectedItem).Name;

            // update font name
            currentFont.FontName = ((FontFamily)lbFonts.SelectedItem).Name;
            lbFonts.Refresh(); 
        }
        private void txtFont_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                FontFamily ff = new FontFamily(txtFont.Text);
                lbFonts.SelectedItem = ff;
                txtFont.Text = ff.Name;
            }
            catch(ArgumentException)
            {
                System.Diagnostics.Trace.WriteLine("Argument exception: " + txtFont.Text);
            }
        }
        private void txtFont_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                e.Handled = true;
                this.SelectNextControl(txtFont, true, true, false, true);
            }
        }
        private void lbFonts_DrawItem(object sender, System.Windows.Forms.DrawItemEventArgs e)
        {
            if (e.Index > -1)
            {
                FontFamily ff = (FontFamily)lbFonts.Items[e.Index];
                FontStyle fs = FontStyle.Regular;
                if (ff.IsStyleAvailable(FontStyle.Regular))
                    fs = FontStyle.Regular; 
                if(!ff.IsStyleAvailable(fs)) 
                    fs = FontStyle.Italic; 
                if(!ff.IsStyleAvailable(fs)) 
                    fs = FontStyle.Bold; 
                if(!ff.IsStyleAvailable(fs)) 
                    fs = FontStyle.Strikeout; 
                if(!ff.IsStyleAvailable(fs)) 
                    fs = FontStyle.Underline; 
                Font font = new Font(ff,10,fs); 
                e.DrawBackground(); e.DrawFocusRectangle(); 
                if (lbFonts.SelectedIndex == e.Index)
                    e.Graphics.DrawString(font.Name,font,Brushes.White,e.Bounds.X + 4,e.Bounds.Y + 4);
                else
                    e.Graphics.DrawString(font.Name,font,Brushes.Black,e.Bounds.X + 4,e.Bounds.Y + 4);
            }
        }

        // Font Style
        private void lbFontStyle_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if (txtFontStyle.ContainsFocus == false)
                txtFontStyle.Text = lbFontStyle.SelectedItem.ToString();

            // update font
            currentFont.Bold = false;
            currentFont.Italic = false;
            switch(lbFontStyle.SelectedIndex)
            {
                case 1:
                    currentFont.Bold = true;
                    break;
                case 2:
                    currentFont.Italic = true;
                    break;
                case 3:
                    currentFont.Bold = true;
                    currentFont.Italic = true;
                    break;
                default:
                    break;
            }
        }
        private void txtFontStyle_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (txtFontStyle.SelectedIndex == -1)
                txtFontStyle.Text = lbFontStyle.SelectedItem.ToString();
        }
        private void txtFontStyle_Validated(object sender, System.EventArgs e)
        {
            lbFontStyle.SelectedIndex = txtFontStyle.SelectedIndex;
        }
        private void txtFontStyle_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                e.Handled = true;
                this.SelectNextControl(txtFontStyle, true, true, false, true);
            }
        }
            
        // Font Size
        private void lbSize_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if (txtSize.ContainsFocus == false && lbSize.SelectedIndex != -1)
                txtSize.Text = lbSize.SelectedItem.ToString();

            // update size
            currentFont.SizeInPoints = Convert.ToInt32(txtSize.Text);
        }
        private void txtSize_Validated(object sender, System.EventArgs e)
        {
            if (lbSize.Items.Contains(txtSize.Text))
                lbSize.SelectedItem = txtSize.Text;    

            // update size
            currentFont.SizeInPoints = Convert.ToInt32(txtSize.Text);
        }
        private void txtSize_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                e.Handled = true;
                this.SelectNextControl(txtSize, true, true, false, true);
            }

            if (e.KeyChar == (char)Keys.Back)
                e.Handled = false;
        }
        private void txtSize_TextChanged(object sender, EventArgs e)
        {
            txtSize_Validated(null, null);
        }

        // Alignment
        private void pbAlignmentClick(object sender, System.EventArgs e)
        {
            currentFont.FontAlignment = (FontAlignment)((Control)sender).Tag;
            refreshAlignment();
        }
        private void pbAlignmentMouseEnter(object sender, System.EventArgs e)
        {
            if (!currentFont.FontAlignment.Equals((FontAlignment)((Control)sender).Tag))
                ((Control)sender).BackColor = Color.Beige;
        }
        private void pbAlignmentMouseLeave(object sender, System.EventArgs e)
        {
            if (!currentFont.FontAlignment.Equals((FontAlignment)((Control)sender).Tag))
                ((Control)sender).BackColor = Color.Transparent;
        }

        // Effects
        private void cbShadow_CheckStateChanged(object sender, System.EventArgs e)
        {
            currentFont.Shadow = cbShadow.Checked;
            if (currentFont.Color != Color.Transparent)
                cpShadow.Visible = cbShadow.Checked;
        }
        private void cbOutline_CheckStateChanged(object sender, System.EventArgs e)
        {
            currentFont.Outline = cbOutline.Checked;
            if (currentFont.Color != Color.Transparent)
                cpOutline.Visible = cbOutline.Checked;
        }
        private void cpShadow_ColorSelected(object sender, System.EventArgs e)
        {
            cpShadow.BackColor = cpShadow.SelectedColor;
            currentFont.ShadowColor = cpShadow.SelectedColor;
        }
        private void cpOutline_ColorSelected(object sender, System.EventArgs e)
        {
            cpOutline.BackColor = cpOutline.SelectedColor;
            currentFont.OutlineColor = cpOutline.SelectedColor;
        }
        private void cbDoubleSpace_CheckedChanged(object sender, System.EventArgs e)
        {
            currentFont.DoubleSpace = cbDoubleSpace.Checked;
        }

        // Color
        private void TextBox_Click(object sender, EventArgs e)
        {
            showHideColorSelection();
            btnShowPicker.Focus();
        }
        private void TextBox_Enter(object sender, EventArgs e)
        {
            btnShowPicker.Focus();
        }
        private void btnShowPicker_Click(object sender, System.EventArgs e)
        {
            showHideColorSelection();
        }
        private void showHideColorSelection()
        {
            if (popupControlContainer1.IsShowing() == false)
            {
                popupControlContainer1.ShowPopup(new Point(0,0));
                popupControlContainer1.Focus();
            }
            else
                popupControlContainer1.HidePopup(Syncfusion.Windows.Forms.PopupCloseType.Deactivated);
        }
        private void cbAutomatic_CheckStateChanged(object sender, System.EventArgs e)
        {
            popupControlContainer1.Height = cbAutomatic.Checked ? 32 : 240;
            if (popupControlContainer1.IsShowing())
                popupControlContainer1.FindForm().Height = popupControlContainer1.Height;

            if (cbAutomatic.Checked == true)
            {
                currentFont.Color = Color.Transparent;
                popupControlContainer1.HidePopup(Syncfusion.Windows.Forms.PopupCloseType.Done);
            }
        }
        private void colorUI_ColorSelected(object sender, System.EventArgs e)
        {
            if (colorUI.SelectedColor == Color.Transparent)
                cbAutomatic.Checked = true;

            currentFont.Color = colorUI.SelectedColor;
            popupControlContainer1.HidePopup(Syncfusion.Windows.Forms.PopupCloseType.Done);
        }
        private void popupControlContainer1_CloseUp(object sender, Syncfusion.Windows.Forms.PopupClosedEventArgs e)
        {
            if (e.PopupCloseType == Syncfusion.Windows.Forms.PopupCloseType.Done)
                refreshColor();
        }
        private void txtColor_TextBox_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        // Buttons
        private void btnClose_Click(object sender, System.EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
        //public void SaveChangesToDB()
        //{
        //    DataBaseFontHelper.SaveFontToDatabase(FontId, PresenterFont);
        //}
        private void btnSave_Click(object sender, System.EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        #endregion

        private EmpowerPresenter.PresenterFont currentFont;
        public PresenterFont PresenterFont
        {
            get { return currentFont; }
            set { LoadFont(value); }
        }
        //private int fontId = -2;
        //public int FontId
        //{
        //    get { return fontId; }
        //    set { fontId = value; }
        //}
    }
}
