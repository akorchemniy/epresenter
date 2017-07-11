using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace EmpowerPresenter.Dialogs
{
    public partial class eSwordImpDlg : Form
    {
        //////////////////////////////////////////////////////////////////
        public eSwordImpDlg()
        {
            // TODO: log ceip
            // TODO: build list of Bibles

            InitializeComponent();
        }
        private void UpdateBtns()
        {
            btnImport.Enabled = lbBibles.SelectedIndex != -1;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void lbBibles_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateBtns();
        }
        private void btnImport_Click(object sender, EventArgs e)
        {
            // TODO: import
        }
    }
}