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
    public partial class SongSearchPnl : UserControl, IPopup, IKeyClient, ISearchPanel
    {
#if DEMO
        private bool searchExpired = false;
#endif

        private DataView dvSongs = new DataView();
        private PresenterDataset.SongsRow currentSong;
        private Point ptLastGridClick;

        // Searching section
        private List<SongSearchResult> lSearchResults = new List<SongSearchResult>();
        private List<SongVerse> lSearchResultVerses = new List<SongVerse>();
        private BackgroundWorker bwSearch = new BackgroundWorker();
        private string lastSearch = "";

        ////////////////////////////////////////////////////////////////////////////////////////////////
        public SongSearchPnl()
        {
            InitializeComponent();
            pnlSearchSelection.SendToBack();
            pnlInitTumbler.Visible = true;
            pnlInitTumbler.BringToFront();

            bwSearch.DoWork += new DoWorkEventHandler(bwSearch_DoWork);
            bwSearch.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwSearch_RunWorkerCompleted);
        }
        public void Init() // To support design time we keep things out of the constructor
        {
            dvSongs.Table = Program.SongsDS.Songs;
            dvSongs.AllowEdit = false;
            dvSongs.AllowNew = false;
            dvSongs.Sort = "Number";
            dvSongs.RowFilter = "";

            g.SelectionBackColor = Color.LightSteelBlue;
            g.SelectionForeColor = Color.Black;
            g.DataSource = dvSongs;
            g.CurrentCellChanged += new EventHandler(g_CurrentCellChanged);

            // Enable the user interface
            pnlInitTumbler.Visible = false;
            txtFulltextSearch.Enabled = true;
            txtSongsSearch.Enabled = true;
            llNewSong.Enabled = true;
            llAttachPPT.Enabled = true;
            llDetach.Enabled = true;
            AddSongNoteLinklb.Enabled = true;

            // Register exclusive key owners
            Program.Presenter.RegisterExKeyOwnerControl(txtFulltextSearch);
            Program.Presenter.RegisterExKeyOwnerControl(txtSongsSearch);
            Program.Presenter.RegisterExKeyOwnerControl(g);
            Program.Presenter.RegisterExKeyOwnerControl(lbSearchResults);
        }
        private void UpdateUI()
        {
            btnPrepare.Enabled = btnDisplay.Enabled = (currentSong != null);

            if (currentSong != null && currentSong.DisplayDefault)
            {
                llAttachPPT.Visible = true;
                llDetach.Visible = false;
            }
            else
            {
                llAttachPPT.Visible = false;
                llDetach.Visible = true;
            }
        }

        private void g_CurrentCellChanged(object sender, EventArgs e)
        {
            if (g.CurrentCell.RowNumber > -1 && dvSongs.Count != 0)
            {
                if (currentSong != (PresenterDataset.SongsRow)dvSongs[g.CurrentCell.RowNumber].Row)
                {
                    g.Select(g.CurrentCell.RowNumber);
                    currentSong = (PresenterDataset.SongsRow)dvSongs[g.CurrentCell.RowNumber].Row;
                }
            }
            else
                currentSong = null;
            g.Refresh();

            UpdateUI();
        }
        private void g_DoubleClick(object sender, EventArgs e)
        {
            Rectangle r = new Rectangle(0,0,g.Width, 22);
            if (g.CurrentCell.RowNumber > -1 && ptLastGridClick != null && !r.Contains(ptLastGridClick))
                StartProject(true);
        }
        private void g_Click(object sender, System.EventArgs e)
        {
            if (g.CurrentCell.RowNumber > -1 && dvSongs.Count != 0)
            {
                g.Select(g.CurrentCell.RowNumber);
                g_CurrentCellChanged(null, null);
            }
            else
                currentSong = null;
        }
        private void g_MouseDown(object sender, MouseEventArgs e)
        {
            ptLastGridClick = e.Location;
        }        
        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            // FIX: The context menu shows before the underlying verse is selected
            Point ptGridLocationScreen = g.PointToScreen(new Point(0, 0));
            Point ptMouseLocation = Control.MousePosition;
            ptMouseLocation.X -= ptGridLocationScreen.X;
            ptMouseLocation.Y -= ptGridLocationScreen.Y;
            DataGrid.HitTestInfo hti = g.HitTest(ptMouseLocation);
            g.CurrentCell = new DataGridCell(hti.Row, hti.Column);
        }
        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (currentSong != null)
            {
                SongEditorForm songEditor = new SongEditorForm(currentSong);
                if (songEditor.ShowDialog() == DialogResult.OK) // TODO: in the future improve this: non-modal
                {
                    g.Refresh();
                }
            }
        }
        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (currentSong == null)
                return;

            DialogResult result = MessageBox.Show(this,
                Loc.Get("Are you sure you want to delete") + " " + currentSong.Number + " - " + currentSong.Title + "?", 
                Loc.Get("Confirm Deletion"), MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, 
                MessageBoxDefaultButton.Button2);

            if (result == DialogResult.Yes)
            {
                Data.Songs.DeleteSong(currentSong.AutoNumber);
                currentSong.Delete();
                currentSong.AcceptChanges();
                currentSong = null;
            }
        }
        private void llNewSong_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            int newAutoNumber = Data.Songs.GetNextAutoNumber();

            PresenterDataset.SongsRow newSong = Program.SongsDS.Songs.NewSongsRow();
            newSong.Image = -2;
            newSong.AutoNumber = newAutoNumber;
            newSong.Number = -1;
            newSong.FontId = -2;
            newSong.Title = "";
            newSong.Chorus = "";
            newSong.Overlay = 777;

            SongEditorForm sef = new SongEditorForm(newSong);
            sef.Show();

            
           
        }
        private void AddSongNotellbl_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            int songNumber=0;
            if (currentSong != null)
            {
                 songNumber = currentSong.AutoNumber;
            }         
                
            SongNotesForm snf = new SongNotesForm(songNumber);
            snf.Show();

        } 
        
        private void llAttachPPT_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (currentSong == null || Type.GetTypeFromProgID("PowerPoint.Application") == null)
                return;

            try
            {
                Program.Presenter.BeginExKeyOwner();
                OpenFileDialog f = new OpenFileDialog();
                f.Title = Loc.Get("Select PowerPoint presentation");
                f.Filter = Loc.Get("PowerPoint presentation") + " (*.ppt;*.pps;*.pot)| *.ppt;*.pps;*.pot";
                if (f.ShowDialog() == DialogResult.Yes && File.Exists(f.FileName))
                {
                    currentSong.DisplayDefault = false;
                    currentSong.Location = f.FileName;
                    Data.Songs.AddUpdateSong(currentSong);
                    currentSong.AcceptChanges();
                    llAttachPPT.Visible = false;
                    llDetach.Visible = true;
                }
            }
            finally { Program.Presenter.EndExKeyOwner(); }
        }
        private void llDetach_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            currentSong.DisplayDefault = true;
            currentSong.Location = "";
            Data.Songs.AddUpdateSong(currentSong);
            currentSong.AcceptChanges();
            llAttachPPT.Visible = true;
            llDetach.Visible = false;
        }
        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            if (txtFulltextSearch.Text != "" && lSearchResults.Count != 0 && lbSearchResults.Items.Count != 0)
                RefreshSearchList(); // Force remeasure
        }

        // Fulltext Searching
        private void ClearSearchResults()
        {
            if (lSearchResults != null)
                lSearchResults.Clear();
            fullSongPreview.Clear();
            lbSearchResults.Items.Clear();
            lSearchResultVerses.Clear();
            dvSongs.RowFilter = "";
        }
        private void bwSearch_DoWork(object sender, DoWorkEventArgs e)
        {
#if DEMO
            new DemoVersionOnly("Fulltext song searching").ShowDialog();
#else
            lock (lSearchResults)
            {
                // Get search terms
                List<string> searchTerms = SearchHelper.BreakSearchTerms(txtFulltextSearch.Text);
                lSearchResults = SongSearchHelper.Search(searchTerms);
            }
            Application.DoEvents();
#endif
        }
        private void bwSearch_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            RefreshSearchList();
            fullSongPreview.Visible = true;
            lbSearchResults.Visible = true;
            pnlInitTumbler.Visible = false;
            lblSort.Visible = true;
            rNoSort.Visible = true;
            rRelevent.Visible = true;
        }
        private void RefreshSearchList()
        {
            if (txtFulltextSearch.Text == "")
                return;

            pnlSearchSelection.BringToFront();
            lbSearchResults.SuspendLayout();
            lbSearchResults.Items.Clear();
            lock (lSearchResults)
            {
                #if !DEMO
                List<string> searchTerms = SearchHelper.BreakSearchTerms(txtFulltextSearch.Text); // Caller is responsible to make sure that search terms have not changed

                if (rRelevent.Checked && searchTerms.Count > 1)
                {
                    // Sort the verses based on score
                    SongSearchHelper.ScoreResults(lSearchResults, searchTerms);
                    SearchRelevancyComparer comp = new SearchRelevancyComparer();
                    lSearchResults.Sort(comp);
                }
                else
                {
                    lSearchResults.Sort(new SongNumberComparer());
                }

                // Add to list
                foreach (SongSearchResult ssr in lSearchResults)
                    lbSearchResults.Items.Add(ssr);
                #endif
            }
            lbSearchResults.ResumeLayout();

            lblTitleSearchResults.Text = Loc.Get("Search results:") + " (" + lSearchResults.Count + ")";
        }
        private void btnSearchGo_Click(object sender, EventArgs e)
        {
#if DEMO
            new DemoVersionOnly("Fulltext song searching").ShowDialog();
#else

            if (txtFulltextSearch.Text == "")
                return;
            lastSearch = txtFulltextSearch.Text;

            // Wait for the current search
            int sleepCount = 50;
            while (bwSearch.IsBusy)
            {
                System.Threading.Thread.Sleep(100);
                Application.DoEvents();
                sleepCount--;
                if (sleepCount < 0)
                    return;
            }

            // Clear data
            ClearSearchResults();

            // Reset user interface
            pnlSearchSelection.BringToFront();
            lbSearchResults.Visible = false;
            fullSongPreview.Visible = false;
            pnlInitTumbler.Visible = true;
            lblSort.Visible = false;
            rNoSort.Visible = false;
            rRelevent.Visible = false;

            // Start background worker
            bwSearch.RunWorkerAsync();
#endif
        }
        private void txtFulltextSearch_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                e.Handled = true;
                if (lastSearch == txtFulltextSearch.Text)
                    StartProject(true);
                else
                    btnSearchGo_Click(null, null);
            }
        }
        private void txtFulltextSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Down)
            {
                e.Handled = true;
                if (lbSearchResults.Items.Count > 0 && lbSearchResults.SelectedIndex < lbSearchResults.Items.Count - 1)
                    lbSearchResults.SelectedIndex++;
            }
            if (e.KeyCode == Keys.Up)
            {
                e.Handled = true;
                if (lbSearchResults.Items.Count > 0 && lbSearchResults.SelectedIndex > 0)
                    lbSearchResults.SelectedIndex--;
            }
        }
        private void rRelevent_CheckedChanged(object sender, EventArgs e)
        {
#if DEMO
            new DemoVersionOnly("Fulltext song searching").ShowDialog();
#else    
            if (txtFulltextSearch.Text == "")
                return;
            RefreshSearchList();
#endif
        }
        private void btnSearchClose_Click(object sender, EventArgs e)
        {
            pnlSearchSelection.SendToBack();
            ClearSearchResults();
        }
        private void lbSearchResults_DoubleClick(object sender, EventArgs e)
        {
            if (lbSearchResults.SelectedIndex == -1)
                return;

            // Start the song
            SongSearchResult ssr = (SongSearchResult)lbSearchResults.SelectedItem;
            int autoNumber = ssr.autoNumber;
            dvSongs.RowFilter = "AutoNumber = " + autoNumber;
            if (dvSongs.Count > 0)
            {
                currentSong = (PresenterDataset.SongsRow)dvSongs[0].Row;
                StartProject(true);
            }
        }
        private void lbSearchResults_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index > -1 && e.Index < lbSearchResults.Items.Count)
            {
                // Init brushes
                int padding = 3;
                SolidBrush brushNormal = new SolidBrush(Color.FromArgb(223, 223, 223));
                SolidBrush brushSelected = new SolidBrush(Color.FromArgb(255, 204, 91));
                Rectangle textr = new Rectangle(padding + e.Bounds.X, padding + e.Bounds.Y, e.Bounds.Width - padding * 2, e.Bounds.Height - padding * 2);

                // Background
                if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
                    e.Graphics.FillRectangle(brushSelected, e.Bounds.X + 1, e.Bounds.Y + 1, e.Bounds.Width - 2, e.Bounds.Height - 2);
                else
                    e.Graphics.FillRectangle(brushNormal, e.Bounds.X + 1, e.Bounds.Y + 1, e.Bounds.Width - 2, e.Bounds.Height - 2);

                // Build the display string
                SongSearchResult ssr = (SongSearchResult)lbSearchResults.Items[e.Index];
                string s = ssr.songNumber + ". ";
                if (!ssr.isAtStart)
                    s += "...";
                s += ssr.verseData;

                // Measure and paint highlights
                StringFormat sf = new StringFormat();
                sf.SetMeasurableCharacterRanges(SearchHelper.FindCharacterRanges(s, SearchHelper.BreakSearchTerms(txtFulltextSearch.Text)));
                foreach (Region rg in e.Graphics.MeasureCharacterRanges(s, lbSearchResults.Font, textr, sf))
                    e.Graphics.FillRegion(new SolidBrush(Color.FromArgb(100, Color.Yellow)), rg);

                // Text
                if (ssr.score > 600)
                    e.Graphics.DrawString(s, lbSearchResults.Font, new SolidBrush(Color.FromArgb(80, 80, 80)), textr, sf);
                else
                    e.Graphics.DrawString(s, lbSearchResults.Font, Brushes.Black, textr, sf);
            }
        }
        private void lbSearchResults_MeasureItem(object sender, MeasureItemEventArgs e)
        {
            if (e.Index > -1 && e.Index < lbSearchResults.Items.Count)
            {
                // Build the display string
                SongSearchResult ssr = (SongSearchResult)lbSearchResults.Items[e.Index];
                string s = ssr.songNumber + ". ";
                if (!ssr.isAtStart)
                    s += "...";
                s += ssr.verseData;

                int padding = 3;
                int h = (int)e.Graphics.MeasureString(s, lbSearchResults.Font, lbSearchResults.Width - padding * 2).Height;
                e.ItemHeight = h + padding * 2;
            }
        }
        private void lbSearchResults_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lbSearchResults.SelectedIndex > -1 && lbSearchResults.SelectedIndex < lbSearchResults.Items.Count)
            {
                SongSearchResult ssr = (SongSearchResult)lbSearchResults.Items[lbSearchResults.SelectedIndex];
                UpdateSearchDisplayer(ssr.autoNumber, ssr.songNumber);
            }
        }
        private void UpdateSearchDisplayer(int autoNumber, int songNumber)
        {
            SongProject.LoadVerses(lSearchResultVerses, autoNumber, true);
            fullSongPreview.Init(lSearchResultVerses, songNumber, SearchHelper.BreakSearchTerms(txtFulltextSearch.Text));

            // Select the song behind the scenes
            dvSongs.RowFilter = "AutoNumber = " + autoNumber;
            if (dvSongs.Count > 0)
            {
                currentSong = (PresenterDataset.SongsRow)dvSongs[0].Row;
                UpdateUI();
            }
        }
        private void fullSongPreview_DoubleClick(object sender, EventArgs e)
        {
            StartProject(true); // all checks will be done... dont worrry about checking
        }

        // Quick searching
        private void txtSongsSearch_TextChanged(object sender, EventArgs e)
        {
#if DEMO
            if (searchExpired && txtSongsSearch.Text != "")
            {
                new DemoVersionOnly("Song searching").ShowDialog();
                return;
            }
#endif
            // Remove invalid chars
            string searchText = txtSongsSearch.Text;
            StringBuilder sb = new StringBuilder();
            string badChars = ",./<>?[]\\{}|!@#$%^&*()-=_+:;'\"";
            foreach (char c in searchText)
                if (!badChars.Contains(c.ToString()))
                    sb.Append(c);
            searchText = sb.ToString();

            // Try to parse the text to get song number
            int songnum = -1;
            if (searchText != "")
                if (!int.TryParse(searchText, out songnum))
                    songnum = -1;

            // Update row filter
            dvSongs.RowFilter =
                    "Title LIKE '" + searchText + "*' OR " +
                    "Chorus LIKE '" + searchText + "*' OR " +
                    "Number = " + songnum;
            if (searchText.Trim() != "" && songnum == -1)
                dvSongs.Sort = "Title ASC";
            else
                dvSongs.Sort = "Number ASC";

            // Update the visible selection
            if (dvSongs.Count > 0)
            {
                g.CurrentCell = new DataGridCell(0, 1);
                g.Select(0);
            }

            // FIX: changing current cell doesn't call cell changed
            g_CurrentCellChanged(null, null);
        }
        private void txtSongsSearch_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '*' || e.KeyChar == '%')
                e.Handled = true;
            if (e.KeyChar == (char)Keys.Enter)
            {
                e.Handled = true;

                if (dvSongs.Count > 0)
                    currentSong = (PresenterDataset.SongsRow)dvSongs[g.CurrentCell.RowNumber].Row;
                StartProject(true);
            }
        }
        private void txtSongsSearch_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Up && g.CurrentCell.RowNumber > 0)
            {
                g.CurrentCell = new DataGridCell(g.CurrentCell.RowNumber - 1, 1);
                e.Handled = true;
            }
            if (e.KeyCode == Keys.Down && g.CurrentCell.RowNumber + 1 < dvSongs.Count)
            {
                g.CurrentCell = new DataGridCell(g.CurrentCell.RowNumber + 1, 1);
                e.Handled = true;
            }
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
        private void prepareToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StartProject(false);
        }
        private void startToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StartProject(true);
        }
        private void StartProject(bool activate)
        {
#if DEMO
            searchExpired = true;
#endif
            if (currentSong == null)
                return;

            // Check if the song is already running
            foreach (IProject p in Program.Presenter.projects)
            {
                if (p.GetProjectType() == ProjectType.Song)
                {
                    SongProject songp = (SongProject)p;
                    if (songp.currentSong.AutoNumber == currentSong.AutoNumber)
                    {
                        Program.Presenter.ActivateController(songp);
                        this.Deactivate();
                        return;
                    }
                }
            }

            SongProject sp = new SongProject(currentSong);
            Program.Presenter.AddProject(sp);
            if (activate)
                sp.Activate();
            this.Deactivate(); // Clear ui 
        }

        #region IPopup Members

        public void Deactivate()
        {
            this.Hide();

            // NOTE: it would be a good time to cancel the bg worker for search
            // Not impl currently

            // Clear search section
            ClearSearchResults();
            lbSearchResults.Visible = false;
            fullSongPreview.Visible = false;
            lblSort.Visible = false;
            rNoSort.Visible = false;
            rRelevent.Visible = false;
            txtFulltextSearch.Text = "";
            pnlSearchSelection.SendToBack();
            pnlInitTumbler.Visible = false;

            // Clear grid selection and update button/link states
            currentSong = null;
            if (!Program.Presenter.bwSongsInit.IsBusy)
                g.UnSelect(g.CurrentCell.RowNumber);
            UpdateUI();

#if DEMO
            if (searchExpired)
                txtSongsSearch.Enabled = false;
#endif

            Program.Presenter.ClearNavList();
            Program.Presenter.UnregisterPopupWindow(this);
            Program.Presenter.DeactivateKeyClient(this);
            Program.Presenter.DeactiveSearcher();
        }
        public void Activate()
        {
            currentSong = null;
            txtSongsSearch.Text = "";
            if (dvSongs.Table != null)
                dvSongs.Sort = "Number ASC";

            this.Height = this.Parent.Height - this.Location.Y - 40;
            this.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom;
            Program.Presenter.ShowPopupWindow(this, this.PointToScreen(Point.Empty));
            this.Show();

            if (!Program.Presenter.bwSongsInit.IsBusy)
            {
                // Select the first song
                g.CurrentCell = new DataGridCell(0, 1);
                g.Select(0);
            }

            UpdateUI();
            txtSongsSearch.Focus();
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
