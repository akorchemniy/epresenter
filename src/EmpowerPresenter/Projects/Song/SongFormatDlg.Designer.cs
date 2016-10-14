namespace EmpowerPresenter
{
	partial class SongFormatDlg
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

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SongFormatDlg));
			this.label1 = new System.Windows.Forms.Label();
			this.pnlImages = new System.Windows.Forms.Panel();
			this.rWithoutFormatting = new System.Windows.Forms.RadioButton();
			this.rStandard = new System.Windows.Forms.RadioButton();
			this.btnOK = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this.cbNumbers = new System.Windows.Forms.CheckBox();
			this.pbFormatted = new System.Windows.Forms.PictureBox();
			this.pbStripFormatting = new System.Windows.Forms.PictureBox();
			this.pnlImages.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.pbFormatted)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.pbStripFormatting)).BeginInit();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.AccessibleDescription = null;
			this.label1.AccessibleName = null;
			resources.ApplyResources(this.label1, "label1");
			this.label1.Name = "label1";
			// 
			// pnlImages
			// 
			this.pnlImages.AccessibleDescription = null;
			this.pnlImages.AccessibleName = null;
			resources.ApplyResources(this.pnlImages, "pnlImages");
			this.pnlImages.BackgroundImage = null;
			this.pnlImages.Controls.Add(this.pbFormatted);
			this.pnlImages.Controls.Add(this.pbStripFormatting);
			this.pnlImages.Font = null;
			this.pnlImages.Name = "pnlImages";
			// 
			// rWithoutFormatting
			// 
			this.rWithoutFormatting.AccessibleDescription = null;
			this.rWithoutFormatting.AccessibleName = null;
			resources.ApplyResources(this.rWithoutFormatting, "rWithoutFormatting");
			this.rWithoutFormatting.BackgroundImage = null;
			this.rWithoutFormatting.Font = null;
			this.rWithoutFormatting.Name = "rWithoutFormatting";
			this.rWithoutFormatting.TabStop = true;
			this.rWithoutFormatting.UseVisualStyleBackColor = true;
			this.rWithoutFormatting.CheckedChanged += new System.EventHandler(this.rWithoutFormatting_CheckedChanged);
			// 
			// rStandard
			// 
			this.rStandard.AccessibleDescription = null;
			this.rStandard.AccessibleName = null;
			resources.ApplyResources(this.rStandard, "rStandard");
			this.rStandard.BackgroundImage = null;
			this.rStandard.Checked = true;
			this.rStandard.Font = null;
			this.rStandard.Name = "rStandard";
			this.rStandard.TabStop = true;
			this.rStandard.UseVisualStyleBackColor = true;
			this.rStandard.CheckedChanged += new System.EventHandler(this.rStandard_CheckedChanged);
			// 
			// btnOK
			// 
			this.btnOK.AccessibleDescription = null;
			this.btnOK.AccessibleName = null;
			resources.ApplyResources(this.btnOK, "btnOK");
			this.btnOK.BackgroundImage = null;
			this.btnOK.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnOK.Font = null;
			this.btnOK.Name = "btnOK";
			this.btnOK.UseVisualStyleBackColor = true;
			this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
			// 
			// btnCancel
			// 
			this.btnCancel.AccessibleDescription = null;
			this.btnCancel.AccessibleName = null;
			resources.ApplyResources(this.btnCancel, "btnCancel");
			this.btnCancel.BackgroundImage = null;
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Font = null;
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.UseVisualStyleBackColor = true;
			this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
			// 
			// cbNumbers
			// 
			this.cbNumbers.AccessibleDescription = null;
			this.cbNumbers.AccessibleName = null;
			resources.ApplyResources(this.cbNumbers, "cbNumbers");
			this.cbNumbers.BackgroundImage = null;
			this.cbNumbers.Font = null;
			this.cbNumbers.Name = "cbNumbers";
			this.cbNumbers.UseVisualStyleBackColor = true;
			this.cbNumbers.CheckedChanged += new System.EventHandler(this.cbNumbers_CheckedChanged);
			// 
			// pbFormatted
			// 
			this.pbFormatted.AccessibleDescription = null;
			this.pbFormatted.AccessibleName = null;
			resources.ApplyResources(this.pbFormatted, "pbFormatted");
			this.pbFormatted.BackgroundImage = null;
			this.pbFormatted.Font = null;
			this.pbFormatted.ImageLocation = null;
			this.pbFormatted.Name = "pbFormatted";
			this.pbFormatted.TabStop = false;
			// 
			// pbStripFormatting
			// 
			this.pbStripFormatting.AccessibleDescription = null;
			this.pbStripFormatting.AccessibleName = null;
			resources.ApplyResources(this.pbStripFormatting, "pbStripFormatting");
			this.pbStripFormatting.BackgroundImage = null;
			this.pbStripFormatting.Font = null;
			this.pbStripFormatting.ImageLocation = null;
			this.pbStripFormatting.Name = "pbStripFormatting";
			this.pbStripFormatting.TabStop = false;
			// 
			// SongFormatDlg
			// 
			this.AccessibleDescription = null;
			this.AccessibleName = null;
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			resources.ApplyResources(this, "$this");
			this.BackgroundImage = null;
			this.Controls.Add(this.cbNumbers);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnOK);
			this.Controls.Add(this.rStandard);
			this.Controls.Add(this.rWithoutFormatting);
			this.Controls.Add(this.pnlImages);
			this.Controls.Add(this.label1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "SongFormatDlg";
			this.ShowInTaskbar = false;
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
			this.Load += new System.EventHandler(this.ImageDispStyle_Load);
			this.pnlImages.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.pbFormatted)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.pbStripFormatting)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Panel pnlImages;
		private System.Windows.Forms.PictureBox pbFormatted;
		private System.Windows.Forms.PictureBox pbStripFormatting;
		private System.Windows.Forms.RadioButton rWithoutFormatting;
		private System.Windows.Forms.RadioButton rStandard;
		private System.Windows.Forms.Button btnOK;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.CheckBox cbNumbers;
	}
}