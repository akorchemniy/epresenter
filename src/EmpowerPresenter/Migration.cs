using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Security.Cryptography;
using FirebirdSql.Data.FirebirdClient;
using System.Data.OleDb;

namespace EmpowerPresenter
{
	public partial class Migration : Form
	{
		private BackgroundWorker bw = new BackgroundWorker();

		////////////////////////////////////////////////////////////////////////
		public Migration()
		{
			#if DEMO
			new DemoVersionOnly("Migration").ShowDialog();
			#else
			InitializeComponent();
			#endif	
		}

		private void btnStart_Click(object sender, EventArgs e)
		{
			if (btnStart.Text == "Finish")
			{
				Environment.Exit(0);
				return;
			}
			else
			{
				GC.AddMemoryPressure(20000);
				bw.DoWork += new DoWorkEventHandler(bw_DoWork);
				bw.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bw_RunWorkerCompleted);
				bw.RunWorkerAsync();

				lblPleaseWait.Visible = true;
				noProgressBar1.Visible = true;
				btnExit.Enabled = false;
				btnStart.Enabled = false;
			}
		}
		private void bw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
			lblPleaseWait.Text = "Migration finished. ePresenter will be restarted.";
			noProgressBar1.Visible = false;
			btnStart.Text = "Finish";
			btnExit.Visible = false;
			btnStart.Enabled = true;
		}
		private void bw_DoWork(object sender, DoWorkEventArgs e)
		{
#if !DEMO
			// Conditional execution for debugging
			bool bPhotos = true;
			bool bVerses = true;
			bool bWriteVerses = true;
			bool bSongs = true;
			bool bWriteSongs = true;

			// Locate the directory where previous version is installed
			string root = Path.GetPathRoot(Application.ExecutablePath);
			string progFilesRoot = Path.Combine(root, "Program Files");
			string vendisoftRoot = Path.Combine(progFilesRoot, "Vendisoft");
			string oldRoot = Path.Combine(vendisoftRoot, "ePresenter");
			if (!new DirectoryInfo(oldRoot).Exists)
			{
				this.TopMost = false;
				MessageBox.Show(this, "The migration wizard was unable to locate the previous installation of ePresenter. Contact Vendisoft for technical support.", "Error");
				return;
			}

			// Remove images and index from the current directory & copy from previous version
			if (bPhotos)
				if (!MigratePhotos(Path.GetDirectoryName(Application.ExecutablePath), oldRoot))
					return;

			// Migrate song verse rows
			if (bVerses)
			{
				Dictionary<string, MigrationSongVerse> mVerses = new Dictionary<string, MigrationSongVerse>();
				if (!GetDifSongVerses(oldRoot, mVerses))
					return;
				LogMigrationChangesVerses(mVerses);
				if (bWriteVerses)
					AddInsertSongVerses(mVerses);
				mVerses.Clear();
			}

			// Migrate song rows
			if (bSongs)
			{
				Dictionary<int, MigrationSong> mSongs = new Dictionary<int, MigrationSong>();
				if (!GetDifSongs(oldRoot, mSongs))
					return;
				LogMigrationChangesSongs(mSongs);
				if (bWriteSongs)
					AddInsertSongs(mSongs);
				mSongs.Clear();
			}

			// Set flag for completion
			Program.Configuration.SetSetting("MigrationCompleted", "True");
			Program.Configuration.Save();
#endif
		}

		private bool MigratePhotos(string root, string oldRoot)
		{
			// Original photo index
			string photoCategories = Path.Combine(root, "Photocategories.xml");
			string photoCategoriesOriginal = Path.Combine(oldRoot, "Photocategories.xml");
			try
			{
				if (File.Exists(photoCategoriesOriginal))
				{
					File.Delete(photoCategories);
					File.Copy(photoCategoriesOriginal, photoCategories);
				}
				else
				{
					this.TopMost = false;
					MessageBox.Show(this, "Necessary files not located in previous installation location.", "Error");
					return false;
				}
			} catch (Exception ex) { System.Diagnostics.Trace.WriteLine(ex.ToString()); }

			// Original full size images
			string full = Path.Combine(root, "fullsize");
			string fullOriginal = Path.Combine(oldRoot, "fullsize");
			try
			{
				if (Directory.Exists(fullOriginal))
				{
					List<FileInfo> files2delete = new List<FileInfo>();
					foreach (FileInfo fi in new DirectoryInfo(full).GetFiles())
						files2delete.Add(fi);
					foreach (FileInfo fi in files2delete)
						fi.Delete();
					foreach (FileInfo fi in new DirectoryInfo(fullOriginal).GetFiles())
						fi.CopyTo(Path.Combine(full, fi.Name));
				}
			} catch (Exception ex) { System.Diagnostics.Trace.WriteLine(ex.ToString()); }

			// Original preview size images
			string previews = Path.Combine(root, "previews");
			string previewsOriginal = Path.Combine(oldRoot, "previews");
			try
			{
				if (Directory.Exists(previewsOriginal))
				{
					List<FileInfo> files2delete = new List<FileInfo>();
					foreach (FileInfo fi in new DirectoryInfo(previews).GetFiles())
						files2delete.Add(fi);
					foreach (FileInfo fi in files2delete)
						fi.Delete();
					foreach (FileInfo fi in new DirectoryInfo(previewsOriginal).GetFiles())
						fi.CopyTo(Path.Combine(previews, fi.Name));
				}
			} catch (Exception ex) { System.Diagnostics.Trace.WriteLine(ex.ToString()); }

			return true;
		}
		private bool GetDifSongVerses(string oldRoot, Dictionary<string, MigrationSongVerse> mVerses)
		{
			// Approach: 
			//  - open two database connection and send identical sorted queries
			//  - add each result to the list
			//  - if two results have no changes then drop them from the list
			//  - if there are changes mark what kind
			//  - after queries are finished execute add / update queries

			using (FBirdTask told = new FBirdTask())
            using (FBirdTask tnew = new FBirdTask())
			{
				// Set old connection string
				string path2olddb = Path.Combine(oldRoot, "bibdata.fdb");
				string oldConstring = "ServerType=1;Database=" + path2olddb + ";User=SYSDBA;Password=masterkey;Charset=WIN1251";
				told.Connection = new FbConnection(oldConstring);

				string fbQuery = "SELECT " +
					"\"SongVerses\".\"AutoNumber\", " +
					"\"SongVerses\".\"OrderNum\", " +
					"\"SongVerses\".\"IsChorus\", " +
					"\"SongVerses\".\"Verse\" " +
					"FROM " +
					"\"SongVerses\" " +
					"ORDER BY " +
					"\"SongVerses\".\"AutoNumber\", " +
					"\"SongVerses\".\"OrderNum\"";

                string jtQuery = "SELECT " +
					"[SongVerses].[AutoNumber], " +
					"[SongVerses].[OrderNum], " +
					"[SongVerses].[IsChorus], " +
					"[SongVerses].[Verse] " +
					"FROM " +
					"[SongVerses] " +
					"ORDER BY " +
					"[SongVerses].[AutoNumber], " +
					"[SongVerses].[OrderNum]";

				told.CommandText = fbQuery;
				tnew.CommandText = jtQuery;
				told.ExecuteReader();
				tnew.ExecuteReader();
				if (told.DR == null || tnew.DR == null)
				{
					this.TopMost = false;
					MessageBox.Show(this, "Migration failed to migrate song data", "Error");
					return false;
				}

				bool oldAvailable = true;
				bool newAvailable = true;
				while (oldAvailable || newAvailable)
				{
					oldAvailable = told.DR.Read();
					newAvailable = tnew.DR.Read();

					// Old data
					if (oldAvailable)
					{
						MigrationSongVerse vo = new MigrationSongVerse();
						DrFillSongVerseObj(told.DR, vo);
						vo.foundInOld = true;

						string id = vo.GetId();
						if (mVerses.ContainsKey(id))
						{
							MigrationSongVerse v2 = mVerses[id];
							v2.foundInOld = true;

							// Drop if the two are the same
							if (v2.Compare(vo, v2))
								mVerses.Remove(id);
						}
						else
							mVerses[id] = vo;
					}

					// New data
					if (newAvailable)
					{
						MigrationSongVerse vn = new MigrationSongVerse();
						DrFillSongVerseObj(tnew.DR, vn);
						vn.foundInNew = true;

						string id = vn.GetId();
						if (mVerses.ContainsKey(id))
						{
							MigrationSongVerse v2 = mVerses[id];
							v2.foundInNew = true;

							// Drop if the two are the same
							if (v2.Compare(vn, v2))
								mVerses.Remove(id);
						}
						else
							mVerses[id] = vn;
					}
				}
			}
			return true;
		}
        private static void DrFillSongVerseObj(FbDataReader dr, MigrationSongVerse vo)
        {
            vo.autoNumber = dr.GetInt32(0);
            vo.orderNumber = dr.GetInt32(1);
            vo.isChorus = dr.GetBoolean(2);
            vo.verse = dr.GetString(3);
        }
		private static void DrFillSongVerseObj(OleDbDataReader dr, MigrationSongVerse vo)
		{
			vo.autoNumber = dr.GetInt32(0);
			vo.orderNumber = dr.GetInt32(1);
			vo.isChorus = dr.GetBoolean(2);
			vo.verse = dr.GetString(3);
		}
		private void AddInsertSongVerses(Dictionary<string, MigrationSongVerse> mVerses)
		{
			using (JetTask t = new JetTask())
			{
				t.Parameters.Add("@AutoNumber", OleDbType.Integer);
                t.Parameters.Add("@IsChorus", OleDbType.SmallInt);
                t.Parameters.Add("@Verse", OleDbType.VarWChar, 0);
                t.Parameters.Add("@OrderNum", OleDbType.Integer);

				foreach (MigrationSongVerse msv in mVerses.Values)
				{
					t.Parameters["@AutoNumber"].Value = msv.autoNumber;
					t.Parameters["@IsChorus"].Value = msv.isChorus ? 1 : 0;
					t.Parameters["@Verse"].Value = msv.verse;
					t.Parameters["@OrderNum"].Value = msv.orderNumber;

					if (!msv.foundInNew)
					{
						t.CommandText = "INSERT INTO [SongVerses] ([AutoNumber], [IsChorus], [Verse], [OrderNum]) " +
							"VALUES (@AutoNumber, @IsChorus, @Verse, @OrderNum)";
						t.ExecuteNonQuery();
					}
					else
					{
						t.CommandText = "UPDATE [SongVerses] " +
							"SET [AutoNumber] = @AutoNumber, " + 
							"[IsChorus] = @IsChorus, " + 
							"[Verse] = @Verse, " +
							"[OrderNum] = @OrderNum  " +
							"WHERE ([AutoNumber] = @AutoNumber AND [OrderNum] = @OrderNum)";
						t.ExecuteNonQuery();
					}
				}
			}
		}
		private bool GetDifSongs(string oldRoot, Dictionary<int, MigrationSong> mSongs)
		{
			using (FBirdTask told = new FBirdTask())
			using (JetTask tnew = new JetTask())
			{
				// Set old connection string
				string path2olddb = Path.Combine(oldRoot, "bibdata.fdb");
				string oldConstring = "ServerType=1;Database=" + path2olddb + ";User=SYSDBA;Password=masterkey;Charset=WIN1251";
				told.Connection = new FbConnection(oldConstring);

				string fbQuery = "SELECT  " +
					"\"Songs\".\"AutoNumber\", " +
					"\"Songs\".\"Number\", " +
					"\"Songs\".\"Title\", " +
					"\"Songs\".\"Chorus\", " +
					"\"Songs\".\"Location\", " +
					"\"Songs\".\"DisplayDefault\", " +
					"\"Songs\".\"ImageId\", " +
					"\"Songs\".\"Overlay\", " +
					"\"Songs\".\"Copyright\" " +
					"FROM \"Songs\" " +
					"ORDER BY \"AutoNumber\"";

                string jtQuery = "SELECT  " +
                    "[Songs].[AutoNumber], " +
                    "[Songs].[Number], " +
                    "[Songs].[Title], " +
                    "[Songs].[Chorus], " +
                    "[Songs].[Location], " +
                    "[Songs].[DisplayDefault], " +
                    "[Songs].[ImageId], " +
                    "[Songs].[Overlay], " +
                    "[Songs].[Copyright] " +
                    "FROM [Songs] " +
                    "ORDER BY [AutoNumber]";

				told.CommandText = fbQuery;
				tnew.CommandText = jtQuery;
				told.ExecuteReader();
				tnew.ExecuteReader();
				if (told.DR == null || tnew.DR == null)
				{
					this.TopMost = false;
					MessageBox.Show(this, "Migration failed to migrate song data", "Error");
					return false;
				}

				bool oldAvailable = true;
				bool newAvailable = true;
				while (oldAvailable || newAvailable)
				{
					oldAvailable = told.DR.Read();
					newAvailable = tnew.DR.Read();

					// Old data
					if (oldAvailable)
					{
						MigrationSong mgo = new MigrationSong();
						DrFillSongObj(told.DR, mgo);
						mgo.foundInOld = true;

						// Check if image got changed
						if (mgo.imageId == 4)
						{
							mgo.imageId = -2;
							mgo.overlay = 777;
						}

						if (mSongs.ContainsKey(mgo.autoNumber))
						{
							MigrationSong v2 = mSongs[mgo.autoNumber];
							v2.foundInOld = true;

							// Drop if the two are the same
							if (v2.Compare(mgo, v2))
								mSongs.Remove(mgo.autoNumber);
						}
						else
							mSongs[mgo.autoNumber] = mgo;
					}

					// New data
					if (newAvailable)
					{
						MigrationSong mgn = new MigrationSong();
						DrFillSongObj(tnew.DR, mgn);
						mgn.foundInNew = true;

						if (mSongs.ContainsKey(mgn.autoNumber))
						{
							MigrationSong v2 = mSongs[mgn.autoNumber];
							v2.foundInNew = true;

							// Drop if the two are the same
							if (v2.Compare(mgn, v2))
								mSongs.Remove(mgn.autoNumber);
						}
						else
							mSongs[mgn.autoNumber] = mgn; 
					}
				}
			}
			return true;
		}
        private void DrFillSongObj(FbDataReader dr, MigrationSong mgo)
        {
            mgo.autoNumber = dr.GetInt32(0);
            mgo.number = dr.GetInt32(1);
            mgo.title = dr.GetString(2);
            mgo.chorus = dr.GetString(3);
            mgo.location = dr.GetString(4);
            mgo.displayDefault = dr.GetBoolean(5);
            mgo.imageId = dr.GetInt32(6);
            mgo.overlay = dr.GetInt32(7);
            mgo.copyright = dr.GetString(8);
        }
		private void DrFillSongObj(OleDbDataReader dr, MigrationSong mgo)
		{
			mgo.autoNumber = dr.GetInt32(0);
			mgo.number = dr.GetInt32(1);
			mgo.title = dr.GetString(2);
			mgo.chorus = dr.GetString(3);
			mgo.location = dr.GetString(4);
			mgo.displayDefault = dr.GetBoolean(5);
			mgo.imageId = dr.GetInt32(6);
			mgo.overlay = dr.GetInt32(7);
			mgo.copyright = dr.GetString(8);
		}
		private void AddInsertSongs(Dictionary<int, MigrationSong> mSongs)
		{
			using (JetTask t = new JetTask())
			{
				t.Parameters.Add("@AutoNumber", OleDbType.Integer);
                t.Parameters.Add("@Number", OleDbType.Integer);
                t.Parameters.Add("@Title", OleDbType.VarWChar, 0);
                t.Parameters.Add("@Chorus", OleDbType.VarWChar, 0);
                t.Parameters.Add("@Location", OleDbType.VarWChar, 0);
                t.Parameters.Add("@DisplayDefault", OleDbType.SmallInt);
                t.Parameters.Add("@ImageId", OleDbType.Integer);
                t.Parameters.Add("@Overlay", OleDbType.Integer);
                t.Parameters.Add("@FontId", OleDbType.Integer);
                t.Parameters.Add("@Settings", OleDbType.VarWChar, 0);

				foreach (MigrationSong ms in mSongs.Values)
				{
					t.Parameters["@AutoNumber"].Value = ms.autoNumber;
					t.Parameters["@Number"].Value = ms.number;
					t.Parameters["@Title"].Value = ms.title;
					t.Parameters["@Chorus"].Value = ms.chorus;
					t.Parameters["@Location"].Value = ms.location;
					t.Parameters["@DisplayDefault"].Value = ms.displayDefault ? 1 : 0;
					t.Parameters["@ImageId"].Value = ms.imageId;
					t.Parameters["@Overlay"].Value = ms.overlay;
					t.Parameters["@FontId"].Value = ms.fontId;
					t.Parameters["@Settings"].Value = ms.settings;

					if (!ms.foundInNew)
					{
						t.CommandText = "INSERT INTO [Songs] " +
							"([AutoNumber], [Number], [Title], [Chorus], [Location], [DisplayDefault], [ImageId], [Overlay], [FontId], [Settings]) VALUES " +
							"(@AutoNumber, @Number, @Title, @Chorus, @Location, @DisplayDefault, @ImageId, @Overlay, @FontId, @Settings)";
						t.ExecuteNonQuery();
					}
					else
					{
						t.CommandText = "UPDATE [Songs] SET " +
							"[AutoNumber] = @AutoNumber, " +
							"[Number] = @Number, " +
							"[Title] = @Title, " +
							"[Chorus] = @Chorus, " +
							"[Location] = @Location, " +
							"[DisplayDefault] = @DisplayDefault, " +
							"[ImageId] = @ImageId, " +
							"[Overlay] = @Overlay, " +
							"[FontId] = @FontId, " +
							"[Settings] = @Settings WHERE [AutoNumber] = @AutoNumber";
						t.ExecuteNonQuery();
					}
				}
			}
		}
		private void LogMigrationChangesVerses(Dictionary<string, MigrationSongVerse> mVerses)
		{
			StringBuilder sb = new StringBuilder();
			System.Diagnostics.Trace.Write("Migration changes for verses (keys only): ");
			foreach (string key in mVerses.Keys)
			{
				sb.Append(key);
				sb.Append(", ");
			}
			System.Diagnostics.Trace.WriteLine(sb.ToString());
		}
		private void LogMigrationChangesSongs(Dictionary<int, MigrationSong> mSongs)
		{
			StringBuilder sb = new StringBuilder();
			System.Diagnostics.Trace.Write("Migration changes for songs (keys only): ");
			foreach (MigrationSong s in mSongs.Values)
			{
				sb.Append(s.number.ToString());
				sb.Append(", ");
			}
			System.Diagnostics.Trace.WriteLine(sb.ToString());
		}

		public static bool OldVersionExists
		{
			get
			{
				string root = Path.GetPathRoot(Application.ExecutablePath);
				string progFilesRoot = Path.Combine(root, "Program Files");
				string vendisoftRoot = Path.Combine(progFilesRoot, "Vendisoft");
				string oldRoot = Path.Combine(vendisoftRoot, "ePresenter");
				return new DirectoryInfo(oldRoot).Exists;
			}
		}
		public static bool NeedMigration
		{
			get
			{
				if (Program.Configuration.GetSetting("MigrationCompleted") == "True")
					return false;
				return OldVersionExists;
			}
		}

		////////////////////////////////////////////////////////////////////////////////////////
		class MigrationSongVerse
		{
			public int autoNumber;
			public int orderNumber;
			public bool isChorus;
			public string verse;
			public bool foundInOld = false;
			public bool foundInNew = false;

			public string GetId() { return autoNumber.ToString() + "." + orderNumber.ToString(); }
			public bool Compare(MigrationSongVerse v1, MigrationSongVerse v2)
			{
				return (v1.autoNumber == v2.autoNumber &&
					v1.orderNumber == v2.orderNumber &&
					v1.isChorus == v2.isChorus &&
					v1.verse == v2.verse);
			}
		}
		class MigrationSong
		{
			public int autoNumber;
			public int number;
			public string title = "";
			public string chorus = "";
			public string location = "";
			public bool displayDefault = true;
			public int imageId = -2;
			public int overlay = 777;
			public string copyright = "";
			public int fontId = -2;
			public string settings = "";
			public bool foundInOld = false;
			public bool foundInNew = false;

			public bool Compare(MigrationSong v1, MigrationSong v2)
			{
				return (v1.autoNumber == v2.autoNumber &&
					v1.number == v2.number &&
					v1.title == v2.title &&
					v1.chorus == v2.chorus &&
					v1.location == v2.location &&
					v1.displayDefault == v2.displayDefault &&
					v1.imageId == v2.imageId &&
					v1.overlay == v2.overlay &&
					v1.copyright == v2.copyright);
			}
		}
	}
}