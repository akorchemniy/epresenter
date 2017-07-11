/* ePresenter is licensed under the GPLV3 -- see the 'COPYING' file details.
   Copyright (C) 2006 Alex Korchemniy */
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace EmpowerPresenter
{
    public partial class SongFormatDlg : Form
    {
        private bool stripFormatting = false;
        private bool verseNumbers = false;

        //////////////////////////////////////////////////////////////////////
        public SongFormatDlg()
        {
            InitializeComponent();
        }
        public void LoadFromStr(string setting)
        {
            string[] parts = setting.Split("|".ToCharArray());
            if (parts.Length == 2)
            {
                StripFormatting = bool.Parse(parts[0]);
                VerseNumbers = bool.Parse(parts[1]);
            }
        }
        public string SaveToStr()
        {
            return stripFormatting.ToString() + "|" + verseNumbers.ToString();
        }

        private void ImageDispStyle_Load(object sender, EventArgs e)
        {
            // Force refresh after showing
            StripFormatting = stripFormatting;
            VerseNumbers = verseNumbers;
        }
        private void rStandard_CheckedChanged(object sender, EventArgs e)
        {
            if (rStandard.Checked)
                StripFormatting = false;
        }
        private void rWithoutFormatting_CheckedChanged(object sender, EventArgs e)
        {
            if (rWithoutFormatting.Checked)
                StripFormatting = true;
        }
        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            this.Close();
        }
        private void btnOK_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            this.Close();
        }
        private void cbNumbers_CheckedChanged(object sender, EventArgs e)
        {
            VerseNumbers = cbNumbers.Checked;
        }

        public bool StripFormatting
        {
            get { return stripFormatting; }
            set
            {
                stripFormatting = value;
                if (stripFormatting)
                {
                    rWithoutFormatting.Checked = true;
                    pbStripFormatting.BringToFront();
                }
                else
                {
                    rStandard.Checked = true;
                    pbFormatted.BringToFront();
                }
            }
        }
        public bool VerseNumbers
        {
            get { return verseNumbers; }
            set
            {
                verseNumbers = value;
                cbNumbers.Checked = verseNumbers;
            }
        }
    }
}