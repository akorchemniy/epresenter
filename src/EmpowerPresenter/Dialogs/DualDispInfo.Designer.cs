namespace EmpowerPresenter
{
	partial class DualDispInfo
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DualDispInfo));
			this.btnActivate = new System.Windows.Forms.Button();
			this.llDispInfo = new System.Windows.Forms.LinkLabel();
			this.label1 = new System.Windows.Forms.Label();
			this.pictureBox1 = new System.Windows.Forms.PictureBox();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
			this.SuspendLayout();
			// 
			// btnActivate
			// 
			this.btnActivate.AccessibleDescription = null;
			this.btnActivate.AccessibleName = null;
			resources.ApplyResources(this.btnActivate, "btnActivate");
			this.btnActivate.BackgroundImage = null;
			this.btnActivate.Font = null;
			this.btnActivate.Name = "btnActivate";
			this.btnActivate.UseVisualStyleBackColor = true;
			this.btnActivate.Click += new System.EventHandler(this.btnActivate_Click);
			// 
			// llDispInfo
			// 
			this.llDispInfo.AccessibleDescription = null;
			this.llDispInfo.AccessibleName = null;
			resources.ApplyResources(this.llDispInfo, "llDispInfo");
			this.llDispInfo.Font = null;
			this.llDispInfo.Name = "llDispInfo";
			this.llDispInfo.TabStop = true;
			this.llDispInfo.UseCompatibleTextRendering = true;
			this.llDispInfo.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.llDispInfo_LinkClicked);
			// 
			// label1
			// 
			this.label1.AccessibleDescription = null;
			this.label1.AccessibleName = null;
			resources.ApplyResources(this.label1, "label1");
			this.label1.Name = "label1";
			// 
			// pictureBox1
			// 
			this.pictureBox1.AccessibleDescription = null;
			this.pictureBox1.AccessibleName = null;
			resources.ApplyResources(this.pictureBox1, "pictureBox1");
			this.pictureBox1.BackgroundImage = null;
			this.pictureBox1.Font = null;
			this.pictureBox1.ImageLocation = null;
			this.pictureBox1.Name = "pictureBox1";
			this.pictureBox1.TabStop = false;
			// 
			// DualDispInfo
			// 
			this.AccessibleDescription = null;
			this.AccessibleName = null;
			resources.ApplyResources(this, "$this");
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackgroundImage = null;
			this.ControlBox = false;
			this.Controls.Add(this.label1);
			this.Controls.Add(this.pictureBox1);
			this.Controls.Add(this.llDispInfo);
			this.Controls.Add(this.btnActivate);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "DualDispInfo";
			this.ShowInTaskbar = false;
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
			this.TopMost = true;
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button btnActivate;
		private System.Windows.Forms.LinkLabel llDispInfo;
		private System.Windows.Forms.PictureBox pictureBox1;
		private System.Windows.Forms.Label label1;
	}
}