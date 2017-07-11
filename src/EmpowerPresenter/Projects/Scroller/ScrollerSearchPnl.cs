/* ePresenter is licensed under the GPLV3 -- see the 'COPYING' file details.
   Copyright (C) 2006 Alex Korchemniy */
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace EmpowerPresenter
{
    public partial class ScrollerSearchPnl : UserControl, IPopup, IKeyClient, ISearchPanel
    {
        private Dictionary<string, ScrollerData> lProjects = null;
        private ScrollerData currentProject = null;

        ////////////////////////////////////////////////////////////////
        public ScrollerSearchPnl()
        {
            InitializeComponent();
        }
        public void Init()
        {
            // Note: the list is initialized every time activate is called
        }
        private void UpdateUI()
        {
            llDeleteProj.Enabled = btnPrepare.Enabled = btnDisplay.Enabled = (currentProject != null);
        }
        private void LoadProjList()
        {
            currentProject = null;
            lbProjects.Items.Clear();
            ScrollerStore ss = new ScrollerStore();
            lProjects = ss.GetProjectList();
            foreach (ScrollerData s in lProjects.Values)
                lbProjects.Items.Add(s);
        }

        private void llNewProj_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            ScrollerProject sp = new ScrollerProject();
            Program.Presenter.AddProject(sp);
            this.Deactivate();
        }
        private void llDeleteProj_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (currentProject == null)
                return;

            if (MessageBox.Show(this, Loc.Get("Are you sure you want to delete?"), Loc.Get("Warning"), MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                ScrollerStore ss = new ScrollerStore();
                ss.DeleteAnouncement(currentProject.name);
                this.LoadProjList();
                UpdateUI();
            }
        }
        private void listBox1_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index < 0 || e.Index >= lbProjects.Items.Count)
                return;

            ScrollerData d = (ScrollerData)lbProjects.Items[e.Index];
            Font boldFont = new Font(this.Font, FontStyle.Bold);
            SolidBrush brushNormal = new SolidBrush(Color.FromArgb(223, 223, 223));
            SolidBrush brushSelected = new SolidBrush(Color.FromArgb(255, 204, 91));
            StringFormat sf = new StringFormat();
            sf.Trimming = StringTrimming.EllipsisCharacter;

            // Draw the background
            if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
                e.Graphics.FillRectangle(brushSelected, e.Bounds.X + 1, e.Bounds.Y + 1, e.Bounds.Width - 2, e.Bounds.Height - 2);
            else
                e.Graphics.FillRectangle(brushNormal, e.Bounds.X + 1, e.Bounds.Y + 1, e.Bounds.Width - 2, e.Bounds.Height - 2);

            // Draw text
            e.Graphics.DrawString(d.name + ": " + d.message, boldFont, Brushes.Black, new Rectangle(e.Bounds.X + 30, e.Bounds.Y + 6, e.Bounds.Width - 35, this.Font.Height + 2), sf);

            // Draw image
            Image i = global::EmpowerPresenter.Properties.Resources.comment_edit;
            e.Graphics.DrawImage(i, new Rectangle(e.Bounds.X + 6, e.Bounds.Y + 5, i.Width, i.Height), new Rectangle(0, 0, i.Width, i.Height), GraphicsUnit.Pixel);
        }
        private void lbProjects_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lbProjects.SelectedIndex == -1)
            {
                currentProject = null;
                UpdateUI();
                return;
            }
            else
            {
                currentProject = (ScrollerData)lbProjects.SelectedItem;
                UpdateUI();
            }
        }
        private void lbProjects_DoubleClick(object sender, EventArgs e)
        {
            StartProject(false);
        }
        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            // TODO: Force remeasure
        }

        private void btnDisplay_Click(object sender, EventArgs e)
        {
            StartProject(true);
        }
        private void btnPrepare_Click(object sender, EventArgs e)
        {
            StartProject(false);
        }
        private void btnCancel_Click(object sender, EventArgs e)
        {
            Deactivate();
        }
        private void startToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StartProject(false);
        }
        private void StartProject(bool activate)
        {
            if (currentProject == null)
                return;

            // Check if this project is already opened
            foreach (IProject p in Program.Presenter.projects)
            {
                if (p.GetProjectType() == ProjectType.Scroller && p.GetName() == "Scroller: " + currentProject.name)
                {
                    Program.Presenter.ActivateController(p);
                    this.Deactivate();
                    return;
                }
            }

            ScrollerProject sp = new ScrollerProject();
            Program.Presenter.AddProject(sp);
            sp.LoadData(currentProject); // Load data after the client can refresh
            if (activate)
                sp.Activate();

            // Clear ui 
            this.Deactivate();
        }

        #region IPopup Members

        public void Deactivate()
        {
            this.Hide();

            currentProject = null;
            lbProjects.Items.Clear();
            lProjects.Clear();
            llDeleteProj.Enabled = false;

            Program.Presenter.ClearNavList();
            Program.Presenter.UnregisterPopupWindow(this);
            Program.Presenter.DeactivateKeyClient(this);
            Program.Presenter.DeactiveSearcher();
        }
        public void Activate()
        {
            LoadProjList();

            this.Height = this.Parent.Height - this.Location.Y - 40;
            this.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom;
            Program.Presenter.ShowPopupWindow(this, this.PointToScreen(Point.Empty));
            this.Show();
            Program.Presenter.ActivateKeyClient(this);
        }
        #endregion        

        #region IKeyClient Members

        public void ProccesKeys(Keys k, bool exOwer)
        {
            if (k == Keys.Escape)
                Deactivate();
        }

        #endregion

        #region ISearchPanel Members

        public void TryPrepare()
        {
            StartProject(false);
        }
        public void TryDisplay()
        {
            StartProject(true);
        }

        #endregion
    }
}
