namespace EmpowerPresenter.Dialogs
{
	partial class BibleManagerDlg
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BibleManagerDlg));
			this.label1 = new System.Windows.Forms.Label();
			this.btnClose = new System.Windows.Forms.Button();
			this.lbBibles = new System.Windows.Forms.ListBox();
			this.btnAddBib = new Vendisoft.Controls.ImageButton();
			this.btnRemoveBib = new Vendisoft.Controls.ImageButton();
			this.lblInfo = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label1.Location = new System.Drawing.Point(12, 7);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(151, 15);
			this.label1.TabIndex = 0;
			this.label1.Text = "Currently installed Bibles:";
			// 
			// btnClose
			// 
			this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnClose.Location = new System.Drawing.Point(338, 262);
			this.btnClose.Name = "btnClose";
			this.btnClose.Size = new System.Drawing.Size(75, 23);
			this.btnClose.TabIndex = 1;
			this.btnClose.Text = "Close";
			this.btnClose.UseVisualStyleBackColor = true;
			// 
			// lbBibles
			// 
			this.lbBibles.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.lbBibles.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
			this.lbBibles.FormattingEnabled = true;
			this.lbBibles.IntegralHeight = false;
			this.lbBibles.ItemHeight = 28;
			this.lbBibles.Location = new System.Drawing.Point(12, 27);
			this.lbBibles.Name = "lbBibles";
			this.lbBibles.Size = new System.Drawing.Size(401, 226);
			this.lbBibles.TabIndex = 2;
			this.lbBibles.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.lbBibles_DrawItem);
			this.lbBibles.SelectedIndexChanged += new System.EventHandler(this.lbBibles_SelectedIndexChanged);
			// 
			// btnAddBib
			// 
			this.btnAddBib.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.btnAddBib.BackColor = System.Drawing.Color.Transparent;
			this.btnAddBib.Cursor = System.Windows.Forms.Cursors.Hand;
			this.btnAddBib.DisabledImg = global::EmpowerPresenter.Properties.Resources.add_d;
			this.btnAddBib.IsHighlighted = false;
			this.btnAddBib.IsPressed = false;
			this.btnAddBib.IsSelected = false;
			this.btnAddBib.Location = new System.Drawing.Point(12, 262);
			this.btnAddBib.Name = "btnAddBib";
			this.btnAddBib.NormalImg = global::EmpowerPresenter.Properties.Resources.add_n;
			this.btnAddBib.OverImg = global::EmpowerPresenter.Properties.Resources.add_n;
			this.btnAddBib.PressedImg = global::EmpowerPresenter.Properties.Resources.add_n;
			this.btnAddBib.Size = new System.Drawing.Size(24, 23);
			this.btnAddBib.TabIndex = 35;
			this.btnAddBib.Tag = "Add Bible";
			this.btnAddBib.Text = "Previous";
			this.btnAddBib.MouseLeave += new System.EventHandler(this.Info_MouseLeave);
			this.btnAddBib.Click += new System.EventHandler(this.btnAddBib_Click);
			this.btnAddBib.MouseEnter += new System.EventHandler(this.Info_MouseEnter);
			// 
			// btnRemoveBib
			// 
			this.btnRemoveBib.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.btnRemoveBib.BackColor = System.Drawing.Color.Transparent;
			this.btnRemoveBib.Cursor = System.Windows.Forms.Cursors.Hand;
			this.btnRemoveBib.DisabledImg = global::EmpowerPresenter.Properties.Resources.remove_dis;
			this.btnRemoveBib.Enabled = false;
			this.btnRemoveBib.IsHighlighted = false;
			this.btnRemoveBib.IsPressed = false;
			this.btnRemoveBib.IsSelected = false;
			this.btnRemoveBib.Location = new System.Drawing.Point(42, 262);
			this.btnRemoveBib.Name = "btnRemoveBib";
			this.btnRemoveBib.NormalImg = global::EmpowerPresenter.Properties.Resources.remove;
			this.btnRemoveBib.OverImg = global::EmpowerPresenter.Properties.Resources.remove;
			this.btnRemoveBib.PressedImg = global::EmpowerPresenter.Properties.Resources.remove;
			this.btnRemoveBib.Size = new System.Drawing.Size(24, 23);
			this.btnRemoveBib.TabIndex = 36;
			this.btnRemoveBib.Tag = "Remove Bible";
			this.btnRemoveBib.Text = "Previous";
			this.btnRemoveBib.MouseLeave += new System.EventHandler(this.Info_MouseLeave);
			this.btnRemoveBib.Click += new System.EventHandler(this.btnRemoveBib_Click);
			this.btnRemoveBib.MouseEnter += new System.EventHandler(this.Info_MouseEnter);
			// 
			// lblInfo
			// 
			this.lblInfo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.lblInfo.Location = new System.Drawing.Point(72, 266);
			this.lblInfo.Name = "lblInfo";
			this.lblInfo.Size = new System.Drawing.Size(260, 13);
			this.lblInfo.TabIndex = 37;
			// 
			// BibleManagerDlg
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.ClientSize = new System.Drawing.Size(425, 297);
			this.Controls.Add(this.lblInfo);
			this.Controls.Add(this.btnRemoveBib);
			this.Controls.Add(this.btnAddBib);
			this.Controls.Add(this.lbBibles);
			this.Controls.Add(this.btnClose);
			this.Controls.Add(this.label1);
			this.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "BibleManagerDlg";
			this.ShowInTaskbar = false;
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
			this.Text = "Bible manager";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Button btnClose;
		private System.Windows.Forms.ListBox lbBibles;
		private Vendisoft.Controls.ImageButton btnAddBib;
		private Vendisoft.Controls.ImageButton btnRemoveBib;
		private System.Windows.Forms.Label lblInfo;
	}
}