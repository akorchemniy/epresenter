/* ePresenter is licensed under the GPLV3 -- see the 'COPYING' file details.
   Copyright (C) 2006 Alex Korchemniy */
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace EmpowerPresenter
{
    public partial class ScrollerProjectView : UserControl, IController
    {
        private int suspendUpdate = 0;

        ////////////////////////////////////////////////////////////////////////////////////
        public ScrollerProjectView()
        {
            InitializeComponent();
            if (Program.ConfigHelper.CurrentLanguage == "ru-RU")
            {
                btnActivate.DisabledImg = global::EmpowerPresenter.Properties.Resources.activate_btndis_ru;
                btnActivate.NormalImg = global::EmpowerPresenter.Properties.Resources.activate_btn_ru;
                btnActivate.OverImg = global::EmpowerPresenter.Properties.Resources.activate_btnover_ru;
                btnActivate.PressedImg = global::EmpowerPresenter.Properties.Resources.activate_btn_ru;
            }

            // Register exclusive key owners
            Program.Presenter.RegisterExKeyOwnerControl(txtLeft);
            Program.Presenter.RegisterExKeyOwnerControl(txtMessage);
            Program.Presenter.RegisterExKeyOwnerControl(txtRight);
            this.txtMessage.LostFocus += delegate
            {
                System.Diagnostics.Trace.WriteLine("Lost");
            };
        }
        internal void RefreshUI()
        {
            suspendUpdate++;
            this.txtLeft.Text = proj.LeftMessage;
            this.txtMessage.Text = proj.Message;
            this.txtRight.Text = proj.RightMessage;
            suspendUpdate--;
        }

        private void btnActivate_Click(object sender, EventArgs e)
        {
            proj.UpdateMessage(txtMessage.Text, txtLeft.Text, txtRight.Text);
            proj.Activate();
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
#if DEMO
            new DemoVersionOnly("Save project").ShowDialog();
#else
            ScrollerStore ss = new ScrollerStore();
            if (!proj.bSaved)
            {
                ProjSaveDlg f = new ProjSaveDlg();
                f.ValidateName += delegate(object s, CancelEventArgs ec)
                {
                    ec.Cancel = ss.NameExists((string)s);
                };
                if (f.ShowDialog() != DialogResult.OK)
                    return;
                proj.name = f.FileName;
            }

            ScrollerData sd = new ScrollerData();
            sd.name = proj.name;
            sd.leftMessage = proj.LeftMessage;
            sd.message = proj.Message;
            sd.rightMessage = proj.RightMessage;
            sd.font = proj.font;
            ss.SaveProject(sd);
            proj.dirty = false;

            proj.bSaved = true;
#endif
        }
        private void btnFont_Click(object sender, EventArgs e)
        {
            FontSelection f = new FontSelection();
            f.LoadFont(proj.font);
            f.gbAlignment.Visible = false;
            f.gbEffects.Visible = false;
            if (f.ShowDialog() == DialogResult.OK)
            {
                proj.font = f.PresenterFont;
                proj.RefreshUI();
                proj.dirty = true;
            }
        }
        private void btnMouseEnter(object sender, EventArgs e)
        {
            lblInfo.Text = Loc.Get(((Control)sender).Tag.ToString());
        }
        private void btnMouseLeave(object sender, EventArgs e)
        {
            lblInfo.Text = "";
        }
        private void txtLeft_TextChanged(object sender, EventArgs e)
        {
            if (suspendUpdate == 0)
            {
                if (proj != null)
                    proj.UpdateMessage(txtMessage.Text, txtLeft.Text, txtRight.Text);
                proj.dirty = true;
            }
        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            if (proj.dirty && MessageBox.Show(this, Loc.Get("Project not saved. Save now?"), Loc.Get("Warning"), MessageBoxButtons.YesNo) == DialogResult.Yes)
                btnSave_Click(null, null);

            proj.CloseProject();
        }

        #region IController Members

        private ScrollerProject proj;
        private EventHandler ehRefresh;

        public void AttachProject(IProject proj)
        {
            if (this.proj != proj)
            {
                DetachProject();

                this.proj = (ScrollerProject)proj;

                ehRefresh = new EventHandler(project_Refresh);
                this.proj.Refresh += ehRefresh;
            }
            this.RefreshUI();
        }
        public void DetachProject()
        {
            if (this.proj != null)
            {
                if (ehRefresh != null)
                    this.proj.Refresh -= ehRefresh;
            }
            this.proj = null;
        }
        public void DoEditSettings() { btnFont_Click(null, null); }
        private void project_Refresh(object sender, EventArgs e)
        {
            RefreshUI();
        }

        #endregion

    }
}
