namespace EmpowerPresenter.Dialogs
{
	partial class eSwordImpDlg
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(eSwordImpDlg));
			this.pbInfo = new System.Windows.Forms.PictureBox();
			this.lblDisclaimer = new System.Windows.Forms.Label();
			this.pictureBox1 = new System.Windows.Forms.PictureBox();
			this.label1 = new System.Windows.Forms.Label();
			this.btnCancel = new System.Windows.Forms.Button();
			this.btnImport = new System.Windows.Forms.Button();
			this.lbBibles = new System.Windows.Forms.ListBox();
			((System.ComponentModel.ISupportInitialize)(this.pbInfo)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
			this.SuspendLayout();
			// 
			// pbInfo
			// 
			this.pbInfo.Image = global::EmpowerPresenter.Properties.Resources.icon_alert;
			this.pbInfo.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.pbInfo.Location = new System.Drawing.Point(12, 12);
			this.pbInfo.Name = "pbInfo";
			this.pbInfo.Size = new System.Drawing.Size(19, 18);
			this.pbInfo.TabIndex = 1;
			this.pbInfo.TabStop = false;
			// 
			// lblDisclaimer
			// 
			this.lblDisclaimer.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.lblDisclaimer.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.lblDisclaimer.Location = new System.Drawing.Point(37, 12);
			this.lblDisclaimer.Name = "lblDisclaimer";
			this.lblDisclaimer.Size = new System.Drawing.Size(389, 51);
			this.lblDisclaimer.TabIndex = 2;
			this.lblDisclaimer.Text = "This wizard allows you to import an eSword Bible into ePresenter. You must have e" +
				"Sword installed and have permission from the Bible author.";
			// 
			// pictureBox1
			// 
			this.pictureBox1.Image = global::EmpowerPresenter.Properties.Resources.icon_info;
			this.pictureBox1.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.pictureBox1.Location = new System.Drawing.Point(12, 63);
			this.pictureBox1.Name = "pictureBox1";
			this.pictureBox1.Size = new System.Drawing.Size(19, 18);
			this.pictureBox1.TabIndex = 3;
			this.pictureBox1.TabStop = false;
			// 
			// label1
			// 
			this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.label1.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.label1.Location = new System.Drawing.Point(37, 63);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(389, 18);
			this.label1.TabIndex = 4;
			this.label1.Text = "ePresenter has located the following Bibles in eSword:";
			// 
			// btnCancel
			// 
			this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Location = new System.Drawing.Point(12, 253);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(75, 23);
			this.btnCancel.TabIndex = 5;
			this.btnCancel.Text = "Cancel";
			this.btnCancel.UseVisualStyleBackColor = true;
			this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
			// 
			// btnImport
			// 
			this.btnImport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnImport.Enabled = false;
			this.btnImport.Location = new System.Drawing.Point(351, 253);
			this.btnImport.Name = "btnImport";
			this.btnImport.Size = new System.Drawing.Size(75, 23);
			this.btnImport.TabIndex = 6;
			this.btnImport.Text = "Import";
			this.btnImport.UseVisualStyleBackColor = true;
			this.btnImport.Click += new System.EventHandler(this.btnImport_Click);
			// 
			// lbBibles
			// 
			this.lbBibles.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.lbBibles.FormattingEnabled = true;
			this.lbBibles.IntegralHeight = false;
			this.lbBibles.ItemHeight = 15;
			this.lbBibles.Location = new System.Drawing.Point(40, 84);
			this.lbBibles.Name = "lbBibles";
			this.lbBibles.Size = new System.Drawing.Size(386, 154);
			this.lbBibles.TabIndex = 7;
			this.lbBibles.SelectedIndexChanged += new System.EventHandler(this.lbBibles_SelectedIndexChanged);
			// 
			// eSwordImpDlg
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.ClientSize = new System.Drawing.Size(438, 288);
			this.Controls.Add(this.lbBibles);
			this.Controls.Add(this.btnImport);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.pictureBox1);
			this.Controls.Add(this.lblDisclaimer);
			this.Controls.Add(this.pbInfo);
			this.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "eSwordImpDlg";
			this.ShowInTaskbar = false;
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
			this.Text = "eSword Bible import wizard";
			((System.ComponentModel.ISupportInitialize)(this.pbInfo)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.PictureBox pbInfo;
		private System.Windows.Forms.Label lblDisclaimer;
		private System.Windows.Forms.PictureBox pictureBox1;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Button btnImport;
		private System.Windows.Forms.ListBox lbBibles;
	}
}