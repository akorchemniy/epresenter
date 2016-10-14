namespace EmpowerPresenter
{
	partial class ErrorDlg
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ErrorDlg));
			this.label1 = new System.Windows.Forms.Label();
			this.txtError = new System.Windows.Forms.TextBox();
			this.btnOK = new System.Windows.Forms.Button();
			this.pbInfo = new System.Windows.Forms.PictureBox();
			this.llClipboard = new System.Windows.Forms.LinkLabel();
			((System.ComponentModel.ISupportInitialize)(this.pbInfo)).BeginInit();
			this.SuspendLayout();
			// 
			// label1
			// 
			resources.ApplyResources(this.label1, "label1");
			this.label1.Name = "label1";
			// 
			// txtError
			// 
			resources.ApplyResources(this.txtError, "txtError");
			this.txtError.Name = "txtError";
			this.txtError.ReadOnly = true;
			// 
			// btnOK
			// 
			this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			resources.ApplyResources(this.btnOK, "btnOK");
			this.btnOK.Name = "btnOK";
			this.btnOK.UseVisualStyleBackColor = true;
			this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
			// 
			// pbInfo
			// 
			this.pbInfo.Image = global::EmpowerPresenter.Properties.Resources.icon_alert;
			resources.ApplyResources(this.pbInfo, "pbInfo");
			this.pbInfo.Name = "pbInfo";
			this.pbInfo.TabStop = false;
			// 
			// llClipboard
			// 
			this.llClipboard.ActiveLinkColor = System.Drawing.Color.Blue;
			resources.ApplyResources(this.llClipboard, "llClipboard");
			this.llClipboard.Name = "llClipboard";
			this.llClipboard.TabStop = true;
			this.llClipboard.VisitedLinkColor = System.Drawing.Color.Blue;
			this.llClipboard.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.llClipboard_LinkClicked);
			// 
			// ErrorDlg
			// 
			resources.ApplyResources(this, "$this");
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.llClipboard);
			this.Controls.Add(this.btnOK);
			this.Controls.Add(this.txtError);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.pbInfo);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "ErrorDlg";
			this.ShowInTaskbar = false;
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
			this.TopMost = true;
			((System.ComponentModel.ISupportInitialize)(this.pbInfo)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.PictureBox pbInfo;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox txtError;
		private System.Windows.Forms.Button btnOK;
		private System.Windows.Forms.LinkLabel llClipboard;
	}
}