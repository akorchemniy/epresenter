/* ePresenter is licensed under the GPLV3 -- see the 'COPYING' file details.
   Copyright (C) 2006 Alex Korchemniy */
using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;

using EmpowerPresenter.Data;
using System.ComponentModel;
using System.Windows.Forms;
using FirebirdSql.Data.FirebirdClient;

namespace EmpowerPresenter
{
    /*
        @startuml
        title ePresenter Bible feature - Class Diagram
        class BibleProject {
            - selection range
            - PrepareForDisplay()
        }
        class BibleVerse {
            - ref verse
            - secondary verse
        }
        class ParseData
        enum BibleRenderingFormat
        class BibleSearchResult {
            - verse
            - score
        }
        class BibleSearchHelper {
            - GetScore()
        }

        BibleSearchPnl -down-> BibleSearchHelper
        BibleSearchHelper -down-> BibleSearchResult
        BibleSearchResult -down-> BibleVerse
        BibleSearchPnl -down-> BibleVerse
        BibleSearchPnl -down-> BibleProject
        BibleProject -down-> BibleVerse
        BibleProject -down-> FireBird: LoadBibData >
        BibleProject -down-> VerseBreakDown: PrepareForDisplay >
        VerseBreakDown -down-> GfxTextRegion: PrepSlide >
        GfxContext -down-> GfxTextRegion
        DisplayEngine -down-> GfxContext
        @enduml
    */
    public class BibleProject : IProject, ISlideShow, IKeyClient, ISupportGfxCtx
    {
        public event EventHandler Refresh;
        public event EventHandler RequestActivate;
        public event EventHandler RequestDeactivate;
        private string name = "";
        
        // Bible data
        internal Dictionary<int, BibleVerse> bibVerses = new Dictionary<int,BibleVerse>();
        internal int currentVerseNum = -1;
        internal int selectionRangeStartVerse = -1; // UI helper for operator to see where
        internal int selectionRangeEndVerse = -1; // selection starts and ends

        // Slides (Prep data)
        internal Dictionary<int, VerseBreakDown> slideData = new Dictionary<int, VerseBreakDown>();
        internal int currentSubIndex = 0;
        
        // Graphics
        private PresenterFont bibFont;
        private ImageFactory imageFactory = new ImageFactory();
        private GfxContext graphicsContext = new GfxContext();

        //////////////////////////////////////////////////////////////////////////////
        public BibleProject(BibleVerse verse)
        {
            Program.ConfigHelper.BibleFormatChanged += delegate { RefreshData(); };

            this.name = verse.ToString();

            // Data
            currentVerseNum = verse.RefVerse;
            LoadBibDat(verse); // Load the bible data from the database
            PrepareForDisplay();
        }
        List<string> TranslationList()
        {
            List<string> translations = new List<string>();
            if (!string.IsNullOrEmpty(Program.ConfigHelper.BiblePrimaryTranslation))
                translations.Add(Program.ConfigHelper.BiblePrimaryTranslation);
            if (!string.IsNullOrEmpty(Program.ConfigHelper.BibleSecondaryTranslation))
                translations.Add(Program.ConfigHelper.BibleSecondaryTranslation);
            if (!string.IsNullOrEmpty(Program.ConfigHelper.BibleTertiaryTranslation))
                translations.Add(Program.ConfigHelper.BibleTertiaryTranslation);
            return translations;
        }
        private void LoadBibDat(BibleVerse verse)
        {
            // Understanding the databse
            // SELECT FIRST 10 * FROM BIBLEVERSES
            // SELECT DISTINCT VERSION FROM BIBLEVERSES
            // SELECT DISTINCT BOOK FROM BIBLEVERSES

            // Verses are queried in the number scheme of the primary translation
            // KJV Psalms 23 == RST Psalms 22
            // SELECT FIRST 10 * FROM BIBLEVERSES AS PRI WHERE PRI.VERSION = 'RST' AND PRI.BOOK = 'Psalms'

            // Old implementation of the Bibleverse table
            // KJV | Ref book = original KJV numbering | Book = RST equivalent numbering
            // RST | Ref book = original RST numbering | Book = KJV equivalent numbering
            // One advantage of the old method is that ability to preload all the version of a chapter relative to the primary translation

            // PENDING: This code needs to be refactored to use a universal numbering scheme
            // https://www.biblegateway.com/passage/?search=Psalm+23&version=RUSV
            // KJV | Ref book = KJV numbering | Book = KJV numbering
            // RST | Ref book = KJV numbering | Book = RST numbering

            List<string> translations = TranslationList();
            List<BibleVerse> verses = new List<BibleVerse>();
            bibVerses.Clear();
            currentVerseNum = verse.RefVerse;
            using (FBirdTask t = new FBirdTask())
            {
                string select = "SELECT ";
                for (int tnum = 1; tnum <= translations.Count; tnum++)
                {
                    if (tnum > 1)
                        select += ", ";
                    select += string.Format("T{0}.REFBOOK, T{0}.REFCHAPTER, T{0}.REFVERSE, T{0}.DATA", tnum);
                }
                string from = " FROM BIBLEVERSES AS T1";
                for (int transIx = 1; transIx < translations.Count; transIx++)
                {
                    bool doMaping = translations[0] == "KJV" || translations[transIx] == "KJV";
                    if (doMaping)
                        from += string.Format(" LEFT JOIN BIBLEVERSES AS T{0} ON T1.BOOK = T{0}.REFBOOK AND T1.CHAPTER = T{0}.REFCHAPTER AND T1.VERSE = T{0}.REFVERSE", (transIx + 1));
                    else
                        from += string.Format(" LEFT JOIN BIBLEVERSES AS T{0} ON T1.REFBOOK = T{0}.REFBOOK AND T1.REFCHAPTER = T{0}.REFCHAPTER AND T1.REFVERSE = T{0}.REFVERSE", (transIx + 1));
                }
                string where = " WHERE T1.VERSION = @Version1 AND T1.REFBOOK = @Book AND T1.REFCHAPTER = @Chapter";
                for (int transIx = 1; transIx < translations.Count; transIx++)
                    where += string.Format(" AND T{0}.VERSION = @Version{0}", (transIx + 1));
                string order = " ORDER BY T1.REFBOOK, T1.REFCHAPTER, T1.REFVERSE, T1.VERSE";

                t.CommandText = select + from + where + order;
                t.Command.Parameters.Add("@Book", FbDbType.VarChar, 50).Value = verse.RefBook;
                t.Command.Parameters.Add("@Chapter", FbDbType.Integer).Value = verse.RefChapter;
                for (int tnum = 1; tnum <= translations.Count; tnum++)
                    t.Command.Parameters.Add("@Version" + tnum, FbDbType.VarChar, 10).Value = translations[tnum - 1];
                t.ExecuteReader();

                while (t.DR.Read())
                {
                    int ix = 0;
                    BibleVerse bVerse = new BibleVerse();
                    bVerse.RefVersion = translations[0];
                    bVerse.RefBook = t.GetString(ix++);
                    bVerse.RefChapter = t.GetInt32(ix++);
                    bVerse.RefVerse = t.GetInt32(ix++);
                    bVerse.Text = t.GetString(ix++);
                    if (translations.Count > 1)
                    {
                        bVerse.SecondaryVersion = translations[1];
                        bVerse.SecondaryBook = t.GetString(ix++);
                        bVerse.SecondaryChapter = t.GetInt32(ix++);
                        bVerse.SecondaryVerse = t.GetInt32(ix++);
                        bVerse.SecondaryText = t.GetString(ix++);
                    }
                    if (translations.Count > 2)
                    {
                        bVerse.TertiaryVersion = translations[2];
                        bVerse.TertiaryBook = t.GetString(ix++);
                        bVerse.TertiaryChapter = t.GetInt32(ix++);
                        bVerse.TertiaryVerse = t.GetInt32(ix++);
                        bVerse.TertiaryText = t.GetString(ix++);
                    }
                    verses.Add(bVerse);
                }
            }
            verses.ForEach(v => bibVerses.Add(v.RefVerse, v));
        }
        public void RefreshData()
        {
            if (bibVerses.Count == 0) // Dont know where to start from
                return;

            // Check if bible version changed & reload data if necessary
            if (bibVerses[currentVerseNum].RefVersion != Program.ConfigHelper.BiblePrimaryTranslation ||
                bibVerses[currentVerseNum].SecondaryVersion != Program.ConfigHelper.BibleSecondaryTranslation ||
                bibVerses[currentVerseNum].TertiaryVersion != Program.ConfigHelper.BibleTertiaryTranslation)
                LoadBibDat(bibVerses[currentVerseNum]);

            currentSubIndex = 0;

            // Reprep all the graphics
            PrepareForDisplay();

            // Go to slide
            if (currentVerseNum > bibVerses.Count)
                currentVerseNum = bibVerses.Count;
            GotoVerse(currentVerseNum);
        }
        private bool IsMultiTrans()
        {
            return TranslationList().Count > 1;
        }
        
        // Graphics prep
        private void PrepareForDisplay()
        {
            // This method will prepare the data for the graphics engine to make it faster
            // It take all the bible text and break it across multiple slides
            // Requirements: reload data on the fly (save state and start from where left off)

            // Clean up
            int currentVerseBackup = currentVerseNum; // back this up just in case
            this.slideData.Clear();

            // Init
            bibFont = PresenterFont.GetFontFromDatabase(-1); // Get the bib font from db

            /// Split each of the verses if necessary
            foreach (BibleVerse bvCurrent in this.bibVerses.Values)
            {
                // Calculate max block size
                Size nativeSize = DisplayEngine.NativeResolution.Size;
                int transCount = TranslationList().Count;
                double insideHeight = nativeSize.Height;
                insideHeight -= imageFactory.paddingPixels * 2; // Top and bottom
                insideHeight -= imageFactory.paddingPixels * (transCount - 1); // Between translations
                insideHeight /= transCount;
                insideHeight -= bibFont.SizeInPoints * 1.5; // Space for verse labels
                int maxh = (int)insideHeight;
                Size maxSize = new Size(imageFactory.maxInsideWidth, maxh);

                VerseBreakDown d = new VerseBreakDown();
                d.bibleVerse = bvCurrent;
                d.primaryText = InternalBreakString(bvCurrent.RefVerse, bvCurrent.Text, maxSize);
                d.secondaryText = InternalBreakString(bvCurrent.SecondaryVerse, bvCurrent.SecondaryText, maxSize);
                d.tertiaryText = InternalBreakString(bvCurrent.TertiaryVerse, bvCurrent.TertiaryText, maxSize);
                slideData.Add(bvCurrent.RefVerse, d);
            }

            // Read initial opacity
            EnsureBackground();
            UpdateOpacity(Program.ConfigHelper.BibleImageOpacity);

            // Reset the current location
            if (bibVerses.Count > 0 && currentVerseBackup < bibVerses.Count + 1
                && currentVerseBackup != -1)
                currentVerseNum = currentVerseBackup;
            else
                currentVerseNum = 1;
        }
        private List<String> InternalBreakString(int versenumber, string originalText, Size maxSize)
        {
            /// Function summary:
            /// Go through the original string and construct new strings that compose
            /// a screenful of text. Repeat until the original string is exhausted.
            if (versenumber == -1)
                return new List<string>();

            List<String> stringParts = new List<string>();

            // Performance detour - check if the entire string fits
            if ((int)MeasureVerse(versenumber + ". " + originalText, bibFont, maxSize.Width).Height <= maxSize.Height)
            {
                stringParts.Add(versenumber + ". " + originalText);
                return stringParts;
            }
            
            // Initial step: get the raw split data
            string remainingText = originalText;
            while (remainingText.Length > 0)
            {
                string currentPart = "";
                string nextStringlet = "";
                while ((int)MeasureVerse(versenumber + ". " + currentPart + nextStringlet + "...", bibFont, maxSize.Width).Height <= maxSize.Height)
                {
                    currentPart += nextStringlet;
                    remainingText = remainingText.Substring(nextStringlet.Length).Trim();
                    if (remainingText != null && remainingText != "")
                    {
                        int ix = remainingText.IndexOf(" ");
                        if (ix != -1)
                            nextStringlet = remainingText.Substring(0, ix+1);
                        else
                            nextStringlet = remainingText;
                    }
                    else
                        break;
                }
                stringParts.Add(currentPart);
            }
            
            // Step two: Balance word counts for last two
            string a = stringParts[stringParts.Count - 1];
            string b = stringParts[stringParts.Count - 2];
            string c = b + a;
            int mid = c.Length / 2;
            if (b.Length - a.Length > 20 && c.IndexOf(" ") != -1)
            {
                // Merge the two string and find a point to break the string
                int ixBreakPoint = -1;
                int ixProximity = int.MaxValue;
                char[] chars = c.ToCharArray();
                for(int i = 0; i < chars.Length; i++)
                {
                    if (chars[i] == ' ')
                    {
                        if (Math.Abs(mid - i) < ixProximity)
                        {
                            ixBreakPoint = i;
                            ixProximity = Math.Abs(mid - i);
                        }
                    }
                }

                // Remove the two last items
                stringParts.RemoveAt(stringParts.Count - 1);
                stringParts.RemoveAt(stringParts.Count - 1);

                // Add new two last items
                stringParts.Add(c.Substring(0, ixBreakPoint + 1));
                stringParts.Add(c.Substring(ixBreakPoint + 1));
            }

            // Step three: Add elipsis
            List<string> output = new List<string>();
            for (int i = 0; i < stringParts.Count; i++)
            {
                if (i == 0)
                    output.Add(versenumber + ". " + stringParts[i] + "...");
                else if (i == stringParts.Count - 1)
                    output.Add(versenumber + ". " + "..." + stringParts[i]);
                else
                    output.Add(versenumber + ". " + "..." + stringParts[i] + "...");
            }
            return output;
        }
        private RectangleF MeasureVerse(string text, PresenterFont f, int width)
        {
            // init uninitialized objects
            Graphics gMeasuring = Graphics.FromImage(new Bitmap(1, 1));
            GraphicsPath pathMeasuring = new GraphicsPath();
            StringFormat sfMeasuring = (StringFormat)StringFormat.GenericTypographic.Clone();
            sfMeasuring.Alignment = StringAlignment.Near;
            sfMeasuring.LineAlignment = StringAlignment.Near;
            sfMeasuring.FormatFlags = StringFormatFlags.FitBlackBox | StringFormatFlags.NoClip | StringFormatFlags.MeasureTrailingSpaces;

            pathMeasuring.Reset();
            pathMeasuring.AddString(text, f.FontFamily, (int)f.FontStyle, f.SizeInPoints, new Rectangle(0, 0, width, 1), sfMeasuring);
            return pathMeasuring.GetBounds();
        }

        // Graphics
        public void RefreshUI()
        {
            if (Refresh != null)
                Refresh(this, null);
        }
        public void UpdateOpacity(int newVal)
        {
            graphicsContext.opacity = newVal;
            RefreshUI();
        }
        public void UpdateBackgroundImage(Image background)
        {
            graphicsContext.img = background;
            RefreshUI();
        }
        public void UpdateBackgroundImage(PhotoInfo photo)
        {
            UpdateBackgroundImage(photo.FullSizeImage);
        }

        public GfxContext GetCurrentGfxContext()
        {
            // Determine current format ?? TODO: refactor to rely on center information
            BibleRenderingFormat format = BibleRenderingFormat.SingleVerse;
            if (Program.ConfigHelper.BibleNumVerses == 3)
                format = BibleRenderingFormat.FontFit;
            else if (Program.ConfigHelper.BibleSecondaryTranslation != "")
                format = BibleRenderingFormat.MultiTranslation;
            else if (Program.ConfigHelper.BibleNumVerses == 2)
                format = BibleRenderingFormat.DoubleVerse;

            EnsureBackground();

            switch(format)
            {
                case BibleRenderingFormat.SingleVerse:
                    imageFactory.PrepSlideSingleVerse(graphicsContext, slideData[currentVerseNum], currentSubIndex, bibFont);
                    break;
                case BibleRenderingFormat.DoubleVerse:
                    if (currentVerseNum == bibVerses.Count)
                        imageFactory.PrepSlideSingleVerse(graphicsContext, slideData[currentVerseNum], currentSubIndex, bibFont);
                    else
                    {
                        if (!imageFactory.PrepSlideDoubleVerse(graphicsContext, bibVerses, currentVerseNum, bibFont))
                            imageFactory.PrepSlideSingleVerse(graphicsContext, slideData[currentVerseNum], currentSubIndex, bibFont);
                    }
                    break;
                case BibleRenderingFormat.MultiTranslation:
                    if (!imageFactory.PrepSlideMultiTranslation(graphicsContext, slideData[currentVerseNum], currentSubIndex, bibFont))
                        imageFactory.PrepSlideSingleVerse(graphicsContext, slideData[currentVerseNum], currentSubIndex, bibFont);
                        break;
                case BibleRenderingFormat.FontFit:
                    imageFactory.PrepFontFit(graphicsContext, bibVerses, currentVerseNum, bibFont);
                    break;
            }
            return graphicsContext.Clone();
        }
        public void EnsureBackground()
        {
            if (graphicsContext.img == null)
            {
                PhotoInfo pi = new PhotoInfo();
                pi.ImageId = Program.ConfigHelper.BibleImage;
                UpdateBackgroundImage(pi.FullSizeImage); // Set if not init
            }
        }

        // Properties
        public bool IsFirst 
        { 
            get 
            { 
                return (this.currentVerseNum == 1 && this.bibVerses[1].RefChapter == 1); 
            } 
        }
        public bool IsLast 
        { 
            get 
            {
                BibleVerse bv = bibVerses[1];
                int chapCount = Program.BibleDS.BibleLookUp.FindByVersionIdMappingBook(bv.RefVersion, bv.RefBook).NumberOfChapters;
                return (this.bibVerses.Count == this.currentVerseNum && this.bibVerses[1].RefChapter == chapCount && currentSubIndex == slideData[currentVerseNum].MaxCount - 1); 
            } 
        }

        #region IProject Members

        public string GetName(){return name;}
        public ProjectType GetProjectType() { return ProjectType.Bible; }
        public void Activate()
        {
            Program.Presenter.ActivateKeyClient(this);
            if (RequestActivate != null)
                RequestActivate(this, null);
        }
        public void CloseProject()
        {
            if (RequestDeactivate != null)
                RequestDeactivate(this, null);
        }
        public Type GetControllerUIType() { return typeof(BibleProjectView);}
        public Type GetDisplayerType() { return typeof(DisplayEngine); }
        public bool IsPrimaryDisplay() { return true; }

        #endregion

        #region ISlideShow Members

        public void GotoVerse(int verseNumber)
        {
            GotoSlide(verseNumber, 0, true);
        }
        public void GotoSlide(int verseNumber, int subIndex)
        {
            GotoSlide(verseNumber, subIndex, false);
        }
        public void GotoSlide(int verseNumber, int subIndex, bool forceRefresh)
        {
            if (currentVerseNum != verseNumber || currentSubIndex != subIndex || forceRefresh)
            {
                // Update the current verse number
                currentVerseNum = verseNumber;
                currentSubIndex = subIndex;

                // Update the name of the project
                this.name = bibVerses[currentVerseNum].ToString();

                RefreshUI();

                Program.Presenter.ActivateKeyClient(this);
            }
        }
        public void GoNextSlide()
        {
            // Try to step up the sub index
            if (currentSubIndex < slideData[currentVerseNum].MaxCount - 1)
                GotoSlide(currentVerseNum, currentSubIndex + 1);
            else
            {
                // Try to step up the verse count
                if (currentVerseNum < bibVerses.Count)
                    GotoSlide(currentVerseNum + 1, 0);
                else
                {
                    // Try to navigate a chapter forward
                    BibleVerse bv = bibVerses[1];
                    int chapCount = Program.BibleDS.BibleLookUp.FindByVersionIdMappingBook(bv.RefVersion, bv.RefBook).NumberOfChapters;
                    if (bv.RefChapter < chapCount)
                    {
                        bv.RefChapter++;
                        bv.RefVerse = 1;
                        LoadBibDat(bv);
                        PrepareForDisplay();
                        GotoVerse(1);
                    }
                }
            }
        }
        public void GoPrevSlide()
        {
            if (currentSubIndex > 0)
                GotoSlide(currentVerseNum, currentSubIndex - 1);
            else
            {
                if (currentVerseNum > 1)
                    GotoSlide(currentVerseNum - 1, slideData[currentVerseNum - 1].MaxCount - 1);
                else
                {
                    // Try to navigate a chapter back
                    BibleVerse bv = bibVerses[1];
                    if (bv.RefChapter > 1)
                    {
                        bv.RefChapter--;
                        bv.RefVerse = 1;
                        LoadBibDat(bv);
                        PrepareForDisplay();
                        GotoVerse(bibVerses.Count);
                    }
                }
            }
        }

        #endregion

        #region IKeyClient Members

        public void ProccesKeys(Keys k, bool exOwer)
        {
            if (exOwer)
                return;

            if (k == Keys.Right || k == Keys.Down || k == Keys.PageDown || k == Keys.Space || k == Keys.Enter)
            {
                GoNextSlide();
                ActivateController();
            }
            else if (k == Keys.Left || k == Keys.Up || k == Keys.PageUp || k == Keys.Back)
            {
                GoPrevSlide();
                ActivateController();
            }
        }
        private void ActivateController()
        {
            if (Program.Presenter.activeController == null ||
                Program.Presenter.activeController.GetType() != this.GetControllerUIType())
                Program.Presenter.ActivateController(this);
        }

        #endregion

        //////////////////////////////////////////////////////////////////////////////
        internal class VerseBreakDown
        {
            // Shared information
            public BibleVerse bibleVerse;

            // Broken across slides
            public List<string> primaryText = new List<string>();
            public List<string> secondaryText = new List<string>();
            public List<string> tertiaryText = new List<string>();

            public List<string> LinesForSlide(int slide)
            {
                List<string> text = new List<string>();
                if (primaryText.Count > 0)
                    text.Add(primaryText[Math.Min(slide, primaryText.Count - 1)]);
                if (secondaryText.Count > 0)
                    text.Add(secondaryText[Math.Min(slide, secondaryText.Count - 1)]);
                if (tertiaryText.Count > 0)
                    text.Add(tertiaryText[Math.Min(slide, tertiaryText.Count - 1)]);
                return text;
            }
            public int MaxCount
            {
                get 
                {
                    return Math.Max(primaryText.Count, Math.Max(secondaryText.Count, tertiaryText.Count));
                }
            }
        }
        internal class ImageFactory
        {
            /// This internal class is responsible for creating images for slides

            internal int paddingPixels = 35;
            internal int maxInsideWidth
            {
                get { return DisplayEngine.NativeResolution.Size.Width - paddingPixels * 2; }
            }
            private StringFormat GetStringFormat()
            {
                StringFormat sf = (StringFormat)StringFormat.GenericTypographic.Clone();
                sf.Alignment = StringAlignment.Near;
                sf.LineAlignment = StringAlignment.Near;
                sf.FormatFlags = StringFormatFlags.FitBlackBox | StringFormatFlags.NoClip | StringFormatFlags.MeasureTrailingSpaces;
                return sf;
            }
            private RectangleF InternalMeasureString(string text, PresenterFont font, int width, StringFormat sf)
            {
                GraphicsPath pathMeasure = new GraphicsPath();
                pathMeasure.AddString(text, font.FontFamily, (int)font.FontStyle, font.SizeInPoints, new Rectangle(0, 0, width, 1), sf);
                return pathMeasure.GetBounds();
            }
            private void SetRefFont(PresenterFont font)
            {
                font.Italic = true;
                font.SizeInPoints = (int)(font.SizeInPoints * 0.9);
                font.VerticalAlignment = VerticalAlignment.Top;
                font.HorizontalAlignment = HorizontalAlignment.Right;
            }

            public bool PrepSlideMultiTranslation(GfxContext ctx, VerseBreakDown data, int subIndex, PresenterFont font)
            {
                // If only one version is available then fall back
                if (data.bibleVerse.RefVersion == "" || data.bibleVerse.SecondaryVersion == "" 
                    || data.bibleVerse.Text == "" || data.bibleVerse.SecondaryText == "")
                    return false;

                // Format data

                List<string> text = data.LinesForSlide(subIndex);
                List<string> refs = data.bibleVerse.ReferenceList();
                if (text.Count != refs.Count)
                    Debug.WriteLine("Number of refs and text lines don't match");
                Size nativeSize = DisplayEngine.NativeResolution.Size;

                #region Measure
                StringFormat sf = GetStringFormat();
                int insideHeight = nativeSize.Height - paddingPixels * 2; // subtract outside padding
                int insideWidth = nativeSize.Width - paddingPixels * 2;
                //Point anchorTop = new Point(paddingPixels, paddingPixels);
                //Point anchorBottom = new Point(paddingPixels, paddingPixels + (int)((double)insideHeight / 2));

                // Measure the reference blocks
                int refemSize = (int)(font.SizeInPoints * .9); // actual drawing size is smaller than usual
                int refBlockHeight = (int)(font.SizeInPoints * 1.2);

                List<RectangleF> rectangles = new List<RectangleF>();
                for (int ix = 0; ix < text.Count; ix++)
                {
                    double yOffset = paddingPixels + (ix * (double)insideHeight / text.Count);
                    rectangles.Add(new RectangleF(paddingPixels, (int)yOffset, insideWidth,
                        InternalMeasureString(text[ix], font, insideWidth, sf).Height));
                }

                // Validate blocks fit
                float totalHeight = rectangles.Sum(r => r.Height + refBlockHeight);
                if (totalHeight > insideHeight)
                    System.Diagnostics.Debug.WriteLine("Failed to fit layout of " + data.ToString());
                #endregion

                #region Build context
                ctx.destSize = DisplayEngine.NativeResolution.Size;
                ctx.textRegions.Clear();

                int sectionCount = Math.Min(text.Count, refs.Count);
                for (int section = 0; section < sectionCount; section++)
                {
                    GfxTextRegion rVerse1 = new GfxTextRegion();
                    rVerse1.font = font;
                    rVerse1.font.HorizontalAlignment = HorizontalAlignment.Left;
                    rVerse1.font.VerticalAlignment = VerticalAlignment.Top;
                    rVerse1.message = text[section];
                    rVerse1.bounds = rectangles[section];
                    rVerse1.bounds.Height += refBlockHeight; // Some slack in case the measurement fell short
                    ctx.textRegions.Add(rVerse1);

                    GfxTextRegion rRef1 = new GfxTextRegion();
                    rRef1.font = (PresenterFont)font.Clone();
                    SetRefFont(rRef1.font);
                    rRef1.message = refs[section];
                    rRef1.bounds = rectangles[section];
                    // Position ref after text with some room to avoid collision
                    rRef1.bounds.Y += rRef1.bounds.Height + refemSize;
                    rRef1.bounds.Height = refBlockHeight;
                    ctx.textRegions.Add(rRef1);
                }
                // PENDING: Handle asymetric block sizes

                /* OLD
                // Adjust bounds
                int standardMax = (int)((double)insideHeight / 2);
                if (r1.Height + refBlockHeight > standardMax || r2.Height + refBlockHeight > standardMax)
                {
                    rVerse1.bounds = r1; // First part
                    rVerse1.bounds.Height += refBlockHeight; // give some slack
                    r1.Y += r1.Height; // First reference
                    rRef1.bounds = r1;
                    rRef1.bounds.Height = refBlockHeight;
                    r1.Y += refBlockHeight; // Second part
                    rVerse2.bounds = r1;
                    rVerse2.bounds.Height += refBlockHeight; // give some slack
                    r1.Y += r2.Height + refBlockHeight; // Second reference
                    rRef2.bounds = r1;
                    rRef2.bounds.Height = refBlockHeight;
                }
                else
                {
                    rVerse1.bounds = r1; // First part
                    rVerse1.bounds.Height += refBlockHeight; // give some slack
                    r1.Y += r1.Height + refBlockHeight; // First reference
                    rRef1.bounds = r1;
                    rRef1.bounds.Height = refBlockHeight;
                    r1.Y += refBlockHeight; // Second part
                    rVerse2.bounds = r2;
                    rVerse2.bounds.Height += refBlockHeight; // give some slack
                    r2.Y += r2.Height + refBlockHeight; // Second reference
                    rRef2.bounds = r2;
                    rRef2.bounds.Height = refBlockHeight;
                }
                */
                #endregion

                return true;
            }
            public bool PrepSlideDoubleVerse(GfxContext ctx, Dictionary<int, BibleVerse> bibVerses, int currentVerseNum, PresenterFont font)
            {
                #region Format data
                BibleVerse bva = bibVerses[currentVerseNum];
                BibleVerse bvb = bibVerses[currentVerseNum + 1];
                string firstText = bva.RefVerse + ". " + bva.Text;
                string secondText = bvb.RefVerse + ". " + bvb.Text;
                string reference = bva.ToString() + "-" + bvb.RefVerse;
                #endregion

                Size nativeSize = DisplayEngine.NativeResolution.Size;

                #region Measure
                StringFormat sf = GetStringFormat();
                int insideHeight = nativeSize.Height - paddingPixels * 2;
                int insideWidth = nativeSize.Width - paddingPixels * 2;
                Point anchorTop = new Point(paddingPixels, paddingPixels);
                Point anchorBottom = new Point(paddingPixels, paddingPixels + (int)((double)insideHeight / 2));

                // Measure the reference blocks
                int refemSize = (int)(font.SizeInPoints * .9); // actual drawing size is smaller than usual
                int refBlockHeight = (int)(font.SizeInPoints * 1.2);

                // Determine top of rectangle
                // Measure both strings
                RectangleF r = new RectangleF(paddingPixels, 0, insideWidth,
                    InternalMeasureString(firstText, font, insideWidth, sf).Height);
                int h1 = (int)r.Height; // Get the size of the verse so we can fix the reference at the bottom
                int h2 = (int)InternalMeasureString(secondText, font, insideWidth, sf).Height;
                int offsetY = (int)(((double)insideHeight - h1 - h2 - refBlockHeight) / 2);
                if (font.VerticalAlignment == VerticalAlignment.Top)
                    r.Y = paddingPixels;
                else if (font.VerticalAlignment == VerticalAlignment.Middle)
                    r.Y = offsetY;
                else
                    r.Y = nativeSize.Height - paddingPixels * 2 - h1 - h2 - refBlockHeight;

                // Size check
                int standardMax = (int)((double)insideHeight / 2);
                if (h1 + h2 + refBlockHeight > standardMax)
                    return false;
                #endregion

                #region Build context

                ctx.destSize = DisplayEngine.NativeResolution.Size;
                ctx.textRegions.Clear();

                // Draw the first part
                GfxTextRegion rVerse = new GfxTextRegion();
                ctx.textRegions.Add(rVerse);
                rVerse.font = font;
                rVerse.font.HorizontalAlignment = HorizontalAlignment.Left;
                rVerse.message = firstText;
                rVerse.bounds = r;

                GfxTextRegion rVerse2 = new GfxTextRegion();
                ctx.textRegions.Add(rVerse2);
                rVerse2.font = font;
                rVerse2.message = secondText;
                r.Y += h1 + refemSize;
                r.Height = h2;
                rVerse2.bounds = r;

                // Reference
                GfxTextRegion rRef = new GfxTextRegion();
                ctx.textRegions.Add(rRef);
                rRef.font = (PresenterFont)font.Clone();
                SetRefFont(rRef.font);
                rRef.message = reference;
                r.Y += h2 + refBlockHeight - refemSize; // Move to the bottom of the rectangle
                rRef.bounds = r;
                rRef.bounds.Height = refBlockHeight;

                #endregion

                return true;
            }
            public void PrepSlideSingleVerse(GfxContext ctx, VerseBreakDown data, int subIndex, PresenterFont font)
            {
                #region Format data
                string txt;
                string reference;
                if (data.bibleVerse.RefVersion != "")
                {
                    reference = data.bibleVerse.ToString();
                    txt = data.primaryText[subIndex];
                }
                else
                {
                    reference = data.bibleVerse.ReferenceForTranslation(2);
                    txt = data.secondaryText[subIndex];
                }
                #endregion

                Size nativeSize = DisplayEngine.NativeResolution.Size;

                #region Measure
                StringFormat sf = GetStringFormat();
                int insideHeight = nativeSize.Height - paddingPixels * 2;
                int insideWidth = nativeSize.Width - paddingPixels * 2;
                Point anchorTop = new Point(paddingPixels, paddingPixels);
                Point anchorBottom = new Point(paddingPixels, paddingPixels + (int)((double)insideHeight / 2));

                // Measure the reference blocks
                int refemSize = (int)(font.SizeInPoints * .9); // actual drawing size is smaller than usual
                int refBlockHeight = (int)(font.SizeInPoints * 1.2);
                #endregion

                #region Build context

                ctx.destSize = DisplayEngine.NativeResolution.Size;
                ctx.textRegions.Clear();

                // Primary text
                GfxTextRegion rVerse = new GfxTextRegion();
                ctx.textRegions.Add(rVerse);
                rVerse.font = font;
                rVerse.message = txt;
                
                RectangleF r1 = new RectangleF(paddingPixels, 0, insideWidth,
                    InternalMeasureString(txt, font, insideWidth, sf).Height);
                if (font.VerticalStringAlignment == StringAlignment.Near)
                    r1.Y = paddingPixels;
                else if (font.VerticalStringAlignment == StringAlignment.Center)
                    r1.Y = (float)(((double)insideHeight - r1.Height) / 2) + paddingPixels;
                else
                    r1.Y = nativeSize.Height - r1.Height - refBlockHeight - paddingPixels*2;
                rVerse.bounds = r1;
                rVerse.bounds.Height += refBlockHeight; // give some slack

                // Reference
                GfxTextRegion rRef = new GfxTextRegion();
                ctx.textRegions.Add(rRef);
                rRef.font = (PresenterFont)font.Clone();
                SetRefFont(rRef.font);
                rRef.message = reference;

                r1.Y += r1.Height + refBlockHeight; // +refemSize; // relocate the box
                rRef.bounds = r1;
                rRef.bounds.Height = refBlockHeight;

                #endregion
            }
            public void PrepFontFit(GfxContext ctx, Dictionary<int, BibleVerse> verses, int startVerse, PresenterFont font)
            {
                Size nativeSize = DisplayEngine.NativeResolution.Size;

                //////////////////////////////////////////////////////////////////
                // Format data
                BibleVerse bv1 = verses[1];
                string bookname = Program.BibleDS.BibleLookUp.FindByVersionIdMappingBook(bv1.RefVersion, bv1.RefBook).DisplayBook;
                string title = bookname + " " + bv1.RefChapter;
                string data = "";
                for(int i = startVerse; i <= verses.Count; i++)
                    data += verses[i].RefVerse + ". " + verses[i].Text + "\r\n";
                                
                //////////////////////////////////////////////////////////////////
                // Measure
                StringFormat sf = GetStringFormat();
                int insideHeight = nativeSize.Height - paddingPixels * 2;
                int insideWidth = nativeSize.Width - paddingPixels * 2;
                int titleHeight = font.SizeInPoints * 2;
                RectangleF rTitle = new RectangleF(paddingPixels, paddingPixels, insideWidth, titleHeight);
                RectangleF rText = new RectangleF(paddingPixels, paddingPixels + titleHeight,
                    insideWidth, insideHeight - titleHeight);

                //////////////////////////////////////////////////////////////////
                // Build context
                ctx.destSize = DisplayEngine.NativeResolution.Size;
                ctx.textRegions.Clear();

                // Title text
                GfxTextRegion trTitle = new GfxTextRegion();
                ctx.textRegions.Add(trTitle);
                trTitle.font = (PresenterFont)font.Clone();
                trTitle.font.SizeInPoints = (int)(trTitle.font.SizeInPoints * 1.5);
                trTitle.message = title;
                trTitle.bounds = rTitle;

                // Data
                GfxTextRegion trData = new GfxTextRegion();
                ctx.textRegions.Add(trData);
                trData.font = (PresenterFont)font.Clone();
                trData.font.VerticalAlignment = VerticalAlignment.Top;
                trData.font.HorizontalAlignment = HorizontalAlignment.Left;
                trData.fontClip = true;
                trData.message = data;
                trData.bounds = rText;
            }
        }
    }
}
