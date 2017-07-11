/* ePresenter is licensed under the GPLV3 -- see the 'COPYING' file details.
   Copyright (C) 2006 Alex Korchemniy */
using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace EmpowerPresenter
{
    public class AddCategoryPrompt : System.Windows.Forms.Form
    {
        #region Designer
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox _name;
        private System.Windows.Forms.Button _OK;
        private System.Windows.Forms.Button _cancel;
        #endregion

        public AddCategoryPrompt()
        {
#if DEMO
            new DemoVersionOnly("Adding a new category").ShowDialog();
#else
            InitializeComponent();
            Program.Presenter.RegisterExKeyOwnerForm(this);
#endif
        }

        #region Windows Form Designer generated code
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AddCategoryPrompt));
            this.label1 = new System.Windows.Forms.Label();
            this._name = new System.Windows.Forms.TextBox();
            this._OK = new System.Windows.Forms.Button();
            this._cancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AccessibleDescription = null;
            this.label1.AccessibleName = null;
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // _name
            // 
            this._name.AccessibleDescription = null;
            this._name.AccessibleName = null;
            resources.ApplyResources(this._name, "_name");
            this._name.BackgroundImage = null;
            this._name.Font = null;
            this._name.Name = "_name";
            // 
            // _OK
            // 
            this._OK.AccessibleDescription = null;
            this._OK.AccessibleName = null;
            resources.ApplyResources(this._OK, "_OK");
            this._OK.BackgroundImage = null;
            this._OK.Font = null;
            this._OK.Name = "_OK";
            this._OK.Click += new System.EventHandler(this._OK_Click);
            // 
            // _cancel
            // 
            this._cancel.AccessibleDescription = null;
            this._cancel.AccessibleName = null;
            resources.ApplyResources(this._cancel, "_cancel");
            this._cancel.BackgroundImage = null;
            this._cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this._cancel.Font = null;
            this._cancel.Name = "_cancel";
            this._cancel.Click += new System.EventHandler(this._cancel_Click);
            // 
            // AddCategoryPrompt
            // 
            this.AcceptButton = this._OK;
            this.AccessibleDescription = null;
            this.AccessibleName = null;
            resources.ApplyResources(this, "$this");
            this.BackgroundImage = null;
            this.CancelButton = this._cancel;
            this.Controls.Add(this._OK);
            this.Controls.Add(this._name);
            this.Controls.Add(this.label1);
            this.Controls.Add(this._cancel);
            this.Font = null;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AddCategoryPrompt";
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion

        private void _cancel_Click(object sender, System.EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            this.Close();
        }
        private void _OK_Click(object sender, System.EventArgs e)
        {
            string[] cats = PhotoInfo.GetCatList();
            foreach(string cat in cats)
            {
                if (cat == _name.Text)
                {
                    MessageBox.Show(this, Loc.Get("Category already exists"), Loc.Get("Error"));
                    return;
                }
            }

            PhotoInfo.AddCategory(_name.Text);

            DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
