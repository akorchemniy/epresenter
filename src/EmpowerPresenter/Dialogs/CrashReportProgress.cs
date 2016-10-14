/* ePresenter is licensed under the GPLV3 -- see the 'COPYING' file details.
   Copyright (C) 2006 Alex Korchemniy */
using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.IO;

namespace Vendisoft.Forms
{
	public class CrashReportProgress : System.Windows.Forms.Form
	{
		private bool bSendingReport = false;
		private System.Threading.Thread crashLogThread;

		#region Designer
		private System.Windows.Forms.Label label1;
		private NoProgress.NoProgressBar noProgressBar1;
		private Button btnStart;
		private Label lblInfo;
		private Label label2;
		private System.ComponentModel.Container components = null;
		#endregion

		////////////////////////////////////////////////////////////////////////
		public CrashReportProgress()
		{
			InitializeComponent();
		}
		protected override void Dispose( bool disposing )
		{
			if( disposing ){if(components != null){components.Dispose();}}
			base.Dispose( disposing );
		}
		#region Windows Form Designer generated code
		private void InitializeComponent()
		{
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CrashReportProgress));
			this.label1 = new System.Windows.Forms.Label();
			this.noProgressBar1 = new NoProgress.NoProgressBar();
			this.btnStart = new System.Windows.Forms.Button();
			this.lblInfo = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.label1.Location = new System.Drawing.Point(8, 8);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(413, 72);
			this.label1.TabIndex = 0;
			this.label1.Text = resources.GetString("label1.Text");
			// 
			// noProgressBar1
			// 
			this.noProgressBar1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.noProgressBar1.BackColor = System.Drawing.SystemColors.Control;
			this.noProgressBar1.BorderStyle = System.Windows.Forms.Border3DStyle.Flat;
			this.noProgressBar1.CycleSpeed = 1000;
			this.noProgressBar1.ForeColor = System.Drawing.Color.Black;
			this.noProgressBar1.Location = new System.Drawing.Point(8, 123);
			this.noProgressBar1.Name = "noProgressBar1";
			this.noProgressBar1.ShapeSize = 3;
			this.noProgressBar1.ShapeSpacing = 5;
			this.noProgressBar1.ShapeToDraw = NoProgress.ElementStyle.Circle;
			this.noProgressBar1.Size = new System.Drawing.Size(411, 23);
			this.noProgressBar1.TabIndex = 1;
			this.noProgressBar1.Text = "noProgressBar1";
			this.noProgressBar1.Visible = false;
			// 
			// btnStart
			// 
			this.btnStart.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnStart.Location = new System.Drawing.Point(342, 159);
			this.btnStart.Name = "btnStart";
			this.btnStart.Size = new System.Drawing.Size(75, 23);
			this.btnStart.TabIndex = 2;
			this.btnStart.Text = "Start";
			this.btnStart.UseVisualStyleBackColor = true;
			this.btnStart.Click += new System.EventHandler(this.start_Click);
			// 
			// lblInfo
			// 
			this.lblInfo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.lblInfo.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblInfo.Location = new System.Drawing.Point(8, 160);
			this.lblInfo.Name = "lblInfo";
			this.lblInfo.Size = new System.Drawing.Size(313, 22);
			this.lblInfo.TabIndex = 3;
			this.lblInfo.Text = "Please click Start";
			// 
			// label2
			// 
			this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.label2.Location = new System.Drawing.Point(8, 80);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(375, 34);
			this.label2.TabIndex = 4;
			this.label2.Text = "If you do not have an internet connection, please deliver contact Vendisoft (info" +
				"@vendisoft.biz)";
			// 
			// CrashReportProgress
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(6, 14);
			this.ClientSize = new System.Drawing.Size(431, 192);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.lblInfo);
			this.Controls.Add(this.btnStart);
			this.Controls.Add(this.noProgressBar1);
			this.Controls.Add(this.label1);
			this.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.Name = "CrashReportProgress";
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Crash report";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.CrashReportProgress_FormClosing);
			this.ResumeLayout(false);

		}

		void CrashReportProgress_FormClosing(object sender, FormClosingEventArgs e)
		{
			this.Visible = false;
			e.Cancel = true;
		}
		#endregion

		private void start_Click(object sender, EventArgs e)
		{
			if (bSendingReport && crashLogThread != null)
			{
				// Kill the report
				crashLogThread.Join();

				this.Close();
			}

			bSendingReport = true;
			btnStart.Visible = false;
			noProgressBar1.Visible = true;
			lblInfo.Text = "Connecting... Sending...";

			// Start sending the report
			crashLogThread = new System.Threading.Thread(new System.Threading.ThreadStart(CrashLogThread));
			crashLogThread.Start();
		}

		private void CrashLogThread()
		{
			System.Diagnostics.Trace.WriteLine("Basic system info:");
			System.Diagnostics.Trace.WriteLine(Environment.OSVersion.ToString());
			System.Diagnostics.Trace.WriteLine(Environment.Version.ToString());
			System.Diagnostics.Trace.WriteLine(Application.ProductVersion.ToString());
			System.Diagnostics.Trace.WriteLine("Working set: " + Environment.WorkingSet);

			try
			{
				string lpath = Application.ExecutablePath + ".log";
				StreamReader sr = File.OpenText(lpath);
				string trace = sr.ReadToEnd();
				sr.Close();

                // NOTE: Disabled for public release
				//EmpowerPresenter.biz.vendisoft.ladybug l = new EmpowerPresenter.biz.vendisoft.ladybug();
				//ret = l.ReportTraceLog("ePresenter", Application.ProductVersion, trace);
				//if (ret == "Finished")
				//{
					//try{File.Delete(lpath);}
					//catch (Exception ex) { System.Diagnostics.Trace.WriteLine(ex.ToString()); }
				//}
				//l.Dispose();
			}
			catch (Exception ex) { System.Diagnostics.Trace.WriteLine(ex.ToString()); }

			//SimpleDelegate sd = new SimpleDelegate(CrashLogFinished);
			//this.Invoke(sd, new object[] { ret });
			//crashLogThread.Join();
		}
		private void CrashLogFinished(string done)
		{
			//this.Close();
			//this.Dispose();

			//if (done != "Finished")
				//MessageBox.Show(this, "ePresenter was unable to contact the bug reporting webservices. Make sure the computer is connected to the internet and try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
		}

		private delegate void SimpleDelegate(string message);
	}
}
