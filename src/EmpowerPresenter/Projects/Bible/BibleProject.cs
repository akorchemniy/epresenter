/* ePresenter is licensed under the GPLV3 -- see the 'COPYING' file details.
   Copyright (C) 2006 Alex Korchemniy */
using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Data.OleDb;

using EmpowerPresenter.Data;
using System.ComponentModel;
using System.Windows.Forms;
using FirebirdSql.Data.FirebirdClient;

namespace EmpowerPresenter
{
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
		private void LoadBibDat(BibleVerse verse)
		{
			currentVerseNum = verse.RefVerse;
			string ver1 = Program.ConfigHelper.BiblePrimaryTranslation;
			string ver2 = Program.ConfigHelper.BibleSecondaryTranslation;
			bool useMapping = (ver2 != "" && (ver1 == "KJV" || ver2 == "KJV")); // mapping is enabled when there is a secondary translation and at least one translation is english KJV
			
			bibVerses.Clear();

            using (FBirdTask t = new FBirdTask())
			{
				if (useMapping)
				{
					t.CommandText = "SELECT PRI.REFVERSE, PRI.BOOK, PRI.CHAPTER, PRI.VERSE, SEC.DATA, PRI.DATA " +
					"FROM (BIBLEVERSES AS PRI LEFT JOIN BIBLEVERSES AS SEC " +
					"ON PRI.BOOK = SEC.REFBOOK AND PRI.CHAPTER = SEC.REFCHAPTER AND PRI.VERSE = SEC.REFVERSE) " +
					"WHERE PRI.VERSION = @Version AND SEC.VERSION = @SecondVersion AND PRI.REFBOOK = @Book AND PRI.REFCHAPTER = @Chapter " + 
					"ORDER BY PRI.REFBOOK, PRI.REFCHAPTER, PRI.REFVERSE, PRI.VERSE";
                    t.Command.Parameters.Add("@Version", FbDbType.VarChar, 10).Value = ver1;
                    t.Command.Parameters.Add("@SecondVersion", FbDbType.VarChar, 10).Value = ver2;
                    t.Command.Parameters.Add("@Book", FbDbType.VarChar, 50).Value = verse.RefBook;
                    t.Command.Parameters.Add("@Chapter", FbDbType.Integer).Value = verse.RefChapter;
					t.ExecuteReader();

					BibleVerse bVerse;
					while (t.DR.Read())
					{
						bVerse = new BibleVerse();
						bVerse.RefVersion = ver1;
						bVerse.RefBook = verse.RefBook;
						bVerse.RefChapter = verse.RefChapter;
						bVerse.RefVerse = t.GetInt32(0);
						bVerse.SecondaryVersion = ver2;
						bVerse.SecondaryBook = t.GetString(1);
						bVerse.SecondaryChapter = t.GetInt32(2);
						bVerse.SecondaryVerse = t.GetInt32(3);
						bVerse.SecondaryText = t.GetString(4);
						bVerse.Text = t.GetString(5);
						bibVerses.Add(bVerse.RefVerse, bVerse);
					}
				}
				else if (ver2 != "") // RST & UK
				{
					t.CommandText =
						"SELECT PRI.REFVERSE, SEC.REFVERSE, PRI.DATA, SEC.DATA " +
						"FROM (BIBLEVERSES AS PRI JOIN BIBLEVERSES AS SEC " +
						"ON PRI.REFBOOK = SEC.REFBOOK AND PRI.CHAPTER = SEC.REFCHAPTER AND PRI.VERSE = SEC.REFVERSE) " +
						"WHERE PRI.VERSION = @Version AND SEC.VERSION = @SecondVersion AND PRI.REFBOOK = @Book AND PRI.REFCHAPTER = @Chapter " +
						"ORDER BY PRI.REFBOOK, PRI.REFCHAPTER, PRI.REFVERSE, PRI.VERSE";
                    t.Command.Parameters.Add("@Version", FbDbType.VarChar, 10).Value = ver1;
                    t.Command.Parameters.Add("@SecondVersion", FbDbType.VarChar, 10).Value = ver2;
                    t.Command.Parameters.Add("@Book", FbDbType.VarChar, 50).Value = verse.RefBook;
                    t.Command.Parameters.Add("@Chapter", FbDbType.Integer).Value = verse.RefChapter;
					t.ExecuteReader();

					BibleVerse bVerse;
					while (t.DR.Read())
					{
						bVerse = new BibleVerse();
						bVerse.RefVersion = ver1;
						bVerse.RefBook = verse.RefBook;
						bVerse.RefChapter = verse.RefChapter;
						bVerse.RefVerse = t.GetInt32(0);
						bVerse.SecondaryVersion = ver2;
						bVerse.SecondaryBook = verse.RefBook;
						bVerse.SecondaryChapter = verse.RefChapter;
						bVerse.SecondaryVerse = t.GetInt32(1);
						bVerse.SecondaryText = t.GetString(3);
						bVerse.Text = t.GetString(2);
						bibVerses.Add(bVerse.RefVerse, bVerse);
					}
				}
				else // One version
				{
					t.CommandText = "SELECT RefVerse, Data FROM BibleVerses " +
					"WHERE Version = @Version AND RefBook = @Book AND RefChapter = @Chapter " +
					"ORDER BY RefBook, RefChapter, RefVerse";
                    t.Command.Parameters.Add("@Version", FbDbType.VarChar, 10).Value = ver1;
                    t.Command.Parameters.Add("@Book", FbDbType.VarChar, 50).Value = verse.RefBook;
                    t.Command.Parameters.Add("@Chapter", FbDbType.Integer).Value = verse.RefChapter;
					t.ExecuteReader();

					BibleVerse bVerse;
					while (t.DR.Read())
					{
						bVerse = new BibleVerse();
						bVerse.RefVersion = ver1;
						bVerse.RefBook = verse.RefBook;
						bVerse.RefChapter = verse.RefChapter;
						bVerse.RefVerse = t.GetInt32(0);
						bVerse.Text = t.GetString(1);
						bibVerses.Add(bVerse.RefVerse, bVerse);
					}
				}
			}
		}
		public void RefreshData()
		{
			if (bibVerses.Count == 0) // Dont know where to start from
				return;

			// Check if bible version changed & reload data if necessary
			if (bibVerses[currentVerseNum].RefVersion != Program.ConfigHelper.BiblePrimaryTranslation ||
				bibVerses[currentVerseNum].SecondaryVersion != Program.ConfigHelper.BibleSecondaryTranslation)
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
			string ver1 = Program.ConfigHelper.BiblePrimaryTranslation;
			string ver2 = Program.ConfigHelper.BibleSecondaryTranslation;
			return (ver2 != "" && ver1 != ver2);
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
				// Calculate max block size for multi
				Size nativeSize = DisplayEngine.NativeResolution.Size;
				int maxh; Size maxSize;
				if (IsMultiTrans())
				{
					double insideHeight = nativeSize.Height - imageFactory.paddingPixels * 3;
					insideHeight /= 2;
					insideHeight -= bibFont.SizeInPoints * 1.2 * 2;
					maxh = (int)insideHeight;
					maxSize = new Size(imageFactory.maxInsideWidth, maxh);
				}
				else
				{
					maxh = (int)(((double)nativeSize.Height - imageFactory.paddingPixels * 2.5));
					maxSize = new Size(imageFactory.maxInsideWidth, maxh);
				}

				VerseBreakDown d = new VerseBreakDown();
				d.bibleVerse = bvCurrent;
				d.primaryText = InternalBreakString(bvCurrent.RefVerse, bvCurrent.Text, maxSize);
				d.secondaryText = InternalBreakString(bvCurrent.SecondaryVerse, bvCurrent.SecondaryText, maxSize);
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
				return null;

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

			public int MaxCount
			{
				get 
				{
					if (secondaryText == null)
						return primaryText.Count;
					if (primaryText == null)
						return secondaryText.Count;
					return primaryText.Count > secondaryText.Count ? primaryText.Count : secondaryText.Count; 
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
				string primaryText = subIndex > data.primaryText.Count - 1 ? data.primaryText[data.primaryText.Count - 1] : data.primaryText[subIndex];
				string secondaryText = subIndex > data.secondaryText.Count - 1 ? data.secondaryText[data.secondaryText.Count - 1] : data.secondaryText[subIndex];
				string priRef = data.bibleVerse.ToString();
				string secRef = data.bibleVerse.ToString(true);

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

				// Measure both strings
				RectangleF r1 = new RectangleF(anchorTop.X, anchorTop.Y, insideWidth,
					InternalMeasureString(primaryText, font, insideWidth, sf).Height);
				RectangleF r2 = new RectangleF(anchorBottom.X, anchorBottom.Y, insideWidth,
					InternalMeasureString(secondaryText, font, insideWidth, sf).Height);

				if (r1.Height + r2.Height + refBlockHeight * 2 > insideHeight)
					return false;

				#endregion

				#region Build context
				ctx.destSize = DisplayEngine.NativeResolution.Size;
				ctx.textRegions.Clear();

				// First part
				GfxTextRegion rVerse1 = new GfxTextRegion();
				ctx.textRegions.Add(rVerse1);
				rVerse1.font = font;
				rVerse1.font.HorizontalAlignment = HorizontalAlignment.Left;
				rVerse1.font.VerticalAlignment = VerticalAlignment.Top;
				rVerse1.message = primaryText;

				// First reference
				GfxTextRegion rRef1 = new GfxTextRegion();
				ctx.textRegions.Add(rRef1);
				rRef1.font = (PresenterFont)font.Clone();
				SetRefFont(rRef1.font);
				rRef1.message = priRef;

				// Second part
				GfxTextRegion rVerse2 = new GfxTextRegion();
				ctx.textRegions.Add(rVerse2);
				rVerse2.font = font;
				rVerse2.font.HorizontalAlignment = HorizontalAlignment.Left;
				rVerse2.font.VerticalAlignment = VerticalAlignment.Top;
				rVerse2.message = secondaryText;

				// Second reference
				GfxTextRegion rRef2 = new GfxTextRegion();
				ctx.textRegions.Add(rRef2);
				rRef2.font = (PresenterFont)font.Clone();
				SetRefFont(rRef2.font);
				rRef2.message = secRef;

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
					reference = data.bibleVerse.ToString(true);
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
