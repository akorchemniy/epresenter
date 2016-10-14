namespace EmpowerPresenter
{
	partial class Migration
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
		private void InitializeComponent()
		{
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Migration));
			this.label1 = new System.Windows.Forms.Label();
			this.lblMigInfo = new System.Windows.Forms.Label();
			this.btnStart = new System.Windows.Forms.Button();
			this.btnExit = new System.Windows.Forms.Button();
			this.noProgressBar1 = new NoProgress.NoProgressBar();
			this.lblPleaseWait = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.BackColor = System.Drawing.Color.Transparent;
			this.label1.Font = new System.Drawing.Font("Arial", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label1.Location = new System.Drawing.Point(164, 30);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(125, 18);
			this.label1.TabIndex = 0;
			this.label1.Text = "Migration wizard";
			// 
			// lblMigInfo
			// 
			this.lblMigInfo.BackColor = System.Drawing.Color.Transparent;
			this.lblMigInfo.Location = new System.Drawing.Point(164, 60);
			this.lblMigInfo.Name = "lblMigInfo";
			this.lblMigInfo.Size = new System.Drawing.Size(311, 79);
			this.lblMigInfo.TabIndex = 1;
			this.lblMigInfo.Text = "The migration wizard will migrate your pictures, and modified or added songs from" +
				" the previous version of the program to the current version.\r\n\r\nTo begin click S" +
				"tart.";
			// 
			// btnStart
			// 
			this.btnStart.Location = new System.Drawing.Point(395, 282);
			this.btnStart.Name = "btnStart";
			this.btnStart.Size = new System.Drawing.Size(93, 23);
			this.btnStart.TabIndex = 4;
			this.btnStart.Text = "Start";
			this.btnStart.UseVisualStyleBackColor = true;
			this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
			// 
			// btnExit
			// 
			this.btnExit.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnExit.Location = new System.Drawing.Point(22, 282);
			this.btnExit.Name = "btnExit";
			this.btnExit.Size = new System.Drawing.Size(75, 23);
			this.btnExit.TabIndex = 5;
			this.btnExit.Text = "Exit";
			this.btnExit.UseVisualStyleBackColor = true;
			// 
			// noProgressBar1
			// 
			this.noProgressBar1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.noProgressBar1.BackColor = System.Drawing.SystemColors.Control;
			this.noProgressBar1.BorderStyle = System.Windows.Forms.Border3DStyle.Flat;
			this.noProgressBar1.CycleSpeed = 1000;
			this.noProgressBar1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(56)))), ((int)(((byte)(78)))), ((int)(((byte)(118)))));
			this.noProgressBar1.Location = new System.Drawing.Point(167, 240);
			this.noProgressBar1.Name = "noProgressBar1";
			this.noProgressBar1.ShapeSize = 3;
			this.noProgressBar1.ShapeSpacing = 5;
			this.noProgressBar1.ShapeToDraw = NoProgress.ElementStyle.Circle;
			this.noProgressBar1.Size = new System.Drawing.Size(321, 23);
			this.noProgressBar1.TabIndex = 6;
			this.noProgressBar1.Text = "noProgressBar1";
			this.noProgressBar1.Visible = false;
			// 
			// lblPleaseWait
			// 
			this.lblPleaseWait.AutoSize = true;
			this.lblPleaseWait.BackColor = System.Drawing.Color.Transparent;
			this.lblPleaseWait.Location = new System.Drawing.Point(164, 215);
			this.lblPleaseWait.Name = "lblPleaseWait";
			this.lblPleaseWait.Size = new System.Drawing.Size(331, 15);
			this.lblPleaseWait.TabIndex = 7;
			this.lblPleaseWait.Text = "Please wait... migration in progress. Do not exit ePresenter.";
			this.lblPleaseWait.Visible = false;
			// 
			// Migration
			// 
			this.AcceptButton = this.btnStart;
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
			this.CancelButton = this.btnExit;
			this.ClientSize = new System.Drawing.Size(508, 325);
			this.Controls.Add(this.lblPleaseWait);
			this.Controls.Add(this.noProgressBar1);
			this.Controls.Add(this.btnExit);
			this.Controls.Add(this.btnStart);
			this.Controls.Add(this.lblMigInfo);
			this.Controls.Add(this.label1);
			this.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.Name = "Migration";
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Migration wizard";
			this.TopMost = true;
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label lblMigInfo;
		private System.Windows.Forms.Button btnStart;
		private System.Windows.Forms.Button btnExit;
		private NoProgress.NoProgressBar noProgressBar1;
		private System.Windows.Forms.Label lblPleaseWait;
	}
}