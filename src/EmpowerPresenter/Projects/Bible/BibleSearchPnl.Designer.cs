namespace EmpowerPresenter
{
	partial class BibleSearchPnl
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BibleSearchPnl));
			this.themedPanel1 = new Vendisoft.Controls.ThemedPanel();
			this.pnlMidSection = new System.Windows.Forms.Panel();
			this.pnlInitTumbler = new Vendisoft.Controls.ThemedPanel();
			this.label2 = new System.Windows.Forms.Label();
			this.pbTumbler = new System.Windows.Forms.PictureBox();
			this.pnlBibBrowser = new System.Windows.Forms.Panel();
			this.lbBibleVerse = new System.Windows.Forms.ListBox();
			this.label5 = new System.Windows.Forms.Label();
			this.lbBibleChapter = new System.Windows.Forms.ListBox();
			this.label8 = new System.Windows.Forms.Label();
			this.lbBibleBook = new System.Windows.Forms.ListBox();
			this.label7 = new System.Windows.Forms.Label();
			this.pnlSearchSelection = new System.Windows.Forms.Panel();
			this.llNoResults = new System.Windows.Forms.LinkLabel();
			this.pbNoItems = new System.Windows.Forms.PictureBox();
			this.lblSort = new System.Windows.Forms.Label();
			this.rNoSort = new System.Windows.Forms.RadioButton();
			this.rRelevent = new System.Windows.Forms.RadioButton();
			this.btnSearchClose = new Vendisoft.Controls.ImageButton();
			this.lbSearchVerses = new System.Windows.Forms.ListBox();
			this.lblTitleSearchResults = new System.Windows.Forms.Label();
			this.pnlBottom = new System.Windows.Forms.Panel();
			this.btnCancel = new System.Windows.Forms.Button();
			this.btnBibleDisplayVerse = new System.Windows.Forms.Button();
			this.btnPrepare = new System.Windows.Forms.Button();
			this.panel1 = new System.Windows.Forms.Panel();
			this.btnSearchGo = new Vendisoft.Controls.ImageButton();
			this.label1 = new System.Windows.Forms.Label();
			this.txtBibSearch = new System.Windows.Forms.TextBox();
			this.txtBibleLocation = new System.Windows.Forms.TextBox();
			this.label4 = new System.Windows.Forms.Label();
			this.themedPanel1.SuspendLayout();
			this.pnlMidSection.SuspendLayout();
			this.pnlInitTumbler.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.pbTumbler)).BeginInit();
			this.pnlBibBrowser.SuspendLayout();
			this.pnlSearchSelection.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.pbNoItems)).BeginInit();
			this.pnlBottom.SuspendLayout();
			this.panel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// themedPanel1
			// 
			this.themedPanel1.BottomSplice = 50;
			this.themedPanel1.Controls.Add(this.pnlMidSection);
			this.themedPanel1.Controls.Add(this.pnlBottom);
			this.themedPanel1.Controls.Add(this.panel1);
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
			this.pnlMidSection.Controls.Add(this.pnlBibBrowser);
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
			// pnlBibBrowser
			// 
			this.pnlBibBrowser.Controls.Add(this.lbBibleVerse);
			this.pnlBibBrowser.Controls.Add(this.label5);
			this.pnlBibBrowser.Controls.Add(this.lbBibleChapter);
			this.pnlBibBrowser.Controls.Add(this.label8);
			this.pnlBibBrowser.Controls.Add(this.lbBibleBook);
			this.pnlBibBrowser.Controls.Add(this.label7);
			resources.ApplyResources(this.pnlBibBrowser, "pnlBibBrowser");
			this.pnlBibBrowser.Name = "pnlBibBrowser";
			// 
			// lbBibleVerse
			// 
			resources.ApplyResources(this.lbBibleVerse, "lbBibleVerse");
			this.lbBibleVerse.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
			this.lbBibleVerse.Name = "lbBibleVerse";
			this.lbBibleVerse.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.lbBibleVerse_DrawItem);
			this.lbBibleVerse.DoubleClick += new System.EventHandler(this.lbBibleVerse_DoubleClick);
			this.lbBibleVerse.SelectedIndexChanged += new System.EventHandler(this.lbBibleVerse_SelectedIndexChanged);
			this.lbBibleVerse.MeasureItem += new System.Windows.Forms.MeasureItemEventHandler(this.lbBibleVerse_MeasureItem);
			// 
			// label5
			// 
			this.label5.BackColor = System.Drawing.Color.Transparent;
			resources.ApplyResources(this.label5, "label5");
			this.label5.Name = "label5";
			// 
			// lbBibleChapter
			// 
			resources.ApplyResources(this.lbBibleChapter, "lbBibleChapter");
			this.lbBibleChapter.Name = "lbBibleChapter";
			this.lbBibleChapter.SelectedIndexChanged += new System.EventHandler(this.lbBibleChapter_SelectedIndexChanged);
			// 
			// label8
			// 
			this.label8.BackColor = System.Drawing.Color.Transparent;
			resources.ApplyResources(this.label8, "label8");
			this.label8.Name = "label8";
			// 
			// lbBibleBook
			// 
			resources.ApplyResources(this.lbBibleBook, "lbBibleBook");
			this.lbBibleBook.Name = "lbBibleBook";
			this.lbBibleBook.SelectedIndexChanged += new System.EventHandler(this.lbBibleBook_SelectedIndexChanged);
			// 
			// label7
			// 
			this.label7.BackColor = System.Drawing.Color.Transparent;
			resources.ApplyResources(this.label7, "label7");
			this.label7.Name = "label7";
			// 
			// pnlSearchSelection
			// 
			this.pnlSearchSelection.Controls.Add(this.llNoResults);
			this.pnlSearchSelection.Controls.Add(this.pbNoItems);
			this.pnlSearchSelection.Controls.Add(this.lblSort);
			this.pnlSearchSelection.Controls.Add(this.rNoSort);
			this.pnlSearchSelection.Controls.Add(this.rRelevent);
			this.pnlSearchSelection.Controls.Add(this.btnSearchClose);
			this.pnlSearchSelection.Controls.Add(this.lbSearchVerses);
			this.pnlSearchSelection.Controls.Add(this.lblTitleSearchResults);
			resources.ApplyResources(this.pnlSearchSelection, "pnlSearchSelection");
			this.pnlSearchSelection.Name = "pnlSearchSelection";
			// 
			// llNoResults
			// 
			this.llNoResults.ActiveLinkColor = System.Drawing.Color.Blue;
			resources.ApplyResources(this.llNoResults, "llNoResults");
			this.llNoResults.Name = "llNoResults";
			this.llNoResults.TabStop = true;
			this.llNoResults.UseCompatibleTextRendering = true;
			this.llNoResults.VisitedLinkColor = System.Drawing.Color.Blue;
			this.llNoResults.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(llNoResults_LinkClicked);
			// 
			// pbNoItems
			// 
			this.pbNoItems.Image = global::EmpowerPresenter.Properties.Resources.icon_alert;
			resources.ApplyResources(this.pbNoItems, "pbNoItems");
			this.pbNoItems.Name = "pbNoItems";
			this.pbNoItems.TabStop = false;
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
			// lbSearchVerses
			// 
			resources.ApplyResources(this.lbSearchVerses, "lbSearchVerses");
			this.lbSearchVerses.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
			this.lbSearchVerses.FormattingEnabled = true;
			this.lbSearchVerses.Name = "lbSearchVerses";
			this.lbSearchVerses.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.lbSearchVerses_DrawItem);
			this.lbSearchVerses.DoubleClick += new System.EventHandler(this.lbSearchVerses_DoubleClick);
			this.lbSearchVerses.SelectedIndexChanged += new System.EventHandler(this.lbSearchVerses_SelectedIndexChanged);
			this.lbSearchVerses.MeasureItem += new System.Windows.Forms.MeasureItemEventHandler(this.lbSearchVerses_MeasureItem);
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
			this.pnlBottom.Controls.Add(this.btnBibleDisplayVerse);
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
			// btnBibleDisplayVerse
			// 
			resources.ApplyResources(this.btnBibleDisplayVerse, "btnBibleDisplayVerse");
			this.btnBibleDisplayVerse.BackColor = System.Drawing.SystemColors.Control;
			this.btnBibleDisplayVerse.Name = "btnBibleDisplayVerse";
			this.btnBibleDisplayVerse.UseVisualStyleBackColor = false;
			this.btnBibleDisplayVerse.Click += new System.EventHandler(this.btnBibleDisplayVerse_Click);
			// 
			// btnPrepare
			// 
			resources.ApplyResources(this.btnPrepare, "btnPrepare");
			this.btnPrepare.BackColor = System.Drawing.SystemColors.Control;
			this.btnPrepare.Name = "btnPrepare";
			this.btnPrepare.UseVisualStyleBackColor = false;
			this.btnPrepare.Click += new System.EventHandler(this.btnPrepare_Click);
			// 
			// panel1
			// 
			this.panel1.BackColor = System.Drawing.Color.Transparent;
			this.panel1.Controls.Add(this.btnSearchGo);
			this.panel1.Controls.Add(this.label1);
			this.panel1.Controls.Add(this.txtBibSearch);
			this.panel1.Controls.Add(this.txtBibleLocation);
			this.panel1.Controls.Add(this.label4);
			resources.ApplyResources(this.panel1, "panel1");
			this.panel1.Name = "panel1";
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
			// txtBibSearch
			// 
			this.txtBibSearch.AcceptsReturn = true;
			resources.ApplyResources(this.txtBibSearch, "txtBibSearch");
			this.txtBibSearch.Name = "txtBibSearch";
			this.txtBibSearch.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtBibSearch_KeyPress);
			this.txtBibSearch.KeyDown += new System.Windows.Forms.KeyEventHandler(txtBibSearch_KeyDown);
			this.txtBibSearch.TextChanged += new System.EventHandler(txtBibSearch_TextChanged);
			// 
			// txtBibleLocation
			// 
			this.txtBibleLocation.AcceptsReturn = true;
			resources.ApplyResources(this.txtBibleLocation, "txtBibleLocation");
			this.txtBibleLocation.Name = "txtBibleLocation";
			this.txtBibleLocation.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtBibleLocation_KeyPress);
			this.txtBibleLocation.KeyDown += new System.Windows.Forms.KeyEventHandler(txtBibleLocation_KeyDown);
			this.txtBibleLocation.TextChanged += new System.EventHandler(this.txtBibleFinder_TextChanged);
			// 
			// label4
			// 
			resources.ApplyResources(this.label4, "label4");
			this.label4.Name = "label4";
			// 
			// BibleSearchPnl
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.Controls.Add(this.themedPanel1);
			resources.ApplyResources(this, "$this");
			this.Name = "BibleSearchPnl";
			this.themedPanel1.ResumeLayout(false);
			this.pnlMidSection.ResumeLayout(false);
			this.pnlInitTumbler.ResumeLayout(false);
			this.pnlInitTumbler.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.pbTumbler)).EndInit();
			this.pnlBibBrowser.ResumeLayout(false);
			this.pnlSearchSelection.ResumeLayout(false);
			this.pnlSearchSelection.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.pbNoItems)).EndInit();
			this.pnlBottom.ResumeLayout(false);
			this.panel1.ResumeLayout(false);
			this.panel1.PerformLayout();
			this.ResumeLayout(false);

		}
		#endregion

		private System.Windows.Forms.TextBox txtBibleLocation;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.ListBox lbBibleVerse;
		private System.Windows.Forms.ListBox lbBibleChapter;
		private System.Windows.Forms.ListBox lbBibleBook;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.Button btnBibleDisplayVerse;
		private System.Windows.Forms.Button btnPrepare;
		private System.Windows.Forms.Panel pnlMidSection;
		private System.Windows.Forms.Panel pnlBibBrowser;
		private System.Windows.Forms.Panel pnlBottom;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox txtBibSearch;
		private System.Windows.Forms.Panel pnlSearchSelection;
		private Vendisoft.Controls.ThemedPanel themedPanel1;
		private System.Windows.Forms.Button btnCancel;
		private Vendisoft.Controls.ImageButton btnSearchGo;
		private System.Windows.Forms.ListBox lbSearchVerses;
		private Vendisoft.Controls.ThemedPanel pnlInitTumbler;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.PictureBox pbTumbler;
		private System.Windows.Forms.Label lblTitleSearchResults;
		private Vendisoft.Controls.ImageButton btnSearchClose;
		private System.Windows.Forms.RadioButton rNoSort;
		private System.Windows.Forms.RadioButton rRelevent;
		private System.Windows.Forms.Label lblSort;
		private System.Windows.Forms.LinkLabel llNoResults;
		private System.Windows.Forms.PictureBox pbNoItems;
	}
}
