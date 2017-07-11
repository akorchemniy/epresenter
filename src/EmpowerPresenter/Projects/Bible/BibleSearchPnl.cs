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
    public partial class BibleSearchPnl : UserControl, IPopup, IKeyClient, ISearchPanel
    {
#if DEMO
        private bool searchExpired = false;
#endif
        
        private DataView dvBibBooks = new DataView();
        private int loadingSuspendCount = 0;

        // Searching
        private List<BibleSearchResult> lResults = new List<BibleSearchResult>();
        private BackgroundWorker bwSearch = new BackgroundWorker();
        private string lastSearch = "";
        private List<string> searchTerms = new List<string>();

        ////////////////////////////////////////////////////////////////
        public BibleSearchPnl()
        {
            InitializeComponent();
            pnlSearchSelection.SendToBack();
            pnlInitTumbler.Visible = true;
            pnlInitTumbler.BringToFront();

            bwSearch.WorkerSupportsCancellation = true;
            bwSearch.DoWork += new DoWorkEventHandler(bwSearch_DoWork);
            bwSearch.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwSearch_RunWorkerCompleted);
        }
        public void Init() // To support design time we keep things out of the constructor
        {
            Application.DoEvents();
            Properties.Settings.Default.PropertyChanged += new PropertyChangedEventHandler(OnPropertyChanged);
            
            // Init data
            loadingSuspendCount++;
            dvBibBooks.Table = Program.BibleDS.BibleLookUp;
            ResetBibleVerses(); // Load genesis chapter 1 
            loadingSuspendCount--;
            
            pnlInitTumbler.Visible = false;
            txtBibleLocation.Enabled = true;
            txtBibSearch.Enabled = true;

            // Register exclusive key owners for key manager
            Program.Presenter.RegisterExKeyOwnerControl(txtBibleLocation);
            Program.Presenter.RegisterExKeyOwnerControl(txtBibSearch);
            Program.Presenter.RegisterExKeyOwnerControl(lbBibleBook);
            Program.Presenter.RegisterExKeyOwnerControl(lbBibleChapter);
            Program.Presenter.RegisterExKeyOwnerControl(lbBibleVerse);
            Program.Presenter.RegisterExKeyOwnerControl(lbSearchVerses);
        }

        private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            ResetBibleVerses();
        }
        private void SelectVerse(string mappingbook, int chapter, int verse)
        {
            loadingSuspendCount++;
            UpdateBookList();

            // Find the book in the list
            BookData bdBook = null;
            foreach (BookData bd in lbBibleBook.Items)
            {
                if (bd.mappingName == mappingbook)
                {bdBook = bd; break;}
            }
            if (bdBook == null) 
            {
                lbBibleChapter.Items.Clear();
                lbBibleVerse.Items.Clear(); 
                return;
            }

            lbBibleBook.SelectedItem = bdBook;
            UpdateChapterList();
            lbBibleChapter.SelectedIndex = chapter - 1; 
            UpdateVerseList();
            lbBibleVerse.SelectedIndex = verse - 1; 

            loadingSuspendCount--;
        }
        private void UpdateBookList()
        {
            if (dvBibBooks.Count == 0 || dvBibBooks.RowFilter == "")
                dvBibBooks.RowFilter = "(VersionId = '" + Program.ConfigHelper.BiblePrimaryTranslation + "')"; ;

            // Build list of books
            List<BookData> books = new List<BookData>();
            foreach (DataRowView v in dvBibBooks)
            {
                PresenterDataset.BibleLookUpRow r = ((PresenterDataset.BibleLookUpRow)v.Row);
                books.Add(new BookData(r.DisplayBook, r.MappingBook));
            }

            // Get a list of items currently in the list
            List<BookData> inListBox = new List<BookData>();
            foreach (BookData s in lbBibleBook.Items)
                inListBox.Add(s);

            // Check if the two list are the same
            bool same = true;
            if (inListBox.Count != books.Count)
                same = false;
            else
            {
                for (int i = 0; i < inListBox.Count; i++)
                {
                    if (inListBox[i].mappingName != books[i].mappingName ||
                        inListBox[i].displayName != books[i].displayName)
                    {same = false; break;}
                }
            }

            // Update the listbox if the new list differs
            if (!same)
            {
                lbBibleBook.SuspendLayout();
                lbBibleBook.Items.Clear();
                foreach (BookData s in books)
                    lbBibleBook.Items.Add(s);
                lbBibleBook.ResumeLayout();
            }
        }
        private void UpdateChapterList()
        {
            // Check data integrity
            if (lbBibleBook.SelectedIndex >= dvBibBooks.Count)
                throw (new ApplicationException("Data mismatch"));
            if (lbBibleBook.SelectedIndex == -1)
                return;

            // Get number of verses
            int chapterCount = Program.BibleDS.BibleLookUp.FindByVersionIdMappingBook(Program.ConfigHelper.BiblePrimaryTranslation, dvBibBooks[lbBibleBook.SelectedIndex]["MappingBook"].ToString()).NumberOfChapters;
            int chaptersInList = lbBibleChapter.Items.Count;

            // Update the list if different
            if (chapterCount != chaptersInList)
            {
                lbBibleChapter.SuspendLayout();
                lbBibleChapter.Items.Clear();
                for(int i = 1; i <= chapterCount; i++)
                    lbBibleChapter.Items.Add(i.ToString());
                lbBibleChapter.ResumeLayout();
            }
        }
        private void UpdateVerseList()
        {
            if (lbBibleBook == null || lbBibleBook.SelectedIndex == -1 || lbBibleChapter.SelectedIndex == -1)
                return;

            // Get the data from db & load into the list
            lbBibleVerse.SuspendLayout();
            lbBibleVerse.Items.Clear();
            using (FBirdTask t = new FBirdTask())
            {
                t.CommandText = "SELECT \"DATA\" FROM \"BIBLEVERSES\" WHERE " +
                    "\"VERSION\" = '" + Program.ConfigHelper.BiblePrimaryTranslation + "' AND " +
                    "\"REFBOOK\" = '" + ((BookData)lbBibleBook.SelectedItem).mappingName + "' AND " +
                    "\"REFCHAPTER\" = " + lbBibleChapter.SelectedItem.ToString() + " " +
                    "ORDER BY \"REFBOOK\", \"REFCHAPTER\", \"REFVERSE\"";
                t.ExecuteReader();

                int i = 0;
                while (t.DR.Read())
                {
                    i++;
                    VerseData vd = new VerseData(t.GetString(0), i);
                    lbBibleVerse.Items.Add(vd);
                }
            }
            lbBibleVerse.ResumeLayout();
        }
        private ParseData ParseVerse(string reference, bool select)
        {
#if DEMO
            if (searchExpired)
            {
                ResetBibleVerses();
                new DemoVersionOnly("Bible searching").ShowDialog();
                return null;
            }
#endif
            if (Program.Presenter.bwBibleInit.IsBusy)
                return null;

            loadingSuspendCount++;

            string bibVersion = Program.ConfigHelper.BiblePrimaryTranslation;
            string text = reference;
            ParseData pd = new ParseData();
            int state = 0;

            // TODO: redo this code to be a real FSM like it should be
            foreach(char c in text)
            {
                if (c == '\'' || c == '*' || c == '%')
                    continue;

                #region IsSeperator

                if (char.IsSeparator(c) || char.IsWhiteSpace(c) || char.IsPunctuation(c))
                {
                    switch(state)
                    {
                        case 0:
                            if (pd.book.Length == 0)
                                continue;
                            if (pd.book.Length > 0 && pd.book.Length < 3 && char.IsDigit(pd.book, 0))
                            {
                                pd.book = pd.book.Trim() + c.ToString();
                                continue;
                            }
                            state++;
                            break;
                        case 1:
                            if (pd.chap.Length == 0)
                                continue;
                            state++;
                            break;
                        case 2:
                            if (pd.verse.Length == 0)
                                continue;
                            state++;
                            if (c == '-')
                                state++;
                            break;
                        case 3:
                            if (c == '-')
                                state++;
                            else
                                continue;
                            break;
                        case 4:
                            if (pd.endVerse.Length == 0)
                                continue;
                            goto label_Finish;
                        default:
                            break;
                    }
                }

                #endregion

                #region Is Letter

                else if (char.IsLetter(c))
                {
                    switch(state)
                    {
                        case 0:
                            pd.book += c.ToString();
                            break;
                        case 1:
                            if (pd.chap.Length != 0)
                                break;
                            if (pd.book.Length == 0)
                                pd.book += c.ToString();
                            else
                                pd.book += " " + c;
                            state = 0;
                            break;
                        case 2:
                            if (pd.verse.Length == 0)
                                continue;
                            goto label_Finish;
                        default:
                            break;
                    }
                }

                #endregion

                #region Is Digit

                else if (char.IsDigit(c))
                {
                    switch(state)
                    {
                        case 0:
                            if (pd.book.Length == 0)
                                pd.book += c.ToString() + " ";
                            else
                            {
                                state++;
                                pd.chap = c.ToString();
                            }
                            break;
                        case 1:
                            pd.chap += c.ToString();
                            break;
                        case 2:
                            pd.verse += c.ToString();
                            break;
                        case 4:
                            pd.endVerse += c.ToString();
                            break;
                        default:
                            break;
                    }
                }
                #endregion
            }

        label_Finish:
            if (select)
            {
                if (pd.book.Length > 0)
                {
                    dvBibBooks.RowFilter = "(VersionId = '" + bibVersion + "') AND " +
                            "(MappingBook LIKE '" + pd.book + "*' OR " +
                            "Book LIKE '" + pd.book + "*' OR " +
                            "DisplayBook LIKE '" + pd.book + "*')";
                    UpdateBookList();

                    lbBibleBook.SelectedIndex = 0;
                    UpdateChapterList();

                    if (pd.chap.Length > 0)
                    {
                        int chap = int.Parse(pd.chap);
                        if (lbBibleChapter.Items.Count < chap)
                            lbBibleChapter.SelectedIndex = -1;
                        else
                        {
                            lbBibleChapter.SelectedIndex = chap - 1;
                            UpdateVerseList();

                            if (pd.verse.Length > 0)
                            {
                                int v = int.Parse(pd.verse);
                                if (lbBibleVerse.Items.Count < v)
                                    lbBibleVerse.SelectedIndex = -1;
                                else
                                {
                                    lbBibleVerse.SelectedIndex = v - 1;
                                    UpdateStartButtons();
                                }
                            }
                        }
                    }
                    else { lbBibleVerse.Items.Clear(); }
                }
                else
                    ResetBibleVerses();
            }
            loadingSuspendCount--;

            return pd;
        }
        private void ResetBibleVerses()
        {
            loadingSuspendCount++;
            try
            {
                dvBibBooks.RowFilter = "";
                UpdateBookList(); // Reset the list
                Application.DoEvents();
                lbBibleBook.SelectedIndex = 0; // Select the first book
                UpdateChapterList();
                Application.DoEvents();
                lbBibleChapter.SelectedIndex = 0; // Select the first chap
                UpdateVerseList();
            } catch { /* TODO: Lame: catch all bugs that result from early list switches */ }
            loadingSuspendCount--;
        }
        private void UpdateStartButtons()
        {
            btnPrepare.Enabled = btnBibleDisplayVerse.Enabled = (lbBibleVerse.SelectedIndex != -1);
        }
        internal BibleVerse CopyCurrentVerse()
        {
            if (this.lbBibleBook.SelectedIndex == -1 ||
                this.lbBibleChapter.SelectedIndex == -1 ||
                this.lbBibleVerse.SelectedIndex == -1)
                return null;

            // Build start verse
            BibleVerse bv = new BibleVerse();
            bv.RefBook = ((BookData)this.lbBibleBook.SelectedItem).mappingName;
            bv.RefChapter = this.lbBibleChapter.SelectedIndex + 1;
            bv.RefVerse = ((VerseData)this.lbBibleVerse.SelectedItem).num;
            bv.Text = ((VerseData)this.lbBibleVerse.SelectedItem).text;
            return bv;
        }

        private void lbBibleBook_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (loadingSuspendCount > 0)
                return;

            UpdateChapterList();
            lbBibleVerse.Items.Clear();
        }
        private void lbBibleChapter_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (loadingSuspendCount > 0)
                return;

            UpdateVerseList();
        }
        private void lbBibleVerse_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (loadingSuspendCount > 0)
                return;
            lbBibleVerse.Refresh();
            UpdateStartButtons();
        }
        private void txtBibleFinder_TextChanged(object sender, EventArgs e)
        {
            // Quick searching
            if (txtBibleLocation.Text.Trim() == "")
                ResetBibleVerses();
            else
                ParseVerse(txtBibleLocation.Text, true);

            // Fulltext searcing
            if (lResults.Count > 0)
                RefreshSearchList();
        }
        private void lbBibleVerse_DoubleClick(object sender, EventArgs e)
        {
            StartProject(true);
        }
        private void lbBibleVerse_MeasureItem(object sender, MeasureItemEventArgs e)
        {
            if (e.Index >= lbBibleVerse.Items.Count || e.Index < 0)
                return;

            VerseData vd = (VerseData)lbBibleVerse.Items[e.Index];
            e.ItemWidth = lbBibleVerse.Width;
            e.ItemHeight = Convert.ToInt32(e.Graphics.MeasureString(vd.ToString(), lbBibleVerse.Font, lbBibleVerse.Width).Height);
            if (e.ItemHeight >= lbBibleVerse.Height - 10)
                e.ItemHeight = lbBibleVerse.Height - 50;
        }
        private void lbBibleVerse_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index >= lbBibleVerse.Items.Count || e.Index < 0 || lbBibleVerse.Items.Count == 0)
                return;

            VerseData vd = (VerseData)lbBibleVerse.Items[e.Index];
            string s = vd.ToString();

            RectangleF r = new RectangleF(e.Bounds.X, e.Bounds.Y, e.Bounds.Width, e.Bounds.Height);
            StringFormat sf = new StringFormat();
            CharacterRange[] charRanges = SearchHelper.FindCharacterRanges(s, searchTerms);
            sf.SetMeasurableCharacterRanges(charRanges);

            // Highlight verse if something found
            if (charRanges.Length > 0)
                e.Graphics.FillRectangle(Brushes.LightYellow, e.Bounds.X + 1, e.Bounds.Y + 1, e.Bounds.Width - 2, e.Bounds.Height - 1);

            if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
            {
                e.Graphics.FillRectangle(SystemBrushes.Highlight, e.Bounds.X + 1, e.Bounds.Y + 1, e.Bounds.Width - 2, e.Bounds.Height - 1);

                // Fill in the highlights
                foreach (Region rg in e.Graphics.MeasureCharacterRanges(s, lbSearchVerses.Font, r, sf))
                    e.Graphics.FillRegion(new SolidBrush(Color.FromArgb(100, Color.Orange)), rg);
                
                e.Graphics.DrawString(s, lbBibleVerse.Font, SystemBrushes.HighlightText, r);
            }
            else
            {
                // Fill in the highlights
                foreach (Region rg in e.Graphics.MeasureCharacterRanges(s, lbSearchVerses.Font, r, sf))
                    e.Graphics.FillRegion(new SolidBrush(Color.FromArgb(100, Color.Orange)), rg);

                e.Graphics.DrawString(s, lbBibleVerse.Font, Brushes.Black, r);
            }
        }
        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            UpdateVerseList();

            if (txtBibSearch.Text != "" && lResults.Count != 0 && lbSearchVerses.Items.Count != 0)
                RefreshSearchList(); // Force remeasure
        }

        // Search
        private void btnSearchGo_Click(object sender, EventArgs e)
        {
#if DEMO
            new DemoVersionOnly("Bible searching").ShowDialog();
#else
            if (txtBibSearch.Text == "")
                return;

            if (lastSearch == txtBibSearch.Text)
            {
                pnlSearchSelection.BringToFront();
            }
            else
            {
                int sleepCount = 50;
                while(bwSearch.IsBusy)
                {
                    System.Threading.Thread.Sleep(100);
                    Application.DoEvents();
                    sleepCount--;
                    if (sleepCount < 0)
                        return;
                }

                lastSearch = txtBibSearch.Text;
                pnlSearchSelection.BringToFront();
                lbSearchVerses.Visible = false;
                pnlInitTumbler.Visible = true;
                pnlInitTumbler.BringToFront();
                lblSort.Visible = false;
                rNoSort.Visible = false;
                rRelevent.Visible = false;
                pbNoItems.Visible = llNoResults.Visible = false;

                bwSearch.RunWorkerAsync();
            }
#endif
        }
        private void txtBibSearch_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                e.Handled = true;
                if (lastSearch == txtBibSearch.Text)
                    StartProject(true);
                else
                    btnSearchGo_Click(null, null);
            }
        }
        private void txtBibSearch_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Down && lbSearchVerses.Items.Count > 0 &&
                lbSearchVerses.SelectedIndex < lbSearchVerses.Items.Count - 1)
                lbSearchVerses.SelectedIndex++;
            else if (e.KeyCode == Keys.Up && lbSearchVerses.Items.Count > 0 &&
                lbSearchVerses.SelectedIndex > 0)
                lbSearchVerses.SelectedIndex--;
        }
        private void txtBibSearch_TextChanged(object sender, System.EventArgs e)
        {
            // Refresh the highlighting in the lists
            searchTerms = SearchHelper.BreakSearchTerms(txtBibSearch.Text);
            lbBibleVerse.Invalidate();
            lbSearchVerses.Invalidate();
        }
        private void bwSearch_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            RefreshSearchList();
        }
        private void bwSearch_DoWork(object sender, DoWorkEventArgs e)
        {
#if DEMO
            new DemoVersionOnly("Bible searching").ShowDialog();
#else
            lock (lResults)
            {
                List<string> searchTerms = SearchHelper.BreakSearchTerms(txtBibSearch.Text);
                lResults = BibleSearchHelper.Search(searchTerms);
            }
            Application.DoEvents();
#endif
        }
        private void RefreshSearchList()
        {
            if (txtBibSearch.Text == "")
                return;

            pnlSearchSelection.BringToFront();
            lbSearchVerses.SuspendLayout();
            lbSearchVerses.Items.Clear();
            lock (lResults)
            {
                #if !DEMO
                // Get search terms
                List<string> searchTerms = SearchHelper.BreakSearchTerms(txtBibSearch.Text); // Caller is responsible to make sure that search terms have not changed

                // Sort the results
                BibleVerseComparer bvc = new BibleVerseComparer();
                if (rRelevent.Checked && searchTerms.Count > 1)
                {
                    // Sort the verses based on score
                    BibleSearchHelper.ScoreResults(lResults, searchTerms);
                    SearchRelevancyComparer comp = new SearchRelevancyComparer();
                    lResults.Sort(comp);
                }
                else
                    lResults.Sort(bvc);

                // Double filter the results
                List<BibleSearchResult> lDoubleFilter = new List<BibleSearchResult>();
                foreach (BibleSearchResult r in lResults)
                {
                    foreach (BookData bd in lbBibleBook.Items) // Check book
                    {
                        if (bd.mappingName == r.bibVerse.RefBook)
                        {
                            lDoubleFilter.Add(r);
                            break;
                        }
                    }
                }

                // Add to the list
                foreach (BibleSearchResult r in lDoubleFilter)
                    lbSearchVerses.Items.Add(r);

                // Turn off the tumbler
                pnlInitTumbler.Visible = false;

                // Set visible items based on search result count
                pbNoItems.Visible = llNoResults.Visible = lDoubleFilter.Count == 0;
                lblSort.Visible = rNoSort.Visible = rRelevent.Visible = lbSearchVerses.Visible = !pbNoItems.Visible;

                lblTitleSearchResults.Text = Loc.Get("Search results:") + " (" + lDoubleFilter.Count + ")";
                #endif
            }
            lbSearchVerses.ResumeLayout();
        }
        private void lbSearchVerses_DoubleClick(object sender, EventArgs e)
        {
            if (lbSearchVerses.SelectedIndex != -1)
                StartProject(true);
        }
        private void lbSearchVerses_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lbSearchVerses.SelectedIndex == -1)
                return;
            BibleSearchResult si = (BibleSearchResult)lbSearchVerses.Items[lbSearchVerses.SelectedIndex];

            // Map search version to primary version
            int chap = si.bibVerse.RefChapter;
            int verse = si.bibVerse.RefVerse;
            string primTrans = Program.ConfigHelper.BiblePrimaryTranslation;
            if (si.bibVerse.RefVersion != primTrans)
            {
                if (((si.bibVerse.RefVersion == "RST" || si.bibVerse.RefVersion == "UK") && primTrans == "KJV") ||
                    ((primTrans == "RST" || primTrans == "UK") && si.bibVerse.RefVersion == "KJV"))
                {
                    chap = si.bibVerse.SecondaryChapter;
                    verse = si.bibVerse.SecondaryVerse;
                }
            }

            // Select verse in the list
            SelectVerse(si.bibVerse.RefBook, chap, verse);
            UpdateStartButtons();
        }
        private void lbSearchVerses_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index < 0)
                return;
            
            // Init
            BibleSearchResult si = (BibleSearchResult)lbSearchVerses.Items[e.Index];
            string txt = si.ToString();
            SolidBrush brushNormal = new SolidBrush(Color.FromArgb(223, 223, 223));
            SolidBrush brushSelected = new SolidBrush(Color.FromArgb(255, 204, 91));
            StringFormat sf = new StringFormat();
            sf.SetMeasurableCharacterRanges(SearchHelper.FindCharacterRanges(txt, searchTerms));
            int h = (int)e.Graphics.MeasureString(txt, lbSearchVerses.Font, lbSearchVerses.Width - 15).Height;
            Rectangle r = new Rectangle(e.Bounds.X + 7, e.Bounds.Y + 3, lbSearchVerses.Width - 15, h);

            // Draw the background
            if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
                e.Graphics.FillRectangle(brushSelected, e.Bounds.X + 1, e.Bounds.Y + 1, e.Bounds.Width - 2, e.Bounds.Height - 2);
            else
                e.Graphics.FillRectangle(brushNormal, e.Bounds.X + 1, e.Bounds.Y + 1, e.Bounds.Width - 2, e.Bounds.Height - 2);

            // Fill in the highlights
            foreach (Region rg in e.Graphics.MeasureCharacterRanges(txt, lbSearchVerses.Font, r, sf))
                e.Graphics.FillRegion(new SolidBrush(Color.FromArgb(100, Color.Yellow)), rg);

            // Draw text
            if (si.score > 600)
                e.Graphics.DrawString(txt, lbSearchVerses.Font, new SolidBrush(Color.FromArgb(80,80,80)), r, sf);
            else
                e.Graphics.DrawString(txt, lbSearchVerses.Font, Brushes.Black, r, sf);
        }
        private void lbSearchVerses_MeasureItem(object sender, MeasureItemEventArgs e)
        {
            if (e.Index < 0)
                return;
            BibleSearchResult si = (BibleSearchResult)lbSearchVerses.Items[e.Index];
            e.ItemHeight = (int)e.Graphics.MeasureString(si.ToString(), lbSearchVerses.Font, lbSearchVerses.Width - 15).Height + 6;
        }
        private void btnSearchClose_Click(object sender, EventArgs e)
        {
            pnlSearchSelection.SendToBack();
        }
        private void rRelevent_CheckedChanged(object sender, EventArgs e)
        {
            if (txtBibSearch.Text == "")
                return;
            if (lastSearch != txtBibSearch.Text)
                btnSearchGo_Click(null, null);
            else
                RefreshSearchList();
        }
        private void txtBibleLocation_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter && btnPrepare.Enabled)
            {
                e.Handled = true;
                StartProject(true);
            }
        }
        private void txtBibleLocation_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Down && lbBibleVerse.Items.Count > 0 &&
                lbBibleVerse.SelectedIndex < lbBibleVerse.Items.Count - 1)
                lbBibleVerse.SelectedIndex++;
            else if (e.KeyCode == Keys.Up && lbBibleVerse.Items.Count > 0 &&
                lbBibleVerse.SelectedIndex > 0)
                lbBibleVerse.SelectedIndex--;
        }
        private void btnBibleDisplayVerse_Click(object sender, EventArgs e)
        {
            StartProject(true);
        }
        private void llNoResults_LinkClicked(object sender, System.Windows.Forms.LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                string loc = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "help\\bible_searching.html");
                System.Diagnostics.Process.Start(loc);
            } catch { }
        }
        private void btnPrepare_Click(object sender, EventArgs e)
        {
            StartProject(false);
        }
        private void btnCancel_Click(object sender, EventArgs e)
        {
            Deactivate();
        }
        private void StartProject(bool activate)
        {
#if DEMO
            searchExpired = true;
#endif

            if (this.lbBibleBook.SelectedIndex == -1 ||
                this.lbBibleChapter.SelectedIndex == -1 ||
                this.lbBibleVerse.SelectedIndex == -1)
                return;

            // Build start verse
            BibleVerse bv = new BibleVerse();
            bv.RefBook = ((BookData)this.lbBibleBook.SelectedItem).mappingName;
            bv.RefChapter = this.lbBibleChapter.SelectedIndex + 1;
            bv.RefVerse = ((VerseData)this.lbBibleVerse.SelectedItem).num;

            #if !DEMO
            // Get selection range
            int startR = -1;
            int endR = -1;
            bool validSelction = false;
            ParseData pd = ParseVerse(txtBibleLocation.Text, false);
            if (pd.verse != "" && pd.endVerse != ""
                && int.TryParse(pd.verse, out startR) && int.TryParse(pd.endVerse, out endR)
                && endR >= startR)
                validSelction = true;
            #endif

            // Create & activate project
            BibleProject bp = new BibleProject(bv);
            #if !DEMO
            if (validSelction)
            {
                bp.selectionRangeStartVerse = startR;
                bp.selectionRangeEndVerse = endR;
            }
            #endif
            Program.Presenter.AddProject(bp);
            if (activate)
                bp.Activate();

            // Clear ui 
            this.Deactivate();
        }

        #region IPopup Members

        public void Deactivate()
        {
            this.Hide();

            // NOTE: it would be a good time to cancel the bib bg worker for search
            // Not impl currently

            // Clear search section
            if (lResults != null)
                lResults.Clear();
            lbSearchVerses.Visible = false;
            lblSort.Visible = false;
            rNoSort.Visible = false;
            rRelevent.Visible = false;
            txtBibSearch.Text = "";
            pnlSearchSelection.SendToBack();
            lastSearch = "";
            pnlInitTumbler.Visible = false;

            // Clear searches
            new DelayedTask(200, delegate {
                if (Program.Presenter.bwBibleInit.IsBusy)
                    return;
                if (txtBibleLocation.Text != "")
                    txtBibleLocation.Text = "";
                else
                {
                    #if DEMO
                    ResetBibleVerses();
                    #else
                    ParseVerse("", true);
                    #endif
                }
            });

            Program.Presenter.ClearNavList();
            Program.Presenter.UnregisterPopupWindow(this);
            Program.Presenter.DeactivateKeyClient(this);
            Program.Presenter.DeactiveSearcher();
        }
        public void Activate()
        {
            this.Height = this.Parent.Height - this.Location.Y - 40;
            this.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom;
            Program.Presenter.ShowPopupWindow(this, this.PointToScreen(Point.Empty));
            this.Show();

            UpdateStartButtons();
            txtBibleLocation.Focus();
            Program.Presenter.ActivateKeyClient(this);
        }

        #endregion

        #region IKeyClient Members

        public void ProccesKeys(Keys k, bool exOwer)
        {
            if (k == Keys.Escape)
                this.Deactivate();
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

        ////////////////////////////////////////////////////////////////
        class BookData
        {
            public string displayName = "";
            public string mappingName = "";
            public BookData(string displayName, string mappingName)
            {
                this.displayName = displayName;
                this.mappingName = mappingName;
            }
            public override string ToString()
            {
                return displayName;
            }
        }
        class VerseData
        {
            public int num = -1;
            public string text = "";
            public VerseData(string text, int num)
            {
                this.text = text;
                this.num = num;
            }
            public override string ToString()
            {
                return num.ToString() + ". " + text;
            }
        }
        class ParseData
        {
            public string book = "";
            public string chap = "";
            public string verse = "";
            public string endVerse = "";
        }
    }
}
