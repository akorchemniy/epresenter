using System.Windows.Forms;
namespace EmpowerPresenter
{
	partial class DemoVersionOnly
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DemoVersionOnly));
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.llWebsite = new System.Windows.Forms.LinkLabel();
			this.btnOK = new System.Windows.Forms.Button();
			this.lblFeature = new System.Windows.Forms.Label();
			this.pbWarning = new System.Windows.Forms.PictureBox();
			((System.ComponentModel.ISupportInitialize)(this.pbWarning)).BeginInit();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.AccessibleDescription = null;
			this.label1.AccessibleName = null;
			resources.ApplyResources(this.label1, "label1");
			this.label1.Name = "label1";
			// 
			// label2
			// 
			this.label2.AccessibleDescription = null;
			this.label2.AccessibleName = null;
			resources.ApplyResources(this.label2, "label2");
			this.label2.Font = null;
			this.label2.Name = "label2";
			// 
			// label3
			// 
			this.label3.AccessibleDescription = null;
			this.label3.AccessibleName = null;
			resources.ApplyResources(this.label3, "label3");
			this.label3.Font = null;
			this.label3.Name = "label3";
			// 
			// llWebsite
			// 
			this.llWebsite.AccessibleDescription = null;
			this.llWebsite.AccessibleName = null;
			this.llWebsite.ActiveLinkColor = System.Drawing.Color.Blue;
			resources.ApplyResources(this.llWebsite, "llWebsite");
			this.llWebsite.Font = null;
			this.llWebsite.ForeColor = System.Drawing.Color.Black;
			this.llWebsite.Name = "llWebsite";
			this.llWebsite.TabStop = true;
			this.llWebsite.UseCompatibleTextRendering = true;
			this.llWebsite.VisitedLinkColor = System.Drawing.Color.Blue;
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
			// lblFeature
			// 
			this.lblFeature.AccessibleDescription = null;
			this.lblFeature.AccessibleName = null;
			resources.ApplyResources(this.lblFeature, "lblFeature");
			this.lblFeature.Font = null;
			this.lblFeature.ForeColor = System.Drawing.Color.Red;
			this.lblFeature.Name = "lblFeature";
			// 
			// pbWarning
			// 
			this.pbWarning.AccessibleDescription = null;
			this.pbWarning.AccessibleName = null;
			resources.ApplyResources(this.pbWarning, "pbWarning");
			this.pbWarning.BackgroundImage = null;
			this.pbWarning.Font = null;
			this.pbWarning.Image = global::EmpowerPresenter.Properties.Resources.icon_alert;
			this.pbWarning.ImageLocation = null;
			this.pbWarning.Name = "pbWarning";
			this.pbWarning.TabStop = false;
			// 
			// DemoVersionOnly
			// 
			this.AccessibleDescription = null;
			this.AccessibleName = null;
			resources.ApplyResources(this, "$this");
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackgroundImage = null;
			this.Controls.Add(this.lblFeature);
			this.Controls.Add(this.btnOK);
			this.Controls.Add(this.llWebsite);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.pbWarning);
			this.Controls.Add(this.label1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "DemoVersionOnly";
			this.TopMost = true;
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.DemoVersionOnly_FormClosing);
			((System.ComponentModel.ISupportInitialize)(this.pbWarning)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.PictureBox pbWarning;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.LinkLabel llWebsite;
		private System.Windows.Forms.Button btnOK;
		private Label lblFeature;
	}
}