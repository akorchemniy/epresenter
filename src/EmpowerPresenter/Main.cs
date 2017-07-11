/* ePresenter is licensed under the GPLV3 -- see the 'COPYING' file details.
   Copyright (C) 2006 Alex Korchemniy */
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Text;
using System.Globalization;
using System.Windows.Forms;
using System.IO;
using System.Drawing.Drawing2D;
using System.Collections;

namespace EmpowerPresenter
{
    public partial class Main : Form, IPopupManager, IMessageFilter, IDisplayManager
    {
        internal List<IProject> projects = new List<IProject>();
        private Dictionary<IProject, EventHandler> activationHandlers = new Dictionary<IProject, EventHandler>();
        private Dictionary<IProject, EventHandler> deactivationHandlers = new Dictionary<IProject, EventHandler>();
        private Dictionary<IProject, EventHandler> refreshHandlers = new Dictionary<IProject, EventHandler>();
        internal Dictionary<Type, Control> controllers = new Dictionary<Type, Control>();
        internal Dictionary<Type, IDisplayer> displayers = new Dictionary<Type, IDisplayer>();
        internal IProject activeProjectInUI;
        internal IController activeController;
        internal IProject activeProjectInDisplay;
        private IDisplayer activeDisplayer;
        internal BackgroundWorker bwBibleInit = new BackgroundWorker();
        internal BackgroundWorker bwSongsInit = new BackgroundWorker();
        internal bool songsInitComplete = false;

        ////////////////////////////////////////////////////////////////////////
        public Main()
        {
            Application.AddMessageFilter(this);
            InitializeComponent();
            InitData();
            InitDragDrop();

            KillSplash(); // Close the native exe for splash
        }
        internal static void KillSplash()
        {
            try
            {
                Process[] p = System.Diagnostics.Process.GetProcessesByName("LaunchEP");
                if (p.Length > 0)
                    p[0].Kill();
            }
            catch { /*System.Diagnostics.Trace.WriteLine("Failed to close the splash");*/ }
        }
        private void InitData()
        {
            Program.SongsDS = new PresenterDataset();
            Program.BibleDS = new PresenterDataset();
            

            bwBibleInit.DoWork += new DoWorkEventHandler(bwBibleInit_DoWork);
            bwBibleInit.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwBibleInit_RunWorkerCompleted);
            bwBibleInit.RunWorkerAsync();

            bwSongsInit.DoWork += new DoWorkEventHandler(bwSongInit_DoWork);
            bwSongsInit.WorkerSupportsCancellation = true;
            bwSongsInit.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwSongInit_RunWorkerCompleted);
            bwSongsInit.RunWorkerAsync();
        }
        private void bwBibleInit_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            bibleSearchPnl1.Init();
        }
        private void bwBibleInit_DoWork(object sender, DoWorkEventArgs e)
        {
            using (FBirdTask t = new FBirdTask())
            {
                /////////////////////////////// BibleLookUp
                t.CommandText = "SELECT \"OrderNum\", \"VersionId\", \"RefBook\", \"Book\", \"DisplayBook\", \"NumberOfChapters\" FROM \"BibleLookUp\"";

                t.ExecuteReader();
                PresenterDataset.BibleLookUpRow bRow;
                Program.BibleDS.BibleLookUp.BeginLoadData();
                while (t.DR.Read())
                {
                    bRow = Program.BibleDS.BibleLookUp.NewBibleLookUpRow();
                    bRow.OrderNum = t.GetInt32(0);
                    bRow.VersionId = t.GetString(1);
                    bRow.MappingBook = t.GetString(2);
                    bRow.Book = t.GetString(3);
                    bRow.DisplayBook = t.GetString(4);
                    bRow.NumberOfChapters = t.GetInt32(5);
                    Program.BibleDS.BibleLookUp.AddBibleLookUpRow(bRow);
                }
                Program.BibleDS.BibleLookUp.EndLoadData();

                Program.BibleDS.AcceptChanges();
            }
        }
        private void bwSongInit_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            songsInitComplete = true;
            songSearchPnl1.Init();

            // Check the number of displays
            if (Screen.AllScreens.Length == 1)
                new DualDispInfo().ShowDialog();

            string logPath = Application.ExecutablePath + ".log";
            if (File.Exists(logPath) && new FileInfo(logPath).Length > 2000000)
                new Vendisoft.Forms.CrashReportProgress().Show();
        }
        private void bwSongInit_DoWork(object sender, DoWorkEventArgs e)
        {
            Random r = new Random(DateTime.Now.Millisecond);

            using (FBirdTask t = new FBirdTask())
            {
                /////////////////////////////// Songs
                t.CommandText = "SELECT \"AutoNumber\", \"Number\", \"Title\", \"Chorus\", " +
                    "\"Location\", \"DisplayDefault\", \"ImageId\", \"Overlay\", \"FontId\", \"Settings\" FROM \"Songs\"";
                t.ExecuteReader();
                PresenterDataset.SongsRow sRow;
                Program.SongsDS.Songs.BeginLoadData();
                int checkCancel = 0;
                while (t.DR.Read())
                {
                    if (checkCancel % 10 == 0 && bwSongsInit.CancellationPending)
                        return;

                    sRow = Program.SongsDS.Songs.NewSongsRow();
                    sRow.AutoNumber = t.GetInt32(0);
                    sRow.Number = t.GetInt32(1);
                    sRow.Title = RemovePunctuation(t.GetString(2));
                    sRow.Chorus = RemovePunctuation(t.GetString(3));
                    sRow.Location = t.GetString(4);
                    sRow.DisplayDefault = t.GetInt32(5) == 1 ? true : false;
                    sRow.Image = t.GetInt32(6);
                    sRow.Overlay = t.GetInt32(7);
                    sRow.FontId = t.GetInt32(8);
                    sRow.Settings = t.GetString(9);

                    Program.SongsDS.Songs.AddSongsRow(sRow);
                    checkCancel++;
                }
                Program.SongsDS.AcceptChanges();
                Program.SongsDS.Songs.EndLoadData();
                t.DR.Close();
            }
        }
        private string RemovePunctuation(string str)
        {
            string badChars = "?.,!:;'";
            foreach (char c in badChars)
                str = str.Replace(c.ToString(), "");
            return str;
        }
        private void LoadSettings()
        {
            this.selectTranslationPrimary(Program.ConfigHelper.BiblePrimaryTranslation);
            this.selectTranslationSecondary(Program.ConfigHelper.BibleSecondaryTranslation);
            this.selectDisplayVerse(Program.ConfigHelper.BibleNumVerses);
            this.setCurrentLanguage();
            if (Program.ConfigHelper.UseBlackBackdrop)
                DisplayEngine.BlackScreen = true;

            // Attach event last, this will prevent useless refreshes while loading
            Properties.Settings.Default.PropertyChanged += new PropertyChangedEventHandler(OnPropertyChanged);
        }

        // Main events
        private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            selectTranslationPrimary(Program.ConfigHelper.BiblePrimaryTranslation);
            selectTranslationSecondary(Program.ConfigHelper.BibleSecondaryTranslation);
            selectDisplayVerse(Program.ConfigHelper.BibleNumVerses);
        }
        private void Main_Load(object sender, EventArgs e)
        {
            InitNavigation();
            InitKeyManager();
            LoadSettings();
        }
        private void Main_Resize(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                if (Program.ConfigHelper.HideContentOnMin)
                    foreach (IDisplayer d in displayers.Values)
                        if (!d.ResidentDisplay())
                            d.HideDisplay();
            }
            this.Refresh();
        }
        private void Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                this.Hide();

                // Close all the projects
                ArrayList lProjects = new ArrayList();
                foreach (IProject p in this.projects) // Cannot modify the collection directly
                    lProjects.Add(p);
                for(int i = 0; i < lProjects.Count; i++)
                    ((IProject)lProjects[i]).CloseProject();

                // Close all the remaining displays
                //ArrayList lDisplayers = new ArrayList();
                //foreach (IDisplayer d in this.displayers.Values) // Cannot modify the collection directly
                //    lDisplayers.Add(d);
                //for (int i = 0; i < lProjects.Count; i++)
                //    ((IDisplayer)lDisplayers[i]).CloseDisplay();
            }
            catch (Exception ex){System.Diagnostics.Trace.WriteLine(ex.Message + " " + ex.ToString());}
        }

        // Project selection
        internal ISearchPanel currentSearcher = null;
        private void InitNavigation()
        {
            // Disable items that are not available
            btnMouseLeave(null, null); // reset button text
        }
        internal void ClearNavList()
        {
            btnBible.IsSelected = false;
            btnSongs.IsSelected = false;
        }
        internal void DeactiveSearcher()
        {
            currentSearcher = null;
        }
        private void btnBible_Click(object sender, EventArgs e)
        {
            ClearNavList();
            btnBible.IsSelected = true;
            bibleSearchPnl1.Activate();
            currentSearcher = bibleSearchPnl1;
        }
        private void btnSongs_Click(object sender, EventArgs e)
        {
            ClearNavList();
            btnSongs.IsSelected = true;
            songSearchPnl1.Activate();
            currentSearcher = songSearchPnl1;
        }
        private void lblF7_Click(object sender, EventArgs e)
        {
            btnSongs_Click(null, null);
        }
        private void lblF6_Click(object sender, EventArgs e)
        {
            btnBible_Click(null, null);
        }
        private void btnPPT_Click(object sender, EventArgs e)
        {

        }
        private void btnVideo_Click(object sender, EventArgs e)
        {

        }
        private void btnImage_Click(object sender, EventArgs e)
        {
            ClearNavList();
            this.AddProject(new ImageProject());
        }
        private void btnAnouncement_Click(object sender, EventArgs e)
        {
            ClearNavList();
            anouncementSearchPnl1.Activate();
            currentSearcher = anouncementSearchPnl1;
        }
        private void btnScroller_Click(object sender, EventArgs e)
        {
            ClearNavList();
            scrollerSearchPnl1.Activate();
            currentSearcher = scrollerSearchPnl1;
        }
        private void pbAddAnnouncement_Click(object sender, EventArgs e)
        {
            ClearNavList();
            AddProject(new AnouncementProject());
        }
        private void pbAddScroller_Click(object sender, EventArgs e)
        {
            ClearNavList();
            AddProject(new ScrollerProject());
        }

        // Project management
        public void AddProject(IProject proj)
        {
            if (proj.GetName() == "")
                throw (new ApplicationException("Project must have a name"));
            projects.Add(proj);

            // Add a list item
            ProjectListItem pli = new ProjectListItem();
            pli.project = proj;
            lbRunning.Items.Add(pli);

            // Check controller
            Type t = proj.GetControllerUIType();
            if (!controllers.ContainsKey(t))
                controllers.Add(t, (Control)Activator.CreateInstance(t));
            
            // Attach events
            EventHandler activationH = new EventHandler(project_requestactivate);
            proj.RequestActivate += activationH;
            activationHandlers.Add(proj, activationH);
            EventHandler deactivationH = new EventHandler(project_requestdeactivate);
            proj.RequestDeactivate += deactivationH;
            deactivationHandlers.Add(proj, deactivationH);
            EventHandler refreshH = new EventHandler(project_refreshevnt);
            proj.Refresh += refreshH;
            refreshHandlers.Add(proj, refreshH);

            ActivateController(proj);
        }
        public void ActivateController(IProject proj)
        {
            // Check if active already
            if (proj != activeProjectInUI)
            {
                activeProjectInUI = proj;
                Type t = proj.GetControllerUIType();
                if (!controllers.ContainsKey(t))
                    return;
                Control c = controllers[t];

                // Attach the project to the controller
                IController ctrl = (IController)c;
                ctrl.AttachProject(proj); // This method in turn refreshes te controller

                // UI tasks
                if (activeController != ctrl)
                {
                    activeController = ctrl;
                    activeProjectInUI = proj;
                    pnlProjectControllerZone.Controls.Clear();
                    pnlProjectControllerZone.Controls.Add(c);
                    c.Dock = DockStyle.Fill;
                    c.BringToFront();
                }
                
                // Select corresponding item in the list
                ProjectListItem pliTarget = null;
                foreach (ProjectListItem pli in lbRunning.Items)
                {if (pli.project == proj){pliTarget = pli; break;}}
                if (pliTarget != null)
                    lbRunning.SelectedItem = pliTarget;
            }
        }
        public void ActivateProjectInDisplay(IProject proj)
        {
            // First make sure the controller is visible
            ActivateController(proj);

            // Check displayer
            Type t = proj.GetDisplayerType();
            if (!displayers.ContainsKey(t))
                displayers.Add(t, (IDisplayer)Activator.CreateInstance(t));
            IDisplayer disp = displayers[t];

            // Check key controller state
            ActivateKeyClient(proj as IKeyClient);

            // Check if the original displayer needs to be hidden
            if (disp.ExclusiveDisplay() && activeDisplayer != null
                && disp.GetType() != activeDisplayer.GetType())
            {
                activeDisplayer.DetachProject();
                activeDisplayer.HideDisplay();
            }

            // Attach project to the displayer
            disp.AttachProject(proj);
            disp.ShowDisplay(); // Turns on content
            UpdateProjHideBtn(true); // Content is turned on
            if (disp.ExclusiveDisplay())
            {
                activeDisplayer = disp;
                activeProjectInDisplay = proj;
            }

            // Turn black screen off if we got an exclusive displayer
            if (disp.ExclusiveDisplay())
                DisplayEngine.BlackScreen = false;

            lbRunning.Refresh();
        }
        public void HidePrimaryDisplay()
        {
            if (activeDisplayer != null)
                activeDisplayer.HideDisplay();
            UpdateProjHideBtn(false);
        }
        public void UpdateProjHideBtn(bool on)
        {
            btnHideDisplay.IsSelected = on;
            btnHideDisplay.Refresh();
            if (on)
                btnHideDisplay.Tag = "Hide content";
            else
                btnHideDisplay.Tag = "Show content";
        }
        private void project_requestactivate(object sender, EventArgs e)
        {
            IProject proj = (IProject)sender;
            ActivateProjectInDisplay(proj);
        }
        private void project_requestdeactivate(object sender, EventArgs e)
        {
            IProject proj = (IProject)sender;
            
            // Remove from the list
            int ixRemove = -1;
            for (int i = 0; i < lbRunning.Items.Count; i++)
            {
                if (((ProjectListItem)lbRunning.Items[i]).project == proj)
                {
                    ixRemove = i; break;
                }
            }
            if (ixRemove != -1)
                lbRunning.Items.RemoveAt(ixRemove);

            // Remove the project
            projects.Remove(proj);

            if (activeProjectInUI == proj)
            {
                pnlProjectControllerZone.Controls.Clear();
                activeController.DetachProject();
                activeController = null;
                activeProjectInUI = null;
            }

            // Turn off the display
            UpdateProjHideBtn(false);
            if (activeProjectInDisplay == proj)
            {
                // Detach key client
                if (proj is IKeyClient)
                    DeactivateKeyClient((IKeyClient)proj);

                activeProjectInDisplay = null;
                lbRunning.Refresh();

                if (activeDisplayer.GetType() == typeof(DisplayEngine) && pinBg)
                {
                    DisplayEngine.PaintContent = false;
                    activeDisplayer.DetachProject();
                }
                else
                {
                    activeDisplayer.DetachProject();
                    activeDisplayer.HideDisplay();
                    activeDisplayer = null;
                }
            }
            
            // Run through projects determine if controller is neceesary
            Type tController = proj.GetControllerUIType();
            bool keepController = false;
            foreach (IProject p in projects)
            {
                if (p.GetControllerUIType() == tController)
                {
                    keepController = true;
                    break;
                }
            }
            if (!keepController)
            {
                Control c = controllers[tController];
                controllers.Remove(tController);
                pnlProjectControllerZone.Controls.Remove(c);
                c.Dispose();
            }

            // Run through projects determine if displayer is neceesary
            Type tDisplayer = proj.GetDisplayerType();
            if (displayers.ContainsKey(tDisplayer) && tDisplayer != typeof(DisplayEngine))
            {
                bool keepDisplayer = false;
                foreach (IProject p in projects)
                {
                    if (p.GetDisplayerType() == tDisplayer)
                    {
                        keepDisplayer = true;
                        break;
                    }
                }
                if (!keepDisplayer)
                {
                    IDisplayer disp = displayers[tDisplayer];
                    if (!disp.ResidentDisplay())
                    {
                        disp.CloseDisplay();
                        displayers.Remove(tDisplayer);
                        if ((object)disp is IDisposable)
                            ((IDisposable)(object)disp).Dispose();
                    }
                }
            }

            // Detach events
            proj.RequestActivate -= activationHandlers[proj];
            proj.RequestDeactivate -= deactivationHandlers[proj];
            proj.Refresh -= refreshHandlers[proj];
            activationHandlers.Remove(proj);
            deactivationHandlers.Remove(proj);
            refreshHandlers.Remove(proj);

            // Select next item in the list
            if (ixRemove == -1 && lbRunning.Items.Count > 0)
                lbRunning.SelectedIndex = 0; // Select the first item
            else
            {
                if (lbRunning.Items.Count - 1 < ixRemove) // If last item was removed
                    lbRunning.SelectedIndex = lbRunning.Items.Count - 1;
                else
                    lbRunning.SelectedIndex = ixRemove;
            }
        }
        private void project_refreshevnt(object sender, EventArgs e)
        {
            lbRunning.Refresh(); // Repaint
        }

        // Key contoller
        private int exKeyOwner = 0;
        private List<IKeyClient> keyClients = new List<IKeyClient>();
        private void InitKeyManager()
        {
            foreach (ToolStripMenuItem ti in menuStrip1.Items)
                RegisterExKeyOwnerMenu(ti);
        }
        public void ActivateKeyClient(IKeyClient kc) 
        {
            if (kc == null)
                return;

            if (!(keyClients.Count > 0 && keyClients.Contains(kc)))
                keyClients.Add(kc); 
        }
        public void DeactivateKeyClient(IKeyClient kc)
        {
            if ((keyClients.Count > 0 && keyClients.Contains(kc)))
                keyClients.Remove(kc);
        }
        public void RegisterExKeyOwnerForm(Form f)
        {
            BeginExKeyOwner();
            f.FormClosed += delegate { EndExKeyOwner(); };
        }
        public void RegisterExKeyOwnerControl(Control c)
        {
            c.GotFocus += delegate { BeginExKeyOwner(); };
            c.LostFocus += delegate { EndExKeyOwner(); };
        }
        public void RegisterExKeyOwnerMenu(ToolStripMenuItem ti)
        {
            ti.DropDownOpened += delegate { BeginExKeyOwner(); };
            ti.DropDownClosed += delegate { EndExKeyOwner(); };
        }
        public void BeginExKeyOwner() { exKeyOwner++;}
        public void EndExKeyOwner() { exKeyOwner--;}

        // Running projects
        Brush brushNormal;
        Brush brushSelected;
        Font boldFont;
        int ctxHit = -1;
        private void lbRunning_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index < 0 || e.Index >= lbRunning.Items.Count)
                return;

            if (boldFont == null)
                boldFont = new Font(this.Font, FontStyle.Bold);
            if (brushNormal == null)
                brushNormal = new SolidBrush(Color.FromArgb(223, 223, 223));
            if (brushSelected == null)
                brushSelected = new SolidBrush(Color.FromArgb(255, 204, 91));

            ProjectListItem pli = (ProjectListItem)lbRunning.Items[e.Index];

            // Draw the background
            if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
                e.Graphics.FillRectangle(brushSelected, e.Bounds.X + 1, e.Bounds.Y + 1, e.Bounds.Width - 2, e.Bounds.Height - 2);
            else
                e.Graphics.FillRectangle(brushNormal, e.Bounds.X + 1, e.Bounds.Y + 1, e.Bounds.Width - 2, e.Bounds.Height - 2);

            // Draw the name
            StringFormat sf = new StringFormat();
            sf.Trimming = StringTrimming.EllipsisCharacter;
            e.Graphics.DrawString(pli.project.GetName(), boldFont, Brushes.Black, new Rectangle(e.Bounds.X + 30, e.Bounds.Y + 9, e.Bounds.Width - 35, this.Font.Height + 2), sf);

            // Draw the image
            Image i = null;
            switch (pli.project.GetProjectType()) // Select the image based on the tag value
            {
                case ProjectType.Bible:
                    i = this.Enabled ? global::EmpowerPresenter.Properties.Resources.bib16 : global::EmpowerPresenter.Properties.Resources.bib16d;
                    break;
                case ProjectType.Song:
                    i = this.Enabled ? global::EmpowerPresenter.Properties.Resources.music16 : global::EmpowerPresenter.Properties.Resources.music16d;
                    break;
                case ProjectType.PPT:
                    i = this.Enabled ? global::EmpowerPresenter.Properties.Resources.ppt16 : global::EmpowerPresenter.Properties.Resources.ppt16d;
                    break;
                case ProjectType.Video:
                    i = this.Enabled ? global::EmpowerPresenter.Properties.Resources.video16 : global::EmpowerPresenter.Properties.Resources.video16d;
                    break;
                case ProjectType.Image:
                    i = this.Enabled ? global::EmpowerPresenter.Properties.Resources.image : global::EmpowerPresenter.Properties.Resources.image;
                    break;
                case ProjectType.Anouncement:
                    i = this.Enabled ? global::EmpowerPresenter.Properties.Resources.comment_edit : global::EmpowerPresenter.Properties.Resources.comment_edit;
                    break;
                case ProjectType.Scroller:
                    i = this.Enabled ? global::EmpowerPresenter.Properties.Resources.comment_edit : global::EmpowerPresenter.Properties.Resources.comment_edit;
                    break;
            }
            if (i != null)
                e.Graphics.DrawImage(i, new Rectangle(e.Bounds.X + 6, e.Bounds.Y + 8, i.Width, i.Height), new Rectangle(0, 0, i.Width, i.Height), GraphicsUnit.Pixel);

            // Draw active triangle
            if (pli.project == activeProjectInDisplay)
            {
                Point[] pts = new Point[] { new Point(e.Bounds.Width - 2, e.Bounds.Y + 1), // top left
                    new Point(e.Bounds.Width - 10, e.Bounds.Y + (int)((double)e.Bounds.Height / 2)),  // mid left
                    new Point(e.Bounds.Width - 2, e.Bounds.Y + e.Bounds.Height - 2),  // bottom left
                    new Point(e.Bounds.Width - 2, e.Bounds.Y + 1)}; // top left
                GraphicsPath gp = new GraphicsPath();
                gp.AddPolygon(pts);
                e.Graphics.FillPath(Brushes.Orange, gp);
            }
        }
        private void lbRunning_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lbRunning.SelectedItem is ProjectListItem)
            {
                ProjectListItem pli = (ProjectListItem)lbRunning.SelectedItem;
                ActivateController(pli.project);
            }
        }
        private void lbRunning_DoubleClick(object sender, EventArgs e)
        {
            if (lbRunning.SelectedItem is ProjectListItem)
            {
                ProjectListItem pli = (ProjectListItem)lbRunning.SelectedItem;
                ActivateProjectInDisplay(pli.project);
            }
        }
        private void lbRunning_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (Form.MouseButtons == MouseButtons.Right)
            {
                ctxHit = lbRunning.IndexFromPoint(e.Location);
                if (ctxHit != -1)
                    contextMenuStrip1.Show(lbRunning.PointToScreen(e.Location));
            }
        }
        private void lbRunning_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            e.Handled = true;
        }
        private void closeProjectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ((ProjectListItem)lbRunning.Items[ctxHit]).project.CloseProject();
        }
        private void activateProjectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ((ProjectListItem)lbRunning.Items[ctxHit]).project.Activate();
        }

        // Misc buttons
        private bool pinBg = false;
        private void btnMouseEnter(object sender, EventArgs e)
        {
            lblInfo.Text = Loc.Get(((Control)sender).Tag.ToString());
        }
        private void btnMouseLeave(object sender, EventArgs e)
        {
            lblInfo.Text = "";
        }
        private void btnBlackScreen_Click(object sender, EventArgs e)
        {
            DisplayEngine.BlackScreen = !DisplayEngine.BlackScreen;
        }
        private void btnHideDisplay_Click(object sender, EventArgs e)
        {
            if (activeDisplayer != null)
            {
                if (activeDisplayer.GetType() == typeof(DisplayEngine))
                {
                    if (pinBg)
                    {
                        DisplayEngine.PaintContent = !DisplayEngine.PaintContent;
                        UpdateProjHideBtn(DisplayEngine.PaintContent);
                    }
                    else
                    {
                        activeDisplayer.HideDisplay();
                        UpdateProjHideBtn(false);
                    }
                }
                else
                {
                    activeDisplayer.HideDisplay();
                    UpdateProjHideBtn(false);
                }
            }
            // If the displayer is off and the user wants to turn something on 
            // then pass the control on to activate
            else if (activeProjectInUI != null)
            {
                activeProjectInUI.Activate();
                UpdateProjHideBtn(true);
            }

            // Update the description for the button by simulating a mouse over
            btnMouseEnter(btnHideDisplay, null);
        }
        private void bntPinBackground_Click(object sender, EventArgs e)
        {
            pinBg = !pinBg;
            bntPinBackground.IsSelected = pinBg;
            bntPinBackground.Invalidate();

            if (activeDisplayer != null && activeProjectInDisplay == null)
                activeDisplayer.HideDisplay();
        }
        private void btnSmoothTransitions_Click(object sender, EventArgs e)
        {
            DisplayEngine.AnimateTransitions = !DisplayEngine.AnimateTransitions;
            btnSmoothTransitions.IsSelected = DisplayEngine.AnimateTransitions;
            btnSmoothTransitions.Invalidate();
        }

        // Drag and drop support
        internal List<string> allowedDragDropFiles = new List<string>();
        internal List<string> dragDropImageFiles = new List<string>();
        private void InitDragDrop()
        {
            // Image formats
            dragDropImageFiles.AddRange(new string[] { ".jpeg", ".jpg", ".png", ".gif", ".bmp" });
            allowedDragDropFiles.AddRange(dragDropImageFiles.ToArray());

            // Office file formats
            if (Type.GetTypeFromProgID("PowerPoint.Application") != null)
                allowedDragDropFiles.AddRange(new string[] { ".ppt", ".pps", ".pot" });
        }
        private void Main_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                bool ok = false;
                foreach(string f in files)
                {
                    if (File.Exists(f) && allowedDragDropFiles.Contains(Path.GetExtension(f).ToLower()))
                    {
                        ok = true;
                        break;
                    }
                }
                if (ok)
                    e.Effect = DragDropEffects.All;
                else
                    e.Effect = DragDropEffects.None;
            }
            else if (e.Data.GetDataPresent(DataFormats.Bitmap))
            {
                e.Effect = DragDropEffects.All;
            }
        }
        private void Main_DragDrop(object sender, DragEventArgs e)
        {
            bool doDragDrop = true;

            if (activeProjectInUI != null && activeController as IDragDropClient != null)
            {
                IDragDropClient ddc = (IDragDropClient)activeController;
                Point ptMouse = new Point(e.X, e.Y);
                if (ddc.DragDropHitTest(ptMouse, e.Data))
                {
                    ddc.OnDragDrop(e.Data);
                    doDragDrop = false;
                }
            }

            if (doDragDrop)
            {
                // Normal drag and drop
                if (e.Data.GetDataPresent(DataFormats.FileDrop))
                    OpenFileList((string[])e.Data.GetData(DataFormats.FileDrop));
                else if (e.Data.GetDataPresent(DataFormats.Bitmap))
                {
                    Image i = (Image)e.Data.GetData(DataFormats.Bitmap);
                    ImageProject ip = InternalFindImageProject();
                    ip.AddImage("Image", i);
                }
            }
        }
        private void OpenFileList(string[] files)
        {
            List<string> imageFiles = new List<string>();
            foreach (string file in files)
            {
                if (!File.Exists(file))
                    continue;
                string ext = Path.GetExtension(file).ToLower();
                switch (ext)
                {
                    case ".bmp":
                    case ".jpg":
                    case ".jpeg":
                    case ".png":
                    case ".gif":
                        imageFiles.Add(file);
                        break;
                }
            }

            // Start image project
            if (imageFiles.Count > 0)
                InternalFindImageProject().AddFiles(imageFiles.ToArray());
        }
        private void lblDragDrop_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            // Build filter string
            StringBuilder sb = new StringBuilder();
            sb.Append(Loc.Get("Supported files") + " | ");
            foreach (string s in allowedDragDropFiles)
            {
                sb.Append("*");
                sb.Append(s);
                sb.Append(";");
            }
            sb.Remove(sb.Length - 1, 1); // remove the extra ';' from last loop
            foreach (string s in allowedDragDropFiles)
            {
                sb.Append(" | ");
                sb.Append(s.Remove(0,1).ToUpper()); // In capital letters 
                sb.Append(" | ");
                sb.Append("*");
                sb.Append(s);
            }

            try
            {
                OpenFileDialog f = new OpenFileDialog();
                Program.Presenter.BeginExKeyOwner();
                f.Title = Loc.Get("Select files to open");
                f.Multiselect = true;
                f.Filter = sb.ToString();
                if (f.ShowDialog() == DialogResult.OK)
                    OpenFileList(f.FileNames);
            }
            finally { Program.Presenter.EndExKeyOwner(); }
        }
        private ImageProject InternalFindImageProject()
        {
            ImageProject imageProj = null;
            
            // Check if the project active in the UI is a ImageProject
            if (activeProjectInUI != null && activeProjectInUI.GetType() == typeof(ImageProject))
                return (ImageProject)activeProjectInUI;

            // Run through projects to find one
            foreach (IProject p in projects)
            {
                if (p.GetType() == typeof(ImageProject))
                {
                    imageProj = (ImageProject)p;
                    break;
                }
            }

            // If we didn't find anything then make a new one
            if (imageProj == null)
            {
                imageProj = new ImageProject();
                this.AddProject(imageProj);
            }
            return imageProj;
        }

        public DisplayEngine DisplayEngine
        {
            get 
            { 
                if (!displayers.ContainsKey(typeof(DisplayEngine)))
                    displayers.Add(typeof(DisplayEngine), new DisplayEngine());
                return (DisplayEngine)displayers[typeof(DisplayEngine)]; 
            }
        }

        #region Menu
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        // Primary translation
        private void englishKJVToolStripMenuItem_Click(object sender, EventArgs e)
        {
            selectTranslationPrimary(0);
        }
        private void russianToolStripMenuItem_Click(object sender, EventArgs e)
        {
            selectTranslationPrimary(1);
        }
        private void ukrainianToolStripMenuItem_Click(object sender, EventArgs e)
        {
            selectTranslationPrimary(2);
        }
        private void selectTranslationPrimary(int i)
        {
            englishKJVToolStripMenuItem.Checked = i == 0 ? true : false;
            russianToolStripMenuItem.Checked = i == 1 ? true : false;
            ukrainianToolStripMenuItem.Checked = i == 2 ? true : false;
            if (Program.ConfigHelper.BiblePrimaryTranslation != getTranslationPrimary())
            {
                Program.ConfigHelper.BiblePrimaryTranslation = getTranslationPrimary();
            }
        }
        private void selectTranslationPrimary(string version)
        {
            int i;
            switch (version)
            {
                case "RST": i = 1; break;
                case "UK": i = 2; break;
                default: i = 0; break;
            }

            selectTranslationPrimary(i);
        }
        private string getTranslationPrimary()
        {
            if (englishKJVToolStripMenuItem.Checked == true)
                return "KJV";
            else if (russianToolStripMenuItem.Checked == true)
                return "RST";
            else
                return "UK";
        }

        // Secondary translation
        private void noneToolStripMenuItem_Click(object sender, EventArgs e)
        {
            selectTranslationSecondary(0);
        }
        private void englishKJVToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            selectTranslationSecondary(1);
        }
        private void russianToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            selectTranslationSecondary(2);
        }
        private void ukrainianToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            selectTranslationSecondary(3);
        }
        private void selectTranslationSecondary(int i)
        {
            numberOfVersesToDisplayToolStripMenuItem.Visible = i == 0 ? true : false;
            noneToolStripMenuItem.Checked = i == 0 ? true : false;
            englishKJVToolStripMenuItem1.Checked = i == 1 ? true : false;
            russianToolStripMenuItem1.Checked = i == 2 ? true : false;
            ukrainianToolStripMenuItem1.Checked = i == 3 ? true : false;
            if (Program.ConfigHelper.BibleSecondaryTranslation != getTranslationSecondary())
            {
                Program.ConfigHelper.BibleSecondaryTranslation = getTranslationSecondary();
            }
        }
        private void selectTranslationSecondary(string version)
        {
            int i;
            switch (version)
            {
                case "KJV": i = 1; break;
                case "RST": i = 2; break;
                case "UK": i = 3; break;
                default: i = 0; break;
            }

            selectTranslationSecondary(i);
        }
        private string getTranslationSecondary()
        {
            if (noneToolStripMenuItem.Checked == true)
                return "";
            else if (englishKJVToolStripMenuItem1.Checked == true)
                return "KJV";
            else if (russianToolStripMenuItem1.Checked == true)
                return "RST";
            else
                return "UK";
        }

        // Verse count
        private void verseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            selectDisplayVerse(1); // number indicates setting index
        }
        private void versesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            selectDisplayVerse(2); // number indicates setting index
        }
        private void fitByFontSizeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            selectDisplayVerse(3); // number indicates setting index
        }
        private void selectDisplayVerse(int i)
        {
            verseToolStripMenuItem.Checked = i == 1 ? true : false;
            versesToolStripMenuItem.Checked = i == 2 ? true : false;
            fitByFontSizeToolStripMenuItem.Checked = i == 3 ? true : false;
            if (Program.ConfigHelper.BibleNumVerses != i)
            {
                Program.ConfigHelper.BibleNumVerses = i;
            }
        }

        // Application language and other options
        private System.ComponentModel.ComponentResourceManager resourceManager = new System.ComponentModel.ComponentResourceManager(typeof(Main));
        private void englishToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Program.ConfigHelper.CurrentLanguage != "")
            {
                Program.ConfigHelper.CurrentLanguage = "";
                applyNewLanguage();
            }
            setCurrentLanguage();
        }
        private void russianLanguageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Program.ConfigHelper.CurrentLanguage != "ru-RU")
            {
                Program.ConfigHelper.CurrentLanguage = "ru-RU";
                applyNewLanguage();
            }
            setCurrentLanguage();
        }
        private void setCurrentLanguage()
        {
            englishToolStripMenuItem.Checked = Program.ConfigHelper.CurrentLanguage == "";
            russianLangToolsStripMenuItem.Checked = Program.ConfigHelper.CurrentLanguage == "ru-RU";
        }
        private void applyNewLanguage()
        {
            MessageBox.Show(this, "Restart ePresenter to use the new language", "ePresenter", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        private void otherSettingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Settings settings = new Settings();
            settings.ShowDialog();
            settings.Dispose();
        }

        // Help menu
        private void helpContentsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Open the help contents
            try
            {
                // TODO: 
                string loc = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "help\\bible_selecting.html");
                System.Diagnostics.Process.Start(loc);
            }
            catch { }
        }
        private void migrationWizardToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // TODO: Clean out migration paths

            //#if DEMO
            //new DemoVersionOnly("Migration").ShowDialog();
            //#else
            //if (!Migration.OldVersionExists)
            //{
            //    MessageBox.Show(this, Loc.Get("A previous version was not detected."), Loc.Get("Error"));
            //}
            //else
            //{
            //    if (Program.Configuration.GetSetting("MigrationCompleted") == "True")
            //        if (MessageBox.Show(this, Loc.Get("Migration wizard has already been run. Running it again can corrupt your data. Do you wish to continue?"), Loc.Get("Warning"), MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes)
            //            return;
            //    new Migration().ShowDialog();
            //}
            //#endif
        }
        private void checkForUpdatesToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }
        private void commentsAndFeedbackToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                Process.Start("http://vendisoft.biz/contacts.aspx");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine(ex.ToString());
            }
        }
        private void bugReportToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }
        private void submitCrashLogToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Vendisoft.Forms.CrashReportProgress f = new Vendisoft.Forms.CrashReportProgress();
            f.Show();
        }
        private void aboutEPresenterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Form.ModifierKeys == Keys.Shift)
            {
                // TODO: Hidden diagnostics tool
            }
            else
                new AboutDialog().Show();
        }

        // Shortcuts
        private void keyControllerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Program.ConfigHelper.BibleNumVerses == 3)
                Program.ConfigHelper.BibleNumVerses = 1;
            else
                Program.ConfigHelper.BibleNumVerses = 3;
        }
        private void bibleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (btnBible.Enabled)
            {
                DeactiavetAllPopups();
                btnBible_Click(null, null);
            }
        }
        private void songsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (btnSongs.Enabled)
            {
                DeactiavetAllPopups();
                btnSongs_Click(null, null);
            }
        }
        private void startToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (currentSearcher != null)
                currentSearcher.TryDisplay();
            else if (activeProjectInUI != null)
                activeProjectInUI.Activate();
        }
        private void prepareToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (currentSearcher != null)
                currentSearcher.TryPrepare();
        }
        private void copyVerseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (bibleSearchPnl1 != null)
            {
                BibleVerse bv = bibleSearchPnl1.CopyCurrentVerse();
                if (bv != null)
                    Clipboard.SetText(bv.ToString() + " " + bv.Text);
            }
        }
        private void searchBoxToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (activeController != null && activeController.GetType() == typeof(BibleProjectView))
            {
                BibleProjectView c = (BibleProjectView)activeController;
                c.ShowSearchPopup();
            }
        }
        #endregion

        #region IPopupManager Members
        /// This region handles popup window regitrations

        private List<IPopup> lPopups = new List<IPopup>(); // Currently only one at a time
        public void ShowPopupWindow(IPopup p, Point ptScreen)
        {
            if (lPopups.Count > 0)
                UnregisterPopupWindow(lPopups[0]);
            lPopups.Add(p);

            Control c = (Control)(object)p;
            c.Location = this.PointToClient(ptScreen);
            if (!this.Controls.Contains(c))
                this.Controls.Add(c);

            c.BringToFront();
            c.Show();
        }
        public void UnregisterPopupWindow(IPopup c)
        {
            if (lPopups.Contains(c))
            {
                lPopups.Remove(c);
            }
        }
        public void DeactiavetAllPopups()
        {
            List<IPopup> dup = new List<IPopup>();
            foreach (IPopup p in lPopups)
                dup.Add(p);
            foreach (IPopup p in dup)
                p.Deactivate();
        }

        #endregion

        #region IMessageFilter Members

        public bool PreFilterMessage(ref Message m)
        {
            switch (m.Msg)
            {
                case 0x100:
                    Keys k = (Keys)m.WParam;
                    if (exKeyOwner == 0)
                    {
                        if (k == Keys.A)
                            btnSmoothTransitions_Click(null, null);
                        else if (k == Keys.B)
                            DisplayEngine.BlackScreen = !DisplayEngine.BlackScreen;
                        else if (k == Keys.Escape)
                        {
                            if (pinBg && activeDisplayer != null && activeDisplayer.GetType() == typeof(DisplayEngine))
                            {
                                DisplayEngine.PaintContent = false;
                                DisplayEngine.DetachProject();
                                activeProjectInDisplay = null;
                                lbRunning.Refresh();

                                // Update ui
                                UpdateProjHideBtn(false);
                                btnMouseEnter(btnHideDisplay, null); // Description
                            }
                            else
                                Program.Presenter.HidePrimaryDisplay();
                        }
                    }

                    if (keyClients.Count > 0)
                    {
                        // Send keys to the top of the "stack"
                        keyClients[keyClients.Count - 1].ProccesKeys((Keys)m.WParam, exKeyOwner != 0);

                        // LIMITATION: Only the top of the "stack" receives key notifications
                        // TODO: switch to chained (link-list) implementation
                    }
                    break;
                case 0x201: // Handle detecting popup deactivation
                    if (lPopups.Count > 0)
                    {
                        Control c = (Control)lPopups[0];
                        Rectangle r = c.ClientRectangle;
                        r.X += c.Location.X;
                        r.Y += c.Location.Y;

                        // Child popup can determine the behavior of deactivation and 
                        // prevent incorrect closing (ie. overhanging combobox popup)
                        if (!r.Contains(this.PointToClient(Form.MousePosition)))
                            lPopups[0].Deactivate(); 
                    }
                    break;
            }
            return false;
        }

        #endregion

        #region IDisplayManager Members

        public void RemoveDisplayer(Type t)
        {
            if (displayers.ContainsKey(t))
            {
                displayers.Remove(t);
            }
            if (activeDisplayer != null && activeDisplayer.GetType() == t)
            {
                activeDisplayer = null;
                activeProjectInDisplay = null;
                lbRunning.Refresh();
            }
        }
        public void DeactiveDisplayer()
        {
            activeDisplayer = null;
            activeProjectInDisplay = null;
            lbRunning.Refresh();
        }

        #endregion
    }
}