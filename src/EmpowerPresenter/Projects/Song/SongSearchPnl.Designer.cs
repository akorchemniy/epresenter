namespace EmpowerPresenter
{
	partial class SongSearchPnl
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
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SongSearchPnl));
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.startToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.prepareToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.themedPanel1 = new Vendisoft.Controls.ThemedPanel();
            this.pnlMidSection = new System.Windows.Forms.Panel();
            this.pnlInitTumbler = new Vendisoft.Controls.ThemedPanel();
            this.label2 = new System.Windows.Forms.Label();
            this.pbTumbler = new System.Windows.Forms.PictureBox();
            this.pnlSongBrowser = new System.Windows.Forms.Panel();
            this.g = new System.Windows.Forms.DataGrid();
            this.dataGridTableStyle1 = new System.Windows.Forms.DataGridTableStyle();
            this.colNumber = new EmpowerPresenter.DataGridNoActiveCellColumn();
            this.colTitle = new EmpowerPresenter.DataGridNoActiveCellColumn();
            this.colChorus = new EmpowerPresenter.DataGridNoActiveCellColumn();
            this.pnlSearchSelection = new System.Windows.Forms.Panel();
            this.fullSongPreview = new EmpowerPresenter.FullSongPreviewControl();
            this.lblSort = new System.Windows.Forms.Label();
            this.rNoSort = new System.Windows.Forms.RadioButton();
            this.rRelevent = new System.Windows.Forms.RadioButton();
            this.btnSearchClose = new Vendisoft.Controls.ImageButton();
            this.lbSearchResults = new System.Windows.Forms.ListBox();
            this.lblTitleSearchResults = new System.Windows.Forms.Label();
            this.pnlBottom = new System.Windows.Forms.Panel();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnDisplay = new System.Windows.Forms.Button();
            this.btnPrepare = new System.Windows.Forms.Button();
            this.pnlTop = new System.Windows.Forms.Panel();
            this.AddSongNoteLinklb = new System.Windows.Forms.LinkLabel();
            this.btnSearchGo = new Vendisoft.Controls.ImageButton();
            this.label1 = new System.Windows.Forms.Label();
            this.txtFulltextSearch = new System.Windows.Forms.TextBox();
            this.pictureBox3 = new System.Windows.Forms.PictureBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.llAttachPPT = new System.Windows.Forms.LinkLabel();
            this.llNewSong = new System.Windows.Forms.LinkLabel();
            this.txtSongsSearch = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.llDetach = new System.Windows.Forms.LinkLabel();
            this.dataGridTextBoxColumn3 = new EmpowerPresenter.DataGridNoActiveCellColumn();
            this.dataGridTextBoxColumn2 = new EmpowerPresenter.DataGridNoActiveCellColumn();
            this.dataGridTextBoxColumn1 = new EmpowerPresenter.DataGridNoActiveCellColumn();
            this.contextMenuStrip1.SuspendLayout();
            this.themedPanel1.SuspendLayout();
            this.pnlMidSection.SuspendLayout();
            this.pnlInitTumbler.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbTumbler)).BeginInit();
            this.pnlSongBrowser.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.g)).BeginInit();
            this.pnlSearchSelection.SuspendLayout();
            this.pnlBottom.SuspendLayout();
            this.pnlTop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.SuspendLayout();
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.startToolStripMenuItem,
            this.prepareToolStripMenuItem,
            this.editToolStripMenuItem,
            this.deleteToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            resources.ApplyResources(this.contextMenuStrip1, "contextMenuStrip1");
            this.contextMenuStrip1.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuStrip1_Opening);
            // 
            // startToolStripMenuItem
            // 
            this.startToolStripMenuItem.Name = "startToolStripMenuItem";
            resources.ApplyResources(this.startToolStripMenuItem, "startToolStripMenuItem");
            this.startToolStripMenuItem.Click += new System.EventHandler(this.startToolStripMenuItem_Click);
            // 
            // prepareToolStripMenuItem
            // 
            this.prepareToolStripMenuItem.Name = "prepareToolStripMenuItem";
            resources.ApplyResources(this.prepareToolStripMenuItem, "prepareToolStripMenuItem");
            this.prepareToolStripMenuItem.Click += new System.EventHandler(this.prepareToolStripMenuItem_Click);
            // 
            // editToolStripMenuItem
            // 
            this.editToolStripMenuItem.Name = "editToolStripMenuItem";
            resources.ApplyResources(this.editToolStripMenuItem, "editToolStripMenuItem");
            this.editToolStripMenuItem.Click += new System.EventHandler(this.editToolStripMenuItem_Click);
            // 
            // deleteToolStripMenuItem
            // 
            this.deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
            resources.ApplyResources(this.deleteToolStripMenuItem, "deleteToolStripMenuItem");
            this.deleteToolStripMenuItem.Click += new System.EventHandler(this.deleteToolStripMenuItem_Click);
            // 
            // themedPanel1
            // 
            this.themedPanel1.BottomSplice = 50;
            this.themedPanel1.Controls.Add(this.pnlMidSection);
            this.themedPanel1.Controls.Add(this.pnlBottom);
            this.themedPanel1.Controls.Add(this.pnlTop);
            resources.ApplyResources(this.themedPanel1, "themedPanel1");
            this.themedPanel1.Image = global::EmpowerPresenter.Properties.Resources.popupbg;
            this.themedPanel1.LeftSlice = 50;
            this.themedPanel1.Name = "themedPanel1";
            this.themedPanel1.RightSplice = 50;
            this.themedPanel1.TopSplice = 50;
            // 
            // pnlMidSection
            // 
            this.pnlMidSection.BackColor = System.Drawing.Color.Transparent;
            this.pnlMidSection.Controls.Add(this.pnlInitTumbler);
            this.pnlMidSection.Controls.Add(this.pnlSongBrowser);
            this.pnlMidSection.Controls.Add(this.pnlSearchSelection);
            resources.ApplyResources(this.pnlMidSection, "pnlMidSection");
            this.pnlMidSection.Name = "pnlMidSection";
            // 
            // pnlInitTumbler
            // 
            resources.ApplyResources(this.pnlInitTumbler, "pnlInitTumbler");
            this.pnlInitTumbler.BackColor = System.Drawing.Color.Transparent;
            this.pnlInitTumbler.BottomSplice = 7;
            this.pnlInitTumbler.Controls.Add(this.label2);
            this.pnlInitTumbler.Controls.Add(this.pbTumbler);
            this.pnlInitTumbler.Image = global::EmpowerPresenter.Properties.Resources.whitepnl;
            this.pnlInitTumbler.LeftSlice = 7;
            this.pnlInitTumbler.Name = "pnlInitTumbler";
            this.pnlInitTumbler.RightSplice = 7;
            this.pnlInitTumbler.TopSplice = 7;
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.label2.Name = "label2";
            // 
            // pbTumbler
            // 
            this.pbTumbler.Image = global::EmpowerPresenter.Properties.Resources.indicator;
            resources.ApplyResources(this.pbTumbler, "pbTumbler");
            this.pbTumbler.Name = "pbTumbler";
            this.pbTumbler.TabStop = false;
            // 
            // pnlSongBrowser
            // 
            this.pnlSongBrowser.Controls.Add(this.g);
            resources.ApplyResources(this.pnlSongBrowser, "pnlSongBrowser");
            this.pnlSongBrowser.Name = "pnlSongBrowser";
            // 
            // g
            // 
            resources.ApplyResources(this.g, "g");
            this.g.CaptionVisible = false;
            this.g.ContextMenuStrip = this.contextMenuStrip1;
            this.g.DataMember = "";
            this.g.HeaderForeColor = System.Drawing.SystemColors.ControlText;
            this.g.Name = "g";
            this.g.SelectionBackColor = System.Drawing.Color.LightSteelBlue;
            this.g.SelectionForeColor = System.Drawing.SystemColors.WindowText;
            this.g.TableStyles.AddRange(new System.Windows.Forms.DataGridTableStyle[] {
            this.dataGridTableStyle1});
            this.g.Click += new System.EventHandler(this.g_Click);
            this.g.DoubleClick += new System.EventHandler(this.g_DoubleClick);
            this.g.MouseDown += new System.Windows.Forms.MouseEventHandler(this.g_MouseDown);
            // 
            // dataGridTableStyle1
            // 
            this.dataGridTableStyle1.AlternatingBackColor = System.Drawing.Color.WhiteSmoke;
            this.dataGridTableStyle1.BackColor = System.Drawing.Color.WhiteSmoke;
            this.dataGridTableStyle1.DataGrid = this.g;
            this.dataGridTableStyle1.GridColumnStyles.AddRange(new System.Windows.Forms.DataGridColumnStyle[] {
            this.colNumber,
            this.colTitle,
            this.colChorus});
            this.dataGridTableStyle1.GridLineStyle = System.Windows.Forms.DataGridLineStyle.None;
            this.dataGridTableStyle1.HeaderForeColor = System.Drawing.SystemColors.ControlText;
            this.dataGridTableStyle1.MappingName = "Songs";
            this.dataGridTableStyle1.ReadOnly = true;
            this.dataGridTableStyle1.RowHeadersVisible = false;
            this.dataGridTableStyle1.SelectionBackColor = System.Drawing.Color.LightSteelBlue;
            this.dataGridTableStyle1.SelectionForeColor = System.Drawing.SystemColors.WindowText;
            // 
            // colNumber
            // 
            this.colNumber.Format = "";
            this.colNumber.FormatInfo = null;
            resources.ApplyResources(this.colNumber, "colNumber");
            this.colNumber.ReadOnly = true;
            // 
            // colTitle
            // 
            this.colTitle.Format = "";
            this.colTitle.FormatInfo = null;
            resources.ApplyResources(this.colTitle, "colTitle");
            this.colTitle.ReadOnly = true;
            // 
            // colChorus
            // 
            this.colChorus.Format = "";
            this.colChorus.FormatInfo = null;
            resources.ApplyResources(this.colChorus, "colChorus");
            this.colChorus.ReadOnly = true;
            // 
            // pnlSearchSelection
            // 
            this.pnlSearchSelection.Controls.Add(this.fullSongPreview);
            this.pnlSearchSelection.Controls.Add(this.lblSort);
            this.pnlSearchSelection.Controls.Add(this.rNoSort);
            this.pnlSearchSelection.Controls.Add(this.rRelevent);
            this.pnlSearchSelection.Controls.Add(this.btnSearchClose);
            this.pnlSearchSelection.Controls.Add(this.lbSearchResults);
            this.pnlSearchSelection.Controls.Add(this.lblTitleSearchResults);
            resources.ApplyResources(this.pnlSearchSelection, "pnlSearchSelection");
            this.pnlSearchSelection.Name = "pnlSearchSelection";
            // 
            // fullSongPreview
            // 
            resources.ApplyResources(this.fullSongPreview, "fullSongPreview");
            this.fullSongPreview.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(233)))), ((int)(((byte)(240)))));
            this.fullSongPreview.Name = "fullSongPreview";
            this.fullSongPreview.DoubleClick += new System.EventHandler(this.fullSongPreview_DoubleClick);
            // 
            // lblSort
            // 
            resources.ApplyResources(this.lblSort, "lblSort");
            this.lblSort.Name = "lblSort";
            // 
            // rNoSort
            // 
            resources.ApplyResources(this.rNoSort, "rNoSort");
            this.rNoSort.Name = "rNoSort";
            this.rNoSort.UseVisualStyleBackColor = true;
            // 
            // rRelevent
            // 
            resources.ApplyResources(this.rRelevent, "rRelevent");
            this.rRelevent.Checked = true;
            this.rRelevent.Name = "rRelevent";
            this.rRelevent.TabStop = true;
            this.rRelevent.UseVisualStyleBackColor = true;
            this.rRelevent.CheckedChanged += new System.EventHandler(this.rRelevent_CheckedChanged);
            // 
            // btnSearchClose
            // 
            resources.ApplyResources(this.btnSearchClose, "btnSearchClose");
            this.btnSearchClose.BackColor = System.Drawing.Color.Transparent;
            this.btnSearchClose.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSearchClose.DisabledImg = global::EmpowerPresenter.Properties.Resources.close_n;
            this.btnSearchClose.IsHighlighted = false;
            this.btnSearchClose.IsPressed = false;
            this.btnSearchClose.IsSelected = false;
            this.btnSearchClose.Name = "btnSearchClose";
            this.btnSearchClose.NormalImg = global::EmpowerPresenter.Properties.Resources.close_n;
            this.btnSearchClose.OverImg = global::EmpowerPresenter.Properties.Resources.close_o;
            this.btnSearchClose.PressedImg = global::EmpowerPresenter.Properties.Resources.close_n;
            this.btnSearchClose.Tag = "Close";
            this.btnSearchClose.Click += new System.EventHandler(this.btnSearchClose_Click);
            // 
            // lbSearchResults
            // 
            resources.ApplyResources(this.lbSearchResults, "lbSearchResults");
            this.lbSearchResults.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.lbSearchResults.FormattingEnabled = true;
            this.lbSearchResults.Name = "lbSearchResults";
            this.lbSearchResults.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.lbSearchResults_DrawItem);
            this.lbSearchResults.MeasureItem += new System.Windows.Forms.MeasureItemEventHandler(this.lbSearchResults_MeasureItem);
            this.lbSearchResults.SelectedIndexChanged += new System.EventHandler(this.lbSearchResults_SelectedIndexChanged);
            this.lbSearchResults.DoubleClick += new System.EventHandler(this.lbSearchResults_DoubleClick);
            // 
            // lblTitleSearchResults
            // 
            this.lblTitleSearchResults.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.lblTitleSearchResults, "lblTitleSearchResults");
            this.lblTitleSearchResults.Name = "lblTitleSearchResults";
            // 
            // pnlBottom
            // 
            this.pnlBottom.BackColor = System.Drawing.Color.Transparent;
            this.pnlBottom.Controls.Add(this.btnCancel);
            this.pnlBottom.Controls.Add(this.btnDisplay);
            this.pnlBottom.Controls.Add(this.btnPrepare);
            resources.ApplyResources(this.pnlBottom, "pnlBottom");
            this.pnlBottom.Name = "pnlBottom";
            // 
            // btnCancel
            // 
            resources.ApplyResources(this.btnCancel, "btnCancel");
            this.btnCancel.BackColor = System.Drawing.SystemColors.Control;
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnDisplay
            // 
            resources.ApplyResources(this.btnDisplay, "btnDisplay");
            this.btnDisplay.BackColor = System.Drawing.SystemColors.Control;
            this.btnDisplay.Name = "btnDisplay";
            this.btnDisplay.UseVisualStyleBackColor = false;
            this.btnDisplay.Click += new System.EventHandler(this.btnDisplay_Click);
            // 
            // btnPrepare
            // 
            resources.ApplyResources(this.btnPrepare, "btnPrepare");
            this.btnPrepare.BackColor = System.Drawing.SystemColors.Control;
            this.btnPrepare.Name = "btnPrepare";
            this.btnPrepare.UseVisualStyleBackColor = false;
            this.btnPrepare.Click += new System.EventHandler(this.btnPrepare_Click);
            // 
            // pnlTop
            // 
            this.pnlTop.BackColor = System.Drawing.Color.Transparent;
            this.pnlTop.Controls.Add(this.AddSongNoteLinklb);
            this.pnlTop.Controls.Add(this.btnSearchGo);
            this.pnlTop.Controls.Add(this.label1);
            this.pnlTop.Controls.Add(this.txtFulltextSearch);
            this.pnlTop.Controls.Add(this.pictureBox3);
            this.pnlTop.Controls.Add(this.pictureBox1);
            this.pnlTop.Controls.Add(this.pictureBox2);
            this.pnlTop.Controls.Add(this.llAttachPPT);
            this.pnlTop.Controls.Add(this.llNewSong);
            this.pnlTop.Controls.Add(this.txtSongsSearch);
            this.pnlTop.Controls.Add(this.label4);
            this.pnlTop.Controls.Add(this.llDetach);
            resources.ApplyResources(this.pnlTop, "pnlTop");
            this.pnlTop.Name = "pnlTop";
            // 
            // AddSongNoteLinklb
            // 
            resources.ApplyResources(this.AddSongNoteLinklb, "AddSongNoteLinklb");
            this.AddSongNoteLinklb.Name = "AddSongNoteLinklb";
            this.AddSongNoteLinklb.TabStop = true;
            this.AddSongNoteLinklb.UseCompatibleTextRendering = true;
            this.AddSongNoteLinklb.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.AddSongNotellbl_LinkClicked);
            // 
            // btnSearchGo
            // 
            resources.ApplyResources(this.btnSearchGo, "btnSearchGo");
            this.btnSearchGo.BackColor = System.Drawing.Color.Transparent;
            this.btnSearchGo.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSearchGo.DisabledImg = global::EmpowerPresenter.Properties.Resources.greenright_d;
            this.btnSearchGo.IsHighlighted = false;
            this.btnSearchGo.IsPressed = false;
            this.btnSearchGo.IsSelected = false;
            this.btnSearchGo.Name = "btnSearchGo";
            this.btnSearchGo.NormalImg = global::EmpowerPresenter.Properties.Resources.greenright_n;
            this.btnSearchGo.OverImg = global::EmpowerPresenter.Properties.Resources.greenright_n;
            this.btnSearchGo.PressedImg = global::EmpowerPresenter.Properties.Resources.greenright_n;
            this.btnSearchGo.Click += new System.EventHandler(this.btnSearchGo_Click);
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // txtFulltextSearch
            // 
            this.txtFulltextSearch.AcceptsReturn = true;
            resources.ApplyResources(this.txtFulltextSearch, "txtFulltextSearch");
            this.txtFulltextSearch.Name = "txtFulltextSearch";
            this.txtFulltextSearch.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtFulltextSearch_KeyDown);
            this.txtFulltextSearch.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtFulltextSearch_KeyPress);
            // 
            // pictureBox3
            // 
            this.pictureBox3.Image = global::EmpowerPresenter.Properties.Resources.accept;
            resources.ApplyResources(this.pictureBox3, "pictureBox3");
            this.pictureBox3.Name = "pictureBox3";
            this.pictureBox3.TabStop = false;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::EmpowerPresenter.Properties.Resources.add;
            resources.ApplyResources(this.pictureBox1, "pictureBox1");
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.TabStop = false;
            // 
            // pictureBox2
            // 
            this.pictureBox2.Image = global::EmpowerPresenter.Properties.Resources.add;
            resources.ApplyResources(this.pictureBox2, "pictureBox2");
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.TabStop = false;
            // 
            // llAttachPPT
            // 
            resources.ApplyResources(this.llAttachPPT, "llAttachPPT");
            this.llAttachPPT.Name = "llAttachPPT";
            this.llAttachPPT.TabStop = true;
            this.llAttachPPT.UseCompatibleTextRendering = true;
            this.llAttachPPT.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.llAttachPPT_LinkClicked);
            // 
            // llNewSong
            // 
            resources.ApplyResources(this.llNewSong, "llNewSong");
            this.llNewSong.Name = "llNewSong";
            this.llNewSong.TabStop = true;
            this.llNewSong.UseCompatibleTextRendering = true;
            this.llNewSong.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.llNewSong_LinkClicked);
            // 
            // txtSongsSearch
            // 
            this.txtSongsSearch.AcceptsReturn = true;
            resources.ApplyResources(this.txtSongsSearch, "txtSongsSearch");
            this.txtSongsSearch.Name = "txtSongsSearch";
            this.txtSongsSearch.TextChanged += new System.EventHandler(this.txtSongsSearch_TextChanged);
            this.txtSongsSearch.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtSongsSearch_KeyPress);
            this.txtSongsSearch.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtSongsSearch_KeyUp);
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // llDetach
            // 
            resources.ApplyResources(this.llDetach, "llDetach");
            this.llDetach.Name = "llDetach";
            this.llDetach.TabStop = true;
            this.llDetach.UseCompatibleTextRendering = true;
            this.llDetach.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.llDetach_LinkClicked);
            // 
            // dataGridTextBoxColumn3
            // 
            this.dataGridTextBoxColumn3.Format = "";
            this.dataGridTextBoxColumn3.FormatInfo = null;
            resources.ApplyResources(this.dataGridTextBoxColumn3, "dataGridTextBoxColumn3");
            this.dataGridTextBoxColumn3.ReadOnly = true;
            // 
            // dataGridTextBoxColumn2
            // 
            this.dataGridTextBoxColumn2.Format = "";
            this.dataGridTextBoxColumn2.FormatInfo = null;
            resources.ApplyResources(this.dataGridTextBoxColumn2, "dataGridTextBoxColumn2");
            this.dataGridTextBoxColumn2.ReadOnly = true;
            // 
            // dataGridTextBoxColumn1
            // 
            this.dataGridTextBoxColumn1.Format = "";
            this.dataGridTextBoxColumn1.FormatInfo = null;
            resources.ApplyResources(this.dataGridTextBoxColumn1, "dataGridTextBoxColumn1");
            this.dataGridTextBoxColumn1.ReadOnly = true;
            // 
            // SongSearchPnl
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.themedPanel1);
            this.DoubleBuffered = true;
            resources.ApplyResources(this, "$this");
            this.Name = "SongSearchPnl";
            this.contextMenuStrip1.ResumeLayout(false);
            this.themedPanel1.ResumeLayout(false);
            this.pnlMidSection.ResumeLayout(false);
            this.pnlInitTumbler.ResumeLayout(false);
            this.pnlInitTumbler.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbTumbler)).EndInit();
            this.pnlSongBrowser.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.g)).EndInit();
            this.pnlSearchSelection.ResumeLayout(false);
            this.pnlSearchSelection.PerformLayout();
            this.pnlBottom.ResumeLayout(false);
            this.pnlTop.ResumeLayout(false);
            this.pnlTop.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.ResumeLayout(false);

		}
		#endregion

		private System.Windows.Forms.TextBox txtSongsSearch;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Button btnDisplay;
		private System.Windows.Forms.Button btnPrepare;
		private System.Windows.Forms.Panel pnlMidSection;
		private System.Windows.Forms.Panel pnlSongBrowser;
		private System.Windows.Forms.Panel pnlBottom;
		private System.Windows.Forms.Panel pnlTop;
		private Vendisoft.Controls.ThemedPanel themedPanel1;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.LinkLabel llNewSong;
		private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
		private System.Windows.Forms.ToolStripMenuItem startToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem deleteToolStripMenuItem;
		private System.Windows.Forms.LinkLabel llAttachPPT;
		private System.Windows.Forms.PictureBox pictureBox2;
		private System.Windows.Forms.PictureBox pictureBox3;
		private System.Windows.Forms.LinkLabel llDetach;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox txtFulltextSearch;
		private Vendisoft.Controls.ImageButton btnSearchGo;
		private System.Windows.Forms.Panel pnlSearchSelection;
		private System.Windows.Forms.Label lblSort;
		private System.Windows.Forms.RadioButton rNoSort;
		private System.Windows.Forms.RadioButton rRelevent;
		private Vendisoft.Controls.ImageButton btnSearchClose;
		private Vendisoft.Controls.ThemedPanel pnlInitTumbler;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.PictureBox pbTumbler;
		private System.Windows.Forms.ListBox lbSearchResults;
		private System.Windows.Forms.Label lblTitleSearchResults;
		private System.Windows.Forms.DataGrid g;
		private System.Windows.Forms.DataGridTableStyle dataGridTableStyle1;
		private DataGridNoActiveCellColumn dataGridTextBoxColumn3;
		private DataGridNoActiveCellColumn dataGridTextBoxColumn2;
		private DataGridNoActiveCellColumn dataGridTextBoxColumn1;
		private DataGridNoActiveCellColumn colNumber;
		private DataGridNoActiveCellColumn colTitle;
		private DataGridNoActiveCellColumn colChorus;
		private FullSongPreviewControl fullSongPreview;
		private System.Windows.Forms.ToolStripMenuItem prepareToolStripMenuItem;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.LinkLabel AddSongNoteLinklb;
	}
}
