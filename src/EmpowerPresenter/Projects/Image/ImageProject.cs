/* ePresenter is licensed under the GPLV3 -- see the 'COPYING' file details.
   Copyright (C) 2006 Alex Korchemniy */
using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace EmpowerPresenter
{
	public class ImageProject : IProject, ISlideShow, IKeyClient, ISupportGfxCtx
	{
		private string name = "Picture slideshow";
		internal List<string> names = new List<string>(); // used for sorted indexable list 
		internal Dictionary<string, string> files = new Dictionary<string, string>(); // Support for temporary items
		internal List<string> tempFiles = new List<string>();
		private ImageDisplayStyle style = ImageDisplayStyle.SmartFit;
		internal int currentIndex = 0;

		// Graphics
		private string cachedName = "";
		private Image imgCached;

		////////////////////////////////////////////////////////////
		public ImageProject()
		{
		}
		public void AddFiles(string[] f)
		{
			string currentName = "";
			if (currentIndex == -1 && names.Count > 0)
				currentIndex = 0;
			if (names.Count != 0)
				currentName = names[currentIndex];

			// Add all the files
			foreach (string s in f)
			{
				if (File.Exists(s))
				{
					string n = Path.GetFileNameWithoutExtension(s);
					n = GetUniqueName(n);
					files.Add(n, s);
					names.Add(n);
				}
			}
			names.Sort();

			// Preserve current image (calc index and shift it back)
			if (currentName != "")
			{
				if (!names.Contains(currentName))
					currentIndex = 0;
				else
					currentIndex = names.IndexOf(currentName);
			}

			// Bring this project to front in view (not display)
			Program.Presenter.ActivateController(this);

			RefreshUI();
		}
		public void AddImage(string name, Image i)
		{
			string currentName = "";
			if (currentIndex == -1 && names.Count > 0)
				currentIndex = 0;
			if (names.Count != 0)
				currentName = names[currentIndex];

			// Save the image to a temporary file and add to files list
			string fn = Path.GetTempFileName();
			i.Save(fn);

			tempFiles.Add(fn);
			name = GetUniqueName(name);
			files.Add(fn, name);
			names.Add(name);
			names.Sort();

			// Preserve current image (calc index and shift it back)
			if (currentName != "")
			{
				if (!names.Contains(currentName))
					currentIndex = 0;
				else
					currentIndex = names.IndexOf(currentName);
			}

			// Bring this project to front in view (not display)
			Program.Presenter.ActivateController(this);

			RefreshUI();
		}
		public void RemoveImage(string name)
		{
			// Remove from names
			string filename = files[name];
			files.Remove(name);
			names.Remove(name);

			// Remove the file if it is temporary
			if (tempFiles.Contains(filename))
			{
				tempFiles.Remove(filename);
				try
				{
					File.Delete(filename);
				}catch {}				
			}
			
			// Check index
			if (currentIndex >= names.Count)
				currentIndex = names.Count - 1;

			RefreshUI();
		}
		private string GetUniqueName(string n)
		{
			if (files.ContainsKey(n)) // Get a unique name
			{
				for (int i = 1; i < 300; i++)
				{
					if (!files.ContainsKey(n + " - " + i.ToString()))
					{
						n = n + " - " + i.ToString();
						break;
					}
				}
			}
			return n;
		}

		public void RefreshUI()
		{
			if (currentIndex == -1 && names.Count > 0)
				currentIndex = 0;

			// Clear the cache only if name is different or no images left
			if (names.Count == 0 || names[currentIndex] != cachedName) 
				cachedName = "";

			if (Refresh != null)
				Refresh(this, null);
		}
		public Image GetCurrentImage()
		{
			if (cachedName == "")
			{
				if (imgCached != null)
					imgCached.Dispose(); // Save some memory (all subscribes should not have a reference to this image any more)

				if (names.Count > 0)
				{
					try
					{
						if (currentIndex == -1) // Safegaurd
							return null;

						string name = names[currentIndex];
						string path = files[name];

						Size currentSize = DisplayEngine.NativeResolution.Size;
						Bitmap b = new Bitmap(currentSize.Width, currentSize.Height);
						Graphics g = Graphics.FromImage(b);
						Image original = Image.FromFile(path);
						switch (style)
						{
							case ImageDisplayStyle.Center:
								{
									int x = (int)((currentSize.Width - original.Width) / (double)2);
									int y = (int)((currentSize.Height - original.Height) / (double)2);
									g.DrawImage(original, new Rectangle(x, y, original.Width, original.Height));
									break;
								}
							case ImageDisplayStyle.SmartFit:
								{
									double scaleFactorW = (double)currentSize.Width / original.Width;
									if (currentSize.Height - (int)(original.Height * scaleFactorW) >= 0) // Try first by width
									{
										int destHeight = (int)(original.Height * scaleFactorW);
										int cy = (int)((currentSize.Height - destHeight) / 2);
										g.DrawImage(original, new Rectangle(0, cy, currentSize.Width, destHeight),
											new Rectangle(0, 0, original.Width, original.Height), GraphicsUnit.Pixel);
									}
									else // Paint by height
									{
										double scaleFactorH = (double)currentSize.Height / original.Height;
										int destWidth = (int)(original.Width * scaleFactorH);
										int cx = (int)((currentSize.Width - destWidth) / 2);
										g.DrawImage(original, new Rectangle(cx, 0, destWidth, currentSize.Height),
											new Rectangle(0, 0, original.Width, original.Height), GraphicsUnit.Pixel);
									}
								}
								break;
							case ImageDisplayStyle.Stretch:
								g.DrawImage(original, 0, 0, currentSize.Width, currentSize.Height);
								break;
						}
						g.Dispose();
						imgCached = b;

						cachedName = name;
					}
					catch(Exception ex)
					{
						System.Diagnostics.Trace.WriteLine(ex.ToString());
						cachedName = "";
					}
				}
				else
					return null;
			}
			return imgCached;
		}

		public ImageDisplayStyle Style
		{
			get { return style; }
			set
			{
				if (value != style)
				{
					style = value;
					cachedName = ""; // clear the cache
				}
			}
		}

		#region IProject Members

		public event EventHandler Refresh;
		public event EventHandler RequestActivate;
		public event EventHandler RequestDeactivate;

		public string GetName() { return name; }
		public ProjectType GetProjectType() { return ProjectType.Image; }
		public void Activate()
		{
			Program.Presenter.ActivateKeyClient(this);
			if (RequestActivate != null)
				RequestActivate(this, null);
		}
		public void CloseProject()
		{
			// Clear all the temporary files
			foreach (string s in tempFiles)
			{
				if (File.Exists(s))
				{
					try {File.Delete(s);}catch { }	
				}
			}

			if (RequestDeactivate != null)
				RequestDeactivate(this, null);
		}
		public Type GetControllerUIType() { return typeof(ImageProjectView); }
		public Type GetDisplayerType() { return typeof(DisplayEngine); }
		public bool IsPrimaryDisplay() { return true; }

		#endregion

		#region ISlideShow Members

		public void GotoSlide(int x)
		{
			GotoSlide(x, false);
		}
		public void GotoSlide(int x, bool forceRefresh)
		{
			if (currentIndex != x || forceRefresh)
			{
				currentIndex = x;
				RefreshUI(); // This method will trigger all controllers to refresh

				// Keys
				// TODO: register for keys
			}
		}
		public void GoNextSlide()
		{
			if (currentIndex < names.Count - 1)
				GotoSlide(currentIndex + 1);
		}
		public void GoPrevSlide()
		{
			if (currentIndex > 0)
				GotoSlide(currentIndex - 1);
		}

		#endregion

		#region IKeyClient Members

		public void ProccesKeys(Keys k, bool exOwer)
		{
			if (exOwer)
				return;

			if (k == Keys.Right || k == Keys.Down || k == Keys.PageDown || k == Keys.Space || k == Keys.Enter)
				GoNextSlide();
			else if (k == Keys.Left || k == Keys.Up || k == Keys.PageUp || k == Keys.Back)
				GoPrevSlide();
		}

		#endregion

		#region ISupportGfxCtx Members

		public GfxContext GetCurrentGfxContext()
		{
			GfxContext ctx = new GfxContext();
			ctx.destSize = DisplayEngine.NativeResolution.Size;
			ctx.img = GetCurrentImage();
			ctx.opacity = 0;
			return ctx;
		}

		#endregion
	}
	public enum ImageDisplayStyle
	{
		Center, SmartFit, Stretch
	}
}
