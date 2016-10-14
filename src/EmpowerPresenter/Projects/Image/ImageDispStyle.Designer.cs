namespace EmpowerPresenter
{
	partial class ImageDispStyle
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ImageDispStyle));
			this.label1 = new System.Windows.Forms.Label();
			this.pnlImages = new System.Windows.Forms.Panel();
			this.pbSmartFit = new System.Windows.Forms.PictureBox();
			this.pbCenter = new System.Windows.Forms.PictureBox();
			this.pbStretch = new System.Windows.Forms.PictureBox();
			this.rCenter = new System.Windows.Forms.RadioButton();
			this.rSmartFit = new System.Windows.Forms.RadioButton();
			this.rStretch = new System.Windows.Forms.RadioButton();
			this.btnOK = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this.pnlImages.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.pbSmartFit)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.pbCenter)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.pbStretch)).BeginInit();
			this.SuspendLayout();
			// 
			// label1
			// 
			resources.ApplyResources(this.label1, "label1");
			this.label1.Name = "label1";
			// 
			// pnlImages
			// 
			this.pnlImages.Controls.Add(this.pbSmartFit);
			this.pnlImages.Controls.Add(this.pbCenter);
			this.pnlImages.Controls.Add(this.pbStretch);
			resources.ApplyResources(this.pnlImages, "pnlImages");
			this.pnlImages.Name = "pnlImages";
			// 
			// pbSmartFit
			// 
			resources.ApplyResources(this.pbSmartFit, "pbSmartFit");
			this.pbSmartFit.Name = "pbSmartFit";
			this.pbSmartFit.TabStop = false;
			// 
			// pbCenter
			// 
			resources.ApplyResources(this.pbCenter, "pbCenter");
			this.pbCenter.Name = "pbCenter";
			this.pbCenter.TabStop = false;
			// 
			// pbStretch
			// 
			resources.ApplyResources(this.pbStretch, "pbStretch");
			this.pbStretch.Name = "pbStretch";
			this.pbStretch.TabStop = false;
			// 
			// rCenter
			// 
			resources.ApplyResources(this.rCenter, "rCenter");
			this.rCenter.Name = "rCenter";
			this.rCenter.TabStop = true;
			this.rCenter.UseVisualStyleBackColor = true;
			this.rCenter.CheckedChanged += new System.EventHandler(this.rCenter_CheckedChanged);
			// 
			// rSmartFit
			// 
			resources.ApplyResources(this.rSmartFit, "rSmartFit");
			this.rSmartFit.Checked = true;
			this.rSmartFit.Name = "rSmartFit";
			this.rSmartFit.TabStop = true;
			this.rSmartFit.UseVisualStyleBackColor = true;
			this.rSmartFit.CheckedChanged += new System.EventHandler(this.rSmartFit_CheckedChanged);
			// 
			// rStretch
			// 
			resources.ApplyResources(this.rStretch, "rStretch");
			this.rStretch.Name = "rStretch";
			this.rStretch.TabStop = true;
			this.rStretch.UseVisualStyleBackColor = true;
			this.rStretch.CheckedChanged += new System.EventHandler(this.rStretch_CheckedChanged);
			// 
			// btnOK
			// 
			resources.ApplyResources(this.btnOK, "btnOK");
			this.btnOK.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnOK.Name = "btnOK";
			this.btnOK.UseVisualStyleBackColor = true;
			this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
			// 
			// btnCancel
			// 
			resources.ApplyResources(this.btnCancel, "btnCancel");
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.UseVisualStyleBackColor = true;
			this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
			// 
			// ImageDispStyle
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			resources.ApplyResources(this, "$this");
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnOK);
			this.Controls.Add(this.rStretch);
			this.Controls.Add(this.rSmartFit);
			this.Controls.Add(this.rCenter);
			this.Controls.Add(this.pnlImages);
			this.Controls.Add(this.label1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "ImageDispStyle";
			this.ShowInTaskbar = false;
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
			this.Load += new System.EventHandler(this.ImageDispStyle_Load);
			this.pnlImages.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.pbSmartFit)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.pbCenter)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.pbStretch)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Panel pnlImages;
		private System.Windows.Forms.PictureBox pbCenter;
		private System.Windows.Forms.PictureBox pbStretch;
		private System.Windows.Forms.PictureBox pbSmartFit;
		private System.Windows.Forms.RadioButton rCenter;
		private System.Windows.Forms.RadioButton rSmartFit;
		private System.Windows.Forms.RadioButton rStretch;
		private System.Windows.Forms.Button btnOK;
		private System.Windows.Forms.Button btnCancel;
	}
}