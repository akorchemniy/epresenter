using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace EmpowerPresenter
{
	public class TestForm : System.Windows.Forms.Form
	{
		private Syncfusion.Windows.Forms.Tools.ButtonEdit buttonEdit1;
		private Syncfusion.Windows.Forms.Tools.ButtonEditChildButton buttonEditChildButton1;
		#region Designer

		private Skybound.VisualStyles.VisualStyleProvider visualStyleProvider1;
		#endregion

		public TestForm()
		{
			this.Bounds = MainStaticClass.MainForm.Bounds;
			InitializeComponent();
		}


		#region Windows Form Designer generated code
		private void InitializeComponent()
		{
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(TestForm));
			this.visualStyleProvider1 = new Skybound.VisualStyles.VisualStyleProvider();
			this.buttonEdit1 = new Syncfusion.Windows.Forms.Tools.ButtonEdit();
			this.buttonEditChildButton1 = new Syncfusion.Windows.Forms.Tools.ButtonEditChildButton();
			((System.ComponentModel.ISupportInitialize)(this.buttonEdit1)).BeginInit();
			this.buttonEdit1.SuspendLayout();
			this.SuspendLayout();
			// 
			// buttonEdit1
			// 
			this.buttonEdit1.AccessibleDescription = resources.GetString("buttonEdit1.AccessibleDescription");
			this.buttonEdit1.AccessibleName = resources.GetString("buttonEdit1.AccessibleName");
			this.buttonEdit1.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("buttonEdit1.Anchor")));
			this.buttonEdit1.AutoScroll = ((bool)(resources.GetObject("buttonEdit1.AutoScroll")));
			this.buttonEdit1.AutoScrollMargin = ((System.Drawing.Size)(resources.GetObject("buttonEdit1.AutoScrollMargin")));
			this.buttonEdit1.AutoScrollMinSize = ((System.Drawing.Size)(resources.GetObject("buttonEdit1.AutoScrollMinSize")));
			this.buttonEdit1.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("buttonEdit1.BackgroundImage")));
			this.buttonEdit1.Buttons.Add(this.buttonEditChildButton1);
			this.buttonEdit1.Controls.Add(this.buttonEditChildButton1);
			this.buttonEdit1.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("buttonEdit1.Dock")));
			this.buttonEdit1.Enabled = ((bool)(resources.GetObject("buttonEdit1.Enabled")));
			this.buttonEdit1.Font = ((System.Drawing.Font)(resources.GetObject("buttonEdit1.Font")));
			this.buttonEdit1.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("buttonEdit1.ImeMode")));
			this.buttonEdit1.Location = ((System.Drawing.Point)(resources.GetObject("buttonEdit1.Location")));
			this.buttonEdit1.Name = "buttonEdit1";
			this.buttonEdit1.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("buttonEdit1.RightToLeft")));
			this.buttonEdit1.SelectionLength = 0;
			this.buttonEdit1.SelectionStart = 0;
			this.buttonEdit1.ShowTextBox = true;
			this.buttonEdit1.Size = ((System.Drawing.Size)(resources.GetObject("buttonEdit1.Size")));
			this.buttonEdit1.TabIndex = ((int)(resources.GetObject("buttonEdit1.TabIndex")));
			this.buttonEdit1.Text = resources.GetString("buttonEdit1.Text");
			// 
			// buttonEdit1.TextBox
			// 
			this.buttonEdit1.TextBox.AccessibleDescription = resources.GetString("buttonEdit1.TextBox.AccessibleDescription");
			this.buttonEdit1.TextBox.AccessibleName = resources.GetString("buttonEdit1.TextBox.AccessibleName");
			this.buttonEdit1.TextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("buttonEdit1.TextBox.Anchor")));
			this.buttonEdit1.TextBox.AutoSize = ((bool)(resources.GetObject("buttonEdit1.TextBox.AutoSize")));
			this.buttonEdit1.TextBox.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("buttonEdit1.TextBox.BackgroundImage")));
			this.buttonEdit1.TextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.buttonEdit1.TextBox.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("buttonEdit1.TextBox.Dock")));
			this.buttonEdit1.TextBox.Enabled = ((bool)(resources.GetObject("buttonEdit1.TextBox.Enabled")));
			this.buttonEdit1.TextBox.Font = ((System.Drawing.Font)(resources.GetObject("buttonEdit1.TextBox.Font")));
			this.buttonEdit1.TextBox.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("buttonEdit1.TextBox.ImeMode")));
			this.buttonEdit1.TextBox.Location = ((System.Drawing.Point)(resources.GetObject("buttonEdit1.TextBox.Location")));
			this.buttonEdit1.TextBox.MaxLength = ((int)(resources.GetObject("buttonEdit1.TextBox.MaxLength")));
			this.buttonEdit1.TextBox.Multiline = ((bool)(resources.GetObject("buttonEdit1.TextBox.Multiline")));
			this.buttonEdit1.TextBox.Name = "";
			this.buttonEdit1.TextBox.PasswordChar = ((char)(resources.GetObject("buttonEdit1.TextBox.PasswordChar")));
			this.buttonEdit1.TextBox.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("buttonEdit1.TextBox.RightToLeft")));
			this.buttonEdit1.TextBox.ScrollBars = ((System.Windows.Forms.ScrollBars)(resources.GetObject("buttonEdit1.TextBox.ScrollBars")));
			this.buttonEdit1.TextBox.Size = ((System.Drawing.Size)(resources.GetObject("buttonEdit1.TextBox.Size")));
			this.buttonEdit1.TextBox.TabIndex = ((int)(resources.GetObject("buttonEdit1.TextBox.TabIndex")));
			this.buttonEdit1.TextBox.Text = resources.GetString("buttonEdit1.TextBox.Text");
			this.buttonEdit1.TextBox.TextAlign = ((System.Windows.Forms.HorizontalAlignment)(resources.GetObject("buttonEdit1.TextBox.TextAlign")));
			this.buttonEdit1.TextBox.Visible = ((bool)(resources.GetObject("buttonEdit1.TextBox.Visible")));
			this.buttonEdit1.TextBox.WordWrap = ((bool)(resources.GetObject("buttonEdit1.TextBox.WordWrap")));
			this.buttonEdit1.Visible = ((bool)(resources.GetObject("buttonEdit1.Visible")));
			// 
			// buttonEditChildButton1
			// 
			this.buttonEditChildButton1.AccessibleDescription = resources.GetString("buttonEditChildButton1.AccessibleDescription");
			this.buttonEditChildButton1.AccessibleName = resources.GetString("buttonEditChildButton1.AccessibleName");
			this.buttonEditChildButton1.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("buttonEditChildButton1.Anchor")));
			this.buttonEditChildButton1.BackColor = System.Drawing.SystemColors.Control;
			this.buttonEditChildButton1.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("buttonEditChildButton1.BackgroundImage")));
			this.buttonEditChildButton1.ButtonEditParent = this.buttonEdit1;
			this.buttonEditChildButton1.ButtonType = Syncfusion.Windows.Forms.Tools.ButtonTypes.Browse;
			this.buttonEditChildButton1.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("buttonEditChildButton1.Dock")));
			this.buttonEditChildButton1.Enabled = ((bool)(resources.GetObject("buttonEditChildButton1.Enabled")));
			this.buttonEditChildButton1.Font = ((System.Drawing.Font)(resources.GetObject("buttonEditChildButton1.Font")));
			this.buttonEditChildButton1.Image = ((System.Drawing.Image)(resources.GetObject("buttonEditChildButton1.Image")));
			this.buttonEditChildButton1.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("buttonEditChildButton1.ImageAlign")));
			this.buttonEditChildButton1.ImageIndex = ((int)(resources.GetObject("buttonEditChildButton1.ImageIndex")));
			this.buttonEditChildButton1.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("buttonEditChildButton1.ImeMode")));
			this.buttonEditChildButton1.Location = ((System.Drawing.Point)(resources.GetObject("buttonEditChildButton1.Location")));
			this.buttonEditChildButton1.Name = "buttonEditChildButton1";
			this.buttonEditChildButton1.PreferredWidth = 16;
			this.buttonEditChildButton1.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("buttonEditChildButton1.RightToLeft")));
			this.buttonEditChildButton1.Size = ((System.Drawing.Size)(resources.GetObject("buttonEditChildButton1.Size")));
			this.buttonEditChildButton1.TabIndex = ((int)(resources.GetObject("buttonEditChildButton1.TabIndex")));
			this.buttonEditChildButton1.Text = resources.GetString("buttonEditChildButton1.Text");
			this.buttonEditChildButton1.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("buttonEditChildButton1.TextAlign")));
			this.buttonEditChildButton1.Visible = ((bool)(resources.GetObject("buttonEditChildButton1.Visible")));
			this.visualStyleProvider1.SetVisualStyleSupport(this.buttonEditChildButton1, true);
			// 
			// TestForm
			// 
			this.AccessibleDescription = resources.GetString("$this.AccessibleDescription");
			this.AccessibleName = resources.GetString("$this.AccessibleName");
			this.AutoScaleBaseSize = ((System.Drawing.Size)(resources.GetObject("$this.AutoScaleBaseSize")));
			this.AutoScroll = ((bool)(resources.GetObject("$this.AutoScroll")));
			this.AutoScrollMargin = ((System.Drawing.Size)(resources.GetObject("$this.AutoScrollMargin")));
			this.AutoScrollMinSize = ((System.Drawing.Size)(resources.GetObject("$this.AutoScrollMinSize")));
			this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
			this.ClientSize = ((System.Drawing.Size)(resources.GetObject("$this.ClientSize")));
			this.Controls.Add(this.buttonEdit1);
			this.Enabled = ((bool)(resources.GetObject("$this.Enabled")));
			this.Font = ((System.Drawing.Font)(resources.GetObject("$this.Font")));
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("$this.ImeMode")));
			this.Location = ((System.Drawing.Point)(resources.GetObject("$this.Location")));
			this.MaximizeBox = false;
			this.MaximumSize = ((System.Drawing.Size)(resources.GetObject("$this.MaximumSize")));
			this.MinimizeBox = false;
			this.MinimumSize = ((System.Drawing.Size)(resources.GetObject("$this.MinimumSize")));
			this.Name = "TestForm";
			this.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("$this.RightToLeft")));
			this.StartPosition = ((System.Windows.Forms.FormStartPosition)(resources.GetObject("$this.StartPosition")));
			this.Text = resources.GetString("$this.Text");
			((System.ComponentModel.ISupportInitialize)(this.buttonEdit1)).EndInit();
			this.buttonEdit1.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		private void _cancel_Click(object sender, System.EventArgs e)
		{
			DialogResult = DialogResult.Cancel;
			this.Close();
		}

		private void _OK_Click(object sender, System.EventArgs e)
		{
			DialogResult = DialogResult.OK;
			this.Close();
		}
	}
}
