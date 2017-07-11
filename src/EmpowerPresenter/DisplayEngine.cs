/* ePresenter is licensed under the GPLV3 -- see the 'COPYING' file details.
   Copyright (C) 2006 Alex Korchemniy */
using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.IO;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace EmpowerPresenter
{
    /// <summary>
    /// Display engine is a pull model (updates based on events only)
    /// - renders only a GfxContext
    ///   + renders background and content seperately
    /// - supports animation between GfxContext updates
    ///   + images are switch midway
    ///   + image opacity is tweened
    ///   + text is tweened to zero between start and finish
    ///   + if there is text -> text tween then a choke is added to make smoother transition
    /// </summary>
    public class DisplayEngine : System.Windows.Forms.Form, IDisplayer
    {
        private EventHandler ehRefresh;

        internal IProject proj;
        private ContextMenu cm;
        internal bool windowMode = false;
        internal GfxContext currentCtx = null;
        private bool blackscreen = false;
        private bool paintContent = true;

        // Animation support
        private GfxContext startingContext = null;
        private GfxContext endingContext = null;
        private bool useAnimation = false;
        internal GfxContext secondaryCtx = null;
        private int animationDuration = 20;
        private int animationChoke = 7; 
        private int animationStep = 0;
        private bool animating = false;
        private bool rendering = false;
        private Timer t = new Timer();

        //////////////////////////////////////////////////////////////////////////////
        public DisplayEngine()
        {
            SetStyle(ControlStyles.ResizeRedraw, true);
            SetStyle(ControlStyles.DoubleBuffer, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.UserPaint, true);

            if (Form.ModifierKeys == Keys.Control)
                windowMode = true;

            InitializeComponent();

            cm = new ContextMenu();
            cm.MenuItems.Add(Loc.Get("Hide"), delegate { this.HideDisplay(); });
            this.ContextMenu = cm;
            this.TopMost = false;

            this.VisibleChanged += new EventHandler(DisplayEngine_VisibleChanged);
            this.FormClosing += new FormClosingEventHandler(ImageDisplayer_FormClosing);
            Microsoft.Win32.SystemEvents.DisplaySettingsChanged += new EventHandler(SystemEvents_DisplaySettingsChanged);

            // Anmiation
            t.Interval = 100;
            t.Tick += new EventHandler(t_Tick);

            // Configuration
            Properties.Settings.Default.PropertyChanged += new PropertyChangedEventHandler(OnPropertyChanged);
        }

        void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            //NativeResolution = Rectangle.Empty; // Fore settings to load
            UpdateWindowProperties();
            RefreshCurrentProj();
            if (Program.ConfigHelper.UseBlackBackdrop && Program.Presenter.activeProjectInDisplay == null)
                BlackScreen = true;
        }
        private void SystemEvents_DisplaySettingsChanged(object sender, EventArgs e)
        {
            //NativeResolution = Rectangle.Empty; // Fore settings to load
            UpdateWindowProperties();
            RefreshCurrentProj();
        }
        private void RefreshCurrentProj()
        {
            if (proj != null)
            {
                switch (proj.GetProjectType())
                {
                    case ProjectType.Anouncement:
                        ((AnouncementProject)proj).RefreshUI();
                        break;
                    case ProjectType.Bible:
                        ((BibleProject)proj).RefreshUI();
                        break;
                    case ProjectType.Song:
                        ((SongProject)proj).RefreshUI();
                        break;
                    case ProjectType.Image:
                        ((ImageProject)proj).RefreshUI();
                        break;
                }
            }
        }
        private void UpdateWindowProperties()
        {
            if (windowMode)
            {
                this.Size = new Size(800, 600);
                this.FormBorderStyle = FormBorderStyle.FixedDialog;
                this.Activate();
                this.BringToFront();
            }
            else
            {
                Screen s = Screen.AllScreens[Program.ConfigHelper.PresentationMonitor];
                this.Size = s.Bounds.Size;
                this.Location = s.Bounds.Location;
                this.BringToFront();
            }
        }

        #region Windows Form Designer generated code
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DisplayEngine));
            this.SuspendLayout();
            // 
            // DisplayEngine
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(576, 472);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Name = "DisplayEngine";
            this.ShowInTaskbar = false;
            this.TopMost = true;
            this.ResumeLayout(false);

        }
        #endregion

        private void project_Refresh(object sender, EventArgs e)
        {
            if (proj == null)
                return;

            if (!blackscreen)
            {
                GfxContext receivedContext = GetContextFromProject();
                if (useAnimation && receivedContext.supportAnimation)
                    StartAnimation(currentCtx, receivedContext);
                else
                {
                    currentCtx = receivedContext.Clone();
                    this.Refresh();
                }
            }
        }
        public GfxContext GetContextFromProject()
        {
            if (proj == null || proj as ISupportGfxCtx == null)
                return null;

            GfxContext receivedContext = ((ISupportGfxCtx)proj).GetCurrentGfxContext();

            // Correction for no content
            if (!paintContent)
                receivedContext.animTextOpacity = 0;

            return receivedContext;
        }
        private void SwitchToBlack()
        {
            GfxContext g = new GfxContext();
            g.destSize = NativeResolution.Size;
            g.animTextOpacity = 0;
            if (useAnimation)
                StartAnimation(currentCtx, g);
            else
            {
                currentCtx = g;
                this.Invalidate();
            }
            this.Visible = true;
            //this.BringToFront();
        }
        internal void RestoreContent()
        {
            /// Pre: previous state was black screen

            GfxContext g = GetContextFromProject();
            currentCtx.img = g.img; // Since last was black, copy image so we get fade
            if (useAnimation)
                StartAnimation(currentCtx, g);
            else
            {
                currentCtx = g;
                this.Invalidate();
            }
        }
        private void InternalHideDisplay()
        {
            this.Visible = false;
            currentCtx = new GfxContext(); // Clear out context
            currentCtx.destSize = NativeResolution.Size;
            currentCtx.animTextOpacity = 0;

            Program.Presenter.DeactiveDisplayer();
        }

        private static ImageAttributes GetIA(float transparency)
        {
            ImageAttributes a = new ImageAttributes();
            float[][] mi ={ 
               new float[] {1, 0, 0, 0, 0},
               new float[] {0, 1, 0, 0, 0},
               new float[] {0, 0, 1, 0, 0},
               new float[] {0, 0, 0, transparency, 0}, 
               new float[] {0, 0, 0, 0, 1}};
            ColorMatrix cm = new ColorMatrix(mi);
            a.SetColorMatrix(cm);
            return a;
        }
        public static void PaintGfxContext(GfxContext gfx, Graphics g, bool forceContent)
        {
            PaintGfxContextBg(gfx, g);
            PaintGfxContextContent(gfx, g, forceContent);
            if (gfx.customPaintingHandler != null)
                gfx.customPaintingHandler(gfx);
        }
        private static void PaintGfxContextBg(GfxContext gfx, Graphics g)
        {
            Size currentSize = gfx.destSize;
            if (gfx.img == null) // Check for null
            {
                g.FillRectangle(Brushes.Black, 0, 0, currentSize.Width, currentSize.Height);
                return;
            }

            // Draw the image along with opacity
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.CompositingMode = CompositingMode.SourceOver;
            Rectangle destr = new Rectangle(0, 0, currentSize.Width, currentSize.Height);
            g.DrawImage(gfx.img, destr, 0, 0, gfx.img.Width, gfx.img.Height, GraphicsUnit.Pixel);
            //g.DrawImage(gfx.img, destr, 0, 0, gfx.img.Width, gfx.img.Height, GraphicsUnit.Pixel, GetIA(gfx.animImageState));
            if (gfx.opacity < 0 && gfx.opacity > -256)
                g.FillRectangle(new SolidBrush(Color.FromArgb(Math.Abs(gfx.opacity), 0, 0, 0)),
                    -1, -1, currentSize.Width + 1, currentSize.Height + 1);
            else if (gfx.opacity > 0 && gfx.opacity < 256)
                g.FillRectangle(new SolidBrush(Color.FromArgb(Math.Abs(gfx.opacity), 255, 255, 255)),
                    -1, -1, currentSize.Width + 1, currentSize.Height + 1);
        }
        private static void PaintGfxContextContent(GfxContext gfx, Graphics g, bool forceContent)
        {
            Size currentSize = gfx.destSize;

            // Paint the text
            foreach (GfxTextRegion textReg in gfx.textRegions)
            {
                // Build the path
                GraphicsPath pth = new GraphicsPath();
                StringFormat sf = new StringFormat(StringFormatFlags.FitBlackBox);
                if (!textReg.fontClip)
                    sf.Trimming = (StringTrimming.None | sf.Trimming);
                sf.Trimming = StringTrimming.None;
                sf.Alignment = textReg.font.HorizontalStringAlignment;
                sf.LineAlignment = textReg.font.VerticalStringAlignment;
                pth.AddString(textReg.message, textReg.font.FontFamily, (int)textReg.font.FontStyle,
                    textReg.font.SizeInPoints, textReg.bounds, sf);

                // Shadow support
                if (textReg.font.Shadow)
                {
                    if (!gfx.imgShadowCache.ContainsKey(textReg.id))
                    {
                        // Calc color for shadow
                        Color cshadow = Color.FromArgb(130, textReg.font.ShadowColor);
                        if (textReg.font.Color == Color.Transparent)
                        {
                            Color c = gfx.opacity > 0 ? Color.White : Color.Black;
                            cshadow = Color.FromArgb(130, c);
                        }

                        // Create pen and fill
                        Pen p = new Pen(cshadow);
                        p.LineJoin = LineJoin.Round;

                        Bitmap imgShadowCache = new Bitmap(currentSize.Width / 5, currentSize.Height / 5);
                        Graphics g2 = Graphics.FromImage(imgShadowCache);
                        Matrix mx = new Matrix(1.0f / 5, 0, 0, 1.0f / 5, -(1.0f / 5), -(1.0f / 5));
                        g2.SmoothingMode = SmoothingMode.AntiAlias;
                        g2.Transform = mx;

                        g2.DrawPath(p, pth); // Draw

                        // Store in cache
                        gfx.imgShadowCache[textReg.id] = imgShadowCache;
                    }

                    if (gfx.animTextOpacity == 255)
                    {
                        Image shadow = gfx.imgShadowCache[textReg.id];
                        //g.CompositingMode = CompositingMode.SourceOver;
                        //g.CompositingQuality = CompositingQuality.AssumeLinear;
                        g.DrawImage(shadow, new Rectangle(0, 0, currentSize.Width, currentSize.Height),
                            0, 0, shadow.Width, shadow.Height, GraphicsUnit.Pixel); // , GetIA((float)gfx.animTextOpacity / 255));
                    }
                }

                // Outline support
                if (textReg.font.Outline == true)
                {
                    Color clrOutline = Color.FromArgb(gfx.animTextOpacity, textReg.font.OutlineColor);
                    Pen p = new Pen(clrOutline, 2);
                    p.LineJoin = LineJoin.Round;
                    g.DrawPath(p, pth);
                }

                // Determine auto color
                Color clr = textReg.font.Color;
                if (textReg.font.Color == Color.Transparent)
                    clr = gfx.opacity > 0 ? Color.Black : Color.White;

                // Adjust text opacity
                if (!forceContent)
                    clr = Color.FromArgb(gfx.animTextOpacity, clr);
                Brush br = new SolidBrush(clr);
                g.FillPath(br, pth);

                // Debug boxes
                //g.DrawRectangle(Pens.Red, Rectangle.Round(textReg.bounds));
            }
        }

        // Animation support
        private void StartAnimation(GfxContext startContext, GfxContext endContext)
        {
            if (animating) // If already animating
            {
                if (endContext != null)
                    endingContext = endContext.Clone();
                SkipAnimation(); // jumps to end without animating
                return;
            }

            // Check input
            if (endContext == null)
            {
                endContext = new GfxContext(); // empty context (paint black)
                endContext.destSize = NativeResolution.Size;
                endContext.opacity = -255;
            }
            if (startContext == null)
            {
                startContext = new GfxContext();
                startContext.destSize = NativeResolution.Size;
                startContext.animTextOpacity = 0;
            }

            // Check image to correct ramp
            if (startContext.img == null)
            {
                startContext.img = endContext.img;
                startContext.opacity = -255;
            }
            if (endContext.img == null)
            {
                endContext.img = startContext.img;
                endContext.opacity = -255;
            }

            // Release previous resources
            if (startingContext != null)
                startingContext.Dispose();
            if (endingContext != null)
                endingContext.Dispose();

            // Adjust animation duration and choke
            if (startContext.animTextOpacity == 0 || endContext.animTextOpacity == 0)
            {
                animationDuration = 10;
                animationChoke = 0;
            }
            else
            {
                animationDuration = 20;
                animationChoke = 7;
            }

            this.animating = true;
            this.startingContext = startContext.Clone();
            this.endingContext = endContext.Clone();
            t.Start();
        }
        private void SkipAnimation()
        {
            t.Stop();

            if (currentCtx != null)
                currentCtx.Dispose();
            this.currentCtx = endingContext.Clone();
            if (secondaryCtx != null)
            {
                secondaryCtx.Dispose();
                this.secondaryCtx = null;
            }
            if (startingContext != null)
            {
                startingContext.Dispose();
                startingContext = null;
            }
            if (endingContext != null)
            {
                endingContext.Dispose();
                endingContext = null;
            }
            this.animating = false;

            this.Invalidate();
        }
        private void t_Tick(object sender, EventArgs e)
        {
            animationStep++;
            if (animationStep >= animationDuration)
            {
                SkipAnimation();
                animationStep = 0;
                return;
            }

            if (startingContext != null && endingContext != null)
            {
                if (secondaryCtx == null)
                    secondaryCtx = endingContext.Clone();

                // Calc mid image switch
                secondaryCtx.animImageState = startingContext.animImageState = (float)animationStep / animationDuration;
                if (secondaryCtx.animImageState < 0.5f)
                    currentCtx.img = startingContext.img;
                else
                    currentCtx.img = endingContext.img;

                // Calc opacity changes
                double deltaOpacity = endingContext.opacity - startingContext.opacity;
                deltaOpacity /= animationDuration;
                currentCtx.opacity = (int)(startingContext.opacity + animationStep * deltaOpacity);

                // Calc text values
                double delta1 = ((double)startingContext.animTextOpacity / (animationDuration - animationChoke));
                double delta2 = ((double)endingContext.animTextOpacity / (animationDuration - animationChoke));
                currentCtx.animTextOpacity = (int)(startingContext.animTextOpacity - animationStep * delta1);
                currentCtx.animTextOpacity = currentCtx.animTextOpacity > 255 ? 255 : currentCtx.animTextOpacity;
                currentCtx.animTextOpacity = currentCtx.animTextOpacity < 0 ? 0 : currentCtx.animTextOpacity;
                if (animationStep > animationChoke)
                {
                    secondaryCtx.animTextOpacity = (int)(endingContext.animTextOpacity - ((animationDuration - animationStep) * delta2));
                    secondaryCtx.animTextOpacity = secondaryCtx.animTextOpacity > 255 ? 255 : secondaryCtx.animTextOpacity;
                    secondaryCtx.animTextOpacity = secondaryCtx.animTextOpacity < 0 ? 0 : secondaryCtx.animTextOpacity;
                }
                else
                    secondaryCtx.animTextOpacity = 0;

                // Issue invalidate
                this.Invalidate();
            }
        }

        // Event handlers
        protected override void OnPaint(PaintEventArgs e)
        {
            rendering = true;
            if (currentCtx == null)
            {
                currentCtx = new GfxContext();
                currentCtx.destSize = NativeResolution.Size;
            }
            if (windowMode)
            {
                float scaleFactorW = (float)this.Width / NativeResolution.Width;
                Matrix mx = new Matrix(scaleFactorW, 0, 0, scaleFactorW, -(scaleFactorW), -(scaleFactorW));
                e.Graphics.Transform = mx;
            }
            if (useAnimation && animating && secondaryCtx != null)
            {
                PaintGfxContextBg(currentCtx, e.Graphics);
                PaintGfxContextContent(currentCtx, e.Graphics, false);
                PaintGfxContextContent(secondaryCtx, e.Graphics, false);
            }
            else
                PaintGfxContext(currentCtx, e.Graphics, false);
            rendering = false;
        }
        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            if (e.Button == MouseButtons.Left && proj != null)
            {
                switch (proj.GetProjectType())
                {
                    case ProjectType.Bible:
                        BibleProject bp = (BibleProject)proj;
                        bp.GoNextSlide();
                        break;
                    case ProjectType.Song:
                        SongProject sp = (SongProject)proj;
                        sp.GoNextSlide();
                        break;
                }
            }
        }
        private void DisplayEngine_VisibleChanged(object sender, EventArgs e)
        {
            if (this.Visible)
                UpdateWindowProperties();
            else
                blackscreen = false;
        }
        private void ImageDisplayer_FormClosing(object sender, FormClosingEventArgs e)
        {
            InternalCloseDisplay();
        }

        public bool UseBlankCursor
        {
            get { return this.Cursor == Cursors.Default ? false : true; }
            set
            {
                if (value)
                {
                    byte[] b = global::EmpowerPresenter.Properties.Resources.BCursor;
                    MemoryStream ms = new MemoryStream(b);
                    this.Cursor = new Cursor(ms);
                }
                else
                    this.Cursor = Cursors.Default;
            }
        }
        public bool BlackScreen
        {
            get {return blackscreen; }
            set
            {
                if (blackscreen != value)
                {
                    blackscreen = value;
                    if (value)
                        SwitchToBlack();
                    else if (proj != null)
                        RestoreContent();
                    else
                        InternalHideDisplay();
                }
            }
        }
        public bool PaintContent
        {
            get { return paintContent; }
            set
            {
                if (paintContent != value)
                {
                    paintContent = value;

                    // Project check
                    if (proj == null)
                        return;

                    if (useAnimation)
                    {
                        // Build ending context
                        GfxContext endingContext = currentCtx.Clone();
                        endingContext.animTextOpacity = paintContent && proj != null ? 255 : 0;

                        StartAnimation(currentCtx, endingContext);
                    }
                    else
                    {
                        if (paintContent)
                            currentCtx.animTextOpacity = 255;
                        else
                            currentCtx.animTextOpacity = 0;
                        this.Invalidate();
                    }
                }
            }
        }
        public bool AnimateTransitions
        {
            get { return useAnimation; }
            set
            {
                if (useAnimation != value)
                {
                    useAnimation = value;
                    if (animating && !value)
                        SkipAnimation();
                }
            }
        }
        
        #region Static members

        private static Rectangle nativeRes;

        public static Image GetImageIncompatible(double scale)
        {
            using (Image i = new Bitmap(800, 600))
            {
                Graphics g = Graphics.FromImage(i);
                g.FillRectangle(Brushes.White, g.ClipBounds);
                string text = Loc.Get("Incompatible Presentation");
                Font f = new Font("Arial", 36F, FontStyle.Bold);
                StringFormat sf = new StringFormat();
                sf.Alignment = StringAlignment.Center;
                SizeF s = g.MeasureString(text, f, 800, sf);
                int w = Convert.ToInt32(s.Width);
                int h = Convert.ToInt32(s.Height);
                g.DrawString(text, f, Brushes.Red, new System.Drawing.Rectangle((800 -
                    w) / 2, (600 - h) / 2, w, h), sf);
                Image.GetThumbnailImageAbort imageAbort = new System.Drawing.Image.GetThumbnailImageAbort(ImageAbort);
                Image ret = i.GetThumbnailImage((int)(i.Width * scale), (int)(i.Height * scale), imageAbort, IntPtr.Zero);
                i.Dispose();
                return ret;
            }
        }
        public static bool ImageAbort()
        {
            return false;
        }
        public static Rectangle NativeResolution
        {
            set
            {
                if (Program.ConfigHelper.PresentationMonitor > (Screen.AllScreens.Length - 1))
                {
                    System.Diagnostics.Trace.WriteLine("Monitor configuration bad");
                    if (Screen.AllScreens.Length == 2)
                        DisplayEngine.nativeRes = Screen.AllScreens[1].Bounds;
                    else
                        DisplayEngine.nativeRes = Screen.AllScreens[0].Bounds;
                }
                else
                    DisplayEngine.nativeRes = Screen.AllScreens[Program.ConfigHelper.PresentationMonitor].Bounds;
            }
            get
            {
                if (nativeRes.Width == 0)
                    NativeResolution = Rectangle.Empty; // fore the settings to load
                return nativeRes;
            }
        }
        
        #endregion

        #region IDisplayer Members

        public void AttachProject(IProject proj)
        {
            if (this.proj != proj)
            {
                DetachProject();

                this.proj = proj;
                ehRefresh = new EventHandler(project_Refresh);
                this.proj.Refresh += ehRefresh;
            }
            this.Refresh();
        }
        public void DetachProject()
        {
            if (this.proj != null)
            {
                if (ehRefresh != null)
                {
                    this.proj.Refresh -= ehRefresh;
                    ehRefresh = null;
                }
            }
            this.proj = null;
        }
        public void ShowDisplay()
        {
            this.Visible = true;
            this.paintContent = true;
            this.project_Refresh(null, null);
        }
        public void HideDisplay()
        {
            InternalHideDisplay();
        }
        
        public void CloseDisplay()
        {
            this.Close();
            InternalCloseDisplay();
        }
        private void InternalCloseDisplay()
        {
            DetachProject();
            this.InternalHideDisplay();
            Program.Presenter.RemoveDisplayer(this.GetType());
            Program.Presenter.btnHideDisplay.IsSelected = false;
            Program.Presenter.btnSmoothTransitions.IsSelected = false;
            Program.Presenter.bntPinBackground.IsSelected = false;

            // Remove event handlers
            Microsoft.Win32.SystemEvents.DisplaySettingsChanged -= new EventHandler(SystemEvents_DisplaySettingsChanged);
        }
        public bool ExclusiveDisplay() { return true; }
        public bool ResidentDisplay() { return true; }

        #endregion
    }
    public delegate void GfxCustomPaint(GfxContext gfxContext);
    public class GfxContext : IDisposable
    {
        public Image img;
        public int opacity = -255;
        public Size destSize;
        public List<GfxTextRegion> textRegions = new List<GfxTextRegion>();

        // Animation support
        public bool supportAnimation = true;
        public int animTextOpacity = 255;
        public float animImageState = 1.0f;
        public Dictionary<Guid, Image> imgShadowCache = new Dictionary<Guid, Image>();

        // Custom painting support
        public object customPaintingArgs;
        public GfxCustomPaint customPaintingHandler;

        public GfxContext Clone()
        {
            GfxContext c = new GfxContext();
            c.img = this.img;
            c.opacity = this.opacity;
            c.destSize = this.destSize;
            c.supportAnimation = this.supportAnimation;
            c.textRegions = new List<GfxTextRegion>();
            foreach (GfxTextRegion tr in this.textRegions)
                c.textRegions.Add(tr);
            c.animTextOpacity = this.animTextOpacity;
            return c;
        }

        #region IDisposable Members
        public void Dispose()
        {
            this.img = null; // release a reference
            if (imgShadowCache != null)
            {
                foreach (Image i in imgShadowCache.Values)
                    i.Dispose();
                this.imgShadowCache.Clear();
            }
            if (textRegions != null)
                textRegions.Clear();
        }
        #endregion
    }
    public class GfxTextRegion
    {
        public Guid id = Guid.NewGuid();
        public RectangleF bounds;
        public string message;

        //////////////////////////
        // Font
        public PresenterFont font;
        public bool fontClip = false;

        public GfxTextRegion(){}
        public GfxTextRegion(Rectangle bounds, PresenterFont font, string message)
        {
            this.bounds = bounds;
            this.font = font;
            this.message = message;
        }
    }
}