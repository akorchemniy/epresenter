namespace EmpowerPresenter
{
	partial class AnouncementSearchPnl
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AnouncementSearchPnl));
			this.dataGridTextBoxColumn3 = new EmpowerPresenter.DataGridNoActiveCellColumn();
			this.dataGridTextBoxColumn2 = new EmpowerPresenter.DataGridNoActiveCellColumn();
			this.dataGridTextBoxColumn1 = new EmpowerPresenter.DataGridNoActiveCellColumn();
			this.themedPanel1 = new Vendisoft.Controls.ThemedPanel();
			this.pnlMidSection = new System.Windows.Forms.Panel();
			this.lbProjects = new System.Windows.Forms.ListBox();
			this.pnlBottom = new System.Windows.Forms.Panel();
			this.btnCancel = new System.Windows.Forms.Button();
			this.btnDisplay = new System.Windows.Forms.Button();
			this.btnPrepare = new System.Windows.Forms.Button();
			this.pnlTop = new System.Windows.Forms.Panel();
			this.pictureBox3 = new System.Windows.Forms.PictureBox();
			this.pictureBox2 = new System.Windows.Forms.PictureBox();
			this.llNewProj = new System.Windows.Forms.LinkLabel();
			this.label4 = new System.Windows.Forms.Label();
			this.llDeleteProj = new System.Windows.Forms.LinkLabel();
			this.themedPanel1.SuspendLayout();
			this.pnlMidSection.SuspendLayout();
			this.pnlBottom.SuspendLayout();
			this.pnlTop.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
			this.SuspendLayout();
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
			this.pnlMidSection.Controls.Add(this.lbProjects);
			resources.ApplyResources(this.pnlMidSection, "pnlMidSection");
			this.pnlMidSection.Name = "pnlMidSection";
			// 
			// lbProjects
			// 
			resources.ApplyResources(this.lbProjects, "lbProjects");
			this.lbProjects.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
			this.lbProjects.FormattingEnabled = true;
			this.lbProjects.Name = "lbProjects";
			this.lbProjects.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.lbProjects_MouseDoubleClick);
			this.lbProjects.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.listBox1_DrawItem);
			this.lbProjects.SelectedIndexChanged += new System.EventHandler(this.lbProjects_SelectedIndexChanged);
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
			this.pnlTop.Controls.Add(this.pictureBox3);
			this.pnlTop.Controls.Add(this.pictureBox2);
			this.pnlTop.Controls.Add(this.llNewProj);
			this.pnlTop.Controls.Add(this.label4);
			this.pnlTop.Controls.Add(this.llDeleteProj);
			resources.ApplyResources(this.pnlTop, "pnlTop");
			this.pnlTop.Name = "pnlTop";
			// 
			// pictureBox3
			// 
			resources.ApplyResources(this.pictureBox3, "pictureBox3");
			this.pictureBox3.Name = "pictureBox3";
			this.pictureBox3.TabStop = false;
			// 
			// pictureBox2
			// 
			this.pictureBox2.Image = global::EmpowerPresenter.Properties.Resources.add;
			resources.ApplyResources(this.pictureBox2, "pictureBox2");
			this.pictureBox2.Name = "pictureBox2";
			this.pictureBox2.TabStop = false;
			// 
			// llNewProj
			// 
			resources.ApplyResources(this.llNewProj, "llNewProj");
			this.llNewProj.Name = "llNewProj";
			this.llNewProj.TabStop = true;
			this.llNewProj.UseCompatibleTextRendering = true;
			this.llNewProj.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.llNewProj_LinkClicked);
			// 
			// label4
			// 
			resources.ApplyResources(this.label4, "label4");
			this.label4.Name = "label4";
			// 
			// llDeleteProj
			// 
			this.llDeleteProj.ActiveLinkColor = System.Drawing.Color.Blue;
			resources.ApplyResources(this.llDeleteProj, "llDeleteProj");
			this.llDeleteProj.Name = "llDeleteProj";
			this.llDeleteProj.TabStop = true;
			this.llDeleteProj.UseCompatibleTextRendering = true;
			this.llDeleteProj.VisitedLinkColor = System.Drawing.Color.Blue;
			this.llDeleteProj.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.llDeleteProj_LinkClicked);
			// 
			// AnouncementSearchPnl
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.Controls.Add(this.themedPanel1);
			this.DoubleBuffered = true;
			resources.ApplyResources(this, "$this");
			this.Name = "AnouncementSearchPnl";
			this.themedPanel1.ResumeLayout(false);
			this.pnlMidSection.ResumeLayout(false);
			this.pnlBottom.ResumeLayout(false);
			this.pnlTop.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Button btnDisplay;
		private System.Windows.Forms.Button btnPrepare;
		private System.Windows.Forms.Panel pnlMidSection;
		private System.Windows.Forms.Panel pnlBottom;
		private System.Windows.Forms.Panel pnlTop;
		private Vendisoft.Controls.ThemedPanel themedPanel1;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.LinkLabel llNewProj;
		private System.Windows.Forms.PictureBox pictureBox2;
		private System.Windows.Forms.PictureBox pictureBox3;
		private System.Windows.Forms.LinkLabel llDeleteProj;
		private DataGridNoActiveCellColumn dataGridTextBoxColumn3;
		private DataGridNoActiveCellColumn dataGridTextBoxColumn2;
		private DataGridNoActiveCellColumn dataGridTextBoxColumn1;
		private System.Windows.Forms.ListBox lbProjects;
	}
}
